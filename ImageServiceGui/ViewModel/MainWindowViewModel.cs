
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
    /// <summary>
    /// Class MainWindowViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class MainWindowViewModel : INotifyPropertyChanged
    {
        // implement the iINotifyPropertyChanged interface
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
        /// The m vm is server connected
        /// </summary>
        private bool m_vm_isServerConnected;
        /// <summary>
        /// Gets or sets a value indicating whether [vm is server connected].
        /// </summary>
        /// <value><c>true</c> if [vm is server connected]; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
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
        /// <summary>
        /// The m main win model
        /// </summary>
        private MainWindowModel m_mainWinModel;
        /// <summary>
        /// Gets the window closing.
        /// </summary>
        /// <value>The window closing.</value>
        public ICommand WindowClosing { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.m_mainWinModel = new MainWindowModel();
            VM_IsServerConnected = m_mainWinModel.IsServerConnected;
            m_mainWinModel.PropertyChanged += PropertyChangedMethod;
            this.WindowClosing = new DelegateCommand<object>(this.OnWindowClosing);

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
        /// Called when [window closing].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnWindowClosing(object obj)
        {
            CommandRecievedEventArgs e =new CommandRecievedEventArgs((int)CommandEnum.CloseGui, null, null);
            m_mainWinModel.NotifyServer(e);
        }
    }
}
