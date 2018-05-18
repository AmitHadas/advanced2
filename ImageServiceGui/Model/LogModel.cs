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
    class LogModel : ILogModel
    {
        public static bool isFirstTime;
        private bool updateLog;
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public GuiClientSingleton GuiClient { get; }

        private ObservableCollection<LogEntry> m_logList;
        public ObservableCollection<LogEntry> LogList
        {
            get { return this.m_logList; }
            set
            {
                this.m_logList = value;
                OnPropertyChanged("Log List");
            }
        }

        public LogModel(){
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

        private void InitializeLog()
        {
            try
            {

                LogList = new ObservableCollection<LogEntry>();
                Object thisLock = new Object();
                //לברררררר
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
                        if (!isFirstTime) {
                            LogEntry newLog = JsonConvert.DeserializeObject<LogEntry>(e.Args[0]);
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                LogList.Insert(0, newLog);
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        public void UpdateLogList(ObservableCollection<LogEntry> logs)
        {
            this.m_logList = logs;
            this.updateLog = true;
        }
    }
}
