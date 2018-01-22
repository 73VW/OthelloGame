using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloCommand
{
    [Serializable]
    abstract class Command
    {
        public abstract bool Execute();
        public abstract bool UnExecute();
    }
}
