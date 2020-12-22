using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhotoStudio.Model
{
    class Service : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int id;
        private string name;
        private int price;
        private int timeWork;
        private int discount;
        private int discountCondition;
        private bool schedule;

        [Key]
        public int Id 
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Name 
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public int Price 
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }
        public int TimeWork 
        {
            get { return timeWork; }
            set { timeWork = value; OnPropertyChanged(); }
        }
        public int Discount 
        {
            get { return discount; }
            set { discount = value; OnPropertyChanged(); }
        }
        public int DiscountCondition 
        {
            get { return discountCondition; }
            set { discountCondition = value; OnPropertyChanged(); }
        }
        public bool Schedule 
        {
            get { return schedule; }
            set { schedule = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
    }
}
