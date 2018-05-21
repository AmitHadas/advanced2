using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        // The Modal Object
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;
        private ILoggingService m_logging;
        private ImageServer m_imageServer;
        //public ImageServer ImageServerProp
        //{
        //    get
        //    {
        //        return this.m_imageServer;
        //    }

        //    set
        //    {
        //        this.m_imageServer = value;
        //    }
        //}


        public void setServer(ImageServer server)
        {
            this.m_imageServer = server;
        }

        //constructor
        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            m_logging = logging;
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(this.m_modal)},
                {(int)CommandEnum.CloseHandler, new RemoveHandlerCommand(m_imageServer)},
                {(int)CommandEnum.GetConfigCommand, new AppConfigCommand()},
                {(int)CommandEnum.GetLogList, new GetLogListCommand(m_logging) }
            };
        }


        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command = this.commands[commandID];
            // make task for each command
            Task<Tuple<string, bool>> t = new Task<Tuple<string, bool>>(() => {
                bool result;
                return Tuple.Create(command.Execute(args, out result), result);
            });
            t.Start();
            Tuple<string, bool> taskOutput = t.Result;
            resultSuccesful = taskOutput.Item2;
            return taskOutput.Item1;
        }
    }
}
