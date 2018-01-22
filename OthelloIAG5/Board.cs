using System;
using OthelloCommand;
using System.Collections.Generic;
using System.Diagnostics;

namespace OthelloIAG5
{
    [Serializable]
    public class Board : LegalMove, IPlayable.IPlayable
    {
        private const String NAME = "Fasmeyer-Pedretti-SpaceOthello";
        public const int BOARD_SIZE = 8;
        private IA ia;
        private int timerWhite;
        private int timerBlack;
        private bool isWhiteTurn;

        //used for undo
        private List<Command> _commands;
        private int _current;

        public Board()
        {
            boxes = new int[BOARD_SIZE, BOARD_SIZE];
            ia = new IA(this);
            ResetBoxes();
            _commands = new List<Command>();
            _current = 0;
        }

        public new void Print()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    char display = '_';
                    if (boxes[j, i] == 1) display = 'b';
                    else if (boxes[j, i] == 0) display = 'w';
                    Console.Write("{0,2} ", display);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void ResetBoxes()
        {

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    boxes[i, j] = (int)EBoxType.free;
                }
            }
            boxes[3, 3] = (int)EBoxType.white;
            boxes[4, 3] = (int)EBoxType.black;
            boxes[3, 4] = (int)EBoxType.black;
            boxes[4, 4] = (int)EBoxType.white;
        }

        public int[,] Boxes
        {
            //use clone in order to have your own int[,]
            get => boxes.Clone() as int[,];
            set => boxes = (int[,])value.Clone();
        }
        public int TimerWhite { get => timerWhite; set => timerWhite = value; }
        public int TimerBlack { get => timerBlack; set => timerBlack = value; }
        public bool IsWhiteTurn { get => isWhiteTurn; set => isWhiteTurn = value; }

        public int this[int col, int row]
        {
            get => boxes[col, row];
            set => boxes[col, row] = (int)value;
        }

        public int[,] GetBoard()
        {
            return this.Boxes;
        }

        public string GetName()
        {
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            Boxes = game;
            return ia.GetNextMove(game, level, whiteTurn);
        }

        public int GetBlackScore()
        {
            return GetPlayerScore(EBoxType.black);
        }

        public int GetWhiteScore()
        {
            return GetPlayerScore(EBoxType.white);
        }

        private int GetPlayerScore(EBoxType player)
        {
            int score = 0;
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    if (this.Boxes[col, row] == (int)player)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return ChangeBox(column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            Command command = new FlipCommand(this, column, line, isWhite);
            bool res = command.Execute();
            _commands.Add(command);
            _current++;

            return res;
        }

        public void Undo(int levels = 1)
        {
            for(int i = 0; i < levels; i++)
            {
                if(_current>0)
                {
                    Command command = _commands[--_current] as Command;
                    isWhiteTurn = command.UnExecute();
                }
            }
        }

        public bool Redo(int levels = 1)
        {
            for (int i = 0; i < levels; i++)
            {
                if (_current < _commands.Count)
                {
                    Command command = _commands[_current++] as Command;
                    command.Execute();
                    isWhiteTurn = !isWhiteTurn;
                }
            }
            return isWhiteTurn;
        }
    }
}
