using DartMaster9000.Class;
using DartMaster9000.CustomControls;
using DartMaster9000.ViewModels;
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
        private MainWindowViewModel vm;

        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox1.Text) == false)
            //{
            //    txt1Enter();
            //}
        }



        private void TextBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox2.Text) == false)
            //{
            //    txt2Enter();
            //}
        }
        private void txt2Enter()
        {
            //textbox3.IsEnabled = true;
            //textbox3.Focus();
        }


        private void TextBox_KeyUp_2(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter && string.IsNullOrEmpty(textbox3.Text) == false)
            //{
            //    txt3Enter();
            //}
        }

        private void EndGame()
        {
            //CurrentPlayer.Victories++;
            //Label x = standoffsLabel[CurrentPlayer];
            //x.Content = $"{CurrentPlayer.Name}  : {CurrentPlayer.Victories}";
            //foreach (KeyValuePair<Player, StackPanel> panel in ScorePanels)
            //{
            //    panel.Value.Children.Clear();
            //    Label l = new Label
            //    {
            //        Content = panel.Key.Name
            //    };
            //    CustomTextbox t = new CustomTextbox
            //    {
            //        Height = CHEIGHT,
            //        Text = 0.ToString(),
            //        IsReadOnly = true
            //    };
            //    panel.Value.Children.Add(l);
            //    panel.Value.Children.Add(t);
            //    CurrentPlayer = Players[0];

            //    //spTurns.Children.Clear();

            //    Label lblTurns = new Label
            //    {
            //        Content = "TURNS"
            //    };

            //    Label total = new Label
            //    {
            //        Content = "Total"
            //    };

            //    StackPanel sp = (StackPanel)this.FindName("spTurns");
            //    sp.Children.Add(lblTurns);
            //    sp.Children.Add(total);
            //}
        }

        #region events

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


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show($"Are you sure you want to end the game and crown {CurrentPlayer.Name} as the winner?",
            //                  "Winner", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    EndGame();
            //}
        }
        #endregion
    }
}
