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

        private const int CHEIGHT = 30;
        private int _turnNumber = 0;
        public Dictionary<Player, StackPanel> ScorePanels { get; set; } = new Dictionary<Player, StackPanel>();
        private StackPanel spTurn;

        //static StackPanelScores()
        //{
        //    //DefaultStyleKeyProperty.OverrideMetadata(typeof(StackPanelScores), new FrameworkPropertyMetadata(typeof(StackPanelScores)));
        //}

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(PlayersSource))
            {
                LoadPlayersPanels();
            }

            if (e.Property.Name == nameof(LastTurn))
            {
                if (spTurn != null)
                    AddPlayerTurn();
            }

            if (e.Property.Name == nameof(ResetGame))
            {
                if (ResetGame == true)
                    ResetScoreBoard();
            }
            base.OnPropertyChanged(e);
        }
        private void InitSPTurn()
        {

            spTurn = new StackPanel();
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

            spTurn.Children.Add(turns);
            spTurn.Children.Add(total);
            this.Children.Insert(0, spTurn);

        }
        protected override void OnInitialized(EventArgs e)
        {
            //InitSPTurn();
            base.OnInitialized(e);
        }

        private void LoadPlayersPanels()
        {
            this.Children.Clear();
            ScorePanels.Clear();
            InitSPTurn();

            if (PlayersSource == null)
                return;

            foreach (Player p in PlayersSource)
            {
                //if (ScorePanels.ContainsKey(p))
                //    continue;

                StackPanel s = new StackPanel()
                {
                    Width = 80,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                Label l = new Label
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Content = p.Name
                };
                TextBox t = new TextBox
                {
                    Height = CHEIGHT,
                    Text = MaxScore.ToString(),
                    IsReadOnly = true
                };
                s.Children.Add(l);
                s.Children.Add(t);

                this.Children.Add(s);
                ScorePanels.Add(p, s);
            }
        }




        private void AddPlayerTurn()
        {
            if (ScorePanels.ContainsKey(CurrentPlayer) == false)
                return;
            StackPanel sp = ScorePanels[CurrentPlayer];

            TextBox txt = new TextBox
            {
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
                spTurn.Children.Insert(spTurn.Children.Count - 1,
                    new Label
                    {
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Height = CHEIGHT,
                        Margin = new Thickness(0),
                        Content = _turnNumber
                    });
            }
        }

        private void ResetScoreBoard()
        {
            this.Children.Clear();
            //InitSPTurn();
            LoadPlayersPanels();
            ScorePanels = new Dictionary<Player, StackPanel>();
        }
    }
}

