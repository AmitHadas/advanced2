using ImageServiceGui.Model;
using ImageServiceGui.ViewModel;
using System;
using ImageService.Logging.Modal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;

namespace ImageServiceGui.Views_Model
{
    /// <summary>
    /// Class LogViewModel.
    /// </summary>
    /// <seealso cref="ImageServiceGui.ViewModel.ILogViewModel" />
    class LogViewModel : ILogViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// The m log model
        /// </summary>
        private LogModel m_logModel;
        /// <summary>
        /// The m vm log list
        /// </summary>
        private ObservableCollection<LogEntry> m_vm_logList;
        /// <summary>
        /// Gets or sets the vm log list.
        /// </summary>
        /// <value>The vm log list.</value>
        public ObservableCollection<LogEntry> VM_LogList
        {
            get
            {
                return this.m_logModel.LogList;
            }
            set
            {
                this.m_logModel.LogList = value;
            }
        }
        /// <summary>
        /// Properties the changed method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewModel"/> class.
        /// </summary>
        public LogViewModel()
        {
            this.m_logModel = new LogModel();
            m_logModel.PropertyChanged += PropertyChangedMethod;
            
        }
    }
}