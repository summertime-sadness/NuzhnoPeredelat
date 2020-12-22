using Microsoft.EntityFrameworkCore;
using PhotoStudio.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhotoStudio.ViewModels
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<Employee> Employees { get; set; }
        private Employee selectedEmployee { get; set; }
        private ObservableCollection<Order> orders { get; set; }

        public ObservableCollection<Order> Orders 
        {
            get { return orders; }
            set { orders = value; OnPropertyChanged(); } 
        }

        public Employee SelectedEmployee 
        {
            get { return selectedEmployee; }
            set 
            {
                selectedEmployee = value;
                PhotoStudioContext db = new PhotoStudioContext();
                Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1 && o.Employee.Name == selectedEmployee.Name).OrderBy(o => o.DateToWork.AddMinutes(o.TimeToWork.Minute)));
                OnPropertyChanged(); 
            }
        }

        public TimeTableViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            Employees = new ObservableCollection<Employee>(db.Employees.Include(u => u.Orders));
        }
    }
}
