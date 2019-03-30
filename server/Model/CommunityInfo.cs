using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class CommunityInfo : CaseInfo
    {
        public enum TypeAction {
            reparation =1,
            paiement = 2,
            moove =3,
            freefromjail = 4
        };

        public enum TypeCard
        {
            community = 1,
            chance = 2,
           
        };

        public TypeAction typeCard;
        public TypeCard type;

        public int value;

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
