using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer.metier
{
    public class BoardB
    {
        private int[,] boxes = new int[8, 8];
        public int[,] Boxes{ get{ return boxes.Clone() as int[,]; } }

        private bool isWhiteTurn;
        public bool IsWhiteTurn { get{ return isWhiteTurn; } }

        // Not implemented yet.
        public bool useTimer = false;
        public int timer = 0;

        private readonly List<Tuple<int, int>> DIRECTIONS = new List<Tuple<int, int>>()
        {
            Tuple.Create(1,0),
            Tuple.Create(-1,0),
            Tuple.Create(0,1),
            Tuple.Create(0,-1),
            Tuple.Create(1,1),
            Tuple.Create(1,-1),
            Tuple.Create(-1,1),
            Tuple.Create(-1,-1)
        };

        private readonly Tuple<int, int> INVALID = Tuple.Create(-1, -1);

        public BoardB()
        {
            isWhiteTurn = false;
            boxes[3, 3] = -1;
            boxes[4, 3] = 1;
            boxes[3, 4] = 1;
            boxes[4, 4] = -1;
        }

        public BoardB(BoardB board)
        {
            isWhiteTurn = board.IsWhiteTurn;
            boxes = board.Boxes;
        }

        public BoardB(int[,] board, bool isWhiteTurn)
        {
            this.isWhiteTurn = isWhiteTurn;
            boxes = board;
        }

        public void Print()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write("{0,2} ", boxes[j, i]);
                }
                Console.Write("\n");
            }
            Console.Write("\n");
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
            if (op.player == 0)
                // No modificaiton (final move).
                return false;

            if (Math.Abs(boxes[op.x, op.y]) == 1)
                // Board[x,y] not empty(error).
                return false;

            int currentPlayer = (isWhiteTurn) ? -1 : 1;
            if (op.player != currentPlayer)
                return false;

            // Create a list of all modifications to be done.
            List<Tuple<int, int>> moveList = new List<Tuple<int, int>>();
            foreach (var dir in DIRECTIONS)
            {
                int x = dir.Item1;
                int y = dir.Item2;
                Tuple<int, int> move = LegalLine(op, x, y, currentPlayer);
                if (!move.Equals(INVALID))
                    moveList.Add(move);
            }

            // If no legal move(no modifications), stops.
            if (moveList.Count == 0)
                return false;

            // Add pawn.
            boxes[op.x, op.y] = currentPlayer;

            // Exchange lines of pawn.
            foreach (var move in moveList)
            {
                int x = op.x;
                int y = op.y;
                int destX = move.Item1;
                int destY = move.Item2;
                int incX = (destX > op.x)? 1 : (destX < op.x)? -1 : 0;
                int incY = (destY > op.y)? 1 : (destY < op.y)? -1 : 0;

                while (x != destX || y != destY)
                {
                    if (boxes[x, y] == -currentPlayer)
                        boxes[x, y] = currentPlayer;

                    x += incX;
                    y += incY;
                }
            }

            //change player turn.
            isWhiteTurn = !isWhiteTurn;

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
            int currentPlayer = (isWhiteTurn) ? -1 : 1;
            List<Operation> opList = new List<Operation>();
            for (int i = 0; i < 8; i++ )
            {
                for (int j = 0; j < 8; j++)
                {
                    Operation op = new Operation(i, j, currentPlayer);
                    if (IsPlayable(op))
                        opList.Add(op);
                }                
            }
            return opList;
        }

        /// <summary>
        /// Check if the game is finished.
        /// </summary>
        /// <returns> Whether it is finished or not.</returns>
        public bool IsFinished()
        {
            // Is timer out?
            if (useTimer && timer == 0)
                return true;

            // Are players stuck?
            if (Ops().Count == 0)
                isWhiteTurn = !isWhiteTurn;
                if (Ops().Count == 0)
                    return true;

                isWhiteTurn = !isWhiteTurn;

            return false;
        }

        private bool LegalMove(Operation op, int currentPlayer)
        {
            // Check for all directions.
            foreach (var dir in DIRECTIONS)
            {
                int x = dir.Item1;
                int y = dir.Item2;
                if (!LegalLine(op, x, y, currentPlayer).Equals(INVALID))
                    return true;
            }
            // Nothing found.
            return false;
            
        }

        /// <summary>
        /// Check if a legal move can be made in a direction incX, incY.
        /// </summary>
        /// <param name="op">Operation, where we want to place our pawn.</param>
        /// <param name="incX">Direction x in which we look for a legal move. {-1, 0, 1}</param>
        /// <param name="incY">Direction y in which we look for a legal move. {-1, 0, 1}</param>
        /// <param name="currentPlayer">Who's color we are looking for.</param>
        /// <returns>Return INVALID(-1, -1) if nothing found or the line end pawn(x, y) if a legal move is found.</returns>
        private Tuple<int, int> LegalLine(Operation op, int incX, int incY, int currentPlayer)
        {
            const int MAX = 8;
            const int MIN = 0;
            int x = op.x + incX;
            int y = op.y + incY;

            // Incorect input.
            if ((incX == 0 && incY == 0) || Math.Abs(incX) > 1 || Math.Abs(incY) >1)
                return INVALID;

            if (x >= MAX || x < MIN || y >= MAX || y < MIN)
                return INVALID;

            // Box next to operation must be owned by the opponent to even be a legal move. Exemple: xox.
            if (boxes[x, y] != -currentPlayer)
                return INVALID;

            x += incX;
            y += incY;
            while (x < MAX && x >= MIN && y < MAX && y >= MIN)
            {
                // If there is a hole in the line, it can not be a legal move.
                if (boxes[x, y] == 0)
                    return INVALID;

                // If we found our own pawn, we got a legal move.
                if (boxes[x, y] == currentPlayer)
                    return Tuple.Create(x, y);

                x += incX;
                y += incY;
            }

            //Noting found.
            return INVALID;
        }
    }
}
