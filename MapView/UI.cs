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
    public partial class UI : Form
    {
        public UI()
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

        City.City city;

        public void SetCity(int c, int r)
        {
            city = FindCity(c, r);
            cityMap1.World = world;
            cityMap1.SetCity(city, c, r);
        }

        private City.City FindCity(int c, int r)
        {
            foreach (var player in world.playerList)
            {
                foreach (var city in player.cities)
                {
                    if (city.Key.Item1 == c && city.Key.Item2 == r)
                    {
                        return city.Value;
                    }
                }
            }
            return null;
        }
    }
}
