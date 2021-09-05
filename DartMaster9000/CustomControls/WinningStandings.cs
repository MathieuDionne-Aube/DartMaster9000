using DartMaster9000.Class;
using DartMaster9000.Tools;
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
    ///     <MyNamespace:WinningStandings/>
    ///
    /// </summary>
    public class WinningStandings : StackPanel
    {
        #region Props
        public static readonly DependencyProperty ResetGameProperty =
                           DependencyProperty.Register(
                           nameof(ResetGame), typeof(bool),
                           typeof(WinningStandings)
                            );
        public bool ResetGame
        {
            get { return (bool)GetValue(ResetGameProperty); }
            set
            {
                SetValue(ResetGameProperty, value);
            }
        }

        public static readonly DependencyProperty PlayersSourceProperty =
                              DependencyProperty.Register(
                              nameof(PlayersSource), typeof(List<Player>),
                              typeof(WinningStandings)
                               );
        public List<Player> PlayersSource
        {
            get { return (List<Player>)GetValue(PlayersSourceProperty); }
            set
            {
                SetValue(PlayersSourceProperty, value);
            }
        }


        #endregion
        public Dictionary<Player, Label> standoffsLabel { get; set; } = new Dictionary<Player, Label>();
        static WinningStandings()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(WinningStandings), new FrameworkPropertyMetadata(typeof(WinningStandings)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(ResetGame) ||
                e.Property.Name == nameof(PlayersSource))
            {
                LoadStandings();
            }
            base.OnPropertyChanged(e);
        }
        protected override void OnInitialized(EventArgs e)
        {
            Init();
            base.OnInitialized(e);
        }

        private void Init()
        {
            Label l = new Label();
            l.FontSize = 25;
            l.Content = "WINNING STANDINGS";
            l.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            LoadStandings();
            this.Children.Insert(0, l);
        }


        private void LoadStandings()
        {
            foreach (Player p in PlayersSource)
            {
                string lbcontent = $"{p.Name}  : {p.MyStats.Victories}";

                if (standoffsLabel.ContainsKey(p))
                {
                    Label lb = standoffsLabel[p];
                    lb.Content = lbcontent;
                    lb.FontSize = 20;
                    standoffsLabel[p] = lb;
                }
                else
                {
                    Label lb = new Label
                    {
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Content = lbcontent,
                        FontSize = 20
                };
                    standoffsLabel.Add(p, lb);
                    this.Children.Add(lb);
                }

            }
        }
    }
}
