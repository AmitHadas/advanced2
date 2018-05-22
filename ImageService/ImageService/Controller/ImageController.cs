using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
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

        public void setServer(ImageServer server)
        {
            this.m_imageServer = server;
            this.SetDictionary();
        }

        //constructor
        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            m_logging = logging;
        }

        private void SetDictionary()
        {
            if (m_imageServer == null)
            {
                m_logging.Log("server is null 3", MessageTypeEnum.WARNING);
            }
            else
            {
                m_logging.Log("server isn't null 3", MessageTypeEnum.WARNING);
            }
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
