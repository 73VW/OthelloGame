<<<<<<< HEAD
﻿using OthelloPedrettiFasmeyer.metier;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
﻿using System;
>>>>>>> Begining ui
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

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

            Width = 800;
            Height = 600;
            MinWidth = Width;
            MinHeight = Height;

            updateGridSize();

            //Create the board
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                board.ColumnDefinitions.Add(columnDefinition);
                board.RowDefinitions.Add(rowDefinition);
            }

            Button b;
            //Fill each case of the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    b = new Button();
                    b.Click += new RoutedEventHandler(button_Click);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                    board.Children.Add(b);
                }
            }

            Othello.Instance.InitGame(this);
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

        protected void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //identify which button was clicked and perform necessary actions
            Debug.WriteLine("Click!");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateGridSize();
        }

        private void updateGridSize()
        {
            board.Width = Width / 2;
            board.Height = Height / 2;
            Thickness margin = Margin;
            margin.Left = margin.Right = Width / 4;
            margin.Top = margin.Bottom = Height / 4;
            board.Margin = margin;
        }
    }
}
