using LiveCharts;
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
    class ProfitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ChartValues<int> profitValues { get; set; }
        public int FullPrice { get; set; }

        public ProfitViewModel()
        {
            PhotoStudioContext db = new PhotoStudioContext();
            profitValues = new ChartValues<int>(db.Orders.Where(u => u.Status == 0 && u.DateEnd.Month == DateTime.Now.Month).Select(u => u.PriceEnd));
            var fp = db.Orders.Where(u => u.Status == 0 && u.DateEnd.Month == DateTime.Now.Month).Select(u => u.PriceEnd);
            foreach (var t in fp)
            {
                FullPrice += t;
            }
        }
    }
}
