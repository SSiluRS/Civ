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
    public partial class MiniMap : Control
    {
        Bitmap map;
        string fileName = "../../Map/map.png";
        Rectangle rect;
        Pen myPen = new Pen(Color.White, 2);
        public event EventHandler<MapClickEventArgs> MapClick;

        public int ViewPortWidth { get; set; }
        public int ViewPortHeigth { get; set; }

        public MiniMap()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (!this.DesignMode)
            {
                pe.Graphics.DrawImage(map, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                pe.Graphics.DrawRectangle(myPen, rect);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (this.DesignMode) return;
            map = new Bitmap(fileName);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.DesignMode)
            map.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (map != null && !this.DesignMode)
            {
                map.Dispose();
                map = new Bitmap(fileName);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            double prop = (double)map.Width / (double)this.ClientSize.Width;
            int x = Convert.ToInt32(e.X * prop * 64);
            int y = Convert.ToInt32(e.Y * prop * 64);

            DrawRect(e.X, e.Y);

            MapClick?.Invoke(this, new MapClickEventArgs() { Column = x, Row = y});
        }

        public void DrawRect(int x, int y)
        {
            x -= (int)(ViewPortWidth / 2);
            y -= (int)(ViewPortHeigth / 2);
            int w = (int)ViewPortWidth;
            int h = (int)ViewPortHeigth;
            rect = new Rectangle(x, y, w, h);
            this.Invalidate();
        }
    }

    public class MapClickEventArgs : EventArgs
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
