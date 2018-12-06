using Monopoly.Core;
using Monopoly.Model.Case;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Monopoly.Model.Board
{
    class Board : BoardLayout
    {
        readonly List<BaseCase> CasesList = new List<BaseCase>();

        public Board()
        {
            LoadBoardInfo();
            var testdial = new UI.DialogueBox("Ceci est une box de text ! Elle est très belle ! Gloire à Satan et à bientôt !");
            testdial.SetValue(Grid.ColumnSpanProperty, 4);
            Grid.SetRow(testdial, 3);
            Grid.SetColumn(testdial, 3);
            this.Children.Add(testdial);

        }

        public void LoadBoardInfo()
        {
            ThemeParser template = new ThemeParser(".\\ressources\\level.json");

            List<BaseCase> CaseList = template.CasesList;

            foreach(BaseCase BaseCase in CaseList)
            {
                if(BaseCase is PropertyCase)
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
            }



        }
    }
        

}
