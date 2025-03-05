using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using System.Drawing;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using HS.Facade;
using NLog;
using HS.Framework.Utils;


namespace HS.Web.UI.Controllers
{
    public class ImageController : BaseController
    {
        string S3Domain = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
        public ImageController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        HSApiFacade HSApiFacade = new HSApiFacade();
        HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
        public FileResult EmpShow(int? W, int? H, string X)
        {
            string fileUrl = "";
            string path = "";
            bool? Demo=false;
            /*Guid? EM*/
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            if (Guid.TryParse(X, out EM2))
            {
                EM = EM2;
            }
            Employee emp = new Employee();
            if (EM.HasValue && EM.Value != Guid.Empty)
            {
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EM.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.ProfilePicture) || (EM.HasValue && EM.Value == Guid.Empty))
            {
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region DataReady 
            fileUrl = emp.ProfilePicture;
            fileUrl = $"{S3Domain.Replace("dgtest02", "dfwsec01").TrimEnd('/')}{emp.ProfilePicture}";
            if (string.IsNullOrWhiteSpace(emp.ProfilePicture))
            {
                //thumbCheck = true;
                fileUrl = emp.ProfilePicture;
            }
            Image img = null;
            try
            {
                //img = Image.FromFile(Server.MapPath(filepath), true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                WebClient webClient = new  WebClient();
                webClient.Headers.Add("User-Agent: Other");
                byte[] imageContent = webClient.DownloadData(fileUrl);
                using (MemoryStream imageBuffer = new MemoryStream(imageContent))
                {
                    img = Image.FromStream(imageBuffer, true);
                }

                //using ()
                //{
                //    //Issue the GET request to a URL and read the response into a 
                //    //stream that can be used to load the image
                //    
                //    webClient.Proxy = null;



                //}                
            }
            catch (Exception e)
            {
                logger.Error(e);
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.ProfilePicture), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult EmpShow1(int? W, int? H,/*Guid? EM*/ string EMP, bool? Demo)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            if (Guid.TryParse(EMP, out EM2))
            {
                EM = EM2;
            }
            Employee emp = new Employee();
            if (EM.HasValue && EM.Value != Guid.Empty)
            {
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EM.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.ProfilePicture) || (EM.HasValue && EM.Value == Guid.Empty))
            {
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region DataReady 
            filepath = emp.ProfilePicture;
            if (string.IsNullOrWhiteSpace(emp.ProfilePicture))
            {
                //thumbCheck = true;
                filepath = emp.ProfilePicture;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                logger.Error(e);
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.ProfilePicture), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }
        public FileResult EmpShow_old(int? W, int? H,/*Guid? EM*/ string EMP,bool? Demo)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            if(Guid.TryParse(EMP, out EM2))
            {
                EM = EM2;
            }
            Employee emp = new Employee();
            if(EM.HasValue && EM.Value != Guid.Empty)
            {
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EM.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.ProfilePicture) || (EM.HasValue && EM.Value == Guid.Empty) )
            {
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
               return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            
            #region DataReady 
            filepath = emp.ProfilePicture;
            if (string.IsNullOrWhiteSpace(emp.ProfilePicture))
            {
                //thumbCheck = true;
                filepath = emp.ProfilePicture;
            }
            Image img = null; 
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                logger.Error(e);
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            } 

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.ProfilePicture), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion
                
            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult TicketStatusImageShow(int? W, int? H, string STATUS, bool? Demo)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            TicketStatusImageSetting sts = new TicketStatusImageSetting();
            if (!string.IsNullOrWhiteSpace(STATUS))
            {
                sts = _Util.Facade.TicketFacade.GetTicketStatusImageSettingByStatus(STATUS);
            }
            if (sts == null || string.IsNullOrWhiteSpace(sts.Filename))
            {
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region DataReady 
            filepath = sts.Filename;
            if (string.IsNullOrWhiteSpace(sts.Filename))
            {
                //thumbCheck = true;
                filepath = sts.Filename;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/WhiteDot.png");
                if (Demo.HasValue && Demo.Value)
                {
                    path = Server.MapPath("~/Content/img/profile_pic_dami.png");
                }
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(sts.Filename), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }


        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }

        public FileResult CustomerImgShow(int? W, int? H, string CustomerId, string UserName, string CompanyId)
        {

            var logcompany = HSMainApiFacade.GetCompanyConnectionByUserName(UserName);
            if (logcompany == null)
            {
                string errorpath = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                return new FileStreamResult(new FileStream(errorpath, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            if(logcompany.CompanyId.ToString() != CompanyId.ToLower())
            {
                string errorpath = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                return new FileStreamResult(new FileStream(errorpath, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            HSApiFacade = new HSApiFacade(logcompany.ConnectionString);

            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            if (Guid.TryParse(CustomerId, out EM2))
            {
                EM = EM2;
            }
            Customer customer = new Customer();
            if (EM.HasValue && EM.Value != Guid.Empty)
            {
                customer = HSApiFacade.GetCustomerByCustomerId(EM.Value);
            }
            if (customer == null || string.IsNullOrWhiteSpace(customer.ProfileImage) || (EM.HasValue && EM.Value == Guid.Empty))
            {
                path = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region DataReady 
            filepath = customer.ProfileImage;
            if (string.IsNullOrWhiteSpace(customer.ProfileImage))
            {
                
                filepath = customer.ProfileImage;
            }
            Image img = null;
            try
            {
                img = Bitmap.FromStream(new MemoryStream(new WebClient().DownloadData(filepath)));
                foreach (var prop in img.PropertyItems)
                {
                    if (prop.Id == 0x0112) //value of EXIF
                    {
                        int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                        RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                        img.RotateFlip(rotateFlipType);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            if (img == null)
            {
                img = Bitmap.FromStream(new MemoryStream(new WebClient().DownloadData(filepath)));
                foreach (var prop in img.PropertyItems)
                {
                    if (prop.Id == 0x0112) //value of EXIF
                    {
                        int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                        RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                        img.RotateFlip(rotateFlipType);

                        break;
                    }
                }
            }
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult EstimateCameraImgShow(int? W, int? H, string InvoiceId, string ImageType, string UserName, string CompanyId)
        {

            var logcompany = HSMainApiFacade.GetCompanyConnectionByUserName(UserName);
            if (logcompany == null)
            {
                string errorpath = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                return new FileStreamResult(new FileStream(errorpath, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            if (logcompany.CompanyId.ToString() != CompanyId.ToLower())
            {
                string errorpath = Server.MapPath("~/Content/img/default-profile-img-male.jpg");
                return new FileStreamResult(new FileStream(errorpath, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            HSApiFacade = new HSApiFacade(logcompany.ConnectionString);

            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            EstimateImage EstimateImage = new EstimateImage();
            if (!string.IsNullOrWhiteSpace(InvoiceId))
            {
                EstimateImage = HSApiFacade.GetEstimateImageByInvoiceIdAndImageType(InvoiceId, ImageType);
            }
            if (EstimateImage == null || string.IsNullOrWhiteSpace(EstimateImage.ImageLoc))
            {
                path = Server.MapPath("~/Content/img/default-profile-img-male.jpg");

                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            #region DataReady 
            filepath = EstimateImage.ImageLoc;
            if (string.IsNullOrWhiteSpace(EstimateImage.ImageLoc))
            {

                filepath = EstimateImage.ImageLoc;
            }
            Image img = null;
            try
            {
                img = Bitmap.FromStream(new MemoryStream(new WebClient().DownloadData(filepath)));
                foreach (var prop in img.PropertyItems)
                {
                    if (prop.Id == 0x0112) //value of EXIF
                    {
                        int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                        RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                        img.RotateFlip(rotateFlipType);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/default-profile-img-male.jpg");

                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }

            if (img == null)
            {
                img = Bitmap.FromStream(new MemoryStream(new WebClient().DownloadData(filepath)));
                foreach (var prop in img.PropertyItems)
                {
                    if (prop.Id == 0x0112) //value of EXIF
                    {
                        int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                        RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                        img.RotateFlip(rotateFlipType);

                        break;
                    }
                }
            }
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult WebLocImageShow(int? W, int? H, int? ID, Guid CompanyId)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            WebsiteLocation emp = new WebsiteLocation();
            Company com = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetIeateryCompanyConnectionByCompanyId(CompanyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }
            else
            {
                Guid comid = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                    string Text = "No" + Environment.NewLine + "Photo";
                    Image bitimg;
                    using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;

                                graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                            }
                        }
                        bitimg = new Bitmap(bitmap);
                        bitmap.Dispose();
                    }
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        bitimg.Save(memory, ImageFormat.Jpeg);
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}

                    if (bitimg != null)
                    {
                        double imgWidht = bitimg.Width;
                        double imgHeight = bitimg.Height;
                        double newX = 0;
                        double newY = 0;
                        if (imgWidht < imgHeight)
                        {
                            newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                            //imgHeight = imgWidht * 0.85;
                        }
                        else
                        {
                            newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                            //imgWidht = imgHeight / 0.82;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                    }

                    if (W.HasValue && H.HasValue)
                    {
                        bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                        int newX = 0, newY = 0;
                        if (W.Value < bitimg.Width)
                        {
                            newX = (bitimg.Width - W.Value) / 2;
                        }
                        else if (H.Value < bitimg.Height)
                        {
                            newY = (bitimg.Height - H.Value) / 2;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                    }

                    #region ImageSend  
                    FileContentResult bitimgresult;
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                        //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                        long length = memStream.Length;
                        
                        codecParams.Dispose();
                    }
                    return bitimgresult;
                    #endregion
                }
            }
            if (ID.HasValue && ID.Value > 0)
            {
                emp = _Util.Facade.MenuFacade.GetWebLocById(ID.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.ImageLoc))
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region DataReady 
            filepath = emp.ImageLoc;
            if (string.IsNullOrWhiteSpace(emp.ImageLoc))
            {
                //thumbCheck = true;
                filepath = emp.ImageLoc;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.ImageLoc), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult WebLocCoverImageShow(int? W, int? H, int? ID, Guid CompanyId)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            //Guid EM2 = new Guid();
            WebsiteLocation emp = new WebsiteLocation();
            Company com = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetIeateryCompanyConnectionByCompanyId(CompanyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;

                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }
            else
            {
                Guid comid = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                    string Text = "No" + Environment.NewLine + "Photo";
                    Image bitimg;
                    using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;

                                graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                            }
                        }
                        bitimg = new Bitmap(bitmap);
                        bitmap.Dispose();
                    }
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        bitimg.Save(memory, ImageFormat.Jpeg);
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}

                    if (bitimg != null)
                    {
                        double imgWidht = bitimg.Width;
                        double imgHeight = bitimg.Height;
                        double newX = 0;
                        double newY = 0;
                        if (imgWidht < imgHeight)
                        {
                            newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                            //imgHeight = imgWidht * 0.85;
                        }
                        else
                        {
                            newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                            //imgWidht = imgHeight / 0.82;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                    }

                    if (W.HasValue && H.HasValue)
                    {
                        bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                        int newX = 0, newY = 0;
                        if (W.Value < bitimg.Width)
                        {
                            newX = (bitimg.Width - W.Value) / 2;
                        }
                        else if (H.Value < bitimg.Height)
                        {
                            newY = (bitimg.Height - H.Value) / 2;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                    }

                    #region ImageSend  
                    FileContentResult bitimgresult;
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                        //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                        long length = memStream.Length;

                        codecParams.Dispose();
                    }
                    return bitimgresult;
                    #endregion
                }
            }
            if (ID.HasValue && ID.Value > 0)
            {
                emp = _Util.Facade.MenuFacade.GetWebLocById(ID.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.CoverImageLoc))
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;

                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region DataReady 
            filepath = emp.CoverImageLoc;
            if (string.IsNullOrWhiteSpace(emp.CoverImageLoc))
            {
                //thumbCheck = true;
                filepath = emp.CoverImageLoc;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;

                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.CoverImageLoc), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult CategoryImageShow(int? W, int? H, int? ID, Guid CompanyId)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            RestCategory emp = new RestCategory();
            Company com = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetIeateryCompanyConnectionByCompanyId(CompanyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }
            else
            {
                Guid comid = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                    string Text = "No" + Environment.NewLine + "Photo";
                    Image bitimg;
                    using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;

                                graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                            }
                        }
                        bitimg = new Bitmap(bitmap);
                        bitmap.Dispose();
                    }
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        bitimg.Save(memory, ImageFormat.Jpeg);
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}

                    if (bitimg != null)
                    {
                        double imgWidht = bitimg.Width;
                        double imgHeight = bitimg.Height;
                        double newX = 0;
                        double newY = 0;
                        if (imgWidht < imgHeight)
                        {
                            newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                            //imgHeight = imgWidht * 0.85;
                        }
                        else
                        {
                            newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                            //imgWidht = imgHeight / 0.82;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                    }

                    if (W.HasValue && H.HasValue)
                    {
                        bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                        int newX = 0, newY = 0;
                        if (W.Value < bitimg.Width)
                        {
                            newX = (bitimg.Width - W.Value) / 2;
                        }
                        else if (H.Value < bitimg.Height)
                        {
                            newY = (bitimg.Height - H.Value) / 2;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                    }

                    #region ImageSend  
                    FileContentResult bitimgresult;
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                        //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                        long length = memStream.Length;
                        
                        codecParams.Dispose();
                    }
                    return bitimgresult;
                    #endregion
                }
            }
            if (ID.HasValue && ID.Value > 0)
            {
                emp = _Util.Facade.MenuFacade.GetCategoryById(ID.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.Image))
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region DataReady 
            filepath = emp.Image;
            if (string.IsNullOrWhiteSpace(emp.Image))
            {
                //thumbCheck = true;
                filepath = emp.Image;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.Image), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult MenuItemImageShow(int? W, int? H, int? ID, Guid CompanyId)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            RestMenuItem emp = new RestMenuItem();
            Company com = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetIeateryCompanyConnectionByCompanyId(CompanyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }
            else
            {
                Guid comid = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                    string Text = "No" + Environment.NewLine + "Photo";
                    Image bitimg;
                    using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;

                                graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                            }
                        }
                        bitimg = new Bitmap(bitmap);
                        bitmap.Dispose();
                    }
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        bitimg.Save(memory, ImageFormat.Jpeg);
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}

                    if (bitimg != null)
                    {
                        double imgWidht = bitimg.Width;
                        double imgHeight = bitimg.Height;
                        double newX = 0;
                        double newY = 0;
                        if (imgWidht < imgHeight)
                        {
                            newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                            //imgHeight = imgWidht * 0.85;
                        }
                        else
                        {
                            newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                            //imgWidht = imgHeight / 0.82;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                    }

                    if (W.HasValue && H.HasValue)
                    {
                        bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                        int newX = 0, newY = 0;
                        if (W.Value < bitimg.Width)
                        {
                            newX = (bitimg.Width - W.Value) / 2;
                        }
                        else if (H.Value < bitimg.Height)
                        {
                            newY = (bitimg.Height - H.Value) / 2;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                    }

                    #region ImageSend  
                    FileContentResult bitimgresult;
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                        //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                        long length = memStream.Length;
                        
                        codecParams.Dispose();
                    }
                    return bitimgresult;
                    #endregion
                }
            }
            if (ID.HasValue && ID.Value > 0)
            {
                emp = _Util.Facade.MenuFacade.GetMenuItemById(ID.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.Photo))
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region DataReady 
            filepath = emp.Photo;
            if (string.IsNullOrWhiteSpace(emp.Photo))
            {
                //thumbCheck = true;
                filepath = emp.Photo;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.Photo), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }

        public FileResult MenuImageShow(int? W, int? H, int? ID, Guid CompanyId)
        {
            string filepath = "";
            string path = "";
            //int x = 0, y = 0, w = 0, h = 0;
            Guid? EM = new Guid();
            Guid EM2 = new Guid();
            RestMenu emp = new RestMenu();
            Company com = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetIeateryCompanyConnectionByCompanyId(CompanyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }
            else
            {
                Guid comid = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                    string Text = "No" + Environment.NewLine + "Photo";
                    Image bitimg;
                    using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;

                                graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                            }
                        }
                        bitimg = new Bitmap(bitmap);
                        bitmap.Dispose();
                    }
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        bitimg.Save(memory, ImageFormat.Jpeg);
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}

                    if (bitimg != null)
                    {
                        double imgWidht = bitimg.Width;
                        double imgHeight = bitimg.Height;
                        double newX = 0;
                        double newY = 0;
                        if (imgWidht < imgHeight)
                        {
                            newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                            //imgHeight = imgWidht * 0.85;
                        }
                        else
                        {
                            newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                            //imgWidht = imgHeight / 0.82;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                    }

                    if (W.HasValue && H.HasValue)
                    {
                        bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                        int newX = 0, newY = 0;
                        if (W.Value < bitimg.Width)
                        {
                            newX = (bitimg.Width - W.Value) / 2;
                        }
                        else if (H.Value < bitimg.Height)
                        {
                            newY = (bitimg.Height - H.Value) / 2;
                        }
                        bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                    }

                    #region ImageSend  
                    FileContentResult bitimgresult;
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                        //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                        long length = memStream.Length;
                        
                        codecParams.Dispose();
                    }
                    return bitimgresult;
                    #endregion
                }
            }
            if (ID.HasValue && ID.Value > 0)
            {
                emp = _Util.Facade.MenuFacade.GetMenuById(ID.Value);
            }
            if (emp == null || string.IsNullOrWhiteSpace(emp.Photo))
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region DataReady 
            filepath = emp.Photo;
            if (string.IsNullOrWhiteSpace(emp.Photo))
            {
                //thumbCheck = true;
                filepath = emp.Photo;
            }
            Image img = null;
            try
            {
                img = Image.FromFile(Server.MapPath(filepath), true);
            }
            catch (Exception e)
            {
                path = Server.MapPath("~/Content/img/Solid_White_Image.jpg");
                string Text = "No" + Environment.NewLine + "Photo";
                Image bitimg;
                using (var bitmap = (Bitmap)Image.FromFile(path))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 10, bitmap.Height - 10);

                            StringFormat sf = new StringFormat();
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            graphics.DrawString(Text, arialFont, Brushes.Black, rect, sf);
                        }
                    }
                    bitimg = new Bitmap(bitmap);
                    bitmap.Dispose();
                }
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        bitimg.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                if (bitimg != null)
                {
                    double imgWidht = bitimg.Width;
                    double imgHeight = bitimg.Height;
                    double newX = 0;
                    double newY = 0;
                    if (imgWidht < imgHeight)
                    {
                        newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                        //imgHeight = imgWidht * 0.85;
                    }
                    else
                    {
                        newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                        //imgWidht = imgHeight / 0.82;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
                }

                if (W.HasValue && H.HasValue)
                {
                    bitimg = ImageHelper.GetImageResizeMin(W.Value, H.Value, bitimg);

                    int newX = 0, newY = 0;
                    if (W.Value < bitimg.Width)
                    {
                        newX = (bitimg.Width - W.Value) / 2;
                    }
                    else if (H.Value < bitimg.Height)
                    {
                        newY = (bitimg.Height - H.Value) / 2;
                    }
                    bitimg = ImageHelper.CropByUserCoord(bitimg, W.Value, H.Value, newX, newY);
                }

                #region ImageSend  
                FileContentResult bitimgresult;
                using (var memStream = new System.IO.MemoryStream())
                {
                    var qualityEncoder = Encoder.Quality;
                    var quality = (long)80;
                    var ratio = new EncoderParameter(qualityEncoder, quality);
                    var codecParams = new EncoderParameters(1);
                    codecParams.Param[0] = ratio;
                    var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                    bitimg.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                    //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitimgresult = this.File(memStream.GetBuffer(), "image/jpeg");
                    long length = memStream.Length;
                    
                    codecParams.Dispose();
                }
                return bitimgresult;
                #endregion
            }

            #region If remote server
            /*else
            {
                using (var client = new WebClient())
                {
                    Byte[] imga = null;
                    try
                    {
                        imga = client.DownloadData("https://propertyqueen.com.my/" + filepath);

                    }
                    catch (Exception e)
                    {
                        path = Server.MapPath("~/Content/Icons/upload.jpg");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    using (var ms = new MemoryStream(imga))
                    {
                        img = new Bitmap(ms);
                    }
                }
            }*/
            #endregion

            if (img == null)
            {
                //thumbCheck = true;
                img = Image.FromFile(Server.MapPath(emp.Photo), true);
            }
            #region If there is Croods
            //else if (!string.IsNullOrWhiteSpace(pc.Coordinate))
            //{
            //    string[] cor = pc.Coordinate.Split(',');
            //    x = (Int32)Convert.ToDouble(cor[0]);
            //    y = (Int32)Convert.ToDouble(cor[1]);
            //    w = (Int32)Convert.ToDouble(cor[2]);
            //    h = (Int32)Convert.ToDouble(cor[3]);
            //    img = ImageHelper.CropByUserCoord(img, w, h, x, y);
            //}
            #endregion
            else
            {
                double imgWidht = img.Width;
                double imgHeight = img.Height;
                double newX = 0;
                double newY = 0;
                if (imgWidht < imgHeight)
                {
                    newY = (imgHeight - (imgWidht /* * 0.85*/)) / 2;
                    //imgHeight = imgWidht * 0.85;
                }
                else
                {
                    newX = (imgWidht - (imgHeight /*/ 0.85*/)) / 2;
                    //imgWidht = imgHeight / 0.82;
                }
                img = ImageHelper.CropByUserCoord(img, (int)imgWidht, (int)imgHeight, (int)newX, (int)newY);
            }
            #endregion

            #region Height and Width Calculations
            if (W.HasValue && H.HasValue)
            {
                img = ImageHelper.GetImageResizeMin(W.Value, H.Value, img);

                int newX = 0, newY = 0;
                if (W.Value < img.Width)
                {
                    newX = (img.Width - W.Value) / 2;
                }
                else if (H.Value < img.Height)
                {
                    newY = (img.Height - H.Value) / 2;
                }
                img = ImageHelper.CropByUserCoord(img, W.Value, H.Value, newX, newY);
            }
            #endregion

            #region setting watermark
            //if (!thumbCheck && !WM.HasValue)
            //{
            //    img = ImageHelper.SetWaterMark(img);
            //}
            #endregion

            #region ImageSend  
            FileContentResult result;
            using (var memStream = new System.IO.MemoryStream())
            {
                var qualityEncoder = Encoder.Quality;
                var quality = (long)80;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                img.Save(memStream, GetEncoder(ImageFormat.Jpeg), codecParams);
                //img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
                long length = memStream.Length;
                img.Dispose();
                codecParams.Dispose();
            }
            return result;
            #endregion 
        }
    }
}