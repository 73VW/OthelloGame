using System;
using System.Collections.Generic;

namespace OthelloIAG5
{
    /// <summary>Specific methods for the AlphaBeta algorithm.</summary>
    public class State : LegalMove
    {
        private EBoxType currentType;

        // Matrix from http://dhconnelly.com/paip-python/docs/paip/othello.html

        public State(int[,] currentState, EBoxType type)
        {
            this.boxes = currentState;
            this.currentType = type;
            this.evalMatrix = new int[8,8]
            {
                    { 120, -20,  20,   5,   5,  20, -20, 120},
                    { -20, -40,  -5,  -5,  -5,  -5, -40, -20},
                    {  20,  -5,  15,   3,   3,  15,  -5,  20},
                    {   5,  -5,   3,   3,   3,   3,  -5,   5},
                    {   5,  -5,   3,   3,   3,   3,  -5,   5},
                    {  20,  -5,  15,   3,   3,  15,  -5,  20},
                    { -20, -40,  -5,  -5,  -5,  -5, -40, -20},
                    { 120, -20,  20,   5,   5,  20, -20, 120},
            };
        }

        public EBoxType CurrentType
        {
            get => currentType;
        }

        public double Eval()
        {
            int playerScore = 0;
            int opponentScore = 0;

            // Dynamically modify evalMatrix.
            for (int row = 0; row < Board.BOARD_SIZE; row++)
            {
                for (int col = 0; col < Board.BOARD_SIZE; col++)
                {
                    Definitive(row, col);
                }
            }

            for (int row = 0; row < Board.BOARD_SIZE; row++)
            {
                for (int col = 0; col < Board.BOARD_SIZE; col++)
                {
                    if (boxes[row, col] == (int)currentType)
                        playerScore += evalMatrix[row, col];
                    else if (boxes[row, col] != (int)EBoxType.free)
                        opponentScore += evalMatrix[row, col];
                }
            }
            return playerScore - opponentScore;
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
