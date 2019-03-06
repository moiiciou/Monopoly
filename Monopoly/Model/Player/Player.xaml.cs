using Monopoly.Model.Card;
using Monopoly.Model.UI;
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
        public PlayerInfoDisplay playerInfo;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Accesseurs

        public int Balance { get; set; }

        public int IdPlayer { get; }

        public object Image { get; set; }

        public string NamePlayer { get; set; }

        public int Position
        {
            get
            {
                return _position;
            }

            set
            {
                if (value >= 0)
                {
                    _position = value % 40;
                    NotifyPropertyChanged();
                }
                else
                {
                    throw new ArgumentException("La valeur de la position ne peut pas être négative");
                }
            }
        }


        public List<BaseCard> Cards { get; }

        #endregion

        #region Constructeurs
        /// <summary>
        /// Instancie un joueur avec tous les attributs.
        /// </summary>
        /// <param name="idPlayer">Id du Player</param>
        /// <param name="name">Nom du Player</param>
        /// <param name="balance">Argent du Player</param>
        /// <param name="position">Position du Player sur le plateau</param>
        /// <param name="cards">Liste de cartes que le Player possède</param>
        /// <param name="image">Skin du Player</param>
        public Player(int idPlayer, string name, int balance, int position, List<BaseCard> cards, object image)
        {
            InitializeComponent();
            Balance = balance;
            IdPlayer = idPlayer;
            Image = image;
            NamePlayer = name;
            _position = position;
            Cards = cards;
            

        }
        /// <summary>
        /// Instancie un Player avec le nom et l'id, avec tous les autres paramètres ( Balance, Position, Cartes) vide ou nul.
        /// </summary>
        /// <param name="idPlayer"> Id du Player </param>
        /// <param name="name"> Nom du Player </param>
        public Player(int idPlayer, string name)
        {
            InitializeComponent();
            IdPlayer = idPlayer;
            NamePlayer = name;
            Balance = 0;
            _position = 0;
            Cards = new List<BaseCard>();
            Image = null;

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
            if (Balance < 0)
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

            if (amount > 0 && Balance > amount)
            {
                Balance -= amount;


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
                Balance += amount;

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
            string res = "Id du joueur : " + IdPlayer + "; Nom du joueur : " + Name + "; Argent : " + Balance + "; Position sur le plateau : " + Position + "; Liste de cartes : { ";
            Console.WriteLine(this.IdPlayer);
            for (int i = 0; i < Cards.Count - 1; i++)
            {
                res += Cards[i].ToString() + " , ";
            }
            if (Cards.Count > 0)
            {
                res += Cards[Cards.Count - 1].ToString() + " }";
            }
            return res;
        }
        #endregion


    }
}
