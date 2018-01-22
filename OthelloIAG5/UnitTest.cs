using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIAG5
{
    public class UnitTest
    {
        public UnitTest()
        {
            Console.WriteLine("Start unit test.");
            TestBoard();
            TestIA();
            TestState();
            Console.WriteLine("End unit test.");
        }

        private void TestState()
        {
            Console.WriteLine("Testing State class...");

            // Eval
            {
                Board board = new Board();
                State state = new State(board.Boxes, EBoxType.black);
                Debug.Assert(state.Eval() == 0);
            }
            {
                Board board = new Board();
                int[,] game = board.Boxes;
                game[0, 0] = 1;
                game[1, 0] = 1;
                game[1, 1] = 1;
                game[0, 1] = 1;

                State state = new State(game, EBoxType.black);
                state.Eval();

                int[,] evalBoard = state.EvalMatrix;
                Console.WriteLine("Displaying final evalMatrix");
                state.Print();
                Debug.Assert(evalBoard[0, 1] == 120);
                Debug.Assert(evalBoard[1, 0] == 120);
                Debug.Assert(evalBoard[1, 1] != 120);
            }
        }

        private void TestIA()
        {
            Console.WriteLine("Testing IA class...");
            {
                IA ai = new IA();
                // ...
            }

            // No test done yet.
        }

        private void TestBoard()
        {
            Console.WriteLine("Testing Board class...");

            {
                Board board = new Board();
                board.Boxes = new int[8, 8]
                {
                    {   0,   0,   0,   0,   0,   0,   0,  -1},
                    {   0,   0,   0,   0,   0,   0,  -1,   1},
                    {   0,   0,   0,   0,   0,   0,   1,  -1},
                    {   0,   0,   0,   0,   0,   1,   0,  -1},
                    {   0,   1,   0,   0,   0,   0,   0,   0},
                    {   0,   0,   0,   0,   0,   0,   0,  -1},
                    {   0,   0,   0,   0,   0,   0,   0,  -1},
                    {   0,   0,   0,   0,   0,   0,   0,  -1},
                };

                Tuple<int,int> move = board.GetNextMove(board.Boxes, 5, true);
                board.Print();
                Console.WriteLine("{0} {1}", move.Item1, move.Item2);
                int[,] result = board.Boxes;
                result[3, 7] = 1;
                board.Boxes = result;
                board.Print();
            }
            {
                Board board = new Board();
                Console.WriteLine("Initial state:");
                board.Print();
            }

            // IsPlayable.
            {
                Board board = new Board();
                Debug.Assert(board.IsPlayable(3, 2, false) == true);
            }
            {
                Board board = new Board();
                Debug.Assert(board.IsPlayable(4, 2, false) == false);
            }

            // GetScores.
            {
                Board board = new Board();
                Debug.Assert(board.GetBlackScore() == 2);
                Debug.Assert(board.GetWhiteScore() == 2);
            }

            // GetNextMove.
            {
                Board board = new Board();
                Tuple<int, int> move = board.GetNextMove(board.GetBoard(), 4, false);
                Debug.Assert(board.IsPlayable(move.Item1, move.Item2, false));
            }

            // PlayMove.
            {
                Board board = new Board();
                Tuple<int, int> move = board.GetNextMove(board.GetBoard(), 4, false);
                bool result = board.PlayMove(move.Item1, move.Item2, false);
                Debug.Assert(result);

                Console.WriteLine("Final state:");
                board.Print();
            }
        }
    }
}
