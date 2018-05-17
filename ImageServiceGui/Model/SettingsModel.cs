using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageServiceGui.ViewModel;
using ImageServiceGui.Communication;
using System.Windows.Data;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;

namespace ImageServiceGui.Model
{
    class SettingsModel : ISettingsModel
    {
        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private bool updateApp;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public GuiClientSingleton GuiClient { get; }
        private string m_outputDir;
        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("Output Directory");
            }
        }

        private string m_sourceName;
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

        private string m_logName;
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

        private string m_thumbSize;
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

        private ObservableCollection<string> m_handlersList;
        public ObservableCollection<string> HandlersList
        {
            get { return m_handlersList; }
            set
            {
                m_handlersList = value;
                OnPropertyChanged("Handlers List");
            }
        }
        private string m_selectedHandler;
        public string SelectedHandler
        {
            get { return m_selectedHandler; }
            set
            {
                m_selectedHandler = value;
                OnPropertyChanged("Selected Handler");
            }
        }
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

        public void RemoveHandler(string [] args)
        {
            //for (int i = 0; i < HandlersList.Count; i++)
            //{
            //    if (HandlersList[i].Equals(handler))
            //    {
            //        this.HandlersList.RemoveAt(i);
            //        return;
            //    }
            //}

            string[] directories = args[0].Split(';');
            //ObservableCollection<string> tempHandlersList = new ObservableCollection<string>();
            //HandlersList.Clear();

            //foreach (var dir in directories)
            //{
            //    HandlersList.Add(dir);
            //}

            foreach (var dir in HandlersList)
            {
                if (!directories.Contains(dir))
                {
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        HandlersList.Remove(dir);
                    });
                }
            }
        }

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

        public void InformServer(CommandRecievedEventArgs e)
        {
            this.GuiClient.SendCommand(e);
        }

    }
}
