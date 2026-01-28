//using PinkSpace.Clases;
//using PinkSpace.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LD.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        }

        public event EventHandler _TextChanged;

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                this.Invalidate(); 
            }
        }
        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Minusculas
        {
            get => minusculas;
            set
            {
                minusculas = value;
                this.Invalidate();
            }
        }
        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AceptaNegativos
        {
            get => minusculas;
            set
            {
                aceptaNegativos = value;
                this.Invalidate();
            }
        }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                textBox1.Enabled = value;
                this.Invalidate();
            }
        }
        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = value;
                this.Invalidate(); 
            }
        }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UnderlinedStyle
        {
            get => underlinedStyle;
            set
            {
                underlinedStyle = value;
                this.Invalidate(); 
            }
        }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PasswordChar
        {
            get => textBox1.UseSystemPasswordChar;
            set => textBox1.UseSystemPasswordChar = value;
        }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Multiline
        {
            get => textBox1.Multiline;
            set => textBox1.Multiline = value;
        }

        [Category("LD_CONTROLS")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                textBox1.BackColor = value;
            }
        }
        [Category("LD_CONTROLS")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
                if (this.DesignMode) UpdateControlHeight();
            }
        }
        [Category("LD_CONTROLS")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                textBox1.Font = value;
            }
        }
        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Texts
        {
            get
            {
                return textBox1.Text;
            }
            set
            {

                textBox1.Text = value;
                FormatiarNumero();
            }

        }
        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumber
        {
            get => isNumber;
            set
            {
                isNumber = value;
                textBox1.TextAlign = isNumber ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
        }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderFocusColor { get => borderFocusColor; set => borderFocusColor = value; }

        [Category("LD_CONTROLS")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    this.Invalidate(); 
                }
            }
        }


        // sobreescrituras
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            if (borderRadius > 1)
            {
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    // Drawing
                    this.Region = new Region(pathBorderSmooth);
                    if (borderRadius > 15) SetTextBoxRoundedRegion();
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                    if (isFocused && IsEnabled) penBorder.Color = borderFocusColor;

                    if (underlinedStyle)
                    {
                        // Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        // Draw border
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else // Normal Style
                    {
                        // Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        // Draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                    if (isFocused && IsEnabled)
                    {
                        penBorder.Color = borderFocusColor;
                        if (underlinedStyle) 
                            graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                        else 
                            graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    }
                    else
                    {
                        if (underlinedStyle) 
                            graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                        else 
                            graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    }
                }

            }


        }
        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt;
            if (Multiline)
            {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
                textBox1.Region = new Region(pathTxt);
            }
            else
            {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderSize * 2);
                textBox1.Region = new Region(pathTxt);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
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
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void UpdateControlHeight()
        {
            if (textBox1.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isNumber)
            {
                // Filtrar el texto
                string textoFiltrado = "";
                if (aceptaNegativos)
                {
                    textoFiltrado = new string(textBox1.Text
                                .Where((c, i) => char.IsDigit(c) || c == '.' || c == ',' || (c == '-' && i == 0))
                                .ToArray());
                }
                else
                {
                    textoFiltrado = new string(textBox1.Text.Where(c => char.IsDigit(c) || c == '.' || c == ',' ).ToArray());

                }
               

                //aqui cuando me lo pidan voy a poner algo que deje o no poner negativos

                // Asegurarse de que haya solo un punto decimal
                int firstDotIndex = textoFiltrado.IndexOf('.');
                if (firstDotIndex >= 0)
                {
                    textoFiltrado = textoFiltrado.Substring(0, firstDotIndex + 1) +
                                    textoFiltrado.Substring(firstDotIndex + 1).Replace(".", "");
                }

                // Si el texto es diferente, actualizamos el control y mostramos advertencia
                if (textoFiltrado != textBox1.Text)
                {
                 
                    FrmWarning frmWarning = new FrmWarning($"Este campo solo permite números {(aceptaNegativos ? "" : "positivos")}");
                    frmWarning.ShowDialog();
                    textBox1.Text = textoFiltrado;
                    textBox1.SelectionStart = textoFiltrado.Length;
                }
            }

            if (!minusculas)
            {

                int cursorPosition = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.ToUpper();
                textBox1.SelectionStart = cursorPosition;
                textBox1.SelectionLength = 0;
            }


            _TextChanged?.Invoke(sender, e);
        }

        //        // Filtrar el texto: permitir solo dígitos, un único punto decimal, y comas
        //        string textoFiltrado = new string(textBox1.Text.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());

        //        // Eliminar comas innecesarias para facilitar la validación
        //        string textoSinComas = textoFiltrado.Replace(",", "");

        //        // Asegurarse de que haya solo un punto decimal
        //        int firstDotIndex = textoSinComas.IndexOf('.');
        //                if (firstDotIndex >= 0)
        //                {
        //                    textoSinComas = textoSinComas.Substring(0, firstDotIndex + 1) +
        //                                    textoSinComas.Substring(firstDotIndex + 1).Replace(".", "");
        //    }

        //                // Si el texto es diferente, actualizar el control y mostrar advertencia
        //                if (textoSinComas != textBox1.Text.Replace(",", ""))
        //                {
        //                    FrmWarning frmWarning = new FrmWarning("Este campo solo permite números en formato válido");
        //    frmWarning.ShowDialog();
        //                }

        //// Validar y formatear como número con separadores de miles y dos decimales
        //if (decimal.TryParse(textoSinComas, out decimal numero))
        //{
        //    // Formatear con separadores de miles y dos decimales
        //    textBox1.Text = numero.ToString("");
        //}

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                isFocused = true;
                if (isNumber && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    // Eliminar las comas y restaurar el número en formato normal
                    textBox1.Text = textBox1.Text.Replace(",", "");
                }

            }catch(Exception ex)
            {
                /*ErrorForm errorForm = new ErrorForm("Hubo un error al formatear el numero");
                errorForm.ShowDialog();*/
            }

            this.Invalidate();
        }
        private void FormatiarNumero()
        {
            try
            {
                isFocused = false;
                if (isNumber && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    if (decimal.TryParse(textBox1.Text, out decimal number))
                    {
                        // Formatear el número con comas y 2 decimales
                        textBox1.Text = number.ToString("N2", CultureInfo.CurrentCulture);
                    }
                }

            }
            catch (Exception ex)
            {
              /*  ErrorForm errorForm = new ErrorForm("Hubo un error al formatear el numero");
                errorForm.ShowDialog();*/
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            FormatiarNumero();
            this.Invalidate();
        }
    }
}
