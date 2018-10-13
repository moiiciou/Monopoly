using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Monopoly.Model.Case
{
    class BoardCase : Button
    {
  
        public  int [] Coordinate { get; set; }
        public int[] ImageSize { get; set; }
        private string SkinPath { get; set; }

        public BoardCase(string name, int[] coordinate , string skinPath ) : base()

        {
            Name = name;
            Coordinate = coordinate;
            SkinPath = skinPath;
            Content = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(skinPath)),
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = System.Windows.Media.Stretch.Fill
        };



        }




    }

}