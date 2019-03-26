using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server.Model;

namespace server.Controleur
{

    /* A faire :
     *  Implémenter tout les méthodes pour pouvoir effectué sur les joueurs tout les actions possible au monopoly en respectant les régles
     * 
     * 
     * 
     * 
     * */
    public class PlayerManager 
    {
        public static PlayerInfo GetPlayerByPseuso(string pseudo)
        {
            foreach(PlayerInfo player in GameData.GetGameData.PlayerList)
            {
                if (player.Pseudo == pseudo)
                    return player;
            }

            return new PlayerInfo();
        }

        public static PropertyInfo searchProperty(PlayerInfo pl , string location)
        {
            foreach(PropertyInfo p in pl.Properties)
            {
                if(p.Location == location)
                {
                    return p;
                }
            }
            return null;
        }
    }


}
