using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Class AddLogCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class AddLogCommand : ICommand
    {
        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            return "";
        }
    }
}
