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
    public partial class PlayerInfoDisplay : UserControl
    {
        private int balance;
        public int Balance
        {

            get { return balance; }
            set
            {
                balance = value;
                labelBalance.Content = balance.ToString() + " €";

            }

        }
        public string Pseudo { get; set; }

        public PlayerInfoDisplay(string pseudo, int balance)
        {
            InitializeComponent();
            Pseudo = pseudo;
            Balance = balance;
            labelPseudo.Content = Pseudo;
            labelBalance.Content = Balance.ToString() + " €";
        }

    }
}
