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

namespace OthelloPedrettiFasmeyer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            PlayerNames playerNames;
            string whiteName = "White player";
            string blackName = "Black player";
            do
            {
                playerNames = new PlayerNames();
                playerNames.ShowDialog();
                whiteName = playerNames.WhiteName.Text;
                blackName = playerNames.BlackName.Text;
            } while (playerNames.WhiteName.Text == "" || playerNames.BlackName.Text == "");
            InitializeComponent();
            MinWidth = 800;
            MinHeight = 600;
            WhitePlayerName.Text = whiteName;
            BlackPlayerName.Text = blackName;

            new OthelloGame(this);
        }
    }
}
