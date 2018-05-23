using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModel
{
    /// <summary>
    /// Interface ISettingsModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ISettingsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the vm output dir.
        /// </summary>
        /// <value>The vm output dir.</value>
        string VM_OutputDir { get; set; }
        /// <summary>
        /// Gets or sets the name of the vm source.
        /// </summary>
        /// <value>The name of the vm source.</value>
        string VM_SourceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the vm log.
        /// </summary>
        /// <value>The name of the vm log.</value>
        string VM_LogName { get; set; }
        /// <summary>
        /// Gets or sets the size of the vm tumbnail.
        /// </summary>
        /// <value>The size of the vm tumbnail.</value>
        string VM_TumbnailSize { get; set; }
        /// <summary>
        /// Gets or sets the vm handlers list.
        /// </summary>
        /// <value>The vm handlers list.</value>
        ObservableCollection<string> VM_HandlersList { get; set; }
        /// <summary>
        /// Gets or sets the vm selected handler.
        /// </summary>
        /// <value>The vm selected handler.</value>
        string VM_SelectedHandler { get; set; }
    }
}
