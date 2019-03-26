﻿using server.Model;
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
        public bool isInJail { get; set; }
        public List<PropertyInfo> Properties { get; set; }
        public List<StationInfo> Stations {get; set;}
        public object Image { get; set; }

        public PlayerInfo()
        {
            Pseudo = "Pseudo_ERROR";
            Balance = 0;
            isInJail = false;
            Properties = new List<PropertyInfo>();
            Stations = new List<StationInfo>();
        }
    }
    
}
