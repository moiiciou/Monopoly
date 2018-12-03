using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monopoly.Model
{
    static class  Tools
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
        private static readonly Action EmptyDelegate = delegate { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, EmptyDelegate);
        }




    }
}
