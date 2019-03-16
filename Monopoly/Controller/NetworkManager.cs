﻿using Monopoly.Model;
using Monopoly.Model.Board;
using Monopoly.Model.Case;
using Monopoly.Model.UI;
using Newtonsoft.Json;
using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;


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



                                    if (p.Type == "updateGameData")
                                    {
                                        Console.WriteLine(p.Content);
                                        GameData gameData = JsonConvert.DeserializeObject <GameData> (p.Content);

                                        if(gameData.PlayerList != null)
                                        {

                                            foreach (PlayerInfo player in gameData.PlayerList)
                                            {

                                                if (!GameManager.MonopolyGameData.PlayerList.Any(pl => pl.Pseudo == player.Pseudo))
                                                {
                                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { PlayerManager.CreatePlayer(Board.GetBoard, player.Pseudo, player.Balance, player.Position); }));
                                                    PlayerInterface playerHudPanel = (PlayerInterface)GameManager.controls["playerHud"];

                                                    playerHudPanel.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.AddNewPlayerCallback(playerHudPanel.AddNewPlayer), player);

                                                    GameManager.playersList.Add(player.Pseudo, player);

                                                }

                                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { PlayerManager.MoovePlayer(Board.GetBoard, player.Pseudo, player.Position); }));



                                                //Update l'affichage des infos du joueur
                                                PlayerInterface playerHudPanel2 = (PlayerInterface)GameManager.controls["playerHud"];
                                                playerHudPanel2.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.UpdateBalanceByPlayerInfoCallback(playerHudPanel2.UpdateBalanceByPlayerInfo), player);
                                                playerHudPanel2.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.UpdatePropertyCallback(playerHudPanel2.UpdateProperty), player);


                                                //Update les propriétés sur le board
                                                if (player.Estates != null)
                                                {
                                                    foreach (CaseInfo estate in player.Estates)
                                                    {
                                                        foreach (BaseCase baseCase in Board.GetBoard.CasesList)
                                                        {
                                                            if (baseCase is PropertyCase)
                                                            {
                                                                PropertyCase property = (PropertyCase)baseCase;

                                                                if (property.CaseInformation.Location == estate.Location)
                                                                {
                                                                    property.CaseInformation = estate;
                                                                }

                                                            }
                                                        }

                                                    }

                                                }

                                                if (player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
                                                    if (player.Position != PlayerManager.CurrentPlayerLastPosition)
                                                        PlayerManager.CurrentPlayerLastPosition = player.Position;
                                            }

                                        }


                                        GameManager.MonopolyGameData = gameData;


                                    }




                                    if (p.Type == "updatePlayers")
                                    {

                                        List<PlayerInfo> playerInfoList = JsonConvert.DeserializeObject<List<PlayerInfo>>(p.Content);

                                        foreach(PlayerInfo playerInfo in playerInfoList)
                                        {
                                            GameManager.playersList[playerInfo.Pseudo] = playerInfo;

                                            PlayerInterface playerHudPanel = (PlayerInterface)GameManager.controls["playerHud"];
                                            playerHudPanel.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.UpdateBalanceByPlayerInfoCallback(playerHudPanel.UpdateBalanceByPlayerInfo), playerInfo);
                                            playerHudPanel.PlayerPanel.Dispatcher.Invoke(new PlayerInterface.UpdatePropertyCallback(playerHudPanel.UpdateProperty), playerInfo);

                                            //Update les propriétés sur le board
                                            if (playerInfo.Estates != null)
                                            {
                                                foreach (CaseInfo estate in playerInfo.Estates)
                                                {
                                                    foreach (BaseCase baseCase in Board.GetBoard.CasesList)
                                                    {
                                                        if (baseCase is PropertyCase)
                                                        {
                                                            PropertyCase property = (PropertyCase)baseCase;

                                                            if (property.CaseInformation.Location == estate.Location)
                                                            {
                                                                property.CaseInformation = estate;
                                                            }

                                                        }
                                                    }

                                                }

                                            }


                                        }


                                    }


                                    PlayerInterface playerHud = (PlayerInterface)GameManager.controls["playerHud"];

                                    //Update le chat
                                    playerHud.chatBox.Dispatcher.Invoke(new ChatBox.UpdateTextCallback(playerHud.chatBox.UpdateText), p.ChatMessage);

                                    //Update le pseudo des joueurs
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
