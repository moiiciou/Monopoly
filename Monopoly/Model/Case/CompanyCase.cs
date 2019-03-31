using server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model.Case
{
    public class CompanyCase:CustomCase
    {
        public CompanyInfo ComInfo { get; set; }
        public CompanyCase(string text, int amount, string skinPath, int[] position, int angle, int price) : base(text, amount, skinPath, position, angle)
        {
            ComInfo = new CompanyInfo(text, amount, skinPath, angle, 0, price);
            InitializeComponent();
            DataContext = ComInfo;
            Position = position;
        }
    }
}
