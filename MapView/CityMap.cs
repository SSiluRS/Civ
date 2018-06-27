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
    public partial class CityMap : Control
    {

        Bitmap cityMap;
        City.City city;
        int r;
        int c;
        int farmerC;
        int farmerR;

        public CityMap()
        {
            InitializeComponent();
        }

        private GameModel.World.World world;

        public GameModel.World.World World
        {
            get { return world; }
            set
            {
                world = value;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (DesignMode) return;
            pe.Graphics.DrawImage(cityMap, 0, 0);
            pe.Graphics.FillRectangle(Brushes.Gray, new Rectangle(2 * 64, 2 * 64, 64, 64));

            var farmList = City.GetFarmersYield(world.worldMap, c, r, city);
            var farmerNumber = -1;
            City.Occupation.Farmer farmer;
            foreach (var human in city.occupation)
            {
                if (human.IsFarmer)
                {
                    farmerNumber++;
                    farmer = human as City.Occupation.Farmer;
                    farmerC = farmer.Item1;
                    farmerR = farmer.Item2;
                }
                var foodCount = farmList[farmerNumber].Item1;
                var productionCount = farmList[farmerNumber].Item2;
                var tradeCount = farmList[farmerNumber].Item3;
                DrawRecources(c, r, foodCount, productionCount, tradeCount, pe.Graphics, farmerC, farmerR);
            }

        }

        public void SetCity(City.City city, int c, int r)
        {
            using (var mapRender = new MapRenderer(5 * MapRenderer.tileSize, 5 * MapRenderer.tileSize, world))
                cityMap = mapRender.Render((c - 2) * 64, (r - 2) * 64);
            this.city = city;
            this.c = c;
            this.r = r;
            Invalidate();
        }

        public void DrawRecources(int c, int r, int foodCount, int productionCount, int tradeCount, Graphics g, int farmerC, int farmerR)
        {
            var dev = new Bitmap("../../Map/DevelopmentS.png");
            var totalCount = foodCount + productionCount + tradeCount;
            var secondRowCount = totalCount / 2;
            var firstRowCount = totalCount - secondRowCount;
            var n = -1;
            for (int l = 0; l < 2; l++)
            {
                var rcount = (l == 0 ? firstRowCount : secondRowCount);
                for (int i = 0; i < rcount; i++)
                {
                    if (foodCount > 0)
                    {
                        foodCount--;
                        n = 0;
                    }
                    else if (productionCount > 0)
                    {
                        productionCount--;
                        n = 1;
                    }
                    else
                    {
                        tradeCount--;
                        n = 2;
                    }
                    var dest = new Rectangle(i * (MapRenderer.tileSize) / (rcount), l * 32, 32, 32);
                    var src = new Rectangle(n * 16, 0, 16, 16);
                    g.DrawImage(dev, dest, src, GraphicsUnit.Pixel);
                }
            }
        }

    }
}
