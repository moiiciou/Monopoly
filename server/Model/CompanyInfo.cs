using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public partial class CompanyInfo : CustomInfo
    {
        public int multiply { get; set; }
        public int multiplyWith2Prop { get; set; }
        public bool isMortaged { get; set; }
        public int price { get; set; }
        public string owner { get; set; }

        public CompanyInfo(string text, int income, string skinPath, int angle, int posPlateau, int price) : base(text, income, skinPath, angle, posPlateau)
        {
            multiply = 4;
            multiplyWith2Prop = 10;
            this.price = price;
            isMortaged = false;
            owner = null;
        }
    }
}
