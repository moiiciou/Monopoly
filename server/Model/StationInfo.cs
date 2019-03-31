using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class StationInfo : CaseInfo
    {
        public bool isMortaged { get; set; }
        public string Owner { get; set; }
        public int Price { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle, int posPlateau)
        {
            positionPlateau = posPlateau;

            this.ImageTemplate = skinPath;

            TextLabel = text;
            this.Rotation = angle;
            isMortaged = false;
            this.Price = price;
            this.Owner = null;
        }
        public StationInfo()
        {
            Owner = null;
            Price = 0;
            isMortaged = false;
        }
    }
}
