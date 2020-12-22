using PhotoStudio.Commands;
using PhotoStudio.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PhotoStudio.ViewModels
{

    class MainViewModel : INotifyPropertyChanged // интерфейс для отслеживания смены значения, реализуется 5 строками ниже
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        UserControl currentView; //поле для получения конкретного представления которое будет выведено

        public UserControl CurrentView // свойство реализующее поле выше
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged(); // метод который отслеживает изменение свойства, реализуется интерфейсом указанным в самом начале
            }
        }

        DelegateCommand changeCurrentViewCmd;//поле реализующее команду

        public DelegateCommand ChangeCurrentViewCommand // команда на смену представления
        {

            get => changeCurrentViewCmd ?? (changeCurrentViewCmd = new DelegateCommand(obj => ChangeCurrentView((int)obj)));
        }

        private void ChangeCurrentView(int i) // перечисления представлений по номеру, в MainViewModel, можно увидеть номера под некоторым кнопками
        {
            switch (i)
            {
                case 1:
                    CurrentView = new TimeTableView();
                    break;
                case 2:
                    CurrentView = new OrderView();
                    break;
                case 3:
                    CurrentView = new ServiceView();
                    break;
                case 4:
                    CurrentView = new ClientView();
                    break;
                case 5:
                    CurrentView = new ProfitView();
                    break;
                case 6:
                    CurrentView = new EmployeeView();
                    break;
                case 7:
                    CurrentView = new LocationView();
                    break;
                case 8:
                    CurrentView = new PositionView();
                    break;
                default:
                    MessageBox.Show("Ошибка открытия окна!");
                    break;
            }
        }
    }
}
