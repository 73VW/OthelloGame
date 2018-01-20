using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Input;
using System;
using System.Windows.Data;
using System.Windows.Threading;
using OthelloIAG5;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board board;
        private int nbEmptyCells;
        private int cantPlay; // CantPlay = 1 mean one player can't move, 2 mean both players can't move... So game ended
        private bool whiteTurn;
        private ImageBrush whiteBrush;
        private ImageBrush blackBrush;
        private ImageBrush whiteBrushCanPlay;
        private ImageBrush blackBrushCanPlay;
        private Player whitePlayer;
        private Player blackPlayer;
        private Binding whiteBinding;
        private Binding blackBinding;
        private Binding whiteTimerBinding;
        private Binding blackTimerBinding;
        private DispatcherTimer dtClockTime;
        private bool endGame;

        public MainWindow()
        {
            InitializeComponent();
            MinWidth = 800;
            MinHeight = 600;

            //Start a timer to count elapsed time playing
            dtClockTime = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1) //in Hour, Minutes, Second.
            };
            dtClockTime.Tick += DtClockTime_Tick;

            InitGame();

            LoadPictures();

            PlayersBinding(); //ok until here

            LaunchGame();
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
                    b.Click += new RoutedEventHandler(Button_Click);
                    b.MouseEnter += new MouseEventHandler(MouseEnter);
                    b.MouseLeave += new MouseEventHandler(MouseLeave);
                    b.BorderBrush = null;
                    b.Background = System.Windows.Media.Brushes.Transparent;
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                    gameGrid.Children.Add(b);
                }
            }


        }

        private void LoadPictures()
        {
            whiteBrush = CreateImageBrushFromFile("fig1.png");
            blackBrush = CreateImageBrushFromFile("fig2.png");

            BtnWhitePlayer.Background = whiteBrush;
            BtnBlackPlayer.Background = blackBrush;

            whiteBrushCanPlay = whiteBrush.Clone();
            whiteBrushCanPlay.Opacity = 0.3;
            blackBrushCanPlay = blackBrush.Clone();
            blackBrushCanPlay.Opacity = 0.3;
        }

        private void PlayersBinding()
        {
            whitePlayer = new Player("White");
            blackPlayer = new Player("Black");

            whiteBinding = new Binding("Score")
            {
                Source = whitePlayer
            };

            blackBinding = new Binding("Score")
            {
                Source = blackPlayer
            };

            whiteTimerBinding = new Binding("Timer")
            {
                Source = whitePlayer
            };

            blackTimerBinding = new Binding("Timer")
            {
                Source = blackPlayer
            };

            ScoreWhite.SetBinding(TextBlock.TextProperty, whiteBinding);
            ScoreBlack.SetBinding(TextBlock.TextProperty, blackBinding);

            TimerWhite.SetBinding(TextBlock.TextProperty, whiteTimerBinding);
            TimerBlack.SetBinding(TextBlock.TextProperty, blackTimerBinding);
        }

        private void LaunchGame()
        {
            whiteTurn = true;

            //update the ui toggle turn
            ToggleTurnUi();

            //reset count
            cantPlay = 0;
            nbEmptyCells = 0;

            board = new Board();
            board.ResetBoxes();

            whitePlayer.Reset();
            blackPlayer.Reset();

            UpdateGrid();

            ShowAvailableCells();

            endGame = false;

            dtClockTime.Start();
        }

        private void DtClockTime_Tick(object sender, EventArgs e)
        {
            if (whiteTurn)
            {
                this.whitePlayer.Timer += 1;
                TimerWhite.Text = TimeSpan.FromSeconds(this.whitePlayer.Timer).ToString(@"m'm 's's'");
            }
            else
            {
                this.blackPlayer.Timer += 1;
                TimerBlack.Text = TimeSpan.FromSeconds(this.blackPlayer.Timer).ToString(@"m'm 's's'");
            }
        }

        private void ToggleTurnUi()
        {
            if (whiteTurn)
            {
                BtnWhitePlayer.Opacity = 1;
                BtnBlackPlayer.Opacity = 0.25;
            }
            else
            {
                BtnWhitePlayer.Opacity = 0.25;
                BtnBlackPlayer.Opacity = 1;
            }
        }

        private ImageBrush CreateImageBrushFromFile(string filename)
        {
            string path = "images/" + filename;
            Uri resourceHeroe = new Uri(path, UriKind.Relative);

            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceHeroe);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);

            ImageBrush brush = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = temp
            };
            return brush;
        }

        private void UpdateGrid()
        {
            foreach (Button button in gameGrid.Children)
            {
                var col = Grid.GetColumn(button);
                var row = Grid.GetRow(button);
                if (board[col, row] == (int)EBoxType.black)
                    button.Background = blackBrush;
                else if (board[col, row] == (int)EBoxType.white)
                    button.Background = whiteBrush;
                else
                {
                    nbEmptyCells++;
                }
            }


            UpdateScore();
        }

        /// <summary>
        /// Update the score of both player and change the background
        /// </summary>
        private void UpdateScore()
        {
            this.whitePlayer.Score = this.board.GetWhiteScore();
            this.blackPlayer.Score = this.board.GetBlackScore();

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Colors.White)
            {
                Opacity = 1
            };
            if (this.whitePlayer.Score > this.blackPlayer.Score)
            {
                PanelBtnWhitePlayer.Background = brush;
                PanelBtnBlackPlayer.Background = System.Windows.Media.Brushes.Black;
            }
            else if (this.whitePlayer.Score < this.blackPlayer.Score)
            {
                PanelBtnWhitePlayer.Background = System.Windows.Media.Brushes.Black;
                PanelBtnBlackPlayer.Background = brush;
            }
            else
            {
                PanelBtnWhitePlayer.Background = System.Windows.Media.Brushes.Black;
                PanelBtnBlackPlayer.Background = System.Windows.Media.Brushes.Black;
            }
        }

        /*
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
        */

        private void IsWinner()
        {
            if (!ShowAvailableCells() && nbEmptyCells != 0)
            {
                MessageBox.Show("Plus de coups possibles pour ce joueur");
                cantPlay += 1;
                whiteTurn = !whiteTurn;
                ShowAvailableCells();
            }
            else {
                cantPlay = 0;
            }

            if (cantPlay >= 2 || nbEmptyCells == 0)
            {
                dtClockTime.Stop();
                endGame = true;
                ShowWinner();
            }
        }

        private void ShowWinner()
        {
            String message = "";
            if (whitePlayer.Score != blackPlayer.Score)
            {
                if (whitePlayer.Score > blackPlayer.Score) message += "Joueur blanc a ";
                else if (whitePlayer.Score < blackPlayer.Score) message += "Joueur noir a ";
                message += "gagné!";
            }
            else message += "Égalité!";
            MessageBox.Show(message);
            LaunchGame();
        }

        private bool ShowAvailableCells()
        {
            bool isPlayable = false;
            foreach (Button button in gameGrid.Children)
            {
                button.Content = String.Empty;
                int col = Grid.GetColumn(button);
                int row = Grid.GetRow(button);
                if (board.IsPlayable(col, row, whiteTurn))
                {
                    if (whiteTurn) { button.Background = whiteBrushCanPlay; }
                    else { button.Background = blackBrushCanPlay; }

                    isPlayable = true;
                }
                else if (board[col, row] == (int)EBoxType.free)
                {
                    button.Background = System.Windows.Media.Brushes.Transparent;
                }
            }
            return isPlayable;
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (endGame)
            {
                this.LaunchGame();
                return;
            }

            var col = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            if (board.PlayMove(col, row, whiteTurn)) UpdateGrid();
            else return;

            whiteTurn = !whiteTurn;
            IsWinner();

            ToggleTurnUi();
        }

        private void MouseEnter(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), whiteTurn))
            {
                button.Background = (whiteTurn) ? whiteBrush : blackBrush;
            }
        }

        private void MouseLeave(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), whiteTurn))
            {
                button.Background = (whiteTurn) ? whiteBrushCanPlay : blackBrushCanPlay;
            }
        }
    }
}
