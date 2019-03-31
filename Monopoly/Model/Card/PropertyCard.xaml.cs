using server.Model;
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
    public partial class PropertyCard : BaseCard
    {
        public PropertyInfo CardInformation { get; set; }
        public PropertyCard(string propertyName, int houseCost, int hotelCost, int rentWith1house, int rentWith2house, int rentWith3house, int rentWith4house, int rentWithHotel, int rentValue, int mortgageValue, string color, int angle)
        {

            InitializeComponent();
            CardInformation = new PropertyInfo {
                Location = propertyName,
                TextHouseCost = houseCost.ToString() + "€ each",
                TextHotelCost = hotelCost.ToString() + "€ plus 4 houses",
                TextRentWith1House = rentWith1house.ToString() + "€",
                TextRentWith2House = rentWith2house.ToString() + "€",
                TextRentWith3House = rentWith3house.ToString() + "€",
                TextRentWith4House = rentWith4house.ToString() + "€",
                TextRentWithHotel = rentWithHotel.ToString() + "€",
                TextRentValue = "Rent :" + rentValue.ToString() + "€",
                TextMortgageValue = mortgageValue.ToString() + "€",
                Rent = rentValue,
                RentWith1House = rentWith1house,
                RentWith2House = rentWith2house,
                RentWith3House = rentWith3house,
                RentWith4House = rentWith4house,
                RentWithHotel = rentWithHotel,
                Color = color,
                HouseCost = houseCost,
                HostelCost = hotelCost,
                Rotation = angle
            };
            DataContext = CardInformation;
        }

    }

}
