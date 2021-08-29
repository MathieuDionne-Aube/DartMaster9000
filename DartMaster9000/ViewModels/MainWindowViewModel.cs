using DartMaster9000.Class;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DartMaster9000.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Props
        private Game _game;
        public List<Player> Players { get; set; } = new List<Player>();

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
                NotifyPropertyChanged(nameof(CurrentPlayer));
            }
        }

        private Turn _currentTurn;
        public Turn CurrentTurn
        {
            get { return _currentTurn; }
            set
            {
                _currentTurn = value;
                NotifyPropertyChanged(nameof(CurrentTurn));
            }
        }

        private Turn _lastTurn;
        public Turn LastTurn
        {
            get { return _lastTurn; }
            set
            {
                _lastTurn = value;
                NotifyPropertyChanged(nameof(LastTurn));
            }
        }

        private Dart _dart1 = new Dart(0);
        public Dart Dart1
        {
            get { return _dart1; }
            set
            {
                _dart1 = value;
                NotifyPropertyChanged(nameof(Dart1));
            }
        }

        private Dart _dart2 = new Dart(0);
        public Dart Dart2
        {
            get { return _dart2; }
            set
            {
                _dart2 = value;
                NotifyPropertyChanged(nameof(Dart2));
            }
        }

        private Dart _dart3 = new Dart(0);
        public Dart Dart3
        {
            get { return _dart3; }
            set
            {
                _dart3 = value;
                NotifyPropertyChanged(nameof(Dart3));
            }
        }

        #endregion


        #region Commands
        public RelayCommand<string> AddScoreCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            AddScoreCommand = new RelayCommand<string>((s) => AddScore(int.Parse(s)));
            _game = new Game();

            //temp
            Players.Add(new Player("Player1"));
            Players.Add(new Player("Player2"));
            Players.Add(new Player("Player3"));
            Players.Add(new Player("Player4"));
            //======

            NotifyPropertyChanged(nameof(Players));

            _game.Players = Players;
            CurrentPlayer = Players[0];
            CurrentTurn = new Turn(CurrentPlayer);

            _game.InitializePlayerTurns();
        }

        private void SetThrownDarts()
        {
            if (CurrentTurn.DartsThrown.Count == 1)
            {
                Dart1 = CurrentTurn.DartsThrown[0];
            }
            else if (CurrentTurn.DartsThrown.Count == 2)
            {
                Dart2 = CurrentTurn.DartsThrown[1];
            }
            else if (CurrentTurn.DartsThrown.Count == 3)
            {
                Dart3 = CurrentTurn.DartsThrown[2];
            }
        }

        private void ResetDarts()
        {
            Dart1 = new Dart(0);
            Dart2 = new Dart(0);
            Dart3 = new Dart(0);
        }

        private void AddScore(int score)
        {
            Dart d = new Dart(score);
            CurrentTurn.AddDartThrown(d);
            SetThrownDarts();
            if (CurrentTurn.IsOver)
            {
                EndTurn();
            }
        }

        private void NextPlayer()
        {
            int index = Players.IndexOf(CurrentPlayer);
            if (index == Players.Count - 1)
                index = 0;
            else
                index++;


            CurrentPlayer = Players[index];
            CurrentTurn = new Turn(CurrentPlayer);
        }

        private void EndTurn()
        {
            _game.PlayersTurns[CurrentPlayer].Add(CurrentTurn);
            LastTurn = CurrentTurn;
            ResetDarts();
            NextPlayer();
        }
    }
}
