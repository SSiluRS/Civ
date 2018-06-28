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

            var farmers = city
                .occupation
                .Where(o => o.IsFarmer)
                .Cast<City.Occupation.Farmer>()
                .Zip(farmList.Skip(1), Tuple.Create)
                .ToArray();

            for (int i = 0; i < farmers.Length + 1; i++)
            {
                var isCityCell = i == farmers.Length;
                // if (i == farmers.Length)
                var f = isCityCell ? farmList[0] : farmers[i].Item2;
                var foodCount = f.Item1;
                var productionCount = f.Item2;
                var tradeCount = f.Item3;
                var fc = isCityCell ? c : farmers[i].Item1.Item1;
                var fr = isCityCell ? r : farmers[i].Item1.Item2;
                DrawRecources(c, r, foodCount, productionCount, tradeCount, pe.Graphics, fc, fr);

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
                    var dest = new Rectangle(
                        i * 32 / (rcount > 1 ? rcount-- : rcount) + (farmerC - (c - 2)) * 64, 
                        l * 32 + (farmerR - (r - 2)) * 64, 32, 32);
                    var src = new Rectangle(n * 16, 0, 16, 16);
                    g.DrawImage(dev, dest, src, GraphicsUnit.Pixel);
                }
            }
        }

    }
}
