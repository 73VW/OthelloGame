using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace OthelloPedrettiFasmeyer.metier
{
    class IA : IPlayable.IPlayable
    {
        private const String NAME = "FasmeyerPedretti";
        private State root;
        private Board board;

        public IA()
        {
            board = new Board();
            root = new State(board);
        }

        public Tuple<double, Operation> Alphabeta(State root, int depth, int minOrMax, double parentValue)
        {
            // Minimize = -1; Maximize(when black) = 1;
            if (depth == 0)
            {
                return Tuple.Create((double)root.Eval(), new Operation(0, 0, 0));
            }
            double optVal = minOrMax * Double.NegativeInfinity;
            Operation optOp = null;
            foreach (Operation op in root.Ops())
            {
                State newRoot = root.Apply(op);
                Tuple<double, Operation> valDummy = Alphabeta(newRoot, depth - 1, -minOrMax, optVal);
                double val = valDummy.Item1;
                Operation dummy = valDummy.Item2;
                if (val * minOrMax > optVal * minOrMax)
                {
                    optVal = val;
                    optOp = op;
                    if (optVal * minOrMax > parentValue * minOrMax)
                    {
                        break;
                    }
                }
            }
            return Tuple.Create(optVal, optOp);
        }

        public int GetBlackScore()
        {
            return root.Eval();
        }

        public int GetWhiteScore()
        {
            return -root.Eval();
        }

        public int[,] GetBoard()
        {
            return board.Boxes;
        }

        public String GetName()
        {
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] board, int level, bool isWhiteTurn)
        {
            this.board = new Board(board, isWhiteTurn);
            root = new State(this.board);

            Tuple<double, Operation> bestMove = Alphabeta(root, level, (isWhiteTurn) ? -1 : 1, root.Eval());
            Operation move = bestMove.Item2;
            return Tuple.Create(move.x, move.y);
        }

        public bool PlayMove(int col, int line, bool isWhite)
        {
            return board.Apply(new Operation(col, line, (isWhite) ? -1 : 1));
        }

        public bool IsPlayable(int col, int line, bool isWhite)
        {
            return board.isPlayable(new Operation(col, line, (isWhite) ? -1 : 1));
        }
    }
}
