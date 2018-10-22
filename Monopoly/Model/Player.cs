using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    public class Player
    {
        #region Attributs

      //  private int _balance;
      //  private int _idPlayer;
       // private object _image;
       // private string _name;
        private int _position;


        public int Balance
        {
            get;set;
        }

        public int IdPlayer
        {
            get;           
        }

        public object Image
        {
            get; set;
           
        }

        public string Name
        {
            get;
            set;
        }

        public int Position 
        {
            get
            {
                return _position;
            }
        
            set
            {
                if(value>=0)
                {
                    _position = value;
                }
                else
                {
                    throw new ArgumentException("La valeur de la position ne peut pas être négative");
                }
             }
        }

        public List<Card> Cards
        {
            get; set;
        }

        #endregion

        #region Constructeurs

        public Player(int idPlayer, string name, int balance, int position, List<Card> cards, object image)
        {
            Balance = balance;
            IdPlayer = idPlayer;
            Image = image;
            Name = name;
            Position = position;
            Cards = cards;
        }

        public Player(int idJoueur,string name)
        {
           
            new Player(idJoueur,name , 0, 0, new List<Card>(), null);
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
            if(Balance<0){
                result=true;
            }
            return result;
        }

        /// <summary>
        ///  On retire de l'argent au joueur (dans le cas d'un paiement pour autre joueur par exemple).
        ///  Le montant ne peut pas être négatif.
        /// </summary>
        /// <param name="amount"> Montant à retirer au joueur courant. </param>
        public void RemoveAmount(int amount)
        {
         
            if (amount > 0 && Balance > amount)
            {
                Balance -= amount;

            }else
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
        public void AddCard(Card c)
        {
            if(c != null)
            {
                Cards.Add(c);
            }
           
        }

        public override string ToString()
        {
            string res = "Id du joueur : " + IdPlayer + "; Nom du joueur : " + Name + "; Argent : " + Balance + "; Position sur le plateau : " + Position + "; Liste de cartes : {";
            foreach (Card c in Cards)
            {
                res += c.ToString()+ " , ";
            }
            res += "}";
            return res;
        }
        #endregion
    }
}
