using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Monopoly.Model
{
    class Drawable : Card
    {
        
        public Drawable(int id, string name, string description) : base(id,name,description)    
        {
        
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
