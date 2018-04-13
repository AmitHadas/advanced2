using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        //constructor
        public NewFileCommand(IImageServiceModal modal)
        {
            // Storing the Modal
            m_modal = modal;           
        }

        //The function execute the command according to args - notify the madal to add file.
        public string Execute(string[] args, out bool result)
        {
            result = true;
            // The String Will Return the New Path if result = true,
            //and will return the error message
            return this.m_modal.AddFile(args[0], out result);
        }
    }
}
