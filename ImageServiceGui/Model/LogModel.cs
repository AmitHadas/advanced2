﻿using ImageService.Infrastructure.Enums;
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

namespace ImageServiceGui.Model
{
    class LogModel : ILogModel
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public GuiClientSingleton GuiClient { get; }

        private ObservableCollection<Log> m_logList;
        public ObservableCollection<Log> LogList
        {
            get { return this.m_logList; }
            set
            {
                this.m_logList = value;
                OnPropertyChanged("Log List");
            }
        }

        public LogModel()
        {
            this.GuiClient = GuiClientSingleton.ClientInsatnce;
            //this.GuiClient.ReceivedCommand();
            this.GuiClient.UpdateResponse += UpdateResponse;
            this.InitializeLog();
        }

        private void InitializeLog()
        {
            try
            {
              
                LogList = new ObservableCollection<Log>();
                Object thisLock = new Object();
                //לברררררר
                BindingOperations.EnableCollectionSynchronization(LogList, thisLock);
                string[] array = new string[5];
                CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetLogList, array, "");
              //  this.GuiClient.SendCommand(request);
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
                        UpdateLogList(e.Args[0]);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        public void UpdateLogList(string args)
        {
            this.LogList = JsonConvert.DeserializeObject<ObservableCollection<Log>>(args);
        }
    }
}
