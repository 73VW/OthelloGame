using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OthelloPedrettiFasmeyer.metier
{
    class IA
    {

        public Tuple<double, Operation> Alphabeta(State root, int depth, int minOrMax, double parentValue)
        {
            /**
             *  Minimize = -1; Maximize = 1;
             */
            if (depth == 0)
            {
                return Tuple.Create(root.Eval(), new Operation(-1, -1, 0));
            }
            double optVal = minOrMax * Double.NegativeInfinity; ;
            Operation optOp = null;
            foreach (Operation op in root.Ops())
            {
                State newRoot = root.Apply(op);
                Tuple<double, Operation> valDummy = this.Alphabeta(newRoot, depth - 1, -minOrMax, optVal);
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
    }
}
