using System;
using System.Collections.Generic;

namespace server
{
    public sealed class GameData
    {
        public List<PlayerInfo> PlayerList { get; set;}

        public string CurrentPlayerTurn;

        private static readonly Lazy<GameData> lazy = new Lazy<GameData> (() => new GameData());

        public static GameData GetGameData { get { return lazy.Value; } }

        private GameData()
        {
           
         
            PlayerList = new List<PlayerInfo>();

            CurrentPlayerTurn = "";

        }
    }
}