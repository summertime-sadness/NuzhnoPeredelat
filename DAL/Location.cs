using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhotoStudio.Model
{
    class Location : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int id;
        private string name;
        private string description;

        [Key]
        public int Id 
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Name {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public string Description {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
    }
}
