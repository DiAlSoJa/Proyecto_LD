using LD.Forms.Views.Dialogs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace LD.Controls
{
    [DefaultEvent("_TextChanged")]
    public partial class TextBoxControl : UserControl
    {
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private Color borderFocusColor = Color.Black;
        private bool isFocused = false;
        private bool isNumber = false;
        private int borderRadius = 0;
        private bool isEnabled = true;
        private bool minusculas = false;
        private bool aceptaNegativos = false;

        public TextBoxControl()
        {
            InitializeComponent();
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            textBox1.Click += textBox1_Click;
        }

        public event EventHandler _TextChanged;

        [Category("LD_CONTROLS")]
        [DefaultValue(typeof(Color), "MediumSlateBlue")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool Minusculas
        {
            get => minusculas;
            set
            {
                minusculas = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool AceptaNegativos
        {
            get => aceptaNegativos;
            set
            {
                aceptaNegativos = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(true)]
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                textBox1.Enabled = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(2)]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool UnderlinedStyle
        {
            get => underlinedStyle;
            set
            {
                underlinedStyle = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool PasswordChar
        {
            get => textBox1.UseSystemPasswordChar;
            set => textBox1.UseSystemPasswordChar = value;
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool Multiline
        {
            get => textBox1.Multiline;
            set
            {
                textBox1.Multiline = value;
                UpdateControlHeight();
            }
        }

        [Category("LD_CONTROLS")]
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                if (textBox1 != null)
                    textBox1.BackColor = value;
            }
        }

        [Category("LD_CONTROLS")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                if (textBox1 != null)
                    textBox1.ForeColor = value;
                if (DesignMode)
                    UpdateControlHeight();
            }
        }

        [Category("LD_CONTROLS")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (textBox1 != null)
                    textBox1.Font = value;

                UpdateControlHeight();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue("")]
        public string Texts
        {
            get => textBox1.Text;
            set
            {
                textBox1.Text = value;
                if (isNumber)
                    FormatearNumero();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(false)]
        public bool IsNumber
        {
            get => isNumber;
            set
            {
                isNumber = value;
                textBox1.TextAlign = isNumber ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderFocusColor
        {
            get => borderFocusColor;
            set
            {
                borderFocusColor = value;
                Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DefaultValue(0)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            if (borderRadius > 1)
            {
                var rectBorderSmooth = ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;
                Color parentColor = Parent?.BackColor ?? SystemColors.Control;

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, Math.Max(borderRadius - borderSize, 1)))
                using (Pen penBorderSmooth = new Pen(parentColor, smoothSize))
                using (Pen penBorder = new Pen(isFocused && IsEnabled ? borderFocusColor : borderColor, borderSize))
                {
                    Region = new Region(pathBorderSmooth);

                    if (borderRadius > 15)
                        SetTextBoxRoundedRegion();

                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Inset;

                    if (underlinedStyle)
                    {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    }
                    else
                    {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                using (Pen penBorder = new Pen(isFocused && IsEnabled ? borderFocusColor : borderColor, borderSize))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;

                    if (underlinedStyle)
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    else
                        graph.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                }
            }
        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt;

            if (Multiline)
                pathTxt = GetFigurePath(textBox1.ClientRectangle, Math.Max(borderRadius - borderSize, 1));
            else
                pathTxt = GetFigurePath(textBox1.ClientRectangle, Math.Max(borderSize * 2, 1));

            textBox1.Region = new Region(pathTxt);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateControlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void UpdateControlHeight()
        {
            if (textBox1 == null)
                return;

            if (!textBox1.Multiline)
            {
                int txtHeight = TextRenderer.MeasureText("Text", Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;
                Height = textBox1.Height + Padding.Top + Padding.Bottom;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string textoOriginal = textBox1.Text;
            string textoNuevo = textoOriginal;
            int cursorPosition = textBox1.SelectionStart;

            if (isNumber)
                textoNuevo = FiltrarTextoNumerico(textoNuevo);

            if (!isNumber)
            {
                textoNuevo = minusculas
                    ? textoNuevo.ToLower()
                    : textoNuevo.ToUpper();
            }

            if (textoNuevo != textoOriginal)
            {
                textBox1.Text = textoNuevo;
                textBox1.SelectionStart = Math.Min(cursorPosition, textBox1.Text.Length);
            }

            _TextChanged?.Invoke(sender, e);
        }

        private string FiltrarTextoNumerico(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            string resultado = new string(texto
                .Where((c, i) =>
                    char.IsDigit(c) ||
                    c == '.' ||
                    c == ',' ||
                    (aceptaNegativos && c == '-' && i == 0))
                .ToArray());

            int firstDot = resultado.IndexOf('.');
            if (firstDot >= 0)
            {
                resultado = resultado.Substring(0, firstDot + 1) +
                            resultado.Substring(firstDot + 1).Replace(".", "");
            }

            int firstComma = resultado.IndexOf(',');
            if (firstComma >= 0)
            {
                resultado = resultado.Substring(0, firstComma + 1) +
                            resultado.Substring(firstComma + 1).Replace(",", "");
            }

            return resultado;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            isFocused = true;

            try
            {
                if (isNumber && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    string groupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                    if (!string.IsNullOrEmpty(groupSeparator))
                        textBox1.Text = textBox1.Text.Replace(groupSeparator, "");
                }
            }
            catch
            {
            }

            Invalidate();
        }

        private void FormatearNumero()
        {
            try
            {
                isFocused = false;

                if (isNumber && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    string texto = textBox1.Text.Trim();

                    if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal number))
                    {
                        textBox1.Text = number.ToString("N2", CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            FormatearNumero();
            Invalidate();
        }
    }
}