using DartMaster9000.Class;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using DartMaster9000.Tools;

namespace DartMaster9000.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Props
        private Game _currentGame;
        public Game CurrentGame
        {
            get { return _currentGame; }
            set
            {
                _currentGame = value;
                NotifyPropertyChanged(nameof(CurrentGame));
            }
        }
        public List<Player> Players { get; set; }

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
        public RelayCommand EndGameCommand { get; set; }
        public RelayCommand OnExitGameCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            AddScoreCommand = new RelayCommand<string>((s) => AddScore(int.Parse(s)));
            EndGameCommand = new RelayCommand(() => EndGame());
            OnExitGameCommand = new RelayCommand(() => OnExitGame());


            //temp
            Players = new List<Player>();
            Players.Add(new Player("Player1"));
            Players.Add(new Player("Player2"));
            Players.Add(new Player("Player3"));
            Players.Add(new Player("Player4"));
            //======

            LoadPlayers();
            CurrentGame = new Game(Players);
            NotifyPropertyChanged(nameof(Players));

            CurrentGame.Players = Players;
            CurrentPlayer = Players[0];
            CurrentTurn = new Turn(CurrentPlayer);
        }

        private void SetThrownDarts()
        {
            if (CurrentTurn.DartsThrown.Count == 1)
            {
                Dart1 = CurrentTurn.DartsThrown[0];
                Dart2.IsCurrentDart = true;
                NotifyPropertyChanged(nameof(Dart2));
            }
            else if (CurrentTurn.DartsThrown.Count == 2)
            {
                Dart2 = CurrentTurn.DartsThrown[1];
                Dart3.IsCurrentDart = true;
                NotifyPropertyChanged(nameof(Dart3));
            }
            else if (CurrentTurn.DartsThrown.Count == 3)
            {
                Dart3 = CurrentTurn.DartsThrown[2];
                Dart1.IsCurrentDart = true;
                NotifyPropertyChanged(nameof(Dart1));
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
            CurrentGame.PlayersTurns[CurrentPlayer].Add(CurrentTurn);
            LastTurn = CurrentTurn;
            ResetDarts();
            NextPlayer();
        }


        private void EndGame()
        {
            CurrentPlayer.MyStats.Victories++;
            CurrentGame.IsOver = true;
            NotifyPropertyChanged(nameof(CurrentGame));
            ResetDarts();
            CurrentPlayer = Players[0];
            CurrentGame = new Game(Players);
        }

        public void LoadPlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Player p = null;
                p = SaveManager.GetPlayer(Players[i]);
                if (p != null)
                    Players[i] = p;
            }
        }

        private void OnExitGame()
        {
            SaveManager.Save(Players);
        }
    }
}
