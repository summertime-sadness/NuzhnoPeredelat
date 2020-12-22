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
    class PositionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Position> _positions;
        private Position _selectedPosition;
        private Position _addedPosition;

        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { _positions = value; OnPropertyChanged(); }
        }

        public Position SelectedPosition
        {
            get { return _selectedPosition; }
            set { _selectedPosition = value; OnPropertyChanged(); }
        }

        public Position AddedPosition
        {
            get { return _addedPosition; }
            set { _addedPosition = value; OnPropertyChanged(); }
        }

        public PositionViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            AddedPosition = new Position();
            Positions = new ObservableCollection<Position>(db.Positions);
        }

        public ICommand DeletePosition
        {
            get
            {
                return new DelegateCommand((obj)=>
                {
                    if (SelectedPosition != null)
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        var deletePosition = db.Positions.Include(e => e.Employees).FirstOrDefault(p => p.Id == SelectedPosition.Id);
                        db.Remove(deletePosition);
                        db.SaveChanges();
                        Positions = new ObservableCollection<Position>(db.Positions);
                        SelectedPosition = null;
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand AddPosition
        {
            get
            {
                return new DelegateCommand((obj)=>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        db.Add(AddedPosition);
                        db.SaveChanges();
                        Positions = new ObservableCollection<Position>(db.Positions);
                        AddedPosition = new Position();
                    }
                });
            }
        }

        public ICommand EditPosition
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedPosition != null)
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            Position position = db.Positions.FirstOrDefault(u => u.Id == SelectedPosition.Id);
                            position.Name = SelectedPosition.Name;
                            db.SaveChanges();
                            SelectedPosition = null;
                            Positions = new ObservableCollection<Position>(db.Positions);
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
