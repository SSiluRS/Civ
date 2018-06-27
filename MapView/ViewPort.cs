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
        public event EventHandler<UnitMoveEventArgs> UnitMove;
        public event EventHandler<CellSelectedEventArgs> CellSelected;

        MapRenderer mapRenderer;

        Random rnd = new Random();

        int x;
        int y;
        int oldX;
        int oldY;
        int clickX;
        int clickY;
        int rX;
        int rY;
        int cellC;
        int cellR;
        bool drawCell = false;
        int oldCellX;
        int oldCellY;
        Pen myPen = new Pen(Color.White, 2);
        Rectangle cell;
        List<Unit> units;
        GameModel.World.World world;
        
        int tileSize = MapRenderer.tileSize;

        public ViewPort()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.Selectable, true);
        }

        public GameModel.World.World World
        {
            get => world;

            set
            {
                this.world = value;
                units = CreateUnits();
                mapRenderer = new MapRenderer(this.ClientSize.Width, this.ClientSize.Height, World);
                Invalidate();
            }
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.DesignMode)
                mapRenderer.Dispose();
        }

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);

        //    if (!this.DesignMode && mapRenderer != null)
        //    {
        //        mapRenderer.Dispose();
        //        mapRenderer = new MapRenderer(this.ClientSize.Width, this.ClientSize.Height);
        //    }
        //}

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.DesignMode || world == null) return;

            var image = mapRenderer.Render(x - 1, y);
            pe.Graphics.DrawImageUnscaled(image, 0, 0);

            foreach (var player in world.playerList)
            {
                foreach (var city in player.cities)
                {
                    pe.Graphics.FillRectangle(Brushes.Gray, new Rectangle(city.Key.Item1 * tileSize - x, city.Key.Item2 * tileSize - y, tileSize, tileSize));
                }
            }

            if (drawCell)
            {
                cell = new Rectangle(cellC * tileSize - x, cellR * tileSize - y, tileSize, tileSize);
                pe.Graphics.DrawRectangle(myPen, cell);
            }
            var unitTiles = new Bitmap("../../Map/unitsS.png");
            var k = 0;
            foreach (var u in units)
            {
                if (CheckUnit(u))
                {
                    var dest = new Rectangle(u.Column * tileSize - x, u.Row * tileSize - y, tileSize, tileSize);
                    var cst = new Rectangle(u.ImageIndex * tileSize, u.ImageIndex / 20 * tileSize, tileSize, tileSize);
                    pe.Graphics.DrawImage(unitTiles, dest, cst, GraphicsUnit.Pixel);
                    k++;
                }
            }
            pe.Graphics.DrawString(k.ToString(), SystemFonts.DefaultFont, Brushes.White, 0, 0);
        }

        public List<Unit> CreateUnits()
        {
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < 1000; i++)
            {
                units.Add(new Unit { Column = rnd.Next(1, 318), Row = rnd.Next(1, 158), ImageIndex = rnd.Next(0, 28) });
            }
            return units;
        }

        public bool CheckUnit(Unit unit)
        {
            if (unit.Column >= x / tileSize && unit.Column <= (x + this.ClientSize.Width) / tileSize && unit.Row >= y / tileSize && unit.Row <= (y + this.ClientSize.Height) / tileSize)
            {
                return true;
            }
            else return false;
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
                cellC = (x + e.X) / tileSize;
                cellR = (y + e.Y) / tileSize;
                oldCellX = cellC;
                oldCellY = cellR;
                drawCell = true;
                CellSelected?.Invoke(this, new CellSelectedEventArgs() { Column = cellC, Row = cellR});
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

        public Unit GetUnit()
        {
            Unit _unit = null;
            foreach (var unit in units)
            {
                if (CheckUnit(unit))
                {
                    _unit = unit;
                }
            }
            return _unit;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);            
            UnitMove?.Invoke(this, new UnitMoveEventArgs { Key = e.KeyCode});
            Invalidate();
        }
        
        public void MoveUnit(int dx, int dy, int n)
        {
            units[n].Column += dx;
            units[n].Row += dy;
            cellC += dx;
            cellR += dy;

            int a = 0;
            if (units[n].Column * tileSize + tileSize > x + this.ClientSize.Width)
            {
                a = units[n].Column * tileSize + tileSize - (x + this.ClientSize.Width);
                x += a;
            }
            if (units[n].Column * tileSize < x)
            {
                a = units[n].Column * tileSize - x;
                x += a;
            }
            if (units[n].Row * tileSize + tileSize > y + this.ClientSize.Height)
            {
                a = units[n].Row * tileSize + tileSize - (y + this.ClientSize.Height);
                y += a;
            }
            if (units[n].Row * tileSize < y)
            {
                a = units[n].Row * tileSize - y;
                y += a;
            }
        }

        public int SelectUnit()
        {
            int currentUnit = -1;
            for (int i = 0; i < units.Count; i++)
            {
                if (cellC == units[i].Column && cellR == units[i].Row)
                {
                    currentUnit = i;
                }
            }
            return currentUnit;
        }

        public void SetLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Invalidate();
        }
    }

    public class CellSelectedEventArgs: EventArgs
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }

    public class MapMoveEventArgs : EventArgs
    {
        public int C { get; set; }
        public int R { get; set; }
    }

    public class Unit
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int ImageIndex { get; set; }
    }

    public class UnitMoveEventArgs : EventArgs
    {
        public Keys Key { get; set; }        
    }
}
