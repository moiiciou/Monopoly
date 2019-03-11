using System.ComponentModel;
using System.Windows.Controls;

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


        public class CardInfo
        {
            public string Label { get; set; }
            public string Text { get; set; }
            public string Effect { get; set; }

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);

        }
    }

}
