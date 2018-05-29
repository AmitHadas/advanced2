using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Image
    {
        public string ImageName { get; set; }
        public int Size { get; set; }
        public Image ImageFile { get; set; }
    }
}