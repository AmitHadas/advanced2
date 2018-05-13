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
    class LogViewModel : ILogViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private LogModel m_logModel;
        private ObservableCollection<LogEntry> m_vm_logList;
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
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
        public LogViewModel()
        {
            this.m_logModel = new LogModel();
            m_logModel.PropertyChanged += PropertyChangedMethod;
            m_logModel.LogList = new ObservableCollection<LogEntry> { new LogEntry("INFO", "AMIT HADAS"),
                new LogEntry("WARNING", "NOA OR"), new LogEntry("ERROR", "HI")};
        }
    }
}