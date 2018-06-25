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
        int cellC;
        int cellR;
        bool drawCell = false;
        int oldCellX;
        int oldCellY;
        Pen myPen = new Pen(Color.White, 2);
        Rectangle cell;
        List<Unit> units;

        int tileSize = MapRenderer.tileSize;

        public ViewPort()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            rnd = new Random();
            units = CreateUnits();
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

            var image = mapRenderer.Render(x - 1, y);
            pe.Graphics.DrawImageUnscaled(image, 0, 0);

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
            if (unit.Column > x / tileSize - 1 && unit.Column < (x + this.ClientSize.Width) / tileSize +1 && unit.Row > y / tileSize && unit.Row < (y + this.ClientSize.Height) / tileSize)
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
            int n = SelectUnit();
            if (n > -1)
            {
                switch (e.KeyCode)
                {
                    case Keys.A: units[n].Column -= 1; cellC -= 1; break;
                    case Keys.D: units[n].Column += 1; cellC += 1; break;
                    case Keys.W: units[n].Row -= 1; cellR -= 1; break;
                    case Keys.S: units[n].Row += 1; cellR += 1; break;
                    case Keys.Q: units[n].Column -= 1; units[n].Row -= 1; cellR -= 1; cellC -= 1; break;
                    case Keys.E: units[n].Column += 1; units[n].Row -= 1; cellR -= 1; cellC += 1; break;
                    case Keys.Z: units[n].Column -= 1; units[n].Row += 1; cellR += 1; cellC -= 1; break;
                    case Keys.C: units[n].Column += 1; units[n].Row += 1; cellR += 1; cellC += 1; break;
                    default:
                        break;
                }
                if (units[n].Column + 0.5 > (x + this.ClientSize.Width) / tileSize)
                {
                    x += tileSize;
                }
                if (units[n].Column < x / tileSize)
                {
                    x -= tileSize;
                }
            }
            Invalidate();
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
}
