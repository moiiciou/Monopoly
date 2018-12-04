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
        readonly List<UserControl> CasesList = new List<UserControl>();

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
            using (StreamReader r = File.OpenText(".\\ressources\\level.json"))
            {
                string json = r.ReadToEnd();
                List<BoardItem> items = JsonConvert.DeserializeObject<List<BoardItem>>(json);
                
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
                            CasesList.Add(Property);
                            break;

                        case "Start":
                            StartCase Start = new StartCase(item.caseAttributes["label"].ToString(), Convert.ToInt16(item.caseAttributes["income"]), item.caseAttributes["skin"].ToString());
                            Grid.SetRow(Start, item.position[0]);
                            Grid.SetColumn(Start, item.position[1]);
                            this.Children.Add(Start);
                            CasesList.Add(Start);
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


        /// <summary>
        /// return the cases in the board
        /// </summary>
        /// <returns> cases in board </returns>
         List<UserControl> GetCasesList()
        {
            return CasesList;
        }


        //static getCaseInfo(idCase) : retourn le type de la case et ses info

        // static isPropertyAvailable(idCase) : return true or false si la proprieté peux etre acheter



    }

}
