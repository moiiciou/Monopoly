using Monopoly.Model.Card;
using Monopoly.Model.UI;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace Monopoly.Model
{
    /// <summary>
    /// Logique d'interaction pour Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {        
        #region Attributs

        private int _balance;
        private int _idPlayer;
        private object _image;
        private string _name;
        private int _position;
        private bool _canMoove;
        private int _numberDoubleDice;
        private List<BaseCard> _cards;
        public int grid;
        public PlayerInfoDisplay playerInfo;
        #endregion

        #region Accesseurs

        public int Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;

            }
        }

        public bool CanMoove
        {
            get
            {
                bool value = true;
                if (_numberDoubleDice > 3)
                {
                    value = false;
                }
                return value;
            }
        }
        public int IdPlayer
        {
            get
            {
                return _idPlayer;
            }
        }

        public object Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }


        }

        public string NamePlayer
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

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
                }
                else
                {
                    throw new ArgumentException("La valeur de la position ne peut pas être négative");
                }
            }
        }

        public int NumberDoubleDice
        {
            get
            {
                return _numberDoubleDice;
            }
            set
            {
                _numberDoubleDice = value;
            }
        }

        public List<BaseCard> Cards
        {
            get
            {
                return _cards;
            }

        }

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
            _balance = balance;
            _idPlayer = idPlayer;
            _image = image;
            _name = name;
            _position = position;
            _cards = cards;
            

        }
        /// <summary>
        /// Instancie un Player avec le nom et l'id, avec tous les autres paramètres ( Balance, Position, Cartes) vide ou nul.
        /// </summary>
        /// <param name="idPlayer"> Id du Player </param>
        /// <param name="name"> Nom du Player </param>
        public Player(int idPlayer, string name)
        {
            InitializeComponent();
            _idPlayer = idPlayer;
            _name = name;
            _balance = 0;
            _position = 0;
            _cards = new List<BaseCard>();
            _image = null;

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

        /// <summary>
        ///  On rajoute une carte au joueur courant.
        /// </summary>
        /// <param name="c"> La carte que l'on veut ajouter. </param>
        public void AddCard(BaseCard c)
        {
            if (c != null)
            {
                Cards.Add(c);
            }

        }
        /// <summary>
        ///  On déplace le joueur de dice1 + dice2 cases, si le joueur dépasse la case départ, on remet sa position à 0.
        /// </summary>
        /// <param name="dice1">Valeur comprise entre 1 et 6. Valeur du premier dé</param>
        /// <param name="dice2">Valeur comprise entre 1 et 6. Valeur du deuxième dé</param>
        public void Moove(int dice1, int dice2)
        {

            if (CanMoove && dice1 < 7 && dice1 > 0 && dice2 > 0 && dice2 < 7)
            {
                Position += dice1 + dice2;
            }
            else
            {
                throw new ArgumentException("Le joueur ne peut pas se déplacer ! ");
            }

        }

        /// <summary>
        ///  Calcule le string de description d'un joueur. 
        /// </summary>
        /// <returns> Renvoie le string de description de l'objet player. </returns>
        public override string ToString()
        {
            string res = "Id du joueur : " + IdPlayer + "; Nom du joueur : " + Name + "; Argent : " + Balance + "; Position sur le plateau : " + Position + "; Liste de cartes : { ";
            Console.WriteLine(this._idPlayer);
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
