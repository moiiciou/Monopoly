using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class StartInfo : CaseInfo
    {
        public int Income;
        public StartInfo(string text, int income, string skinPath, int posPlateau)
        {
            positionPlateau = posPlateau;


            Income = income;
            TextLabel = text;
            ImageTemplate = skinPath;


        }
        
    }

    public class JailInfo : CaseInfo
    {

        public JailInfo(string text, string skinPath, int posPlateau)
        {
            positionPlateau = posPlateau;

            TextLabel = text;
            ImageTemplate = skinPath;


        }

    }
}
