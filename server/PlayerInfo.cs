using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class PlayerInfo : INotifyPropertyChanged
    {
        private string pseudo;
        private int position;
        private int balance;
        public string ColorCode;
         
        public event PropertyChangedEventHandler PropertyChanged;

        public string Pseudo
        {
            get { return pseudo; }
            set
            {
                pseudo = value;
                OnPropertyChanged("Pseudo");
            }
        }

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        public int Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                OnPropertyChanged("Balance");
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            string res = " Nom du joueur : " + pseudo + "; Argent : " + Balance + "; Position sur le plateau : " + Position + "; Liste de cartes : { ";
           /* Console.WriteLine(this._idPlayer);
            for (int i = 0; i < Cards.Count - 1; i++)
            {
                res += Cards[i].ToString() + " , ";
            }
            if (Cards.Count > 0)
            {
                res += Cards[Cards.Count - 1].ToString() + " }";
            }*/
            return res;
        }
    }
    
}
