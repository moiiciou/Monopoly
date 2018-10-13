﻿using Monopoly.Properties;
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



            }

            /* Verifier que la taille Grille = taille layout
             * Placer les cases sur la grille en fonction du layout
             * 
             *
             */ 


        }


    }
}