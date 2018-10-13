using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model.Case
{
    class PropertyCase : BoardCase
    {
        public int Price { get; set; }
        
        
        public PropertyCase(string name, int[] coordinate, string skinPath, int price) : base(name, coordinate, skinPath)
        {
            Price = price;

        }
    }
}
