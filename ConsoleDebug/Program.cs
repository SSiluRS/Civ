using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //MapGenerator.MapGenerator mg = new MapGenerator.MapGenerator();
            //mg.GenerateToFile("map.sav");
            //var map = mg.LoadMapFromFile("map.sav");
            /*var a = World.WorldUpdate.GetCityCells(10000).ToArray();
            var b = World.WorldUpdate.GetSortedCityCells(10000).ToArray();
            var c = World.WorldUpdate.AssignFarmersToCell(10000, 5).ToArray();*/
            var e = GameModel.GameModel.createWorld;
            var b = GameModel.GameModel.demoUnit(e, e.playerList);
            var c = GameModel.World.settlerBuildsRoad(b, b.units[new Tuple<int, int>(0, 0)].units[0]);
            //var c = GameModel.World.unitMakesCity(b, b.units[new Tuple<int, int>(160, 80)].units[0]);
            //var c = GameModel.World.moveUnit(b, b.units[new Tuple<int, int>(0, 0)].units[0], 1, 1);
            for (int i = 0; i < 100; i++)
            {
                c = GameModel.World.UpdateWorld(c);
            }
            /*foreach (var c in b.playerList[0].cities)
            {
                var d = City.GetFarmersYield(b.worldMap, c.Key.Item1, c.Key.Item2, c.Value);
            }*/
        }
    }
}
