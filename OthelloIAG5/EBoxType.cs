using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIAG5
{
    /// <summary>
    /// Enumerate box types.
    /// Used to logicaly acces box values (free is more logical than -1).
    /// </summary>
    public enum EBoxType
    {
        free = -1,
        white = 0,
        black = 1
    }
}
