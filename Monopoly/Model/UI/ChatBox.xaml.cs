using Monopoly.Controller;
using Monopoly.Core;
using Monopoly.Model.Board;
using Monopoly.Model.Case;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour ChatBox.xaml
    /// </summary>
    public partial class ChatBox : UserControl
    {

        private Connection conn = Connection.GetConnection;

        public delegate void UpdateTextCallback(string message);


        public ChatBox()
        {
            InitializeComponent();
            this.button.Click += new RoutedEventHandler(this.SendMessage);

        }






        void SendMessage(object sender, System.EventArgs e)
        {

            try
            {
                if (textBox.Text != "")
                {
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(conn.GetSequence() + PlayerManager.SearchPlayer(0).NamePlayer + textBox.Text + "\r\n");
                    int DtSent = conn.ClientSocket.Send(msg, msg.Length, SocketFlags.None);

                    if (DtSent == 0)
                    {
                        MessageBox.Show("Aucune donnée n'a été envoyée");
                    }
                    textBox.Clear();
                }


            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

        }



        public void UpdateText(string message)
        {
           
           chatBody.AppendText(Environment.NewLine + message);
        }
        void SendMsg(string message)
        {

            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            int DtSent = conn.ClientSocket.Send(msg, msg.Length, SocketFlags.None);

            if (DtSent == 0)
            {
                MessageBox.Show("Aucune donnée n'a été envoyée");
            }
        }

        private void launchCommand(string command)
        {
            command = command.TrimStart('/');
            switch (command)
            {
                case "moove":
                    PlayerManager.MoovePlayer(Board.Board.GetBoard, 0);
                    PlayerManager.SearchPlayer(0).AddAmount(999);
                    BaseCase disCase = Board.Board.GetBoard.CasesList[PlayerManager.SearchPlayer(0).Position];
                    UpdateText("vous bougez");
                    break;
                default:
                    break;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
