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

        private GameModel.World.World world;

        public GameModel.World.World World
        {
            get { return world; }
            set
            {
                world = value;
                OnWorldSet();
            }
        }

        private void OnWorldSet()
        {
            map = new Bitmap(320, 160);
            foreach (var p in world.worldMap)
            {
                Color c;
                if (p.Value.IsOcean)
                    c = Color.DarkBlue;
                else if (p.Value.IsDesert)
                    c = Color.Yellow;
                else c = Color.DarkGreen;
                map.SetPixel(p.Key.Item1, p.Key.Item2, c);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (!this.DesignMode && map != null)
            {
                pe.Graphics.DrawImage(map, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                var vpRect = new Rectangle((int)w2mx(vpX), (int)w2my(vpY), (int)w2mx(ViewPortWidth), (int)w2my(ViewPortHeigth));
                pe.Graphics.DrawRectangle(myPen, vpRect);
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.DesignMode)
            map.Dispose();
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

        public void MoveRectD(int dx, int dy)
        {
            vpX += dx;
            vpY += dy;
            Invalidate();
        }
    }

    public class MapClickEventArgs : EventArgs
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
