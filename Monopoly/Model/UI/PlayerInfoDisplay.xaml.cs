using System.Windows.Controls;


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
        public string ImagePath { get; set; }

        public PlayerInfoDisplay(string pseudo, int balance, string image)
        {
            InitializeComponent();
            Pseudo = pseudo;
            Balance = balance;
            labelPseudo.Content = Pseudo;
            labelBalance.Content = Balance.ToString() + " €";
            ImagePath = "/Monopoly;component/ressources/templates/default/avatar/"+image.Trim('#')+".png";
            DataContext = this;
        }

    }
}
