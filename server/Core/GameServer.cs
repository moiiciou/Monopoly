﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using server.Controleur;
using server.Core;
using server.Model;

namespace server
{
    public class GameServer : forwardToAll
    {
        Packet lastPacketSend = new Packet();
        ArrayList readList = new ArrayList(); //liste utilisée par socket.select 
        string msgString = null; //contiendra le message envoyé aux autres clients
        string msgDisconnected = null; //Notification connexion/déconnexion
        byte[] msg;//Message sous forme de bytes pour socket.send et socket.receive
        public bool useLogging = false; //booleen permettant de logger le processing dans un fichier log
        public bool readLock = false;//Flag aidant à la synchronisation
        private int initPosition = 0;
        public static int initBalance = 1500;
        private Packet response = new Packet();
        private ThemeParser tp = new ThemeParser("Ressources\\level.json");
        private List<string> avatar = new List<string>();
        private List<PlayerInfo> playerPret = new List<PlayerInfo>();
        private List<PlayerInfo> playerLost = new List<PlayerInfo>();
        private string oldPlayer;


        int nextIndComm = 0;
        int nextIndChance = 0;
        public int salaire = 200;
#pragma warning disable CS0414 // Le champ 'GameServer.gameOver' est assigné, mais sa valeur n'est jamais utilisée
        private bool gameOver = false;
#pragma warning restore CS0414 // Le champ 'GameServer.gameOver' est assigné, mais sa valeur n'est jamais utilisée

        public void Start()
        {

#pragma warning disable CS0618 // 'Dns.Resolve(string)' est obsolète : 'Resolve is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202'
            IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
#pragma warning restore CS0618 // 'Dns.Resolve(string)' est obsolète : 'Resolve is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202'
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            Console.WriteLine("IP=" + ipAddress.ToString());
            Socket CurrentClient = null;
            Socket ServerSocket = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream,
              ProtocolType.Tcp);

            avatar.Add("#06d694");
            avatar.Add("#089dd8");
            avatar.Add("#0ea02b");
            avatar.Add("#0f08d8");
            avatar.Add("#6408db");
            avatar.Add("#e50daf");
            avatar.Add("#e58612");
            avatar.Add("#ea0e0e");
            avatar.Add("#eaf213");
            avatar.Add("#ff865e");

            try
            {
                ServerSocket.Bind(new IPEndPoint(ipAddress, 8000));
                ServerSocket.Listen(10);
                //Démarrage du thread avant la première connexion client
                Thread getReadClients = new Thread(new ThreadStart(getRead));
                getReadClients.Start();

                //Thread d'update des informations

                //Démarrage du thread vérifiant l'état des connexions clientes
                Thread CheckConnectionThread = new Thread(new ThreadStart(CheckIfStillConnected));
                CheckConnectionThread.Start();


                //Boucle infinie
                while (true)
                {
                    Console.WriteLine("Attente d'une nouvelle connexion...");
                    CurrentClient = ServerSocket.Accept();
                    Console.WriteLine("Nouveau client:" + CurrentClient.GetHashCode());
                    acceptList.Add(CurrentClient);

                }
            }
            catch (SocketException E)
            {
                Console.WriteLine(E.Message);
            }

        }

        //Méthode permettant de générer du logging
        private void Logging(string message)
        {
            using (StreamWriter sw = File.AppendText("Server.log"))
            {
                sw.WriteLine(DateTime.Now + ": " + message);
            }
        }

        public static void AutoVente(PlayerInfo p, int rent, ref ThemeParser tp)
        {

            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);
            #region autovente
            List<PropertyInfo> propertiesFreeWithHouse = PlayerManager.searchPropertyUnMortgageWithHouse(player);
            List<PropertyInfo> propertyFree = PlayerManager.searchPropertyUnMortgage(player);
            List<CompanyInfo> companiesFree = PlayerManager.searchCompaniesUnMortgage(player);
            List<StationInfo> stationFree = PlayerManager.searchStationUnMortgage(player);

            List<PropertyInfo> propertyMortgage = PlayerManager.searchPropertyMortgage(player);
            List<CompanyInfo> companiesMortgage = PlayerManager.searchCompaniesMortgage(player);
            List<StationInfo> stationMortgage = PlayerManager.searchStationMortgage(player);



            while (player.Balance < rent && (stationMortgage.Count > 0 || companiesMortgage.Count > 0 || propertyMortgage.Count > 0 || stationFree.Count > 0 || companiesFree.Count > 0 || propertyFree.Count > 0 || propertiesFreeWithHouse.Count > 0)) // tant que le joueur possède du patrimoine immobilier
            {

                #region auto-hypothèque
                if (companiesFree.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchCompaniesUnMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        CompanyInfo c = PlayerManager.searchCompany(player, companiesFree[0].TextLabel);
                        if (c != null)
                            mortgage(ref player, ref c);
                        companiesFree = PlayerManager.searchCompaniesUnMortgage(player);
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                if (stationFree.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchStationUnMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        StationInfo c = PlayerManager.searchStation(player, stationFree[0].TextLabel);
                        if (c != null)
                            mortgage(ref player, ref c);
                        stationFree = PlayerManager.searchStationUnMortgage(player);
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                if (propertyFree.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchPropertyUnMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        PropertyInfo c = PlayerManager.searchProperty(player, propertyFree[0].TextLabel);
                        if (c != null)
                            mortgage(ref player, ref c);
                        propertyFree = PlayerManager.searchPropertyUnMortgage(player);
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                #endregion

                #region auto vente terrain hypothéqué

                if (player.Companies.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchCompaniesMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        CompanyInfo c = PlayerManager.searchCompany(player, companiesMortgage[0].TextLabel);
                        if (c != null)
                            sell(player, c, ref tp);
                        companiesMortgage = PlayerManager.searchCompaniesMortgage(player);

                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                if (player.Companies.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchStationMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        StationInfo c = PlayerManager.searchStation(player, stationMortgage[0].TextLabel);
                        if (c != null)
                            sell(player, c, ref tp);
                        stationMortgage = PlayerManager.searchStationMortgage(player);
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                if (propertyFree.Count > 0 && player.Balance < rent)
                {
                    while (PlayerManager.searchPropertyMortgage(player).Count > 0 && player.Balance < rent)
                    {
                        PropertyInfo c = PlayerManager.searchProperty(player, propertyMortgage[0].TextLabel);
                        if (c != null)
                            sell(player, c, ref tp);
                        propertyMortgage = PlayerManager.searchPropertyMortgage(player);
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }

                }
                #endregion

                #region autovente maison sur un terrain de couleur
                if (propertiesFreeWithHouse.Count > 0)
                {
                    List<PropertyInfo> color = tp.searchCasePropertyOfColor(propertiesFreeWithHouse[0].Color);
                    int it = 0;
                    while (player.Balance < rent && propertiesFreeWithHouse.Contains(color[it]))
                    {
                        PropertyInfo current = color[it];
                        if (canSellHouse(player, current, ref tp))
                        {

                            sellHouseOfColor(player, current, ref tp);
                        }
                        else
                        {
                            propertiesFreeWithHouse.Remove(current);
                        }
                        it++;
                        it %= color.Count;
                        player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
                    }
                }
                #endregion


                // on remet à jour toutes les listes
                propertiesFreeWithHouse = PlayerManager.searchPropertyUnMortgageWithHouse(player);
                propertyFree = PlayerManager.searchPropertyUnMortgage(player);
                companiesFree = PlayerManager.searchCompaniesUnMortgage(player);
                stationFree = PlayerManager.searchStationUnMortgage(player);

                propertyMortgage = PlayerManager.searchPropertyMortgage(player);
                companiesMortgage = PlayerManager.searchCompaniesMortgage(player);
                stationMortgage = PlayerManager.searchStationMortgage(player);
                player = PlayerManager.GetPlayerByPseuso(player.Pseudo);
            }
            #endregion

        }





        //Méthode démarrant l'écriture du message reu par un client
        //vers tous les autres clients
        private void writeToAll()
        {
            base.sendMsg(msg);
        }
        private void infoToAll()
        {
            base.sendMsg(msgDisconnected);
        }


        #region utils pour auto vente
        private static void sell( PlayerInfo p,  StationInfo c, ref ThemeParser tp)
        {
            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);

            if (p.Pseudo == c.Owner)
            {
                p.Balance += c.Price;
                tp.searchCaseStation(c.TextLabel).Owner = null;
                if (c.isMortgaged)
                {
                    player.Balance += c.Price / 2;
                    tp.searchCaseStation(c.TextLabel).isMortgaged = false;
                }
                else
                {
                    player.Balance += c.Price;
                }

                player.Stations.Remove(c);
            }
        }

            private static void sell( PlayerInfo p, PropertyInfo c, ref ThemeParser tp)
        {
            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);

            if (p.Pseudo == c.Owner)
            {
                p.Balance += c.Price;
                tp.searchCaseProperty(c.TextLabel).Owner = null;
                if (c.isMortgaged)
                {
                    player.Balance += c.Price / 2;
                    tp.searchCaseProperty(c.TextLabel).isMortgaged = false;
                }
                else
                {
                    player.Balance += c.Price;
                }
                player.Properties.Remove(c);


            }
        }
        private static bool canSellHouse(PlayerInfo p, PropertyInfo prop, ref ThemeParser tp)
        {
            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);

            bool canSell = true;
            List<PropertyInfo> listColor = tp.searchCasePropertyOfColor(prop.Color);
            if (listColor.Count > 0)
            {
                if (prop.NumberOfHouse > 0)
                {
                    for (int it = 0; it < listColor.Count && canSell; it++)
                    {
                        PropertyInfo pi = listColor[it];
                        if (pi.NumberOfHouse > prop.NumberOfHouse || pi.Owner != prop.Owner || pi.Owner == null)
                        {
                            canSell = false;
                        }
                    }
                }
                else
                {
                    canSell = false;
                }
            }
            else
            {
                canSell = false;
            }

            return canSell;
        }

        private static void sellHouseOfColor(PlayerInfo p, PropertyInfo cell, ref ThemeParser tp)
        {

            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);

            if (cell.Location != null || cell.Location != "") // je check si la propriété possède un nom
            {
                PropertyInfo propToSellHouse = tp.searchCaseProperty(cell.Location);
                if (propToSellHouse.Owner == player.Pseudo)
                {
                    List<PropertyInfo> listColor = tp.searchCasePropertyOfColor(propToSellHouse.Color);
                    if (listColor.Count > 0)
                    {
                        if (canSellHouse(player,propToSellHouse,ref tp))
                        {
                            if (propToSellHouse.NumberOfHouse > 0 && !propToSellHouse.HasHostel) // je check si la propriété  possède une maison
                            {
                                propToSellHouse.NumberOfHouse--;
                                player.Balance += propToSellHouse.HouseCost / 2;

                            }
                            else if (propToSellHouse.NumberOfHouse == 4 && !propToSellHouse.HasHostel) // je check si la propriété  possède  un hotel 
                            {
                                player.Balance += propToSellHouse.HostelCost / 2;
                                propToSellHouse.HasHostel = false;

                            }
                        }
                    }
                }
            }
        }

        private static  void sell( PlayerInfo p, CompanyInfo c, ref ThemeParser tp)
        {
            PlayerInfo player = PlayerManager.GetPlayerByPseuso(p.Pseudo);
            
            if (p.Pseudo == c.Owner)
            {
                p.Balance += c.Price;
                tp.searchCaseCompany(c.TextLabel).Owner = null;
                if (c.isMortgaged)
                {
                    player.Balance += c.Price / 2;
                    tp.searchCaseCompany(c.TextLabel).isMortgaged = false;
                }
                else
                {
                    player.Balance += c.Price;
                }

                player.Companies.Remove(c);

            }

        }

        private static void mortgage(ref PlayerInfo p, ref PropertyInfo prop)
        {
            if(p.Pseudo == prop.Owner && !prop.isMortgaged)
            {
                p.Balance += prop.Price / 2;
            }
        }
        private static void mortgage(ref PlayerInfo p, ref CompanyInfo prop)
        {
            if (p.Pseudo == prop.Owner && !prop.isMortgaged)
            {
                p.Balance += prop.Price / 2;
            }
        }
        private static void mortgage(ref PlayerInfo p, ref StationInfo prop)
        {
            if (p.Pseudo == prop.Owner && !prop.isMortgaged)
            {
                p.Balance += prop.Price / 2;
            }
        }
        #endregion

        //Un peu moisie comme tricks, mais j'ai pas trouvé mieux :D puis ça permet de géré les déco/réco 
        private void updateClient()
        {

                Packet packet = new Packet();
                packet.Type = "updateGameData";
                string gameDataString = JsonConvert.SerializeObject(GameData.GetGameData, Formatting.Indented);
                packet.Content = gameDataString;
                packet.ChatMessage = response.ChatMessage;
                packet.ServerContent = response.ServerContent;
                packet.ServerMessage = response.ServerMessage;
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                msg = Encoding.UTF8.GetBytes(packetToSend);
                sendMsg(msg);
                response.ServerContent = "";
                response.ServerMessage = "";
        }

        private string GetNextPlayer()
        {
            PlayerInfo firstPlayer = GameData.GetGameData.PlayerList.FirstOrDefault();
            PlayerInfo lastPlayer = GameData.GetGameData.PlayerList.LastOrDefault();

            if (GameData.GetGameData.CurrentPlayerTurn == "")
                return firstPlayer.Pseudo;

            for (int i=0; i < GameData.GetGameData.PlayerList.Count; i++)
                {
                    if(GameData.GetGameData.PlayerList[i].Pseudo == GameData.GetGameData.CurrentPlayerTurn)
                    {

                    if (GameData.GetGameData.PlayerList[i].Pseudo == lastPlayer.Pseudo)
                    {
                        return firstPlayer.Pseudo;
                    }
                    else
                    {
                        return GameData.GetGameData.PlayerList[i+1].Pseudo;
                    }
                }

             }
            return lastPlayer.Pseudo;
        }
        

        private void CheckIfStillConnected()
        {
            while (true)
            {
                for (int i = 0; i < acceptList.Count; i++)
                {
                    if (((Socket)acceptList[i]).Poll(10, SelectMode.SelectRead) && ((Socket)acceptList[i]).Available == 0)
                    {
                        if (!readLock)
                        {
                            Console.WriteLine("Client " + ((Socket)acceptList[i]).GetHashCode() + " déconnecté");
                            removePseudo(((Socket)acceptList[i]));
                            ((Socket)acceptList[i]).Close();
                            acceptList.Remove(((Socket)acceptList[i]));
                            i--;
                        }
                    }
                }
                if (acceptList.Count == 1 && GameData.GetGameData.CurrentPlayerTurn != "" && GameData.GetGameData.CurrentPlayerTurn != null)
                {
                    Packet packet = new Packet();
                    packet.Type = "updateGameData";
                    string gameDataString = JsonConvert.SerializeObject(GameData.GetGameData, Formatting.Indented);
                    packet.Content = gameDataString;
                    packet.ChatMessage = response.ChatMessage;
                    packet.ServerContent = response.ServerContent;
                    packet.ServerMessage = response.ServerMessage;
                    string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                    msg = Encoding.UTF8.GetBytes(packetToSend);
                    sendMsg(msg);
                    response.ServerContent = GameData.GetGameData.CurrentPlayerTurn;
                    response.ServerMessage = "won";

                }
                Thread.Sleep(5);
            }
        }
        //Vérifie que le pseudo n'est pas déjà attribué à un autre utilisateur
        private bool checkPseudo(string pseudo, Socket Resource)
        {
            if (MatchList.ContainsValue(pseudo))
            {
                //Le pseudo est déjà pris, on refuse la connexion.
                ((Socket)acceptList[acceptList.IndexOf(Resource)]).Shutdown(SocketShutdown.Both);
                ((Socket)acceptList[acceptList.IndexOf(Resource)]).Close();
                acceptList.Remove(Resource);
                Console.WriteLine("Pseudo déjà pris");
                return false;
            }
            else
            {
                MatchList.Add(Resource, pseudo);
                getConnected();
            }
            return true;
        }

        private void getConnected()
        {

            foreach (object item in MatchList.Values)
            {
                Console.WriteLine(item);
            }
        }
        //Lorsqu'un client se déconnecte, il faut supprimer le pseudo associé à cette connexion
        private void removePseudo(Socket Resource)
        {
            Console.Write("DECONNEXION DE:" + MatchList[Resource]);
            msgDisconnected =  ((string)MatchList[Resource]).Trim() + " vient de se déconnecter!";
            Thread DiscInfoToAll = new Thread(new ThreadStart(infoToAll));
            DiscInfoToAll.Start();
            DiscInfoToAll.Join();
            MatchList.Remove(Resource);
        }


        private void getRead()
        {
            while (true)
            {
                readList.Clear();
                for (int i = 0; i < acceptList.Count; i++)
                {
                    readList.Add((Socket)acceptList[i]);
                }
                if (readList.Count > 0)
                {
                    Socket.Select(readList, null, null, 1000);
                    for (int i = 0; i < readList.Count; i++)
                    {
                        if (((Socket)readList[i]).Available > 0)
                        {
                            readLock = true;
                            int paquetsReceived = 0;
                            long sequence = 0;
                            string Nick = null;
                            string formattedMsg = "";

                            while (((Socket)readList[i]).Available > 0)
                            {
                                msg = new byte[((Socket)readList[i]).Available];
                                ((Socket)readList[i]).Receive(msg, msg.Length, SocketFlags.None);
                                msgString = System.Text.Encoding.UTF8.GetString(msg);
                                if (paquetsReceived == 0)
                                {

                                    string seq = msgString.Substring(0, 6);

                                    try
                                    {
                                        sequence = Convert.ToInt64(seq);

                                        Nick = msgString.Substring(6,15);


                                        try
                                        {
                                            string json = msgString.Substring(21, (msgString.Length - 21));
                                            Packet p = JsonConvert.DeserializeObject<Packet>(json);

                                            if(p.Type == "pret")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                if (!playerPret.Contains(player))
                                                    playerPret.Add(player);
                                            }

                                            if (p.Type == "message")
                                            {
                                                response.Type = "message";
                                                response.ChatMessage = Nick.Trim('0') + " a écrit: " + p.ChatMessage;


                                            }

                                            if (p.Type == "moove")
                                            {
                                               
                                                Random rnd = new Random();
                                                int dice1 = rnd.Next(1, 7);
                                                int dice2 = rnd.Next(1, 7);
                                                response.Type = "message";

                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                oldPlayer = player.Pseudo;
                                                if (!player.lost)
                                                {
                                                    if ((GameData.GetGameData.CurrentPlayerTurn == null || GameData.GetGameData.CurrentPlayerTurn == "") && playerPret.Count == PlayerManager.nbPlayer()) // on check si le jeu à commencer
                                                    {
                                                        GameData.GetGameData.CurrentPlayerTurn = GetNextPlayer(); // on init le premier joueur à bouger
                                                    }
                                                    if (GameData.GetGameData.CurrentPlayerTurn == player.Pseudo) // on check si c'est le tour du joueur qui a envoyé le packet.
                                                    {

                                                        if (!player.isInJail) // partie ou le joueur peut jouer son tour normalement
                                                        {
                                                            int nbCase = dice1 + dice2;
                                                            response.ChatMessage = Nick.Trim('0') + " avance de " + dice1 + ", " + dice2;
                                                            if (player.Position / 40 < (player.Position + nbCase) / 40)
                                                                player.Balance += salaire;
                                                            player.Position += nbCase;
                                                        }
                                                        else // le joueur est en prison
                                                        {
                                                            if (dice1 == dice2)
                                                            {

                                                                response.ChatMessage = Nick.Trim('0') + " est libéré  et avance de " + dice1 + " + " + dice2;
                                                                player.isInJail = false;
                                                                player.Position += dice1 + dice2;
                                                            }
                                                            response.ChatMessage = Nick.Trim('0') + " est en prison et a fait le jet de dés : " + dice1 + ", " + dice2;

                                                        }

                                                        if (player.Position % 40 == tp.searchPosGoToJail()) // on check si le joueur atterri en prison ou non.
                                                        {
                                                            player.Position = tp.searchPositionJail();
                                                            player.isInJail = true;
                                                        }
                                                        // si le joueur tombe sur une case chance ou caisse de communauté
                                                        if (tp.posChance.Contains(player.Position % 40))
                                                        {
                                                            Console.WriteLine("pioche chance");

                                                            CardInfo chance = tp.chanceCards.ElementAt(nextIndComm % tp.chanceCards.Count);
                                                            nextIndChance++;
                                                            // nextIndChance %= tp.chanceCards.Count;

                                                            Dictionary<string, CardInfo> dicoChance = new Dictionary<string, CardInfo>();
                                                            dicoChance.Add(Nick.Trim('0'), chance);
                                                            response.ServerContent = JsonConvert.SerializeObject(dicoChance,Formatting.Indented);
                                                            response.ServerMessage = "drawChance";
                                                            GameServerManager.useEffectCard(chance, ref player,ref tp, salaire);
                                                            if (player.Balance < 0)
                                                            {
                                                                AutoVente(player, 0, ref tp);
                                                                if (player.Balance < 0)
                                                                {
                                                                     player.lost = true;
                                                                   
                                                                }
                                                            }

                                                        }
                                                        else if (tp.posCommunity.Contains(player.Position % 40))
                                                        {
                                                            Console.WriteLine("pioche community");
                                                            CardInfo comm = tp.communityCards.ElementAt(nextIndComm % tp.communityCards.Count);
                                                            nextIndComm++;
                                                            //nextIndComm %= tp.communityCards.Count;

                                                            Dictionary<string, CardInfo> dicoComm = new Dictionary<string, CardInfo>();
                                                            dicoComm.Add(Nick.Trim('0'), comm);
                                                            response.ServerContent = JsonConvert.SerializeObject(dicoComm, Formatting.Indented);

                                                            GameServerManager.useEffectCard(comm, ref player, ref tp, salaire);
                                                            if (player.Balance < 0)
                                                            {
                                                                AutoVente(player, 0, ref tp);
                                                                if (player.Balance < 0)
                                                                {
                                                                    player.lost = true;
                                                                    
                                                                }
                                                            }

                                                            response.ServerMessage = "drawCommunity";
                                                        }


                                                        PropertyInfo propRent = tp.searchIndexPropertyAtPos(player.Position); // on calcule le loyer qu'il doit payer.
                                                        CompanyInfo companyRent = tp.searchCaseCompanyAtPos(player.Position);
                                                        StationInfo stationRent = tp.searchCaseStationAtPos(player.Position);

                                                        if (propRent != null && player.Pseudo != propRent.Owner)
                                                        {
                                                            
                                                            int rent = RentManager.computeRent(propRent, player, tp);
                                                            if (rent >= 0 && player.Balance >= rent)
                                                            {
                                                                player.Balance -= rent;
                                                                response.ChatMessage += " Le joueur  " + player.Pseudo + " paie " + rent + "€ à " + propRent.Owner;
                                                                PlayerManager.GetPlayerByPseuso(propRent.Owner).Balance += rent;
                                                            }
                                                            else
                                                            {
                                                                #region autovente
                                                                AutoVente(player, rent, ref tp);
                                                                #endregion
                                                            }

                                                            if (player.Balance < rent)
                                                            {
                                                                player.Balance -= rent;
                                                                response.ChatMessage += " Le joueur  " + player.Pseudo + " a perdu. Il paie " + player.Balance + "€ à " + propRent.Owner;
                                                                PlayerManager.GetPlayerByPseuso(propRent.Owner).Balance += player.Balance;

                                                                player.lost = true;
                                                               

                                                      
                                                            }



                                                        }
                                                        if (companyRent != null && player.Pseudo != companyRent.Owner)
                                                        {
                                                            int rent = RentManager.computeRent(companyRent, player, tp);
                                                            if (rent >= 0 && player.Balance >= rent)
                                                            {
                                                                player.Balance -= rent;
                                                               
                                                                PlayerManager.GetPlayerByPseuso(companyRent.Owner).Balance += rent;
                                                            }
                                                            else
                                                            {
                                                                AutoVente(player, rent, ref tp);
                                                            }

                                                            if (player.Balance < rent)
                                                            {
                                                                
                                                               
                                                                PlayerManager.GetPlayerByPseuso(companyRent.Owner).Balance += player.Balance;
                                                                player.Balance -= rent;

                                                                player.lost = true;
                                                               
                                                                
                                                            }
                                                        }

                                                        if (stationRent != null && player.Pseudo != stationRent.Owner)
                                                        {
                                                            int rent = RentManager.computeRent(stationRent, player, tp);
                                                            if (rent >= 0 && player.Balance> rent)
                                                            {
                                                               
                                                                response.ChatMessage += " Le joueur  " + player.Pseudo + " paie " + rent + "€ à " + stationRent.Owner;
                                                                PlayerManager.GetPlayerByPseuso(stationRent.Owner).Balance += rent;
                                                                player.Balance -= rent;
                                                            }
                                                            else
                                                            {
                                                                AutoVente(player, rent, ref tp);
                                                            }

                                                            if (player.Balance < rent)
                                                            {
                                                                
                                                                PlayerManager.GetPlayerByPseuso(stationRent.Owner).Balance += player.Balance;
                                                                player.Balance -= rent;
                                                                player.lost = true;

                                                               
                                                            }
                                                        }

                                                        if (player.lost)
                                                        {
                                                            playerLost.Add(player);
                                                            response.ServerMessage = "lost";
                                                            response.ServerContent = JsonConvert.SerializeObject(player);
                                                        }
                                                        PlayerInfo playerSuivant = PlayerManager.GetPlayerByPseuso(GameData.GetGameData.CurrentPlayerTurn);
                                                        do
                                                        {
                                                            GameData.GetGameData.CurrentPlayerTurn = GetNextPlayer(); // on passe au joueur suivant.
                                                            playerSuivant = PlayerManager.GetPlayerByPseuso(GameData.GetGameData.CurrentPlayerTurn);

                                                        } while (playerSuivant.lost);
                                                        
                                                    }
                                                    else
                                                    {
                                                        response.ChatMessage = "Ce n'est pas au tour de " + player.Pseudo;
                                                    }
                                                }
                                                else
                                                {
                                                    response.ServerMessage = "Le joueur à perdu et ne peut plus jouer.";
                                                }
                                            }

                                            if(p.Type == "mortGageProperty")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                PropertyInfo cell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);

                                                PropertyInfo prop = tp.searchCaseProperty(cell.Location);

                                                if (prop!=null && prop.Owner != null && prop.Owner == player.Pseudo && !prop.isMortgaged && prop.NumberOfHouse == 0)
                                                {
                                                    player.Balance += prop.Price / 2;
                                                    prop.isMortgaged = true;
                                                    response.ChatMessage = player.Pseudo + " hypothèque la propriété " + prop.Location + " et gagne " + (prop.Price / 2);
                                                }
                                                else
                                                {
                                                    if (prop.Owner == player.Pseudo && prop.isMortgaged && player.Balance > prop.Price / 2)
                                                    {
                                                        player.Balance -= prop.Price / 2;
                                                        prop.isMortgaged = false;
                                                        response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la propriété " + prop.Location;
                                                    }
                                                    else
                                                    {
                                                        response.ServerMessage = "Erreur : la propriété ne peut pas être hypothéquée";
                                                    }
                                                }
                                            }

                                            if (p.Type == "mortGageCompany")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                CompanyInfo cell = JsonConvert.DeserializeObject<CompanyInfo>(p.Content);
                                                CompanyInfo prop = tp.searchCaseCompany(cell.TextLabel);
                                                if (prop != null && prop.Owner != null && prop.Owner == player.Pseudo && !prop.isMortgaged)
                                                {
                                                    player.Balance += prop.Price / 2;
                                                    prop.isMortgaged = true;
                                                    response.ChatMessage = player.Pseudo + " hypothèque la propriété " + prop.TextLabel + " et gagne " + (prop.Price / 2);
                                                }
                                                else
                                                {

                                                    if (prop.Owner == player.Pseudo && prop.isMortgaged && player.Balance > prop.Price / 2)
                                                    {
                                                        player.Balance -= prop.Price / 2;
                                                        prop.isMortgaged = false;
                                                        response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la compagnie " + prop.TextLabel;
                                                    }
                                                    else
                                                    {
                                                        response.ServerMessage = "Erreur : la propriété ne peut pas être hypothéquée";
                                                    }
                                                }
                                            }

                                            if (p.Type == "mortGageStation")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                StationInfo cell  = JsonConvert.DeserializeObject<StationInfo>(p.Content);
                                                StationInfo prop = tp.searchCaseStation(cell.TextLabel);


                                                if (prop != null && prop.Owner != null && prop.Owner == player.Pseudo && !prop.isMortgaged)
                                                {
                                                    player.Balance += prop.Price / 2;
                                                    prop.isMortgaged = true;
                                                    response.ChatMessage = player.Pseudo + " hypothèque la propriété " + prop.TextLabel + " et gagne " + (prop.Price / 2);
                                                }
                                                else
                                                {
                                                    if (prop.Owner == player.Pseudo && prop.isMortgaged && player.Balance > prop.Price / 2)
                                                    {
                                                        player.Balance -= prop.Price / 2;
                                                        prop.isMortgaged = false;
                                                        response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la propriété " + prop.TextLabel;
                                                    }
                                                    else
                                                    {

                                                        response.ServerMessage = "Erreur : la propriété ne peut pas être hypothéquée";
                                                    }
                                                }
                                            }

                                            #region useless

                                            if(p.Type == "unMortGageProperty")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                PropertyInfo cell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);

                                                PropertyInfo stat = tp.searchCaseProperty(cell.Location);

                                                if (stat.Owner == player.Pseudo && stat.isMortgaged && player.Balance > stat.Price / 2)
                                                {
                                                    player.Balance -= stat.Price / 2;
                                                    stat.isMortgaged = false;
                                                    response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la propriété " + stat.TextLabel;
                                                }
                                                else
                                                {
                                                    response.ChatMessage = "La propriété " + stat.TextLabel + " n'a pas été déshypothéquée.";
                                                }
                                            }
                                            if (p.Type == "unMortGageStation")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                StationInfo cell = JsonConvert.DeserializeObject<StationInfo>(p.Content);

                                                StationInfo stat = tp.searchCaseStation(cell.TextLabel);

                                                if(stat.Owner == player.Pseudo && stat.isMortgaged && player.Balance>stat.Price/2)
                                                {
                                                    player.Balance -= stat.Price / 2;
                                                    stat.isMortgaged = false;
                                                    response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la station " + stat.TextLabel;
                                                }
                                                else
                                                {
                                                    response.ChatMessage = "La station "+stat.TextLabel+" n'a pas été déshypothéquée.";
                                                }



                                            }
                                            if (p.Type == "unMortGageCompany")
                                            {
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                CompanyInfo cell = JsonConvert.DeserializeObject<CompanyInfo>(p.Content);

                                                CompanyInfo stat = tp.searchCaseCompany(cell.TextLabel);

                                                if (stat.Owner == player.Pseudo && stat.isMortgaged && player.Balance > stat.Price / 2)
                                                {
                                                    player.Balance -= stat.Price / 2;
                                                    stat.isMortgaged = false;
                                                    response.ChatMessage = "Le joueur " + Nick.Trim('0') + "a déhypothéqué la compagnie " + stat.TextLabel;
                                                }
                                                else
                                                {
                                                    response.ChatMessage = "La compagnie " + stat.TextLabel + " n'a pas été déshypothéquée.";
                                                }

                                            }
                                            #endregion

                                            if (p.Type == "buyStation")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);// le content doit être le nom de la case info.
                                                StationInfo cell = JsonConvert.DeserializeObject<StationInfo>(p.Content);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                StationInfo stationToBuy = tp.searchCaseStation(cell.TextLabel); // search station dans la liste dans le themeparser.


                                                if ((stationToBuy.Owner == null || stationToBuy.Owner == "") && player.Balance > stationToBuy.Price)
                                                {
                                                    player.Balance -= stationToBuy.Price;
                                                    stationToBuy.Owner = player.Pseudo;
                                                    player.Stations.Add(stationToBuy);
                                                    response.ChatMessage = Nick.Trim('0') + " achète " + stationToBuy.TextLabel;

                                                }
                                                else
                                                {
                                                    response.ChatMessage = Nick.Trim('0') + " ne peut pas acheter cette station"; // on retourne un message de retour indiquant que la transaction ne s'est pas bien passée.
                                                }


                                            }
                                            if (p.Type == "buyProperty")
                                            {

                                                response.Type = "message";
                                                Console.WriteLine(p.Content);// le content doit être le nom de la case info.

                                                PropertyInfo cell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);

                                                PropertyInfo propertyToBuy = tp.searchCaseProperty(cell.Location);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                // On cherche la case info dans la liste des cases qui sont contenus dans le serveur
                                                if ((propertyToBuy.Owner == null || propertyToBuy.Owner == "") && player.Balance > propertyToBuy.Price)
                                                {
                                                    player.Balance -= propertyToBuy.Price;
                                                    propertyToBuy.Owner = player.Pseudo;
                                                    player.Properties.Add(propertyToBuy);
                                                    response.ChatMessage = Nick.Trim('0') + " achète " + propertyToBuy.Location;  // on initialise un message de retour
                                                }

                                                /*else if (cell is CustomInfo)
                                                { // de même pour les cases spéciales où l'on doit tirer les dés.
                                                    customToBuy = (CustomInfo)tp.searchCase(cell.TextLabel);

                                                    if (customToBuy.Owner != player.Pseudo && customToBuy.Owner != "" && player.Balance > customToBuy.Income)
                                                    {
                                                        player.Balance -= customToBuy.Income;
                                                        customToBuy.Owner = player.Pseudo;
                                                        player.Add(customToBuy);
                                                        response.ChatMessage = Nick.Trim('0') + " achète " + customToBuy.TextLabel;

                                                    }
                                                }*/
                                                else
                                                {
                                                    response.ChatMessage = Nick.Trim('0') + " ne peut pas acheter cette propriété"; // on retourne un message de retour indiquant que la transaction ne s'est pas bien passée.
                                                }
                                            }
                                            if(p.Type == "buyCompany")
                                            {
                                                CompanyInfo cell = JsonConvert.DeserializeObject<CompanyInfo>(p.Content);

                                                CompanyInfo propertyToBuy = tp.searchCaseCompany(cell.TextLabel);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                // On cherche la case info dans la liste des cases qui sont contenus dans le serveur
                                                if ((propertyToBuy.Owner == null || propertyToBuy.Owner == "") && player.Balance > propertyToBuy.Price)
                                                {
                                                    player.Balance -= propertyToBuy.Price;
                                                    propertyToBuy.Owner = player.Pseudo;
                                                    player.Companies.Add(propertyToBuy);
                                                    response.ChatMessage = Nick.Trim('0') + " achète " + propertyToBuy.TextLabel;  // on initialise un message de retour
                                                }
                                            }

                                            if (p.Type == "sellProperty")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);
                                                PropertyInfo propertyToSell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                if (propertyToSell != null && propertyToSell.Owner == player.Pseudo && PlayerManager.searchProperty(player, propertyToSell.Location) != null && propertyToSell.NumberOfHouse == 0)
                                                {
                                                    if (!propertyToSell.isMortgaged)
                                                        player.Balance += propertyToSell.Price;
                                                    else
                                                        player.Balance += propertyToSell.Price / 2;
                                                    player.Properties.Remove(PlayerManager.searchProperty(player, propertyToSell.Location));

                                                    /*  player.Balance += (propertyToSell.NumberOfHouse * propertyToSell.HouseCost) / 2;
                                                      if (propertyToSell.HasHostel)
                                                      {
                                                          player.Balance += propertyToSell.HostelCost / 2;

                                                      }


                                                      tp.searchCaseProperty(propertyToSell.Location).NumberOfHouse = 0;
                                                      tp.searchCaseProperty(propertyToSell.Location).HasHostel = false;
                                                      */
                                                    tp.searchCaseProperty(propertyToSell.Location).Owner = null;
                                                    tp.searchCaseProperty(propertyToSell.Location).isMortgaged = false;
                                                    response.ChatMessage = Nick.Trim('0') + " vend la propriété " + propertyToSell.Location;
                                                }
                                                else
                                                {
                                                    response.ChatMessage = Nick.Trim('0') + " ne peut pas vendre la propriété " + propertyToSell.Location;
                                                }
                                            }
                                            if(p.Type == "sellStation")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);
                                                StationInfo cell = JsonConvert.DeserializeObject<StationInfo>(p.Content);
                                                StationInfo station = tp.searchCaseStation(cell.TextLabel);

                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                if(station != null && station.Owner == player.Pseudo)
                                                {
                                                    player.Stations.Remove(station);
                                                    tp.searchCaseStation(station.TextLabel).Owner = null;
                                                    tp.searchCaseStation(station.TextLabel).isMortgaged = false;
                                                    if (station.isMortgaged)
                                                        player.Balance += station.Price/2;
                                                    else
                                                        player.Balance += station.Price;
                                                }
                                            }
                                            if(p.Type == "sellCompany")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);
                                                CustomInfo cell = JsonConvert.DeserializeObject<CustomInfo>(p.Content);

                                                CompanyInfo company = tp.searchCaseCompany(cell.TextLabel);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                if (company != null && company.Owner == player.Pseudo)
                                                {
                                                    
                                                    player.Companies.Remove(company);
                                                    tp.searchCaseStation(company.TextLabel).Owner = null;
                                                    tp.searchCaseStation(company.TextLabel).isMortgaged = false;
                                                    if (company.isMortgaged)
                                                        player.Balance += company.Price / 2;
                                                    else
                                                        player.Balance += company.Price;
                                                }

                                            }
#region useless
                                            if (p.Type == "drawChance")
                                            {
                                                /*
                                                Console.WriteLine("pioche chance");

                                                CardInfo chance = tp.chanceCards.ElementAt(nextIndComm);
                                                nextIndChance++;
                                                nextIndChance %= tp.chanceCards.Count;

                                                Dictionary<string, CardInfo> dicoChance = new Dictionary<string, CardInfo>();
                                                dicoChance.Add(Nick.Trim('0'), chance);
                                                response.ServerContent = JsonConvert.SerializeObject(dicoChance);
                                                response.ServerMessage = "drawChance";*/
                                            }


                                            if (p.Type == "drawCommunity")
                                            {
                                                /*
                                                Console.WriteLine("pioche community");
                                                CardInfo comm = tp.communityCards.ElementAt(nextIndComm);
                                                nextIndComm++;
                                                nextIndComm %= tp.communityCards.Count;
                                                
                                                Dictionary<string, CardInfo> dicoComm = new Dictionary<string, CardInfo>();
                                                dicoComm.Add(Nick.Trim('0'), comm);
                                                response.ServerContent = JsonConvert.SerializeObject(dicoComm);

                                                Console.WriteLine(p.ServerContent);

                                                response.ServerMessage = "drawCommunity";*/

                                            }
                                            #endregion

                                            if (p.Type == "payFreedom")
                                            {
                                                response.Type = "message";
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                if (tp.jail.priceOfFreedom < player.Balance) // si le joueur peut payer.
                                                {
                                                    player.Balance -= tp.jail.priceOfFreedom;
                                                    player.isInJail = false;
                                                    response.ChatMessage = player.Pseudo + " a payé " + tp.jail.priceOfFreedom + " et est sorti de prison.";

                                                }
                                                else
                                                {
                                                    response.ChatMessage = player.Pseudo + " n'a pas les fonds disponibles pour sortir de prison. (" + tp.jail.priceOfFreedom + "€ )";
                                                }
                                                GameData.GetGameData.CurrentPlayerTurn = GetNextPlayer(); // on passe au tour suivant
                                            }

                                            if (p.Type == "buildHouse") // Maison + hotel
                                            {

                                                Console.WriteLine("Construction d'une maison demandé");
                                                Console.WriteLine(p.Content);

                                                PropertyInfo cell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                if (cell.Location != null || cell.Location != "") // je check si la propriété possède un nom
                                                {
                                                    PropertyInfo propertyToBuild = tp.searchCaseProperty(cell.Location);
                                                    if (propertyToBuild.Owner == player.Pseudo)
                                                    {
                                                        List<PropertyInfo> listColor = tp.searchCasePropertyOfColor(propertyToBuild.Color);
                                                        if (listColor.Count > 0)
                                                        {
                                                            bool canBuild = true;

                                                            if (propertyToBuild.NumberOfHouse > 0)
                                                            {
                                                                for (int it = 0; it < listColor.Count && canBuild; it++)
                                                                {
                                                                    PropertyInfo pi = listColor[it];
                                                                    Console.WriteLine(" Owner : " + pi.Owner);
                                                                    Console.WriteLine(" Property to build owner : " + propertyToBuild.Owner);
                                                                    if (pi.NumberOfHouse < propertyToBuild.NumberOfHouse || pi.Owner != propertyToBuild.Owner || pi.Owner == null)
                                                                    {
                                                                        Console.WriteLine("Je suis dans le if");
                                                                        canBuild = false;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int it = 0; it < listColor.Count && canBuild; it++)
                                                                {
                                                                    PropertyInfo pi = listColor[it];
                                                                    if ((pi.Owner == null || pi.Owner != player.Pseudo))
                                                                    {
                                                                        Console.WriteLine("Je suis dans le else");
                                                                        canBuild = false;
                                                                    }
                                                                }
                                                            }
                                                            if (canBuild)
                                                            {
                                                                if (propertyToBuild.NumberOfHouse < 4 && player.Balance > propertyToBuild.HostelCost) // je check si la propriété ne possède pas le nombre maximale de maison
                                                                {
                                                                    propertyToBuild.NumberOfHouse++;
                                                                    player.Balance -= propertyToBuild.HouseCost;
                                                                    response.ChatMessage = Nick.Trim('0') + " construit une maison sur le terrain " + propertyToBuild.Location;
                                                                }
                                                                else if (propertyToBuild.NumberOfHouse == 4 && !propertyToBuild.HasHostel && player.Balance > propertyToBuild.HostelCost) // je check si la propriété ne possède pas d'hotel
                                                                {
                                                                    player.Balance -= propertyToBuild.HostelCost;
                                                                    propertyToBuild.HasHostel = true;
                                                                    response.ChatMessage = Nick.Trim('0') + " construit un hotel sur le terrain " + propertyToBuild.Location;
                                                                }
                                                            }
                                                            else // sinon on renvoie un message indiquant qu'on ne peut pas construire.
                                                            {
                                                                response.ChatMessage = Nick.Trim('0') + " ne peut pas construire sur le terrain " + propertyToBuild.Location;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        response.ChatMessage = Nick.Trim('0') + " n'est pas propriétaire du terrain " + propertyToBuild.Location;
                                                    }
                                                }
                                            }
                                            if (p.Type == "sellHouse")
                                            {
                                                Console.WriteLine("Construction d'une maison demandé");
                                                Console.WriteLine(p.Content);

                                                PropertyInfo cell = JsonConvert.DeserializeObject<PropertyInfo>(p.Content);
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                if (cell.Location != null || cell.Location != "") // je check si la propriété possède un nom
                                                {
                                                    PropertyInfo propToSellHouse = tp.searchCaseProperty(cell.Location);
                                                    if (propToSellHouse.Owner == player.Pseudo)
                                                    {
                                                        List<PropertyInfo> listColor = tp.searchCasePropertyOfColor(propToSellHouse.Color);
                                                        if (listColor.Count > 0)
                                                        {
                                                            bool canSell = true;

                                                            if (propToSellHouse.NumberOfHouse > 0)
                                                            {
                                                                for (int it = 0; it < listColor.Count && canSell; it++)
                                                                {
                                                                    PropertyInfo pi = listColor[it];
                                                                    if (pi.NumberOfHouse > propToSellHouse.NumberOfHouse || pi.Owner != propToSellHouse.Owner || pi.Owner == null)
                                                                    {
                                                                        canSell = false;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                response.ChatMessage = "Le terrain " + propToSellHouse.Location + " n'a plus de maison à vendre.";
                                                            }
                                                            if (canSell)
                                                            {
                                                                if (propToSellHouse.NumberOfHouse > 0 && !propToSellHouse.HasHostel) // je check si la propriété  possède une maison
                                                                {
                                                                    propToSellHouse.NumberOfHouse--;
                                                                    player.Balance += propToSellHouse.HouseCost / 2;
                                                                    response.ChatMessage = Nick.Trim('0') + " vend une maison sur le terrain " + propToSellHouse.Location;
                                                                }
                                                                else if (propToSellHouse.NumberOfHouse == 4 && !propToSellHouse.HasHostel) // je check si la propriété  possède  un hotel 
                                                                {
                                                                    player.Balance += propToSellHouse.HostelCost / 2;
                                                                    propToSellHouse.HasHostel = false;
                                                                    response.ChatMessage = Nick.Trim('0') + " vend un hotel sur le terrain " + propToSellHouse.Location;
                                                                }
                                                            }
                                                            else // sinon on renvoie un message indiquant qu'on ne peut pas vendre.
                                                            {
                                                                response.ChatMessage = Nick.Trim('0') + " ne peut pas vendre sur le terrain " + propToSellHouse.Location;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        response.ChatMessage = Nick.Trim('0') + " n'est pas propriétaire du terrain " + propToSellHouse.Location;
                                                    }
                                                }


                                            }

                                            if(p.Type == "useFreeFromJailCard")
                                            {

                                               // Console.WriteLine("utilise une carte libéré de prison");
                                                Console.WriteLine(Nick.Trim('0') + " utilise une carte libéré de prison");
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));

                                                if(p.Content == "freeFromJailCommunity" && player.hasCommunityCardFree && player.isInJail)
                                                {
                                                    tp.communityCards.Add(tp.freeFromJail);
                                                    player.hasCommunityCardFree = false;
                                                    player.isInJail = false;
                                                    p.ChatMessage = Nick.Trim('0') + " est libéré prison en utilisant une carte caisse de communauté";
                                                }
                                                else if(p.Content == "freeFromJailChance" && player.hasChanceCardFree && player.isInJail)
                                                {
                                                    tp.communityCards.Add(tp.freeFromJail);
                                                    player.hasChanceCardFree = false;
                                                    player.isInJail = false;
                                                    response.ChatMessage = Nick.Trim('0') + " est libéré prison en utilisant une carte chance";
                                                }
                                                else
                                                {
                                                    response.ChatMessage = Nick.Trim('0') + " n'est pas en prison ou ne possède pas de cartes.";
                                                }
                                            }
                                            
                                            if (p.Type == "finTour")
                                            {
                                                // get le tour du joueur suivant.
                                            }
                                            
                                            if(oldPlayer == GameData.GetGameData.CurrentPlayerTurn)
                                            {
                                                PlayerInfo pl = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                Console.WriteLine("tu as gagner");
                                                response.ServerContent = JsonConvert.SerializeObject(pl);
                                                response.ServerMessage = "won";

                                            }
                                                
                                               
                                            
                                           
                                            if(p.Type == "erreurPacket")
                                            {
                                                if(lastPacketSend != null)
                                                {
                                                    response = lastPacketSend;
                                                }
                                            }
                                            lastPacketSend = response;
                                        }
                                        catch
                                        {

                                        }

                                    }
                                    catch( Exception e)
                                    {

                                        Console.Write(e);
                                        acceptList.Remove(((Socket)readList[i]));
                                        break;
                                    }
                                }

                                if (sequence == 1)
                                {
                                    if (!checkPseudo(Nick, ((Socket)readList[i])))
                                    {
                                        break;
                                    }
                                    else
                                    {

                                        msg = Encoding.UTF8.GetBytes(formattedMsg);
                                        PlayerInfo playerInfo = new PlayerInfo();
                                        playerInfo.Pseudo = Nick.Trim('0');
                                        playerInfo.Balance = initBalance;
                                        playerInfo.Position = initPosition;
                                        playerInfo.ColorCode = avatar.ElementAt(0);
                                        playerInfo.Image = "ressources/templates/default/avatar/" + playerInfo.ColorCode + ".png";
                                        avatar.Remove(playerInfo.ColorCode);
                                        response.Type = "updateGameData";
                                        response.ChatMessage = Nick.Trim('0') + " vient de se connecter";

                                        if (!GameData.GetGameData.PlayerList.Any(p => p.Pseudo.Contains(playerInfo.Pseudo)))
                                        {
                                            GameData.GetGameData.PlayerList.Add(playerInfo);
                                        }
                                        response.Content = JsonConvert.SerializeObject(GameData.GetGameData, Formatting.Indented);

                                    }
                                }
                                if (useLogging)
                                {
                                    Logging(formattedMsg);
                                }
                               string  packetToSend = JsonConvert.SerializeObject(response, Formatting.Indented);
                                msg = Encoding.UTF8.GetBytes(packetToSend);
                                Thread forwardingThread = new Thread(new ThreadStart(updateClient));
                                forwardingThread.Start();
                                forwardingThread.Join();
                                Console.WriteLine(response.Content);

                                paquetsReceived++;
                                
                            }
                            readLock = false;
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
    }


    public class forwardToAll
    {
        public ArrayList acceptList = new ArrayList();
        public Hashtable MatchList = new Hashtable();
        public forwardToAll() { }
        public void sendMsg(byte[] msg)
        {
            for (int i = 0; i < acceptList.Count; i++)
            {
                if (((Socket)acceptList[i]).Connected)
                {
                    try
                    {
                        int bytesSent = ((Socket)acceptList[i]).Send(msg, msg.Length, SocketFlags.None);
                    }
                    catch
                    {
                        Console.Write(((Socket)acceptList[i]).GetHashCode() + " déconnecté");
                    }
                }
                else
                {
                    acceptList.Remove((Socket)acceptList[i]);
                    i--;
                }
            }
        }

        public void sendMsg(string message)
        {
            for (int i = 0; i < acceptList.Count; i++)
            {
                if (((Socket)acceptList[i]).Connected)
                {
                    try
                    {
                        byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
                        int bytesSent = ((Socket)acceptList[i]).Send(msg, msg.Length, SocketFlags.None);
                        Console.WriteLine("Writing to:" + acceptList.Count.ToString());
                    }
                    catch
                    {
                        Console.Write(((Socket)acceptList[i]).GetHashCode() + " déconnecté");
                    }
                }
                else
                {
                    acceptList.Remove((Socket)acceptList[i]);
                    i--;
                }
            }
        }
    }

}
