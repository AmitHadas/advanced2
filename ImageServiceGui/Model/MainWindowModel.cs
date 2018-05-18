using ImageService.Modal;
using ImageServiceGui.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    class MainWindowModel : INotifyPropertyChanged
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private bool updateApp;
        private GuiClientSingleton m_guiClient;
        public GuiClientSingleton GuiClient
        {
            get { return m_guiClient; }
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private bool m_isServerConnect;
        public bool IsServerConnected
        {
            get { return m_isServerConnect; }
            set { m_isServerConnect = value;
                OnPropertyChanged("Server Connected");
            }
        }

        public MainWindowModel()
        {
            m_guiClient = GuiClientSingleton.ClientInsatnce;
            m_isServerConnect = m_guiClient.IsConnected;
        }

        public void NotifyServer(CommandRecievedEventArgs e)
        {
            this.m_guiClient.SendCommand(e);
        }
    }
}
