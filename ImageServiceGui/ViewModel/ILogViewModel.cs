using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;


namespace ImageServiceGui.ViewModel
{
    /// <summary>
    /// Interface ILogViewModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ILogViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the vm log list.
        /// </summary>
        /// <value>The vm log list.</value>
        ObservableCollection<LogEntry> VM_LogList { get; set; }

    }
}
