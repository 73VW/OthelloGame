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

        /// <summary>
        /// Determines how well is a given player performing.
        /// A number of values are used to determine what makes a good game state:
        ///     The number of pawns on the board.
        ///     The number of avaliable moves.
        ///     The number of definitive pawns (not modifiable by opponent).
        ///     The position of the pawn relative to the borders and critical boxes.
        /// We will note that most of those values are calculated trough evaluation matrix. 
        /// Only the number of avaliable moves is calculated without the matrix. 
        /// </summary>
        /// <returns>
        /// Performance of the given player - performance of its opponent.
        /// </returns>
        public double Eval()
        {
            int playerScore = 0;
            int opponentScore = 0;

            for (int row = 0; row < Board.BOARD_SIZE; row++)
            {
                for (int col = 0; col < Board.BOARD_SIZE; col++)
                {
                    // Determines which boxes are definitive (opponent can not take it back) and set them at 120 in the evalMatrix.
                    Definitive(row, col);

                    // Evaluate all pawns using the ponderation (Multiply boxes[x,y] with evalMatrix[x,y]).
                    // Player score.
                    if (boxes[row, col] == (int)currentType)
                        playerScore += evalMatrix[row, col];
                    // Opponent score.
                    else if (boxes[row, col] != (int)EBoxType.free)
                        opponentScore += evalMatrix[row, col];
                }
            }

            // Calculates the number of avaliable moves.
            // ChangeBox(col, row, currentType == EBoxType.white);
            // ChangeBox(col, row, currentType != EBoxType.white);

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
