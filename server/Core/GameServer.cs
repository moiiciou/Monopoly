using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Controleur;

namespace server
{
    public class GameServer : forwardToAll
    {
        ArrayList readList = new ArrayList(); //liste utilisée par socket.select 
        string msgString = null; //contiendra le message envoyé aux autres clients
        string msgDisconnected = null; //Notification connexion/déconnexion
        byte[] msg;//Message sous forme de bytes pour socket.send et socket.receive
        public bool useLogging = false; //booleen permettant de logger le processing dans un fichier log
        public bool readLock = false;//Flag aidant à la synchronisation
        private int initPosition = 0;
        public static int initBalance = 10000;
        private Packet response = new Packet();
        private bool gameOver = false;


        public void Start()
        {

            IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            Console.WriteLine("IP=" + ipAddress.ToString());
            Socket CurrentClient = null;
            Socket ServerSocket = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream,
              ProtocolType.Tcp);
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

        //Un peu moisie comme tricks, mais j'ai pas trouvé mieux :D puis ça permet de géré les déco/réco 
        private void updateClient()
        {

                Packet packet = new Packet();
                packet.Type = "updateGameData";
                string gameDataString = JsonConvert.SerializeObject(GameData.GetGameData, Formatting.Indented);
                packet.Content = gameDataString;
                packet.ChatMessage = "Game Data updated";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                msg = Encoding.UTF8.GetBytes(packetToSend);
                sendMsg(msg);


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


                                            if (p.Type == "message")
                                            {
                                                response.Type = "message";
                                                response.ChatMessage = Nick.Trim('0') + " a écrit: " + p.ChatMessage;


                                            }

                                            if (p.Type == "moove")
                                            {

                                                    Random rnd = new Random();
                                                    int dice = rnd.Next(1, 13);
                                                    response.Type = "message";
                                                    response.ChatMessage = Nick.Trim('0') + " avance de " + dice;
                                                    PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                    player.Position += dice;
                                                    GameData.GetGameData.CurrentPlayerTurn = GetNextPlayer();
                                            }

                                            if (p.Type == "buyProperty")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);
                                                CaseInfo propertyToBuy = JsonConvert.DeserializeObject<CaseInfo>(p.Content);
                                                /*
                                                 * Faire les check necessaire pour savoir si je peux acheter la propriété
                                                 */
                                                PlayerInfo player = PlayerManager.GetPlayerByPseuso(Nick.Trim('0'));
                                                player.Balance -= propertyToBuy.Price;
                                                propertyToBuy.Owner = player.Pseudo;
                                                player.Estates.Add(propertyToBuy);
                                                response.ChatMessage = Nick.Trim('0') +" achete une propriete";
                                            }

                                            if (p.Type == "sellProperty")
                                            {
                                                response.Type = "message";
                                                Console.WriteLine(p.Content);
                                                CaseInfo propertyToBuy = JsonConvert.DeserializeObject<CaseInfo>(p.Content);
  
                                                response.ChatMessage = Nick.Trim('0') + " vend une propriete";
                                            }


                                            if (p.Type == "drawChance")
                                            {

                                            }


                                            if(p.Type == "drawCommunity")
                                            {

                                            }

                                            if(p.Type =="useFreeFromJailCard")
                                            {

                                            }

                                            if(p.Type == "buildHouse") // Maison + hotel
                                            {
                                                Console.WriteLine("Construction d'une maison demandé");
                                                Console.WriteLine(p.Content);
                                                response.ChatMessage = Nick.Trim('0') + " construit une maison";

                                            }


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

                                Console.WriteLine("response :");
                                Console.WriteLine(response.Type);
                                Console.WriteLine(response.Content);
                                Console.WriteLine(response.ChatMessage);

                                Thread forwardingThread = new Thread(new ThreadStart(updateClient));
                                forwardingThread.Start();
                                forwardingThread.Join();
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
