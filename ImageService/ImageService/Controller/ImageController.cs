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
    /// <summary>
    /// Class ImageController.
    /// </summary>
    /// <seealso cref="ImageService.Controller.IImageController" />
    public class ImageController : IImageController
    {
        // The Modal Object
        /// <summary>
        /// The m modal
        /// </summary>
        private IImageServiceModal m_modal;
        /// <summary>
        /// The commands
        /// </summary>
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// The m logging
        /// </summary>
        private ILoggingService m_logging;
        /// <summary>
        /// The m image server
        /// </summary>
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
        /// <summary>
        /// Sets the dictionary.
        /// </summary>
        public void SetDictionary()
        {
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(this.m_modal)},
                {(int)CommandEnum.CloseHandler, new RemoveHandlerCommand(m_imageServer)},
                {(int)CommandEnum.GetConfigCommand, new AppConfigCommand()},
                {(int)CommandEnum.GetLogList, new GetLogListCommand(m_logging) }
            };
        }


        /// <summary>
        /// Sets the server.
        /// </summary>
        /// <param name="server">The server.</param>
        public void setServer(ImageServer server)
        {
            this.m_imageServer = server;
            SetDictionary();
        }

        //constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="logging">The logging.</param>
        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            m_logging = logging;
            //commands = new Dictionary<int, ICommand>()
            //{
            //    {(int)CommandEnum.NewFileCommand, new NewFileCommand(this.m_modal)},
            //    {(int)CommandEnum.CloseHandler, new RemoveHandlerCommand(m_imageServer)},
            //    {(int)CommandEnum.GetConfigCommand, new AppConfigCommand()},
            //    {(int)CommandEnum.GetLogList, new GetLogListCommand(m_logging) }
            //};
        }


        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="resultSuccesful">if set to <c>true</c> [result succesful].</param>
        /// <returns>System.String.</returns>
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