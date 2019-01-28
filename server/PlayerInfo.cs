﻿using System;
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
            get { return position; }
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
    }
    
}
