using Monopoly.Controller;
using Monopoly.Model;
using Monopoly.Model.Board;
using Monopoly.Model.Case;
using server;
using server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
    }
}
