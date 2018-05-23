using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    /// <summary>
    /// Interface ISettingsModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface ISettingsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        string OutputDir { get; set; }
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>The name of the source.</value>
        string SourceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>The name of the log.</value>
        string LogName { get; set; }
        /// <summary>
        /// Gets or sets the size of the thumb.
        /// </summary>
        /// <value>The size of the thumb.</value>
        string ThumbSize { get; set; }
        /// <summary>
        /// Gets or sets the handlers list.
        /// </summary>
        /// <value>The handlers list.</value>
        ObservableCollection<string> HandlersList { get; set; }
        /// <summary>
        /// Gets or sets the selected handler.
        /// </summary>
        /// <value>The selected handler.</value>
        string SelectedHandler { get; set; }
       // IImageServiceClient GuiClient { get; set; }

    }
}
