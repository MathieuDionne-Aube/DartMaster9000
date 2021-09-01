using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartMaster9000.Class
{
    public class Player
    {

        public Player(string n)
        {
            Name = n;
        }

        public Stats MyStats { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Player))
                return false;

            Player p = (Player)obj;
            return (p.Name == Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
