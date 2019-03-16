using Monopoly.Model.Card;
using Monopoly.Model.UI;
using server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;


namespace Monopoly.Model
{
    /// <summary>
    /// Logique d'interaction pour Player.xaml
    /// </summary>
    public partial class Player : UserControl , INotifyPropertyChanged
    {

        #region Attributs
        private int _position;
        private bool _canMoove;
        public int grid;
        public PlayerInfoDisplay playerInfoDisplay;
        public event PropertyChangedEventHandler PropertyChanged;
        public PlayerInfo playerInfo;

        #endregion

        #region Accesseurs
        

        #endregion

        #region Constructeurs
        /// <summary>
        /// Instancie un joueur avec tous les attributs.
        /// </summary>
        /// <param name="name">Nom du Player</param>
        /// <param name="balance">Argent du Player</param>
        /// <param name="position">Position du Player sur le plateau</param>
        /// <param name="cards">Liste de cartes que le Player possède</param>
        /// <param name="image">Skin du Player</param>
        public Player( string name, int balance, int position, List<CaseInfo> estates, object image, string colorCode )
        {
            InitializeComponent();
            playerInfo = new PlayerInfo();
            playerInfo.Balance = balance;
            playerInfo.Image = image;
            playerInfo.Pseudo = name;
            playerInfo.Position = position;
            playerInfo.Estates = estates;    
            playerInfo.ColorCode = colorCode;

        }
        /// <summary>
        /// Instancie un Player avec le nom et l'id, avec tous les autres paramètres ( Balance, Position, Cartes) vide ou nul.
        /// </summary>
        /// <param name="idPlayer"> Id du Player </param>
        /// <param name="name"> Nom du Player </param>
        public Player(string name)
        {
            InitializeComponent();
            playerInfo.Pseudo = name;
            playerInfo.Balance = 0;
            playerInfo.Position = 0;
            playerInfo.Estates = new List<CaseInfo>();
            playerInfo.Image = null;

        }
        #endregion

        #region Méthodes


        /// <summary>
        /// TODO : Vérifie si le joueur est en faillite ou non (actuellement vérifie que son solde n'est pas négatif).
        /// </summary>
        /// <returns></returns>
        public bool IsBankruptcy()
        {
            bool result = false;
            if (playerInfo.Balance < 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        ///  On retire de l'argent au joueur (dans le cas d'un paiement pour autre joueur par exemple).
        ///  Le montant ne peut pas être négatif.
        /// </summary>
        /// <param name="amount"> Montant à retirer au joueur courant. </param>
        public void SoustractAmount(int amount)
        {

            if (amount > 0 && playerInfo.Balance > amount)
            {
                playerInfo.Balance -= amount;


            }
            else
            {
                throw new ArgumentException("Le joueur n'a pas assez d'argent.");
            }

        }

        /// <summary>
        ///  On rajoute de l'argent au joueur (dans le cas d'un paiement d'un autre joueur par exemple).
        ///  Le montant ne peut pas être négatif.
        /// </summary>
        /// <param name="amount"> Montant à ajouter au joueur courant. </param>
        public void AddAmount(int amount)
        {
            if (amount > 0)
            {
                playerInfo.Balance += amount;

            }
            else
            {
                throw new ArgumentException("On ne peut pas rajouter de montant négatif au joueur.");
            }

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        ///  Calcule le string de description d'un joueur. 
        /// </summary>
        /// <returns> Renvoie le string de description de l'objet player. </returns>
        public override string ToString()
        {
            string res = "; Nom du joueur : " + playerInfo.Pseudo + "; Argent : " + playerInfo.Balance + "; Position sur le plateau : " + playerInfo.Position;
            return res;
        }
        #endregion


    }
}
