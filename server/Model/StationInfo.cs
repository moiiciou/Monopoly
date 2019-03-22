using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class StationInfo : CaseInfo
    {

        public string Owner;
        public int Price { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle, int posPlateau)
        {
            positionPlateau = posPlateau;

            this.ImageTemplate = skinPath;

            TextLabel = text;
            this.Rotation = angle;
        }
        public StationInfo()
        {
            Owner = "";
            Price = 0;
        }
    }
}
