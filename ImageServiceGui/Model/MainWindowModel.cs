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
    /// <summary>
    /// Class MainWindowModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class MainWindowModel : INotifyPropertyChanged
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The update application
        /// </summary>
        private bool updateApp;
        /// <summary>
        /// The m GUI client
        /// </summary>
        private GuiClientSingleton m_guiClient;
        /// <summary>
        /// Gets the GUI client.
        /// </summary>
        /// <value>The GUI client.</value>
        public GuiClientSingleton GuiClient
        {
            get { return m_guiClient; }
        }
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// The m is server connect
        /// </summary>
        private bool m_isServerConnect;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is server connected.
        /// </summary>
        /// <value><c>true</c> if this instance is server connected; otherwise, <c>false</c>.</value>
        public bool IsServerConnected
        {
            get { return m_isServerConnect; }
            set { m_isServerConnect = value;
                OnPropertyChanged("Server Connected");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowModel"/> class.
        /// </summary>
        public MainWindowModel()
        {
            m_guiClient = GuiClientSingleton.ClientInsatnce;
            m_isServerConnect = m_guiClient.IsConnected;
        }

        /// <summary>
        /// Notifies the server.
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public void NotifyServer(CommandRecievedEventArgs e)
        {
            this.m_guiClient.SendCommand(e);
        }
    }
}
