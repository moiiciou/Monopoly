using MonopolyClient.Core;
using MonopolyClient.Core.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class AsynchIOServer
{
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    public static ObservableCollection<ClientInfo> Clients = new ObservableCollection<ClientInfo>();





    public AsynchIOServer()
    {
    }

    public static void StartListening()
    {
 
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                allDone.Reset();

                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void AcceptCallback(IAsyncResult ar)
    {
        allDone.Set();

        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0)
        {
            state.sb.Append(Encoding.UTF8.GetString(
                state.buffer, 0, bytesRead));
  
            content = state.sb.ToString();
            if (content.IndexOf("<EOF>") > -1)
            {
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content.Remove(content.Length - 5));

                Object incomingData = Tools.DerializeObject<Object>(content.Remove(content.Length - 5));

                Console.WriteLine("Object Type : {0}",
                   incomingData.GetType().ToString());

                ServerMessage serverMessage = new ServerMessage();

                if(incomingData.GetType() == typeof(ClientInfo))
                {
                    ClientInfo client = (ClientInfo)incomingData;
                    if(CheckPseudo(client.Pseudo))
                    {
                        Clients.Add(client);
                        serverMessage.Content = " Connection réussi !";
                    }
                    else
                    {
                        serverMessage.Content = "Pseudo déjà pris, veuillez réessayer !";
                    }
                    string response = Tools.SerializeObject<ServerMessage>(serverMessage);
                    Send(handler, response);
                }

            }
            else
            {
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
    }

    private static void Send(Socket handler, String data)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(data);

        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static bool CheckPseudo(string pseudo)
    {
        var item =Clients.FirstOrDefault(i => i.Pseudo == pseudo);
        if (item == null)
        {
            return true;
        }
        return false;
    }
}
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }  