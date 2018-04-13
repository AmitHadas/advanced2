using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        /// The Function Addes A file to the system
        ///path is the Path of the Image from the file</param>
        /// the function returns indication if the Addition Was Successful</returns>
        string AddFile(string path, out bool result);
    }
}
