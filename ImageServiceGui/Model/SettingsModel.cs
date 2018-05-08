using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageServiceGui.ViewModel;

namespace ImageServiceGui.Model
{
    class SettingsModel : ISettingsViewModel
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        private string m_outputDir;
        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("Output Directory");
            }
        }

        private string m_sourceName;
        public string SourceName
        {
            get
            {
                return m_sourceName;
            }
            set {
                m_sourceName = value;
                OnPropertyChanged("Source Name");
            }
        }

        private string m_logName;
        public string LogName
        {
            get
            {
                return m_logName;
            }
            set
            {
                m_logName = value;
                OnPropertyChanged("Log Name");
            }
        }

        private string m_thumbSize;
        public string ThumbSize
        {
            get
            {
                return m_thumbSize;
            }
            set
            {
                m_thumbSize = value;
                OnPropertyChanged("Thumbnail Size");
            }
        }

        private ObservableCollection<string> m_handlersList;
        public ObservableCollection<string> HandlersList
        {
            get { return m_handlersList; }
            set
            {
                m_handlersList = value;
                OnPropertyChanged("Handlers List");
            }
        }
        private string m_selectedHandler;
        public string SelectedHandler
        {
            get { return m_selectedHandler; }
            set
            {
                m_selectedHandler = value;
                OnPropertyChanged("Selected Handler");
            }
        }

    }
}
