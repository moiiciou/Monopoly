using server.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour StationCase.xaml
    /// </summary>
    public partial class StationCase : BaseCase
    {

        public StationInfo CaseInformation { get; set; }

        public StationCase(string text, int price, string skinPath, int[] position, int angle)
        {
            InitializeComponent();
            CaseInformation = new StationInfo(text, price, skinPath, angle, 0, 0);
            DataContext = CaseInformation;
            Position = position;
        }

    }
}
