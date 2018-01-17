using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer.metier
{
    public class Board
    {
        private int[,] boxes = new int[8, 8];
        public int[,] Boxes{ get{ return boxes.Clone() as int[,]; } }
        private bool isWhiteTurn;
        public bool IsWhiteTurn { get{ return isWhiteTurn; } }

        public Board()
        {
            isWhiteTurn = false;
            boxes[4, 4] = -1;
            boxes[5, 4] = 1;
            boxes[4, 5] = 1;
            boxes[5, 5] = -1;
        }

        public Board(Board board)
        {
            isWhiteTurn = board.IsWhiteTurn;
            boxes = board.Boxes;
        }

        public Board(int[,] board, bool isWhiteTurn)
        {
            this.isWhiteTurn = isWhiteTurn;
            boxes = board;
        }

        public bool IsPlayable(Operation op)
        {
            if (op.player == 0)
                //no modificaiton (final move).
                return false;

            if (Math.Abs(boxes[op.x, op.y]) == 1)
                // board[x,y] not empty(error).
                return false;

            // check for lignes and diagonals.
            int currentPlayer = (isWhiteTurn) ? -1: 1;
            return LegalMove(op, currentPlayer);
        }

        /// <summary>
        /// Modify the board with the player's move(Operation).
        /// </summary>
        /// <param name="op">Operation object, provide position of and player performing the operation.</param>
        /// <returns>Wherer or not the operation has been applied properly.</returns>
        public bool Apply(Operation op)
        {

            // TO DO.

            if (!IsPlayable(op))
                return false;

            // Apply modification.
                // Add pawn.
                // Exchange lines of pawn.
                //change player turn.

            return true;
        }

        /// <summary>
        /// Tells which player is winning.
        /// </summary>
        /// <returns>If positive, black is winning.</returns>
        public int Eval()
        {
            // Black := 1; White := -1
            int sum = 0;
            foreach (int check in boxes)
            {
                sum += check;
            }
            return sum;
        }

        /// <summary>A list of all legal moves for the current player.</summary>
        public List<Operation> Ops()
        {

            //TO DO.

            List<Operation> list = new List<Operation>();
            return list;
        }

        public bool IsFinished()
        {
            if (this.Ops().Count == 0)
                return true;

            return false;
        }

        private bool LegalMove(Operation op, int currentPlayer)
        {
            // opponent pawn = -currentPlayer.

            //Horizontal
            bool checkRight = false;
            bool checkLeft = false;
            if (op.x < boxes.Length-2 &&  boxes[op.x + 1, op.y] == -currentPlayer)
            {
                checkRight = true;
            }
            if (op.x > 1 && boxes[op.x - 1, op.y] == -currentPlayer)
            {
                checkLeft = true;
            }

            int x = op.x + 2;
            while (checkRight && op.x < boxes.Length)
            {
                int result = boxes[x, op.y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkRight = false;
                x++;
            }

            x = op.x -2;
            while (checkLeft && op.x >= 0)
            {
                int result = boxes[x, op.y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkLeft = false;
                x--;
            }

            //Vertical
            bool checkUp = false;
            bool checkDown = false;
            if (op.y < boxes.Length - 2 && boxes[op.x, op.y + 1] == -currentPlayer)
            {
                checkUp = true;
            }
            if (op.y > 1 && boxes[op.x, op.y - 1] == -currentPlayer)
            {
                checkDown = true;
            }

            int y = op.y + 2;
            while (checkUp && op.y < boxes.Length)
            {
                int result = boxes[op.x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkUp = false;
                y++;
            }

            y = op.y - 2;
            while (checkDown && op.y >= 0)
            {
                int result = boxes[op.x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkDown = false;
                y--;
            }

            //Diagonal1
            bool checkUpRight = false;
            bool checkDownLeft = false;
            if (op.y < boxes.Length - 2 && op.x < boxes.Length - 2 && boxes[op.x + 1, op.y + 1] == -currentPlayer)
            {
                checkUpRight = true;
            }
            if (op.y > 1 && op.x > 1 && boxes[op.x - 1, op.y - 1] == -currentPlayer)
            {
                checkDownLeft = true;
            }

            y = op.y + 2;
            x = op.x + 2; ;
            while (checkUpRight && op.y < boxes.Length && op.x < boxes.Length)
            {
                int result = boxes[x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkUpRight = false;
                x++;
                y++;
            }

            y = op.y - 2;
            x = op.x - 2; ;
            while (checkDownLeft && op.y >=0 && op.x >= 0)
            {
                int result = boxes[x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkDownLeft = false;
                x--;
                y--;
            }

            //Diagonal2
            bool checkUpLeft = false;
            bool checkDownRight = false;
            if (op.y < boxes.Length - 2 && op.x > 1 && boxes[op.x - 1, op.y + 1] == -currentPlayer)
            {
                checkUpLeft = true;
            }
            if (op.y > 1 && op.x < boxes.Length - 2 && boxes[op.x + 1, op.y - 1] == -currentPlayer)
            {
                checkDownRight = true;
            }

            y = op.y + 2;
            x = op.x - 2; ;
            while (checkUpLeft && op.y < boxes.Length && op.x <= 0)
            {
                int result = boxes[x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkUpLeft = false;
                x--;
                y++;
            }

            y = op.y - 2;
            x = op.x + 2; ;
            while (checkDownRight && op.y >= 0 && op.x < boxes.Length)
            {
                int result = boxes[x, y];
                if (result == currentPlayer)
                    return true;
                else if (result == 0)
                    checkDownRight = false;
                x++;
                y--;
            }

            return false;
        }
    }
}
