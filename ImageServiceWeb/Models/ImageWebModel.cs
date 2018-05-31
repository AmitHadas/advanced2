using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {

    public ImageWebModel()
        {
            this.FirstName = "noa";
            this.LastName = "Hadas";
            this.ID = 1111;
        }
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

    }
}