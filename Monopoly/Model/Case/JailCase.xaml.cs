using System.ComponentModel;
using System.IO;
using System.Windows.Controls;


namespace Monopoly.Model.Case
{
    /// <summary>
    /// Logique d'interaction pour JailCase.xaml
    /// </summary>
    public partial class JailCase : BaseCase
    {

        public JailCase(string text, string skinPath, int[] position)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, TextLabel = text };

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", TextLabel = text };

            }

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


        }
    }
}
