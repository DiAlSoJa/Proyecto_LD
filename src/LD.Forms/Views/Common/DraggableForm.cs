using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Views.Common
{
    public class DraggableForm : Form
    {
        private bool _mouseDown;
        private Point _lastLocation;

        protected void EnableDrag(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                _mouseDown = true;
                _lastLocation = e.Location;
            };

            control.MouseMove += (s, e) =>
            {
                if (_mouseDown)
                {
                    Location = new Point(
                        (Location.X - _lastLocation.X) + e.X,
                        (Location.Y - _lastLocation.Y) + e.Y);

                    Update();
                }
            };

            control.MouseUp += (s, e) =>
            {
                _mouseDown = false;
            };
        }
    }
}
