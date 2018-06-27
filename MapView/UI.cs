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
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        internal void SetCity(int c, int r)
        {
            //var mapRender = new MapRenderer(5 * MapRenderer.tileSize, 5 * MapRenderer.tileSize, );
            //cityMap1.DrawCity(mapRender.Render(c, r));
        }
    }
}
