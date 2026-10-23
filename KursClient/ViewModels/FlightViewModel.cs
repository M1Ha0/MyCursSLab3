using KursClient.Models;
using KursClient.Services;
using KursClient.Utills;
using KursClient.Views;
using KursProjectISP31.Utills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace KursClient.ViewModels
{
    public class FlightViewModel:ViewModelBase
    {
        private FlightService flightService;
        private ObservableCollection<Flight> flightList;
        public ObservableCollection<Flight> FlightList
        {
            get { return flightList; }
            set
            {
                if (flightList != value)
                {
                    flightList = value;
                    OnPropertyChanged(nameof(FlightList));
                }
            }
        }
        private Flight selected;
        public Flight Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
        public FlightViewModel()
        {
            flightService = new FlightService();
            Load();
        }
        private void Load()
        {
            try
            {
                FlightList = null!;
                Task<List<Flight>> task = Task.Run(() => flightService.GetAll());
                FlightList = new ObservableCollection<Flight> (task.Result);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(async obj =>
                  {
                      try
                      {
                          AddEditFlight window = new AddEditFlight(new Flight());
                          if (window.ShowDialog() == true)
                          {
                              Flight l = window.Fligh;
                              l.Date=DateOnly.FromDateTime(window.Date.SelectedDate!.Value);
                              await flightService.Add(window.Fligh);
                              Load();
                          }
                      }
                      catch { }
                  }));
            }
        }
        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand(async obj =>
                  {
                      Flight flight = (obj as Flight)!;
                      AddEditFlight window = new AddEditFlight(flight);
                      if (window.ShowDialog() == true)
                      {
                          Flight l = window.Fligh;
                          l.Date = DateOnly.FromDateTime(window.Date.SelectedDate!.Value);
                          await flightService.Update(window.Fligh);
                          Load();
                      }
                  }));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(async obj =>
                  {
                      Flight flight = (obj as Flight)!;
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить объект " + flight!.NumberFlight + " " + flight.Аirplane, "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                      if (result == MessageBoxResult.Yes)
                      {
                          await flightService.Delete(flight);
                          Load();
                      }
                  }));
            }
        }

    }
}
