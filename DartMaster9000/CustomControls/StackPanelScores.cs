using DartMaster9000.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DartMaster9000.CustomControls
{
    /// <summary>
    /// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
    ///
    /// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:DartMaster9000.CustomControls"
    ///
    ///
    /// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:DartMaster9000.CustomControls;assembly=DartMaster9000.CustomControls"
    ///
    /// Vous devrez également ajouter une référence du projet contenant le fichier XAML
    /// à ce projet et regénérer pour éviter des erreurs de compilation :
    ///
    ///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
    ///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
    ///
    ///
    /// Étape 2)
    /// Utilisez à présent votre contrôle dans le fichier XAML.
    ///
    ///     <MyNamespace:StackPanelPlayers/>
    ///
    /// </summary>
    public class StackPanelScores : StackPanel
    {
        #region DependencyProperties
        public static readonly DependencyProperty MaxScoreProperty =
                             DependencyProperty.Register(
                             nameof(MaxScore), typeof(int),
                             typeof(StackPanelScores)
                              );
        public int MaxScore
        {
            get { return (int)GetValue(MaxScoreProperty); }
            set
            {
                SetValue(MaxScoreProperty, value);
            }
        }

        public static readonly DependencyProperty PlayersSourceProperty =
                               DependencyProperty.Register(
                               nameof(PlayersSource), typeof(List<Player>),
                               typeof(StackPanelScores)
                                );
        public List<Player> PlayersSource
        {
            get { return (List<Player>)GetValue(PlayersSourceProperty); }
            set
            {
                SetValue(PlayersSourceProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentPlayerProperty =
                               DependencyProperty.Register(
                               nameof(CurrentPlayer), typeof(Player),
                               typeof(StackPanelScores)
                                );
        public Player CurrentPlayer
        {
            get { return (Player)GetValue(CurrentPlayerProperty); }
            set
            {
                SetValue(CurrentPlayerProperty, value);
            }
        }

        public static readonly DependencyProperty LastTurnProperty =
                              DependencyProperty.Register(
                              nameof(LastTurn), typeof(Turn),
                              typeof(StackPanelScores)
                               );
        public Turn LastTurn
        {
            get { return (Turn)GetValue(LastTurnProperty); }
            set
            {
                SetValue(LastTurnProperty, value);
            }
        }

        public static readonly DependencyProperty ResetGameProperty =
                              DependencyProperty.Register(
                              nameof(ResetGame), typeof(bool),
                              typeof(StackPanelScores)
                               );
        public bool ResetGame
        {
            get { return (bool)GetValue(ResetGameProperty); }
            set
            {
                SetValue(ResetGameProperty, value);
            }
        }
        #endregion

        #region Members
        private const int CHEIGHT = 35;
        private const int FONTSIZE = 20;
        private const int SCROLL_HEIGHT = 200;
        private int _turnNumber = 0;
        public Dictionary<Player, StackPanel> ScorePanels { get; set; } = new Dictionary<Player, StackPanel>();
        private StackPanel spTurnLabels;
        private StackPanel spPlayerNames = new StackPanel()
        {
            Orientation = Orientation.Horizontal
        };
        private StackPanel spTurnsScores = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            MaxHeight = SCROLL_HEIGHT,
            MinHeight = SCROLL_HEIGHT
        };
        private ScrollViewer sv = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            MaxHeight = SCROLL_HEIGHT,
            MinHeight = SCROLL_HEIGHT,
            CanContentScroll = true
        };

        #endregion


        /// <summary>
        /// When property changes, do actions
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(PlayersSource))
            {
                LoadPlayersScorePanels();
            }

            if (e.Property.Name == nameof(LastTurn))
            {
                if (spTurnLabels != null)
                    AddPlayerTurn();
            }

            if (e.Property.Name == nameof(ResetGame))
            {
                if (ResetGame == true)
                    ResetScoreBoard();
            }
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Initialize the stackpanel acting as the column
        /// with labels of turn numbers
        /// </summary>
        private void InitSPTurn()
        {

            StackPanel spTurnHeader = new StackPanel();
            spTurnLabels = new StackPanel();


            Label turns = new Label
            {
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Content = "Turns"
            };
            Label total = new Label
            {
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Content = "Total",
                Height = CHEIGHT,

            };

            spTurnHeader.Children.Add(turns);
            spTurnLabels.Children.Add(total);
            spTurnsScores.Children.Add(spTurnLabels);
            spPlayerNames.Children.Add(spTurnHeader);
        }

        /// <summary>
        /// initialize control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            Init();
            base.OnInitialized(e);
        }

        /// <summary>
        /// Add the stackpanel containing 
        /// the names of the players and 
        /// the scrollviewer containing
        /// the turns score values
        /// </summary>
        private void Init()
        {
            sv.Content = spTurnsScores;
            this.Children.Add(spPlayerNames);
            this.Children.Add(sv);
        }

        /// <summary>
        /// Load the stackpanels acting as 
        /// the column for each column score by turns.
        /// Each players has a stackpanel assigned to it.
        /// </summary>
        private void LoadPlayersScorePanels()
        {
            ClearAll();
            ScorePanels.Clear();
            InitSPTurn();

            if (PlayersSource == null)
                return;

            foreach (Player p in PlayersSource)
            {
                StackPanel spPlayerNameHeader = new StackPanel()
                {
                    Width = 80,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                Label l = new Label
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Content = p.Name
                };


                StackPanel spTurnsScores = new StackPanel()
                {
                    Width = 80,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                TextBox t = new TextBox
                {
                    FontSize = FONTSIZE,
                    Height = CHEIGHT,
                    Text = MaxScore.ToString(),
                    IsReadOnly = true
                };

                spPlayerNameHeader.Children.Add(l);
                spPlayerNames.Children.Add(spPlayerNameHeader);

                spTurnsScores.Children.Add(t);
                this.spTurnsScores.Children.Add(spTurnsScores);
                ScorePanels.Add(p, spTurnsScores);
            }
        }

        /// <summary>
        /// Add the textbox containing the current turn score
        /// for a player.
        /// </summary>
        private void AddPlayerTurn()
        {
            if (ScorePanels.ContainsKey(CurrentPlayer) == false)
                return;
            StackPanel sp = ScorePanels[CurrentPlayer];

            TextBox txt = new TextBox
            {
                FontSize = FONTSIZE,
                Height = CHEIGHT,
                Text = LastTurn.score.ToString(),
                IsReadOnly = true
            };

            sp.Children.Insert(sp.Children.Count - 1, txt);

            TextBox total = (TextBox)sp.Children[sp.Children.Count - 1];
            total.Text = (int.Parse(total.Text) - LastTurn.score).ToString();

            if (PlayersSource.IndexOf(CurrentPlayer) == 0)
            {
                _turnNumber++;
                spTurnLabels.Children.Insert(spTurnLabels.Children.Count - 1,
                    new Label
                    {
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Height = CHEIGHT,
                        Margin = new Thickness(0),
                        Content = _turnNumber
                    });
            }
            sv.ScrollToBottom();
        }

        private void ResetScoreBoard()
        {
            ClearAll();
            ScorePanels = new Dictionary<Player, StackPanel>();
            LoadPlayersScorePanels();
        }

        private void ClearAll()
        {
            spPlayerNames.Children.Clear();
            spTurnsScores.Children.Clear();
        }
    }
}

