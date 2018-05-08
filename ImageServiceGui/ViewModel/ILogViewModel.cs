using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModel
{
    interface ILogViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Log> VM_LogList { get; set; }

    }
}
