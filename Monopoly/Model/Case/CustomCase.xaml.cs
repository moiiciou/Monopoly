using System.ComponentModel;
using System.IO;
using System.Windows.Controls;


namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour CustomCase.xaml
    /// </summary>
    public partial class CustomCase : BaseCase
    {
        public int Amount;

        public CustomCase(string text, int amount, string skinPath, int[] position, int angle)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, TextLabel = text, Rotation = angle };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", TextLabel = text, Rotation = angle };

            }

            Amount = amount;
            this.Position = position;
        }

        public class CaseInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

            public string ImageTemplate { get; set; }
            public string TextLabel { get; set; }
            public int Rotation { get; set; }


        }
    }
}
