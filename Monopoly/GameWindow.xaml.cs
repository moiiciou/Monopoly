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

        public GameWindow()
        {
            InitializeComponent();
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);

            players.Add(PlayerManager.CreatePlayer(board, "test", 1500, 0));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player p = PlayerManager.SearchPlayer(players[0]);
           // PlayerManager.MoovePlayer(board, p.IdPlayer);
            int nbcase = (p.Position+ 5) %40;
            
            Task.Factory.StartNew(() => mooveto(board,p.Position, p.IdPlayer,nbcase));
            
            


        }
        private void mooveto(Board b, int posPlayer,int id, int posFinal)
        {
            Console.WriteLine(posFinal);
            Console.WriteLine(posPlayer);
           int nbCaseMoove = posFinal - posPlayer;
            if(nbCaseMoove < 0) {
                nbCaseMoove = posPlayer - posFinal; }
           
                for (int i =0; i <=nbCaseMoove; i++)
                {


                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        

                        PlayerManager.DrawPlayer(b, id, posPlayer);
                        posPlayer++;
                        posPlayer = posPlayer % 40;
                        

                    }), System.Windows.Threading.DispatcherPriority.Background);
                Thread.Sleep(500);
            }
            (PlayerManager.SearchPlayer(id)).Position = posFinal;
            
        }

    }
}

  

