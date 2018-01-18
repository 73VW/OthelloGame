using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace OthelloPedrettiFasmeyer.metier
{
    /// <summary>
    /// Decorator for the AI class. The course's program will only use a class named Board.
    /// </summary>
    class Board : IPlayable.IPlayable
    {
        private IA ia;

        public Board(IA ia)
        {
            this.ia = ia;
        }

        public int GetBlackScore()
        {
            return ia.GetBlackScore();
        }

        public int[,] GetBoard()
        {
            return ia.GetBoard();
        }

        public string GetName()
        {
            return ia.GetName();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            return ia.GetNextMove(game, level, whiteTurn);
        }

        public int GetWhiteScore()
        {
            return ia.GetWhiteScore();
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return ia.IsPlayable(column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return ia.PlayMove(column, line, isWhite);
        }
    }
}
