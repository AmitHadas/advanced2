using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageServiceGui.Model;

namespace ImageServiceDesktopApp.Model
{  
    interface ILogModel : INotifyPropertyChanged
    {
        // List of all the event log entries.
        ObservableCollection<Log> LogList { get; set; }

    }
}