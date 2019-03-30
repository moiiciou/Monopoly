using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Model
{
    public class Deal
    {
        private int nextId = 0;
        public int Id { get; }
        public PlayerInfo Asker { get; set; }
        public PlayerInfo Receiver { get; set; }

        public bool Accepted { get; set; }

        public Object ObjectTransfered { get; set; }

        public Deal(PlayerInfo asker, PlayerInfo receiver, Object objectTransfered)
        {
            Id = nextId;
            nextId++;
            Asker = asker;
            Receiver = receiver;
            Accepted = false;
            ObjectTransfered = objectTransfered;
        }

        public bool Process()
        {
            // si object est une propriété ... faire le traitement

            // si object est de type carte libéré de prison, faire le traitement

            //return true si tout c'est bien passé
            return true;
        }

    }
}
