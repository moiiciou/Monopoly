using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void optionButton_Click(object sender, RoutedEventArgs e)
        {
            var optionsForm = new optionsForm();
            optionsForm.Show();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            var GameWindow = new GameWindow();
            GameWindow.Show();
            this.WindowState = WindowState.Minimized;
        }
    }
}
