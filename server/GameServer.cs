﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                            removeNick(((Socket)acceptList[i]));
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
        private bool checkNick(string nick, Socket Resource)
        {
            if (MatchList.ContainsValue(nick))
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
                MatchList.Add(Resource, nick);
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
        private void removeNick(Socket Resource)
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
                            string formattedMsg = null;
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
                                        Console.WriteLine(msgString.Length);
                                        Console.WriteLine(msgString);

                                        Nick = msgString.Substring(6,14);
                                        formattedMsg = Nick.Trim('0') + " a écrit: " + msgString.Substring(21, (msgString.Length - 21));
                                        if(msgString.Substring(21, (msgString.Length - 21)).StartsWith("{") && msgString.Substring(21, (msgString.Length - 21)).EndsWith("}"))
                                        {

                                            formattedMsg = "JSON DETECTED !";
                                        }

                                    }
                                    catch( Exception e)
                                    {

                                        Console.Write(e);
                                        acceptList.Remove(((Socket)readList[i]));
                                        break;
                                    }
                                }

                                msg = System.Text.Encoding.UTF8.GetBytes(formattedMsg);
                                if (sequence == 1)
                                {
                                    if (!checkNick(Nick, ((Socket)readList[i])))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        string rtfMessage = Nick.Trim('0') + " vient de se connecter \r\n";
                                        msg = System.Text.Encoding.UTF8.GetBytes(rtfMessage);
                                    }
                                }
                                if (useLogging)
                                {
                                    Logging(formattedMsg);
                                }

                                Console.WriteLine(formattedMsg);
                                Thread forwardingThread = new Thread(new ThreadStart(writeToAll));
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