﻿using Monopoly.Core;
using Monopoly.Model;
using Monopoly.Model.Board;
using Monopoly.Model.UI;
using Newtonsoft.Json;
using server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Monopoly.Controller
{
    public static class NetworkManager
    {



    }


    public class Connection
    {
        private long sequence;
        public Socket ClientSocket { get; private set; }

        public static string IpServeur { get; set; } = Core.Tools.GetLocalIPAddress();

        private static readonly Lazy<Connection> lazy = new Lazy<Connection>(() => new Connection());

        public static Connection GetConnection { get { return lazy.Value; } }


        public Thread DataReceived = null;


        protected Connection()
        {
            sequence = 0;

            IPAddress ip = IPAddress.Parse(IpServeur);
            IPEndPoint ipEnd = new IPEndPoint(ip, 8000);
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ClientSocket.Connect(ipEnd);
                if (ClientSocket.Connected)
                {
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(GetSequence() + PlayerManager.CurrentPlayerName);
                    int DtSent = ClientSocket.Send(msg, msg.Length, SocketFlags.None);

                }


            }
            catch (SocketException E)
            {
                System.Windows.MessageBox.Show("Connection" + E.Message);
            }
            try
            {
                DataReceived = new Thread(new ThreadStart(CheckData));
                DataReceived.Start();
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show("Démarrage Thread" + E.Message);
            }

        }

        public static void SendMsg(string message)
        {
            
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            int DtSent = Connection.GetConnection.ClientSocket.Send(msg, msg.Length, SocketFlags.None);

            if (DtSent == 0)
            {
                System.Windows.MessageBox.Show("Aucune donnée n'a été envoyée");
            }
        }

        public string GetSequence()
        {
            sequence++;
            string msgSeq = Convert.ToString(sequence);
            char pad = Convert.ToChar("0");
            msgSeq = msgSeq.PadLeft(6, pad);
            return msgSeq;
        }



        public void CheckData()
        {
            try
            {
                while (true)
                {

                    if (ClientSocket.Connected)
                    {
                        if (ClientSocket.Poll(10, SelectMode.SelectRead) && ClientSocket.Available == 0)
                        {
                            System.Windows.MessageBox.Show("La connexion au serveur est interrompue. Essayez avec un autre pseudo");
                            Thread.CurrentThread.Abort();
                        }
                        if (ClientSocket.Available > 0)
                        {
                            string messageReceived = null;

                            while (ClientSocket.Available > 0)
                            {
                                try
                                {

                                    byte[] msg = new Byte[ClientSocket.Available];
                                    ClientSocket.Receive(msg, 0, ClientSocket.Available, SocketFlags.None);
                                    messageReceived = System.Text.Encoding.UTF8.GetString(msg).Trim();
                                    Console.WriteLine(messageReceived);
                                    string json = messageReceived;
                                    Packet p = JsonConvert.DeserializeObject<Packet>(json);

                                    if (p.Type == "newPlayer")
                                    {

                                        Dictionary<string, server.PlayerInfo> playerList = JsonConvert.DeserializeObject<Dictionary<string, server.PlayerInfo>>(p.Content);

                                            foreach (var player in playerList)
                                            {
                                                if (!GameManager.playersList.Any( x => x.Pseudo == player.Key))
                                                {
                                                    Console.WriteLine("Player Current Name :" + PlayerManager.CurrentPlayerName);
                                                    Console.WriteLine("Player Pseudo  :" + player.Value.Pseudo);

                                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { PlayerManager.CreatePlayer(Board.GetBoard, player.Value.Pseudo, player.Value.Balance, player.Value.Position); }));
                                                    PlayerInterface playerHudPanel = (PlayerInterface)GameManager.controls["playerHud"];
                                                    playerHudPanel.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.AddNewPlayerCallback(playerHudPanel.AddNewPlayer), player.Value);


                                                    GameManager.playersList.Add(player.Value);

                                                }

                                            }






                                    }

                                    if (p.Type == "updatePlayerPosition")
                                    {
                                        PlayerInfo player = JsonConvert.DeserializeObject<PlayerInfo>(p.Content);
                                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { PlayerManager.MoovePlayer(Board.GetBoard, player.Pseudo, player.Position); }));

                                    }

                                    PlayerInterface playerHud = (PlayerInterface)GameManager.controls["playerHud"];
                                    playerHud.chatBox.Dispatcher.Invoke(new ChatBox.UpdateTextCallback(playerHud.chatBox.UpdateText), p.ChatMessage);
                                    playerHud.pseudo_label.Dispatcher.Invoke(new PlayerInterface.UpdatePseudoCallback(playerHud.UpdatePseudo), PlayerManager.CurrentPlayerName);



                                }
                                catch (SocketException E)
                                {
                                    System.Windows.MessageBox.Show("CheckData read" + E.Message);
                                }

                            }

                        }
                    }
                    Thread.Sleep(10);
                }

            }
            catch
            {
               // Thread.ResetAbort();
            }
        }
    }

}
