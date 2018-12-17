using System.ComponentModel;


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

 
    public class CardInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public string Label { get; set; }
        public string Text { get; set; }
        public string Effect { get; set; }



    }
    }

}
