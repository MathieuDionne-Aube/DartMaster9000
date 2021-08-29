using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartMaster9000.Class
{
    public class Turn
    {

        public Turn(Player pownder)
        {
            Owner = pownder;
            _dartsThrown = new List<Dart>();
        }
        public Player Owner { get; set; }

        private List<Dart> _dartsThrown;

        public List<Dart> DartsThrown 
        {
            get { return _dartsThrown; }
        }
        public int score { get; set; }

        private bool _isOver;

        public bool IsOver
        {
            get { return _isOver; }
        }


        public void AddDartThrown(Dart d)
        {
                _dartsThrown.Add(d);

            if (_dartsThrown.Count > 2)
            {
                score = _dartsThrown.Sum(x => x.Value);
                _isOver = true;
            }
        }
    }
}
