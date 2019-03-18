using System;
using System.Collections.Generic;

namespace server
{
    public sealed class GameData
    {
        public List<PlayerInfo> PlayerList { get; set;}

        public List<CaseInfo> BoardCaseInfo { get; set; }

        public List<CardInfo> ChanceDeck { get; set; }

        public List<CardInfo> CommunityDeck { get; set; }

        public string CurrentPlayerTurn;

        private static readonly Lazy<GameData> lazy = new Lazy<GameData> (() => new GameData());

        public static GameData GetGameData { get { return lazy.Value; } }

        private GameData()
        {
           
         
            PlayerList = new List<PlayerInfo>();
            BoardCaseInfo = new List<CaseInfo>();
            ChanceDeck = new List<CardInfo>();
            CommunityDeck = new List<CardInfo>();
            CurrentPlayerTurn = "";

        }
    }
}