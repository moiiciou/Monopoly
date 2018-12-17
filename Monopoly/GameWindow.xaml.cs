using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;
using Monopoly.Model.UI;
using Monopoly.Controller;
using System.Threading;
using System.Threading.Tasks;
using Monopoly.Model.UI;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        List<int> players = new List<int>();
        Board board = new Board();
        object lockBoard = new object();
        List<Task> tasks = new List<Task>();

        public GameWindow()
        {
            InitializeComponent();
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            
            root.Children.Add(board);
            Console.WriteLine(PlayerManager.test());

        

            players.Add(PlayerManager.CreatePlayer(board, "test", 1500, 0));

            foreach (int p in players)
            {
                Player pl = PlayerManager.SearchPlayer(p);
                
                root.Children.Add(PlayerManager.playerGrid[pl.grid]);
                //PlayerManager.playerGrid[pl.grid].Children.Add(pl);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player p = PlayerManager.SearchPlayer(players[0]);
            // PlayerManager.MoovePlayer(board, p.IdPlayer);
            List<int> dices = PlayerManager.RollDice();
            
            int nbcase = (dices[0]+dices[1]);
          //  this.lbl_jetde.Content = "Jet de dés : " + dices[0] + dices[1];

           MooveTo(p.Position, p.IdPlayer, 42);

           foreach (Task t in tasks)
            {
               // TaskScheduler.FromCurrentSynchronizationContext();
               
                t.Start();
                t.Wait();

                
            }
            
    



        }
        private void MooveTo(int posPlayer, int id, int nbCaseMoove)
        {
            
            Console.WriteLine(nbCaseMoove);
            Console.WriteLine(posPlayer);
            int posDepart = posPlayer;
           

            for (int i = 0; i < nbCaseMoove; i++)
            {
                Task a = new Task(  () =>
                {

                    
                    posPlayer++;

                    PlayerManager.DrawPlayer(board, PlayerManager.playerGrid[0], id, posPlayer);

                    posPlayer = posPlayer % 40;
                    if (posPlayer == 0)
                    {
                        PlayerManager.SearchPlayer(id).AddAmount(200);
                        Console.WriteLine(PlayerManager.SearchPlayer(id).ToString());
                    }
                    if (i == nbCaseMoove - 1)
                    {
                        Console.WriteLine("  Pos joueur = " + posPlayer);
                        GoToJail(board, id, 10);
                    }
                    Console.WriteLine("fin task " + i);
                    


                });
                tasks.Add(a);
         
                    
                
            /*PlayerInterface playerHud = new PlayerInterface();
            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);
            */
            }



        }

        private void GoToJail(Board b, int player, int posPrison)
        {
            (PlayerManager.SearchPlayer(player)).Position = posPrison;
          
            PlayerManager.DrawPlayer(b, PlayerManager.playerGrid[0], player);
            
        }

    }
}

  

