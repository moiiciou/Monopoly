using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;

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
            Board board = new Board();
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);



        }

    }
}

  

