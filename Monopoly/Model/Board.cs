using Monopoly.Model.Case;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Monopoly.Model
{
    class Board : Grid
    {
        public Board()
        {
            LoadBoardInfo();
            Console.Write("TAILLE GRID : {0}", this.Children.Count);


        }

        public void LoadBoardInfo()
        {
            using (StreamReader r = File.OpenText("C:\\Users\\040-TPHILIPPS\\Pictures\\level.json"))
            {
                string json = r.ReadToEnd();
                List<BoardItem> items = JsonConvert.DeserializeObject<List<BoardItem>>(json);
                foreach (var item in items)
                {
                    this.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto });
                    this.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
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
            Button Test = new Button();
            Grid.SetRow(Test, 3);
            Grid.SetColumn(Test, 2);
            Test.SetValue(ColumnSpanProperty, 2);
            Test.Content = "Chance";
            RotateTransform rotateTransform1 = new RotateTransform(-45, 0, 0);
            Test.RenderTransform = rotateTransform1;

            this.Children.Add(Test);

            Button Test2 = new Button();
            Grid.SetRow(Test2, 8);
            Grid.SetColumn(Test2, 7);
            Test2.SetValue(ColumnSpanProperty, 2);
            Test2.Content = "Caisse de communauté";
            RotateTransform rotateTransform2 = new RotateTransform(-45, 0, 0);
            Test2.RenderTransform = rotateTransform2;

            this.Children.Add(Test2);

        }

        public class BoardItem
        {
            public int[] position;
            public string type;
            public Dictionary<string, object> caseAttributes = new Dictionary<string, object>();

        }

    }

}
