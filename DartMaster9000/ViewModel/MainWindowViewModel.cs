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
using System.Windows;

namespace DartMaster9000.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public int Max_score { get; }
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

        private Dart _currentDart = null;
        public Dart CurrentDart
        {
            get { return _currentDart; }
            set
            {
                _currentDart = value;
                NotifyPropertyChanged(nameof(CurrentDart));
            }
        }
        #endregion


        #region Commands
        public RelayCommand<string> AddScoreCommand { get; set; }
        public RelayCommand EndGameCommand { get; set; }
        public RelayCommand ForceEndGameCommand { get; set; }
        public RelayCommand OnExitGameCommand { get; set; }
        public RelayCommand NextTurnCommand { get; set; }
        public RelayCommand<string> SetCurrentDartCommand { get; set; }
        #endregion

        public MainWindowViewModel(List<Player> players,int pmax_score)
        {
            AddScoreCommand = new RelayCommand<string>((s) => AddScore(int.Parse(s)));
            EndGameCommand = new RelayCommand(() => EndGame());
            ForceEndGameCommand = new RelayCommand(() => ForceEndGame());
            OnExitGameCommand = new RelayCommand(() => OnExitGame());
            NextTurnCommand = new RelayCommand(() => EndTurn());
            SetCurrentDartCommand = new RelayCommand<string>((i) => SetCurrentDart(i));


            Max_score = pmax_score;
            NotifyPropertyChanged(nameof(Max_score));

            Players = players;
            CurrentDart = Dart1;
            LoadPlayers();
            CurrentGame = new Game(Players);
            NotifyPropertyChanged(nameof(Players));

            CurrentGame.Players = Players;
            CurrentPlayer = Players[0];
            CurrentTurn = new Turn(CurrentPlayer);
        }

        /// <summary>
        /// Set the next active dart
        /// automatically from the last one. 
        /// If its the third dart it doesnt set 
        /// another one.
        /// </summary>
        private void SetNextDart()
        {
            Dart1.IsCurrentDart = false;
            Dart2.IsCurrentDart = false;
            Dart3.IsCurrentDart = false;

            if (CurrentDart == Dart1)
            {
                Dart1 = CurrentTurn.DartsThrown[0];
                Dart2.IsCurrentDart = true;
                CurrentDart = Dart2;
            }
            else if (CurrentDart == Dart2)
            {
                Dart2 = CurrentTurn.DartsThrown[1];
                Dart3.IsCurrentDart = true;
                CurrentDart = Dart3;
            }
            else if (CurrentDart == Dart3)
            {
                Dart3 = CurrentTurn.DartsThrown[2];
                NotifyPropertyChanged(nameof(CurrentTurn));
            }
            NotifyPropertyChanged(nameof(Dart1));
            NotifyPropertyChanged(nameof(Dart2));
            NotifyPropertyChanged(nameof(Dart3));

        }

        /// <summary>
        /// Set the current dart on demand.
        /// </summary>
        /// <param name="index"></param>
        private void SetCurrentDart(string index)
        {
            Dart1.IsCurrentDart = false;
            Dart2.IsCurrentDart = false;
            Dart3.IsCurrentDart = false;
            switch (index)
            {
                case "1":
                    CurrentDart = Dart1;
                    Dart1.IsCurrentDart = true;

                    break;
                case "2":
                    CurrentDart = Dart2;
                    Dart2.IsCurrentDart = true;
                    break;
                case "3":
                    CurrentDart = Dart3;
                    Dart3.IsCurrentDart = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
            NotifyPropertyChanged(nameof(Dart1));
            NotifyPropertyChanged(nameof(Dart2));
            NotifyPropertyChanged(nameof(Dart3));
        }

        private void ResetDarts()
        {
            Dart1 = new Dart(0);
            Dart1.IsCurrentDart = true;
            CurrentDart = Dart1;
            NotifyPropertyChanged(nameof(Dart1));
            Dart2 = new Dart(0);
            Dart3 = new Dart(0);
        }

        private void AddScore(int score)
        {
            CurrentDart.Value = score;

            if (_currentDart == _dart1) 
                NotifyPropertyChanged(nameof(Dart1));
            else if (_currentDart == _dart2)
                NotifyPropertyChanged(nameof(Dart2));
            if (_currentDart == _dart3)
                NotifyPropertyChanged(nameof(Dart3));

            if (CurrentTurn.DartsThrown.Contains(CurrentDart) == false)
                CurrentTurn.AddDartThrown(CurrentDart);

            SetNextDart();
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
           
            int player_score = CurrentGame.PlayersTurns[CurrentPlayer].Sum(x => x.score) + CurrentTurn.score;
            if (player_score > Max_score)
            {
                MessageBox.Show("Player is busting.", "Warning", MessageBoxButton.OK);
                return;
            }
            else if (player_score == Max_score)
            {
                MessageBox.Show($"{CurrentPlayer.Name} won the game.", "Winner!", MessageBoxButton.OK);
                EndGame();
                return;
            }
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
            ReorderPlayersByScore();
            CurrentPlayer = Players[0];
            CurrentGame = new Game(Players);
        }

        private void ForceEndGame()
        {
            if (MessageBox.Show($"You are gonna crown {CurrentPlayer.Name} as the winner, Continues?",
                            "Winner",
                            MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            EndGame();
        }


        private void ReorderPlayersByScore()
        {
            List<Player> new_order = new List<Player>();
            foreach (Player p in CurrentGame.PlayersTurns.OrderByDescending(x => x.Value.Sum(y => y.score)).Select(x=> x.Key))
            {
                new_order.Add(p);
            }
            Players = new_order;
            NotifyPropertyChanged(nameof(Players));
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
