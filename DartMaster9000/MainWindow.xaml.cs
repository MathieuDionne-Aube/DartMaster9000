using DartMaster9000.Class;
using DartMaster9000.CustomControls;
using DartMaster9000.ViewModel;
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
        #endregion
    }
}
