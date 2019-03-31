using Monopoly.Model.Card;
using Monopoly.Model.Case;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace Monopoly.Core
{
    class ThemeParser
    {

        public List<BaseCase> CasesList = new List<BaseCase>();
        public List<UserControl> CommunityList = new List<UserControl>();
        public List<ChanceCard> ChanceList = new List<ChanceCard>();
        public List<UserControl> PropertyCardList = new List<UserControl>();

        public ThemeParser(string file)
        {
            using (StreamReader r = File.OpenText(file))
            {
                string json = r.ReadToEnd();
                GameElement items = JsonConvert.DeserializeObject<GameElement>(json);

                foreach (var item in items.Chance)
                {
                    ChanceCard Card = new ChanceCard(item.title, item.text, item.effect);
                    ChanceList.Add(Card);
                }

                foreach (var item in items.Case)
                {
                    int angle = 0;
                    if (item.position[0] > 0 && item.position[1] == 0 && item.position[0] < 10)
                    {
                        angle = 90;
                    }
                    if (item.position[1] == 10 && item.position[0] > 0 && item.position[0] < 10)
                    {
                        angle = -90;
                    }
                    switch (item.type)
                    {
                        case "property":
                            PropertyCard CardProperty = new PropertyCard(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["houseCost"]), Convert.ToInt16(item.caseAttributes["houseCost"]), Convert.ToInt16(item.caseAttributes["rentWith1House"]), Convert.ToInt16(item.caseAttributes["rentWith2House"]), Convert.ToInt16(item.caseAttributes["rentWith3House"]), Convert.ToInt16(item.caseAttributes["rentWith4House"]), Convert.ToInt16(item.caseAttributes["rentWithHostel"]), Convert.ToInt16(item.caseAttributes["rent"]), Convert.ToInt16(item.caseAttributes["mortgageValue"]), item.caseAttributes["color"].ToString(), angle);
                            PropertyCase Property = new PropertyCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), item.caseAttributes["skin"].ToString(), item.caseAttributes["color"].ToString(), angle, item.position, CardProperty);
                            CasesList.Add(Property);
                            PropertyCardList.Add(CardProperty);
                            break;

                        case "start":
                            StartCase Start = new StartCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString(), item.position);
                            CasesList.Add(Start);
                            break;

                        case "custom":
                            CustomCase Custom = new CustomCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString(), item.position, angle);
                            CasesList.Add(Custom);
                            break;

                        case "chance":
                            ChanceCase Chance = new ChanceCase("Chance", "", angle, item.position);
                            CasesList.Add(Chance);

                            break;

                        case "community":
                            CommunityCase Com = new CommunityCase("Caisse de Communauté", "", angle, item.position);
                            CasesList.Add(Com);

                            break;

                        case "station":
                            StationCase Station = new StationCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), item.caseAttributes["skin"].ToString(), item.position, angle);
                            CasesList.Add(Station);

                            break;

                        case "jail":
                            JailCase Jail = new JailCase(item.caseAttributes["label"].ToString(), item.caseAttributes["skin"].ToString(), item.position);
                            CasesList.Add(Jail);
                            break;

                        case "company":
                            CustomCase company = new CustomCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), "", item.position, angle);
                            CasesList.Add(company);
                            break;

                        default:
                            Console.WriteLine("Error parsing case");
                            break;
                    }
                }

                foreach (var item in items.Community)
                {

                }
            }


        }
    }

    class GameElement
    {
        public List<CaseInfo> Case { get; set; }
        public List<CardInfo> Chance { get; set; }
        public List<CardInfo> Community { get; set; }
    }

    class CaseInfo
    {
        public int[] position { get; set; }
        public string type { get; set; }
        public Dictionary<string, object> caseAttributes = new Dictionary<string, object>();
    }

    class CardInfo
    {
        public string title { get; set; }
        public string text { get; set; }
        public string effect { get; set; }
    }
}