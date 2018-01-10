using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer
{
    class State
    {
        private int[,] board = new int[8, 8];
        int playerTurn;

        // Constrictor.
        public State(int[,] gameBoard, int playerTurn)
        {
            board = gameBoard;
            this.playerTurn = playerTurn;
        }

        // Public methodes.
        public double Eval()
        {
            int sum = 0;
            foreach (int check in board)
            {
                sum += check;
            }
            return sum;
        }
        public List<Operation> Ops()
        {
            return null;
        }
        public State Apply(Operation op)
        {

            return null;
        }

    }
}
