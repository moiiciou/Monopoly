using System.Collections.Generic;
using server.Model;

namespace server.Controleur
{

    public class PlayerManager 
    {
        public static int nbPlayer()
        {
            return GameData.GetGameData.PlayerList.Count;
        }
        public static PlayerInfo GetPlayerByPseuso(string pseudo)
        {
            foreach(PlayerInfo player in GameData.GetGameData.PlayerList)
            {
                if (player.Pseudo == pseudo)
                    return player;
            }

            return new PlayerInfo();
        }

        public static PropertyInfo searchProperty(PlayerInfo pl , string location)
        {
            foreach(PropertyInfo p in pl.Properties)
            {
                if(p.Location == location)
                {
                    return p;
                }
            }
            return null;
        }
        public static CompanyInfo searchCompany(PlayerInfo pl, string location)
        {
            foreach (CompanyInfo p in pl.Companies)
            {
                if (p.TextLabel == location)
                {
                    return p;
                }
            }
            return null;
        }
        public static StationInfo searchStation(PlayerInfo pl, string location)
        {
            foreach (StationInfo p in pl.Stations)
            {
                if (p.TextLabel == location)
                {
                    return p;
                }
            }
            return null;
        }
        public static List<PropertyInfo> searchPropertyUnMortgage(PlayerInfo pl)
        {
            List<PropertyInfo> res = new List<PropertyInfo>();
            foreach(PropertyInfo p in pl.Properties)
            {
                if (!p.isMortgaged && p.NumberOfHouse == 0)
                {
                    res.Add(p);
                }
            }
            return res;
        }

        public static List<PropertyInfo> searchPropertyMortgage(PlayerInfo pl)
        {
            List<PropertyInfo> res = new List<PropertyInfo>();
            foreach (PropertyInfo p in pl.Properties)
            {
                if (p.isMortgaged && p.NumberOfHouse == 0)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public static List<CompanyInfo> searchCompaniesMortgage(PlayerInfo pl)
        {
            List<CompanyInfo> res = new List<CompanyInfo>();
            foreach (CompanyInfo p in pl.Companies)
            {
                if (p.isMortgaged)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public static List<StationInfo> searchStationMortgage(PlayerInfo pl)
        {
            List<StationInfo> res = new List<StationInfo>();
            foreach (StationInfo p in pl.Stations)
            {
                if (p.isMortgaged)
                {
                    res.Add(p);
                }
            }
            return res;
        }

        public static List<PropertyInfo> searchPropertyUnMortgageWithHouse(PlayerInfo pl)
        {
            List<PropertyInfo> res = new List<PropertyInfo>();
            foreach (PropertyInfo p in pl.Properties)
            {
                if (!p.isMortgaged && p.NumberOfHouse >0)
                {
                    res.Add(p);
                }
            }
            return res;
        }

        public static List<CompanyInfo> searchCompaniesUnMortgage(PlayerInfo pl)
        {
            List<CompanyInfo> res = new List<CompanyInfo>();
            foreach (CompanyInfo p in pl.Companies)
            {
                if (!p.isMortgaged)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public static List<StationInfo> searchStationUnMortgage(PlayerInfo pl)
        {
            List<StationInfo> res = new List<StationInfo>();
            foreach (StationInfo p in pl.Stations)
            {
                if (!p.isMortgaged)
                {
                    res.Add(p);
                }
            }
            return res;
        }

    }


}
