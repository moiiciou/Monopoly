using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
     abstract class Card
    {
        int _id { get; set; }
        string _name { get; set; }
        string _description { get; set; }

        public Card(int id, string name, string description)
        {
            this._id = id;
            this._name = name;
            this._description = description;
        }
        public override string ToString()
        {
            return this._id + " " + this._name + " " + this._description;
        }
    }
   
}
