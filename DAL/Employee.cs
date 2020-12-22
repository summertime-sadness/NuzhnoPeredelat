using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhotoStudio.Model
{
    class Employee : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private int id;
        private string sername;
        private string name;
        private string patronymic;
        private Position position;
        private DateTime birthday;
        private string phoneNumber;

        [Key]
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Sername
        {
            get { return sername; }
            set { sername = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public string Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; OnPropertyChanged(); }
        }

        public Position Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(); }
        }
        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; OnPropertyChanged(); }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
    }
}
