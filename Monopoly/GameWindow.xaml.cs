using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            this.Width = Settings.Default.Width;
            this.Height = Settings.Default.Height;
            Board board = new Board();
            root.Children.Add(board);

        }

    }
}

  

