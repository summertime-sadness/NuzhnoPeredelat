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
    class LocationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Location> _locations;
        private Location _selectedLocation;
        private Location _addedLocation;

        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set { _locations = value; OnPropertyChanged(); }
        }

        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set { _selectedLocation = value; OnPropertyChanged(); }
        }

        public Location AddedLocation
        {
            get { return _addedLocation; }
            set { _addedLocation = value; OnPropertyChanged(); }
        }

        public LocationViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            AddedLocation = new Location();
            Locations = new ObservableCollection<Location>(db.Locations);
        }

        public ICommand DeleteLocation
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedLocation != null)
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        var deleteLocation = db.Locations.Include(l => l.Orders).FirstOrDefault(l => l.Id == SelectedLocation.Id);
                        db.Remove(deleteLocation);
                        db.SaveChanges();
                        Locations = new ObservableCollection<Location>(db.Locations);
                        SelectedLocation = null;
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand AddLocation
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        db.Add(AddedLocation);
                        db.SaveChanges();
                        Locations = new ObservableCollection<Location>(db.Locations);
                        AddedLocation = new Location();
                    }
                });
            }
        }

        public ICommand EditLocation
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedLocation != null)
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            Location location = db.Locations.FirstOrDefault(u => u.Id == SelectedLocation.Id);
                            location.Name = SelectedLocation.Name;
                            location.Description = SelectedLocation.Description;
                            db.SaveChanges();
                            Locations = new ObservableCollection<Location>(db.Locations);
                            SelectedLocation = null;
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
