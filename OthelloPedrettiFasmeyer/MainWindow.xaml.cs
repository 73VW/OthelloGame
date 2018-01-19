using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using OthelloPedrettiFasmeyer.metier;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board board;
        private int nbEmptyCells;
        private bool whiteTurn;
        private ImageBrush whiteBrush;
        private ImageBrush blackBrush;

        public MainWindow()
        {
            InitializeComponent();
            MinWidth = 800;
            MinHeight = 600;

            InitGame();

            LoadPictures();
        }

        private void LoadPictures()
        {
            whiteBrush = BrushCreator("fig1.png");
            blackBrush = BrushCreator("fig2.png");
            BtnWhitePlayer.Background = whiteBrush;
            BtnBlackPlayer.Background = blackBrush;

            Button b = (Button)gameGrid.Children[0];
            b.Background = whiteBrush;
            b = (Button)gameGrid.Children[1];
            b.Background = blackBrush;
        }

        private ImageBrush BrushCreator(string filename)
        {
            string path = "images/" + filename;
            Uri resourceHeroe = new Uri(path, UriKind.Relative);

            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceHeroe);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);

            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.UniformToFill;
            brush.ImageSource = temp;
            return brush;
        }

        private void InitGame()
        {
            //Create the board
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                gameGrid.ColumnDefinitions.Add(columnDefinition);
                gameGrid.RowDefinitions.Add(rowDefinition);
            }
            Style style = this.FindResource("MyButtonStyle") as Style;

            //Fill each case of the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button b = new Button
                    {
                        Style = style
                    };
                    b.Click += new RoutedEventHandler(button_Click);
                    b.BorderBrush = null;
                    b.Background = System.Windows.Media.Brushes.Transparent;
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                    gameGrid.Children.Add(b);
                }
            }

            this.board = new Board();

            this.nbEmptyCells = 0;

            this.whiteTurn = true;

            //updateGrid();


        }
        /*
        private void updateGrid()
        {
            foreach (Button button in gameGrid.Children)
            {
                var col = Grid.GetColumn(button);
                var row = Grid.GetRow(button);
                if (this.board[col, row] == (int)EColorType.black)
                    button.Background = new SolidColorBrush(Colors.Black);
                else if (this.board[col, row] == (int)EColorType.black)
                    button.Background = new SolidColorBrush(Colors.White);
                else
                {
                    button.Content = "";
                    nbEmptyCells++;
                }
            }
        }*/

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
    }
}
