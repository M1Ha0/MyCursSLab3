using KursClient.Utills;
using KursProjectISP31.Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KursClient.ViewModels
{
    public class NavigationViewModel:ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand HomeCommand { get; set; }
        public ICommand FlightCommand { get; set; }
        public ICommand PassengerCommand { get; set; }
        private void HomeView(object obj) => CurrentView = new HomeViewModel();
        private void FlightView(object obj) => CurrentView = new FlightViewModel();
        private void PassengerView(object obj) => CurrentView = new PassengerViewModel();
        public NavigationViewModel()
        {
            HomeCommand = new RelayCommand(HomeView);
            FlightCommand = new RelayCommand(FlightView);
            PassengerCommand = new RelayCommand(PassengerView);
            CurrentView = new HomeViewModel();
        }
    }
}
