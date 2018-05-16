using ImageServiceGui.Communication;
using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private bool m_vm_isServerConnected;
        public bool VM_IsServerConnected
        {
            get
            {
                return m_mainWinModel.IsServerConnected;
            }
            set
            {
                m_mainWinModel.IsServerConnected = value;

            }
        }
        public string BackgroundColor
        {
            get { if (VM_IsServerConnected)
                {
                    return "Green";
                } else
                {
                    return "Gray";
                }
            }
        }
        private MainWindowModel m_mainWinModel;
        
        public MainWindowViewModel()
        {
            this.m_mainWinModel = new MainWindowModel();
            VM_IsServerConnected = m_mainWinModel.IsServerConnected;
            m_mainWinModel.PropertyChanged += PropertyChangedMethod;

        }
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

    }
}
