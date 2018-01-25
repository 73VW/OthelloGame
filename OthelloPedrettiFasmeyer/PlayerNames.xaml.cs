using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace OthelloPedrettiFasmeyer
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class PlayerNames : Window
    {
        public PlayerNames()
        {
            InitializeComponent();
            WhiteName.Focus();
            WhiteName.SelectionStart = 0;
            WhiteName.SelectionLength = WhiteName.Text.Length;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if(WhiteName.Text!="" && BlackName.Text != "")
            {
                this.Close();
            }
        }

        private void WhiteName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BlackName.Focus();
                BlackName.SelectionStart = 0;
                BlackName.SelectionLength = BlackName.Text.Length;
            }
        }

        private void BlackName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (WhiteName.Text != "" && BlackName.Text != "")
                {
                    this.Close();
                }
            }
        }
    }
}
