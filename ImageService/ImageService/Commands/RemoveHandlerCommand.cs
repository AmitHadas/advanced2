using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class RemoveHandlerCommand : ICommand
    {
        private ImageServer m_imageServer;
        public RemoveHandlerCommand(ImageServer server)
        {
            m_imageServer = server;
        }
        public string Execute(string[] args, out bool result)
        {
            result = true;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Add an Application Setting.
            config.AppSettings.Settings.Remove("Handler");
            config.AppSettings.Settings.Add("Handler", args[0]);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");

            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(args[1], "Remove handler");
            this.m_imageServer.RemoveDirectoryHandler(args[1], e);

            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, "");
            ImageServer.RemoveHandlerEvent(command);
            return "";
        }
    }
}
