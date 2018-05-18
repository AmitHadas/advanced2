
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageServiceGui.Communication;
using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

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
        public ICommand WindowClosing { get; private set; }
        public MainWindowViewModel()
        {
            this.m_mainWinModel = new MainWindowModel();
            VM_IsServerConnected = m_mainWinModel.IsServerConnected;
            m_mainWinModel.PropertyChanged += PropertyChangedMethod;
            this.WindowClosing = new DelegateCommand<object>(this.OnWindowClosing);

        }
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void OnWindowClosing(object obj)
        {
            CommandRecievedEventArgs e =new CommandRecievedEventArgs((int)CommandEnum.CloseGui, null, null);
            m_mainWinModel.NotifyServer(e);
        }
    }
}
