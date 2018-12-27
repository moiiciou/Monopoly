using Monopoly.Controller;
using Monopoly.Model.Board;
using System.Windows;
using System.Windows.Controls;


namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour ChatBox.xaml
    /// </summary>
    public partial class ChatBox : UserControl
    {
        public ChatBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           if (textBox.Text.StartsWith("/"))
            {
                launchCommand(textBox.Text);
            }
        }

        private void launchCommand(string command)
        {
            command = command.TrimStart('/');
            switch (command)
            {
                case "moove":
                    textBlock.Text += "\n fonction moove lancé !";
                    PlayerManager.MoovePlayer(Board.Board.GetBoard, 0);
                    break;
                default:
                    textBlock.Text += "\n commande non trouvé !";
                    break;
            }
        }
    }
}
