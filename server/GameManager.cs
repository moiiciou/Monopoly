using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using server;

namespace server
{
    public static class GameManager
    {
       public static PlayerInfo joueurEnCours;
        static int numJoueur = 0;
        public static bool action = false;
      
        public static void TourManagement()
        {
            while (!checkGagnant())
            {
                joueurSuivant();
                GameServer.message.Add("Tour du joueur : "+ joueurEnCours.ToString());
                Packet p = new Packet();
                p.ChatMessage = GameManager.joueurEnCours.Pseudo;
                p.Content = "test";
                p.Type = "changement de tour";
                string test = JsonConvert.SerializeObject(p, Formatting.Indented);
         
                MainClass.startIt.sendMsg(test);// envoie le pseudo du joueur pour l'identifier et disable tous les buttons des autres joueurs, il faudrait l'ID du joueur pour que ce soit propre.

                // à partir d'ici déterminer le tour, genre lancer dés etc.
                action = false;

                while (!action)
                {
                    Thread.Sleep(10);
                }       
                // implémentation du tour.
               
            }
        }

        public static void joueurSuivant()
        {
            if(joueurEnCours == null)
            {
               joueurEnCours = GameServer.playersList.Values.ToList()[0];
            }
            else
            {
                numJoueur++;
                numJoueur %= (GameServer.playersList.Values.ToList().Count);
                joueurEnCours = GameServer.playersList.Values.ToList()[numJoueur];
            }
        }

        public static bool checkGagnant()
        {
            int counter =0;
            foreach(KeyValuePair<string,PlayerInfo> p in GameServer.playersList){
                if (p.Value.Balance > 0 )
                {
                    counter++;
                }
                         
            }

            if (counter > 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
