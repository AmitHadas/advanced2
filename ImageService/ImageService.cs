using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.Configuration;
using ImageService.Communication;
//using ImageService.Communication;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private System.ComponentModel.IContainer components;
        private int eventId = 1;
        private ILoggingService logging;
        private ImageServer server;

        public ImageService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = "MySource1";
            string logName = "MyLogFile1";
            if (args.Count() > 0) 
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = ConfigurationManager.AppSettings.Get("SourceName");
            eventLog1.Log = ConfigurationManager.AppSettings.Get("LogName");
            EventHandler<MessageRecievedEventArgs> MessageRecieved = new EventHandler<MessageRecievedEventArgs>(onMsg);
            this.logging = new LoggingService(MessageRecieved);
            IImageController controller = new ImageController(new ImageServiceModal(this.logging));
            this.server = new ImageServer(controller, this.logging);      
            controller.ImageServerProp = server;
            ClientHandler clientHandler = new ClientHandler(controller);
            TcpServer tcpServer = new TcpServer(logging, clientHandler, 8000);
            tcpServer.Start();
        }
        
        //The function that is called when we start the service.
        protected override void OnStart(string[] args)
        {
            logging.MessageRecieved += onMsg;
            eventLog1.WriteEntry("log 4");
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart");
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            // Start the timer
            timer.Enabled = true;
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("log 5");
        }

        //The function that is called when we stop the service.
        protected override void OnStop()
        {
            //notify the server that the service is about to close
           // this.server.sendCommand(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null));
            eventLog1.WriteEntry("In onStop.");
        }
        private void onMsg(object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Message);
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private void eventLog2_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}

public enum ServiceState
{
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};