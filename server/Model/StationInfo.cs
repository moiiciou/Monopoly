using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    class StationInfo : CaseInfo
    {
        public string Owner { get; set; }
      
        public int Price { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle)
        {

            this.ImageTemplate = skinPath;
            Price = price;
            TextLabel = text;
            this.Rotation = angle;
        }
        public StationInfo()
        {

        }

    }
}
