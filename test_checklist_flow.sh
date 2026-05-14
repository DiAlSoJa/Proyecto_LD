#!/usr/bin/env bash
# =============================================================
# test_checklist_flow.sh
# Prueba end-to-end del flujo completo de checklist de montacargas.
# Requisitos: curl, node, cygpath  (Git Bash en Windows)
# Uso:        bash test_checklist_flow.sh
# API:        http://localhost:8050  (dotnet run --project src/LD.Api)
# =============================================================

set -euo pipefail

BASE="http://localhost:8050/api"
FAIL=0
PASS=0

GRN='\033[0;32m'; RED='\033[0;31m'; YLW='\033[1;33m'; NC='\033[0m'

# Archivos temporales
# _POSIX: para bash/shell (ls, cat, etc.)
# _WIN:   path mixto con forward slashes para node y curl -F @file
TMP_BODY_POSIX=$(mktemp /tmp/ld_test_body_XXXXXX.json)
TMP_IMG_POSIX=$(mktemp /tmp/ld_test_img_XXXXXX.png)
TMP_BODY=$(cygpath -m "$TMP_BODY_POSIX")   # para node readFileSync
TMP_IMG=$(cygpath -m "$TMP_IMG_POSIX")     # para curl -F @file

trap 'rm -f "$TMP_BODY_POSIX" "$TMP_IMG_POSIX"' EXIT

# jget: extrae campo del JSON en TMP_BODY usando una expresión JS
# Uso: jget "d.data.accesstoken"
#      jget "(d.data||[]).length"
jget() {
  node -e "
const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8').trim()||'{}');
try { const v=($1); process.stdout.write(v===null||v===undefined?'':String(v)); }
catch(e){ process.stdout.write(''); }
" 2>/dev/null || true
}

check() {
  local label="$1" expected="$2"
  if [ "$STATUS" -eq "$expected" ]; then
    printf "${GRN}✅  %-58s HTTP %s${NC}\n" "$label" "$STATUS"
    PASS=$((PASS+1))
  else
    printf "${RED}❌  %-58s HTTP %s  (esperado %s)${NC}\n" "$label" "$STATUS" "$expected"
    FAIL=$((FAIL+1))
  fi
}
info() { printf "     %s\n" "$1"; }
warn() { printf "${YLW}⚠️   %s${NC}\n" "$1"; }

# Generar PNG mínimo válido (1×1 px rojo) con node
node -e "
const fs=require('fs'), zlib=require('zlib');
function u32(n){ const b=Buffer.alloc(4); b.writeUInt32BE(n); return b; }
function chunk(tag, data){
  const t=Buffer.from(tag,'ascii'), d=Buffer.isBuffer(data)?data:Buffer.from(data);
  const c=require('zlib').crc32(Buffer.concat([t,d]));
  return Buffer.concat([u32(d.length),t,d,u32(c)]);
}
const sig=Buffer.from([0x89,0x50,0x4e,0x47,0x0d,0x0a,0x1a,0x0a]);
const ihdr=Buffer.from([0,0,0,1, 0,0,0,1, 8,2, 0,0,0]);
const raw=Buffer.from([0, 255, 0, 0]);
const png=Buffer.concat([sig,chunk('IHDR',ihdr),chunk('IDAT',zlib.deflateSync(raw)),chunk('IEND',Buffer.alloc(0))]);
fs.writeFileSync('$TMP_IMG',png);
console.log('PNG creado:', '$TMP_IMG');
" 2>&1

TIMESTAMP=$(date +%s)
EQUIP_NAME="Montacargas-TEST-$TIMESTAMP"

echo ""
echo "══════════════════════════════════════════════════════════"
echo "   TEST: Flujo completo de checklist de montacargas"
echo "══════════════════════════════════════════════════════════"
echo ""

# ════════════════════════════════════════════════════════════
echo "--- FASE A: Setup admin (pasos 1-6) ---"
echo ""

# ── Paso 1: Login admin ──────────────────────────────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Pa$$w0rd"}')
check "1 – Login admin" 200

ADMIN_TOKEN=$(jget "d.data.accesstoken")
if [ -z "$ADMIN_TOKEN" ]; then
  printf "${RED}ABORT: token vacío. Revisar credenciales o estado de la API.${NC}\n"
  cat "$TMP_BODY_POSIX"; exit 1
fi
info "Token: ${ADMIN_TOKEN:0:45}..."
echo ""

# ── Paso 2: GET tipos de equipo → primer tipo ────────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/equipmenttype" \
  -H "Authorization: Bearer $ADMIN_TOKEN")
check "2 – GET /api/equipmenttype" 200

EQUIPMENT_TYPE_ID=$(jget "(d.data||[])[0]?.equipmentTypeId")
if [ -z "$EQUIPMENT_TYPE_ID" ] || [ "$EQUIPMENT_TYPE_ID" = "undefined" ]; then
  warn "Sin tipos de equipo. Creando 'Montacargas'..."
  STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/equipmenttype" \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"equipmentName":"Montacargas"}')
  check "2b – POST /api/equipmenttype (crear)" 200
  STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/equipmenttype" \
    -H "Authorization: Bearer $ADMIN_TOKEN")
  EQUIPMENT_TYPE_ID=$(jget "(d.data||[])[0].equipmentTypeId")
fi
info "EquipmentTypeId = $EQUIPMENT_TYPE_ID"
echo ""

# ── Paso 3: POST crear equipo de prueba ──────────────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/equipment" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"equipmentName\":       \"$EQUIP_NAME\",
    \"serialNumber\":        \"SN-TEST-$TIMESTAMP\",
    \"brand\":               \"Toyota\",
    \"hourmeter\":           500,
    \"isOperative\":         true,
    \"equipmentTypeId\":     $EQUIPMENT_TYPE_ID,
    \"warehouseId\":         0,
    \"equipmentSupplierId\": 0,
    \"turn1\":               \"\",
    \"turn2\":               \"\",
    \"turn3\":               \"\",
    \"imagePathLeft\":       \"\",
    \"imagePathRight\":      \"\"
  }")
check "3 – POST /api/equipment (crear equipo de prueba)" 200

STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/equipment" \
  -H "Authorization: Bearer $ADMIN_TOKEN")
EQUIPMENT_ID=$(node -e "
const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8'));
const list=d.data||[];
const item=list.find(e=>e.noEquipo==='$EQUIP_NAME')||list[list.length-1]||{};
process.stdout.write(String(item.equipmentId||''));
" 2>/dev/null || echo "")
info "EquipmentId = $EQUIPMENT_ID  ($EQUIP_NAME)"
echo ""

# ── Paso 4: POST subir imagen de prueba al equipo ────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/equipment/upload-image" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -F "file=@$TMP_IMG;type=image/png" \
  -F "side=left")
check "4 – POST /api/equipment/upload-image" 200
IMAGE_PATH=$(jget "d.data.relativePath")
info "ImagePath = $IMAGE_PATH"
echo ""

# ── Paso 5: POST crear 3 preguntas de checklist ──────────────
PAYLOADS=(
  "{\"equipmentQuestionDetId\":0,\"equipmentTypeId\":$EQUIPMENT_TYPE_ID,\"questionText\":\"Nivel de aceite correcto\",\"isYesNo\":true}"
  "{\"equipmentQuestionDetId\":0,\"equipmentTypeId\":$EQUIPMENT_TYPE_ID,\"questionText\":\"Nivel de bateria correcto\",\"isYesNo\":true}"
  "{\"equipmentQuestionDetId\":0,\"equipmentTypeId\":$EQUIPMENT_TYPE_ID,\"questionText\":\"Estado de llantas\",\"isYesNo\":false,\"optionAnswerText\":\"Bueno\\nRegular\\nMalo\"}"
)
for i in 0 1 2; do
  STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/equipmentquestion" \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d "${PAYLOADS[$i]}")
  check "5.$((i+1)) – POST /api/equipmentquestion (pregunta $((i+1)))" 200
done

STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" \
  "$BASE/equipmentquestion/equipment-type/$EQUIPMENT_TYPE_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN")
check "5b – GET /api/equipmentquestion/equipment-type/$EQUIPMENT_TYPE_ID" 200

Q_COUNT=$(jget "(d.data||[]).length")
Q1_ID=$(node -e "const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8')); const a=d.data||[]; process.stdout.write(String(a[a.length-3]?.equipmentQuestionDetId||''));" 2>/dev/null || echo "")
Q2_ID=$(node -e "const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8')); const a=d.data||[]; process.stdout.write(String(a[a.length-2]?.equipmentQuestionDetId||''));" 2>/dev/null || echo "")
Q3_ID=$(node -e "const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8')); const a=d.data||[]; process.stdout.write(String(a[a.length-1]?.equipmentQuestionDetId||''));" 2>/dev/null || echo "")

info "Total preguntas tipo $EQUIPMENT_TYPE_ID: $Q_COUNT"
info "Q1_ID=$Q1_ID  Q2_ID=$Q2_ID  Q3_ID=$Q3_ID"
echo ""

# ── Paso 6: GET equipo → PUT asignar Turn1=admin ─────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/equipment/$EQUIPMENT_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN")
check "6a – GET /api/equipment/$EQUIPMENT_ID" 200

STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X PUT "$BASE/equipment/$EQUIPMENT_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"equipmentId\":         $EQUIPMENT_ID,
    \"equipmentName\":       \"$EQUIP_NAME\",
    \"serialNumber\":        \"SN-TEST-$TIMESTAMP\",
    \"brand\":               \"Toyota\",
    \"hourmeter\":           500,
    \"isOperative\":         true,
    \"equipmentTypeId\":     $EQUIPMENT_TYPE_ID,
    \"warehouseId\":         0,
    \"equipmentSupplierId\": 0,
    \"turn1\":               \"admin\",
    \"turn2\":               \"\",
    \"turn3\":               \"\",
    \"imagePathLeft\":       \"$IMAGE_PATH\",
    \"imagePathRight\":      \"\"
  }")
check "6b – PUT /api/equipment/$EQUIPMENT_ID  (Turn1='admin')" 200
echo ""

# ════════════════════════════════════════════════════════════
echo "--- FASE B: Verificación operador (pasos 7-8) ---"
echo "    (admin como operador de prueba, Turn1='admin')"
echo "    Para operador1 real: crea en WPF con rol que tenga"
echo "    forklift-checklist.read/.execute y units.read"
echo ""

OP_TOKEN="$ADMIN_TOKEN"

# ── Paso 7: GET /api/equipment/assigned-to-me ────────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/equipment/assigned-to-me" \
  -H "Authorization: Bearer $OP_TOKEN")
check "7 – GET /api/equipment/assigned-to-me" 200

ASSIGNED_ID=$(jget "d.data?.equipmentId")
if [ "$ASSIGNED_ID" = "$EQUIPMENT_ID" ]; then
  info "Equipo asignado correctamente ✓  (id=$ASSIGNED_ID)"
elif [ -n "$ASSIGNED_ID" ] && [ "$ASSIGNED_ID" != "undefined" ] && [ "$ASSIGNED_ID" != "null" ]; then
  warn "Asignado id=$ASSIGNED_ID — otro equipo en BD tiene Turn1=admin"
else
  warn "Sin equipo asignado (data=null). ¿El PUT del paso 6b aplicó?"
fi
echo ""

# ── Paso 8: GET preguntas por tipo (como operador) ───────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" \
  "$BASE/equipmentquestion/equipment-type/$EQUIPMENT_TYPE_ID" \
  -H "Authorization: Bearer $OP_TOKEN")
check "8 – GET /api/equipmentquestion/equipment-type/$EQUIPMENT_TYPE_ID (operador)" 200

Q_COUNT_OP=$(jget "(d.data||[]).length")
info "Preguntas disponibles: $Q_COUNT_OP"
if node -e "process.exit(parseInt('$Q_COUNT_OP')>=3?0:1)" 2>/dev/null; then
  info "≥3 preguntas ✓"
else
  warn "Se esperaban ≥3 preguntas, se encontraron $Q_COUNT_OP"
  FAIL=$((FAIL+1))
fi
echo ""

# ════════════════════════════════════════════════════════════
echo "--- FASE C: Flujo móvil simulado (pasos 9-11) ---"
echo ""

# ── Paso 9: POST /api/checklist/upload-photo ─────────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/checklist/upload-photo" \
  -H "Authorization: Bearer $OP_TOKEN" \
  -F "file=@$TMP_IMG;type=image/png" \
  -F "side=left")
check "9 – POST /api/checklist/upload-photo" 200
PHOTO_PATH=$(jget "d.data.relativePath")
info "PhotoPath = $PHOTO_PATH"
echo ""

# ── Paso 10: POST /api/checklist (horómetro=1234) ────────────
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" -X POST "$BASE/checklist" \
  -H "Authorization: Bearer $OP_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"equipmentId\":   $EQUIPMENT_ID,
    \"userName\":      \"admin\",
    \"turno\":         \"Matutino\",
    \"horometro\":     1234,
    \"observaciones\": \"Test automatizado test_checklist_flow.sh\",
    \"answers\": [
      {\"questionId\": $Q1_ID, \"questionText\": \"Nivel de aceite correcto\", \"answerText\": \"Si\",    \"isOk\": true},
      {\"questionId\": $Q2_ID, \"questionText\": \"Nivel de bateria correcto\", \"answerText\": \"Si\",    \"isOk\": true},
      {\"questionId\": $Q3_ID, \"questionText\": \"Estado de llantas\",         \"answerText\": \"Bueno\", \"isOk\": null}
    ],
    \"defectMarks\": [
      {\"side\": \"Left\", \"xPercent\": 0.25, \"yPercent\": 0.30, \"note\": \"Marca de prueba\"}
    ],
    \"photos\": [
      {\"relativePath\": \"$PHOTO_PATH\", \"side\": \"left\", \"order\": 1}
    ]
  }")
check "10 – POST /api/checklist (horometro=1234)" 200

CHECKLIST_ID=$(jget "d.data?.checklistId")
CHECKLIST_MSG=$(jget "d.message")
info "ChecklistId = $CHECKLIST_ID  |  $CHECKLIST_MSG"
echo ""

# ── Paso 11: GET checklists → verificar horómetro guardado ───
STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" \
  "$BASE/checklist?equipmentTypeId=$EQUIPMENT_TYPE_ID&equipmentId=$EQUIPMENT_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN")
check "11 – GET /api/checklist?equipmentTypeId=...&equipmentId=..." 200

SAVED_HM=$(node -e "
const d=JSON.parse(require('fs').readFileSync('$TMP_BODY','utf8'));
const list=d.data||[];
const item=list.find(c=>String(c.checklistId)==='$CHECKLIST_ID')||list[0]||{};
process.stdout.write(String(item.hourmeter??''));
" 2>/dev/null || echo "")

info "Horómetro guardado: $SAVED_HM  (esperado: 1234)"
if [ "$SAVED_HM" = "1234" ] || [ "$SAVED_HM" = "1234.0" ]; then
  printf "${GRN}     ✅ Verificación horómetro PASS${NC}\n"
  PASS=$((PASS+1))
else
  printf "${RED}     ❌ Verificación horómetro FAIL (recibido: %s)${NC}\n" "$SAVED_HM"
  FAIL=$((FAIL+1))
fi
echo ""

# ── GET detalle (bonus, informativo) ─────────────────────────
if [ -n "$CHECKLIST_ID" ] && [ "$CHECKLIST_ID" != "undefined" ] && [ "$CHECKLIST_ID" != "" ]; then
  STATUS=$(curl -s -o "$TMP_BODY_POSIX" -w "%{http_code}" "$BASE/checklist/$CHECKLIST_ID" \
    -H "Authorization: Bearer $ADMIN_TOKEN")
  check "11b – GET /api/checklist/$CHECKLIST_ID (detalle)" 200
  ANS=$(jget    "(d.data?.answers||[]).length")
  MARKS=$(jget  "(d.data?.defectMarks||[]).length")
  PHOTOS=$(jget "(d.data?.photos||[]).length")
  info "Respuestas=$ANS  Marcas=$MARKS  Fotos=$PHOTOS"
  echo ""
fi

# ════════════════════════════════════════════════════════════
echo "══════════════════════════════════════════════════════════"
echo ""
printf "  ${GRN}✅ Pasados:  %s${NC}\n" "$PASS"
printf "  ${RED}❌ Fallados: %s${NC}\n" "$FAIL"
echo ""
if [ "$FAIL" -eq 0 ]; then
  printf "  ${GRN}TODOS LOS PASOS PASARON${NC}\n"
else
  printf "  ${RED}$FAIL PASO(S) FALLARON${NC}\n"
fi
echo "══════════════════════════════════════════════════════════"

exit "$FAIL"
