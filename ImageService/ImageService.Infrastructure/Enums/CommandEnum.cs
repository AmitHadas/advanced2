using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    //enum that represent command name - command id
    /// <summary>
    /// Enum CommandEnum
    /// </summary>
    public enum CommandEnum : int
    {
        /// <summary>
        /// The new file command
        /// </summary>
        NewFileCommand,
        /// <summary>
        /// The close command
        /// </summary>
        CloseCommand,
        /// <summary>
        /// The get configuration command
        /// </summary>
        GetConfigCommand,
        /// <summary>
        /// The log command
        /// </summary>
        LogCommand,
        /// <summary>
        /// The close handler
        /// </summary>
        CloseHandler,
        /// <summary>
        /// The get log list
        /// </summary>
        GetLogList,
        /// <summary>
        /// The new log entry
        /// </summary>
        NewLogEntry,
        /// <summary>
        /// The close GUI
        /// </summary>
        CloseGui
    }
}
