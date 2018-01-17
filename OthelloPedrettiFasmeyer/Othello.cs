using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace OthelloPedrettiFasmeyer
{
    class Othello
    {
        private static Othello instance;
        private UIElementCollection children;

        public Othello() { }

        public static Othello Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new Othello();
                }
                return instance;
            }
        }

        public void InitGame(MainWindow mainWindow)
        {
            Debug.WriteLine("Start!");
            children = mainWindow.board.Children;
            /*Case child = (Case)children[BoardPositionTool(4,4)];
            child.SetColor(Color.FromRgb(255, 255, 255));
            child = (Case)children[BoardPositionTool(3, 3)];
            child.SetColor(Color.FromRgb(255, 255, 255));
            child = (Case)children[BoardPositionTool(3, 4)];
            child.SetColor(Color.FromRgb(0, 0, 0));
            child = (Case)children[BoardPositionTool(4, 3)];
            child.SetColor(Color.FromRgb(0, 0, 0));*/
        }

        /// <summary>
        /// Returns the index in list of element at position [i,j]
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        private int BoardPositionTool(int i, int j, int boardSize = 8)
        {
            return i * 8 + j;
        }
    }
}
