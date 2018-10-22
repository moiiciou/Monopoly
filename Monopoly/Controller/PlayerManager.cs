using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopoly.Model;

namespace Monopoly.Controller
{
    public static class PlayerManager
    {
        #region Attributs
            static List<Player> Players = new List<Player>();
            static int NextId = 0;
        #endregion

        #region Méthodes de gestion des joueurs
        /// <summary>
        /// Créé un nouveau joueur avec les paramètres nom, argent et position.
        /// </summary>
        /// <param name="name"> Nom du joueur </param>
        /// <param name="balance"> Argent du joueur </param>
        /// <param name="position"> Position sur le plateau du joueur </param>
        public static void CreatePlayer(string name, int balance,int position)
        {
            Players.Add(new Player(SearchNextId(), name, balance, position, new List<Card>(), null));
        }

        /// <summary>
        /// Calcule et fourni le nouvel ID d'un joueur.
        /// </summary>
        /// <returns> Renvoie le nouvel ID du joueur </returns>
        public static int SearchNextId()
        {
            return NextId++;
        }

        public static void Pay(int idReciever, int idPayer, int amount)
        {
            Players[SearchPlayer(idReciever)].AddAmount(amount);
            Players[SearchPlayer(idPayer)].RemoveAmount(amount);

           

        }

        public static int SearchPlayer(int idPlayer)
        {
            int indPlayerFind = -1;
            
            for(int i=0; i<Players.Count && indPlayerFind==-1; i++)
            {
                if(Players[i].IdPlayer== idPlayer)
                {
                    indPlayerFind = i;
                }
            }
            return indPlayerFind;
        }

        public static List<Player> listAllPlayer()
        {
            return Players;
        }
        #endregion


    }
}
