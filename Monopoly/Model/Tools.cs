using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    class Tools
    {

        public static List<string> ConvertCollectionToList(System.Collections.Specialized.StringCollection collection)
        {
            List<string> list = new List<string>();
            foreach (string item in collection)
            {
                list.Add(item);
            }
            return list;
        }



    }
}
