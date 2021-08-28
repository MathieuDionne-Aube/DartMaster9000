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
        public string Name { get; set; }
        public int Score { get; set; }
        public bool HasWon { get; set; }

        public int Victories { get; set; }
    }
}
