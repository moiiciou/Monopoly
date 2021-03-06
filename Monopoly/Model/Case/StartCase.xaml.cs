﻿using System.ComponentModel;
using System.IO;
using System.Windows.Controls;


namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour StartCase.xaml
    /// </summary>
    public partial class StartCase : BaseCase
    {
        public int Amount;

        public StartCase(string text, int amount, string skinPath, int[] position)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, TextLabel = text };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", TextLabel = text };

            }

            Amount = amount;
            this.Position = position;
        }

        public class CaseInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

            public string ImageTemplate { get; set; }
            public string TextLabel { get; set; }


        }
    }
}
