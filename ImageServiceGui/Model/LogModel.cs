using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageServiceDesktopApp.Model;
using ImageServiceGui.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace ImageServiceGui.Model
{
    /// <summary>
    /// Class LogModel.
    /// </summary>
    /// <seealso cref="ImageServiceDesktopApp.Model.ILogModel" />
    class LogModel : ILogModel
    {
        /// <summary>
        /// The is first time
        /// </summary>
        public static bool isFirstTime;
        /// <summary>
        /// The update log
        /// </summary>
        private bool updateLog;
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
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
        /// The m log list
        /// </summary>
        private ObservableCollection<LogEntry> m_logList;
        /// <summary>
        /// Gets or sets the log list.
        /// </summary>
        /// <value>The log list.</value>
        public ObservableCollection<LogEntry> LogList
        {
            get { return this.m_logList; }
            set
            {
                this.m_logList = value;
                OnPropertyChanged("Log List");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogModel"/> class.
        /// </summary>
        public LogModel()
        {
            this.updateLog = false;
            isFirstTime = true;
            this.GuiClient = GuiClientSingleton.ClientInsatnce;
            if (GuiClient.IsConnected)
            {
                this.GuiClient.ReceivedCommand();
                this.GuiClient.UpdateResponse += UpdateResponse;
                this.InitializeLog();
                while (!updateLog) { }
            }
        }

        /// <summary>
        /// Initializes the log.
        /// </summary>
        private void InitializeLog()
        {
            try
            {

                LogList = new ObservableCollection<LogEntry>();
                Object thisLock = new Object();
                //  BindingOperations.EnableCollectionSynchronization(LogList, thisLock);
                string[] array = new string[5];
                CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetLogList, array, "");
                this.GuiClient.SendCommand(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// Updates the response.
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        private void UpdateResponse(CommandRecievedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (e.CommandID == (int)CommandEnum.GetLogList)
                    {
                        ObservableCollection<LogEntry> logsList = JsonConvert.DeserializeObject<ObservableCollection<LogEntry>>(e.Args[0]);
                        UpdateLogList(logsList);
                        isFirstTime = false;
                    }
                    if (e.CommandID == (int)CommandEnum.LogCommand)
                    {
                        if (!isFirstTime)
                        {
                            LogEntry newLog = JsonConvert.DeserializeObject<LogEntry>(e.Args[0]);
                            //App.Current.Dispatcher.Invoke((System.Action)delegate
                            //{
                            //    LogList.Insert(0, newLog);
                            //});
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        /// <summary>
        /// Updates the log list.
        /// </summary>
        /// <param name="logs">The logs.</param>
        public void UpdateLogList(ObservableCollection<LogEntry> logs)
        {
            this.m_logList = logs;
            this.updateLog = true;
        }
    }
}