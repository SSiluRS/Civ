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
    public partial class CityMap : Control
    {

        Bitmap city;

        public CityMap()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (DesignMode) return;
            //pe.Graphics.DrawImage(city, 0, 0);
        }

        public void DrawCity(Bitmap city)
        {
            this.city = city;
        }
    }
}
