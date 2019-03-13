using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyClient.Core.Network
{
    [Serializable]
    public class ClientInfo
    {
        public string Pseudo { get; set;}
        public string Image { get; set; }

        public string Commande { get; set;}

        public ClientInfo()
        {
            Pseudo = "ErrorName";
            Image = "ErrorImage";
        }

        public ClientInfo(string pseudo, string image)
        {
            Pseudo = pseudo;
            Image = image;
        }

    }
}
