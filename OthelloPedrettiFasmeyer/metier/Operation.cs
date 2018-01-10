using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer
{
    class Operation
    {
        public int x = 0;
        public int y = 0;
        public int player;

        public Operation(int x, int y, int player)
        {
            this.x = x;
            this.y = y;
            this.player = player;
        }
    }
}
