using PhotoStudio.Model;
using PhotoStudio.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Linq;

namespace PhotoStudio.ViewModels
{
    class EmployeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Employee> employees;
        private Employee selectedEmployee;
        private Employee addedEmployee;
        private ObservableCollection<Position> _positions;
        private Position selectedPosition;

        public Position SelectedPosition
        {
            get { return selectedPosition; }
            set { selectedPosition = value; OnPropertyChanged("SelectedEmployee"); }
        }

        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { _positions = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        public Employee AddedEmployee
        {
            get 
            { return addedEmployee; }
            set
            { addedEmployee = value; OnPropertyChanged(); }
        }

        public EmployeeViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            AddedEmployee = new Employee();
            AddedEmployee.Birthday = DateTime.Now;
            Positions = new ObservableCollection<Position>(db.Positions);
            Employees = new ObservableCollection<Employee>(db.Employees.Include(p => p.Position));
        }

        public ICommand AddEmployee
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        if (SelectedPosition == null)
                        {
                            MessageBox.Show("Ошибка заполнения полей!");
                        }
                        else
                        {
                            Position position = db.Positions.FirstOrDefault(p => p.Name == SelectedPosition.Name);
                            db.Employees.AddRange(new Employee
                            {
                                Sername = AddedEmployee.Sername,
                                Name = AddedEmployee.Sername,
                                Patronymic = AddedEmployee.Patronymic,
                                Position = position,
                                Birthday = AddedEmployee.Birthday,
                                PhoneNumber = AddedEmployee.PhoneNumber
                            });
                            db.SaveChanges();
                            AddedEmployee = new Employee();
                            position = new Position();
                            SelectedPosition = null;
                            Positions = new ObservableCollection<Position>(db.Positions);
                            Employees = new ObservableCollection<Employee>(db.Employees.Include(p => p.Position));
                            AddedEmployee.Birthday = DateTime.Now;
                        }
                    }
                });
            }
        }

        public ICommand EditEmployee
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        if (SelectedEmployee == null)
                        {
                            MessageBox.Show("Вы не выбрали поле!");
                        }
                        else 
                        {
                            Position position = db.Positions.FirstOrDefault(p => p.Name == SelectedEmployee.Position.Name);
                            Employee employee = db.Employees.FirstOrDefault(u => u.Id == SelectedEmployee.Id);
                            employee.Sername = SelectedEmployee.Sername;
                            employee.Name = SelectedEmployee.Name;
                            employee.Patronymic = SelectedEmployee.Patronymic;
                            employee.Position = position;
                            employee.Birthday = SelectedEmployee.Birthday;
                            employee.PhoneNumber = SelectedEmployee.PhoneNumber;
                            db.SaveChanges();
                            SelectedEmployee = null;
                            Positions = new ObservableCollection<Position>(db.Positions);
                            Employees = new ObservableCollection<Employee>(db.Employees.Include(p => p.Position));
                            AddedEmployee.Birthday = DateTime.Now;
                        }
                    }
                });
            }
        }

        public ICommand DeleteEmployee
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedEmployee != null)
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        db.Employees.Remove(SelectedEmployee);
                        db.SaveChanges();
                        Employees = new ObservableCollection<Employee>(db.Employees.Include(p => p.Position));
                        AddedEmployee.Birthday = DateTime.Now;
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
