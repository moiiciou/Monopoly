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
    public class Board : BoardLayout
    {
        public List<BoardItem> BoardItems;
        public Board()
        {
            LoadBoardInfo();
        }


        public void LoadBoardInfo()
        {
            using (StreamReader r = File.OpenText(".\\ressources\\level.json"))
            {
                string json = r.ReadToEnd();
                List<BoardItem> items = JsonConvert.DeserializeObject<List<BoardItem>>(json);
                BoardItems = items;

                foreach (var item in items)
                {

                    int angle = 0;
                    switch (item.type)
                    {
                        case "Property":
                            if (item.position[0] > 0 && item.position[1] == 0 && item.position[0] < 10 )
                            {
                                angle = 90;
                            }
                            if (item.position[1] == 10 && item.position[0] > 0 && item.position[0] < 10)
                            {
                                angle = -90;
                            }
                            PropertyCase Property = new PropertyCase(item.caseAttributes["name"].ToString(), Convert.ToInt16(item.caseAttributes["price"]), item.caseAttributes["skin"].ToString(), item.caseAttributes["color"].ToString(), angle);
                            Grid.SetRow(Property, item.position[0]);
                            Grid.SetColumn(Property, item.position[1]);
                            this.Children.Add(Property);
                            break;

                        case "Start":
                            StartCase Start = new StartCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString());
                            Grid.SetRow(Start, item.position[0]);
                            Grid.SetColumn(Start, item.position[1]);
                            this.Children.Add(Start);
                            break;

                        default:
                            Console.WriteLine("Default case");
                            break;
                    }

                }

            }
             
        }

        public class BoardItem
        {
            public int[] position;
            public string type;
            public Dictionary<string, object> caseAttributes = new Dictionary<string, object>();

        }


    }

}
