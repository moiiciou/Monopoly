using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class StationInfo : CaseInfo
    {
        public bool isMortgaged { get; set; }
        public string Owner { get; set; }
        public int Price { get; set; }
        public int RentBase { get; set; }

        public string TextPrice { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle, int posPlateau)
        {
            RentBase = rent;
            positionPlateau = posPlateau;

            this.ImageTemplate = skinPath;
            TextPrice = price.ToString() + " €";
            TextLabel = text;
            this.Rotation = angle;
            isMortgaged = false;
            this.Price = price;
            this.Owner = null;
        }
        public StationInfo()
        {
            RentBase = 0;
            Owner = null;
            Price = 0;
            isMortgaged = false;
        }
    }
}
