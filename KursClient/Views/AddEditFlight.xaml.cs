using KursClient.Models;
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

namespace KursClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEditFlight.xaml
    /// </summary>
    public partial class AddEditFlight : Window
    {
        public Flight Fligh { get;  private set; }
        public AddEditFlight(Flight _flight)
        {
            InitializeComponent();
            Fligh = _flight;
            DataContext = Fligh;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult= false;
        }
    }
}
