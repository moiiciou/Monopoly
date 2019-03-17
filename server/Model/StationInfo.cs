using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    class StationInfo : CaseInfo
    {
       
        public string TextLabel { get; set; }
        public string Price { get; set; }
        public int Rotation { get; set; }
    }
}
