using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OutputDir { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string TumbSize { get; set; }
        ObservableCollection<string> HandlersList { get; set; }
        string SelectedHandler { get; set; }
       // IImageServiceClient GuiClient { get; set; }

    }
}
