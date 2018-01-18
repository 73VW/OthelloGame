using OthelloPedrettiFasmeyer.metier;
using System;
using System.Collections.Generic;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>Specific methods for the AlphaBeta algorithm.</summary>
    class State
    {
        private BoardB board;

        public State(BoardB gameBoard)
        {
            board = gameBoard;
        }

        public int Eval()
        {
            // For now, same method as board.
            return board.Eval();
        }

        /// <summary>A list of all legal moves for the current player.</summary>
        public List<Operation> Ops()
        {
            return board.Ops();
        }

        public State Apply(Operation op)
        {
            BoardB newBoard = new BoardB(board);
            newBoard.Apply(op);
            return new State(newBoard);
        }
    }
}
