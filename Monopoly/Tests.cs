﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopoly.Model;
using Monopoly.Controller;
namespace Monopoly
{
   static class Tests
    {
     
        public static void test()
        {
            PlayerManager.CreatePlayer("test", 2000, 0);
            PlayerManager.CreatePlayer("test2", 2000, 0);
            List<Player> tests = PlayerManager.ListAllPlayer();

            Player p1 = tests[0];
            Player p2 = tests[1];

            Console.WriteLine(PlayerManager.MoovePlayer(p1.IdPlayer));           
        }
 
  

    }
}
