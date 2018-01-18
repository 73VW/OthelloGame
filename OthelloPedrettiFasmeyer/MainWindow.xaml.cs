using OthelloPedrettiFasmeyer.metier;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestBoard();

        }

        /// <summary>
        /// Testing positive and negative cases of all BoardB methods.
        /// </summary>
        public void TestBoard()
        {
            BoardB board = new BoardB();
            Console.WriteLine(board.Eval());

            // IsPlayable
            {
                Operation op = new Operation(3, 2, 1);
                Debug.Assert(board.IsPlayable(op) == true);
            }
            {
                Operation op = new Operation(4, 2, 1);
                Debug.Assert(board.IsPlayable(op) == false);
            }

            // Apply
            {
                Operation op = new Operation(3, 2, 1);
                bool result = board.Apply(op);
                //board.Print();
                Debug.Assert(board.Boxes[3, 3] == 1);
                Debug.Assert(result == true);
            }
            {
                // White's turn. The move should be made by white (-1) not by black (1).
                Operation op = new Operation(2, 4, 1);
                bool result = board.Apply(op);
                board.Print();
                Debug.Assert(board.Boxes[2, 4] == 0);
                Debug.Assert(board.Boxes[3, 3] == 1);
                Debug.Assert(result == false);
            }
            Console.WriteLine("---------------------------------");
        }
    }
}
