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
            var b = GameModel.GameModel.createWorld;
            foreach (var c in b.playerList[0].cities)
            {
                var d = City.GetFarmersYield(b.worldMap, c.Key, c.Value);
            }
        }
    }
}
