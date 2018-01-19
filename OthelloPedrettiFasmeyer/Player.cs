using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloPedrettiFasmeyer
{
    class Player : INotifyPropertyChanged
    {
        private string name;
        private int score;
        private int timer;

        public Player(String name, int score = 0, int timer=0)
        {
            this.name = name;
            this.score = score;
            this.timer = timer;
        }

        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged("score");
            }
        }

        public int Timer
        {
            get => timer;
            set
            {
                timer = value;
                OnPropertyChanged("timer");
            }
        }
        protected string Name { get => name; set => name = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
