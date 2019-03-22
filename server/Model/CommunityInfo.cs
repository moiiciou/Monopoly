using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class CommunityInfo : CaseInfo
    {

        public CommunityInfo(string text, string skinPath, int angle, int posPlateau)
        {
            positionPlateau = posPlateau;
            this.Rotation = angle;
            TextLabel = text;
            ImageTemplate = skinPath;

         
        }
    }

    public class ChanceInfo : CaseInfo
    {

        public ChanceInfo(string text, string skinPath, int angle, int posPlateau)
        {
            positionPlateau = posPlateau;
            this.Rotation = angle;
            TextLabel = text;
            ImageTemplate = skinPath;


        }
    }


}
