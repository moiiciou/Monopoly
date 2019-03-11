using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;
using Monopoly.Model.UI;
using Monopoly.Controller;
using System.Threading.Tasks;
using System.Threading;

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
            GameManager newGame = new GameManager(root);
      

        }


 
        


    }
}



