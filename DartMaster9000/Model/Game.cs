using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartMaster9000.Class
{
    public class Game
    {

        public List<Player> Players { get; set; }
        public Dictionary<Player,List<Turn>> PlayersTurns { get; set; }
        public bool IsOver { get; set; }

        public Game(List<Player> p)
        {
            Players = p;
            InitializePlayerTurns();
            IsOver = false;
        }

        public void InitializePlayerTurns()
        {
            PlayersTurns = new Dictionary<Player, List<Turn>>();
            foreach (var p in Players)
            {
                PlayersTurns.Add(p, new List<Turn>());
            }
        }
    }
}
