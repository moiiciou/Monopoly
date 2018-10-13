using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopoly.Properties;
using Monopoly.Model.Case;
using System.Windows.Controls;

namespace Monopoly.Model
{
    class Board : Grid
    {
        public string LayoutFile { get; set; }
        public Board(string layoutFile)
        {
            LayoutFile = layoutFile;
            // genere la grille du board
            for (int i = 0; i < Settings.Default.GrideSize; i++)
            {
                this.RowDefinitions.Add(new RowDefinition());
                this.ColumnDefinitions.Add(new ColumnDefinition());
                BoardCase test = new BoardCase("test", new int[] { i, i }, "C:\\Users\\me\\Pictures\\test.png");


                Grid.SetRow(test, i);
                Grid.SetColumn(test, i);
                this.Children.Add(test);

            }

            /* Verifier que la taille Grille = taille layout
             * Placer les cases sur la grille en fonction du layout
             * 
             *
             */ 


        }


    }
}
