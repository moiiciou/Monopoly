﻿using System.ComponentModel;
using System.IO;

namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour ChanceCase.xaml
    /// </summary>
    public partial class ChanceCase : BaseCase
    {
        private string ImageTemplate { get; set; }

        public ChanceCase(string text, string skinPath, int angle, int[] position)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, TextLabel = text, Rotation = angle };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", TextLabel = text, Rotation = angle };

            }

            Position = position;
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
            public string ChanceName { get; set; }
            public int Rotation { get; set; }
        }
    }
}
