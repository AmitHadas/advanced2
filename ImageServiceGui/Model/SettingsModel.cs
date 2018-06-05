using System;
using System.ComponentModel;
using ImageServiceGui.Communication;
using System.Windows.Data;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace ImageServiceGui.Model
{
    /// <summary>
    /// Class SettingsModel.
    /// </summary>
    /// <seealso cref="ImageServiceGui.Model.ISettingsModel" />
    class SettingsModel : ISettingsModel
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The update application
        /// </summary>
        private bool updateApp;
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
        /// Gets the GUI client.
        /// </summary>
        /// <value>The GUI client.</value>
        public GuiClientSingleton GuiClient { get; }
        /// <summary>
        /// The m output dir
        /// </summary>
        private string m_outputDir;
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("Output Directory");
            }
        }

        /// <summary>
        /// The m source name
        /// </summary>
        private string m_sourceName;
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>The name of the source.</value>
        public string SourceName
        {
            get
            {
                return m_sourceName;
            }
            set
            {
                m_sourceName = value;
                OnPropertyChanged("Source Name");
            }
        }

        /// <summary>
        /// The m log name
        /// </summary>
        private string m_logName;
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>The name of the log.</value>
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

        /// <summary>
        /// The m thumb size
        /// </summary>
        private string m_thumbSize;
        /// <summary>
        /// Gets or sets the size of the thumb.
        /// </summary>
        /// <value>The size of the thumb.</value>
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

        /// <summary>
        /// The m handlers list
        /// </summary>
        private ObservableCollection<string> m_handlersList;
        /// <summary>
        /// Gets or sets the handlers list.
        /// </summary>
        /// <value>The handlers list.</value>
        public ObservableCollection<string> HandlersList
        {
            get { return m_handlersList; }
            set
            {
                m_handlersList = value;
                OnPropertyChanged("Handlers List");
            }
        }
        /// <summary>
        /// The m selected handler
        /// </summary>
        private string m_selectedHandler;
        /// <summary>
        /// Gets or sets the selected handler.
        /// </summary>
        /// <value>The selected handler.</value>
        public string SelectedHandler
        {
            get { return m_selectedHandler; }
            set
            {
                m_selectedHandler = value;
                OnPropertyChanged("Selected Handler");
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsModel"/> class.
        /// </summary>
        public SettingsModel()
        {
            this.updateApp = false;
            this.GuiClient = GuiClientSingleton.ClientInsatnce;
            if (GuiClient.IsConnected)
            {
                this.GuiClient.ReceivedCommand();
                this.GuiClient.UpdateResponse += UpdateResponse;
                this.InitializeSettings();
                while (!updateApp) { }
            }
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void RemoveHandler(string [] args)
        {
            string[] directories = args[0].Split(';');
            foreach (var dir in HandlersList)
            {
                if (!directories.Contains(dir))
                {
                   
                    Dispatcher.CurrentDispatcher.Invoke((System.Action)delegate

                    {
                        HandlersList.Remove(dir);
                    });
                }
            }
        }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        private void InitializeSettings()
        {
            try
            {
                this.OutputDir = string.Empty;
                this.SourceName = string.Empty;
                this.LogName = string.Empty;
                this.ThumbSize = string.Empty;
                HandlersList = new ObservableCollection<string>();
                Object thisLock = new Object();
                BindingOperations.EnableCollectionSynchronization(HandlersList, thisLock);
                CommandRecievedEventArgs request =
                    new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
                this.GuiClient.SendCommand(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the configuration.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void UpdateConfig(string[] args)
        {
            this.OutputDir = args[0];
            this.SourceName = args[1];
            this.LogName = args[2];
            this.ThumbSize = args[3];

            string[] directories = args[4].Split(';');
            HandlersList = new ObservableCollection<string>();
            foreach (var dir in directories)
            {
                this.HandlersList.Add(dir);
            }
            this.updateApp = true;
        }
        /// <summary>
        /// Updates the response.
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void UpdateResponse(CommandRecievedEventArgs e)
        {
            try
            {
                string[] args;
                if (e != null)
                {
                    if (e.CommandID == (int)CommandEnum.GetConfigCommand)
                    {
                        args = e.Args;
                        UpdateConfig(args);
                    }
                    else if (e.CommandID == (int)CommandEnum.CloseHandler)
                    {
                        RemoveHandler(e.Args);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        /// <summary>
        /// Informs the server.
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public void InformServer(CommandRecievedEventArgs e)
        {
            this.GuiClient.SendCommand(e);
        }

    }
}
