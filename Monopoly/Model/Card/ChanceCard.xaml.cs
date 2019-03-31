using System.Windows.Controls;
using server;

namespace Monopoly.Model.Card
{
    /// <summary>
    /// Logique d'interaction pour ChanceCard.xaml
    /// </summary>
    public partial class ChanceCard : BaseCard
    {
        public ChanceCard(string titre, string text, string effect)
        {

            InitializeComponent();
            this.DataContext = new CardInfo { Label = titre, Text = text, Effect = effect };
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);           
            Board.Board.GetBoard.BoardLabel.Content = "";
        }
    }

}
