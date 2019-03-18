using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class PropertyInfo : CaseInfo
    {
 
        public int Price { get; set; }
 
        public int NumberOfHouse { get; set; }

        public bool HasHostel { get; set; }

        public string Owner { get; set; }
        public string TxtPrice { get; set; }

        public string Location { get; set; }
        public string TextHouseCost { get; set; }
        public string TextHotelCost { get; set; }

        public string TextRentWith1House { get; set; }
        public string TextRentWith2House { get; set; }
        public string TextRentWith3House { get; set; }
        public string TextRentWith4House { get; set; }
        public string TextRentWithHotel { get; set; }

        public int Rent { get; set; }
        public int HouseCost { get; set; }
        public int HostelCost { get; set; }


        public int RentWith1House { get; set; }
        public int RentWith2House { get; set; }
        public int RentWith3House { get; set; }
        public int RentWith4House { get; set; }
        public int RentWithHotel { get; set; }

        public string TextRentValue { get; set; }

        public string TextMortgageValue { get; set; }


        public string Color { get; set; }

        public PropertyInfo(string propertyName, int houseCost, int hotelCost, int rentWith1house, int rentWith2house, int rentWith3house, int rentWith4house, int rentWithHotel, int rentValue, int mortgageValue,  string color, int angle, int price)
        {
            Price = price;
            TxtPrice = price.ToString() + "€";
            Location = propertyName;

            TextHouseCost = houseCost.ToString() + "€ each";
            TextHotelCost = hotelCost.ToString() + "€ plus 4 houses";

            TextRentWith1House = rentWith1house.ToString() + "€";
            TextRentWith2House = rentWith2house.ToString() + "€";
            TextRentWith3House = rentWith3house.ToString() + "€";
            TextRentWith4House = rentWith4house.ToString() + "€";
            TextRentWithHotel = rentWithHotel.ToString() + "€";

            TextRentValue = "Rent :" + rentValue.ToString() + "€";
            TextMortgageValue = mortgageValue.ToString() + "€";

            Rent = rentValue;

            RentWith1House = rentWith1house;
            RentWith2House = rentWith2house;
            RentWith3House = rentWith3house;
            RentWith4House = rentWith4house;
            RentWithHotel = rentWithHotel;

            Color = color;

            HouseCost = houseCost;
            HostelCost = hotelCost;

            Owner = null;
            NumberOfHouse = 0;
            HasHostel = false;

            
           
        }

        public PropertyInfo()
        {

        }

    }
}
