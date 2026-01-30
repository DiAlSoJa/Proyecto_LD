using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LD.Forms.Controls
{
    [ToolboxItem(true)]
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 20;
        private int _borderSize = 1;
        private Color _borderColor = Color.LightGray;

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public RoundedPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -_borderSize, -_borderSize);

            if (_borderRadius > 1)
            {
                using GraphicsPath pathSurface = GetRoundedPath(rectSurface, _borderRadius);
                using GraphicsPath pathBorder = GetRoundedPath(rectBorder, _borderRadius - _borderSize);

                Region = new Region(pathSurface);

                if (_borderSize > 0)
                {
                    using Pen pen = new(_borderColor, _borderSize);
                    e.Graphics.DrawPath(pen, pathBorder);
                }
            }
            else
            {
                Region = new Region(rectSurface);
            }
        }

        private static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            radius = Math.Max(1, radius);
            GraphicsPath path = new();
            float r = radius * 2f;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
