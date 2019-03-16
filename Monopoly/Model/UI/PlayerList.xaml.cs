using Monopoly.Controller;
using server;
using System.Collections.Generic;
using System.Windows.Controls;


namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerList.xaml
    /// </summary>
    public partial class PlayerList : UserControl
    {
        private List<PlayerInfo> playerInfos;

        public PlayerList()
        {
            InitializeComponent();
            playerInfos = GameManager.MonopolyGameData.PlayerList;
            DataContext = playerInfos;
        }
    }
}
