using DartMaster9000.Class;
using DartMaster9000.Tools;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DartMaster9000.ViewModel
{
    public class PlayerGestionViewModel : ViewModelBase
    {
        private ObservableCollection<Player> _availablePlayers;

        public ObservableCollection<Player> AvailablePlayers
        {
            get { return _availablePlayers; }
            set
            {
                _availablePlayers = value;
                NotifyPropertyChanged(nameof(AvailablePlayers));
            }
        }

        private ObservableCollection<Player> _inGamePlayers;

        public ObservableCollection<Player> InGamePlayers
        {
            get { return _inGamePlayers; }
            set
            {
                _inGamePlayers = value;
                NotifyPropertyChanged(nameof(InGamePlayers));
            }
        }


        public RelayCommand<Player> AddToInGameCommand { get; set; }
        public RelayCommand<Player> RemoveFromInGameCommand { get; set; }
        public RelayCommand AddNewPlayerCommand { get; set; }

        private string _newPlayerName;

        public string NewPlayerName
        {
            get { return _newPlayerName; }
            set { _newPlayerName = value;
                NotifyPropertyChanged(nameof(NewPlayerName));
            }
        }


        public PlayerGestionViewModel()
        {
            AddToInGameCommand = new RelayCommand<Player>((p) => AddToInGame(p));
            RemoveFromInGameCommand = new RelayCommand<Player>((p) => RemoveFromInGame(p));
            AddNewPlayerCommand = new RelayCommand(() => AddNewPlayer());

            AvailablePlayers = new ObservableCollection<Player>();
            foreach (Player item in SaveManager.GetPlayers())
            {
                AvailablePlayers.Add(item);
            }

            InGamePlayers = new ObservableCollection<Player>();

        }

        private void AddToInGame(Player p)
        {
            if (p == null || InGamePlayers.Contains(p))
                return;
            InGamePlayers.Add(p);
            AvailablePlayers.Remove(p);
            NotifyPropertyChanged(nameof(AvailablePlayers));
            NotifyPropertyChanged(nameof(InGamePlayers));

        }

        private void RemoveFromInGame(Player p)
        {
            if (p == null || AvailablePlayers.Contains(p))
                return;
            InGamePlayers.Remove(p);
            AvailablePlayers.Add(p);
            NotifyPropertyChanged(nameof(AvailablePlayers));
            NotifyPropertyChanged(nameof(InGamePlayers));
        }

        private void AddNewPlayer()
        {
            if(string.IsNullOrEmpty(NewPlayerName))
            {
                MessageBox.Show("Name is Empty");
            }    
            Player p = new Player(NewPlayerName);

            if (AvailablePlayers.Contains(p) || InGamePlayers.Contains(p))
            {
                MessageBox.Show("Player with the same name already exist.");
                return;
            }
            AvailablePlayers.Add(p);
            NewPlayerName = string.Empty;
            //NotifyPropertyChanged(nameof(AvailablePlayers));
        }
    }
}
