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

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerInfo.xaml
    /// </summary>
    public partial class PlayerInfo : UserControl
    {
        private int balance;

        public PlayerInfo()
        {
            InitializeComponent();
        }

        public PlayerInfo(string name, int balance)
        {
            Name = name;
            this.balance = balance;
        }
    }
}
