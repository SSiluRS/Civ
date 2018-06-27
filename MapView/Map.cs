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
    public partial class Map : Form
    {
        GameModel.World.World world = GameModel.GameModel.createWorld;

        //int worldX;
        //int worldY;
        private int worldX;

        public int WorldX
        {
            get { return worldX; }
            set { worldX = value; }
        }
        private int worldY;

        public int WorldY
        {
            get { return worldY; }
            set { worldY = value; }
        }



        public Map()
        {            
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            viewPort1.World = world;
            miniMap1.World = world;
            miniMap1.ViewPortWidth = viewPort1.ClientSize.Width / MapRenderer.tileSize;
            miniMap1.ViewPortHeigth = viewPort1.ClientSize.Height / MapRenderer.tileSize;
        }

        private void miniMap1_MapClick(object sender, MapClickEventArgs e)
        {
            WorldX = e.Column * MapRenderer.tileSize;
            WorldY = e.Row * MapRenderer.tileSize;
            viewPort1.SetLocation(
                worldX,
                worldX
            );
        }

        private void viewPort1_MapMove(object sender, MapMoveEventArgs e)
        {
            miniMap1.MoveRect((320 + e.C) % 320, e.R);
        }

        private void viewPort1_CellSelected(object sender, CellSelectedEventArgs e)
        {
            UI ui = new UI();
            ui.World = world;
            ui.SetCity(e.Column, e.Row);
            ui.ShowDialog();
        }

        private void viewPort1_UnitMove(object sender, UnitMoveEventArgs e)
        {
            UnitMove(e.Key);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    var dev = new Bitmap("DevelopmentS.png");
        //    var foodCount = 1;
        //    var productionCount = 2;
        //    var tradeCount = 1;
        //    var totalCount = foodCount + productionCount + tradeCount;
        //    var secondRow = totalCount / 2;
        //    var firstRow = totalCount - secondRow;
        //    var n = -1;
        //    for (int l = 0; l < 2; l++)
        //    {
        //        var rcount = (l == 0 ? firstRow : secondRow);
        //        for (int i = 0; i < rcount; i++)
        //        {
        //            if (foodCount > 0)
        //            {
        //                foodCount--;
        //                n = 0;
        //            }
        //            else if (productionCount > 0)
        //            {
        //                productionCount--;
        //                n = 1;
        //            }
        //            else
        //            {
        //                tradeCount--;
        //                n = 2;
        //            }
        //            var rsize = rcount * 64;
        //            var dest = new Rectangle(i * (MapRenderer.tileSize - 32) / (rcount - 1), l * 32, 32, 32);
        //            var src = new Rectangle(n * 16, 0, 16, 16);
        //            e.Graphics.DrawImage(dev, dest, src, GraphicsUnit.Pixel);
        //        }
        //    }
        //    e.Graphics.DrawRectangle(Pens.Red, 0, 0, 64, 64);
        //}

        public void UnitMove(Keys key)
        {
            int n = viewPort1.SelectUnit();
            int dx = 0;
            int dy = 0;
            if (n > -1)
            {
                switch (key)
                {
                    case Keys.A: dx = -1; /*units[n].Column -= 1; cellC -= 1;*/ break;
                    case Keys.D: dx = 1; /*units[n].Column += 1; cellC += 1;*/ break;
                    case Keys.W: dy = -1;/*units[n].Row -= 1; cellR -= 1*/ break;
                    case Keys.S: dy = 1;/*units[n].Row += 1; cellR += 1;*/ break;
                    case Keys.Q: dx = -1; dy = -1;/*units[n].Column -= 1; units[n].Row -= 1; cellR -= 1; cellC -= 1; */break;
                    case Keys.E: dx = 1; dy = -1;/*units[n].Column += 1; units[n].Row -= 1; cellR -= 1; cellC += 1; */break;
                    case Keys.Z: dx = -1; dy = 1;/*units[n].Column -= 1; units[n].Row += 1; cellR += 1; cellC -= 1;*/ break;
                    case Keys.C: dx = 1; dy = 1;/*units[n].Column += 1; units[n].Row += 1; cellR += 1; cellC += 1;*/ break;
                    default:
                        break;
                }
                viewPort1.MoveUnit(dx, dy, n);
                //miniMap1.MoveRectD(dx, dy);
            }

        }


    }
}
