using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageServiceWeb.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogModel
    {
    
        public ObservableCollection<LogEntry> Logs { get; }
       

        public ObservableCollection<Tuple<string, string>> LogsToShow { get; set; }

        public string FilterType { get; set; }
        public LogModel()
        {
            Logs = new ObservableCollection<LogEntry>();
            LogsToShow = new ObservableCollection<Tuple<string, string>>();
            LogsToShow.Add(new Tuple<string, string>("WARNING", "hi im warning log"));
            LogsToShow.Add(new Tuple<string, string>("WARNING", "hi im warning log2"));
            LogsToShow.Add(new Tuple<string, string>("ERROR", "hi im ERROR log"));
            LogsToShow.Add(new Tuple<string, string>("ERROR", "hi im ERROR log2"));


            FilterType = "";
            ClientSingleton client = ClientSingleton.ClientInsatnce;
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.GetLogList, args, "");
            client.SendCommand(e);
          //  Thread.Sleep(1000);

        }



    }
}