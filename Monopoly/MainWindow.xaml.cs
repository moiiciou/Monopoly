using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Monopoly.Model;
using Monopoly;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           /* Dictionary<string, Drawable> test = new Dictionary<string, Drawable>();
            //test.Add(((Button)sender).Name,new Drawable(10,"fdsgfd","sdfsf"));
            string nameButton=((Button)sender).Name;
            //Drawable testD;
            //test.TryGetValue(nameButton, out testD);
            ((Button)sender).Content = "coucou";
            
            Tests.test();
            */
        }

      
    }
}
