using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    class LogModel : INotifyPropertyChanged
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Log> m_logList;
        public ObservableCollection<Log> LogList
        {
            get { return this.m_logList; }
            set
            {
                this.m_logList = value;
                OnPropertyChanged("Log List");
            }
        }

    }
}
