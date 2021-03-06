﻿using Monopoly.Model.Board;
using Monopoly.Model.Case;
using server;
using server.Model;
using System;
using System.Net;
using System.Net.Sockets;


namespace Monopoly.Core
{
    public  static class Tools
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        public static int GetColorProperty(PropertyCase propertyCase)
        {
            int result = 0;

            foreach(BaseCase baseCase in Board.GetBoard.CasesList)
            {
                if(baseCase.GetType() == propertyCase.GetType())
                {
                    PropertyCase baseCaseProperty = (PropertyCase)baseCase;
                    if (baseCaseProperty.CaseInformation.Color == propertyCase.CaseInformation.Color)
                        result++;
                }
            }


            return result;
        }

        public static int GetColorProperty(PlayerInfo player, PropertyCase property)
        {
            int result = 0;
            foreach(server.CaseInfo caseInfo in player.Properties)
            {
                if(caseInfo.GetType() == typeof(PropertyInfo))
                {
                    PropertyInfo propertyInfo = (PropertyInfo)caseInfo;

                    if (propertyInfo.Color == property.CaseInformation.Color)
                        result++;
                }

            }
            return result;
        }

        public static PropertyCase GetPropertyByName(string propertyName)
        {

            foreach (BaseCase baseCase in Board.GetBoard.CasesList)
            {
                if (baseCase.GetType() == typeof(PropertyCase))
                {
                    PropertyCase baseCaseProperty = (PropertyCase)baseCase;
                    if (baseCaseProperty.CaseInformation.Location == propertyName)
                        return baseCaseProperty;

                }
            }

            return null;
        }

        public static string CleanJson(string json)
        {
            int indexLastValidChar = 0;
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '}')
                    indexLastValidChar = i;
            }

            return json.Remove(indexLastValidChar+1);
        }
    }
}
