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
    class Order : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int id;
        private int count;
        private int priceEnd;
        private string typePay;
        private int status;
        private DateTime dateAdd;
        private DateTime dateToWork;
        private DateTime timeToWork;
        private DateTime dateEnd;
        private Service service;
        private Employee? employee;
        private Client? client;
        private Location? location;
        private string description;

        [Key]
        public int Id 
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public int Count 
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); }
        }
        public int PriceEnd 
        {
            get { return priceEnd; }
            set { priceEnd = value; OnPropertyChanged(); }
        }
        public string TypePay 
        {
            get { return typePay; }
            set { typePay = value; OnPropertyChanged(); }
        }
        public int Status 
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }
        public DateTime DateAdd 
        {
            get { return dateAdd; }
            set { dateAdd = value; OnPropertyChanged(); }
        }
        public DateTime DateToWork 
        {
            get { return dateToWork; }
            set { dateToWork = value; OnPropertyChanged(); }
        }
        public DateTime TimeToWork
        {
            get { return timeToWork; }
            set { timeToWork = value; OnPropertyChanged(); }
        }
        public DateTime DateEnd
        {
            get { return dateEnd; }
            set { dateEnd = value; OnPropertyChanged(); }
        }
        public Service Service 
        {
            get { return service; }
            set { service = value; OnPropertyChanged(); }
        }
        public Employee? Employee 
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged(); }
        }
        public Client? Client 
        {
            get { return client; }
            set { client = value; OnPropertyChanged(); }
        }
        public Location? Location 
        {
            get { return location; }
            set { location = value; OnPropertyChanged(); }
        }
        public string Description 
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
    }
}
