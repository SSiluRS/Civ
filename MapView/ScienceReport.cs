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
    public partial class ScienceReport : Form
    {
        GameModel.World.World world;
        GameModel.Civilization.Civilization civ
        {
            get => world.playerList[world.currentPlayer];
        }

        public ScienceReport()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            ActiveAdvance = civ.currentlyDiscovering;
            label1.Text = ActiveAdvance.name;
            SetProgressBar();
            var allAdvances = GameModel.Science.allAdvances;
            foreach (var advance in civ.discoveries)
            {
                listView1.Items.Add(advance.name);
            }
            var canDiscover = GameModel.Utils.allowedAdvances(civ.discoveries);
            listBox1.DataSource = canDiscover.ToList();
        }

        private void SetProgressBar()
        {
            var max = GameModel.World.researchDestination(civ);
            var progress = civ.researchProgress;
            progressBar1.Maximum = max;
            progressBar1.Minimum = 0;
            progressBar1.Value = progress;
            
        }

        public void SetWorld(GameModel.World.World world)
        {
            this.world = world;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ActiveAdvance = listBox1.SelectedItem as GameModel.Science.Advance;
            label1.Text = ActiveAdvance.name;
        }

        public GameModel.Science.Advance ActiveAdvance { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}