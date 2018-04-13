using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        //The function execute the command according to args.
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}
