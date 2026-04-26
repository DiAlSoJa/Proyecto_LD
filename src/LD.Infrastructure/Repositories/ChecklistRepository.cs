using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.Checklist;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class ChecklistRepository : IChecklistRepository
{
    private readonly LdProyectDbContext _context;

    public ChecklistRepository(LdProyectDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateChecklistAsync(Checklist checklist)
    {
        await _context.Checklists.AddAsync(checklist);
        await _context.SaveChangesAsync();
        return checklist.ChecklistId;
    }

    public async Task<List<ChecklistSummaryDto>> GetSummariesAsync(
        DateTime? from, DateTime? to, int? equipmentTypeId, int? equipmentId)
    {
        var query = _context.Checklists
            .AsNoTracking()
            .Include(c => c.Equipment)
                .ThenInclude(e => e!.EquipmentType)
            .Include(c => c.Equipment)
                .ThenInclude(e => e!.EquipmentSupplier)
            .Include(c => c.DefectMarks)
            .Include(c => c.Answers)
            .Where(c => c.IsActive);

        if (from.HasValue)
            query = query.Where(c => c.CreatedAt >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(c => c.CreatedAt < to.Value.Date.AddDays(1));

        if (equipmentTypeId.HasValue)
            query = query.Where(c => c.EquipmentTypeId == equipmentTypeId.Value);

        if (equipmentId.HasValue)
            query = query.Where(c => c.EquipmentId == equipmentId.Value);

        var checklists = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();

        return checklists.Select(c => new ChecklistSummaryDto
        {
            ChecklistId       = c.ChecklistId,
            EquipmentId       = c.EquipmentId,
            EquipmentName     = c.Equipment?.EquipmentName ?? string.Empty,
            EquipmentTypeId   = c.EquipmentTypeId,
            EquipmentTypeName = c.Equipment?.EquipmentType?.EquipmentName ?? string.Empty,
            SerialNumber      = c.Equipment?.SerialNumber,
            Brand             = c.Equipment?.Brand,
            IsOperative       = c.Equipment?.IsOperative ?? false,
            SupplierName      = c.Equipment?.EquipmentSupplier?.EquipmentSupplierName,
            Hourmeter         = c.Equipment?.Hourmeter,
            UserName          = c.UserName,
            Turno             = c.Turno,
            CreatedAt         = c.CreatedAt,
            TotalDefects      = c.DefectMarks.Count + c.Answers.Count(a => a.IsOk == false),
            Observaciones     = c.Observaciones
        }).ToList();
    }

    public async Task<ChecklistDetailDto?> GetDetailAsync(int checklistId, string photoBaseUrl)
    {
        var c = await _context.Checklists
            .AsNoTracking()
            .Include(x => x.Equipment)
                .ThenInclude(e => e!.EquipmentType)
            .Include(x => x.Equipment)
                .ThenInclude(e => e!.EquipmentSupplier)
            .Include(x => x.DefectMarks)
            .Include(x => x.Answers)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.ChecklistId == checklistId && x.IsActive);

        if (c is null) return null;

        return new ChecklistDetailDto
        {
            ChecklistId       = c.ChecklistId,
            EquipmentId       = c.EquipmentId,
            EquipmentName     = c.Equipment?.EquipmentName ?? string.Empty,
            EquipmentTypeId   = c.EquipmentTypeId,
            EquipmentTypeName = c.Equipment?.EquipmentType?.EquipmentName ?? string.Empty,
            SerialNumber      = c.Equipment?.SerialNumber,
            Brand             = c.Equipment?.Brand,
            IsOperative       = c.Equipment?.IsOperative ?? false,
            SupplierName      = c.Equipment?.EquipmentSupplier?.EquipmentSupplierName,
            Hourmeter         = c.Equipment?.Hourmeter,
            UserName          = c.UserName,
            Turno             = c.Turno,
            CreatedAt         = c.CreatedAt,
            TotalDefects      = c.DefectMarks.Count + c.Answers.Count(a => a.IsOk == false),
            Observaciones     = c.Observaciones,
            Answers = c.Answers.Select(a => new ChecklistAnswerDto
            {
                QuestionId   = a.QuestionId,
                QuestionText = a.QuestionTextSnapshot,
                AnswerText   = a.AnswerText,
                IsOk         = a.IsOk
            }).ToList(),
            DefectMarks = c.DefectMarks.Select(m => new ChecklistDefectMarkDto
            {
                Side     = m.Side,
                XPercent = m.XPercent,
                YPercent = m.YPercent,
                Note     = m.Note
            }).ToList(),
            Photos = c.Photos.OrderBy(p => p.Order).Select(p => new ChecklistPhotoDto
            {
                ChecklistPhotoId = p.ChecklistPhotoId,
                RelativePath     = p.RelativePath,
                PhotoUrl         = $"{photoBaseUrl}?path={Uri.EscapeDataString(p.RelativePath)}",
                Side             = p.Side,
                Order            = p.Order
            }).ToList()
        };
    }
}
