using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;
using static HS.Framework.Utils.EmailTemplateKey;

namespace HS.Facade
{
    public class CustomerFileFacade : BaseFacade
    {
        public CustomerFileFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        CustomerFileDataAccess _CustomerFileDataAccess
        {
            get
            {
                return (CustomerFileDataAccess)_ClientContext[typeof(CustomerFileDataAccess)];
            }
        }
        EstimatorFileDataAccess _EstimatorFileDataAccess
        {
            get
            {
                return (EstimatorFileDataAccess)_ClientContext[typeof(EstimatorFileDataAccess)];
            }
        }

        public List<CustomerFile> GetAllFilesByCustomerIdAndCompanyId(Guid customerId, Guid CompanyId, string SearchText)
        {
            DataSet dsResult = _CustomerFileDataAccess.GetAllFilesByCustomerIdAndCompanyId(customerId, SearchText,true);
            DataTable dt = dsResult.Tables[0];
            List<CustomerFile> fileList = new List<CustomerFile>();
            fileList = (from DataRow dr in dt.Rows
                        select new CustomerFile()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0, 
                            FileDescription = dr["FileDescription"].ToString(),
                            Filename = dr["Filename"].ToString(),
                            FileFullName = dr["FileFullName"].ToString(),
                            Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            FileSize = dr["FileSize"] != DBNull.Value ? Convert.ToDouble(dr["FileSize"]) : 0,
                            Tag = dr["Tag"].ToString(),
                            InvoiceId = dr["InvoiceId"].ToString(),
                            CreatedName = dr["CreatedName"].ToString(),
                            CreatedBy = (Guid)dr["CreatedBy"],
                            GeeseFileType = dr["GeeseFileType"].ToString(),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            UpdatedBy = (Guid)dr["UpdatedBy"],
                            UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),
                            FileId = (Guid)dr["FileId"],
                            WMStatus = dr["WMStatus"].ToString(),
                            //ExpirationDays = dr["ExpirationDays"] != DBNull.Value ? Convert.ToInt32(dr["ExpirationDays"]) : 0,
                            ExiprationDate = dr["ExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["ExpirationDate"]) : new DateTime(),
                            //SentDate = dr["SentDate"] != DBNull.Value ? Convert.ToDateTime(dr["SentDate"]) : new DateTime(),
                        }).ToList();
            return fileList;
        }
        public CustomerFile GetCustomerFileByFileName(string value)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format("Filename = '{0}'", value)).FirstOrDefault();
        }
        //public EstimatorFile GetEstimatorFileByFileName(string value)
        //{
        //    return _EstimatorFileDataAccess.GetByQuery(string.Format("Filename = '{0}'", value)).FirstOrDefault();
        //}
        public List<CustomerFile> GetAllTagFilesByCustomerIdAndCompanyId(Guid customerId, Guid CompanyId)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and CompanyId= '{1}' and IsActive = 'True' and Tag is not null and Tag != ''  order by Uploadeddate desc", customerId, CompanyId));
        }
        public int SaveEstimatorPdfFile(string fileName, string EstimateNo, Guid CustomerId, Guid CompanyId, bool Signed = false, bool SendSMS = false, bool SendMail = false)
        {
            string EndingTitle = "";
            string MailTitle = "";
            string SMSTitle = "";
            if (Signed)
            {
                EndingTitle = "_Signed";
            }
            if (SendSMS)
            {
                SMSTitle = "_SMS";
            }
            if (SendMail)
            {
                MailTitle = "_Mail";
            }
            CustomerFile cuf = new CustomerFile()
            {
                CompanyId = CompanyId,
                FileId = Guid.NewGuid(),
                CustomerId = CustomerId,
                FileDescription = string.Concat(EstimateNo, EndingTitle, SMSTitle, MailTitle, ".pdf"),
                FileFullName = string.Concat(EstimateNo, EndingTitle, SMSTitle, MailTitle, ".pdf"),
                Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
                IsActive = true,
                Uploadeddate = DateTime.UtcNow,
                CreatedBy = new Guid(),
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = new Guid(),
                UpdatedDate = DateTime.Now.UTCCurrentTime()
            };

            return (int)InsertCustomerFile(cuf);
        }
        public int SaveEstimatorPdfEstimatorFile(string fileName, string EstimateNo, Guid CustomerId, Guid CompanyId, bool Signed = false, bool SendSMS = false, bool SendMail = false)
        {
            string EndingTitle = "";
            string MailTitle = "";
            string SMSTitle = "";
            if (Signed)
            {
                EndingTitle = "_Signed";
            }
            if (SendSMS)
            {
                SMSTitle = "_SMS";
            }
            if (SendMail)
            {
                MailTitle = "_Mail";
            }
            List<EstimatorFile> estimatorfile = _EstimatorFileDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}'", EstimateNo)).ToList();
            if(estimatorfile != null && estimatorfile.Count()>0)
            {
                foreach(var item in estimatorfile)
                {
                    EstimatorFile estimatorfiles = new EstimatorFile()
                    {
                        Id = item.Id,
                        Filename = string.Concat(EstimateNo, EndingTitle, SMSTitle, MailTitle, ".pdf"), 
                        FileDescription = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = new Guid(),
                        UpdatedBy = new Guid(),
                        EstimatorId = EstimateNo,
                        FileSize = 0.0,
                        FileFullName = string.Concat(EstimateNo, EndingTitle),
                        IsActive = true,
                    };
                    _EstimatorFileDataAccess.Update(estimatorfiles);
                }
            }

            CustomerFile cf = GetCustomerFileByDescriptionAndCustomerId(string.Concat(EstimateNo, EndingTitle), CustomerId);
            if (cf == null)
            {
                CustomerFile cuf = new CustomerFile()
                {
                    CompanyId = CompanyId,
                    FileId = Guid.NewGuid(),
                    CustomerId = CustomerId,
                    FileDescription = string.Concat(EstimateNo, EndingTitle),
                    FileFullName = string.Concat(EstimateNo, EndingTitle, ".pdf"),
                    Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
                    IsActive = true,
                    Uploadeddate = DateTime.UtcNow,
                    CreatedBy = new Guid(),
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = new Guid(),
                    UpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                return (int)InsertCustomerFile(cuf);
            }
            else
            {
                cf.Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName;
                return UpdateCustomerFile(cf);
            } 
           
        }
        public List<CustomerFile> GetAllInactiveFilesByCustomerIdAndCompanyId(Guid customerId, Guid CompanyId, string searchtext)
        {
            DataSet dsResult = _CustomerFileDataAccess.GetAllFilesByCustomerIdAndCompanyId(customerId, searchtext, false);
            DataTable dt = dsResult.Tables[0];
            List<CustomerFile> fileList = new List<CustomerFile>();
            fileList = (from DataRow dr in dt.Rows
                        select new CustomerFile()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            FileDescription = dr["FileDescription"].ToString(),
                            Filename = dr["Filename"].ToString(),
                            FileFullName = dr["FileFullName"].ToString(),
                            Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            FileSize = dr["FileSize"] != DBNull.Value ? Convert.ToDouble(dr["FileSize"]) : 0,
                            Tag = dr["Tag"].ToString(),
                            InvoiceId = dr["InvoiceId"].ToString(),
                            CreatedName = dr["CreatedName"].ToString(),
                            CreatedBy = (Guid)dr["CreatedBy"],
                            GeeseFileType = dr["GeeseFileType"].ToString(),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            UpdatedBy = (Guid)dr["UpdatedBy"],
                            UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),
                            FileId = (Guid)dr["FileId"]

                        }).ToList();
            return fileList;
        }
        public long InsertCustomerFile(CustomerFile cf)
        {
            return _CustomerFileDataAccess.Insert(cf);
        }
        public long InsertEstimatorFile(EstimatorFile cf)
        {
            return _EstimatorFileDataAccess.Insert(cf);
        }
        public CustomerFile GetAllFilesByCustomerIdAndCompanyId(int id)
        {
            return _CustomerFileDataAccess.Get(id);
        }

        public bool DeleteById(int id)
        {
            return _CustomerFileDataAccess.Delete(id) > 0;
        }

        public CustomerFile GetFileNameById(int value)
        {
            return _CustomerFileDataAccess.Get(value);
        }

        public CustomerFile getCustomerFileCompanyIdAndCustomerId(Guid comid, Guid cusid, string value)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and IsActive = 'True' and FileDescription = '{2}'", comid, cusid, value)).FirstOrDefault();
        }

        public bool DeleteEstimateFile(int id)
        {
            return _CustomerFileDataAccess.Delete(id) > 0;
        }

        public bool DeleteCustomerFile(int value)
        {
            return _CustomerFileDataAccess.Delete(value) > 0;
        }

        public CustomerFile GetCustomerFileByDescriptionAndCustomerId(string description, Guid CustomerId)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format("FileDescription = '{0}'  and CustomerId ='{1}'", description, CustomerId)).FirstOrDefault();
        }
        public CustomerFile GetCustomerFileById(int Id)
        {
            return _CustomerFileDataAccess.Get(Id);
        }
        public List<CustomerFile> GetCustomerAgreementFileByCustomerId(Guid CustomerId)
        {
            string query = string.Format("CustomerId='{0}' and FileFullName like '%Agreement%'", CustomerId);
            return _CustomerFileDataAccess.GetByQuery(query);
        }
        public List<CustomerFile> GetCustomerFileListById(List<int> attId)
        {
            var strId = string.Join(", ", attId);
            string query = string.Format("Id in ({0})", strId);
            return _CustomerFileDataAccess.GetByQuery(query);
        }
        public List<CustomerFile> GetCustomerEcontractFileByCustomerId(Guid CustomerId)
        {
            string query = string.Format("CustomerId='{0}' and FileFullName like '%Econtract%'", CustomerId);
            return _CustomerFileDataAccess.GetByQuery(query);
        }
        public int UpdateCustomerFile(CustomerFile cf)
        {
            return (int)_CustomerFileDataAccess.Update(cf);
        }
        public int UpdateEstimatorFile(EstimatorFile cf)
        {
            return (int)_EstimatorFileDataAccess.Update(cf);
        }
        public int SaveEstimatePdfFile(string fileName, string EstimateNo, Guid CustomerId, Guid CompanyId, bool Signed = false, bool Approved = false)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string EndingTitle = "";
            if (Signed)
            {
                EndingTitle = "_Signed";
            }
            else if (Approved)
            {
                EndingTitle = "_Approved";
            }


            CustomerFile cf = GetCustomerFileByDescriptionAndCustomerId(string.Concat(EstimateNo, EndingTitle), CustomerId);
            if (cf == null)
            {
                CustomerFile cuf = new CustomerFile()
                {
                    CompanyId = CompanyId,
                    FileId = Guid.NewGuid(),
                    CustomerId = CustomerId,
                    FileDescription = string.Concat(EstimateNo, EndingTitle),
                    FileFullName = string.Concat(EstimateNo, EndingTitle, ".pdf"),
                    Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
                    IsActive = true,
                    Uploadeddate = DateTime.UtcNow,
                    CreatedBy = new Guid(),
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = new Guid(),
                    UpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                return (int)InsertCustomerFile(cuf);
            }
            else
            {
                cf.Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName;
                return UpdateCustomerFile(cf);
            }
        }

        public CustomerFile GetCustomerLatestAgreementByCustomerId(Guid customerId)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format(" CustomerId ='{0}' and FileFullName like '%Agreement.pdf' order by id desc", customerId)).FirstOrDefault();
        }
        public GeeseMediaNoteDetails GetMediaANdNoteListByCustomerId(Guid CustomerId)
        {
            DataSet dsResult = _CustomerFileDataAccess.GetMediaANdNoteListByCustomerId(CustomerId);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            List<GeeseMedia> CustomerMediaList = new List<GeeseMedia>();
            CustomerMediaList = (from DataRow dr in dt.Rows
                                 select new GeeseMedia()
                                 {
                                     Notes = dr["Note"].ToString(),
                                     Url = dr["Path"].ToString(),
                                     Address = dr["Address"].ToString(),
                                     Assigner = dr["Assigner"].ToString(),
                                     UploadDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime()
                                 }).ToList();
            List<GeeseNote> CustomerNoteList = new List<GeeseNote>();
            CustomerNoteList = (from DataRow dr in dt1.Rows
                                select new GeeseNote()
                                {
                                    Notes = dr["Note"].ToString(),
                                    Assigner = dr["Assigner"].ToString(),
                                    Date = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime()
                                }).ToList();

            GeeseMediaNoteDetails Detail = new GeeseMediaNoteDetails();
            Detail.GeeseMediaList = CustomerMediaList;
            Detail.GeeseNoteList = CustomerNoteList;
            return Detail;
        }

        public List<CustomerFile> GetAllFilesForWMByCustomerIdAndCompanyId(List<int> CustomerIdList, Guid CompanyId)
        {
            DataSet dsResult = _CustomerFileDataAccess.GetAllFilesForWMByCustomerIdAndCompanyId(CustomerIdList, CompanyId);
            DataTable dt = dsResult.Tables[0];
            List<CustomerFile> fileList = new List<CustomerFile>();
            fileList = (from DataRow dr in dt.Rows
                        select new CustomerFile()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            FileDescription = dr["FileDescription"].ToString(),
                            Filename = dr["Filename"].ToString(),
                            FileFullName = dr["FileFullName"].ToString(),
                            Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            FileSize = dr["FileSize"] != DBNull.Value ? Convert.ToDouble(dr["FileSize"]) : 0,
                            Tag = dr["Tag"].ToString(),
                            InvoiceId = dr["InvoiceId"].ToString(),
                            CreatedBy = (Guid)dr["CreatedBy"],
                            GeeseFileType = dr["GeeseFileType"].ToString(),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            UpdatedBy = (Guid)dr["UpdatedBy"],
                            UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),
                            FileId = (Guid)dr["FileId"],
                            WMStatus = dr["WMStatus"].ToString(),
                            AWSProcessStatus = dr["AWSProcessStatus"].ToString(),
                            AWSUploadTS = dr["AWSUploadTS"] != DBNull.Value ? Convert.ToDateTime(dr["AWSUploadTS"]) : new DateTime(),
                            CustomerIntId = (int)(dr["IntCustID"])

                        }).ToList();
            return fileList;
        }
    }
}
