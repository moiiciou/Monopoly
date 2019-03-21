using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class CustomInfo : CaseInfo
    {
        public int Income;
        public string Owner;
        public CustomInfo(string text, int income, string skinPath, int angle)
        {
            Rotation = angle;
            Income = income;
            TextLabel = text;
            ImageTemplate = skinPath;


        }
        public CustomInfo()
        {

        }
    }
}
