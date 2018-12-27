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


        public static int GetFontSize()
        {
            if(((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualWidth < 1600)
            {
                return 12;
            }
            else
            {
                return 24;
            }
        }

    }
}
