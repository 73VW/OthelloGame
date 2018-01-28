using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloCommand
{
    /// <summary>
    /// Command pattern for do/undo actions on the board.
    /// </summary>
    [Serializable]
    abstract class Command
    {
        public abstract bool Execute();
        public abstract bool UnExecute();
    }
}
