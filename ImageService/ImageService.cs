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
    /// <summary>
    /// Class ImageService.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class ImageService : ServiceBase
    {
        /// <summary>
        /// The components
        /// </summary>
        private System.ComponentModel.IContainer components;
        /// <summary>
        /// The event identifier
        /// </summary>
        private int eventId = 1;
        /// <summary>
        /// The logging
        /// </summary>
        private ILoggingService logging;
        /// <summary>
        /// The server
        /// </summary>
        private ImageServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
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
            IImageController controller = new ImageController(new ImageServiceModal(this.logging), this.logging);
            this.server = new ImageServer(controller, this.logging);
            controller.setServer(server);
            ClientHandler clientHandler = new ClientHandler(controller, logging);
            TcpServer tcpServer = new TcpServer(logging, clientHandler, 8000);
            LoggingService.NotifyLogEntry += tcpServer.NotifyAllClients;
            ImageServer.NotifyHandlerRemoved += tcpServer.NotifyAllClients;
            ImageServer.NotifyCloseGui += tcpServer.NotifyAllClients;
            ImageServer.NotifyCloseGui += tcpServer.CallRemoveClient
;
            tcpServer.Start();
        }

        //The function that is called when we start the service.
        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
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
        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //notify the server that the service is about to close
            // this.server.sendCommand(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null));
            eventLog1.WriteEntry("In onStop.");
        }
        /// <summary>
        /// Ons the MSG.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MessageRecievedEventArgs"/> instance containing the event data.</param>
        private void onMsg(object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Message);
        }
        /// <summary>
        /// Handles the <see cref="E:Timer" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        /// <summary>
        /// When implemented in a derived class, <see cref="M:System.ServiceProcess.ServiceBase.OnContinue" /> runs when a Continue command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service resumes normal functioning after being paused.
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        /// <summary>
        /// Sets the service status.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="serviceStatus">The service status.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// Handles the EntryWritten event of the eventLog2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EntryWrittenEventArgs"/> instance containing the event data.</param>
        private void eventLog2_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}

/// <summary>
/// Enum ServiceState
/// </summary>
public enum ServiceState
{
    /// <summary>
    /// The service stopped
    /// </summary>
    SERVICE_STOPPED = 0x00000001,
    /// <summary>
    /// The service start pending
    /// </summary>
    SERVICE_START_PENDING = 0x00000002,
    /// <summary>
    /// The service stop pending
    /// </summary>
    SERVICE_STOP_PENDING = 0x00000003,
    /// <summary>
    /// The service running
    /// </summary>
    SERVICE_RUNNING = 0x00000004,
    /// <summary>
    /// The service continue pending
    /// </summary>
    SERVICE_CONTINUE_PENDING = 0x00000005,
    /// <summary>
    /// The service pause pending
    /// </summary>
    SERVICE_PAUSE_PENDING = 0x00000006,
    /// <summary>
    /// The service paused
    /// </summary>
    SERVICE_PAUSED = 0x00000007,
}

/// <summary>
/// Struct ServiceStatus
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    /// <summary>
    /// The dw service type
    /// </summary>
    public int dwServiceType;
    /// <summary>
    /// The dw current state
    /// </summary>
    public ServiceState dwCurrentState;
    /// <summary>
    /// The dw controls accepted
    /// </summary>
    public int dwControlsAccepted;
    /// <summary>
    /// The dw win32 exit code
    /// </summary>
    public int dwWin32ExitCode;
    /// <summary>
    /// The dw service specific exit code
    /// </summary>
    public int dwServiceSpecificExitCode;
    /// <summary>
    /// The dw check point
    /// </summary>
    public int dwCheckPoint;
    /// <summary>
    /// The dw wait hint
    /// </summary>
    public int dwWaitHint;
};