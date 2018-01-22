using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IPlayable;

namespace OthelloIAG5
{
    [Serializable]
    public class IA
    {
        private const int MAXDEPTH = 4;

        private IPlayable.IPlayable board;

        public IA(IPlayable.IPlayable board)
        {
            this.board = board;
        }

        public IA()
        {
            this.board = new Board();
        }

        public Tuple<double, Tuple<int, int>> Alphabeta(State root, int depth, int minOrMax, double parentValue)
        {
            // Minimize = -1; Maximize = 1;
            if (depth == 0 || root.Final())
            {
                return Tuple.Create(root.Eval(), Tuple.Create(-1,-1));
            }
            double optVal = minOrMax * Double.NegativeInfinity;
            Tuple<int, int> optOp = Tuple.Create(-1,-1);
            var ops = root.Ops();
            if (ops.Count > 0)
                optOp = ops[0];

            foreach (Tuple<int,int> op in ops)
            {
                State newRoot = root.Apply(op);
                Tuple<double, Tuple<int, int>> valDummy = Alphabeta(newRoot, depth - 1, -minOrMax, optVal);
                double val = valDummy.Item1;
                Tuple<int, int> dummy = valDummy.Item2;
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

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            EBoxType type;
            if (whiteTurn) type = EBoxType.white;
            else type = EBoxType.black;
            State currentState = new State(game, type);
            Tuple<double, Tuple<int, int>> bestMove = Alphabeta(root: currentState, depth: level, minOrMax: -1, parentValue: currentState.Eval());
            Tuple<int, int> nextMove = bestMove.Item2;
            return nextMove;
        }
    }
}
