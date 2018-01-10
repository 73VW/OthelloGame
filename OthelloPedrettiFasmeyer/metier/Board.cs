using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer.metier
{
    public class Board
    {
        int[,] boxes = new int[8, 8];

        public Board()
        {
            Console.WriteLine($"default value: {boxes[0, 0]}");

        }

        public void play()
        {
            String test = Console.ReadLine();
            Console.WriteLine(test);
        }

    }
}
