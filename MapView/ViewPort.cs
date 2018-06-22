using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapView
{
    public partial class ViewPort : Control
    {
        MapRenderer mapRenderer;

        int x;
        int y;

        public ViewPort()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!this.DesignMode)
                mapRenderer = new MapRenderer(this.ClientSize.Width, this.ClientSize.Height);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.DesignMode)
                mapRenderer.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!this.DesignMode && mapRenderer != null)
            {
                mapRenderer.Dispose();
                mapRenderer = new MapRenderer(this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.DesignMode) return;

            var image = mapRenderer.Render(x, y);
            pe.Graphics.DrawImageUnscaled(image, 0, 0);

        }

        public void SetLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Invalidate();
        }
    }
}
