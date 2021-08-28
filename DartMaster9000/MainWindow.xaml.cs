using DartMaster9000.Class;
using DartMaster9000.CustomControls;
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

namespace DartMaster9000
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;
        public List<Player> Players { get; set; } = new List<Player>();
        public Player CurrentPlayer { get; set; }
        const int CHEIGHT = 25;
        public Dictionary<Player, StackPanel> ScorePanels { get; set; } = new Dictionary<Player, StackPanel>();

        public Dictionary<Player, Label> standoffsLabel { get; set; } = new Dictionary<Player, Label>();
        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            Players.Add(new Player("Math"));
            Players.Add(new Player("Will"));
            Players.Add(new Player("Lad"));
            Players.Add(new Player("Simon"));
            _game.Players = Players;
            CurrentPlayer = Players[0];
            GeneratePanels();
            lbCurrentPlayer.Content = CurrentPlayer.Name;
            _game.InitializePlayerTurns();
        }



        private void GeneratePanels()
        {
            foreach (Player p in Players)
            {
                StackPanel s = new StackPanel()
                {
                    Width = 50,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                Label l = new Label
                {
                    Content = p.Name
                };
                CustomTextbox t = new CustomTextbox
                {
                    Height = CHEIGHT,
                    Text = 501.ToString(),
                    IsReadOnly = true
                };
                s.Children.Add(l);
                s.Children.Add(t);

                StackPanel sp = (StackPanel)this.FindName("spScore");
                sp.Children.Add(s);

                ScorePanels.Add(p, s);


                StackPanel standoffs = (StackPanel)this.FindName("spStandoffs");
                Label lb = new Label
                {
                    Content = $"{p.Name}  : {p.Victories}"
                };
                standoffs.Children.Add(lb);
                standoffsLabel.Add(p, lb);
            }
        }

        private void AddPlayerTurn(Turn t)
        {
            StackPanel sp = ScorePanels[CurrentPlayer];

            CustomTextbox txt = new CustomTextbox
            {
                Height = CHEIGHT,
                Text = t.score.ToString()
            };

            sp.Children.Insert(sp.Children.Count - 1, txt);

            CustomTextbox total = (CustomTextbox)sp.Children[sp.Children.Count - 1];
            total.Text = (int.Parse(total.Text) - t.score).ToString();

            if (Players.IndexOf(CurrentPlayer) == 0)
            {
                StackPanel spTurn = (StackPanel)this.FindName("spTurns");
                spTurn.Children.Insert(spTurn.Children.Count - 1,
                    new Label
                    {
                        Height = CHEIGHT,
                        Margin = new Thickness(0),
                        Content = _game.PlayersTurns.Select(x => x.Value.Count).Max().ToString()
                    });
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox1.Text) == false)
            {
                txt1Enter();
            }
        }

        private void txt1Enter()
        {
            textbox2.IsEnabled = true;
            textbox2.Focus();
        }


        private void TextBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox2.Text) == false)
            {
                txt2Enter();
            }
        }
        private void txt2Enter()
        {
            textbox3.IsEnabled = true;
            textbox3.Focus();
        }


        private void TextBox_KeyUp_2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox3.Text) == false)
            {
                txt3Enter();
            }
        }

        private void txt3Enter()
        {

             StackPanel sp = ScorePanels[CurrentPlayer];

            CustomTextbox total = (CustomTextbox)sp.Children[sp.Children.Count - 1];
            if(int.Parse(total.Text) - (int.Parse(textbox3.Text) + int.Parse(textbox2.Text) + int.Parse(textbox1.Text)) < 0)
            {
                MessageBox.Show("Error : Busting 501","Error",MessageBoxButton.OK);
                return;
            }
            NextTurn();
            textbox1.Focus();
        }

        private void NextTurn()
        {
            int total = int.Parse(textbox1.Text) + int.Parse(textbox2.Text) + int.Parse(textbox3.Text);

            Turn t = new Turn(total);
            _game.PlayersTurns[CurrentPlayer].Add(t);
            AddPlayerTurn(t);
            NextPlayer();

            textbox1.Text = "";

            textbox2.Text = "";
            textbox2.IsEnabled = false;

            textbox3.Text = "";
            textbox3.IsEnabled = false;
        }



        private void NextPlayer()
        {
            int index = Players.IndexOf(CurrentPlayer);
            if (index == Players.Count - 1)
                index = 0;
            else
                index++;

            if (string.IsNullOrEmpty(textbox1.Text) ||
                string.IsNullOrEmpty(textbox2.Text) ||
                string.IsNullOrEmpty(textbox3.Text))
                return;

            CurrentPlayer = Players[index];
            lbCurrentPlayer.Content = CurrentPlayer.Name;
        }


        private void EndGame()
        {
            CurrentPlayer.Victories++;
            Label x = standoffsLabel[CurrentPlayer];
            x.Content = $"{CurrentPlayer.Name}  : {CurrentPlayer.Victories}";
            foreach (KeyValuePair<Player, StackPanel> panel in ScorePanels)
            {
                panel.Value.Children.Clear();
                Label l = new Label
                {
                    Content = panel.Key.Name
                };
                CustomTextbox t = new CustomTextbox
                {
                    Height = CHEIGHT,
                    Text = 0.ToString(),
                    IsReadOnly = true
                };
                panel.Value.Children.Add(l);
                panel.Value.Children.Add(t);
                CurrentPlayer = Players[0];

                spTurns.Children.Clear();

                Label lblTurns = new Label
                {
                    Content = "TURNS"
                };

                Label total = new Label
                {
                    Content = "Total"
                };

                StackPanel sp = (StackPanel)this.FindName("spTurns");
                sp.Children.Add(lblTurns);
                sp.Children.Add(total);
            }
        }


        private void AddScoreFromClick(int score)
        {
            if(textbox2.IsEnabled == false)
            {
                textbox1.Text = score.ToString();
                txt1Enter();
            }
            else if (textbox3.IsEnabled == false)
            {
                textbox2.Text = score.ToString();
                txt2Enter();
            }
            else
            {
                textbox3.Text = score.ToString();
                txt3Enter();
            }
        }

    

        public void OnHover(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
             Brush b = new SolidColorBrush(Color.FromRgb(255,255,255));
            b.Opacity = 0.2;
            p.Fill = b;
        }

        public void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            Brush b = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            b.Opacity = 0;
            p.Fill = b;
        }


        public void ChildHover(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            Brush b = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            b.Opacity = 0.2;
            p.Fill = b;
            e.Handled = true;
        }

        public void BullseyeEnter(object sender, MouseEventArgs args)
        {
            Rectangle p = (Rectangle)sender;
            Brush b = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            b.Opacity = 0.2;
            p.Fill = b;
            args.Handled = true;
        }

        public void BullseyeLeave(object sender, MouseEventArgs args)
        {
            Rectangle p = (Rectangle)sender;
            Brush b = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            b.Opacity = 0;
            p.Fill = b;
            args.Handled = true;
        }

        public void OnChildLeave(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            Brush b = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            b.Opacity = 0;
            p.Fill = b;
            e.Handled = true;
        }

        private void double5_MouseEnter(object sender, MouseEventArgs e)
        {
            OnHover(sender, e);
        }
        #region sectionClick


        private void section20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(20);
        }

        private void section5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(5);

        }

        private void secion12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(12);

        }

        private void section9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(9);

        }

        private void section14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(14);

        }

        private void section11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(11);

        }

        private void section8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(8);

        }

        private void section16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(16);

        }

        private void section7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(7);

        }

        private void section19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(19);

        }

        private void section3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(3);

        }

        private void section17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(17);

        }

        private void section2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(2);

        }

        private void section15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(15);

        }

        private void section10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(10);

        }

        private void section6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(6);

        }

        private void section13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(13);

        }

        private void section4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(4);

        }

        private void section18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(18);

        }

        private void section1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(1);

        }


        #endregion

        #region TripleClick
        private void triple20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(60);
        }

        private void triple5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(15);

        }

        private void triple12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(36);

        }

        private void triple9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(27);

        }

        private void triple14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(42);

        }

        private void triple11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(33);

        }

        private void triple8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(24);

        }

        private void triple16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(48);

        }

        private void triple7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(21);

        }

        private void triple19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(57);

        }

        private void triple3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(9);

        }

        private void triple17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(51);

        }

        private void triple2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(6);

        }

        private void triple15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(45);

        }

        private void triple10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(30);

        }

        private void triple6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(18);

        }

        private void triple13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(39);

        }

        private void triple4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(12);


        }

        private void triple18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(54);

        }

        private void triple1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(3);

        }
        #endregion

        #region DoubleClick
        private void double20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(40);
        }



        private void double5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(10);

        }

        private void double12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(24);

        }

        private void double9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(18);

        }

        private void double14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(28);

        }

        private void double11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(22);

        }

        private void double8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(16);

        }

        private void double16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(32);

        }

        private void double7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(14);

        }

        private void double19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(38);

        }

        private void double3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(6);

        }

        private void double17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(34);

        }

        private void double2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(4);

        }

        private void double15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(30);

        }

        private void double10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(20);

        }

        private void double6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(12);

        }

        private void double13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(26);

        }

        private void double4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(8);

        }

        private void double18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(36);

        }

        private void double1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(2);

        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to end the game and crown {CurrentPlayer.Name} as the winner?",
                              "Winner", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                EndGame();
            }
        }

        private void noPoints_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(0);
        }

        private void bull25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(25);

        }

        private void bull50_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddScoreFromClick(50);

        }
    }
}
