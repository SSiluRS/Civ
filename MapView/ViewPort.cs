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
        public event EventHandler<MapMoveEventArgs> MapMove;

        MapRenderer mapRenderer;

        int x;
        int y;
        int oldX;
        int oldY;
        int clickX;
        int clickY;
        int rX;
        int rY;
        int cellX;
        int cellY;
        bool drawCell = false;
        int oldCellX;
        int oldCellY;
        Bitmap map;

        Pen myPen = new Pen(Color.White, 2);
        Rectangle cell;

        int tileSize = MapRenderer.tileSize;

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

            if (drawCell == true)
            {
                cell = new Rectangle(cellX * tileSize - x, cellY * tileSize - y, tileSize, tileSize);
                pe.Graphics.DrawRectangle(myPen, cell);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                clickX = e.X;
                clickY = e.Y;
                oldX = x;
                oldY = y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                cellX = (x + e.X) / tileSize;
                cellY = (y + e.Y) / tileSize;
                oldCellX = cellX;
                oldCellY = cellY;
                drawCell = true;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            clickX = 0;
            clickY = 0;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            rX = clickX - e.X;
            rY = clickY - e.Y;
            if (clickX != 0 || clickY != 0)
            {
                x = oldX + rX;
                y = Math.Min(Math.Max(0, oldY + rY), 160 * tileSize - this.ClientSize.Height);
                this.Invalidate();
                MapMove?.Invoke(this, new MapMoveEventArgs() { C = x / tileSize, R = y / tileSize });
            }
        }

        public void SetLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Invalidate();
        }
    }

    public class MapMoveEventArgs : EventArgs
    {
        public int C { get; set; }
        public int R { get; set; }
    }
}
