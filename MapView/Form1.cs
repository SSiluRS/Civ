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
    public partial class Form1 : Form
    {

        public Form1()
        {            
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            miniMap1.ViewPortWidth = viewPort1.ClientSize.Width / MapRenderer.tileSize;
            miniMap1.ViewPortHeigth = viewPort1.ClientSize.Height / MapRenderer.tileSize;
        }

        private void miniMap1_MapClick(object sender, MapClickEventArgs e)
        {
            viewPort1.SetLocation(e.Column - viewPort1.ClientSize.Width / 2, e.Row - viewPort1.ClientSize.Height / 2);
        }
    }
}
