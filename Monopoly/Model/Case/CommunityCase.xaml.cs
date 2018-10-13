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
    /// Logique d'interaction pour CommunityCase.xaml
    /// </summary>
    public partial class CommunityCase : UserControl
    {

        public CommunityCase(string communityName, string text, string skinPath)
        {
            InitializeComponent();
            if (File.Exists(skinPath))
            {
                this.DataContext = new CaseInfo { ImageTemplate = skinPath, TextLabel = text, CommunityName = communityName};

            }
            else
            {
                this.DataContext = new CaseInfo { ImageTemplate = "C:\\Users\\me\\Pictures\\error.png", TextLabel = text };

            }


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
            public string CommunityName { get; set; }


        }
    }
}
