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
using System.Windows.Shapes;

namespace DartMaster9000
{
    /// <summary>
    /// Logique d'interaction pour PlayerGestion.xaml
    /// </summary>
    public partial class PlayerGestion : Window
    {
        public PlayerGestionViewModel vm { get; set; }
        public PlayerGestion()
        {
            vm = new PlayerGestionViewModel();
            DataContext = vm;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(vm.InGamePlayers.Count == 0)
            {
                MessageBox.Show("No players selected");
                return;
            }
            MainWindow main = new MainWindow(vm.InGamePlayers.ToList());
            main.Show();
            this.Close();
        }
    }
}
