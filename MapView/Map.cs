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

        private int worldX;
        private int worldY;

        GameModel.Unit.Unit activeUnit = null;

        public Map()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            viewPort1.SetWorld(world);
            miniMap1.World = world;
            miniMap1.ViewPortWidth = viewPort1.ClientSize.Width / MapRenderer.tileSize;
            miniMap1.ViewPortHeigth = viewPort1.ClientSize.Height / MapRenderer.tileSize;
        }

        private void miniMap1_MapClick(object sender, MapClickEventArgs e)
        {
            worldX = e.Column * MapRenderer.tileSize;
            worldY = e.Row * MapRenderer.tileSize;
            viewPort1.SetLocation(
                worldX,
                worldY
            );
        }

        private void viewPort1_MapMove(object sender, MapMoveEventArgs e)
        {
            miniMap1.MoveRect((320 + e.C) % 320, e.R);
        }

        private void viewPort1_CellSelected(object sender, CellSelectedEventArgs e)
        {           
            UnitsList.Items.Clear();
            foreach (var u in world.units)
            {
                if (e.Column == u.Key.Item1 && e.Row == u.Key.Item2)
                {
                    foreach (var unit in u.Value.units)
                    {
                        UnitsList.Items.Add(unit);
                    }
                }
            }
            foreach (var player in world.playerList)
            {
                foreach (var city in player.cities)
                {
                    if (e.Column == city.Key.Item1 && e.Row == city.Key.Item2)
                    {
                        UI ui = new UI();
                        ui.World = world;
                        ui.SetCity(e.Column, e.Row);
                        ui.ShowDialog();                   

                    }
                }
            }
        }


        private void Map_KeyDown(object sender, KeyEventArgs e)
        {
            if (activeUnit == null) return;
            var loc = GameModel.World.getUnitLoc(world, activeUnit);
            CheckKey(e.KeyCode, out var dx, out var dy, out var command);
            if (command == Command.Move)
            {
                var moveResult = GameModel.World.moveUnit(world, activeUnit, loc.Item1 + dx, loc.Item2 + dy);
                this.world = moveResult.Item1;
                activeUnit = moveResult.Item2.Value;
            }
            else if (command == Command.BuildCity)
            {
                var civ = GameModel.World.getCivByUnit(world, activeUnit);
                world = GameModel.World.unitMakesCity(world, civ, activeUnit);
                activeUnit = null;
            }
            viewPort1.SetWorld(world);
            miniMap1.World = world;
        }


        public void CheckKey(Keys key, out int dx, out int dy, out Command command)
        {
            dx = 0;
            dy = 0;
            command = Command.Move;
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
                case Keys.B: command = Command.BuildCity; break;
                default:
                    break;
            }
            //miniMap1.MoveRectD(dx, dy);
        }

        private void UnitsList_SelectedValueChanged(object sender, EventArgs e)
        {
            activeUnit = UnitsList.SelectedItem as GameModel.Unit.Unit;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (activeUnit != null)
                viewPort1.BlinkUnit(activeUnit);
        }


        //public List<Unit> CreateUnits()
        //{
        //    List<Unit> units = new List<Unit>();
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        units.Add(new Unit { Column = rnd.Next(1, 318), Row = rnd.Next(1, 158), ImageIndex = rnd.Next(0, 28) });
        //    }
        //    return units;
        //}
    }

    public enum Command
    {
        Move,
        BuildCity,
        BuildRoad,
        Irrigate,
        Fortify,
        Pause
    }
}

