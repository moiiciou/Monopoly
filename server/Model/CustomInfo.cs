using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public partial class CustomInfo : CaseInfo
    {
        public int Income;
        public CustomInfo(string text, int income, string skinPath, int angle, int posPlateau)
        {
            positionPlateau = posPlateau;

            Rotation = angle;
            Income = income;
            TextLabel = text;
            ImageTemplate = skinPath;


        }
      
    }
}
