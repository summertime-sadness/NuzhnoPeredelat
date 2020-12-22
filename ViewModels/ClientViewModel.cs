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
    class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Client> _Clients;
        private Client _selectedClient;
        private Client _addedClient;

        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public Client AddedClient
        {
            get { return _addedClient; }
            set { _addedClient = value; OnPropertyChanged(); }
        }

        public ClientViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            AddedClient = new Client();
            AddedClient.Birthday = DateTime.Now;
            Clients = new ObservableCollection<Client>(db.Clients);
        }

        public ICommand DeleteClient
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedClient != null)
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        var deleteClient = db.Clients.Include(l => l.Orders).FirstOrDefault(l => l.Id == SelectedClient.Id);
                        db.Remove(deleteClient);
                        db.SaveChanges();
                        Clients = new ObservableCollection<Client>(db.Clients);
                        AddedClient.Birthday = DateTime.Now;
                        SelectedClient = null;
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand AddClient
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext())
                    {
                        db.Add(AddedClient);
                        db.SaveChanges();
                        Clients = new ObservableCollection<Client>(db.Clients);
                        AddedClient.Birthday = DateTime.Now;
                        AddedClient = new Client();
                    }
                });
            }
        }

        public ICommand EditClient
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedClient != null)
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            Client client = db.Clients.FirstOrDefault(u => u.Id == SelectedClient.Id);
                            client.Name = SelectedClient.Name;
                            client.PhoneNumber = SelectedClient.PhoneNumber;
                            client.Birthday = SelectedClient.Birthday;
                            db.SaveChanges();
                            Clients = new ObservableCollection<Client>(db.Clients);
                            AddedClient.Birthday = DateTime.Now;
                            SelectedClient = null;
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
