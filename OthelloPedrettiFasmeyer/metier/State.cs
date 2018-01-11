using OthelloPedrettiFasmeyer.metier;
using System;
using System.Collections.Generic;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>Specific methods for the AlphaBeta algorithm.</summary>
    class State
    {
        private Board board;

        public State(Board gameBoard)
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
            // TO DO.
            return null;
        }

        public State Apply(Operation op)
        {
            Board newBoard = new Board(board);
            newBoard.Apply(op);
            return new State(newBoard);
        }
    }
}
