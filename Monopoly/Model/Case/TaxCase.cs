using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model.Case
{
    class TaxCase : BoardCase
    {
        public int Price { get; set; }
        public TaxCase(string name, int[] coordinate, string skinPath) : base(name, coordinate, skinPath)
        {

        }
    }
}
