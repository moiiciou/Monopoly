using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyClient.Core.Network
{
    [Serializable]
    public class ClientMessage
    {
        public string Command { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
    }
}
