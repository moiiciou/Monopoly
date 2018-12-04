﻿using Monopoly.Model;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;


namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class PropertyCase : UserControl
    {
        private string ImageTemplate { get; set; }

        public PropertyCase(string location, int price, string skinPath, string color, int angle)
        {
            InitializeComponent();


            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, Location= location, Price = price.ToString()+"€", Color = color, Rotation = angle, FontSizeTitle= Tools.GetFontSize(), FontSizeContent = Tools.GetFontSize() };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", Location = location, Price = price.ToString()+"€", Color = color, Rotation = angle, FontSizeTitle = Tools.GetFontSize(), FontSizeContent = Tools.GetFontSize() };

            }

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
            public string Location { get; set; }
            public string Price { get; set; }
            public string Color { get; set; }
            public int Rotation { get; set; }
            public int FontSizeTitle { get; set; }
            public int FontSizeContent { get; set; }
            public int IdOwner { get; set; } = 0; //By default, owner is bank(IdOwner : 0)


        }

    }
}
