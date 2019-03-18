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
using Menu = Monopoly.Model.UI.Menu;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Menu menu;

        public GameWindow()
        {
            InitializeComponent();
            GameManager newGame = new GameManager(root);
            menu = new Menu();
            menu.Visibility = Visibility.Hidden;
            Grid.SetColumn(menu, 0);
            Grid.SetRow(menu, 0);
            root.Children.Add(menu);

        }

        private void GameWindow1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape & menu != null)
            {
  

                if (menu.Visibility == Visibility.Hidden)
                    menu.Visibility = Visibility.Visible;
                else
                    menu.Visibility = Visibility.Hidden;


            }
        }
    }
}



