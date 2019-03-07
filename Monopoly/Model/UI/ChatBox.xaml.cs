using Monopoly.Controller;
using Monopoly.Core;
using Monopoly.Model.Board;
using Monopoly.Model.Case;
using Newtonsoft.Json;
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
                if (textBox.Text != "" && textBox.Text.StartsWith("/"))
                {
                    Packet p = new Packet();
                    p.Type = textBox.Text.TrimStart('/');
                    p.Content = JsonConvert.SerializeObject(GameManager.playersList[PlayerManager.CurrentPlayerName.Trim('0')], Formatting.Indented);

                    string message = JsonConvert.SerializeObject(p, Formatting.Indented);
                    Console.WriteLine(message);
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(conn.GetSequence() + PlayerManager.CurrentPlayerName + message);
                    int DtSent = conn.ClientSocket.Send(msg, msg.Length, SocketFlags.None);

                    if (DtSent == 0)
                    {
                        MessageBox.Show("Aucune donnée n'a été envoyée");
                    }
                    textBox.Clear();
                }
                else
                {
                    Packet p = new Packet();
                    p.Type = "message";
                    p.ChatMessage = textBox.Text;
                    string message = JsonConvert.SerializeObject(p, Formatting.Indented);
                    Console.WriteLine(message);
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(conn.GetSequence() + PlayerManager.CurrentPlayerName + message);
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


        private void button_Click(object sender, RoutedEventArgs e)
        {

            PlayerInterface playerHud = (PlayerInterface)GameManager.controls["playerHud"];

        }
    }
}
