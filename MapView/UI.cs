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
            foodCountLabel.Text = city.food.ToString();
            var population = city.occupation.Count();
            var artistCount = 0;
            var scientistCount = 0;
            var taxCollectorCount = 0;
            foreach (var human in city.occupation)
            {
                if (human.IsArtist)
                {
                    artistCount++; 
                }
                if (human.IsScientist)
                {
                    scientistCount++;
                }
                if (human.IsTaxCollector)
                {
                    taxCollectorCount++;
                }
            }
            populationListView.Items.Add("Total popuation:").SubItems.Add(population.ToString());
            populationListView.Items.Add("Artists:").SubItems.Add(artistCount.ToString());
            populationListView.Items.Add("Scientists:").SubItems.Add(scientistCount.ToString());
            populationListView.Items.Add("TaxCollectors:").SubItems.Add(taxCollectorCount.ToString());
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
