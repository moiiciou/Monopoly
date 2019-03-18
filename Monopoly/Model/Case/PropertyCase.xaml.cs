using Monopoly.Model.Card;
using System;
using server;
using System.IO;
using System.Windows.Controls;
using Monopoly.Controller;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using server.Model;

namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour PropertyCase.xaml
    /// </summary>
    public partial class PropertyCase : BaseCase
    {
        private static Action EmptyDelegate = delegate () { };



        public PropertyCard Card { get; set; }
        public PropertyInfo CaseInformation { get; set; }


        public PropertyCase(string location, int price, string skinPath, string color, int angle, int[] position, PropertyCard card , string owner = null)
        {
            InitializeComponent();
            CaseInformation = new PropertyInfo();
            CaseInformation.Price = price;
            CaseInformation.TxtPrice = price.ToString() + "€";
            CaseInformation.Location = location;
            CaseInformation.Color = color;
            CaseInformation.Owner = owner;
            Card = card;
            DataContext = CaseInformation;
            Position = position;
        }

        public void UpdateBackground()
        {
            var brush = new ImageBrush();

                string path = "";

                if (CaseInformation.NumberOfHouse == 1)
                {
                    path = "house";
                }
                if (CaseInformation.NumberOfHouse == 2)
                {
                    path = "house2";
                }
                if (CaseInformation.NumberOfHouse == 3)
                {
                    path = "house3";
                }
                if (CaseInformation.NumberOfHouse == 4)
                {
                    path = "house4";
                }
            if(CaseInformation.NumberOfHouse != 0)
            {
                brush.ImageSource = new BitmapImage(new Uri("ressources/templates/default/" + path + ".png", UriKind.Relative));
                buttonProperty.Background = brush;
                Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            }
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            toolTip.Content = Card ;
        }

        private void BaseCase_IsHitTestVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("ok");
        }

        private void buttonProperty_Click(object sender, System.Windows.RoutedEventArgs e)
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
