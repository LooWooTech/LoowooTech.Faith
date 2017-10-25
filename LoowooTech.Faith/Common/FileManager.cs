using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Common
{
    public static class FileManager
    {
        private static string _folder { get; set; }
        private static string _dir { get; set; }
        static FileManager()
        {
            _folder = "UploadFiles";

            _dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _folder);

            _Conductfolder = "Materials";
        }
        public static string Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0) return string.Empty;
            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
            var newfileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
            var saveFileName = Path.Combine(_dir, newfileName);
            file.SaveAs(saveFileName);
            return saveFileName;
        }

        private static string _Conductfolder { get; set; }

        public static string Upload2(HttpPostedFileBase file,string name)
        {
            return UploadBase(file, Path.Combine(_Conductfolder,name), AppDomain.CurrentDomain.BaseDirectory);
        }


        private static string UploadBase(HttpPostedFileBase file,string folder,string dir)
        {
            if (file.ContentLength == 0) return string.Empty;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var dir2 = Path.Combine(dir, folder);
            if (!Directory.Exists(dir2))
            {
                Directory.CreateDirectory(dir2);
            }
            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + DateTime.Now.Ticks.ToString();
            if (fileName.Length > 255)
            {
                fileName = fileName.Substring(255);
            }
            fileName = fileName + Path.GetExtension(file.FileName);
            var saveFileFullPath = Path.Combine(dir, folder, fileName);
            //if (File.Exists(saveFileFullPath))
            //{
                
            //    //var newfile = Path.GetFileNameWithoutExtension(saveFileFullPath) + "-" + DateTime.Now.Ticks.ToString();
            //    //if (newfile.Length > 256)
            //    //{
            //    //    newfile = newfile.Substring(255);
            //    //}
            //    //newfile = newfile + Path.GetExtension(saveFileFullPath);
            //    //newfile = Path.Combine(dir, folder, newfile);
            //    //File.Copy(saveFileFullPath, newfile);
            //    //File.Delete(saveFileFullPath);
            //}
            file.SaveAs(saveFileFullPath);
            return Path.Combine(folder, fileName);

        }
    }
}