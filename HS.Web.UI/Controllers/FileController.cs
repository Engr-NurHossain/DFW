using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using OS.AWS.S3.Services;
using System.Threading.Tasks;
using NLog;
using System.Threading;
using iTextSharp.text.pdf;
using Image = System.Drawing.Image;
using Customer = HS.Entities.Customer;
using System.Data;
//using System.Data.OleDb;
//using DocumentFormat.OpenXml.Drawing.Diagrams;
//using NLog.Web;
using ClosedXML.Excel;



namespace HS.Web.UI.Controllers
{
    public class FileController : BaseController
    {
        string S3Domain = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
        public FileController()
        {
            logger = LogManager.GetCurrentClassLogger();
            S3Domain = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
        }

        // GET: File
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult NoFile()
        {
            return View();
        }
        public ActionResult Download()
        {
            WebClient webClient;
            byte[] fileBytes1;
            bool FileExists = true;
            string full_Name = string.Empty;
            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                CustomerFile filename = _Util.Facade.CustomerFileFacade.GetFileNameById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                if (filename.FileFullName == "")
                {
                    filename.FileFullName = filename.FileDescription;
                }
                try
                {
                    if(filename.Filename.IndexOf("http") > -1)
                    {
                        webClient = new WebClient();
                        full_Name=filename.Filename;
                        fileBytes1 = webClient.DownloadData(filename.Filename);
                        return File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName); 
                    }

                    // string fullName = Server.MapPath("~" + filename.Filename);

                    //string File_Name = filename.Filename.TrimStart('/');
                    string File_Name = filename.Filename.TrimStart('/');
                    full_Name = S3Domain + File_Name;
                    FileExists = new AWSS3ObjectService().CheckFileExists(File_Name);
                    @ViewBag.FilePath = full_Name;

                    if (FileExists)
                    {
                        webClient = new WebClient();
                        fileBytes1 = webClient.DownloadData(full_Name);
                        return File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                    }
                    else 
                    {
                        full_Name = Server.MapPath(filename.Filename);
                        @ViewBag.FilePath = full_Name;
                        if (System.IO.File.Exists(full_Name))
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(full_Name);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    @ViewBag.FilePath = full_Name;
                    logger.Error(ex);
                }
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        [Authorize]
        public ActionResult DownloadKnowledgeBaseFile(string url)
        {
            string fullpath = "";
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.IndexOf("http") > -1)
                {
                    var filename = Path.GetFileName(url);
                    var webClient = new WebClient();
                    byte[] fileBytes = webClient.DownloadData(url);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    fullpath = Server.MapPath("~" + url);
                    if (!System.IO.File.Exists(fullpath))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }
                    var filename = Path.GetFileName(fullpath);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullpath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
            }
            else
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        [Authorize]
        public ActionResult KnowledgeBaseFileUpload(HttpPostedFileBase[] ImageFile)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string tempFileName = "";
            List<UploadStatus> status = new List<UploadStatus>();

            string filedata = "";
            filedata = ImageFile.Count().ToString();
            foreach (HttpPostedFileBase httpPostedFileBase in ImageFile)
            {
                #region Files Loop
                UploadStatus UploadStatus = new UploadStatus();
                string tempFolderName = ConfigurationManager.AppSettings["File.KnowledgeFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                filedata += " : " + FileName + " : " + DateTime.Now.ToString();
                if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
                {
                    UploadStatus.size = ImageHelper.GetFileSize(httpPostedFileBase.ContentLength);
                    Regex regfile = new Regex("[*'\",&#^@+]");
                    string str = regfile.Replace(httpPostedFileBase.FileName, "--");
                    FileName += "_" + str.Replace(" ", "-");
                    string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                    if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                    {
                        try
                        {
                            if (FileName.Contains("pdf") || FileName.Contains("doc") || FileName.Contains("docx") || FileName.Contains("xls")
                                || FileName.Contains("xlsx") || FileName.Contains("rtf") || FileName.Contains("txt") || FileName.Contains("msg")
                                || FileName.Contains("pptx") || FileName.Contains("ppt") || FileName.Contains("gif"))
                            {
                                httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                                isUploaded = true;
                                tempFileName = FileName;
                            }
                            else
                            {
                                Regex reg = new Regex("[*'\",&#^@+]");
                                string str1 = reg.Replace(httpPostedFileBase.FileName, "--");
                                tempFileName = rand.Next().ToString();
                                tempFileName += "-___" + str1.Replace(" ", "-").Substring(0, str1.LastIndexOf('.')) + ".jpg";
                                httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, tempFileName));
                                filedata += " : Finish time - " + DateTime.Now.ToString();

                                isUploaded = true;
                            }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Rug Ticket File Progress.txt"), true))
                            {
                                file.WriteLine(filedata);
                                file.Close();
                            }
                        }
                        catch (Exception) {  /*TODO: You must process this exception.*/}
                    }
                }
                UploadStatus.filepath = string.Concat("/", tempFolderName, "/", tempFileName);
                UploadStatus.filename = httpPostedFileBase.FileName;
                UploadStatus.filefullpath = ConfigurationManager.AppSettings["SiteDomain"] + UploadStatus.filepath;
                status.Add(UploadStatus);

                #endregion
            }

            return Json(new { isUploaded = isUploaded, status = status }, "text/html");
        }
        public ActionResult Download_v2()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                CustomerFile filename = _Util.Facade.CustomerFileFacade.GetFileNameById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                if (filename.FileFullName == "")
                {
                    filename.FileFullName = filename.FileDescription;
                }
                try
                {
                    if (filename.Filename.IndexOf("http") > -1)
                    {
                        WebClient webClient = new WebClient();
                        byte[] fileBytes1 = webClient.DownloadData(filename.Filename);
                        return File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileDescription); ;
                    }

                    string fullName = Server.MapPath("~" + filename.Filename);

                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }
        public ActionResult BillFileDownload()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                BillFile filename = _Util.Facade.CustomerBillFacade.GetBillFileById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                var sfilename = filename.Filename.Split(new[] { "___" }, StringSplitOptions.None);
                if (filename.FileFullName == "")
                {
                    filename.FileFullName = filename.FileDescription;
                }
                try
                {
                    string fullName = Server.MapPath("~" + filename.Filename);
                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, sfilename[1]);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }

        [Authorize]
        public ActionResult DownloadTicketFile(string url)
        {
            ///Follow BillFileDownload
            //Authorization Check.... 
            ///Get File name by ID 
            ///Read file 
            ///Stream file... 

            if (!string.IsNullOrWhiteSpace(url))
            {
                string fullpath = Server.MapPath("~" + url);
                if (!System.IO.File.Exists(fullpath))
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }
                var filename = Path.GetFileName(fullpath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullpath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            else
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }

        [Authorize]
        public ActionResult DownloadCreditCheckPdf(int id)
        {
            string url = "";
            CustomerCreditCheck credit = new CustomerCreditCheck();
            credit = _Util.Facade.CustomerFacade.GetCustomerCreditCheckById(id);
            if (credit != null)
                url = credit.ReportPdfLink;
            //url = "/" + credit.ReportPdfLink;

            if (!string.IsNullOrWhiteSpace(url))
            {
                //string fullpath = Server.MapPath("~" + url);

                //string File_Name = filename.Filename.TrimStart('/');
                string DownloadFileUrl = S3Domain + url;
                bool FileExists = new AWSS3ObjectService().CheckFileExists(url);

                if (FileExists)
                {
                    WebClient webClient = new WebClient();
                    byte[] fileBytes1 = webClient.DownloadData(DownloadFileUrl);
                    return File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, credit.RepontPdfName);
                }
                else
                {
                    DownloadFileUrl = Server.MapPath(url);
                    if (System.IO.File.Exists(DownloadFileUrl))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(DownloadFileUrl);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, credit.RepontPdfName);
                    }
                }

                return PartialView("~/Views/Shared/_FileNotFound.cshtml");

                //var filename = Path.GetFileName(fullpath);
                //byte[] fileBytes = System.IO.File.ReadAllBytes(fullpath);
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            else
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        public ActionResult DownloadEqFile()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText(HttpUtility.UrlDecode(Request.QueryString[0].ToString()));

                EquipmentFile filename = _Util.Facade.EquipmentFileFacade.GetById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                try
                {
                    string fullName = Server.MapPath("~" + filename.Filename);
                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }

        public ActionResult SupplierDocumentDownload()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                SupplierFile filename = _Util.Facade.SupplierFacade.GetFileNameBySupplierId(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                try
                {
                    string fullName = Server.MapPath("~" + filename.Filename);
                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }

        public ActionResult UserFileDownload()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                HrDoc filename = _Util.Facade.HrDocFacade.GetFileNameById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                string fullName = "";
                try
                {

                    fullName = Server.MapPath("~" + filename.Filename);

                    if (!System.IO.File.Exists(fullName))
                    {

                        _Util.Facade.UserActivityFacade.AddElmah("File no exist");
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                        //return Json(new { result = "File not exists" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    _Util.Facade.UserActivityFacade.AddElmah(ex.Message);
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    //return Json(new { result = "File not exists" },JsonRequestBehavior.AllowGet);
                }

                string name = filename.Filename;

                if (filename.FileDescription.Length < 60)
                {
                    var fileformat = filename.Filename.Split('.');
                    if (fileformat.Length == 2)
                    {
                        name = string.Concat(filename.FileDescription, ".", fileformat[1]);
                    }
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
            }
        }

        public ActionResult DownloadWriteUp()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                EmployeeWriteUp filename = _Util.Facade.HrFacade.GetEmployeeWriteUpById(Int32.Parse(Idstr));

                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                try
                {
                    string fullName = Server.MapPath("~" + filename.FilePath);
                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }
                    if (filename.FileName == null || filename.FileName == "")
                    {
                        filename.FileName = filename.FilePath.Substring(filename.FilePath.Length - 6); ;
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileName);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }



        [Authorize]
        public PartialViewResult CustomerFilesAndDocument(int id, string soldby)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFilesList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //} 
            ViewBag.CustomerId = id;
            ViewBag.SoldBy = soldby;
            return PartialView("_CustomerFilesAndDocument");
        }


        [Authorize]
        public PartialViewResult CustomerFiles(int id, string soldby)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFilesList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            #region count
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerById(id);

            CustomerDetailsTabCount customerDetailsTabCount = _Util.Facade.InvoiceFacade.GetCustomerDetailsTabCountsByCustomerId(customer.CustomerId, CurrentUser.CompanyId.Value);
            ViewBag.ActiveFilesCount = customerDetailsTabCount.ActiveFilesCount;
            ViewBag.InActiveFilesCount = customerDetailsTabCount.InActiveFilesCount;

            #endregion
            ViewBag.CustomerId = id;
            ViewBag.SoldBy = soldby;
            return PartialView("_CustomerFiles");
        }

        [Authorize]
        public PartialViewResult LoadActiveCustomerFiles(int id, string soldby, string SearchText)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFilesList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            Customer Customer = _Util.Facade.CustomerFacade.GetById(id);
            if (Customer == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<CustomerFile> files = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(Customer.CustomerId, CurrentUser.CompanyId.Value, SearchText);

            if (files.Count != 0)
            {
                ViewBag.CusId = files.FirstOrDefault().CustomerId;
            }

            ViewBag.CustomerId = id;
            ViewBag.SoldBy = soldby;
            return PartialView("_LoadActiveCustomerFiles", files);
        }
        [Authorize]
        public PartialViewResult LoadInActiveCustomerFiles(int id, string soldby, string SearchText)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFilesList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            Customer Customer = _Util.Facade.CustomerFacade.GetById(id);
            if (Customer == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<CustomerFile> files = _Util.Facade.CustomerFileFacade.GetAllInactiveFilesByCustomerIdAndCompanyId(Customer.CustomerId, CurrentUser.CompanyId.Value, SearchText);
            if (files.Count != 0)
            {
                ViewBag.CusId = files.FirstOrDefault().CustomerId;
                ViewBag.InvoiceId = files.FirstOrDefault().FileDescription;
            }
            ViewBag.CustomerId = id;
            ViewBag.SoldBy = soldby;
            return PartialView("_LoadInactiveCustomerFiles", files);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteCustomerFile_v2(int Id, int CustomerId, string FileDescription, Guid? CusId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!res)
            //{
            //    return Json(new { result=false, message = "Invalid Customer" });
            //}
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            CustomerFile tmpCF = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(Id);
            if (tmpCF.CustomerId != tmpCustomer.CustomerId)
            {
                return Json(new { result = false, message = "Invalid Customer" });
            }

            var serverFile = Server.MapPath(tmpCF.Filename);
            if (System.IO.File.Exists(serverFile))
            {
                System.IO.File.Delete(serverFile);
            }

            _Util.Facade.CustomerFileFacade.DeleteById(Id);
            base.AddUserActivityForCustomer("File is deleted #Ref:" + Id, LabelHelper.ActivityAction.Delete, CusId, null, Id.ToString());

            return Json(new { result = true, message = "File deleted successfully." });
        }


        public bool WatermarkAWSS3CustomerFile(int FileId, int CustomerId)
        {

            WebClient webClient;
            byte[] fileBytes1;
            byte[] fileBytes_WM;
            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            List<string> DataKeyModel = _Util.Facade.LookupFacade.GetDataKeyList();


            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string _message = "";
            bool _result = false;

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();


            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            CustomerFile tmpCF = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(FileId);

            //if (tmpCF.CustomerId != tmpCustomer.CustomerId)
            //{
            //    return Json(new { result = false, message = "Invalid Customer" });
            //}

            //var Filepath = tmpCF.Filename.Replace(S3Domain, "");
            //Filepath = Filepath.TrimStart('/');
            var Filepath = tmpCF.Filename.TrimStart('/');

            bool status = new AWSS3ObjectService().CheckFileExists(Filepath);

            if (status == true)
            {
         
                var temp_FileName = tmpCF.Filename.Split('/').Last();
                Filepath = temp_FileName;

                string Full_Path = S3Domain + tmpCF.Filename.TrimStart('/');
                

                /// Mayur :: File Download to temp folder :start

                webClient = new WebClient();
                fileBytes1 = webClient.DownloadData(Full_Path);

                File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, temp_FileName).ToString();

                _fileSize = (decimal)fileBytes1.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                var Temp_FileName = Server.MapPath("~/WMS3Cache/tmp_" + temp_FileName);
                

                //if (!System.IO.File.Exists(Temp_FileName))
                //{
                //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                //}
                //else
                //{
                //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                //}

                using (FileStream fileStream = new FileStream(Temp_FileName, FileMode.Create))
                {
                    fileStream.Write(fileBytes1, 0, fileBytes1.Length);
                   
                }

                // Read the input PDF file
              //  byte[] pdfBytes = System.IO.File.ReadAllBytes(Temp_FileName);

            //    PdfReader reader = new PdfReader(Temp_FileName);

                // Create a stamper to modify the PDF

               var Temp_FileName2 = Server.MapPath("~/WMS3Cache/Temp/" + temp_FileName);

               //var Temp_FileName2 = Server.MapPath("~/EmailFileCache/Test.pdf");



                using (PdfReader reader = new PdfReader(Temp_FileName))
                {
                    using (PdfStamper stamper = new PdfStamper(reader, new FileStream(Temp_FileName2, FileMode.Create)))
                    {

                        // Get the number of pages in the PDF   
                        int pageCount = reader.NumberOfPages;

                        // Load the watermark image
                        string watermarkFilePath = Server.MapPath("~/Files/Watermark.png");
                        iTextSharp.text.Image watermarkImage = iTextSharp.text.Image.GetInstance(watermarkFilePath);


                        // Set the opacity of the watermark
                        watermarkImage.Alignment = iTextSharp.text.Image.ALIGN_TOP;
                        watermarkImage.ScaleToFit(500, 500);
                        watermarkImage.SetAbsolutePosition(50, 300);
                        watermarkImage.GetTop(50);

                        // Iterate through each page of the PDF
                        for (int i = 1; i <= pageCount; i++)
                        {
                            // Get the content over the existing page
                            PdfContentByte content = stamper.GetOverContent(i);

                            // Add the watermark image to the page
                            content.AddImage(watermarkImage);
                        }
                        stamper.Close();
                        reader.Dispose();
                    }
                }



                #region File Save on AWS S3

                webClient = new WebClient();
             //   fileBytes_WM = webClient.DownloadData(Temp_FileName2);
                fileBytes_WM = System.IO.File.ReadAllBytes(Temp_FileName2);

                comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
                tempFolderName = tempFolderName.TrimEnd('/');

                // "mayur" //// Converting file to memory stream ////////// Start

                // "mayur" //// Converting file to memory stream ////////// End

                FileName = tmpCF.Filename.Split('/').Last(); 
                FileKey = tmpCF.Filename;
                FileKey= FileKey.TrimStart('/');
                FilePath = FileKey.Replace("/"+FileName, "");
                FilePath = FilePath.TrimStart('/');

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, fileBytes_WM);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();


                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, fileBytes_WM);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();

                //Thread.Sleep(5000);

                //// delete all temp files
              
                System.IO.File.Delete(Temp_FileName2);
                System.IO.File.Delete(Temp_FileName);


                returnurl = S3Domain;
                returnurl = returnurl + FileKey;



                isUploaded = true;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End
                ///


                #region file update to customer file :: Completed status

                CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(FileId);

                if (cusFile != null)
                {
                    cusFile.Id = FileId;
                    cusFile.WMStatus = LabelHelper.WatermarkStatus.Completed;
                    cusFile.AWSUploadTS = DateTime.Now;
                    cusFile.AWSProcessStatus = LabelHelper.AWSProcessStatus.Uploaded;
                }
                _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);

                    #endregion

                    _message = "File Saved to Temp Folder.";
                    _result = true;


            }
            else
            {
                _message = "";
                _result = false;
            }

            return _result;
        }

        public bool WatermarkLocalCustomerFile(int FileId, int CustomerId)
        {

            WebClient webClient;
            byte[] fileBytes1;
            byte[] fileBytes_WM;
            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            List<string> DataKeyModel = _Util.Facade.LookupFacade.GetDataKeyList();


            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string _message = "";
            bool _result = false;

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();


            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            CustomerFile tmpCF = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(FileId);

            //if (tmpCF.CustomerId != tmpCustomer.CustomerId)
            //{
            //    return Json(new { result = false, message = "Invalid Customer" });
            //}

           
            var Filepath = tmpCF.Filename;

            var serverFile = Server.MapPath("~/" + Filepath);
       
            bool status = System.IO.File.Exists(serverFile);
               


            if (status == true)
            {

                var temp_FileName = tmpCF.Filename.Split('/').Last();
                Filepath = temp_FileName;

                string Full_Path = S3Domain + tmpCF.Filename.TrimStart('/');

                Full_Path = AppConfig.DomainSitePath + tmpCF.Filename;

                /// Mayur :: File Download to temp folder :start

                webClient = new WebClient();
                fileBytes1 = webClient.DownloadData(serverFile);
             


                File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, temp_FileName).ToString();

                _fileSize = (decimal)fileBytes1.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                var Temp_FileName = Server.MapPath("~/WMS3Cache/tmp_" + temp_FileName);


                //if (!System.IO.File.Exists(Temp_FileName))
                //{
                //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                //}
                //else
                //{
                //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                //}

                using (FileStream fileStream = new FileStream(Temp_FileName, FileMode.Create))
                {
                    fileStream.Write(fileBytes1, 0, fileBytes1.Length);

                }

                // Read the input PDF file
                //  byte[] pdfBytes = System.IO.File.ReadAllBytes(Temp_FileName);

                //    PdfReader reader = new PdfReader(Temp_FileName);

                // Create a stamper to modify the PDF

                var Temp_FileName2 = Server.MapPath("~/WMS3Cache/Temp/" + temp_FileName);

                //var Temp_FileName2 = Server.MapPath("~/EmailFileCache/Test.pdf");



                using (PdfReader reader = new PdfReader(Temp_FileName))
                {
                    using (PdfStamper stamper = new PdfStamper(reader, new FileStream(Temp_FileName2, FileMode.Create)))
                    {

                        // Get the number of pages in the PDF   
                        int pageCount = reader.NumberOfPages;

                        // Load the watermark image
                        string watermarkFilePath = Server.MapPath("~/Files/Watermark.png");
                        iTextSharp.text.Image watermarkImage = iTextSharp.text.Image.GetInstance(watermarkFilePath);


                        // Set the opacity of the watermark
                        watermarkImage.Alignment = iTextSharp.text.Image.ALIGN_TOP;
                        watermarkImage.ScaleToFit(500, 500);
                        watermarkImage.SetAbsolutePosition(50, 300);
                        watermarkImage.GetTop(50);

                        // Iterate through each page of the PDF
                        for (int i = 1; i <= pageCount; i++)
                        {
                            // Get the content over the existing page
                            PdfContentByte content = stamper.GetOverContent(i);

                            // Add the watermark image to the page
                            content.AddImage(watermarkImage);
                        }
                        stamper.Close();
                        reader.Dispose();
                    }
                }



                #region File Save on AWS S3

                webClient = new WebClient();
                //   fileBytes_WM = webClient.DownloadData(Temp_FileName2);
                fileBytes_WM = System.IO.File.ReadAllBytes(Temp_FileName2);

                comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
                tempFolderName = tempFolderName.TrimEnd('/');

                // "mayur" //// Converting file to memory stream ////////// Start

                // "mayur" //// Converting file to memory stream ////////// End

                FileName = tmpCF.Filename.Split('/').Last();
                FileKey = tmpCF.Filename;
                FileKey = FileKey.TrimStart('/');
                FilePath = FileKey.Replace("/" + FileName, "");
                FilePath = FilePath.TrimStart('/');

        
                serverFile = Server.MapPath("~/" + FileKey);
                string Serverfilename = FileHelper.GetFileFullPath(serverFile);
                FileHelper.SaveFile(fileBytes_WM, Serverfilename);


                //// delete all temp files
                if (System.IO.File.Exists(Serverfilename))
                {
                    System.IO.File.Delete(Temp_FileName2);
                    System.IO.File.Delete(Temp_FileName);

                }

                isUploaded = true;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End
                ///


                #region file update to customer file :: Completed status

                CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(FileId);

                if (cusFile != null)
                {
                    cusFile.Id = FileId;
                    cusFile.WMStatus = LabelHelper.WatermarkStatus.Completed;
                    cusFile.AWSUploadTS = DateTime.Now.UTCCurrentTime();
                    cusFile.AWSProcessStatus = LabelHelper.AWSProcessStatus.Local;
                }
                _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);

                #endregion

                _message = "File Saved to Temp Folder.";
                _result = true;


            }
            else
            {
                _message = "";
                _result = false;
            }

            return _result;
        }


        public JsonResult MassWatermark_customerfiles(string filename)
        {


            //// WMStatus ::

            //Pending    :: default
            //N.A        :: for non pdf files
            //Processing :: while file in process mode
            //Completed  :: After all proccess end final status after success
            //failure    :: if any error in whole process while file watermark process

            ///////////////////////////////////////////////////////////////////////
            ///


            bool _result = false;
            string _message = "PDF Files were processed.";


            //// get customer all customer list by query

         //   Customer tmpCustomer = _Util.Facade.CustomerFacade.GetAllCustomer(CustomerID);

           // ArrayList CustomerIDList = new ArrayList();
            List<int> CustomerIDList = new List<int>();

            CustomerIDList = GetCustomerIdfromXcelsheet(filename);

            // ArrayList CustomerIDList = new ArrayList();
            //List<int> CustomerIDList = GetCustomerIdfromXcelsheet(filename);

            Guid CompanyID = new Guid("C7E72006-6EDF-4B5A-8589-BFD1B2DAE7BA");
           

            List <CustomerFile> CustFile_All = new List<CustomerFile>();

            CustFile_All = _Util.Facade.CustomerFacade.GetAllCustomerAWSFileByCustomerId(CustomerIDList, CompanyID);

            /// for each customer perfor operation

            /// get all file for customer
            /// 
            ///  sort pdf and non pdf file make two 
            ///  

            if (CustFile_All.Count > 0)
            {


                List<CustomerFile> CustFile_pdf = new List<CustomerFile>();

                //    CustFile_pdf.Add(new CustomerFile());

                List<CustomerFile> CustFile_Non_pdf = new List<CustomerFile>();

                //    CustFile_Non_pdf.Add(new CustomerFile());


                //var pdfFiles = fileList.OfType<string>()
                //                   .Where(file => file.EndsWith(".pdf"))
                //                   .ToList();

                CustFile_pdf = CustFile_All                                                             // pdf file list
                .Where(pdf_file => (pdf_file.Filename.ToLower().EndsWith(".pdf")) && (pdf_file.WMStatus == "Pending" || pdf_file.WMStatus == "Processing"))
                .Select(pdf_file => new CustomerFile
                {
                    Id = pdf_file.Id,
                    Filename = pdf_file.Filename,
                    WMStatus = pdf_file.WMStatus,
                    CustomerIntId = pdf_file.CustomerIntId,
                    AWSProcessStatus = pdf_file.AWSProcessStatus,
                    AWSUploadTS = pdf_file.AWSUploadTS

                }).ToList();

                //// imp changed .pdf =>>> .pdf in whole code for identify pdf non pdf files 
                ///
                /// nedds to update all pdf file extention to .pdf to.pdf
                CustFile_Non_pdf = CustFile_All                                                     // Nonpdf file list
                .Where(Nonpdf_file => !(Nonpdf_file.Filename.ToLower().EndsWith(".pdf")) && !(Nonpdf_file.WMStatus == "N.A"))
                .Select(Nonpdf_file => new CustomerFile
                {
                    Id = Nonpdf_file.Id,
                    Filename = Nonpdf_file.Filename,
                    WMStatus = Nonpdf_file.WMStatus,
                    CustomerIntId = Nonpdf_file.CustomerIntId,
                    AWSProcessStatus = Nonpdf_file.AWSProcessStatus,
                    AWSUploadTS = Nonpdf_file.AWSUploadTS

                }).ToList();

                // update status to N.A in customerfile for nonpdf list

                foreach (CustomerFile custfile in CustFile_Non_pdf)
                {
                    CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(custfile.Id); // updaate store proedure UPDATECUSTOMERFILE storeprocedure and Insert sp also

                    if (cusFile != null)
                    {
                        cusFile.WMStatus = LabelHelper.WatermarkStatus.NotApplicable;
                        cusFile.AWSUploadTS = DateTime.Now;
                        cusFile.AWSProcessStatus = cusFile.AWSProcessStatus ?? LabelHelper.AWSProcessStatus.MassWatermark;

                        _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);
                    }



                }


                _result = true;
                foreach (CustomerFile custfile1 in CustFile_pdf)
                {
                    #region customer file update : status :: Processing

                    CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(custfile1.Id); // updaate store proedure UPDATECUSTOMERFILE storeprocedure and Insert sp also

                    if (cusFile != null)
                    {


                        cusFile.WMStatus = LabelHelper.WatermarkStatus.Processing;
                        cusFile.AWSUploadTS = DateTime.Now;
                        cusFile.AWSProcessStatus = cusFile.AWSProcessStatus ?? LabelHelper.AWSProcessStatus.MassWatermark;

                        _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);
                    }

                    #endregion

                    try
                    {
                        Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(custfile1.CustomerIntId); // new int

                        if (custfile1.AWSProcessStatus == LabelHelper.AWSProcessStatus.Local) // check where file exist on AWS or local
                        {
                            var status = WatermarkLocalCustomerFile(custfile1.Id, tmpCustomer.Id);
                            custfile1.UpdatedDate = DateTime.Now;
                            if (status)
                            {
                                custfile1.WMStatus = LabelHelper.WatermarkStatus.Completed;
                                _Util.Facade.CustomerFileFacade.UpdateCustomerFile(custfile1);
                            }
                            else
                            {
                                custfile1.WMStatus = LabelHelper.WatermarkStatus.Failure;
                                _Util.Facade.CustomerFileFacade.UpdateCustomerFile(custfile1);
                                _result = false;
                                //CustomerFile cusFile1 = _Util.Facade.CustomerFileFacade.GetCustomerFileById(custfile1.Id); // updaate store proedure UPDATECUSTOMERFILE storeprocedure and Insert sp also

                                //if (cusFile1 != null)
                                //{

                                //}
                            }
                        }
                        else
                        {
                            var status = WatermarkAWSS3CustomerFile(custfile1.Id, tmpCustomer.Id);

                            if (status == false)
                            {
                                CustomerFile cusFile1 = _Util.Facade.CustomerFileFacade.GetCustomerFileById(custfile1.Id); // updaate store proedure UPDATECUSTOMERFILE storeprocedure and Insert sp also

                                if (cusFile1 != null)
                                {

                                    cusFile1.WMStatus = LabelHelper.WatermarkStatus.Failure;
                                    cusFile1.AWSUploadTS = DateTime.Now.UTCCurrentTime();
                                    cusFile1.AWSProcessStatus = LabelHelper.AWSProcessStatus.MassWatermark;

                                    _Util.Facade.CustomerFileFacade.UpdateCustomerFile(custfile1);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }

                    //  CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(custfile1.Id); // updaate store proedure UPDATECUSTOMERFILE storeprocedure and Insert sp also

                    //if (cusFile != null)
                    //{


                    //    cusFile.WMStatus = "Completed";
                    //    cusFile.AWSUploadTS = DateTime.Now.UTCCurrentTime();
                    //    cusFile.AWSProcessStatus = "";

                    //    _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);
                    //}
                }
            

        //// update WMStatus to N.A. for non pdf files ::: for each


        /// do watermark on pdf file one by one using for each


        /// update WMStatus to Processing before start of the loop

        /// call WatermarkCustomerFile function



        /// on watermark success update WMStatus =>> Completed  before ending the loop or any fail condition WMStatus => Failure


            _message = "PDF Files Successfully Watermarked";
            _result = true;

            }
            else
            {
                _message = "No files found for watermark";
                _result = false;
            }

            if(!_result) _message = "PDF Files were processed with some errors.";

            return Json(new { result_WM = _result, message_WM = _message });
        }

        public List<int> GetCustomerIdfromXcelsheet(string filename)
        {
            byte[] fileBytes;
            string mimeType;

            List<int> customerIds = new List<int>();

            //var Temp_FileName = Server.MapPath("~/WMS3Cache/Watermarklist.xlsx");

            // string filePath = "~/EmailFileCache/Watermarklist.xlsx"; 

            

            string filePath = Server.MapPath("~/WMS3Cache/" + filename); 

            if (System.IO.File.Exists(filePath))
            {

                fileBytes = System.IO.File.ReadAllBytes(filePath);
                mimeType = MimeMapping.GetMimeMapping(filename);

                File(fileBytes, mimeType, filename);    
           
             fileBytes = System.IO.File.ReadAllBytes(filePath);

             MemoryStream memoryStream = new MemoryStream(fileBytes);

            if (fileBytes != null && fileBytes.Length > 0)
            {
                using (var workbook = new XLWorkbook(memoryStream))
                {
                    var worksheet = workbook.Worksheet(1); // Assuming the customer IDs are in the first worksheet


                    foreach (var cell in worksheet.CellsUsed())
                    {
                        int customerId;
                        if (int.TryParse(cell.Value.ToString(), out customerId))
                        {
                            customerIds.Add(customerId);
                        }
                    }


                }
             memoryStream.Dispose();

            }
          }

            return customerIds;
        }
       



        public JsonResult DeleteCustomerFile(int Id, int CustomerId, string FileDescription, Guid? CusId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string _message = "";
            bool _result = false;

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();

            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            CustomerFile tmpCF = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(Id);
            if (tmpCF.CustomerId != tmpCustomer.CustomerId)
            {
                return Json(new { result = false, message = "Invalid Customer" });
            }

            var Filepath = tmpCF.Filename.Replace(S3Domain, "");
            Filepath = Filepath.TrimStart('/');
            bool status = new AWSS3ObjectService().CheckFileExists(Filepath);

            //if (!System.IO.File.Exists(serverFile))
            //{
            //    return Json(new { result = true, message = "File not Exist" });
            //}

            if (status == true)
            {
                new AWSS3ObjectService().DeleteFile(Filepath);
                _Util.Facade.CustomerFileFacade.DeleteById(Id);
                base.AddUserActivityForCustomer("File is deleted #Ref:" + Id, LabelHelper.ActivityAction.Delete, CusId, null, Id.ToString());
                _message = "File deleted successfully.";
                _result = true;
            }
            else
            {
                _Util.Facade.CustomerFileFacade.DeleteById(Id);
                base.AddUserActivityForCustomer("File is deleted #Ref:" + Id, LabelHelper.ActivityAction.Delete, CusId, null, Id.ToString());
                _message = "File entry deleted";
                _result = true;
            }

            //if (System.IO.File.Exists(serverFile))
            //{
            //    System.IO.File.Delete(serverFile);
            //}

            return Json(new { result = _result, message = _message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteEquipmentFile(int Id, Guid EquipmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(EquipmentId);
            if (eq == null || eq.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Permission denied." });
            }

            EquipmentFile eqf = _Util.Facade.EquipmentFileFacade.GetById(Id);
            if (eqf.EquipmentId != eq.EquipmentId)
            {
                return Json(new { result = false, message = "invalid equipment" });
            }

            var serverFile = Server.MapPath(eqf.Filename);
            if (System.IO.File.Exists(serverFile))
            {
                System.IO.File.Delete(serverFile);
            }
            _Util.Facade.EquipmentFileFacade.DeleteById(Id);

            return Json(new { result = true });
        }

        [HttpPost]
        public JsonResult ChangeFileStatus(int id, string isActive)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CustomerFile cusFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(id);
            if (cusFile != null)
            {
                if (isActive == "false")
                {
                    cusFile.IsActive = false;
                    //cusFile.AWSUploadTS = cusFile.AWSUploadTS ?? DateTime.Now;
                    //cusFile.AWSProcessStatus = cusFile.AWSProcessStatus ?? "";
                    //cusFile.WMStatus = cusFile.WMStatus ?? "";

                }
                else
                {
                    cusFile.IsActive = true;
                    //cusFile.AWSUploadTS = DateTime.Now;
                    //cusFile.AWSProcessStatus = "";
                    //cusFile.WMStatus = "";

                }
                _Util.Facade.CustomerFileFacade.UpdateCustomerFile(cusFile);
                result = true;
            }

            return Json(new { result = result });
        }
        [Authorize]
        public PartialViewResult AddFile(int Id)
        {
            ViewBag.CustomerId = Id;
            return PartialView("_AddFile");
        }

        [Authorize]
        public PartialViewResult AddWMFile()
        {
           
           return PartialView("_AddWMFile");
        }

        #region ContactImportFile
        [Authorize]
        public PartialViewResult AddContactImportFile()
        {

            return PartialView("_AddContactImportFile");
        }
        #endregion
        #region OpportunityImportFile
        [Authorize]
        public PartialViewResult AddOpportunityImportFile()
        {

            return PartialView("_AddOpportunityImportFile");
        }
        #endregion
        #region CustomerLeadImportFile
        [Authorize]
        public PartialViewResult AddCustomerLeadImportFile(string isCustomer)
        {
            ViewBag.isCustomer = isCustomer;
            return PartialView("_AddCustomerLeadImportFile");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadCustomerLeadImportFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerLeadImportFile"];
            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerLeadImportFile"];
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserImport()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["UserImport"];
            string tempFolderName = ConfigurationManager.AppSettings["File.UserImport"];
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }
        #endregion

        #region ExpenseVendorsImportFile
        [Authorize]
        public PartialViewResult AddExpenseVendorImportFile()
        {
            return PartialView("_AddExpenseVendorImportFile");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadExpenseVendorImportFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["ExpenseVendorImportFile"];
            string tempFolderName = ConfigurationManager.AppSettings["File.ExpenseVendorImportFile"];
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult UserImportForExpenseVendor()
        //{
        //    bool isUploaded = false;
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    HttpPostedFileBase httpPostedFileBase = Request.Files["UserImport"];
        //    string tempFolderName = ConfigurationManager.AppSettings["File.UserImport"];
        //    var comname = ConfigurationManager.AppSettings["HomePageImage"];
        //    tempFolderName = string.Format(tempFolderName, comname);

        //    Random rand = new Random();
        //    string FileName = rand.Next().ToString();
        //    FileName += "-" + httpPostedFileBase.FileName;

        //    if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
        //    {

        //        string tempFolderPath = Server.MapPath("~/" + tempFolderName);

        //        if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
        //        {
        //            try
        //            {
        //                httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
        //                isUploaded = true;
        //            }
        //            catch (Exception) {  /*TODO: You must process this exception.*/}
        //        }
        //    }

        //    string filePath = string.Concat("/", tempFolderName, "/", FileName);
        //    string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
        //    return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        //}
        #endregion

        [Authorize]
        public ActionResult UploadCompanyLogo()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanyLogo"];
            /*var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();*/
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);
            // tempFolderName += "/" + logoType.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine("/", tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            string FullFilePath = "";
            string filePath = string.Concat(tempFolderName, FileName);
            FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        #region Company Signature
        [Authorize]
        public ActionResult UploadCompanySignature()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CompanyFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanySignature"];
            /*var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();*/
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);
            // tempFolderName += "/" + logoType.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine("/", tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            string FullFilePath = "";
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }
        #endregion

        [Authorize]
        public PartialViewResult AddEquipmentFile(Guid EquipmentId)
        {
            ViewBag.EquipmentId = EquipmentId;
            return PartialView("_AddEquipmentFile");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCustomerFile_v2(int CustomerId)
        {

            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result= _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return Json(new { isUploaded }, "text/html");
            //}

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];
            

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        public async Task<ActionResult> UploadCustomerFile(int CustomerId)
        {
            string FilePath="";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName="";
            string FileName="";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string SanitizedFilename = string.Empty;

            //////////////////////////////////////////// delete this after use
            ViewBag.temp = _Util.Facade.LookupFacade.GetLookupByKey("AWSProcessStatus").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("WMStatus", "Completed");

            
            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("WMStatus").ToList();
            //////////////////////////////////////
            


            //bool result= _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return Json(new { isUploaded }, "text/html");
            //}
            
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            //tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
           // tempFolderName = string.Format(tempFolderName, comname);
           // tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

          ////  Random rand = new Random();
          //  FileName = rand.Next().ToString();
          //  FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
                tempFolderName = tempFolderName.TrimEnd('/');
                var pdfTempFold = string.Format(tempFolderName, comname);
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

                Random rand = new Random();
                FileName = rand.Next().ToString();

                SanitizedFilename= Regex.Replace(httpPostedFileBase.FileName.Trim(), "[^A-Za-z0-9_. ]+", "_");

                FileName += "-___" + SanitizedFilename;

                // "mayur" //// Converting file to memory stream ////////// Start
               
                using (Stream inputStream = httpPostedFileBase.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
                // "mayur" //// Converting file to memory stream ////////// End

                FilePath = tempFolderName;
                FileKey = string.Format($"{FilePath}/{FileName}");
             
               
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, data);
                await AWSobject.MakePublic(FileName, FilePath);


                returnurl = S3Domain;
                returnurl = returnurl + FileKey;

         

                isUploaded = true;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;

                #endregion
                            
                //// "mayur" AWS S3 Changes //// End

                _fileSize = (decimal)data.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2,MidpointRounding.AwayFromZero);


            }
             
            FilePath =  FilePath + "/" + FileName;
            string FullFilePath = returnurl;
            return Json(new { isUploaded = isUploaded, filePath = FilePath, FullFilePath = FullFilePath , fileSize = _fileSize, FullPath = FileKey }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadWMCustomerFile()
        {
            string FileName = "";
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];


            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                FileName = httpPostedFileBase.FileName;

                // "mayur" //// Converting file to memory stream ////////// Start

                var filePath = Path.Combine(Server.MapPath("~/WMS3Cache"), FileName);
                httpPostedFileBase.SaveAs(filePath);

            }
            ViewBag.Watermark_filename = FileName;

        //    return FileName;
            return Json(new { FileName = FileName}, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadWMCustomerFile_v2()
        {
            string FileName = "";
            var filePath = "";
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];


            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                FileName = httpPostedFileBase.FileName;

                // "mayur" //// Converting file to memory stream ////////// Start

                filePath = Path.Combine(Server.MapPath("~/WMS3Cache"), FileName);
                httpPostedFileBase.SaveAs(filePath);

            }
            ViewBag.Watermark_filename = FileName;

            return Json(new { Watermark_filename = FileName, FullFilePath = filePath , isUploaded = true }, "text/html", JsonRequestBehavior.AllowGet);
        }


        public ActionResult UploadCompanyImage()
        {

            bool isUploaded = false;

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            tempFolderName = string.Format(tempFolderName, "CompanyImage");
            tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += httpPostedFileBase.FileName.ReplaceSpecialCharFile();

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadWriteUpsFile(int CustomerId)
        {

            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return Json(new { isUploaded }, "text/html");
            //}

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.WriteUpsFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + CurrentUser.UserId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }



        [Authorize]
        [HttpPost]
        public ActionResult UploadFileTemplate()
        {

            bool isUploaded = true;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string contents = string.Empty;
            if (httpPostedFileBase != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    httpPostedFileBase.InputStream.CopyTo(ms);
                    ms.Position = 0;

                    contents = new StreamReader(ms).ReadToEnd();
                    //contents=TextToHtml(contents);
                }
            }

            return Json(new { isUploaded = isUploaded, FullFilePath = contents }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadAgreementTemplate()
        {

            bool isUploaded = true;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string contents = string.Empty;
            if (httpPostedFileBase != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    httpPostedFileBase.InputStream.CopyTo(ms);
                    ms.Position = 0;

                    contents = new StreamReader(ms).ReadToEnd();
                    //contents=TextToHtml(contents);
                }
            }

            return Json(new { isUploaded = isUploaded, FullFilePath = contents }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadStatusImageFile()
        {

            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadDeclinedReportFile()
        {

            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.DeclinedReports"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string tempFolderPath = Server.MapPath("~/" + tempFolderName);
                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadEquipmentFile(Guid EquipmentId)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(EquipmentId, CurrentUser.CompanyId.Value);
            if (eq == null)
            {
                return Json(new { isUploaded = isUploaded }, "text/html");
            }

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];
            string tempFolderName = ConfigurationManager.AppSettings["File.EquipmentFiles"];

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + eq.Id.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadPaymentInfoFile(int CustomerId)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return Json(new { isUploaded }, "text/html");
            //}

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;
            tempFolderName += "/PaymentAttachments";
            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }
        [Authorize]
        public JsonResult ExpenseFileUpload()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["ExpenseFiles"];

            string tempFolderName = ConfigurationManager.AppSettings["File.ExpenseFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += /*"/" + CustomerId.ToString() +*/ "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;
            //tempFolderName += "/ExpenseF";
            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }



        [Authorize]
        public ActionResult TicketBookingDetailFiles(HttpPostedFileBase[] ImageFile)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string tempFileName = "";
            List<UploadStatus> status = new List<UploadStatus>();
            //HttpPostedFileBase httpPostedFileBase = Request.Files["ImageFile"];
            //if (httpPostedFileBase == null)
            //{
            //    return null;
            //}

            string filedata = "";
            filedata = ImageFile.Count().ToString();
            foreach (HttpPostedFileBase httpPostedFileBase in ImageFile)
            {
                #region Files Loop
                UploadStatus UploadStatus = new UploadStatus();
                string tempFolderName = ConfigurationManager.AppSettings["File.TicketFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                filedata += " : " + FileName + " : " + DateTime.Now.ToString();
                if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
                {

                    FileName += "_" + httpPostedFileBase.FileName;
                    string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                    if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                    {
                        try
                        {
                            Image orgImg = Bitmap.FromStream(httpPostedFileBase.InputStream);
                            foreach (var prop in orgImg.PropertyItems)
                            {
                                if (prop.Id == 0x0112) //value of EXIF
                                {
                                    int orientationValue = orgImg.GetPropertyItem(prop.Id).Value[0];
                                    RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                                    orgImg.RotateFlip(rotateFlipType);

                                    break;
                                }
                            }
                            if (orgImg.Height > 500 || orgImg.Width > 500)
                            {
                                orgImg = ImageHelper.GetImageResize(500, 500, orgImg);
                            }
                            var qualityEncoder = Encoder.Quality;
                            var quality = (long)80;
                            var ratio = new EncoderParameter(qualityEncoder, quality);
                            var codecParams = new EncoderParameters(1);
                            codecParams.Param[0] = ratio;
                            var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                            tempFileName = rand.Next().ToString();
                            tempFileName += "-___" + httpPostedFileBase.FileName + ".jpg";
                            //orgImg.Save((Path.Combine(propertyFolderPath, string.Format(fileName, "full", extention))), GetEncoder(ImageFormat.Jpeg), codecParams);
                            orgImg.Save(Path.Combine(tempFolderPath, tempFileName), ImageFormat.Jpeg);
                            codecParams.Dispose();
                            orgImg.Dispose();
                            filedata += " : Finish time - " + DateTime.Now.ToString();
                            //httpPostedFileBase.SaveAs(Path.Combine(propertyFolderPath, string.Format(fileName, "full", extention)));
                            //ustatus.IsUploaded = true;

                            //httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                            //var spimg = httpPostedFileBase.FileName.Split('.');
                            //if (spimg.Length > 0 && (spimg[1].ToLower() == "png" || spimg[1].ToLower() == "jpg" || spimg[1].ToLower() == "jpeg"))
                            //{
                            //    string tempimgpath = Server.MapPath(string.Concat("/", tempFolderName, "/", FileName));
                            //    Image img = Image.FromFile(tempimgpath);
                            //    img = ImageHelper.GetImageResize(500, 500, img);
                            //    tempFileName = rand.Next().ToString();
                            //    tempFileName += "-___" + httpPostedFileBase.FileName;
                            //    img.Save(Path.Combine(tempFolderPath, tempFileName), ImageFormat.Jpeg);
                            //}
                            //else
                            //{
                            //    tempFileName = FileName;
                            //}
                            isUploaded = true;
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Rug Ticket File Progress.txt"), true))
                            {
                                file.WriteLine(filedata);
                                file.Close();
                            }
                        }
                        catch (Exception) {  /*TODO: You must process this exception.*/}
                    }
                }
                UploadStatus.filepath = string.Concat("/", tempFolderName, "/", tempFileName);
                UploadStatus.filename = httpPostedFileBase.FileName;
                UploadStatus.filefullpath = ConfigurationManager.AppSettings["SiteDomain"] + UploadStatus.filepath;
                status.Add(UploadStatus);

                #endregion
            }

            return Json(new { isUploaded = isUploaded, status = status }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public JsonResult UploadTicketFile(Guid TicketId)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region Validations
            if (TicketId == new Guid())
            {
                return Json(new { isUploaded = isUploaded, filePath = "", message = "Invalid ticket id." }, "text/html");
            }
            Ticket Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            if (Ticket == null || Ticket.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { isUploaded = isUploaded, filePath = "", message = "Access denied." }, "text/html");
            }
            Customer Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
            if (Customer == null)
            {
                return Json(new { isUploaded = isUploaded, filePath = "", message = "Customer not found." }, "text/html");
            }

            #endregion

            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];
            string FileDescription = Request.Form["FileDescription"];

            string tempFolderName = ConfigurationManager.AppSettings["File.TicketFile"];
            var comname = CurrentUser.CompanyName.ReplaceSpecialCharFile();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName.Replace(" ", "_");
            string filePath = "";
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                        filePath = string.Concat("/", tempFolderName, "/", FileName);

                        TicketFile TicketFile = new TicketFile()
                        {
                            FileAddedBy = CurrentUser.UserId,
                            FileAddedDate = DateTime.Now.UTCCurrentTime(),
                            FileLocation = AppConfig.DomainSitePath + filePath,
                            FileName = httpPostedFileBase.FileName.ReplaceSpecialCharFile(),
                            Filesize = httpPostedFileBase.ContentLength,
                            TicketId = TicketId,
                            Description = FileDescription,
                        };
                        _Util.Facade.TicketFacade.InsertTicketFile(TicketFile);

                        CustomerFile CustomerFile = new CustomerFile()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            FileId = Guid.NewGuid(),
                            CustomerId = Ticket.CustomerId,
                            FileDescription = httpPostedFileBase.FileName.ReplaceSpecialCharFile(),
                            Filename = filePath,
                            FileSize = httpPostedFileBase.ContentLength,
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            IsActive = true,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(CustomerFile);

                        List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(Ticket.TicketId);
                        if (UserList.Count() > 0)
                        {
                            #region Insert notification
                            Notification notification = new Notification()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                NotificationId = Guid.NewGuid(),
                                Type = LabelHelper.NotificationType.Employee,
                                Who = CurrentUser.UserId,
                                What = string.Format(@"{0} attached a file on Ticket Ticket #{1}", "{0}", Ticket.Id),
                                NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Ticket.Id

                            };
                            _Util.Facade.NotificationFacade.InsertNotification(notification);
                            #endregion
                            List<Guid> assigned = UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();

                            if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                            {
                                if (Ticket.CreatedBy != CurrentUser.UserId)
                                {
                                    assigned.Add(Ticket.CreatedBy);
                                }
                            }
                            foreach (Guid item in assigned)
                            {
                                if (item != CurrentUser.UserId)
                                {
                                    #region set user to notification
                                    NotificationUser nu = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        IsRead = false,
                                        NotificationPerson = item,
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                                    #endregion
                                }
                            }
                        }
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            if (isUploaded)
            {

                #region Send Ticket Reply Notification Email

                List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(TicketId);

                string ToEmailList = "";
                if (UserList != null && UserList.Count() > 0)
                {
                    List<Guid> assigned = UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                    if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                    {
                        if (Ticket.CreatedBy != CurrentUser.UserId)
                        {
                            assigned.Add(Ticket.CreatedBy);
                        }
                    }

                    foreach (Guid item in assigned)
                    {
                        if (item != CurrentUser.UserId)
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item);
                            if (emp != null && emp.Email.IsValidEmailAddress())
                            {
                                ToEmailList += string.Format("{0}>{1} {2};", emp.Email, emp.FirstName, emp.LastName);
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ToEmailList))
                {

                    string CustomerName = Customer.FirstName + " " + Customer.LastName;
                    if (Customer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(Customer.BusinessName))
                    {
                        CustomerName = Customer.BusinessName;
                    }

                    TicketNotificationEmails TicketReplyNotificationEmail = new TicketNotificationEmails()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedByName = CurrentUser.GetFullName(),
                        TicketMessage = string.Format("Attached a file {0}", httpPostedFileBase.FileName.ReplaceSpecialCharFile()),
                        CreatedForCustomerName = CustomerName,
                        TicketNumber = string.Format("Ticket #{0}", Ticket.Id),
                        ToEmail = ToEmailList,
                        HeaderMessage = "A new file has been added",
                        Subject = string.Format("A new file has been added to Ticket #{0}", Ticket.Id),
                        BodyMessage = string.Format("A new file has been attached by {0} on Ticket #{1} for Customer {2}. {3}", CurrentUser.GetFullName(), Ticket.Id, Customer.FirstName + " " + Customer.LastName, FileDescription),
                        TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                    };
                    _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketReplyNotificationEmail);
                }
                #endregion
            }


            return Json(
                new
                {
                    isUploaded = isUploaded,
                    filePath = filePath,
                    message = "File uploaded successfully."
                }, "text/html");
        }



        [Authorize]
        [HttpPost]
        public JsonResult SaveWatermarkFile(string File, int CustomerId, string Description, string InvoiceId, double _fileSize, String _FullPath)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = File;

            byte[] bytes;
            //var Filepath = serverFile.Replace(AppConfig.AWSS3Url, "");
            //bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!rsult)
            //{

            //    if (System.IO.File.Exists(serverFile))
            //    {
            //        System.IO.File.Delete(serverFile);
            //    }
            //    return Json(new { result = true,message="invalid user" });
            //}

            bool status = new AWSS3ObjectService().CheckFileExists(serverFile);

            //if (!System.IO.File.Exists(serverFile))
            //{
            //    return Json(new { result = true, message = "File not Exist" });
            //}

            if (!status == true)
            {
                return Json(new { result = true, message = "File not Exist" });

            }

            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);


            var OnlyFileName = serverFile.Split('/');

            //// ""Mayur" Calculate File Size : start : already in kb
            #region Calculate file size

            double fileSize = _fileSize;

            #endregion
            //// ""Mayur" Calculate File Size : End

            // var delimeter = new string[] { "-___" };
            // var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            CustomerFile cf = new CustomerFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileId = Guid.NewGuid(),
                CustomerId = tmpCustomer.CustomerId,
                FileDescription = tmpCustomer.Id + "_" + Description,
                Filename = "/" + _FullPath,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = Description,
                IsActive = true,
                InvoiceId = InvoiceId,
                FileSize = fileSize,
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };

            bool result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cf) > 0;
            if (result)
            {
                base.AddUserActivityForCustomer("New File added", LabelHelper.ActivityAction.AddFile, tmpCustomer.CustomerId, null, null);

            }
            return Json(new { result = result, fileid = cf.Id, filename = cf.Filename, filedescription = cf.FileDescription });
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveCustomerFile(string File, int CustomerId, string Description, string InvoiceId,double _fileSize,String _FullPath)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = File;

            byte[] bytes;
            //var Filepath = serverFile.Replace(AppConfig.AWSS3Url, "");
            //bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!rsult)
            //{

            //    if (System.IO.File.Exists(serverFile))
            //    {
            //        System.IO.File.Delete(serverFile);
            //    }
            //    return Json(new { result = true,message="invalid user" });
            //}

            bool status = new AWSS3ObjectService().CheckFileExists(serverFile);

            //if (!System.IO.File.Exists(serverFile))
            //{
            //    return Json(new { result = true, message = "File not Exist" });
            //}

            if(!status==true)
            {
                return Json(new { result = true, message = "File not Exist" });

            }
    
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);

           
            var OnlyFileName = serverFile.Split('/');

             //// ""Mayur" Calculate File Size : start : already in kb
            #region Calculate file size

             double fileSize = _fileSize;

            #endregion
            //// ""Mayur" Calculate File Size : End

           // var delimeter = new string[] { "-___" };
           // var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            CustomerFile cf = new CustomerFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileId = Guid.NewGuid(),
                CustomerId = tmpCustomer.CustomerId,
                FileDescription = tmpCustomer.Id+"_"+ Description,
                Filename = "/" + _FullPath,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = Description,
                IsActive = true,
                InvoiceId = InvoiceId,
                FileSize = fileSize,
                CreatedBy = CurrentUser.UserId,  
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };

            bool result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cf) > 0;
            if (result)
            {
                base.AddUserActivityForCustomer("New File added", LabelHelper.ActivityAction.AddFile, tmpCustomer.CustomerId, null, null);

            }
            return Json(new { result = result, fileid = cf.Id, filename = cf.Filename, filedescription = cf.FileDescription });
        }


        public JsonResult SaveCustomerFile_v2(string File, int CustomerId, string Description, string InvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = File;

            //bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId,CurrentUser.CompanyId.Value);
            //if (!rsult)
            //{

            //    if (System.IO.File.Exists(serverFile))
            //    {
            //        System.IO.File.Delete(serverFile);
            //    }
            //    return Json(new { result = true,message="invalid user" });
            //}

            bool status = new AWSS3ObjectService().CheckFileExists(serverFile);

            //if (!System.IO.File.Exists(serverFile))
            //{
            //    return Json(new { result = true, message = "File not Exist" });
            //}

            if (!status == true)
            {
                return Json(new { result = true, message = "File not Exist" });

            }

            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            var OnlyFileName = File.Split('/');

            float fileSize = OnlyFileName[9].Length;

            //// ""Mayur" Calculate File Size : start
            #region Calculate file size

            var _fileSize = (decimal)OnlyFileName[9].Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

            #endregion
            //// ""Mayur" Calculate File Size : End

            var delimeter = new string[] { "-___" };
            var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            CustomerFile cf = new CustomerFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileId = Guid.NewGuid(),
                CustomerId = tmpCustomer.CustomerId,
                FileDescription = tmpCustomer.Id + "_" + Description,
                Filename = File,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = fullFileName[1],
                IsActive = true,
                InvoiceId = InvoiceId,
                FileSize = (double)_fileSize,
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };

            bool result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cf) > 0;
            if (result)
            {
                base.AddUserActivityForCustomer("New File added", LabelHelper.ActivityAction.AddFile, tmpCustomer.CustomerId, null, null);

            }
            return Json(new { result = result, fileid = cf.Id, filename = cf.Filename, filedescription = cf.FileDescription });
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveEquipmentFile(string File, Guid EquipmentId, string Description, bool IsProfilePicture)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            Equipment Equpment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(EquipmentId);
            var OnlyFileName = File.Split('/');
            float fileSize = OnlyFileName[6].Length;
            var delimeter = new string[] { "-___" };
            var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            EquipmentFile profileEquipment = new EquipmentFile();
            if (IsProfilePicture)
            {
                profileEquipment = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(EquipmentId).Where(x => x.FileType == LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
            }
            //if (IsProfilePicture)
            //{
            //    var fileList =_Util.Facade.EquipmentFileFacade.RemoveEquipmentFileByEquipmentIdAndFileType(EquipmentId,LabelHelper.EquipmentFileType.ProfilePicture);
            //    if (fileList.Count() > 0)
            //    {
            //        foreach(var item in fileList)
            //        {
            //            try
            //            {
            //                var FilePath = Server.MapPath(item.Filename);

            //                if (System.IO.File.Exists(FilePath))
            //                {
            //                    System.IO.File.Delete(FilePath);
            //                }
            //            }
            //            catch (Exception) { }
            //        }
            //    }
            //}
            EquipmentFile ef = new EquipmentFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                EquipmentId = EquipmentId,
                FileDescription = Description,
                Filename = File,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = fullFileName[1],
                IsActive = true,
                FileSize = fileSize,
                FileType = IsProfilePicture ? LabelHelper.EquipmentFileType.ProfilePicture : LabelHelper.EquipmentFileType.File
            };


            bool result = _Util.Facade.EquipmentFileFacade.InsertEquipmentFile(ef) > 0;
            if (result && profileEquipment != null)
            {
                profileEquipment.FileType = LabelHelper.EquipmentFileType.File;
                _Util.Facade.EquipmentFileFacade.UpdateEquipmentFile(profileEquipment);
            }
            return Json(new { result = result });
        }

               

        [Authorize]
        public ActionResult SendFileForCustomerReview(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer model = new Customer();
            if (id.HasValue)
            {
                var objfile = _Util.Facade.CustomerFileFacade.GetFileNameById(id.Value);
                if (objfile != null)
                {
                    model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objfile.CustomerId);
                    ViewBag.Subject = objfile.FileDescription;
                    ViewBag.BodyContent = _Util.Facade.MailFacade.GetTemplateByTemplateKey("FileAttachmentPredefinedTemplate").BodyContent;
                    ViewBag.fileid = objfile.Id;
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SendFileForCustomerReview(string EmailAddress, string ccMail, string SubjectList, string FileBody, int? fileid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;

            WebClient webClient;
            byte[] fileBytes1;
            string Temp_FileName;

            if (fileid.HasValue)
            {
                var objfile = _Util.Facade.CustomerFileFacade.GetFileNameById(fileid.Value);
                if (objfile != null)
                {
                    var FileName = objfile.Filename;
                    FileName = FileName.Split('/').Last();
                    List<string> ListEmailAddress = ccMail.Split(';').ToList<string>();

                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    var Filepath = objfile.Filename;

                    string Full_Path = S3Domain + Filepath.TrimStart('/');


                    /// Mayur :: File Download to temp folder :start

                    webClient = new WebClient();
                    fileBytes1 = webClient.DownloadData(Full_Path);

                    File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();



                    Temp_FileName = Server.MapPath("~/EmailFileCache/tmp_" + FileName);

                    if (!System.IO.File.Exists(Temp_FileName))
                    {
                        System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
                    }

                    /// Mayur :: File Download to temp folder :End

                    foreach (var item in ListEmailAddress)
                    {
                        FileAttachmentEmail email = new FileAttachmentEmail()
                        {
                            CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                            ToEmail = EmailAddress,
                            EmailBody = FileBody,
                            Subject = SubjectList,
                            FileAttachmentPdf = new Attachment(
                                      Temp_FileName,
                                     MediaTypeNames.Application.Octet)
                        };
                        result = _Util.Facade.MailFacade.SendFileAttachmentForCustomerReview(email, CurrentUser.CompanyId.Value);
                        
                    }
                    if (result)
                    {
                        LeadCorrespondence objCor = new LeadCorrespondence()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = objfile.CustomerId,
                            TemplateKey = "FileAttachmentForCustomerReview",
                            Type = LabelHelper.CorrespondenceMessageTyp.Email,
                            ToEmail = EmailAddress,
                            Subject = SubjectList,
                            BodyContent = FileBody,
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            IsSystemAutoSent = true,
                            SentBy = CurrentUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                    }

                    //// Mayur :: delete temp file : Start


                    //if (System.IO.File.Exists(Temp_FileName))
                    //{
                    //  
                    //    System.IO.File.Delete(Temp_FileName);
                    //}

                    //// Mayur :: delete temp file : End
                }
            }


            return Json(result);
        }

        public JsonResult SendFileForCustomerReview_v2(string EmailAddress, string ccMail, string SubjectList, string FileBody, int? fileid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (fileid.HasValue)
            {
                var objfile = _Util.Facade.CustomerFileFacade.GetFileNameById(fileid.Value);
                if (objfile != null)
                {
                    var filename = objfile.Filename;
                    List<string> ListEmailAddress = ccMail.Split(';').ToList<string>();
                    foreach (var item in ListEmailAddress)
                    {
                        FileAttachmentEmail email = new FileAttachmentEmail()
                        {
                            CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                            ToEmail = EmailAddress,
                            EmailBody = FileBody,
                            Subject = SubjectList,
                            FileAttachmentPdf = new Attachment(
                                      FileHelper.GetFileFullPath(filename),
                                     MediaTypeNames.Application.Octet)
                        };
                        result = _Util.Facade.MailFacade.SendFileAttachmentForCustomerReview(email, CurrentUser.CompanyId.Value);
                    }
                    if (result)
                    {
                        LeadCorrespondence objCor = new LeadCorrespondence()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = objfile.CustomerId,
                            TemplateKey = "FileAttachmentForCustomerReview",
                            Type = LabelHelper.CorrespondenceMessageTyp.Email,
                            ToEmail = EmailAddress,
                            Subject = SubjectList,
                            BodyContent = FileBody,
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            IsSystemAutoSent = true,
                            SentBy = CurrentUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                    }
                }
            }
            return Json(result);
        }

        public ActionResult PurchaseOrder(string token)
        {
            try
            {
                string POId = DESEncryptionDecryption.DecryptCipherTextToPlainText(token);
                CreatePurchaseOrder model = new CreatePurchaseOrder();
                model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByPurchaseOrderId(POId);
                if (model.PurchaseOrderWarehouse == null)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }
                model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(model.PurchaseOrderWarehouse.CompanyId);
                model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(model.PurchaseOrderWarehouse.SuplierId);

                string filename = string.Concat(model.PurchaseOrderWarehouse.PurchaseOrderId, ".pdf");
                return File(CreatePurchasOrderPdf(model), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }

        private byte[] CreatePurchasOrderPdf(CreatePurchaseOrder Model)
        {
            if (Model.PurchaseOrderWarehouse == null || Model.PurchaseOrderWarehouse.CompanyId == new Guid())
            {
                return null;
            }
            if (Model.Supplier == null)
            {
                Model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(Model.PurchaseOrderWarehouse.SuplierId);
            }
            if (Model.Company == null)
            {
                Model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.PurchaseOrderWarehouse.CompanyId);
            }
            if (string.IsNullOrWhiteSpace(Model.Company.CompanyLogo))
            {
                Model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(Model.PurchaseOrderWarehouse.CompanyId);
            }
            if (string.IsNullOrWhiteSpace(Model.CompanyAddressFormat))
            {
                Model.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(Model.PurchaseOrderWarehouse.CompanyId);
            }

            Hashtable datatemplate = new Hashtable();
            #region CompanyAddress Info
            datatemplate.Add("ComapnyName", Model.Company.CompanyName);
            datatemplate.Add("Address", Model.Company.Address);
            datatemplate.Add("Street", Model.Company.Street);
            datatemplate.Add("City", Model.Company.City);
            datatemplate.Add("State", Model.Company.State);
            datatemplate.Add("Zip", Model.Company.ZipCode);
            datatemplate.Add("CompanyPhone", Model.Company.Phone);
            datatemplate.Add("EmailAddress", Model.Company.EmailAdress);
            datatemplate.Add("WebAddress", Model.Company.Website);
            Model.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(Model.CompanyAddressFormat, datatemplate);
            #endregion
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/PurchaseOrder/PurchaseOrderPdf.cshtml", Model)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },
            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            return applicationPDFData;

        }

        [Authorize]
        public ActionResult FileTemplate()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<FileTemplate> TemplteList = _Util.Facade.FileFacade.GetAllTemplate().OrderBy(x=> x.FileName).ToList();

            return PartialView("_FileTemplate", TemplteList);
        }
        [Authorize]
        public ActionResult ContractAgreementTemplate()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<ContractAgreementTemplate> TemplteList = _Util.Facade.FileFacade.GetAllContractAgreeemntTemplate().OrderBy(x => x.Name).ToList();

            return PartialView(TemplteList);
        }
        [Authorize]
        public ActionResult AddFiletemplate(int? Id)
        {
            FileTemplate ft = new FileTemplate();
            if (Id > 0)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(Id.Value);
            }
            return View(ft);
        }
        [Authorize]
        public ActionResult AddAgreementtemplate(int? Id)
        {
            ContractAgreementTemplate ft = new ContractAgreementTemplate();
            if (Id > 0)
            {
                ft = _Util.Facade.FileFacade.GetAgreementTemplateById(Id.Value);
            }
            return View(ft);
        }

        [Authorize]
        public ActionResult AddAgreementtemplateReview(int? Id, int? leadid, string invoiceid)
        {
            ContractAgreementTemplate ft = new ContractAgreementTemplate();
            CustomerAgreementTemplate cft = new CustomerAgreementTemplate();
            if (leadid.HasValue && leadid.Value > 0 && Id.HasValue && Id.Value > 0)
            {
                Customer objcus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);
                if (objcus != null)
                {
                    cft = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(Id.Value, objcus.CustomerId, false);
                }
            }

            if (Id > 0)
            {
                ft = _Util.Facade.FileFacade.GetAgreementTemplateById(Id.Value);
            }
            ViewBag.leadid = leadid.HasValue ? leadid.Value : 0;
            ViewBag.invoiceid = invoiceid;
            if (cft != null)
            {
                ft.Id = cft.ReferenceTemplateId.Value;
                ft.Name = cft.Name;
                ft.BodyContent = cft.BodyContent;
                ft.Description = cft.Description;
                ft.CompanyId = cft.CompanyId;
            }
            return View(ft);
        }

        #region Contract Diagram
        [HttpPost]
        public JsonResult SaveContractDiagramImage(string data, string invoiceid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            bool uploadImage = false;
            string filePath = "";
            //var leadID = LeadConvertId;
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerContractDiagramFile"];
                var comname = CurrentUser.CompanyName;//_Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + invoiceid + "Diagram";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Diagram.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            if (!string.IsNullOrWhiteSpace(invoiceid))
            {
                Invoice tempInv = _Util.Facade.InvoiceFacade.GetByInvoiceId(invoiceid);
                if(tempInv != null)
                {
                    tempInv.InvoiceContractDiagram = filePath;
                }
                uploadImage = _Util.Facade.InvoiceFacade.UpdateInvoice(tempInv);
            }
            else
            {
                uploadImage = false;
                return Json(new { uploadImage = uploadImage, message = "Invalid Invoice." });
            }
            
            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }
        #endregion

        [Authorize]
        public ActionResult LoadFiletemplateForFileManagement(int? Id, int? customerId, bool? IsCancellationQueue)
        {
            FileTemplate ft = new FileTemplate();
            CustomerAgreementTemplate cft = new CustomerAgreementTemplate();
            if (IsCancellationQueue == true)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateByName("Cancellation");
                if (ft != null && ft.Id > 0)
                {
                    Id = ft.Id;
                }
                else
                {
                    return PartialView("_CancellationFileUploadMessage");
                    //return Json(new { result = false, message = "Cancellation file is not found. Please upload a file." });
                }
            }
            if (customerId.HasValue && customerId.Value > 0 && Id.HasValue && Id.Value > 0)
            {
                Customer objcus = _Util.Facade.CustomerFacade.GetCustomerById(customerId.Value);
                if (objcus != null)
                {
                    cft = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(Id.Value, objcus.CustomerId, true);
                }
            }
            if (Id > 0 && cft == null && IsCancellationQueue != true)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(Id.Value);
            }
            ViewBag.customerId = customerId.HasValue ? customerId.Value : 0;
            if (cft != null && cft.Id > 0)
            {
                ft.Id = cft.ReferenceTemplateId.Value;
                ft.FileName = cft.Name;
                ft.FileBody = cft.BodyContent;
                ft.FileDescription = cft.Description;
                ft.CompanyId = cft.CompanyId;
            }
            return View(ft);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddFileTemplate(CustomerAgreementTemplate ft, bool RestoreDefault)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region FileTemplate
            if (ft.ReferenceTemplateId == null)
            {
                FileTemplate f = new FileTemplate();
                if (ft.BodyContent == null)
                {
                    return Json(new { result = false, message = "Please upload a file or fill up body content." });
                }
                if (RestoreDefault && ft.Id > 0)
                {
                    f = _Util.Facade.FileFacade.GetFileTemplateById(ft.Id);
                    _Util.Facade.FileFacade.UpdateFileTemplate(f);
                    return Json(new { result = true, message = "File template restored to default successfully." });
                }
                if (ft.Id > 0)
                {
                    f = _Util.Facade.FileFacade.GetFileTemplateById(ft.Id);
                    f.LastUpdatedBy = CurrentUser.UserId;
                    f.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    f.FileBody = ft.BodyContent;
                    f.IsCustomerSignRequired = ft.IsCustomerSignRequired;
                    if (!string.IsNullOrWhiteSpace(ft.Name) && ft.Name != "" && !string.IsNullOrWhiteSpace(ft.Description) && ft.Description != "")
                    {
                        f.FileName = ft.Name;
                        f.FileDescription = ft.Description;
                    }
                    _Util.Facade.FileFacade.UpdateFileTemplate(f);
                    return Json(new { result = true, message = "File template updated successfully." });
                }
                else
                {
                    f = new FileTemplate();
                    f.FileName = ft.Name;
                    f.FileBody = ft.BodyContent;
                    f.FileDescription = ft.Description;
                    f.IsCustomerSignRequired = ft.IsCustomerSignRequired;
                    f.CreatedBy = CurrentUser.UserId;
                    f.CreatedDate = DateTime.Now.UTCCurrentTime();
                    f.LastUpdatedBy = CurrentUser.UserId;
                    f.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.FileFacade.InsertFileTemplate(f);
                    return Json(new { result = true, message = "File template saved successfully." });
                }
            }
            #endregion

            #region CustomerAgreementTemplate
            if (ft.leadid > 0)
            {
                Customer temCus = _Util.Facade.CustomerFacade.GetCustomerById(ft.leadid);
                ft.CustomerId = temCus.CustomerId;
            }
            CustomerAgreementTemplate tempet = new CustomerAgreementTemplate();
            if (ft.ReferenceTemplateId > 0 && ft.CustomerId != null)
            {
                tempet = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(ft.ReferenceTemplateId.Value, ft.CustomerId, true);
            }
            if (ft.BodyContent == null)
            {
                return Json(new { result = false, message = "Please upload a file or fill up body content." });
            }
            if (RestoreDefault && tempet != null)
            {
                _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
                return Json(new { result = true, message = "File template restored to default successfully.", tempetId = tempet.Id });
            }
            //else if(tempet != null)
            //{
            //    tempet.CompanyId = CurrentUser.CompanyId.Value;
            //    tempet.CustomerId = ft.CustomerId;
            //    tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
            //    tempet.IsFileTemplate = true;
            //    tempet.LastUpdatedBy = CurrentUser.UserId;
            //    tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            //    tempet.BodyContent = ft.BodyContent;
            //    if(!string.IsNullOrWhiteSpace(ft.Name) && ft.Name!="" && !string.IsNullOrWhiteSpace(ft.Description) && ft.Description!="")
            //    {
            //        tempet.Name = ft.Name;
            //        tempet.Description = ft.Description;
            //    }
            //    _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
            //    return Json(new { result = true, message = "File template updated successfully." , tempetId = tempet.Id });
            //}
            else if (tempet == null || tempet != null)
            {
                tempet = new CustomerAgreementTemplate();
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
                tempet.CustomerId = ft.CustomerId;
                tempet.Name = ft.Name;
                tempet.IsFileTemplate = true;
                tempet.BodyContent = ft.BodyContent;
                tempet.Description = ft.Description;
                tempet.CreatedBy = CurrentUser.UserId;
                tempet.CreatedDate = DateTime.Now.UTCCurrentTime();
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                long temId = _Util.Facade.FileFacade.InsertCustomerAgreementTemplate(tempet);
                return Json(new { result = true, message = "File template saved successfully.", tempetId = tempet.Id });
            }
            #endregion

            else
            {
                return Json(new { result = false, message = "File template saved failed." });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAgreementTemplate(ContractAgreementTemplate ft, bool RestoreDefault)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ContractAgreementTemplate tempet = _Util.Facade.FileFacade.GetAgreementTemplateById(ft.Id);
            if (ft.BodyContent == null)
            {
                return Json(new { result = false, message = "Please upload a file or fill up body content." });
            }
            if (RestoreDefault)
            {
                _Util.Facade.FileFacade.UpdateAgreementTemplate(tempet);
                return Json(new { result = true, message = "Agreement template restored to default successfully." });
            }
            else if (ft.Id > 0)
            {
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                tempet.BodyContent = ft.BodyContent;
                tempet.IsDrawDiagram = ft.IsDrawDiagram;
                if (!string.IsNullOrWhiteSpace(ft.Name) && ft.Name != "" && !string.IsNullOrWhiteSpace(ft.Description) && ft.Description != "")
                {
                    tempet.Name = ft.Name;
                    tempet.Description = ft.Description;
                }
                _Util.Facade.FileFacade.UpdateAgreementTemplate(tempet);
                return Json(new { result = true, message = "Agreement template updated successfully." });
            }
            else if (tempet == null)
            {
                tempet = new ContractAgreementTemplate();
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.Name = ft.Name;
                tempet.BodyContent = ft.BodyContent;
                tempet.IsDrawDiagram = ft.IsDrawDiagram;
                tempet.Description = ft.Description;
                tempet.CreatedBy = CurrentUser.UserId;
                tempet.CreatedDate = DateTime.Now.UTCCurrentTime();
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.FileFacade.InsertAgreementTemplate(tempet);
                return Json(new { result = true, message = "Agreement template saved successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Agreement template saved failed." });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAgreementTemplateReview(CustomerAgreementTemplate ft, bool RestoreDefault)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (ft.leadid > 0)
            {
                Customer temCus = _Util.Facade.CustomerFacade.GetCustomerById(ft.leadid);
                ft.CustomerId = temCus.CustomerId;
            }
            CustomerAgreementTemplate tempet = new CustomerAgreementTemplate();
            if (ft.ReferenceTemplateId > 0 && ft.CustomerId != null)
            {
                tempet = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(ft.ReferenceTemplateId.Value, ft.CustomerId, false);
            }
            if (ft.BodyContent == null)
            {
                return Json(new { result = false, message = "Please upload a file or fill up body content." });
            }
            if (RestoreDefault && tempet != null)
            {
                _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
                return Json(new { result = true, message = "Agreement template restored to default successfully.", tempetId = tempet.Id });
            }
            //else if (tempet != null)
            //{
            //    tempet.CompanyId = CurrentUser.CompanyId.Value;
            //    tempet.CustomerId = ft.CustomerId;
            //    tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
            //    tempet.IsFileTemplate = false;
            //    tempet.LastUpdatedBy = CurrentUser.UserId;
            //    tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            //    tempet.BodyContent = ft.BodyContent;
            //    if (!string.IsNullOrWhiteSpace(ft.Name) && ft.Name != "" && !string.IsNullOrWhiteSpace(ft.Description) && ft.Description != "")
            //    {
            //        tempet.Name = ft.Name;
            //        tempet.Description = ft.Description;
            //    }
            //    _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
            //    return Json(new { result = true, message = "Agreement template updated successfully.", tempetId = tempet.Id });
            //}

            else if (tempet == null || tempet != null)
            {
                tempet = new CustomerAgreementTemplate();
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
                tempet.CustomerId = ft.CustomerId;
                tempet.Name = ft.Name;
                tempet.IsFileTemplate = false;
                tempet.BodyContent = ft.BodyContent;
                tempet.Description = ft.Description;
                tempet.CreatedBy = CurrentUser.UserId;
                tempet.CreatedDate = DateTime.Now.UTCCurrentTime();
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                long temId = _Util.Facade.FileFacade.InsertCustomerAgreementTemplate(tempet);
                return Json(new { result = true, tempetId = tempet.Id, message = "Agreement template saved successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Agreement template saved failed." });
            }

        }

        public ActionResult LeadDocumentFileManagementPartial(int? CustomerId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<FileTemplate> model = new List<FileTemplate>();
            List<FileTemplate> model1 = new List<FileTemplate>();

            model1 = _Util.Facade.FileFacade.GetAllTemplateForDropdown();
            model = _Util.Facade.FileFacade.GetTemplateWithoutPermissionForDropdown();

            List<SelectListItem> FileTemplateList = new List<SelectListItem>();

            FileTemplateList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });

            if (model != null && model.Count() > 0)

            {

                FileTemplateList.AddRange(model.Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileSmartAgreementDFW))
                {


                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Smart Agreement DFW").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileInstallationCompletionChecklist))
                {

                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Installation Completion Checklist").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileServiceCallCompletionChecklist))
                {
                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Service Call Completion Checklist").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }

                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileCancellation))
                {

                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Cancellation").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }


            }


            ViewBag.FileTemplateList = FileTemplateList;


            ViewBag.customerId = CustomerId.Value;
            return View();
        }
        public ActionResult DocumentFileManagementPartialForLead(int? CustomerId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<FileTemplate> model = new List<FileTemplate>();
            List<FileTemplate> model1 = new List<FileTemplate>();

            model1 = _Util.Facade.FileFacade.GetAllTemplateForDropdown();
            model = _Util.Facade.FileFacade.GetTemplateWithoutPermissionForDropdown();

            List<SelectListItem> FileTemplateList = new List<SelectListItem>();

            FileTemplateList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });

            if (model != null && model.Count() > 0)

            {

                FileTemplateList.AddRange(model.Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileSmartAgreementDFW))
                {


                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Smart Agreement DFW").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileInstallationCompletionChecklist))
                {

                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Installation Completion Checklist").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileServiceCallCompletionChecklist))
                {
                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Service Call Completion Checklist").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }

                if (base.IsPermitted(UserPermissions.CustomerPermissions.ShowFileCancellation))
                {

                    FileTemplateList.AddRange(model1.Where(x => x.FileName == "Cancellation").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FileName.ToString(),
                                    Value = x.Id.ToString()
                                }).ToList());


                }


            }


            ViewBag.FileTemplateList = FileTemplateList;


            ViewBag.customerId = CustomerId.Value;
            return View();
        }
        [Authorize]
        public ActionResult GetFileTemplateForPopUp(int? fileTemplateId, int? customerId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (customerId.HasValue)
            {
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(customerId.Value);
                ViewBag.CustomerId = customerId;
            }
            //if (!string.IsNullOrWhiteSpace(CancellationDate))
            //{
            //    ViewBag.CancellationDate = CancellationDate;
            //}
            //if (RemainingBalance.HasValue)
            //{
            //    ViewBag.RemainingBalance = RemainingBalance;
            //}
            if (fileTemplateId.HasValue)
            {
                ViewBag.FileTemplateId = fileTemplateId;
            }
            ViewBag.UserId = CurrentUser.UserId;
            return View(model);
        }
        #region Customer Addendum
        public string GetCustomerAddendum(int? FileTemplateId, int? CustomerId)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            Guid CompanyId = new Guid();
            Customer cus = new Customer();
            FileTemplate ft = new FileTemplate();
            CustomerAgreementTemplate cat = new CustomerAgreementTemplate();
            if (FileTemplateId.HasValue)
            {
                cat = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(FileTemplateId.Value);
            }
            if (cat != null && cat.IsFileTemplate == true && cat.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(cat.ReferenceTemplateId.Value);
            }
            //var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            cus = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
            //Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            //List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
            //List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            //List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();

            //if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            //{
            //    EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2).ToList();
            //    ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            //}
            Company com = new Company();
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();

            if (cus != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    CompanyId = CurrentUser.CompanyId.Value;
                }
                else
                {
                    custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                    CompanyId = custommerCompany.CompanyId;
                }
                com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                var CustomerAddress = AddressHelper.MakeAddress(cus);
                CustomerSignature csA = new CustomerSignature();
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (cus != null && FileTemplateId != 0)
                {
                    csA = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(cus.CustomerId, FileTemplateId.ToString(), "File Management");
                }
                if (csA != null && FileTemplateId != 0)
                {
                    cusSignature = csA.Signature;
                    if (csA.CreatedDate != null && csA.CreatedDate != new DateTime())
                    {
                        cussignDate = csA.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = csA.CreatedDate.UTCToClientTime();
                    }
                }
                //var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    InstallAddress = CustomerAddress,
                    //TicketId = ticket.Id,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    //ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    //TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = cusSignature,
                    CustomerAddendumSignatureDate = cussignDateVal,
                    CustomerAddendumStringSignatureDate = cussignDate
                };
                if (csA != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(csA.Signature))
                {
                    cusAddendum.CompanySignature = glbs.Value;
                    if (csA.CreatedDate != null && csA.CreatedDate != new DateTime())
                    {
                        cusAddendum.CompanySignatureDate = cussignDate;
                    }
                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    cusAddendum.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
                cusAddendum.ServiceEqpList = new List<CustomerAppointmentEquipment>();
                cusAddendum.EquipmentList = new List<CustomerAppointmentEquipment>();
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
            }
            string body = _Util.Facade.FileFacade.MakeCustomerAddendumPdf(cusAddendum, FileTemplateId);
            return body;
        }
        #endregion

        #region Number to Words for contract term
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            words = textInfo.ToTitleCase(words);
            return words;
        }
        #endregion

        #region Smart Agreement File Body
        public string GetSmartAgreementFileContent(int? FileTemplateId, int? CustomerId)
        {
            InstallationAgreementModel Model = new InstallationAgreementModel();
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            var taxtotal = 0.0;
            Customer Cus = new Customer();
            CustomerExtended CusExd = new CustomerExtended();
            Company Com = new Company();
            Guid CompanyId = new Guid();
            //var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(CustomerId.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            if (CustomerId.Value != 0)
            {
                if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CompanyId))
                {
                    return null;
                }

                Cus = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (Cus != null)
                {
                    CusExd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                }
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }
                var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Com.CompanyId, Cus.CustomerId);
                if (objCusAgree == null)
                {
                    CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = Com.CompanyId,
                        CustomerId = Cus.CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.LoadAgreement,
                        AddedDate = DateTime.UtcNow
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
                }
                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                var ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var NotARBEnabledTotalPrice = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                string contractCreatedDateVal = "";
                if (CusExd.ContractCreatedDate != null)
                {
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.UTCToClientTime().ToString("M/d/yy");
                }
                else
                {
                    CusExd.ContractCreatedDate = DateTime.UtcNow;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(CusExd);
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.UTCToClientTime().ToString("M/d/yy");
                }
                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                if (PackageCustomer != null)
                {
                    if (PackageCustomer.NonConforming && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue < PackageCustomer.MinCredit || Cus.CreditScoreValue > PackageCustomer.MaxCredit))
                    {
                        IsNonConfirming = true;
                        NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                    }
                }


                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerIdNew = new Guid();
                    if (Cus != null)
                    {
                        CustomerIdNew = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerIdNew);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, false, 0);
                if (CustomEquipmentList.Count > 0)
                {
                    foreach (var item in CustomEquipmentList)
                    {
                        EquipmentTotalPrice += item.Total;
                        UpfrontAddOnTotal += item.Total;
                    }
                }

                var CustomServiceList = _Util.Facade.EquipmentFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, false, 0);
                if (CustomServiceList.Count > 0)
                {
                    foreach (var item in CustomServiceList)
                    {
                        EquipmentTotalPrice += item.Total;
                        ServiceTotalPrice += item.Total;
                        MonthlyServiceFeeTotal += item.Total;
                    }
                }
                var NotARBEnabledServiceList = new List<Equipment>();
                NotARBEnabledServiceList = _Util.Facade.EquipmentFacade.GetNotARBEnabledSmartServiceListFromService(Cus.CustomerId, CompanyId);
                if (NotARBEnabledServiceList.Count > 0)
                {
                    foreach (var item in NotARBEnabledServiceList)
                    {
                        NotARBEnabledTotalPrice += item.Total;
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);
                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = ServiceTotalPrice;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;

                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                    TotalDueAtSigning = NewSubTotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                    TotalDueAtSigning = NewSubTotal;
                }
                var PackageCustomerDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, CompanyId);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = _Util.Facade.PackageFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    SmartPackageEquipmentServiceList = _Util.Facade.PackageFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, CompanyId);
                }
                var PaymentDetails = _Util.Facade.PaymentInfoFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId).Where(m => m.PayFor == "First Month").FirstOrDefault();
                var emercontact = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId);
                string overviewheader = "";
                string overviewdata = "";
                if (emercontact != null && emercontact.Count > 0)
                {
                    overviewheader = @"<table style='border-collapse:collapse; width:100%;font-family:Arial;table-layout:fixed;font-size:13px;margin-top:20px;'>
                                                          <thread>
                                            <tr style='height:30px;'>
                                  <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>Name</th>
   
                                   <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> RelationShip </th>
    
                                    <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> Phone </th>
     
                                     <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> Type </th>
      
                                  </tr>
                                            </thread>
                                            <tbody>
                                            {0}
                                            </tbody>
                                        </table>";
                    foreach (var item in emercontact)
                    {
                        overviewdata += @"<tr style='height: 30px;'>
                                  <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.FirstName + " " + item.LastName + @"</td>
   
                                   <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.RelationShip + @"</td>
    
                                    <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.Phone + @"</td>
     
                                     <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.PhoneType + @"</td>
      
                                  </tr> ";
                    }
                }
                var agreementPayment = _Util.Facade.PaymentInfoFacade.GetLeadAgreementPaymentInfoByCustomerId(Cus.CustomerId);
                string paymentoverviewheader = "";
                string paymentoverviewdata = "";
                if (agreementPayment != null && agreementPayment.Count > 0)
                {
                    paymentoverviewheader = "<table style='border-collapse:collapse; width:100%; font-family:Arial; table-layout:fixed; font-size:13px;'>{0}</table>";
                    foreach (var pay in agreementPayment)
                    {
                        var sppay = pay.Type.Split('_');
                        if (sppay.Length > 0)
                        {
                            if (sppay[0] == "CC")
                            {
                                var cardNumber = pay.CardNumber.Replace('-', ' ').Replace(" ", "");
                                if (cardNumber.Length == 16)
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(12, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                                else
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(11, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                            }
                            else if (sppay[0] == "ACH" && pay.AcountNo.Length > 4)
                            {
                                paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Type: " + pay.BankAccountType + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Routing No: " + pay.RoutingNo + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account No: " + string.Concat("".PadLeft(pay.AcountNo.Length - 4, '*'), pay.AcountNo.Substring(pay.AcountNo.Length - 4)) + @"</td>
                                                        </tr>";
                            }
                        }
                    }
                }
                var CustomerBillingAddress = MakeAddress(Cus.StreetPrevious, Cus.CityPrevious, Cus.StatePrevious, Cus.ZipCodePrevious, "");
                var CustomerAddress = AddressHelper.MakeAddress(Cus);
                var CustomerInstallAddress = AddressHelper.MakeInstallAddress(Cus);
                CustomerSignature csA = new CustomerSignature();
                FileTemplate ft = new FileTemplate();
                CustomerAgreementTemplate cat = new CustomerAgreementTemplate();
                if (FileTemplateId.HasValue)
                {
                    cat = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(FileTemplateId.Value);
                }
                if (cat != null && cat.IsFileTemplate == true && cat.ReferenceTemplateId.HasValue)
                {
                    ft = _Util.Facade.FileFacade.GetFileTemplateById(cat.ReferenceTemplateId.Value);
                }
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (Cus != null && FileTemplateId != 0)
                {
                    csA = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, FileTemplateId.ToString(), "File Management");
                }
                else
                {
                    cusSignature = Cus.Singature;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        cussignDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = Cus.CustomerSignatureDate.Value.UTCToClientTime();
                    }

                }
                if (csA != null && FileTemplateId != 0)
                {
                    cusSignature = csA.Signature;
                    if (csA.CreatedDate != null && csA.CreatedDate != new DateTime())
                    {
                        cussignDate = csA.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = csA.CreatedDate.UTCToClientTime();
                    }

                }
                List<EmergencyContact> emrcyContacts = new List<EmergencyContact>();
                emrcyContacts = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId);
                List<CustomerAgreement> cusAgreements = new List<CustomerAgreement>();
                cusAgreements = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CompanyId, Cus.CustomerId);
                List<AgreementAnswer> agrAnsList = new List<AgreementAnswer>();
                agrAnsList = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId);
                CustomerAgreement cusAgr = new CustomerAgreement();
                cusAgr = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CompanyId, Cus.CustomerId);
                Double MMR = 1;
                Double CTerm = 0;

                double.TryParse(Cus.MonthlyMonitoringFee, out MMR);
                double.TryParse(ContractTerm, out CTerm);
                //(!string.IsNullOrWhiteSpace() ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm);
                Double TotalPayments = MMR * CTerm;

                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {

                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;

                            }


                        }

                    }

                }
                #endregion
                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    //InstallDate = Cus.InstallDate != null ? Convert.ToDateTime(Cus.InstallDate).ToShortDateString() : "",
                    InstallDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value.ToShortDateString() : "",
                    //OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    AccountType = Cus.Type,
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? _Util.Facade.CustomerFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    Owner2ndPhone = Cus.SecondaryPhone,
                    InitialStreet = Cus.Street,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.County,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    InitialApt = Cus.Appartment,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    BillingCountry = Cus.CountryPrevious,
                    BillingStreet = Cus.StreetPrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList != null ? SmartPackageEquipmentServiceList : new List<SmartPackageEquipmentService>(),
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails != null ? PaymentDetails : new PaymentInfo(),
                    DisplayName = Cus.DisplayName,
                    BillingAddress = CustomerBillingAddress,
                    OwnerAddress = CustomerAddress,
                    InstallAddress = CustomerInstallAddress,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.PrimaryPhone,
                    OwnerCellPhone = Cus.CellNo,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    EmergencyContactList = emrcyContacts != null ? emrcyContacts : new List<EmergencyContact>(),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    RenewalMonths = Cus.RenewalTerm.HasValue ? Cus.RenewalTerm.Value : 0,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId),
                    EquipmentList = CustomEquipmentList.ToList() != null ? CustomEquipmentList.ToList() : new List<Equipment>(),
                    ServiceList = CustomServiceList.ToList() != null ? CustomServiceList.ToList() : new List<Equipment>(),
                    ActivationFee = PackageCustomer != null ? PackageCustomer.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = cusSignature,
                    CustomerSignatureStringDate = cussignDate,
                    CustomerSignatureStringDateVal= cussignDateVal,
                    //ContractCreatedDateVal = contractCreatedDateVal,
                    ContractCreatedDateVal = (CusExd.ContractStartDate != null && CusExd.ContractStartDate.HasValue) ? CusExd.ContractStartDate.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                    CustomerAgreement = cusAgreements != null ? cusAgreements : new List<CustomerAgreement>(),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = agrAnsList != null ? agrAnsList : new List<AgreementAnswer>(),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = TotalPayments,
                    SingleCustomerAgreement = cusAgr != null ? cusAgr : new CustomerAgreement(),
                    ListContactEmergency = string.Format(overviewheader, overviewdata),
                    ListPaymentInfo = string.Format(paymentoverviewheader, paymentoverviewdata),
                    DoingBusinessAs = Cus.DBA,
                    DispalyName = Cus.DisplayName,
                    CompanyPhone = Com.Phone,
                    NotARBEnabledTotalPrice = NotARBEnabledTotalPrice,
                    NotARBEnabledServiceList = NotARBEnabledServiceList.ToList() != null ? NotARBEnabledServiceList.ToList() : new List<Equipment>(),
                    FinancedAmout = Cus != null && Cus.FinancedAmount != null ? Math.Round(Cus.FinancedAmount.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    MonthlyFinanceRate = CusExd != null && CusExd.MonthlyFinanceRate != null ? Math.Round(CusExd.MonthlyFinanceRate.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
                if (FileTemplateId != 0)
                {
                    if (csA != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(csA.Signature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (csA.CreatedDate != null && csA.CreatedDate != new DateTime())
                        {
                            Model.CompanySignatureDate = csA.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                    else if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        Model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }
                else
                {
                    if (Cus != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(Cus.Singature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                        {
                            Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                    else if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        Model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }

            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            }
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.FileFacade.MakeSmartAgreementPdf(Model, FileTemplateId);
            return body;
        }
        #endregion
        public ActionResult FileInstallation_v2(int? FileTemplateId, int? CustomerId, string UserId)
        {
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            //var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Guid CompanyId = new Guid();
            CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(CustomerId.Value);
            CompanyId = custommerCompany.CompanyId;
            if (CustomerId.HasValue)
            {
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);

                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = CustomerId;

            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CompanyId, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            //if (FileTemplateId.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(FileTemplateId.Value);
            //    ViewBag.FileTemplateId = FileTemplateId;
            //}
            if (FileTemplateId.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(FileTemplateId.Value);
                ViewBag.FileTemplateId = FileTemplateId;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(UserId));
                ViewBag.UserId = UserId;
            }
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (model.customerInfo != null && FileTemplateId.HasValue && FileTemplateId.Value > 0)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, FileTemplateId.ToString(), "File Management");
            }
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + CustomerId.ToString() + FileTemplateId.ToString() + "Signature";
            //string FileName = CustomerId.ToString() + FileTemplateId.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}

            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
            model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
            model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
            model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
            model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
            model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
            model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
            model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
            model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "Commercial Smart Agreement DFW" || model.cusAgreementTemplate.Name == "Residential Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(FileTemplateId, CustomerId);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(FileTemplateId, CustomerId);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                //if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                //{
                //    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                //    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                //}
                //if(model.cusAgreementTemplate.Name == "Finance Terms")
                //{
                //    model.OnitSmartHome= string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                //}
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            if (Request.Browser.IsMobileDevice)
            {
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + CustomerId + FileTemplateId + "File.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);

                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                var returnurl = AppConfig.SiteDomain + "/" + pdftempFolderName;
                return Redirect(returnurl);


                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //var pdfTempFold = string.Format(filename, saveComname);
                //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
                //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
                //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                //var returnurl = AppConfig.SiteDomain + "/" + pdftempFolderName;
                //return Redirect(returnurl);

                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                //Stream stream = new MemoryStream(applicationPDFData);

                //string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
                //string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

                //ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
                //string shortUrl = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl.Code);
                //return View();
            }
            else if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                return new ViewAsPdf()
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 2, 10, 3)
                };
            }
            else
            {
                return new ViewAsPdf()
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 2, 10, 3)
                };
            }
        }

        public ActionResult FileInstallation(int? FileTemplateId, int? CustomerId, string UserId)
        {
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            //var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Guid CompanyId = new Guid();
            CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(CustomerId.Value);
            CompanyId = custommerCompany.CompanyId;
            if (CustomerId.HasValue)
            {
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);

                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = CustomerId;

            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CompanyId, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            //if (FileTemplateId.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(FileTemplateId.Value);
            //    ViewBag.FileTemplateId = FileTemplateId;
            //}
            if (FileTemplateId.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(FileTemplateId.Value);
                ViewBag.FileTemplateId = FileTemplateId;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(UserId));
                ViewBag.UserId = UserId;
            }
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (model.customerInfo != null && FileTemplateId.HasValue && FileTemplateId.Value > 0)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, FileTemplateId.ToString(), "File Management");
            }
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + CustomerId.ToString() + FileTemplateId.ToString() + "Signature";
            //string FileName = CustomerId.ToString() + FileTemplateId.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}

            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
            model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
            model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
            model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
            model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
            model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
            model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
            model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
            model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "Commercial Smart Agreement DFW" || model.cusAgreementTemplate.Name == "Residential Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(FileTemplateId, CustomerId);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(FileTemplateId, CustomerId);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                //if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                //{
                //    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                //    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                //}
                //if(model.cusAgreementTemplate.Name == "Finance Terms")
                //{
                //    model.OnitSmartHome= string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                //}
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            if (Request.Browser.IsMobileDevice)
            {
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                #region File Save

                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + CustomerId + FileTemplateId + "File.pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);

                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                //var returnurl = AppConfig.SiteDomain + "/" + pdftempFolderName;
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                filename = filename.TrimEnd('/');
                var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var pdfTempFold = string.Format(filename, saveComname);
                var pdftempFolderName = CustomerId.ToString() + FileTemplateId.ToString();

                string FileName = rand.Next().ToString() + CustomerId.ToString() + FileTemplateId.ToString() + "_" + model.cusAgreementTemplate.Name + "_" + "File.pdf";
                string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
                string FilePath = cusfilePath;
                string FileKey = string.Format($"{FilePath}/{FileName}");
                var returnurl = "";

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = S3Domain;
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;
                

                #endregion

                //// "mayur" AWS S3 Changes //// End
               

                return Redirect(returnurl);


            }
            else if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                return new ViewAsPdf()
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 2, 10, 3)
                };
            }
            else
            {
                return new ViewAsPdf()
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 2, 10, 3)
                };
            }
        }


        #region Make address
        private string MakeAddress(string street, string city, string state, string zipcode, string country)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(street))
            {
                address += street;
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                if (city != "-1")
                {
                    if (!string.IsNullOrWhiteSpace(street))
                    {
                        address += ", " + city;
                    }
                    else
                    {
                        address += city;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                if (state != "-1")
                {
                    if (!string.IsNullOrWhiteSpace(street) || !string.IsNullOrWhiteSpace(city))
                    {
                        address += ", " + state;
                    }
                    else
                    {
                        address += state;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(zipcode))
            {
                if (!string.IsNullOrWhiteSpace(street) || !string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(state))
                {
                    address += ", " + zipcode;
                }
                else
                {
                    address += zipcode;
                }
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                if (!string.IsNullOrWhiteSpace(street) || !string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(state) || !string.IsNullOrWhiteSpace(zipcode))
                {
                    address += ", " + country;
                }
                else
                {
                    address += country;
                }
            }
            return address.TrimEnd(',');
        }
        #endregion
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> FileToCustomerPDFMail_v2(int? cusid, string PrefferedEmail, int? fileid)
        {
            bool result = false;
            string message = "";
            try
            {
                #region Checking email adress
                List<string> ValidPrefferedEmail = new List<string>();
                if (!string.IsNullOrWhiteSpace(PrefferedEmail))
                {
                    string[] Emailadd = PrefferedEmail.Split(';');
                    if (Emailadd != null)
                    {
                        foreach (var item in Emailadd)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                ValidPrefferedEmail.Add(item);
                            }
                        }
                    }
                    if (ValidPrefferedEmail.Count == 0)
                    {
                        return Json(new { result = false, message = "Invalid email address." });
                    }

                }
                #endregion



                FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
                FileTemplate ft = new FileTemplate();
                var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                if (cusid.HasValue)
                {
                    //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                    model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                    model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                    ViewBag.CustomerId = cusid;
                }
                int term = 0;
                double contract;
                string conterm = "";
                string contermVal = "";
                string from = "";
                if (model.customerInfo != null)
                {
                    bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                    if (success)
                    {
                        term = Convert.ToInt32(Math.Round(contract * 12));
                        ViewBag.termid = term;
                        if (term > 1)
                        {
                            ViewBag.TermMonth = " month";
                        }
                        else
                        {
                            ViewBag.TermMonth = " month";
                        }
                    }
                    conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                    contermVal = string.Concat(ViewBag.termid, " ");
                    model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.customerInfo.CustomerId);
                }
                model.ContractTeam = conterm;
                model.ContractTeamVal = contermVal;
                if (CurrentLoggedInUser.UserId != null)
                {
                    model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                    ViewBag.UserId = CurrentLoggedInUser.UserId;
                }
                //if (fileid.HasValue)
                //{
                //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
                //    ViewBag.FileTemplateId = fileid;
                //}
                if (fileid.HasValue)
                {
                    model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                    ViewBag.FileTemplateId = fileid;
                }
                if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
                {
                    ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
                }
                #region Signature
                //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
                //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
                //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
                //var fullFilePath = Server.MapPath(filePath);
                //if (System.IO.File.Exists(fullFilePath))
                //{
                //    model.FileManagementCustomerSignature = fullFilePath;

                //}
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CompanySignature");
                GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AuthorizedRepresentative");
                model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
                if (model.customerInfo != null && fileid.HasValue)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
                }

                if (cs != null)
                {
                    model.FileManagementCustomerSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                    }
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }

                    }
                    else if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }
                else
                {
                    if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }
                #endregion

                #region PDF
                model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
                model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
                //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
                string body = "";
                if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
                {
                    body = GetSmartAgreementFileContent(fileid, cusid);
                }
                else if (model.cusAgreementTemplate.Name == "Customer Addendum")
                {
                    body = GetCustomerAddendum(fileid, cusid);
                }
                else
                {
                    if (model.cusAgreementTemplate.Name == "Cancellation")
                    {
                        CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                        ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                        if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                        {
                            model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                        }
                        else
                        {
                            model.CancellationDate = "TERMDATE";
                        }
                        if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                        {
                            model.RemainingBalance = (float)ccq.RemainingBalance;
                        }
                        else
                        {
                            model.RemainingBalance = 0;
                        }
                    }
                    if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                    {
                        model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                        model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                    }
                    if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                    {
                        model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                        model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                        model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                        model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                        model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                    }
                    if (model.cusAgreementTemplate.Name == "Finance Terms")
                    {
                        model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                    }
                    if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                    {
                        model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                    }
                    body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
                }
                ViewBag.Body = body;
                ViewAsPdf actionPDF;
                if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
                {
                    actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                    {
                        PageSize = Rotativa.Options.Size.Legal,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                else
                {
                    actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                    {
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                //{
                //    PageSize = Rotativa.Options.Size.A4,
                //    PageOrientation = Rotativa.Options.Orientation.Portrait,
                //    PageMargins = { Left = 1, Right = 1 },

                //};
                string EmailAddress = PrefferedEmail;
                if (ValidPrefferedEmail.Count == 0)
                {
                    EmailAddress = model.customerInfo.EmailAddress;
                }
                else
                {
                    EmailAddress = string.Join(";", ValidPrefferedEmail);
                }

                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                ViewBag.appPdfData = applicationPDFData;
                Stream stream = new MemoryStream(applicationPDFData);
                #endregion

                message = "";
                result = false;
                var returnurl = "";
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
                string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);

                #region File Save
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var pdfTempFold = string.Format(filename, saveComname);
                //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
                //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
                //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);

                // FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                filename = filename.TrimEnd('/');
                var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                var pdfTempFold = string.Format(filename, saveComname);
                var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";

                string FileName = cusid.ToString() + fileid.ToString() + "_" + model.cusAgreementTemplate.Name.Replace(" ", "") + "_" + "File.pdf";
                string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
                string FilePath = cusfilePath;
                string FileKey = string.Format($"{FilePath}/{FileName}");


                await new AWSS3ObjectService().UploadFile(FileKey, applicationPDFData).ConfigureAwait(false);
                await new AWSS3ObjectService().MakePublic(FileName, FilePath).ConfigureAwait(false);


                returnurl = S3Domain;
                returnurl = returnurl + FileKey;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.filename = FileName;
                ViewBag.filename = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End



                #region File Management Body contect
                bool IsFileWithoutCustomerSign = false;
                //string bodyContent = "";
                //if(ft!= null)
                //{

                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    //bodyContent = "Please review the attached document.";
                    IsFileWithoutCustomerSign = true;
                }
                //    else
                //    {
                //        bodyContent = "Please click the link below to review and sign the file!";
                //        bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //    }
                //}
                //else
                //{
                //    bodyContent = "Please click the link below to review and sign the file!";
                //    bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //}
                #endregion

                if (model.customerInfo != null)
                {

                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(model.customerInfo.EmailAddress) && model.customerInfo.EmailAddress.IsValidEmailAddress()))
                    {
                        result = true;
                        string customerFullName = model.customerInfo.FirstName + " " + model.customerInfo.LastName;

                        FileManagement fm = new FileManagement
                        {
                            CustomerNum = customerFullName,
                            ToEmail = EmailAddress,
                            CompanyName = CurrentLoggedInUser.CompanyName,
                            BodyLink = shortUrl,
                            IsFileWithoutCustomerSign = IsFileWithoutCustomerSign,
                            //Subject = string.Format("REVIEW REQUIRED: See attached document from {0} at {1}", CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.CompanyName),
                            //Body = bodyContent,
                            CustomerId = model.customerInfo.CustomerId.ToString(),
                            EmployeeId = CurrentLoggedInUser.UserId.ToString(),
                            fileManagementpdf = ft != null && ft.IsCustomerSignRequired == false ? new System.Net.Mail.Attachment(stream, model.customerInfo.Id + ".pdf") : null
                        };

                        if (model != null && model.cusAgreementTemplate.Name != null)
                        {
                            from = model.cusAgreementTemplate.Name;

                        }

                        result = _Util.Facade.MailFacade.EmailFileManagement(fm, CurrentLoggedInUser.CompanyId.Value, from);
                        if (result == false)
                        {
                            message = "Email send failed.";
                        }
                        else
                        {
                            message = "File sent to " + EmailAddress;
                        }
                    }
                    else
                    {
                        result = false;
                        message = "Invalid email address.";
                    }
                }

                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(model.customerInfo.DBA))
                {
                    Cusname = model.customerInfo.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(model.customerInfo.BusinessName))
                {
                    Cusname = model.customerInfo.BusinessName;
                }
                else
                {
                    Cusname = model.customerInfo.FirstName + ' ' + model.customerInfo.LastName;
                }
                Hashtable TemplateValue = new Hashtable();
                #region Common Templates
                var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
                var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
                var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
                var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
                var FacebookLink = _Util.Facade.MailFacade.GetFacebookUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var InstagramLink = _Util.Facade.MailFacade.GetInstagramUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var YoutubeLink = _Util.Facade.MailFacade.GetYoutubeUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                if (!string.IsNullOrEmpty(FacebookLink))
                {
                    FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                    TemplateValue.Add("FacebookDiv", FacebookTemplate);
                }
                if (!string.IsNullOrEmpty(InstagramLink))
                {
                    InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                    TemplateValue.Add("InstagramDiv", InstagramTemplate);
                }
                if (!string.IsNullOrEmpty(YoutubeLink))
                {
                    YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                    TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
                }

                if (TemplateValue["Logo"] == null)
                {
                    TemplateValue.Add("Logo", _Util.Facade.MailFacade.GetEmailLogoByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CustomerNum"] == null)
                {
                    TemplateValue.Add("CustomerNum", Cusname);
                }
                if (TemplateValue["TeamNameSignature"] == null)
                {
                    TemplateValue.Add("TeamNameSignature", _Util.Facade.MailFacade.GetTeamNameSignatureByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyNameAlt"] == null)
                {
                    TemplateValue.Add("CompanyNameAlt", _Util.Facade.MailFacade.GetCompanyNameByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyInformation"] == null)
                {
                    string Footer = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                    if (Footer.IndexOf("##Year##") > -1)
                    {
                        Footer = Footer.Replace("##Year##", "2017-" + DateTime.Now.Year.ToString());
                    }
                    TemplateValue.Add("CompanyInformation", Footer);
                }
                #endregion
                string TemplateKey = "EmailtoLeadsAggrement";
                EmailTemplate emailTemplate = _Util.Facade.EmailTextTemplateFacade.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
                EmailParser parser = null;
                string toEmailAddress = "";
                string FromName = "";
                bool res = false;
                #region BodyFile And BodyContent
                if (emailTemplate == null || emailTemplate.Id == 0)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                    // return res;
                }
                if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
                {
                    parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
                }
                else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
                {
                    parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
                }
                #endregion
                MailMessage message1 = new MailMessage();
                message1.Body = parser.Parse();

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                // //string AWSS3Url = ConfigurationManager.AppSettings["AWS.S3Url"];
                // // string AWSS3BucketName = ConfigurationManager.AppSettings["AWS.S3BucketName"];

                // filename = cusid.ToString() + fileid.ToString() + "_" + model.cusAgreementTemplate.Name + "_" + "File.pdf";

                // cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName);
                // FilePath = cusfilePath;
                // FileKey = string.Format($"{FilePath}/{filename}");



                ////await new AWSS3ObjectService().UploadFile(FileKey,applicationPDFData).ConfigureAwait(false);
                //// await new AWSS3ObjectService().MakePublic(filename, FilePath).ConfigureAwait(false);

               
                // returnurl = S3Domain;
                // returnurl = returnurl + FileKey;

                // ViewBag.ReturnUrl = returnurl;

                #endregion
                //// "mayur" AWS S3 Changes //// End

                #region file save to customer file
                if (result)
                {
                    CustomerFile cfs = new CustomerFile()
                    {
                        // FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        // Filename = "/" + Filekey,
                        FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        Filename = "/" + ViewBag.FileKey,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = model.customerInfo.CustomerId,
                        CompanyId = model.Company.CompanyId,
                        IsActive = true,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentLoggedInUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);



                    #endregion


                }
            }
          
            catch (Exception  e)
            {
                logger.Error(e);
            }
            return Json(new { result = result, message = message });

        }

        public JsonResult FileToCustomerPDFMail(int? cusid, string PrefferedEmail, int? fileid)
        {
            bool result = false;
            string message = "";
            try
            {
                #region Checking email adress
                List<string> ValidPrefferedEmail = new List<string>();
                if (!string.IsNullOrWhiteSpace(PrefferedEmail))
                {
                    string[] Emailadd = PrefferedEmail.Split(';');
                    if (Emailadd != null)
                    {
                        foreach (var item in Emailadd)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                ValidPrefferedEmail.Add(item);
                            }
                        }
                    }
                    if (ValidPrefferedEmail.Count == 0)
                    {
                        return Json(new { result = false, message = "Invalid email address." });
                    }

                }
                #endregion



                FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
                FileTemplate ft = new FileTemplate();
                var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                if (cusid.HasValue)
                {
                    //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                    model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                    model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                    ViewBag.CustomerId = cusid;
                }
                int term = 0;
                double contract;
                string conterm = "";
                string contermVal = "";
                string from = "";
                if (model.customerInfo != null)
                {
                    bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                    if (success)
                    {
                        term = Convert.ToInt32(Math.Round(contract * 12));
                        ViewBag.termid = term;
                        if (term > 1)
                        {
                            ViewBag.TermMonth = " month";
                        }
                        else
                        {
                            ViewBag.TermMonth = " month";
                        }
                    }
                    conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                    contermVal = string.Concat(ViewBag.termid, " ");
                    model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.customerInfo.CustomerId);
                }
                model.ContractTeam = conterm;
                model.ContractTeamVal = contermVal;
                if (CurrentLoggedInUser.UserId != null)
                {
                    model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                    ViewBag.UserId = CurrentLoggedInUser.UserId;
                }
                //if (fileid.HasValue)
                //{
                //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
                //    ViewBag.FileTemplateId = fileid;
                //}
                if (fileid.HasValue)
                {
                    model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                    ViewBag.FileTemplateId = fileid;
                }
                if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
                {
                    ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
                }
                #region Signature
                //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
                //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
                //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
                //var fullFilePath = Server.MapPath(filePath);
                //if (System.IO.File.Exists(fullFilePath))
                //{
                //    model.FileManagementCustomerSignature = fullFilePath;

                //}
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CompanySignature");
                GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AuthorizedRepresentative");
                model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
                if (model.customerInfo != null && fileid.HasValue)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
                }

                if (cs != null)
                {
                    model.FileManagementCustomerSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                    }
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }

                    }
                    else if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }
                else
                {
                    if (ft != null && ft.IsCustomerSignRequired == false)
                    {
                        model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                    }
                }
                #endregion

                #region PDF
                model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
                model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
                //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
                string body = "";
                if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
                {
                    body = GetSmartAgreementFileContent(fileid, cusid);
                }
                else if (model.cusAgreementTemplate.Name == "Customer Addendum")
                {
                    body = GetCustomerAddendum(fileid, cusid);
                }
                else
                {
                    if (model.cusAgreementTemplate.Name == "Cancellation")
                    {
                        CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                        ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                        if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                        {
                            model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                        }
                        else
                        {
                            model.CancellationDate = "TERMDATE";
                        }
                        if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                        {
                            model.RemainingBalance = (float)ccq.RemainingBalance;
                        }
                        else
                        {
                            model.RemainingBalance = 0;
                        }
                    }
                    if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                    {
                        model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                        model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                    }
                    if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                    {
                        model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                        model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                        model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                        model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                        model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                    }
                    if (model.cusAgreementTemplate.Name == "Finance Terms")
                    {
                        model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                    }
                    if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                    {
                        model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                    }
                    body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
                }
                ViewBag.Body = body;
                ViewAsPdf actionPDF;
                if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
                {
                    actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                    {
                        PageSize = Rotativa.Options.Size.Legal,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                else
                {
                    actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                    {
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                //{
                //    PageSize = Rotativa.Options.Size.A4,
                //    PageOrientation = Rotativa.Options.Orientation.Portrait,
                //    PageMargins = { Left = 1, Right = 1 },

                //};
                string EmailAddress = PrefferedEmail;
                if (ValidPrefferedEmail.Count == 0)
                {
                    EmailAddress = model.customerInfo.EmailAddress;
                }
                else
                {
                    EmailAddress = string.Join(";", ValidPrefferedEmail);
                }

                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                ViewBag.appPdfData = applicationPDFData;
                Stream stream = new MemoryStream(applicationPDFData);
                #endregion

                message = "";
                result = false;
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
                string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);

                #region File Save
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var pdfTempFold = string.Format(filename, saveComname);
                //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
                //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
                //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);

                // FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                filename = filename.TrimEnd('/');

                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();

                var pdfTempFold = string.Format(filename, comname);
                var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";

               
                string FileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";

                string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
                string FilePath = cusfilePath;

                string FileKey = string.Format($"{FilePath}/{FileName}");
                var returnurl = "";

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () =>  {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(FileName, FilePath);
                 
                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = S3Domain;
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.Filekey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End



                // JsonEmail_result = await FileToCustomerPDFMail_Part2(applicationPDFData, shortUrl, EmailAddress, ValidPrefferedEmail, cusid, PrefferedEmail, fileid, model, ft, returnurl, FileName) ;

                #region File Management Body contect
                bool IsFileWithoutCustomerSign = false;
                //string bodyContent = "";
                //if(ft!= null)
                //{

                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    //bodyContent = "Please review the attached document.";
                    IsFileWithoutCustomerSign = true;
                }
                //    else
                //    {
                //        bodyContent = "Please click the link below to review and sign the file!";
                //        bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //    }
                //}
                //else
                //{
                //    bodyContent = "Please click the link below to review and sign the file!";
                //    bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //}
                #endregion

                if (model.customerInfo != null)
                {

                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(model.customerInfo.EmailAddress) && model.customerInfo.EmailAddress.IsValidEmailAddress()))
                    {
                        result = true;
                        string customerFullName = model.customerInfo.FirstName + " " + model.customerInfo.LastName;

                        FileManagement fm = new FileManagement
                        {
                            CustomerNum = customerFullName,
                            ToEmail = EmailAddress,
                            CompanyName = CurrentLoggedInUser.CompanyName,
                            BodyLink = shortUrl,
                            IsFileWithoutCustomerSign = IsFileWithoutCustomerSign,
                            //Subject = string.Format("REVIEW REQUIRED: See attached document from {0} at {1}", CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.CompanyName),
                            //Body = bodyContent,
                            CustomerId = model.customerInfo.CustomerId.ToString(),
                            EmployeeId = CurrentLoggedInUser.UserId.ToString(),
                            fileManagementpdf = ft != null && ft.IsCustomerSignRequired == false ? new System.Net.Mail.Attachment(stream, model.customerInfo.Id + ".pdf") : null
                        };

                        if (model != null && model.cusAgreementTemplate.Name != null)
                        {
                            from = model.cusAgreementTemplate.Name;

                        }


                        result = _Util.Facade.MailFacade.EmailFileManagement(fm, CurrentLoggedInUser.CompanyId.Value, from);



                        if (result == false)
                        {
                            message = "Email send failed.";
                        }
                        else
                        {
                            result = true;
                            message = "File sent to " + EmailAddress;
                        }
                    }
                    else
                    {
                        result = false;
                        message = "Invalid email address.";
                    }
                }

                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(model.customerInfo.DBA))
                {
                    Cusname = model.customerInfo.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(model.customerInfo.BusinessName))
                {
                    Cusname = model.customerInfo.BusinessName;
                }
                else
                {
                    Cusname = model.customerInfo.FirstName + ' ' + model.customerInfo.LastName;
                }
                Hashtable TemplateValue = new Hashtable();
                #region Common Templates
                var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
                var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
                var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
                var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
                var FacebookLink = _Util.Facade.MailFacade.GetFacebookUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var InstagramLink = _Util.Facade.MailFacade.GetInstagramUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var YoutubeLink = _Util.Facade.MailFacade.GetYoutubeUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                if (!string.IsNullOrEmpty(FacebookLink))
                {
                    FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                    TemplateValue.Add("FacebookDiv", FacebookTemplate);
                }
                if (!string.IsNullOrEmpty(InstagramLink))
                {
                    InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                    TemplateValue.Add("InstagramDiv", InstagramTemplate);
                }
                if (!string.IsNullOrEmpty(YoutubeLink))
                {
                    YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                    TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
                }

                if (TemplateValue["Logo"] == null)
                {
                    TemplateValue.Add("Logo", _Util.Facade.MailFacade.GetEmailLogoByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CustomerNum"] == null)
                {
                    TemplateValue.Add("CustomerNum", Cusname);
                }
                if (TemplateValue["TeamNameSignature"] == null)
                {
                    TemplateValue.Add("TeamNameSignature", _Util.Facade.MailFacade.GetTeamNameSignatureByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyNameAlt"] == null)
                {
                    TemplateValue.Add("CompanyNameAlt", _Util.Facade.MailFacade.GetCompanyNameByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyInformation"] == null)
                {
                    string Footer = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                    if (Footer.IndexOf("##Year##") > -1)
                    {
                        Footer = Footer.Replace("##Year##", "2017-" + DateTime.Now.Year.ToString());
                    }
                    TemplateValue.Add("CompanyInformation", Footer);
                }
                #endregion
                string TemplateKey = "EmailtoLeadsAggrement";
                EmailTemplate emailTemplate = _Util.Facade.EmailTextTemplateFacade.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
                EmailParser parser = null;
                string toEmailAddress = "";
                string FromName = "";
                bool res = false;
                #region BodyFile And BodyContent
                if (emailTemplate == null || emailTemplate.Id == 0)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                    // return res;
                }
                if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
                {
                    parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
                }
                else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
                {
                    parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
                }
                #endregion
                MailMessage message1 = new MailMessage();
                message1.Body = parser.Parse();


                //// ""Mayur" Calculate File Size : start
                #region Calculate file size

                var fileSize = (decimal)applicationPDFData.Length / 1024;
                fileSize = Math.Round(fileSize, 2, MidpointRounding.AwayFromZero);

                #endregion
                //// ""Mayur" Calculate File Size : End

                #region file save to customer file
                if (result)
                {
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        Filename = "/" + ViewBag.FileKey,
                        FileSize = (double)fileSize,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = model.customerInfo.CustomerId,
                        CompanyId = model.Company.CompanyId,
                        IsActive = true,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentLoggedInUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);



                    #endregion


                }

            }

            catch (Exception e)
            {
                logger.Error(e);
            }
            return Json(new { result = result, message = message }); ;

        }

       
        public async Task<JsonResult> FileToCustomerPDFMail_Part2(byte[] applicationPDFData,string shortUrl,string EmailAddress, List<string> ValidPrefferedEmail, int? cusid, string PrefferedEmail, int? fileid,FileTemplateWithCustomerInfo model,FileTemplate ft,string returnUrl,string returnfilename)
        {

            Stream stream = new MemoryStream(applicationPDFData);
            string from = "";
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
          
            bool result = false;
            string message = "";
            try
            {

                #region File Management Body contect
                bool IsFileWithoutCustomerSign = false;
                //string bodyContent = "";
                //if(ft!= null)
                //{

                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    //bodyContent = "Please review the attached document.";
                       IsFileWithoutCustomerSign = true;
                }
                //    else
                //    {
                //        bodyContent = "Please click the link below to review and sign the file!";
                //        bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //    }
                //}
                //else
                //{
                //    bodyContent = "Please click the link below to review and sign the file!";
                //    bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
                //}
                #endregion

                if (model.customerInfo != null)
                {

                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(model.customerInfo.EmailAddress) && model.customerInfo.EmailAddress.IsValidEmailAddress()))
                    {
                        result = true;
                        string customerFullName = model.customerInfo.FirstName + " " + model.customerInfo.LastName;

                        FileManagement fm = new FileManagement
                        {
                            CustomerNum = customerFullName,
                            ToEmail = EmailAddress,
                            CompanyName = CurrentLoggedInUser.CompanyName,
                            BodyLink = shortUrl,
                            IsFileWithoutCustomerSign = IsFileWithoutCustomerSign,
                            //Subject = string.Format("REVIEW REQUIRED: See attached document from {0} at {1}", CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.CompanyName),
                            //Body = bodyContent,
                            CustomerId = model.customerInfo.CustomerId.ToString(),
                            EmployeeId = CurrentLoggedInUser.UserId.ToString(),
                            fileManagementpdf = ft != null && ft.IsCustomerSignRequired == false ? new System.Net.Mail.Attachment(stream, model.customerInfo.Id + ".pdf") : null
                        };

                        if (model != null && model.cusAgreementTemplate.Name != null)
                        {
                            from = model.cusAgreementTemplate.Name;

                        }

                        
                        result = _Util.Facade.MailFacade.EmailFileManagement(fm, CurrentLoggedInUser.CompanyId.Value, from);

                     

                        if (result == false)
                        {
                            message = "Email send failed.";
                        }
                        else
                        {
                            message = "File sent to " + EmailAddress;
                        }
                    }
                    else
                    {
                        result = false;
                        message = "Invalid email address.";
                    }
                }

                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(model.customerInfo.DBA))
                {
                    Cusname = model.customerInfo.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(model.customerInfo.BusinessName))
                {
                    Cusname = model.customerInfo.BusinessName;
                }
                else
                {
                    Cusname = model.customerInfo.FirstName + ' ' + model.customerInfo.LastName;
                }
                Hashtable TemplateValue = new Hashtable();
                #region Common Templates
                var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
                var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
                var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
                var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
                var FacebookLink = _Util.Facade.MailFacade.GetFacebookUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var InstagramLink = _Util.Facade.MailFacade.GetInstagramUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                var YoutubeLink = _Util.Facade.MailFacade.GetYoutubeUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                if (!string.IsNullOrEmpty(FacebookLink))
                {
                    FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                    TemplateValue.Add("FacebookDiv", FacebookTemplate);
                }
                if (!string.IsNullOrEmpty(InstagramLink))
                {
                    InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                    TemplateValue.Add("InstagramDiv", InstagramTemplate);
                }
                if (!string.IsNullOrEmpty(YoutubeLink))
                {
                    YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                    TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
                }

                if (TemplateValue["Logo"] == null)
                {
                    TemplateValue.Add("Logo", _Util.Facade.MailFacade.GetEmailLogoByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CustomerNum"] == null)
                {
                    TemplateValue.Add("CustomerNum", Cusname);
                }
                if (TemplateValue["TeamNameSignature"] == null)
                {
                    TemplateValue.Add("TeamNameSignature", _Util.Facade.MailFacade.GetTeamNameSignatureByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyNameAlt"] == null)
                {
                    TemplateValue.Add("CompanyNameAlt", _Util.Facade.MailFacade.GetCompanyNameByCompanyId(CurrentLoggedInUser.CompanyId.Value));
                }
                if (TemplateValue["CompanyInformation"] == null)
                {
                    string Footer = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                    if (Footer.IndexOf("##Year##") > -1)
                    {
                        Footer = Footer.Replace("##Year##", "2017-" + DateTime.Now.Year.ToString());
                    }
                    TemplateValue.Add("CompanyInformation", Footer);
                }
                #endregion
                string TemplateKey = "EmailtoLeadsAggrement";
                EmailTemplate emailTemplate = _Util.Facade.EmailTextTemplateFacade.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
                EmailParser parser = null;
                string toEmailAddress = "";
                string FromName = "";
                bool res = false;
                #region BodyFile And BodyContent
                if (emailTemplate == null || emailTemplate.Id == 0)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                    // return res;
                }
                if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
                {
                    parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
                }
                else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
                {
                    parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
                }
                #endregion
                MailMessage message1 = new MailMessage();
                message1.Body = parser.Parse();


                //// ""Mayur" Calculate File Size : start
                #region Calculate file size

                var _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                #endregion
                //// ""Mayur" Calculate File Size : End

                #region file save to customer file
                if (result)
                {
                    CustomerFile cfs = new CustomerFile()
                    {
                        // FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        // Filename = AppConfig.DomainSitePath + cusfilePath,
                        FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        Filename = "/" + ViewBag.FileKey,
                        FileSize = (double)_fileSize,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = model.customerInfo.CustomerId,
                        CompanyId = model.Company.CompanyId,
                        IsActive = true,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentLoggedInUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);



                    #endregion


                }

            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            return Json(new { result = result, message = message });


        }

            public void SendCancellationAgreement(Guid CustomerId)
            {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId); ;
            List<CustomerCancellationQueue> cencellationList = _Util.Facade.CustomerFacade.GetActiveCustomerQueueCancellationByCustomerId(CustomerId);
            if (cencellationList.Count > 0)
            {

                cus.CancellationSignature = "";
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                // base.AddUserActivityForCustomer("Customer Information Updated", LabelHelper.ActivityAction.AddCustomer, cus.CustomerId, cus.Id);
                foreach (var item in cencellationList)
                {
                    item.IsActive = false;
                    item.IsSigned = false;
                    _Util.Facade.CustomerFacade.UpdateCustomerCancellationQueue(item);
                }

            }
            try
            {

                CustomerCancellationQueue Model = new CustomerCancellationQueue()
                {
                    CancellationId = Guid.NewGuid(),
                    CustomerId = CustomerId,
                    CreatedBy = CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                    CancellationDate = DateTime.Now,
                    RemainingBalance = 0,
                    Reason = "",
                    IsSigned = false,
                    IsActive = true

                };
                _Util.Facade.CustomerFacade.InsertCustomerCancellationQueue(Model);
                string BodyContent = @"<div style='width: 100%; float: left; box-sizing: border-box; padding: 0px 100px; font-size: 12px; font-family: Verdana;'>
                                    <table style='border-collapse: collapse; width: 100%; float: left; table-layout: fixed;'>
                                    <tbody>
                                    <tr>
                                    <td style='width: 32%; text-align: center;' valign='top'><img style='width: 170px; height: 110px;' src='##CompanyLogo##' alt='Company Logo' /></td>
                                    </tr>
                                    <tr>
                                    <td style='font-size: 20px; box-sizing: border-box; text-align: center; padding-top: 20px;'>2533 E Loop 820 North, Fort Worth, TX 76118 <br />Ph: 877-372-0350 Fax: 817-281-0113 <br />service@dfwsecurity.com <br />Texas License B08964</td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left;'>
                                    <div style='padding: 20px 0px;'>##GenerationDate##</div>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left;'>This letter is to serve as written notification to DFW Security that ##FirstName## ##LastName## wishes to terminate service at ##InstallAddress## at the end of term on ##TermDate##. <br /><br />I understand that the balance remaining on the contract is <br />$##RemainingBalance## and must be paid in full by ##TermDate## in order for the <br />contract to be cancelled.</td>
                                    </tr>
                                    <tr>
                                    <td style='font-size: 20px; font-weight: bold; box-sizing: border-box; text-align: left; padding: 20px 0px;'>Please Select the Reason for cancellation:</td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left;'><input type='checkbox' /> &nbsp;Customer service<br /><br /><input type='checkbox' /> &nbsp;Alarm performance<br /><br /><input type='checkbox' /> &nbsp;Cost of service too high<br /><br /><input type='checkbox' /> &nbsp;Sold business<br /><br /><input type='checkbox' /> &nbsp;Moving outside of DFW area<br /><br /><input checked='checked' type='checkbox' /> &nbsp;Moving inside DFW area<br /><br /><input type='checkbox' /> &nbsp;Going with another provider<br /><br /><input type='checkbox' /> &nbsp; Do not use system</td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left; padding: 40px 0px; font-size: 20px; font-style: italic; font-family: Tahoma;'>DFW Security is always looking for ways to improve the customer experience. Please list any additional comments below:</td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left; padding: 0px 0px 40px; font-size: 20px; font-family: Tahoma;'>&nbsp;</td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left;'>Please electronically sign and return this document or print and mail to address above. Cancellation will not be processed without signed request from account holder.</td>
                                    </tr>
                                    <tr>
                                    <td style='padding-top: 30px;'><img style='width: 170px; height: 110px;' src='##CustomerSignature##' alt='' /></td>
                                    </tr>
                                    <tr>
                                    <td style='box-sizing: border-box; text-align: left; padding: 20px 0px; padding-top: 10px;'><span style='border-top: 1px solid #000;'>Please sign here</span></td>
                                    </tr>
                                    </tbody>
                                    </table>
                                    </div>";
                CustomerAgreementTemplate tempet = new CustomerAgreementTemplate();
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.ReferenceTemplateId = 35;
                tempet.CustomerId = CustomerId;
                tempet.Name = "Cancellation";
                tempet.IsFileTemplate = true;
                tempet.BodyContent = BodyContent;
                tempet.Description = "Cancellation";
                tempet.CreatedBy = CurrentUser.UserId;
                tempet.CreatedDate = DateTime.Now.UTCCurrentTime();
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                long temId = _Util.Facade.FileFacade.InsertCustomerAgreementTemplate(tempet);
                int? fileId = Convert.ToInt32(temId);
                FileToCustomerPDFMail(cus.Id, cus.EmailAddress, fileId);

               
            }


            catch (Exception ex)
            {

            }



        }
        public JsonResult FileToCustomerPDFMailAndSMS_v2_old(int? cusid, string PrefferedEmail, string PrefferedNO, int? fileid)
        {
            #region Send Mail
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                string[] Emailadd = PrefferedEmail.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (cusid.HasValue)
            {
                //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = cusid;
            }
            if (CurrentLoggedInUser.UserId != null)
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                ViewBag.UserId = CurrentLoggedInUser.UserId;
            }
            //if (fileid.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
            //    ViewBag.FileTemplateId = fileid;
            //}
            if (fileid.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                ViewBag.FileTemplateId = fileid;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
            //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (model.customerInfo != null && fileid.HasValue)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            string from = "";

            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(fileid, cusid);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(fileid, cusid);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                }
                if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                    model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                    model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                    model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                    model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                }
                if (model.cusAgreementTemplate.Name == "Finance Terms")
                {
                    model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                }
                if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                }
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            ViewAsPdf actionPDF;
            if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            else
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
            //{
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },

            //};
            string EmailAddress = PrefferedEmail;
            if (ValidPrefferedEmail.Count == 0)
            {
                EmailAddress = model.customerInfo.EmailAddress;
            }
            else
            {
                EmailAddress = string.Join(";", ValidPrefferedEmail);
            }

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);

            string message = "";
            bool result = false;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            string ReceiverNumber = "";
            if(model != null && model.cusAgreementTemplate != null)
            {
                from = model.cusAgreementTemplate.Name;
            }
            #region File Save
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
            var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            var pdfTempFold = string.Format(filename, saveComname);
            var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
            string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
            string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
            string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion

            #region File Management Body contect
            SMSFile smsFile = new SMSFile();
            bool IsFileWithoutCustomerSign = false;
            //string bodyContent = "";
            //if (ft != null)
            //{
            if (ft != null && ft.IsCustomerSignRequired == false)
            {
                IsFileWithoutCustomerSign = true;
                //bodyContent = "Please review the attached document.";
                //smsFile.ShortUrl = "Please review the attached file from the link. " + shortUrl;
            }
            smsFile.ShortUrl = shortUrl;
            smsFile.IsFileWithoutCustomerSign = IsFileWithoutCustomerSign;
            smsFile.CompanyName = CurrentLoggedInUser.CompanyName;
            //    else
            //    {
            //        //bodyContent = "Please click the link below to review and sign the file!";
            //        //bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
            //        smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //    }
            //}
            //else
            //{
            //    //bodyContent = "Please click the link below to review and sign the file!";
            //    //bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
            //    smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //}
            #endregion

            if (model.customerInfo != null)
            {

                if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(model.customerInfo.EmailAddress) && model.customerInfo.EmailAddress.IsValidEmailAddress()))
                {
                    result = true;
                    FileManagement fm = new FileManagement
                    {
                        CustomerNum = model.customerInfo.FirstName + " " + model.customerInfo.LastName,
                        ToEmail = EmailAddress,
                        CompanyName = CurrentLoggedInUser.CompanyName,
                        BodyLink = shortUrl,
                        IsFileWithoutCustomerSign = IsFileWithoutCustomerSign,
                        //Subject = string.Format("REVIEW REQUIRED: See attached document from {0} at {1}", CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.CompanyName),
                        //Body = bodyContent,
                        CustomerId = model.customerInfo.CustomerId.ToString(),
                        EmployeeId = CurrentLoggedInUser.UserId.ToString(),
                        fileManagementpdf = ft != null && ft.IsCustomerSignRequired == false ? new System.Net.Mail.Attachment(stream, model.customerInfo.Id + ".pdf") : null
                    };
                    result = _Util.Facade.MailFacade.EmailFileManagement(fm, CurrentLoggedInUser.CompanyId.Value,from);
                    if(result)
                    {
                        #region file save to customer file
                        //if (result)
                        //{
                        //    CustomerFile cfs = new CustomerFile()
                        //    {
                        //        FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail" + ".pdf",
                        //        Filename = AppConfig.DomainSitePath + cusfilePath,
                        //        FileId = Guid.NewGuid(),
                        //        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        //        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        //        CustomerId = model.customerInfo.CustomerId,
                        //        CompanyId = model.Company.CompanyId,
                        //        IsActive = true,
                        //        CreatedBy = CurrentLoggedInUser.UserId,
                        //        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        //        UpdatedBy = CurrentLoggedInUser.UserId,
                        //        UpdatedDate = DateTime.Now.UTCCurrentTime()
                        //    };
                        //    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                        //    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                        //    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);
                        //}
                        #endregion
       
                        message = "File sent to " + EmailAddress;
                    }
                    else
                    {
                        message = "Email send failed.";
                    }
                    //if (result)
                    //{
                    //    base.AddUserActivityForCustomer("File sent to " + EmailAddress, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid,null);
                    //}
                }
                else
                {
                    result = false;
                    message = "Invalid email address.";
                }

                #region Send SMS
                List<string> ReceiverNumberList = new List<string>();
                #region ReceiverNumber Setup
                if (!string.IsNullOrWhiteSpace(PrefferedNO))
                {
                    ReceiverNumber = PrefferedNO.Replace("-", "");
                }
                else if (!string.IsNullOrWhiteSpace(model.customerInfo.SecondaryPhone))
                {
                    ReceiverNumber = model.customerInfo.SecondaryPhone.Replace("-", "");
                }
                else if (!string.IsNullOrWhiteSpace(model.customerInfo.PrimaryPhone))
                {
                    ReceiverNumber = model.customerInfo.PrimaryPhone.Replace("-", "");
                }
                else
                {
                    return Json(new { result = false, message = message + " and no phone number available." });
                }
                ReceiverNumberList.Add(ReceiverNumber);
                #endregion

                //smsFile.ShortUrl = shortUrl;
                string phonenumber = string.Join(";", ReceiverNumberList);
                if (_Util.Facade.SMSFacade.SendFileSMS(smsFile, CurrentLoggedInUser.UserId, CurrentLoggedInUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentLoggedInUser.FirstName, " ", CurrentLoggedInUser.LastName)) == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                        CustomerId = model.customerInfo.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = model.cusAgreementTemplate.Name,
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentLoggedInUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                    if(result)
                    {
                        message += " and " + phonenumber;
                    } 
                }

                #region file save to customer file
                if (result)
                {
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = model.customerInfo.Id+"_"+Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail_SMS" + ".pdf",
                        Filename = AppConfig.DomainSitePath + cusfilePath,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = model.customerInfo.CustomerId,
                        CompanyId = model.Company.CompanyId,
                        IsActive = true,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentLoggedInUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);
                }
                #endregion

                #endregion
            }
            #endregion
            return Json(new { result = result, message = message });
        }

        public JsonResult FileToCustomerPDFMailAndSMS(int? cusid, string PrefferedEmail, string PrefferedNO, int? fileid)
        {
            #region Send Mail
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                string[] Emailadd = PrefferedEmail.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (cusid.HasValue)
            {
                //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = cusid;
            }
            if (CurrentLoggedInUser.UserId != null)
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                ViewBag.UserId = CurrentLoggedInUser.UserId;
            }
            //if (fileid.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
            //    ViewBag.FileTemplateId = fileid;
            //}
            if (fileid.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                ViewBag.FileTemplateId = fileid;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
            //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (model.customerInfo != null && fileid.HasValue)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            string from = "";

            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(fileid, cusid);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(fileid, cusid);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                }
                if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                    model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                    model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                    model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                    model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                }
                if (model.cusAgreementTemplate.Name == "Finance Terms")
                {
                    model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                }
                if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                }
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            ViewAsPdf actionPDF;
            if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            else
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
            //{
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },

            //};
            string EmailAddress = PrefferedEmail;
            if (ValidPrefferedEmail.Count == 0)
            {
                EmailAddress = model.customerInfo.EmailAddress;
            }
            else
            {
                EmailAddress = string.Join(";", ValidPrefferedEmail);
            }

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);

            string message = "";
            bool result = false;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            string ReceiverNumber = "";
            if (model != null && model.cusAgreementTemplate != null)
            {
                from = model.cusAgreementTemplate.Name;
            }
            #region File Save
            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
            //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var pdfTempFold = string.Format(filename, saveComname);
            //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
            //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
            //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
            //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion

            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
            filename = filename.TrimEnd('/');
            var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            var pdfTempFold = string.Format(filename, saveComname);
            var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";

            string FileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
            string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
            string FilePath = cusfilePath;
            string FileKey = string.Format($"{FilePath}/{FileName}");
            var returnurl = "";

            var task = Task.Run(async () => {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            //Thread thread = new Thread(async () => {

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();


            returnurl = S3Domain;
            returnurl = returnurl + FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End

            #region File Management Body contect
            SMSFile smsFile = new SMSFile();
            bool IsFileWithoutCustomerSign = false;
            //string bodyContent = "";
            //if (ft != null)
            //{
            if (ft != null && ft.IsCustomerSignRequired == false)
            {
                IsFileWithoutCustomerSign = true;
                //bodyContent = "Please review the attached document.";
                //smsFile.ShortUrl = "Please review the attached file from the link. " + shortUrl;
            }
            smsFile.ShortUrl = shortUrl;
            smsFile.IsFileWithoutCustomerSign = IsFileWithoutCustomerSign;
            smsFile.CompanyName = CurrentLoggedInUser.CompanyName;
            //    else
            //    {
            //        //bodyContent = "Please click the link below to review and sign the file!";
            //        //bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
            //        smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //    }
            //}
            //else
            //{
            //    //bodyContent = "Please click the link below to review and sign the file!";
            //    //bodyContent += "<br /><br /><a style='line-height: 2.6666666666666665rem; background-color: #2ca01c; color: #333; border: 1px solid #d6d6d6; -webkit-transition: background-color .2s ease-in; transition: background-color .2s ease-in; font-size: 1rem; font-family: Open Sans, Helvetica, Arial, sans-serif; font-weight: 400; display: block; height: 2.8rem; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box; -webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px; -moz-background-clip: padding; -webkit-background-clip: padding-box; background-clip: padding-box; text-align: center; vertical-align: middle; margin: 0 auto 11px; padding: 0 15px; white-space: nowrap; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;' href='" + shortUrl + "'>File</a>";
            //    smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //}
            #endregion

            if (model.customerInfo != null)
            {

                if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(model.customerInfo.EmailAddress) && model.customerInfo.EmailAddress.IsValidEmailAddress()))
                {
                    result = true;
                    FileManagement fm = new FileManagement
                    {
                        CustomerNum = model.customerInfo.FirstName + " " + model.customerInfo.LastName,
                        ToEmail = EmailAddress,
                        CompanyName = CurrentLoggedInUser.CompanyName,
                        BodyLink = shortUrl,
                        IsFileWithoutCustomerSign = IsFileWithoutCustomerSign,
                        //Subject = string.Format("REVIEW REQUIRED: See attached document from {0} at {1}", CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.CompanyName),
                        //Body = bodyContent,
                        CustomerId = model.customerInfo.CustomerId.ToString(),
                        EmployeeId = CurrentLoggedInUser.UserId.ToString(),
                        fileManagementpdf = ft != null && ft.IsCustomerSignRequired == false ? new System.Net.Mail.Attachment(stream, model.customerInfo.Id + ".pdf") : null
                    };
                    result = _Util.Facade.MailFacade.EmailFileManagement(fm, CurrentLoggedInUser.CompanyId.Value, from);

                    //// ""Mayur" Calculate File Size : start
                    #region Calculate file size

                   // var _fileSize = (decimal)applicationPDFData.Length / 1024;
                    //_fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                    #endregion
                    //// ""Mayur" Calculate File Size : End
                    ///
                    if (result)
                    {
                                                
                        
                        #region file save to customer file
                        if (result)
                        {
                            //CustomerFile cfs = new CustomerFile()
                            //{
                            //    FileDescription = ViewBag.filename,
                            //    Filename = ViewBag.returnurl,
                            //    FileSize = (double)_fileSize,
                            //    FileId = Guid.NewGuid(),    
                            //    FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                            //    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            //    CustomerId = model.customerInfo.CustomerId,
                            //    CompanyId = model.Company.CompanyId,
                            //    IsActive = true,
                            //    CreatedBy = CurrentLoggedInUser.UserId,
                            //    CreatedDate = DateTime.Now.UTCCurrentTime(),
                            //    UpdatedBy = CurrentLoggedInUser.UserId,
                            //    UpdatedDate = DateTime.Now.UTCCurrentTime()
                            //};
                            //_Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                            //string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            //base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);
                        }
                        #endregion

                        message = "File sent to " + EmailAddress;
                    }
                    else
                    {
                        message = "Email send failed.";
                    }
                    //if (result)
                    //{
                    //    base.AddUserActivityForCustomer("File sent to " + EmailAddress, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid,null);
                    //}
                }
                else
                {
                    result = false;
                    message = "Invalid email address.";
                }

                #region Send SMS
                List<string> ReceiverNumberList = new List<string>();
                #region ReceiverNumber Setup
                if (!string.IsNullOrWhiteSpace(PrefferedNO))
                {
                    ReceiverNumber = PrefferedNO.Replace("-", "");
                }
                else if (!string.IsNullOrWhiteSpace(model.customerInfo.SecondaryPhone))
                {
                    ReceiverNumber = model.customerInfo.SecondaryPhone.Replace("-", "");
                }
                else if (!string.IsNullOrWhiteSpace(model.customerInfo.PrimaryPhone))
                {
                    ReceiverNumber = model.customerInfo.PrimaryPhone.Replace("-", "");
                }
                else
                {
                    return Json(new { result = false, message = message + " and no phone number available." });
                }
                ReceiverNumberList.Add(ReceiverNumber);
                #endregion

                //smsFile.ShortUrl = shortUrl;
                string phonenumber = string.Join(";", ReceiverNumberList);
                if (_Util.Facade.SMSFacade.SendFileSMS(smsFile, CurrentLoggedInUser.UserId, CurrentLoggedInUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentLoggedInUser.FirstName, " ", CurrentLoggedInUser.LastName)) == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                        CustomerId = model.customerInfo.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = model.cusAgreementTemplate.Name,
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentLoggedInUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                    if (result)
                    {
                        message += " and " + phonenumber;
                    }
                }

                //// ""Mayur" Calculate File Size : start
                #region Calculate file size

                var _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                #endregion
                //// ""Mayur" Calculate File Size : End

                #region file save to customer file
                if (result)
                {
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_Mail_SMS" + ".pdf",
                        Filename = "/" + ViewBag.FileKey,
                        FileSize = (double)_fileSize,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = model.customerInfo.CustomerId,
                        CompanyId = model.Company.CompanyId,
                        IsActive = true,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentLoggedInUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);
                }
                #endregion

                #endregion
            }
            #endregion
            return Json(new { result = result, message = message });
        }

        public JsonResult SMSFileLinkForPrintBlank(int? cusid, string PrefferedNO, int? fileid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<string> ReceiverNumberList = new List<string>();
            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (cusid.HasValue)
            {
                //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = cusid;
            }
            if (CurrentLoggedInUser.UserId != null)
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                ViewBag.UserId = CurrentLoggedInUser.UserId;
            }
            //if (fileid.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
            //    ViewBag.FileTemplateId = fileid;
            //}
            if (fileid.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                ViewBag.FileTemplateId = fileid;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
            //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (model.customerInfo != null && fileid.HasValue)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(fileid, cusid);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(fileid, cusid);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                }
                if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                    model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                    model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                    model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                    model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                }
                if (model.cusAgreementTemplate.Name == "Finance Terms")
                {
                    model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                }
                if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                }
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            ViewAsPdf actionPDF;
            if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            else
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);

            string message = "";
            bool result = false;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + model.customerInfo.EmailAddress + "#" + CurrentLoggedInUser.CompanyId.Value.ToString() + "#" + fileid + "#" + CurrentLoggedInUser.UserId);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);

            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.customerInfo.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            string ReceiverNumber = "";

            #region File Save
            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
            //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var pdfTempFold = string.Format(filename, saveComname);
            //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
            //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
            //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
            //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion


            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
            filename = filename.TrimEnd('/');
            var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            var pdfTempFold = string.Format(filename, saveComname);
            var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";

            string FileName = cusid.ToString() + fileid.ToString() + "_" + model.cusAgreementTemplate.Name + "_" + "File.pdf";
            string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
            string FilePath = cusfilePath;
            string FileKey = string.Format($"{FilePath}/{FileName}");


            var returnurl = "";

            var task = Task.Run(async () => {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            //Thread thread = new Thread(async () => {

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();


            returnurl = S3Domain;
            returnurl = returnurl + FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End


            #region ReceiverNumber Setup
            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                ReceiverNumber = PrefferedNO.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(model.customerInfo.SecondaryPhone))
            {
                ReceiverNumber = model.customerInfo.SecondaryPhone.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(model.customerInfo.PrimaryPhone))
            {
                ReceiverNumber = model.customerInfo.PrimaryPhone.Replace("-", "");
            }
            else
            {
                return Json(new { result = false, message = "Phone number is not available." });
            }
            ReceiverNumberList.Add(ReceiverNumber);
            #endregion
            SMSFile smsFile = new SMSFile();
            bool IsFileWithoutCustomerSign = false;
            //if (ft != null)
            //{
            if (ft != null && ft.IsCustomerSignRequired == false)
            {
                IsFileWithoutCustomerSign = true;
                //smsFile.ShortUrl = "Please review the attached file from the link. " + shortUrl;
            }
            smsFile.ShortUrl = shortUrl;
            smsFile.IsFileWithoutCustomerSign = IsFileWithoutCustomerSign;
            smsFile.CompanyName = CurrentLoggedInUser.CompanyName;
            //    else
            //    {
            //        smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //    }
            //}
            //else
            //{
            //    smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
            //}
            //smsFile.ShortUrl = shortUrl;
            string phonenumber = string.Join(";", ReceiverNumberList);
            if (_Util.Facade.SMSFacade.SendFileSMS(smsFile, CurrentLoggedInUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {

                //// ""Mayur" Calculate File Size : start
                #region Calculate file size

                var _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                #endregion
                //// ""Mayur" Calculate File Size : End


                #region file save to customer file
                CustomerFile cfs = new CustomerFile()
                {
                    FileDescription = model.customerInfo.Id + "_" + Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + "_SMS" + ".pdf",
                    Filename = "/" + FileKey,
                    FileId = Guid.NewGuid(),
                    FileSize = (double)_fileSize,
                    FileFullName = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf",
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = model.customerInfo.CustomerId,
                    CompanyId = model.Company.CompanyId,
                    IsActive = true,
                    CreatedBy = CurrentLoggedInUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = CurrentLoggedInUser.UserId,
                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                };
                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                string logMessage = Regex.Replace(model.cusAgreementTemplate.Name, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cusid, null);
                #endregion
                LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                {
                    CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    CustomerId = model.customerInfo.CustomerId,
                    Type = "SMS",
                    ToMobileNo = phonenumber,
                    BodyContent = "File Management Document",
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now,
                    SentBy = CurrentLoggedInUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);

                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ReceiverNumber) });

            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ReceiverNumber) });
            }
        }

        [HttpPost]
        public JsonResult LoadCustomerSignatureImage(string data, string LeadConvertId, string FileId, string oldPath)
        {
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(int.Parse(LeadConvertId));
                CompanyId = custommerCompany.CompanyId;
            }

            bool uploadImage = false;
            string filePath = "";
            var cusid = LeadConvertId;
            Customer customerInfo = new Customer();
            if (!string.IsNullOrWhiteSpace(cusid))
            {
                customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(Convert.ToInt32(cusid));
            }
            var fileid = FileId;
            if (!string.IsNullOrWhiteSpace(oldPath) && oldPath != "" && !string.IsNullOrWhiteSpace(data) && data != "")
            {
                var oldServerFile = Server.MapPath(oldPath);
                if (System.IO.File.Exists(oldServerFile))
                {
                    System.IO.File.Delete(oldServerFile);
                }
            }
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
                string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;

                        #region Customer Signature Insert
                        if (customerInfo != null)
                        {
                            CustomerSignature cs = new CustomerSignature()
                            {
                                CustomerId = customerInfo.CustomerId,
                                ReferenceIdGuid = Guid.Empty,
                                ReferenceIdnvarchar = fileid,
                                Type = "File Management",
                                Signature = filePath,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = customerInfo.CustomerId
                            };
                            _Util.Facade.CustomerSignatureFacade.InsertCustomerSignature(cs);
                        }
                        else
                        {
                            uploadImage = false;
                            return Json(new { uploadImage = uploadImage, message = "Invalid customer." });
                        }
                        #endregion
                        string status = "";
                        var EmailReceiver = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("SendContractSignEstimateSignAndFileSignNotificationEmail");
                        if (EmailReceiver != null)
                        {
                            status = EmailReceiver.Value;
                        }
                        if (status == "true")
                        {
                            SendFileSignNotificationEmail(customerInfo.CustomerId, CompanyId);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath, LeadID = cusid }, "text/html");
        }

        [HttpPost]
        public JsonResult IAgreeSetup_v2(int? cusid, string PrefferedEmail, int? fileid, string userId)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                string[] Emailadd = PrefferedEmail.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }

            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            Guid CompanyId = new Guid();
            CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusid.Value);
            CompanyId = custommerCompany.CompanyId;
            if (cusid.HasValue)
            {
                //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = cusid;
            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            string from = "";
            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CompanyId, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(userId));
                ViewBag.UserId = userId;
            }
            //if (fileid.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
            //    ViewBag.FileTemplateId = fileid;
            //}
            if (fileid.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                ViewBag.FileTemplateId = fileid;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
            //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (fileid.HasValue && model.customerInfo != null)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
            }
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }

            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(fileid, cusid);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(fileid, cusid);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                }
                if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                    model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                    model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                    model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                    model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                }
                if (model.cusAgreementTemplate.Name == "Finance Terms")
                {
                    model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                }
                if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                }
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            ViewAsPdf actionPDF;
            if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            else
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
            //{
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },

            //};
            string EmailAddress = PrefferedEmail;
            if (ValidPrefferedEmail.Count == 0)
            {
                EmailAddress = model.customerInfo.EmailAddress;
            }
            else
            {
                EmailAddress = string.Join(";", ValidPrefferedEmail);
            }
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);

            if (!string.IsNullOrWhiteSpace(model.FileManagementCustomerSignature) && model.FileManagementCustomerSignature != "")
            {
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                filename = filename.TrimEnd('/');
                var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var pdfTempFold = string.Format(filename, saveComname);
                var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
                string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
                string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                string FileNameExt = "File_Signed.pdf";

                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    FileNameExt = "Cancellation_Signed.pdf";
                }
                else if(model.cusAgreementTemplate!= null && model.cusAgreementTemplate.Name != null)
                {
                    FileNameExt = model.cusAgreementTemplate.Name.Replace(" ", "_")+"_Signed.pdf";
                }
              
                CustomerFile cfs = new CustomerFile()
                {
                    FileDescription = cusid.ToString() + "_" + FileNameExt,
                    Filename = AppConfig.DomainSitePath + cusfilePath,
                    FileId = Guid.NewGuid(),
                    FileFullName = cusid.ToString() + fileid.ToString() + FileNameExt,
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = model.customerInfo.CustomerId,
                    CompanyId = model.Company.CompanyId,
                    IsActive = true,
                    CreatedBy = model.customerInfo.Soldby1,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = model.customerInfo.Soldby1,
                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                };
                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                //base.AddUserActivityForCustomer("New File added #Ref:"+ cusid.ToString() + fileid.ToString() + FileNameExt, LabelHelper.ActivityAction.AddFile, model.customerInfo.CustomerId, null, cusid.ToString() + fileid.ToString() + FileNameExt);
                FileManagement fm = new FileManagement
                {
                    CustomerNum = model.customerInfo.FirstName + " " + model.customerInfo.LastName,
                    ToEmail = EmailAddress,
                    CustomerId = model.customerInfo.CustomerId.ToString(),
                    EmployeeId = CompanyId.ToString(),
                    fileManagementpdf = new System.Net.Mail.Attachment(stream, cusid.ToString() + fileid.ToString() + FileNameExt)
                };
                if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.Name != null)
                {
                    from = model.cusAgreementTemplate.Name +" Signed";
                }
                _Util.Facade.MailFacade.EmailFileManagementConfirmation(fm, CompanyId,from);

                #region Cancellation Queue
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue cencellationQueue = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (cencellationQueue != null)
                    {
                        cencellationQueue.IsSigned = true;
                        _Util.Facade.CustomerFacade.UpdateCustomerCancellationQueue(cencellationQueue);
                    }
                    else
                    {
                        CustomerCancellationQueue Model = new CustomerCancellationQueue()
                        {
                            CancellationId = Guid.NewGuid(),
                            CustomerId = model.customerInfo.CustomerId,
                            CreatedBy = model.customerInfo.CreatedByUid,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CancellationDate = DateTime.Now.UTCCurrentTime(),
                            RemainingBalance = 0,
                            IsSigned = true,
                            IsActive = true

                        };
                        _Util.Facade.CustomerFacade.InsertCustomerCancellationQueue(Model);
                    }
                    //try
                    //{
                    //    _Util.Facade.CustomerFacade.UpdateCustomerCancellationQueue(cencellationQueue);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }

                #endregion
            }
            return Json(new { result = true });
        }


        public JsonResult IAgreeSetup(int? cusid, string PrefferedEmail, int? fileid, string userId)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                string[] Emailadd = PrefferedEmail.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }

            FileTemplateWithCustomerInfo model = new FileTemplateWithCustomerInfo();
            FileTemplate ft = new FileTemplate();
            Guid CompanyId = new Guid();
            CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusid.Value);
            CompanyId = custommerCompany.CompanyId;
            if (cusid.HasValue)
            {
                //model.customerInfo = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);
                model.customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(cusid.Value);
                model.InstallationAddress = MakeAddress(model.customerInfo.Street, model.customerInfo.City, model.customerInfo.State, model.customerInfo.ZipCode, "");
                ViewBag.CustomerId = cusid;
            }
            int term = 0;
            double contract;
            string conterm = "";
            string contermVal = "";
            string from = "";
            if (model.customerInfo != null)
            {
                bool success = Double.TryParse(model.customerInfo.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    ViewBag.termid = term;
                    if (term > 1)
                    {
                        ViewBag.TermMonth = " month";
                    }
                    else
                    {
                        ViewBag.TermMonth = " month";
                    }
                }
                conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                contermVal = string.Concat(ViewBag.termid, " ");
                model._paymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandCustomerId(CompanyId, model.customerInfo.CustomerId);
            }
            model.ContractTeam = conterm;
            model.ContractTeamVal = contermVal;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                model.employeeInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(userId));
                ViewBag.UserId = userId;
            }
            //if (fileid.HasValue)
            //{
            //    model.fileTemplate = _Util.Facade.FileFacade.GetFileTemplateById(fileid.Value);
            //    ViewBag.FileTemplateId = fileid;
            //}
            if (fileid.HasValue)
            {
                model.cusAgreementTemplate = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
                ViewBag.FileTemplateId = fileid;
            }
            if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.IsFileTemplate == true && model.cusAgreementTemplate.ReferenceTemplateId.HasValue)
            {
                ft = _Util.Facade.FileFacade.GetFileTemplateById(model.cusAgreementTemplate.ReferenceTemplateId.Value);
            }
            //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
            //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString() + fileid.ToString() + "Signature";
            //string FileName = cusid.ToString() + fileid.ToString() + "-___" + "Signature.png";
            //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
            //var fullFilePath = Server.MapPath(filePath);
            //if (System.IO.File.Exists(fullFilePath))
            //{
            //    model.FileManagementCustomerSignature = fullFilePath;
            //}
            CustomerSignature cs = new CustomerSignature();
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
            GlobalSetting authorizedRepresentative = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AuthorizedRepresentative");
            model.AuthorizedRepresentative = authorizedRepresentative != null && !string.IsNullOrWhiteSpace(authorizedRepresentative.Value) ? authorizedRepresentative.Value : "";
            if (fileid.HasValue && model.customerInfo != null)
            {
                cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(model.customerInfo.CustomerId, fileid.ToString(), "File Management");
            }
            if (cs != null)
            {
                model.FileManagementCustomerSignature = cs.Signature;
                if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                {
                    model.FileManagementCustomerSignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    model.FileManagementCustomerSignatureDateVal = cs.CreatedDate.UTCToClientTime();
                }
                if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                {
                    model.CompanySignature = glbs.Value;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }

                }
                else if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }
            else
            {
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    model.CompanySignature = glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) ? glbs.Value : "";
                }
            }

            model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            model.Company.Address = MakeAddress(model.Company.Street, model.Company.City, model.Company.State, model.Company.ZipCode, "");
            //string body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            string body = "";
            if (model.cusAgreementTemplate.Name == "Smart Agreement DFW" || model.cusAgreementTemplate.Name == "CommercialFireAlarmAgreement" || model.cusAgreementTemplate.Name == "AgreementRMR" || model.cusAgreementTemplate.Name == "SmartAgreementVault" || model.cusAgreementTemplate.Name == "Smart Choice Agreement ADS")
            {
                body = GetSmartAgreementFileContent(fileid, cusid);
            }
            else if (model.cusAgreementTemplate.Name == "Customer Addendum")
            {
                body = GetCustomerAddendum(fileid, cusid);
            }
            else
            {
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue ccq = new CustomerCancellationQueue();
                    ccq = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (ccq != null && ccq.Id > 0 && ccq.CancellationDate.Value != new DateTime() && ccq.CancellationDate.Value != null)
                    {
                        model.CancellationDate = ccq.CancellationDate.Value.ToString("M/dd/yy");
                    }
                    else
                    {
                        model.CancellationDate = "TERMDATE";
                    }
                    if (ccq != null && ccq.Id > 0 && ccq.RemainingBalance != null)
                    {
                        model.RemainingBalance = (float)ccq.RemainingBalance;
                    }
                    else
                    {
                        model.RemainingBalance = 0;
                    }
                }
                if (model.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    model.AdsLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/ads_logo.PNG");
                    model.BrinksLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/brinks_logo.PNG");
                }
                if (model.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    model.iEateryLogo = string.Concat(AppConfig.SiteDomain, "/iEateryContent/Images/ieateryapp-logo.png");
                    model.VisaLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/visa.png");
                    model.MastercardLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/mastercard.png");
                    model.DiscoverLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/discover.png");
                    model.AmericanExpressLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/AmericanExpress.png");
                }
                if (model.cusAgreementTemplate.Name == "Finance Terms")
                {
                    model.OnitSmartHome = string.Concat(AppConfig.SiteDomain, "/Content/img/onit_smart_home.png");
                }
                if (model.cusAgreementTemplate.Name == "Kazar Commercial Agreement" || model.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
                }
                body = _Util.Facade.FileFacade.MakeFileTemplatePdf(model);
            }
            ViewBag.Body = body;
            ViewAsPdf actionPDF;
            if (model.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.Legal,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            else
            {
                actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
            //{
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },

            //};
            string EmailAddress = PrefferedEmail;
            if (ValidPrefferedEmail.Count == 0)
            {
                EmailAddress = model.customerInfo.EmailAddress;
            }
            else
            {
                EmailAddress = string.Join(";", ValidPrefferedEmail);
            }
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Stream stream = new MemoryStream(applicationPDFData);

            if (!string.IsNullOrWhiteSpace(model.FileManagementCustomerSignature) && model.FileManagementCustomerSignature != "")
            {

                #region File Save 
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //var pdfTempFold = string.Format(filename, saveComname);
                //var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";
                //string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File.pdf";
                //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                //string FileNameExt = "File_Signed.pdf";
                #endregion


                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                filename = filename.TrimEnd('/');
                var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var pdfTempFold = string.Format(filename, saveComname);
                var pdftempFolderName = cusid.ToString() + fileid.ToString() + "CustomerFile";

                string cusFileName = cusid.ToString() + fileid.ToString() + "-___" + "File_Signed.pdf";
                string cusfilePath = string.Concat(pdfTempFold, "/", pdftempFolderName);
                string FilePath = cusfilePath;
                string FileKey = string.Format($"{FilePath}/{cusFileName}");

                string FileNameExt = "File_Signed.pdf";


                var returnurl = "";

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(cusFileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start    

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(cusFileName, FilePath);

                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = S3Domain;
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = cusFileName;
                ViewBag.FileKey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End

                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    FileNameExt = "Cancellation_Signed.pdf";
                }
                else if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.Name != null)
                {
                    FileNameExt = model.cusAgreementTemplate.Name.Replace(" ", "_") + "_Signed.pdf";
                }

                //// ""Mayur" Calculate File Size : start

                #region Calculate file size

                var _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                #endregion

                //// ""Mayur" Calculate File Size : End



                CustomerFile cfs = new CustomerFile()
                {
                    FileDescription = cusid.ToString() + "_" + FileNameExt,
                    Filename = "/" + ViewBag.FileKey,
                    FileId = Guid.NewGuid(),
                    FileSize = (double)_fileSize,
                    FileFullName = cusid.ToString() + fileid.ToString() + FileNameExt,
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = model.customerInfo.CustomerId,
                    CompanyId = model.Company.CompanyId,
                    IsActive = true,
                    CreatedBy = model.customerInfo.Soldby1,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = model.customerInfo.Soldby1,
                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                };
                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                //base.AddUserActivityForCustomer("New File added #Ref:"+ cusid.ToString() + fileid.ToString() + FileNameExt, LabelHelper.ActivityAction.AddFile, model.customerInfo.CustomerId, null, cusid.ToString() + fileid.ToString() + FileNameExt);
                FileManagement fm = new FileManagement
                {
                    CustomerNum = model.customerInfo.FirstName + " " + model.customerInfo.LastName,
                    ToEmail = EmailAddress,
                    CustomerId = model.customerInfo.CustomerId.ToString(),
                    EmployeeId = CompanyId.ToString(),
                    fileManagementpdf = new System.Net.Mail.Attachment(stream, cusid.ToString() + fileid.ToString() + FileNameExt)
                };
                if (model.cusAgreementTemplate != null && model.cusAgreementTemplate.Name != null)
                {
                    from = model.cusAgreementTemplate.Name + " Signed";
                }
                _Util.Facade.MailFacade.EmailFileManagementConfirmation(fm, CompanyId, from);

                #region Cancellation Queue
                if (model.cusAgreementTemplate.Name == "Cancellation")
                {
                    CustomerCancellationQueue cencellationQueue = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(model.customerInfo.CustomerId);
                    if (cencellationQueue != null)
                    {
                        cencellationQueue.IsSigned = true;
                        _Util.Facade.CustomerFacade.UpdateCustomerCancellationQueue(cencellationQueue);
                    }
                    else
                    {
                        CustomerCancellationQueue Model = new CustomerCancellationQueue()
                        {
                            CancellationId = Guid.NewGuid(),
                            CustomerId = model.customerInfo.CustomerId,
                            CreatedBy = model.customerInfo.CreatedByUid,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CancellationDate = DateTime.Now.UTCCurrentTime(),
                            RemainingBalance = 0,
                            IsSigned = true,
                            IsActive = true

                        };
                        _Util.Facade.CustomerFacade.InsertCustomerCancellationQueue(Model);
                    }
                    //try
                    //{
                    //    _Util.Facade.CustomerFacade.UpdateCustomerCancellationQueue(cencellationQueue);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }

                #endregion
            }
            return Json(new { result = true });
        }
        public ActionResult AddEquipmentsFile()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadPaidCommissionImportFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerLeadImportFile"];
            string tempFolderName = ConfigurationManager.AppSettings["File.PaidCommissionImportFile"];
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [HttpPost]
        public JsonResult DeleteFileManagementFile(int? id)
        {
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                var objfile = _Util.Facade.FileFacade.GetFileTemplateById(id.Value);
                if (objfile != null)
                {
                    result = _Util.Facade.FileFacade.DeleteFileManagementFile(id.Value);
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCoverLetterImportFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerLeadImportFile"];
            string tempFolderName = ConfigurationManager.AppSettings["File.EstimatorCoverLetterImportFile"];
            var comname = ConfigurationManager.AppSettings["HomePageImage"];
            tempFolderName = string.Format(tempFolderName, comname);

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception ex) 
                    {  /*TODO: You must process this exception.*/
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\uploaderror.txt"), true))
                        {
                            file.WriteLine("Saving getting error.");
                            file.Close();
                        }
                    }
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }
        #region Send File Sign Notification Email
        public bool SendFileSignNotificationEmail(Guid CustomerId, Guid companyId)
        {
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                //return RedirectToAction("Index", "Login");
            }
            else
            {
                Guid CompanyId = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    //return RedirectToAction("Index", "Login");
                }
            }

            bool result = false;

            if (CustomerId != null)
            {
                string recevieremail = "";

                var EmailReceiver = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("FileSignNotificationReceiverEmail");
                if (EmailReceiver != null)
                {
                    recevieremail = EmailReceiver.Value;
                }

                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (cus != null)
                {
                    FileManagementNotificationEmail EmailNotifiaction = new FileManagementNotificationEmail()
                    {
                        //Subject = "Agreement Has Signed.",
                        CustomerNameWithId= cus.FirstName + " " + cus.LastName + "(" + cus.Id + ").",
                        ToEmail = recevieremail,
                        //EmailBody = "An agreement has signed for " + cus.FirstName + " " + cus.LastName + "(" + cus.Id + ").",
                    };
                    result = _Util.Facade.MailFacade.SendSignNotificationEmailForFileManagement(EmailNotifiaction, companyId);
                }

            }
            return result;
        }
        #endregion
    }
}