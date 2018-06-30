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

        public World.World World { get; set; }

        Civilization.Civilization civ
        {
            get => World.playerList[0];
        }

        City.City city;
        public City.CurrentlyBuilding ActiveBuilding { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            ActiveBuilding = city.currentlyBuilding;
            if (ActiveBuilding.IsBuilding)
            {
                var building = ActiveBuilding as City.CurrentlyBuilding.Building;
                label1.Text = building.Item.name;
            }
            else if (ActiveBuilding.IsUnit)
            {
                var unit = ActiveBuilding as City.CurrentlyBuilding.Unit;
                label1.Text = unit.Item.ToString();
            }
            else if (ActiveBuilding.IsTradeGoods)
            {
                label1.Text = "TradeGoods";
            }
            else
                throw new NotImplementedException();

            SetProgressBar();

            var allUnits = Units.allUnits;
            var allBuildings = Buildings.allBuildings;
            foreach (var building in city.building)
            {
                listView1.Items.Add(building.ToString());
            }
            var canBuild = Utils.allowedBuildings(civ.discoveries);
            var built = city.building.ToLookup(b => b);
            var _canBuild = canBuild.Where(b => !built.Contains(b));
            foreach (var b in _canBuild)
            {
                listBox2.Items.Add(b);
            }

            foreach (var u in city.units)
            {
                listView1.Items.Add(u.ToString());
            }
            var canCreate = Utils.allowedUnits(civ.discoveries);
            foreach (var u in canCreate)
            {
                listBox2.Items.Add(u);
            }
        }

        private void SetProgressBar()
        {
            var production = city.production;
            var max = GameModel.World.currentBuildingDestination(city);
            progressBar1.Maximum = max;
            progressBar1.Value = production;
            
        }

        public void SetCity(int c, int r)
        {
            city = FindCity(c, r);
            cityMap1.World = World;
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
            foreach (var player in World.playerList)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            var b = listBox2.SelectedItem as GameModel.Buildings.Building;
            var u = listBox2.SelectedItem as GameModel.Units.UnitClass;

            ActiveBuilding = 
                b is null ? 
                City.CurrentlyBuilding.NewUnit(u) :
                City.CurrentlyBuilding.NewBuilding(b);            
            label2.Text = ActiveBuilding.ToString();
        }
    }
}
