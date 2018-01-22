using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OthelloIAG5;

namespace OthelloCommand
{
    [Serializable]
    class FlipCommand : Command
    {
        private int col;
        private int row;
        private bool whiteTurn;
        private Board board;
        private int[,] oldState;
        public FlipCommand(Board _board, int _col, int _row, bool _whiteTurn)
        {
            Col = _col;
            Row = _row;
            WhiteTurn = _whiteTurn;
            Board = _board;
            oldState = (int[,])board.GetBoard().Clone();
        }

        public int Col { get => col; set => col = value; }
        public int Row { get => row; set => row = value; }
        public bool WhiteTurn { get => whiteTurn; set => whiteTurn = value; }
        public Board Board { get => board; set => board = value; }

        public override bool Execute()
        {
            return board.ChangeBox(col, row, whiteTurn, true);
        }

        public override bool UnExecute()
        {
            board.Boxes = oldState;
            return whiteTurn;
        }


    }
}
