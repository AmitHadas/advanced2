using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    // enum that represents the type of message
    /// <summary>
    /// Enum MessageTypeEnum
    /// </summary>
    public enum MessageTypeEnum : int
    {
        /// <summary>
        /// The information
        /// </summary>
        INFO,
        /// <summary>
        /// The warning
        /// </summary>
        WARNING,
        /// <summary>
        /// The fail
        /// </summary>
        FAIL
    }
}
