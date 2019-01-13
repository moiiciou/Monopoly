using Monopoly.Model.Card;
using System.ComponentModel;
using System.IO;

namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour PropertyCase.xaml
    /// </summary>
    public partial class PropertyCase : BaseCase
    {

        public PropertyCard Card { get; set; }
        public string Location { get; set; }
        public int IdOwner { get; set; } = 0;


        public PropertyCase(string location, int price, string skinPath, string color, int angle, int[] position, PropertyCard card)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, Location = location, Price = price.ToString() + "€", Color = color, Rotation = angle };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", Location = location, Price = price.ToString() + "€", Color = color, Rotation = angle };

            }
            Position = position;
            Card = card;
            Location = location;
        }

        public class CaseInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

            public string ImageTemplate { get; set; }
            public string Location { get; set; }
            public string Price { get; set; }
            public string Color { get; set; }
            public int Rotation { get; set; }
            public int IdOwner { get; set; } = 0; //By default, owner is bank(IdOwner : 0)


        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            toolTip.Content = Card;
        }
    }
}
