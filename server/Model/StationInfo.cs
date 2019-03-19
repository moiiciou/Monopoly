using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class StationInfo : CaseInfo
    {
       
      
        public string Price { get; set; }
        public StationInfo(string text, int price, string skinPath, int angle)
        {

            this.ImageTemplate = skinPath;

            TextLabel = text;
            this.Rotation = angle;
        }

    }
}
