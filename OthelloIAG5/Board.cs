﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace OthelloIAG5
{

    public class Board : LegalMove, IPlayable.IPlayable
    {
        private const String NAME = "Fasmeyer-Pedretti-SpaceOthello";
        public const int BOARD_SIZE = 8;
        private IA ia;

        public Board()
        {
            boxes = new int[BOARD_SIZE, BOARD_SIZE];
            ia = new IA(this);
            ResetBoxes();
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
                    if (this.Boxes[row, col] == (int)player)
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
            return ChangeBox(column, line, isWhite, true);
        }
    }
}