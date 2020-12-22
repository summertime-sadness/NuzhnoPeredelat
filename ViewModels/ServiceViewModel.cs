using Microsoft.EntityFrameworkCore;
using PhotoStudio.Commands;
using PhotoStudio.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PhotoStudio.ViewModels
{
    class ServiceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Service> _Services;
        private Service _selectedService;
        private Service _addedService;

        public ObservableCollection<Service> Services
        {
            get { return _Services; }
            set { _Services = value; OnPropertyChanged(); }
        }

        public Service SelectedService
        {
            get { return _selectedService; }
            set { _selectedService = value; OnPropertyChanged(); }
        }

        public Service AddedService
        {
            get { return _addedService; }
            set { _addedService = value; OnPropertyChanged(); }
        }

        public ServiceViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            AddedService = new Service();
            Services = new ObservableCollection<Service>(db.Services);
        }

        public ICommand DeleteService
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedService != null)
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        var deleteService = db.Services.Include(l => l.Orders).FirstOrDefault(l => l.Id == SelectedService.Id);
                        db.Remove(deleteService);
                        db.SaveChanges();
                        Services = new ObservableCollection<Service>(db.Services);
                        SelectedService = new Service();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand AddService
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        db.Add(AddedService);
                        db.SaveChanges();
                        Services = new ObservableCollection<Service>(db.Services);
                        AddedService = null;
                    }
                });
            }
        }

        public ICommand EditService
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedService != null)
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            Service service = db.Services.FirstOrDefault(u => u.Id == SelectedService.Id);
                            service.Name = SelectedService.Name;
                            service.Schedule = SelectedService.Schedule;
                            db.SaveChanges();
                            Services = new ObservableCollection<Service>(db.Services);
                            SelectedService = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }
    }
}
