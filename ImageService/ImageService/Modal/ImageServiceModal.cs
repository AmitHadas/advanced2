using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;


namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {

        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageServiceModal()
        {
            this.m_OutputFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            this.m_thumbnailSize = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
        }
        public bool ThumbnailCallback()
        {
            return true;
        }
        public string AddFile(string path, out bool result)
        {

            // if outputDir doesn't exists:
            if (!Directory.Exists(m_OutputFolder))
            {
                System.IO.Directory.CreateDirectory(m_OutputFolder);
// לטפלב log
                System.IO.Directory.CreateDirectory(m_OutputFolder + "\\Thumbnails");
            }
      
            Image myImage = Image.FromFile(path);
            PropertyItem propItem = null;
            string info = "";
            try
            {
                propItem = myImage.GetPropertyItem(20624);
                myImage.Dispose();
                //Convert date taken metadata to a DateTime object
                DateTime dateTime = File.GetCreationTime(path);

                string year = dateTime.Year.ToString();
                string yearPath = this.m_OutputFolder + "\\" + year;
                string thumbMonthPath = this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + dateTime.Month;
                string thumbYearPath = this.m_OutputFolder + "\\Thumbnails\\" + year;
                string monthPath = this.m_OutputFolder + "\\" + year + "\\" + dateTime.Month;
                info = monthPath;
                result = true;
                string imageName = Path.GetFileName(path);

                if (!Directory.Exists(yearPath))
                {
                    Directory.CreateDirectory(yearPath);
                    Directory.CreateDirectory(monthPath);
                    Directory.CreateDirectory(thumbYearPath);
                    Directory.CreateDirectory(thumbMonthPath);

                }
                else if (!Directory.Exists(monthPath))
                {
                    Directory.CreateDirectory(monthPath);
                    Directory.CreateDirectory(thumbMonthPath);
                }
               //PictureBox1 pictureBox1 = pictureBox1.Image.Load("Image Path");
               // string imgPath = pictureBox1.ImageLocation;
               // string nameImage = imgPath.Substring(imgPath.LastIndexOf('\\') + 1);
                
                System.IO.File.Copy(path, monthPath + "\\" + imageName);
                Image thumb = Image.FromFile(path);
                thumb = (Image)(new Bitmap(thumb, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
                thumb.Save(thumbMonthPath + "\\" + imageName);
            }
            catch (Exception e)
            {
                info = e.Data.ToString();
                result = false;
            };

            return info;
        }
        
        public bool CreateFolder(string path)
        {
            System.IO.Directory.CreateDirectory(path);
            return true;
        }

        public bool MoveFile(string fileName, string srcPath, string dstPath)
        {
            System.IO.File.Move(srcPath, dstPath);
            return true;
        }
    }

}