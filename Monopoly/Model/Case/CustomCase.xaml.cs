using server.Model;
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

        public CustomCase(string text, int amount, string skinPath, int[] position, int angle)
        {
            InitializeComponent();
            DataContext = new CustomInfo(text,amount, skinPath, angle,0);
            Position = position;
        }

    }
}
