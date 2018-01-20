using System;
using System.Collections.Generic;

namespace OthelloIAG5
{
    /// <summary>Specific methods for the AlphaBeta algorithm.</summary>
    public class State : LegalMove
    {
        private EBoxType currentType;

        public State(int[,] currentState, EBoxType type)
        {
            this.boxes = currentState;
            this.currentType = type;
        }

        public EBoxType CurrentType
        {
            get => currentType;
        }

        public double Eval()
        {
            int eval = 0;
            for(int row = 0; row < Board.BOARD_SIZE; row++)
            {
                for (int col = 0; col < Board.BOARD_SIZE; col++)
                {

                    //search for corners
                    if ((row == 0 && (col == 0 || col == Board.BOARD_SIZE - 1)) || (row == Board.BOARD_SIZE - 1 && (col == 0 || col == Board.BOARD_SIZE - 1))) eval += 20;
                    else
                    {
                        //search for borders
                        //left
                        if (col == 0) eval += 5;
                        //right
                        if (col == Board.BOARD_SIZE - 1) eval += 5;
                        //top
                        if (row == 0) eval += 5;
                        //bottom
                        if (row == Board.BOARD_SIZE - 1) eval += 5;

                    }

                }
            }
            return eval;
        }

        public bool Final()
        {
            //if one of the box is empty
            foreach(int box in boxes)
            {
                if (box == (int)EBoxType.free) return false;
            }
            return true;
        }

        /// <summary>A list of all legal moves for the current player.</summary>
        public List<Tuple<int, int>> Ops()
        {
            List<Tuple<int, int>> moveList = new List<Tuple<int, int>>();
            for(int col = 0; col < Board.BOARD_SIZE; col++)
            {
                for(int row = 0; row < Board.BOARD_SIZE; row++)
                {
                    if(ChangeBox(col, row, currentType == EBoxType.white)) moveList.Add(new Tuple<int, int>(col, row));
                }
            }
            return moveList;
        }

        public State Apply(Tuple<int, int> move)
        {
            int[,] newState = (int[,])boxes.Clone();
            newState[move.Item1, move.Item2] = (int)currentType;
            EBoxType newType;
            if (currentType == EBoxType.white) newType = EBoxType.black;
            else newType = EBoxType.white;
            return new State(newState, newType);
        }
    }
}
