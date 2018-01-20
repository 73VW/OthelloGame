﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIAG5
{
    public class LegalMove
    {
        protected int[,] boxes;

        //because this function is used in two classes I decided to put it in a separate class.
        public bool ChangeBox(int col, int row, bool isWhite, bool apply = false)
        {
            int box = boxes[col, row];
            int currentTile = -1;
            bool isValid = false;
            bool isLegal = false;

            //
            if (box != (int)EBoxType.free) return false;

            // On recherche dans chaque direction
            // x et y represente les 8 directions dans lequels nous allons chercher
            //le cas 0, 0 n'est pas intéressant dans notre cas
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if ((x != 0 || y != 0) && InBoardArea(col + x, row + y))
                    {
                        currentTile = this.boxes[col + x, row + y];

                        if ((isWhite && currentTile == (int)EBoxType.black) ||
                            (!isWhite && currentTile == (int)EBoxType.white))
                        {
                            int posX = col;
                            int posY = row;

                            //on a trouvé un coup potentiel. 
                            //Donc on va exploité la direction pour voir si on trouve une pièce de la même couleur
                            while (!isValid)
                            {
                                posX += x;
                                posY += y;

                                if (InBoardArea(posX, posY))
                                {
                                    currentTile = this.boxes[posX, posY];
                                    if (currentTile == (int)EBoxType.free)
                                    {
                                        isValid = true;
                                    }
                                    else if ((isWhite && currentTile == (int)EBoxType.white) || (!isWhite && currentTile == (int)EBoxType.black))
                                    {
                                        if (apply)
                                        {
                                            int color = (isWhite) ? (int)EBoxType.white : (int)EBoxType.black;
                                            do
                                            {
                                                posX -= x;
                                                posY -= y;
                                                this.boxes[posX, posY] = color;

                                            } while (posX != col || posY != row);
                                            this.boxes[col, row] = color;
                                            isValid = true;
                                            isLegal = true;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }

                                }
                                else
                                {
                                    isValid = true;
                                }

                            }
                            isValid = false;
                        }
                    }
                }
            }
            return isLegal;
        }

        public static bool InBoardArea(int col, int row)
        {
            return row >= 0 && row <= 7 && col >= 0 && col <= 7;
        }
    }
}