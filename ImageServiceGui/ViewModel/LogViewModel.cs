using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Views_Model
{
    class LogViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private LogModel m_logModel;
        private ObservableCollection<Log> m_vm_logList;
        public ObservableCollection<Log> VM_LogList
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
            m_logModel.LogList = new ObservableCollection<Log> { new Log("INFO", "AMIT HADAS"),
                new Log("WARNING", "NOA OR"), new Log("ERROR", "HI")};
        }
    }
}