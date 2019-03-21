
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using server.Model;

namespace server
{
    public  class ThemeParser
    {

        public List<PropertyInfo> CasesList = new List<PropertyInfo>();
        public List<StationInfo> StationList = new List<StationInfo>();
        public List<CustomInfo> CustomInfo = new List<CustomInfo>();
        public List<CaseInfo> restinfo = new List<CaseInfo>();
       // public List<UserControl> CommunityList = new List<UserControl>();
       // public List<ChanceCard> ChanceList = new List<ChanceCard>();
       // public List<UserControl> PropertyCardList = new List<UserControl>();

        public ThemeParser(string file)
        {
            using (StreamReader r = File.OpenText(file))
            {
                string json = r.ReadToEnd();
                GameElement items = JsonConvert.DeserializeObject<GameElement>(json);

              /*  foreach (var item in items.Chance)
                {
                    ChanceCard Card = new ChanceCard(item.title, item.text, item.effect);
                    ChanceList.Add(Card);
                }*/

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
                            PropertyInfo propertyInfo = new PropertyInfo(
                                item.caseAttributes["label"].ToString(),
                                Convert.ToInt16(item.caseAttributes["houseCost"]),
                                Convert.ToInt16(item.caseAttributes["houseCost"]), 
                                Convert.ToInt16(item.caseAttributes["rentWith1House"]),
                                Convert.ToInt16(item.caseAttributes["rentWith2House"]), 
                                Convert.ToInt16(item.caseAttributes["rentWith3House"]),
                                Convert.ToInt16(item.caseAttributes["rentWith4House"]),
                                Convert.ToInt16(item.caseAttributes["rentWithHostel"]), 
                                Convert.ToInt16(item.caseAttributes["rent"]), 
                                Convert.ToInt16(item.caseAttributes["mortgageValue"]), 
                                item.caseAttributes["color"].ToString(), angle, 
                                Convert.ToInt16(item.caseAttributes["price"]));
                            // CaseInfo Property = new PropertyCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), item.caseAttributes["skin"].ToString(), item.caseAttributes["color"].ToString(), angle, item.position, CardProperty);
                            //CasesList.Add(Property);

                            CasesList.Add(propertyInfo);
                            //PropertyCardList.Add(CardProperty);
                            break;

                        case "start":
                            StartInfo Start = new StartInfo(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString());
                            restinfo.Add(Start);
                            break;

                        case "custom":
                            CustomInfo Custom = new CustomInfo(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString(), angle);
                            CustomInfo.Add(Custom);
                            break;

                        case "chance":
                            ChanceInfo Chance = new ChanceInfo("Chance", "", angle);
                            restinfo.Add(Chance);

                            break;

                        case "community":
                            CommunityInfo Com = new CommunityInfo("Caisse de Communauté", "", angle);
                            restinfo.Add(Com);

                            break;

                        case "station":
                            StationInfo Station = new StationInfo(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), item.caseAttributes["skin"].ToString(), angle);
                            StationList.Add(Station);

                            break;

                        case "jail":
                            JailInfo Jail = new JailInfo(item.caseAttributes["label"].ToString(), item.caseAttributes["skin"].ToString());
                            restinfo.Add(Jail);
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
        public PropertyInfo searchCaseProperty(string textlbl)
        {
            foreach (PropertyInfo c in CasesList)
            {
                if (c.Location == textlbl)
                    return c;
            }

            return null;
        }

        public int searchIndexProperty(string textlbl)
        {
            int compteur = 0;
            foreach (PropertyInfo c in CasesList)
            {
                if (c.Location == textlbl)
                    return compteur;
                else compteur++;
            }

            return -1;
        }

        public class CaseInfoJson
        {
            public int[] position { get; set; }
            public string type { get; set; }
            public Dictionary<string, object> caseAttributes = new Dictionary<string, object>();

        }

       public class GameElement
        {
            public List<CaseInfoJson> Case { get; set; }
            public List<CardInfoJson> Chance { get; set; }
            public List<CardInfoJson> Community { get; set; }
        }

       public class CardInfoJson
        {
            public string title { get; set; }
            public string text { get; set; }
            public string effect { get; set; }
        }
    }



    /**/


                    }