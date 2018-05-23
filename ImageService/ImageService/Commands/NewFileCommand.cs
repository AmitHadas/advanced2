using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Class NewFileCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    public class NewFileCommand : ICommand
    {
        /// <summary>
        /// The m modal
        /// </summary>
        private IImageServiceModal m_modal;

        //constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="NewFileCommand"/> class.
        /// </summary>
        /// <param name="modal">The modal.</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            // Storing the Modal
            m_modal = modal;           
        }

        //The function execute the command according to args - notify the madal to add file.
        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            // The String Will Return the New Path if result = true,
            //and will return the error message
            return this.m_modal.AddFile(args[0], out result);
        }
    }
}
