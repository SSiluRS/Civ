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
        Pen myPen = new Pen(Color.White, 2);
        public event EventHandler<MapClickEventArgs> MapClick;

        int vpX;
        int vpY;

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
                var vpRect = new Rectangle((int)w2mx(vpX), (int)w2my(vpY), (int)w2mx(ViewPortWidth), (int)w2my(ViewPortHeigth));
                pe.Graphics.DrawRectangle(myPen, vpRect);
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

        double w2mx(double x) { return x * (double)this.ClientSize.Width / (double)map.Width; }
        double w2my(double y) { return y * (double)this.ClientSize.Height / (double)map.Height; }

        int m2wx(double x) { return (int)(x * (double)map.Width / (double)this.ClientSize.Width); }
        int m2wy(double y) { return (int)(y * (double)map.Height / (double)this.ClientSize.Height); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            vpX = m2wx(e.X - ViewPortWidth / 2);
            vpY = m2wy(e.Y - ViewPortHeigth / 2);
            MapClick?.Invoke(this, new MapClickEventArgs() { Column = vpX, Row = vpY});
            this.Invalidate();
        }

        public void MoveRect(int c, int r)
        {
            vpX = c;
            vpY = r;
            this.Invalidate();
        }
    }

    public class MapClickEventArgs : EventArgs
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
