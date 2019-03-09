using Monopoly.Model.Card;
using System;
using server;
using System.IO;

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
                CaseInformation = new CaseInfo { ImageTemplate = skinPath, Location = location, Price = price, Color = color, Rotation = angle, Owner = owner };

            }
            else
            {
                CaseInformation = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", Location = location, Price = price, Color = color, Rotation = angle, Owner = owner };

            }
            Card = card;
            DataContext = CaseInformation;
            Position = position;
            CaseInformation.Rent = Card.CardInformation.RentValue;
        }



        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            toolTip.Content = Card;
        }

        private void BaseCase_IsHitTestVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("ok");
        }
    }
}
