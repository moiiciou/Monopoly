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
    /// Logique d'interaction pour JailCase.xaml
    /// </summary>
    public partial class JailCase : UserControl
    {
        public JailCase(string text, string skinPath)
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
