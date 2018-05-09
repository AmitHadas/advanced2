using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        ImageServer ImageServerProp { get; set; }
        //The function get command id and execute the specific command by dictionary.
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}
