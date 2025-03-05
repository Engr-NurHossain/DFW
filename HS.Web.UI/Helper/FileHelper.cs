using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using NLog;

namespace HS.Web.UI.Helper
{
    public static class FileHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        public static void SaveFile(byte[] content, string path)
        {
            string filePath = GetFileFullPath(path);
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.Write("File Path: ");
            //    file.WriteLine(filePath);
            //}

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
            }
            catch (Exception ex)
            {
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                //{
                //    file.Write("Exception when Exsists Check: ");
                //    file.WriteLine(ex.Message);
                //}
            }


            //if (System.IO.File.Exists(filePath))
            //{
            //    FileStream _FileStream = File.OpenWrite(filePath);
            //    var reader = new StreamReader(_FileStream);
            //    File.Delete(filePath);
            //    //using (var stream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
            //    //{

            //    //}
            //}
            //Save file
            try
            {
                
                using (FileStream str = File.Create(filePath))
                {
                    //str.Close();
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                    //{
                    //    file.WriteLine("Writing File");
                    //    file.WriteLine(filePath);
                    //}
                    try
                    {
                        str.Write(content, 0, content.Length);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                        //{
                        //    file.WriteLine("Exception occured");
                        //    file.WriteLine(ex.StackTrace);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                {
                    file.WriteLine("Exception File.Create ");
                    file.WriteLine(ex.StackTrace);
                    file.WriteLine(ex.Message);
                    file.WriteLine(ex.Source);
                }
            }
        }

        public static string GetFileFullPath(string path)
        {
            string relName = "";
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("Inside GetFileFullPath");
            //}
            if (path.IndexOf(":\\") > -1 || path.IndexOf(":/") > -1|| path.StartsWith("~"))
            {
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                //{
                //    file.WriteLine("Inside FirstCondition");
                //}
                relName = path;
            }
            else if (path.StartsWith("/"))
            {
                relName = string.Concat("~", path);
            } 
            else
            {
                relName = string.Concat("~/", path);
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("Before checking Path");
            //}
            string filePath = relName.StartsWith("~") ? HostingEnvironment.MapPath(relName) : relName;
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.Write("Returning Path: ");
            //    file.WriteLine(filePath);
            //}
            return filePath;
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    logger.Info("Folder created {folder_path}", path);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Folder {folder_path} creation failed",path);
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
        public static bool CreateFolder(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}