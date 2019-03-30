using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server.Model;
namespace server.Core
{
    public static class RentManager
    {
        public static int computeRent(PropertyInfo propRent, PlayerInfo p, ThemeParser tp)
        {
            int rent = 0;
            if (propRent.Owner != null && propRent.Owner != "" && propRent.Owner != p.Pseudo && !propRent.isMortgaged) // on compute le rent si le player et l'owner de la propriété sont différents et que la propriété n'est pas possédée par la banque.
                switch (propRent.NumberOfHouse)
                {
                    case 0:
                        if (OwnAllPropertiesOfColor(propRent, p, tp))
                            rent += 2 * propRent.Rent;
                        else
                            rent += propRent.Rent;
                        break;
                    case 1: rent += propRent.RentWith1House; break;
                    case 2: rent += propRent.RentWith2House; break;
                    case 3: rent += propRent.RentWith3House; break;
                    case 4:
                        if (propRent.HasHostel)
                            rent += propRent.RentWithHotel;
                        else
                        {
                            rent += propRent.RentWith4House;
                        }
                        break;

                }

            return rent;

        }

        public static bool OwnAllPropertiesOfColor(PropertyInfo pr, PlayerInfo pl, ThemeParser tp)
    {
            int compteur = 0;
            int compteurOwner = 0;
            foreach (PropertyInfo p in tp.CasesList)
            {

                if (p.Color == pr.Color)
                {
                    compteur++;
                    if (p.Owner == pr.Owner)
                        compteurOwner++;
                    else
                    {
                        return false;
                    }
                }


            }
            return compteur == compteurOwner;
    }

        // TO DO : Implémenter le prix de base des stations.

        /* public static int computeRent(StationInfo propRent, PlayerInfo p) 
         {
             int rent = 0;
             if (propRent.Owner != null && propRent.Owner != "" && propRent.Owner != p.Pseudo)
                 switch (propRent.)
                 {
                     case 0: rent += propRent.Rent; break;
                     case 1: rent += propRent.RentWith1House; break;
                     case 2: rent += propRent.RentWith2House; break;
                     case 3: rent += propRent.RentWith3House; break;
                     case 4:
                         if (propRent.HasHostel)
                             rent += propRent.RentWithHotel;
                         else
                         {
                             rent += propRent.RentWith4House;
                         }
                         break;

                 }

             return rent;

         }*/
    }
}
