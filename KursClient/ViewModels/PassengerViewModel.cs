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
    public class PassengerViewModel : ViewModelBase
    {
        private PassengerService passengerService;
        private ObservableCollection<Passenger> passengerList;
        public ObservableCollection<Passenger> PassengerList
        {
            get { return passengerList; }
            set
            {
                if (passengerList != value)
                {
                    passengerList = value;
                    OnPropertyChanged(nameof(PassengerList));
                }
            }
        }
        private Passenger selected;
        public Passenger Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
        public PassengerViewModel()
        {
            passengerService = new PassengerService();
            Load();
        }
        private void Load()
        {
            try
            {
                PassengerList = null!;
                Task<List<Passenger>> task = Task.Run(() => passengerService.GetAll());
                PassengerList = new ObservableCollection<Passenger> (task.Result);
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
                          AddEditPassenger window = new AddEditPassenger(new Passenger());
                          if (window.ShowDialog() == true)
                          {

                              await passengerService.Add(window.Passenge);
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
                      Passenger passenger = (obj as Passenger)!;
                      AddEditPassenger window = new AddEditPassenger(passenger);
                      if (window.ShowDialog() == true)
                      {
                          await passengerService.Update(window.Passenge);
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
                      Passenger passenger = (obj as Passenger)!;
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить объект " + passenger!.IdPassenger + " " + passenger.FirstName, "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                      if (result == MessageBoxResult.Yes)
                      {
                          await passengerService.Delete(passenger);
                          Load();
                      }
                  }));
            }
        }

    }
}
