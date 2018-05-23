using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageServiceGui.Model;
using ImageService.Logging;

namespace ImageServiceDesktopApp.Model
{
    /// <summary>
    /// Interface ILogModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ILogModel : INotifyPropertyChanged
    {
        // List of all the event log entries.
        /// <summary>
        /// Gets or sets the log list.
        /// </summary>
        /// <value>The log list.</value>
        ObservableCollection<LogEntry> LogList { get; set; }

    }
}