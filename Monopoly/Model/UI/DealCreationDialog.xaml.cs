using Monopoly.Controller;
using Monopoly.Model.Case;
using server;
using server.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour DealCreationDialog.xaml
    /// </summary>
    public partial class DealCreationDialog : UserControl
    {
        public DealCreationDialog()
        {
            InitializeComponent();

            PlayerInfo player = PlayerManager.GetPlayerByPseuso(PlayerManager.CurrentPlayerName.Trim('0'));
            foreach (PropertyInfo property in player.Properties)
            {

                PropertyInfo propertyInfo = (PropertyInfo)property;
                PropertyCase propertyCase = Core.Tools.GetPropertyByName(propertyInfo.Location);


                PropertyInfo cardInfo = propertyCase.Card.CardInformation;

                if (!property_list.Items.Cast<PropertyInfo>().Any(x => x.Location == cardInfo.Location))
                {
                    property_list.Items.Add(cardInfo);

                }
            }
        }
    }
}
