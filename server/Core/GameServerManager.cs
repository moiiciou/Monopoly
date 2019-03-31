using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server.Model;
using server.Controleur;

namespace server.Core
{
    public static class GameServerManager
    {
        public static void useEffectCard(CardInfo card, ref PlayerInfo player, ref ThemeParser tp, int salaire)
        {
            if (card.typeAction == CardInfo.TypeAction.moove)
            {
                int position = card.value; // position de la case où le joueur doit arriver
                int deplacement = 0;
                int oldPosition = player.Position;

                if (player.Position % 40 > position)
                {// si le joueur a dépassé la case à atteindre
                    deplacement = 40 - player.Position % 40;
                    deplacement += position;
                }
                else
                    deplacement = position - player.Position % 40;

                player.Position += deplacement;

                if (player.Position % 40 == tp.jail.positionPlateau)
                {
                    player.isInJail = true;
                }
                else if (oldPosition / 40 < player.Position / 40)
                {
                    player.Balance += salaire;
                }


                PropertyInfo prop = tp.searchIndexPropertyAtPos(player.Position);
                StationInfo stat = tp.searchCaseStationAtPos(player.Position);
                CompanyInfo comp = tp.searchCaseCompanyAtPos(player.Position);

                if (prop != null && prop.Owner != player.Pseudo && prop.Owner != null && prop.Owner != "")
                {
                    int rent = RentManager.computeRent(prop, player, tp);
                    if (rent > 0)
                    {
                        player.Balance -= rent;
                        // response.ChatMessage += " Le joueur  " + player.Pseudo + " paie " + rent + "€ à " + propRent.Owner;
                        PlayerManager.GetPlayerByPseuso(prop.Owner).Balance += rent;
                    }

                }
                else if (comp != null && comp.Owner != player.Pseudo && comp.Owner != null && comp.Owner != "")
                {
                    int rent = RentManager.computeRent(comp, player, tp);
                    if (rent > 0)
                    {
                        player.Balance -= rent;
                        // response.ChatMessage += " Le joueur  " + player.Pseudo + " paie " + rent + "€ à " + propRent.Owner;
                        PlayerManager.GetPlayerByPseuso(comp.Owner).Balance += rent;
                    }

                }
                else if (stat != null && stat.Owner != player.Pseudo && stat.Owner != null && stat.Owner != "")
                {
                    int rent = RentManager.computeRent(stat, player, tp);
                    if (rent > 0)
                    {
                        player.Balance -= rent;
                        // response.ChatMessage += " Le joueur  " + player.Pseudo + " paie " + rent + "€ à " + propRent.Owner;
                        PlayerManager.GetPlayerByPseuso(stat.Owner).Balance += rent;
                    }

                }
                else if (card.typeAction == CardInfo.TypeAction.paiement)
                {
                    if (player.Balance + card.value > 0)
                        player.Balance += card.value;


                }
                else if (card.typeAction == CardInfo.TypeAction.reparation)
                {
                    int cost = 0;
                    foreach (PropertyInfo p in player.Properties)
                    {
                        if (p.HasHostel)
                        {
                            cost += card.value2;
                        }
                        else
                        {
                            cost += p.NumberOfHouse * card.value;
                        }

                    }

                    if (cost < player.Balance)
                        player.Balance -= cost;
                }
                else if (card.typeAction == CardInfo.TypeAction.freefromjail)
                {
                    if (card.typeCard == CardInfo.TypeCard.community)
                    {
                        player.hasCommunityCardFree = true;
                        tp.communityCards.Remove(card);
                    }
                    else
                    {
                        player.hasChanceCardFree = true;
                        tp.chanceCards.Remove(card);
                    }
                }
            }
        }
    }
}

        

