using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OthelloIAG5;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Activities.Statements;
using System.Activities;
using System.Windows.Media.Animation;

namespace OthelloPedrettiFasmeyer
{
    class OthelloGame
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
        private DispatcherTimer dispatcherTimer;
        private bool endGame;
        private MainWindow mainWindow;
        private BrushAnimation animation;

        public OthelloGame(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            UnitTest test = new UnitTest();

            //Start a timer to count elapsed time playing
            dispatcherTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1) //in Hour, Minutes, Second.
            };
            dispatcherTimer.Tick += DtClockTime_Tick;

            InitGame();

            LoadPictures();

            PlayersBinding();

            LaunchGame();
        }
		/// <summary>
        /// Init game.
        /// </summary>
        private void InitGame()
        {
            mainWindow.gameGrid.Background = Brushes.Black;
            animation = new BrushAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
            };
            mainWindow.ResetButton.Click += new RoutedEventHandler(ResetGame);
            mainWindow.SaveButton.Click += new RoutedEventHandler(SaveGame);
            mainWindow.LoadButton.Click += new RoutedEventHandler(LoadGame);
            mainWindow.UndoButton.Click += new RoutedEventHandler(Undo);
            mainWindow.RedoButton.Click += new RoutedEventHandler(Redo);
            //Create the board
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                mainWindow.gameGrid.ColumnDefinitions.Add(columnDefinition);
                mainWindow.gameGrid.RowDefinitions.Add(rowDefinition);
            }

            FillBoard();


        }

        /// <summary>
        /// Fill the UI with buttons for each boxes of the board.
        /// </summary>
        private void FillBoard()
        {
            Style style = mainWindow.FindResource("MyButtonStyle") as Style;

            //Fill each case of the board
            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Board.BOARD_SIZE; j++)
                {
                    Button b = new Button
                    {
                        Style = style
                    };
                    b.Click += new RoutedEventHandler(Button_Click);
                    b.MouseEnter += new MouseEventHandler(MouseEnter);
                    b.MouseLeave += new MouseEventHandler(MouseLeave);
                    b.BorderBrush = null;
                    b.Background = System.Windows.Media.Brushes.Black;
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                    mainWindow.gameGrid.Children.Add(b);
                }
            }
        }

        /// <summary>
        /// Using the command pattern to redo a move.
        /// </summary>
        private void Redo(object sender, RoutedEventArgs e)
        {
            whiteTurn = board.Redo();
            UpdateGrid();
            ToggleTurnUi();
            ShowAvailableCells();
        }

        /// <summary>
        /// Using the command pattern to undo a move.
        /// </summary>
        private void Undo(object sender, RoutedEventArgs e)
        {
            board.Undo();
            whiteTurn = board.IsWhiteTurn;
            UpdateGrid();
            ToggleTurnUi();
            ShowAvailableCells();
        }

        /// <summary>
        /// Load a saved game.
        /// </summary>
        private void LoadGame(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();

            Board loadedBoard = SaveManager.DeserializeObject<Board>();
            if (loadedBoard != null)
            {
                board = loadedBoard;
                blackPlayer.Timer = loadedBoard.TimerBlack;
                whitePlayer.Timer = loadedBoard.TimerWhite;
                mainWindow.WhitePlayerName.Text = board.WhitePlayerName;
                mainWindow.BlackPlayerName.Text = board.BlackPlayerName;
                whiteTurn = loadedBoard.IsWhiteTurn;
                UpdateGrid();
                ToggleTurnUi();
                ShowAvailableCells();
            }
            else
            {
                MessageBox.Show("Le fichier a été chargé mais son contenu n'est pas valide");
            }

            dispatcherTimer.Start();
        }
        /// <summary>
        /// Save the current game to be played later.
        /// </summary>
        private void SaveGame(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();

            board.WhitePlayerName = mainWindow.WhitePlayerName.Text;
            board.BlackPlayerName = mainWindow.BlackPlayerName.Text;
            board.IsWhiteTurn = whiteTurn;
            board.TimerBlack = blackPlayer.Timer;
            board.TimerWhite = whitePlayer.Timer;

            SaveManager.SerializeObject(this.board);

            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Reset the current game.
        /// </summary>
        private void ResetGame(object sender, RoutedEventArgs e)
        {
            mainWindow.gameGrid.Children.Clear();
            FillBoard();
            LaunchGame();
        }

        /// <summary>
        /// Load design elements.
        /// </summary>
        private void LoadPictures()
        {
            whiteBrush = CreateImageBrushFromFile("fig1.png");
            blackBrush = CreateImageBrushFromFile("fig2.png");

            mainWindow.BtnWhitePlayer.Background = whiteBrush;
            mainWindow.BtnBlackPlayer.Background = blackBrush;

            whiteBrushCanPlay = whiteBrush.Clone();
            whiteBrushCanPlay.Opacity = 0.3;
            blackBrushCanPlay = blackBrush.Clone();
            blackBrushCanPlay.Opacity = 0.3;
        }

        /// <summary>
        /// Binding values with the UI.
        /// </summary>
        private void PlayersBinding()
        {

            whitePlayer = new Player("white");
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

            mainWindow.ScoreWhite.SetBinding(TextBlock.TextProperty, whiteBinding);
            mainWindow.ScoreBlack.SetBinding(TextBlock.TextProperty, blackBinding);

            mainWindow.TimerWhite.SetBinding(TextBlock.TextProperty, whiteTimerBinding);
            mainWindow.TimerBlack.SetBinding(TextBlock.TextProperty, blackTimerBinding);
        }

        /// <summary>
        /// Init a new game and start it.
        /// </summary>
        private void LaunchGame()
        {

            whiteTurn = true;

            ToggleTurnUi();

            cantPlay = 0;
            nbEmptyCells = 0;

            board = new Board
            {
                IsWhiteTurn = whiteTurn
            };
            board.ResetBoxes();

            whitePlayer.Reset();
            mainWindow.TimerWhite.Text = TimeSpan.FromSeconds(whitePlayer.Timer).ToString(@"m'm 's's'");
            blackPlayer.Reset();
            mainWindow.TimerBlack.Text = TimeSpan.FromSeconds(blackPlayer.Timer).ToString(@"m'm 's's'");

            UpdateGrid();

            ShowAvailableCells();

            endGame = false;

            dispatcherTimer.Start();
        }

        /// <summary>
        /// Update in-game timer.
        /// </summary>
        private void DtClockTime_Tick(object sender, EventArgs e)
        {
            if (whiteTurn)
            {
                this.whitePlayer.Timer += 1;
                mainWindow.TimerWhite.Text = TimeSpan.FromSeconds(whitePlayer.Timer).ToString(@"m'm 's's'");
            }
            else
            {
                this.blackPlayer.Timer += 1;
                mainWindow.TimerBlack.Text = TimeSpan.FromSeconds(blackPlayer.Timer).ToString(@"m'm 's's'");
            }
        }

        /// <summary>
        /// Change the opacity of the player icon depending on who's turn it is. Highlight current player's icon.
        /// </summary>
        private void ToggleTurnUi()
        {
            if (whiteTurn)
            {
                mainWindow.BtnWhitePlayer.Opacity = 1;
                mainWindow.BtnBlackPlayer.Opacity = 0.25;
            }
            else
            {
                mainWindow.BtnWhitePlayer.Opacity = 0.25;
                mainWindow.BtnBlackPlayer.Opacity = 1;
            }
        }

        /// <summary>
        /// Load an ImageBrush from a file.
        /// </summary>
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

        /// <summary>
        /// Foreach boxes(as buttons) on the mainWindows game grid, count the number of empty cells and
        /// update animations from/to.
        /// </summary>
        private void UpdateGrid()
        {
            nbEmptyCells = 0;
            foreach (Button button in mainWindow.gameGrid.Children)
            {
                var col = Grid.GetColumn(button);
                var row = Grid.GetRow(button);
                if (board[col, row] == (int)EBoxType.black)
                {
                    if (!button.Background.Equals(blackBrush))
                    {
                        try
                        {
                            animation.From = button.Background;
                        animation.To = blackBrush;
                        Storyboard.SetTarget(animation, button);
                        Storyboard.SetTargetProperty(animation, new PropertyPath("Background"));

                        var sb = new Storyboard();
                        sb.Children.Add(animation);
                        sb.Begin();
                        }
                        catch { }
                    }
                    button.Background = blackBrush;
                }
                else if (board[col, row] == (int)EBoxType.white)
                {
                    if (!button.Background.Equals(whiteBrush))
                    {
                        try
                        {
                            animation.From = button.Background;
                            animation.To = whiteBrush;
                            Storyboard.SetTarget(animation, button);
                            Storyboard.SetTargetProperty(animation, new PropertyPath("Background"));

                            var sb = new Storyboard();
                            sb.Children.Add(animation);
                            sb.Begin();
                        }
                        catch { }
                    }
                    button.Background = whiteBrush;
                }
                else
                {
                    button.Background = Brushes.Black;
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
            whitePlayer.Score = board.GetWhiteScore();
            blackPlayer.Score = board.GetBlackScore();
            Brush brush;
            if (this.whitePlayer.Score > this.blackPlayer.Score)
            {
                brush = whiteBrushCanPlay.Clone();
                brush.Opacity = 0.15;
            }
            else if (this.whitePlayer.Score < this.blackPlayer.Score)
            {
                brush = blackBrushCanPlay.Clone();
                brush.Opacity = 0.15;
            }
            else
            {
                brush = System.Windows.Media.Brushes.Black;
            }
            if (!mainWindow.gameGrid.Background.Equals(brush))
            {
                try
                {
                    animation.From = mainWindow.gameGrid.Background;
                    animation.To = brush;
                    Storyboard.SetTarget(animation, mainWindow.gameGrid);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("Background"));

                    var sb = new Storyboard();
                    sb.Children.Add(animation);
                    sb.Begin();
                }
                catch { }
            }
        }

        /// <summary>
        /// Check if there is a winner.
        /// </summary>
        private void IsWinner()
        {
            if (!ShowAvailableCells() && nbEmptyCells != 0)
            {
                MessageBox.Show("Plus de coups possibles pour ce joueur");
                cantPlay += 1;
                whiteTurn = !whiteTurn;
                ShowAvailableCells();
            }
            else
            {
                cantPlay = 0;
            }

            if (cantPlay >= 2 || nbEmptyCells == 0)
            {
                dispatcherTimer.Stop();
                endGame = true;
                ShowWinner();
            }
        }

        /// <summary>
        /// Count the pawns to display the winner.
        /// </summary>
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
            foreach (Button button in mainWindow.gameGrid.Children)
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

        new private void MouseEnter(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), whiteTurn))
            {
                button.Background = (whiteTurn) ? whiteBrush : blackBrush;
            }
        }

        new private void MouseLeave(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), whiteTurn))
            {
                button.Background = (whiteTurn) ? whiteBrushCanPlay : blackBrushCanPlay;
            }
        }
    }
}
