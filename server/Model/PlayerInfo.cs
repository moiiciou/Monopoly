using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class PlayerInfo 
    {

        public string Pseudo { get; set; }
        public int Position { get; set; }
        public int Balance { get; set; }
        public string ColorCode;
        public List<CaseInfo> Estates { get; set; }
        public object Image { get; set; }
    }
    
}
