using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    class LogEntry : INotifyPropertyChanged
    {

        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        private string m_type;
        public string Type
        {
            get { return this.m_type; }
            set
            {
                this.m_type = value;
                OnPropertyChanged("Type");
            }
        }
        private string m_info;
        public string Info
        {
            get { return this.m_info; }
            set
            {
                this.m_info = value;
                OnPropertyChanged("Info");
            }
        }

        public LogEntry(string type, string info)
        {
            this.m_type = type;
            this.m_info = info;
        }
    }
}
