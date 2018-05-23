using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ImageServiceGui.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using ImageService.Infrastructure.Enums;

namespace ImageServiceGui.Views_Model
{
    /// <summary>
    /// Class SettingsViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class SettingsViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
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
        /// The m settings model
        /// </summary>
        private SettingsModel m_settingsModel;
        /// <summary>
        /// The m vm outpur dir
        /// </summary>
        private string m_vm_outpurDir;
        /// <summary>
        /// Gets or sets the vm output dir.
        /// </summary>
        /// <value>The vm output dir.</value>
        public string VM_OutputDir
        {
            get { return this.m_settingsModel.OutputDir; }
            set
            {
                this.m_settingsModel.OutputDir = value;
            }
        }
        /// <summary>
        /// The m vm source name
        /// </summary>
        private string m_vm_sourceName;
        /// <summary>
        /// Gets or sets the name of the vm source.
        /// </summary>
        /// <value>The name of the vm source.</value>
        public string VM_SourceName
        {
            get { return this.m_settingsModel.SourceName; }
            set
            {
                this.m_settingsModel.SourceName = value;
            }
        }

        /// <summary>
        /// The m vm log name
        /// </summary>
        private string m_vm_logName;
        /// <summary>
        /// Gets or sets the name of the vm log.
        /// </summary>
        /// <value>The name of the vm log.</value>
        public string VM_LogName
        {
            get { return this.m_settingsModel.LogName; }
            set
            {
                this.m_settingsModel.LogName = value;
            }
        }

        /// <summary>
        /// The m vm thumbnail size
        /// </summary>
        private string m_vm_thumbnailSize;
        /// <summary>
        /// Gets or sets the size of the vm thumbnail.
        /// </summary>
        /// <value>The size of the vm thumbnail.</value>
        public string VM_ThumbnailSize
        {
            get { return this.m_settingsModel.ThumbSize; }
            set
            {
                this.m_settingsModel.ThumbSize = value;
            }
        }
        /// <summary>
        /// The m vm handlers list
        /// </summary>
        private ObservableCollection<string> m_vm_handlersList;
        /// <summary>
        /// Gets or sets the vm handlers list.
        /// </summary>
        /// <value>The vm handlers list.</value>
        public ObservableCollection<string> VM_HandlersList
        {
            get { return m_settingsModel.HandlersList; }
            set { m_settingsModel.HandlersList = value; }
        }

        /// <summary>
        /// The m vm selected handler
        /// </summary>
        private string m_vm_selectedHandler;
        /// <summary>
        /// Gets or sets the vm selected handler.
        /// </summary>
        /// <value>The vm selected handler.</value>
        public string VM_SelectedHandler
        {
            get { return this.m_settingsModel.SelectedHandler; }
            set
            {
                this.m_settingsModel.SelectedHandler = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.m_settingsModel = new SettingsModel();
            m_settingsModel.PropertyChanged += PropertyChangedMethod;
        }

        /// <summary>
        /// Gets or sets the settings model.
        /// </summary>
        /// <value>The settings model.</value>
        public SettingsModel SettingsModel
        {
            get { return this.m_settingsModel; }
            set
            {
                this.m_settingsModel = value;
            }
        }

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand { get; private set; }
        /// <summary>
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Gets the updated list.
        /// </summary>
        /// <returns>System.String.</returns>
        private string getUpdatedList()
        {
            string handlers = "";
            for (int i = 0; i < VM_HandlersList.Count; i++)
            {
                if (i != 0)
                {
                    handlers += ";";
                }
                handlers += VM_HandlersList[i];
            }
            return handlers;
        }
        /// <summary>
        /// Called when [remove].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnRemove(object obj)
        {
            for (int i = 0; i < VM_HandlersList.Count; i++)
            {
                if (VM_HandlersList[i].Equals(VM_SelectedHandler))
                {
                    string handlerToRemove = VM_SelectedHandler;
                    this.VM_HandlersList.RemoveAt(i);

                    string updatedList = getUpdatedList();
                    string[] args = { updatedList ,handlerToRemove};
                    this.m_settingsModel.InformServer
                         (new ImageService.Modal.CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, ""));
                    return;
                }
            }
        }
        /// <summary>
        /// Determines whether this instance can remove the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if this instance can remove the specified object; otherwise, <c>false</c>.</returns>
        private bool CanRemove(object obj)
        {
            return (!string.IsNullOrEmpty(VM_SelectedHandler));
        }
        /// <summary>
        /// Properties the changed method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}
