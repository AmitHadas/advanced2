using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModel
{
    interface ISettingsViewModel : INotifyPropertyChanged
    {
        string VM_OutputDir { get; set; }
        string VM_SourceName { get; set; }
        string VM_LogName { get; set; }
        string VM_TumbnailSize { get; set; }
        ObservableCollection<string> VM_HandlersList { get; set; }
        string VM_SelectedHandler { get; set; }
    }
}
