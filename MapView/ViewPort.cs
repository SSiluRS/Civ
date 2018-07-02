using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameModel;

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
        GameModel.World.World world;
        bool blink = true;
        GameModel.Unit.Unit blinkingUnit = null;

        Bitmap unitTiles;

        int tileSize = MapRenderer.tileSize;

        public ViewPort()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        protected override void OnCreateControl()
        {
            if (!DesignMode)
                unitTiles = new Bitmap("../../Map/unitsS.png");
        }

        public void SetWorld(GameModel.World.World world)
        {            
            mapRenderer = new MapRenderer(this.ClientSize.Width, this.ClientSize.Height, world);
            this.world = world;
            Invalidate();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!this.DesignMode)
                mapRenderer.Dispose();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.DesignMode || world == null) return;

            var image = mapRenderer.Render(x, y);
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
            var k = 0;
            foreach (var u in world.units)
            {
                var c = u.Key.Item1;
                var r = u.Key.Item2;
                var unit = 
                    (
                        blinkingUnit != null 
                        ? u.Value.units.FirstOrDefault(uu => uu.ID == blinkingUnit.ID)
                        : null
                    ) ?? u.Value.units.First();

                if ((blinkingUnit == null || unit.ID != blinkingUnit.ID || blink) && CheckUnit(c, r))
                {
                    var dest = new Rectangle(c * tileSize - x, r * tileSize - y, tileSize, tileSize);
                    var src = GetUnitImage(unit.unitClass);
                    pe.Graphics.DrawImage(unitTiles, dest, src, GraphicsUnit.Pixel);
                    k++;
                }
            }
            pe.Graphics.DrawString(k.ToString(), SystemFonts.DefaultFont, Brushes.White, 0, 0);
        }

        Rectangle GetUnitImage(GameModel.Units.UnitClass uc)
        {
            if (uc == Units.Settlers)
                return new Rectangle(0, 0, 64, 64);
            else
                return new Rectangle(64, 0, 64, 64);
        }

        public bool CheckUnit(int c, int r)
        {
            if (c >= x / tileSize && c <= (x + this.ClientSize.Width) / tileSize 
                && r >= y / tileSize && r <= (y + this.ClientSize.Height) / tileSize)
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

        internal void BlinkUnit(GameModel.Unit.Unit blinkingUnit)
        {
            this.blinkingUnit = blinkingUnit;
            this.blink = !this.blink;
            this.Invalidate();
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

        //public Unit GetUnit()
        //{
        //    Unit _unit = null;
        //    foreach (var unit in units)
        //    {
        //        if (CheckUnit(unit))
        //        {
        //            _unit = unit;
        //        }
        //    }
        //    return _unit;
        //}
        
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);            
        //    UnitMove?.Invoke(this, new UnitMoveEventArgs { Key = e.KeyCode});
        //    Invalidate();
        //}
        
        //public void MoveUnit(int dx, int dy, int n)
        //{
        //    units[n].Column += dx;
        //    units[n].Row += dy;
        //    cellC += dx;
        //    cellR += dy;

        //    int a = 0;
        //    if (units[n].Column * tileSize + tileSize > x + this.ClientSize.Width)
        //    {
        //        a = units[n].Column * tileSize + tileSize - (x + this.ClientSize.Width);
        //        x += a;
        //    }
        //    if (units[n].Column * tileSize < x)
        //    {
        //        a = units[n].Column * tileSize - x;
        //        x += a;
        //    }
        //    if (units[n].Row * tileSize + tileSize > y + this.ClientSize.Height)
        //    {
        //        a = units[n].Row * tileSize + tileSize - (y + this.ClientSize.Height);
        //        y += a;
        //    }
        //    if (units[n].Row * tileSize < y)
        //    {
        //        a = units[n].Row * tileSize - y;
        //        y += a;
        //    }
        //}
        
        //public int SelectUnit()
        //{
        //    int currentUnit = -1;
        //    for (int i = 0; i < units.Count; i++)
        //    {
        //        if (cellC == units[i].Column && cellR == units[i].Row)
        //        {
        //            currentUnit = i;
        //        }
        //    }
        //    return currentUnit;
        //}

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
