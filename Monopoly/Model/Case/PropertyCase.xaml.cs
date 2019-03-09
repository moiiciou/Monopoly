using Monopoly.Model.Card;
using System;
using server;
using System.IO;
using System.Windows.Controls;
using Monopoly.Controller;

namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour PropertyCase.xaml
    /// </summary>
    public partial class PropertyCase : BaseCase
    {

        public PropertyCard Card { get; set; }
        public CaseInfo CaseInformation { get; set; }


        public PropertyCase(string location, int price, string skinPath, string color, int angle, int[] position, PropertyCard card , string owner = null)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                CaseInformation = new CaseInfo { ImageTemplate = skinPath, Location = location, Price = price, Color = color, Rotation = angle, Owner = owner, TxtPrice = price.ToString() + "€"  };

            }
            else
            {
                CaseInformation = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", Location = location, Price = price, Color = color, Rotation = angle, Owner = owner, TxtPrice = price.ToString() + "€" };

            }
            Card = card;
            DataContext = CaseInformation;
            Position = position;
            CaseInformation.Rent = Card.CardInformation.RentValue;
        }



        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            toolTip.Content = Card ;
        }

        private void BaseCase_IsHitTestVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("ok");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(CaseInformation.Owner == null)
            {
                System.Windows.MessageBox.Show("Cette propriété n'appartient à personne !");

            }
            else
            {
                if( PlayerManager.CurrentPlayerName.Trim('0') == CaseInformation.Owner)
                {
                    System.Windows.MessageBox.Show("Cette propriété vous appartient !"+ Environment.NewLine + "Le loyer est de " + BuyAndSellManager.CalculRent(this) + "€");

                }
                else
                {
                    System.Windows.MessageBox.Show("Cette propriété appartient à " + CaseInformation.Owner + Environment.NewLine + "Vous arretez içi vous coûteras " + BuyAndSellManager.CalculRent(this) + "€");

                }
            }
        }
    }
}
