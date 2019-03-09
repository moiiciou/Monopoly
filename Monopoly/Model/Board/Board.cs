using Monopoly.Core;
using Monopoly.Model.Card;
using Monopoly.Model.Case;
using System;
using System.Collections.Generic;
using System.Windows.Controls;



namespace Monopoly.Model.Board
{
    public class Board : BoardLayout
    {
        public  List<BaseCase> CasesList = new List<BaseCase>();
        private static readonly Lazy<Board> lazy = new Lazy<Board>(() => new Board());
        public List<ChanceCard> ChanceCardList = new List<ChanceCard>();

        public static Board GetBoard { get { return lazy.Value; } }


        protected Board()
        {
            LoadBoardInfo();


        }


        public void LoadBoardInfo()
        {
            ThemeParser template = new ThemeParser(".\\ressources\\level.json");

            List<BaseCase> CaseList = template.CasesList;
            ChanceCardList = template.ChanceList;

            foreach (BaseCase BaseCase in CaseList)
            {
                if (BaseCase is PropertyCase)
                {
                    PropertyCase Case = (PropertyCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is StartCase)
                {
                    StartCase Case = (StartCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is CustomCase)
                {
                    CustomCase Case = (CustomCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is CommunityCase)
                {
                    CommunityCase Case = (CommunityCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is ChanceCase)
                {
                    ChanceCase Case = (ChanceCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is StationCase)
                {
                    StationCase Case = (StationCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }

                if (BaseCase is JailCase)
                {
                    JailCase Case = (JailCase)BaseCase;

                    Grid.SetRow(Case, Case.Position[0]);
                    Grid.SetColumn(Case, Case.Position[1]);
                    this.Children.Add(Case);
                }
                CasesList.Add(BaseCase);
            }



        }
    }


}
