using Monopoly.Model.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Controller
{
    static class CaseManager
    {
        public static int GetPrisonCasePosition()
        {
            return 0;
        }

        public static int CheckOwner(BaseCase caseToCheck)
        {
            if (caseToCheck is PropertyCase)
            {
                PropertyCase Case = (PropertyCase)caseToCheck;

                return Case.IdOwner;
            }



            if (caseToCheck is StationCase)
            {
                StationCase Case = (StationCase)caseToCheck;

                return Case.IdOwner;
            }

            return -1;
        }

        public static string GetCaseName(BaseCase caseToCheck)
        {
            if (caseToCheck is PropertyCase)
            {
                PropertyCase Case = (PropertyCase)caseToCheck;

                return Case.Location;
            }



            if (caseToCheck is StationCase)
            {
                StationCase Case = (StationCase)caseToCheck;

                return Case.TextLabel;
            }

            return "ERROR_NO_NAME_FOUND";

        }
    }
}
