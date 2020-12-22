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
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhotoStudio.ViewModels
{
    class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private ObservableCollection<Order> _orders; // поле всех заказов
        private Order _selectedOrder; // поле выбранного заказа
        private Order _addedOrder; // поле для получения заполненного нового заказа
        private ObservableCollection<Service> _services; // поле получающее все услуги
        private Service _selectedService; // поле получающее выбранную услугу для заказа
        private ObservableCollection<Location> _locations; // поле всех локаций
        private Location _selectedLocation; // поле выбранной локации
        private ObservableCollection<Client> _clients;
        private Client _selectedClient;
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; OnPropertyChanged(); }
        } // свойство реализующее поле получения заказов

        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; OnPropertyChanged(); }
        }// свойство реализующее поле получения выбранного заказа
        // аналогичной ниже
        public Order AddedOrder
        {
            get { return _addedOrder; }
            set { _addedOrder = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Service> Services
        {
            get { return _services; }
            set { _services = value; OnPropertyChanged(); }
        }

        public Service SelectedService
        {
            get { return _selectedService; }
            set { _selectedService = value; OnPropertyChanged(); }
        }

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

        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { _clients = value; OnPropertyChanged(); }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged(); }
        }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        public OrderViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext(); // получаем контекст данных, проще говоря получаем данные из БД
            AddedOrder = new Order(); // присваиваем тип заказа заполненному заказу
            AddedOrder.DateToWork = DateTime.Now; // FIX бага с датами
            Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
            Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
            Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
            Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
            Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1)); // получаем все не завершённые заказы
            var thread = new Thread(o => {
                while (true)
                {
                    DeleteTimerOrder();
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            });
            thread.Start();
        }

        public void DeleteTimerOrder()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            Order orders = db.Orders.Include(s => s.Service).FirstOrDefault(o => o.DateToWork.Date == DateTime.Now.Date && o.TimeToWork.AddMinutes(o.Service.TimeWork).TimeOfDay < DateTime.Now.TimeOfDay && o.Status == 1);
            if (orders != null)
            {
                orders.Status = 0;
                db.SaveChanges();
                Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1)); // получаем все не завершённые заказы
            }
        }

        public ICommand DeleteOrder // команда удаления
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedOrder != null) // если строка не выбрана и мы пытаемся удалить
                    {
                        PhotoStudioContext db = new PhotoStudioContext();
                        var deleteOrder = db.Orders.FirstOrDefault(l => l.Id == SelectedOrder.Id); // находим нужную строку удаления с помощью выбранной строки
                        db.Remove(deleteOrder); // удаляем строку
                        db.SaveChanges(); // сохраняем изменения
                        Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1)); // передаём изменения в коллекцию данных
                        SelectedOrder = null;
                        AddedOrder.DateToWork = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand AddOrder // команда добавления заказа
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    using (PhotoStudioContext db = new PhotoStudioContext()) // директира using, в данном случае указывает, что в её пределах мы сможем обращаться к контексту
                    {
                        if (SelectedClient == null || SelectedEmployee == null || SelectedService == null || db.Orders.Contains(db.Orders.FirstOrDefault(u => u.Employee.Id == SelectedEmployee.Id && u.DateToWork.Date == AddedOrder.DateToWork.Date && u.TimeToWork.TimeOfDay <= AddedOrder.TimeToWork.TimeOfDay && u.TimeToWork.AddMinutes(u.Service.TimeWork).TimeOfDay >= AddedOrder.TimeToWork.TimeOfDay && u.Status == 1)))
                        {
                            MessageBox.Show("Ошибка заполнения полей или локация / исполнитель уже заняты в этом время!"); // если не все поля заполнены
                        }
                        else
                        {
                            if (SelectedLocation == null)
                            {
                                Client client = db.Clients.FirstOrDefault(p => p.Name == SelectedClient.Name); // получаем выбранного клиента
                                Employee employee = db.Employees.FirstOrDefault(p => p.Name == SelectedEmployee.Name); // получаем выбранного исполнителя
                                Service service = db.Services.FirstOrDefault(p => p.Name == SelectedService.Name); // получаем выбранную услугу
                                db.Orders.AddRange(new Order //добавляем новую строку заказа, каждоая строка соответствует строке заказа
                                {
                                    Service = service,
                                    Count = AddedOrder.Count,
                                    TypePay = AddedOrder.TypePay,
                                    Client = client,
                                    Employee = employee,
                                    Location = null,
                                    DateAdd = DateTime.Now,
                                    DateToWork = AddedOrder.DateToWork,
                                    TimeToWork = AddedOrder.TimeToWork,
                                    PriceEnd = AddedOrder.Count >= service.DiscountCondition ? (service.Price * AddedOrder.Count) - ((service.Price * AddedOrder.Count) * service.Discount / 100) : service.Price * AddedOrder.Count,
                                    Description = AddedOrder.Description,
                                    Status = 1
                                });
                                db.SaveChanges(); // сохраняем данные
                                Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1));
                                SelectedClient = null; // очищаем выбранные данные
                                SelectedEmployee = null;
                                SelectedLocation = null;
                                SelectedService = null;
                                AddedOrder = new Order();
                                Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                                Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                                Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                                Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                                AddedOrder.DateToWork = DateTime.Now;
                                client = null;
                                employee = null;
                                service = null;
                            }
                            else
                            {
                                if (db.Orders.Contains(db.Orders.FirstOrDefault(u => u.Location.Id == SelectedLocation.Id && u.DateToWork.Date == AddedOrder.DateToWork.Date && u.TimeToWork.TimeOfDay <= AddedOrder.TimeToWork.TimeOfDay && u.TimeToWork.AddMinutes(u.Service.TimeWork).TimeOfDay >= AddedOrder.TimeToWork.TimeOfDay && u.Status == 1)))
                                {
                                    MessageBox.Show("Ошибка заполнения полей или локация / исполнитель уже заняты в этом время!");
                                }
                                else
                                {
                                    Client client = db.Clients.FirstOrDefault(p => p.Name == SelectedClient.Name); // получаем выбранного клиента
                                    Employee employee = db.Employees.FirstOrDefault(p => p.Name == SelectedEmployee.Name); // получаем выбранного исполнителя
                                    Location location = db.Locations.FirstOrDefault(p => p.Name == SelectedLocation.Name); // получаем выбранную локацию
                                    Service service = db.Services.FirstOrDefault(p => p.Name == SelectedService.Name); // получаем выбранную услугу
                                    db.Orders.AddRange(new Order //добавляем новую строку заказа, каждоая строка соответствует строке заказа
                                    {
                                        Service = service,
                                        Count = AddedOrder.Count,
                                        TypePay = AddedOrder.TypePay,
                                        Client = client,
                                        Employee = employee,
                                        Location = location,
                                        DateAdd = DateTime.Now,
                                        DateToWork = AddedOrder.DateToWork,
                                        TimeToWork = AddedOrder.TimeToWork,
                                        PriceEnd = AddedOrder.Count >= service.DiscountCondition ? (service.Price * AddedOrder.Count) - ((service.Price * AddedOrder.Count) * service.Discount / 100) : service.Price * AddedOrder.Count,
                                        Description = AddedOrder.Description,
                                        Status = 1
                                    });
                                    db.SaveChanges(); // сохраняем данные
                                    Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1));
                                    SelectedClient = null; // очищаем выбранные данные
                                    SelectedEmployee = null;
                                    SelectedLocation = null;
                                    SelectedService = null;
                                    AddedOrder = new Order();
                                    Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                                    Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                                    Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                                    Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                                    AddedOrder.DateToWork = DateTime.Now;
                                    client = null;
                                    employee = null;
                                    location = null;
                                    service = null;
                                }
                            }
                        }
                    }
                });
            }
        }

        public ICommand EditOrder // изменение заказа
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedOrder != null) // если не выбрали что изменять
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            if (db.Orders.Contains(db.Orders.FirstOrDefault(u => u.Employee.Id == SelectedOrder.Employee.Id && u.DateToWork.Date == SelectedOrder.DateToWork.Date && u.TimeToWork.TimeOfDay <= SelectedOrder.TimeToWork.TimeOfDay && u.TimeToWork.AddMinutes(u.Service.TimeWork).TimeOfDay >= SelectedOrder.TimeToWork.TimeOfDay && u.Status == 1 && u.Id != SelectedOrder.Id))) // если поля для выбора пустые
                            {
                                MessageBox.Show("Ошибка заполнения полей или локация/исполнитель уже заняты в этом время!");
                            }
                            else
                            {
                                var client = db.Clients.FirstOrDefault(p => p.Name == SelectedOrder.Client.Name);
                                var employee = db.Employees.FirstOrDefault(p => p.Name == SelectedOrder.Employee.Name);
                                var location = db.Locations.FirstOrDefault(p => p.Name == SelectedOrder.Location.Name);
                                var service = db.Services.FirstOrDefault(p => p.Name == SelectedOrder.Service.Name);
                                Order _order = db.Orders.FirstOrDefault(u => u.Id == SelectedOrder.Id);
                                _order.Service = service; // устанавливаем изменения
                                _order.Count = SelectedOrder.Count;
                                _order.TypePay = SelectedOrder.TypePay;
                                _order.Client = client;
                                _order.Employee = employee;
                                _order.Location = location;
                                _order.DateToWork = SelectedOrder.DateToWork;
                                _order.Employee = employee;
                                _order.TimeToWork = SelectedOrder.TimeToWork;
                                _order.Description = SelectedOrder.Description;
                                _order.PriceEnd = SelectedOrder.Count >= service.DiscountCondition ? (service.Price * SelectedOrder.Count) - ((service.Price * AddedOrder.Count) * service.Discount / 100) : service.Price * SelectedOrder.Count;
                                db.SaveChanges();
                                Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1));
                                SelectedClient = null; // очищаем данные
                                SelectedEmployee = null;
                                SelectedLocation = null;
                                SelectedService = null;
                                AddedOrder = new Order();
                                Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                                Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                                Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                                Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                                AddedOrder.DateToWork = DateTime.Now;
                                client = null;
                                employee = null;
                                location = null;
                                service = null;
                                SelectedOrder = null;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand SuccessOrder // подтверждение заказа
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedOrder != null)
                    {
                        using (PhotoStudioContext db = new PhotoStudioContext())
                        {
                            Order order = db.Orders.FirstOrDefault(u => u.Id == SelectedOrder.Id);
                            order.Status = 0; // меняем статус на завершённый
                            order.DateEnd = DateTime.Now; // ставим дату завершённости
                            db.SaveChanges();
                            Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1));
                            Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                            Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                            Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                            Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                            AddedOrder.DateToWork = DateTime.Now;
                            SelectedOrder = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку!");
                    }
                });
            }
        }

        public ICommand ClearTable
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    PhotoStudioContext db = new PhotoStudioContext();
                    Orders = new ObservableCollection<Order>(db.Orders.Include(u => u.Service).Include(u => u.Client).Include(u => u.Location).Include(u => u.Employee).Where(o => o.Status == 1));
                    SelectedClient = null; // очищаем выбранные данные
                    SelectedEmployee = null;
                    SelectedLocation = null;
                    SelectedService = null;
                    AddedOrder = new Order();
                    Services = new ObservableCollection<Service>(db.Services.Include(s => s.Orders)); // LINQ запрос на получение всех услуг
                    Clients = new ObservableCollection<Client>(db.Clients.Include(s => s.Orders));// LINQ запрос на получение всех клиентов
                    Locations = new ObservableCollection<Location>(db.Locations.Include(s => s.Orders));// LINQ запрос на получение всех локаций
                    Employees = new ObservableCollection<Employee>(db.Employees.Include(s => s.Orders));// LINQ запрос на получение всех исполнителей
                    AddedOrder.DateToWork = DateTime.Now;
                });
            }
        }
    }
}
