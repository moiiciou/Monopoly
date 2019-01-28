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
using System.Diagnostics;

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

         /*   ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "server.exe";
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            Process proc = Process.Start(psi);
         */
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

        private void Multi_button_click(object sender, RoutedEventArgs e)
        {
            var lobbyWindow = new LobbyWindow();
            lobbyWindow.Show();
            this.WindowState = WindowState.Minimized;
        }
    }
}
