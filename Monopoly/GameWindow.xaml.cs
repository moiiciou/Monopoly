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
using System.Windows.Shapes;
using Monopoly.Properties;
using Monopoly.Model;
using System.ComponentModel;
using Monopoly.Model.Case;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            this.Width = Settings.Default.Width;
            this.Height = Settings.Default.Height;
            //Board board = new Board("default");
            StartCase test = new StartCase("GO !",100, "C:\\Users\\me\\Pictures\\test.png");
            root.Children.Add(test);
        

        }

    }
}

  

