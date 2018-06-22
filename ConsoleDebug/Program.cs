using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = World.WorldUpdate.GetCityCells(10000).ToArray();
            var b = World.WorldUpdate.GetSortedCityCells(10000).ToArray();
            var c = World.WorldUpdate.AssignFarmersToCell(10000, 5).ToArray();
        }
    }
}
