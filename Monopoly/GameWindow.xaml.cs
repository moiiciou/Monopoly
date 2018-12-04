using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;
using Monopoly.Controller;
using System.Threading;
using System.Threading.Tasks;

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

        public GameWindow()
        {
            InitializeComponent();
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);
            Console.WriteLine(PlayerManager.test());

            players.Add(PlayerManager.CreatePlayer(board, "test", 1500, 0));
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player p = PlayerManager.SearchPlayer(players[0]);
            // PlayerManager.MoovePlayer(board, p.IdPlayer);
            List<int> dices = PlayerManager.RollDice();
            
            int nbcase = (dices[0]+dices[1]);
          //  this.lbl_jetde.Content = "Jet de dés : " + dices[0] + dices[1];

            Task.Factory.StartNew(() => MooveTo(board, p.Position, p.IdPlayer, 42));

           
            
    



        }
        private void MooveTo(Board b, int posPlayer, int id, int nbCaseMoove)
        {
            
            Console.WriteLine(nbCaseMoove);
            Console.WriteLine(posPlayer);
            int posDepart = posPlayer;

            for (int i = 0; i < nbCaseMoove; i++)
            {
                lock (lockBoard)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Console.WriteLine("je suis dans le thread " + i);

                        posPlayer++;

                        PlayerManager.DrawPlayer(b, id, posPlayer);

                        posPlayer = posPlayer % 40;
                        if (posPlayer == 0)
                        {
                            PlayerManager.SearchPlayer(id).AddAmount(200);
                            Console.WriteLine(PlayerManager.SearchPlayer(id).ToString());
                        }

                    }), System.Windows.Threading.DispatcherPriority.Background);
                    Thread.Sleep(500);
                    Console.WriteLine("Fin thread " + i);
                }

                if ( i == nbCaseMoove-1)
                {
                    Console.WriteLine( "  Pos joueur = " + posPlayer);
                    GoToJail(board,id, 10);
                }

            }
           /* lock (lockBoard)
            {
                (PlayerManager.SearchPlayer(id)).Position = (posPlayer) % 40;
                if ((PlayerManager.SearchPlayer(id)).Position == posDepart + nbCaseMoove && (PlayerManager.SearchPlayer(id)).Position == 30)
                {
                    GoToJail((PlayerManager.SearchPlayer(id)).IdPlayer, 10);
                }
            }*/

        }

        private void GoToJail(Board b, int player, int posPrison)
        {
            (PlayerManager.SearchPlayer(player)).Position = posPrison;
          
            PlayerManager.DrawPlayer(b, player);
            
        }

    }
}

  

