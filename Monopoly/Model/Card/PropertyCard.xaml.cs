using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly.Model.Card
{
    /// <summary>
    /// Logique d'interaction pour PropertyCard.xaml
    /// </summary>
    public partial class PropertyCard : UserControl
    {
        public PropertyCard(string propertyName, int houseCost, int hotelCost, int rentWith1house, int rentWith2house, int rentWith3house, int rentWith4house,int rentWithHotel, int rentValue, int mortgageValue, string color, int angle)
        {
            InitializeComponent();
            this.DataContext = new CardInfo { TextPropertyName = propertyName, TextHouseCost = houseCost.ToString() +"€ each", TextHotelCost = hotelCost.ToString() + "€ plus 4 houses", TextRentWith1House = rentWith1house.ToString() + "€", TextRentWith2House = rentWith2house.ToString() + "€", TextRentWith3House = rentWith3house.ToString() + "€", TextRentWith4House = rentWith4house.ToString() + "€", TextRentWithHotel = rentWithHotel.ToString() + "€", TextRentValue = "Rent :"+ rentValue.ToString() + "€", TextMortgageValue = mortgageValue.ToString() + "€",  Color = color };

        }
    }

    public class CardInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public string TextPropertyName { get; set; }
        public string TextHouseCost { get; set; }
        public string TextHotelCost { get; set; }

        public string TextRentWith1House { get; set; }
        public string TextRentWith2House { get; set; }
        public string TextRentWith3House { get; set; }
        public string TextRentWith4House { get; set; }
        public string TextRentWithHotel { get; set; }

        public string TextRentValue { get; set; }
        public string TextMortgageValue { get; set; }

        
        public string Color { get; set; }


    }
}
