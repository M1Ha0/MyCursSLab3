using KursClient.Models;
using KursClient.Services;
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
    /// Логика взаимодействия для AddEditPassenger.xaml
    /// </summary>
    public partial class AddEditPassenger : Window
    {
        public Passenger Passenge { get;  private set; }
        private FlightService _flightService; 
        public AddEditPassenger(Passenger _passenger)
        {
            InitializeComponent();
            Passenge = _passenger;
            DataContext = Passenge;
            _flightService = new FlightService();
             Load();
        }

        private async Task Load()
        {

            NumberFlightsComboBox.ItemsSource = await _flightService.GetAll();
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
