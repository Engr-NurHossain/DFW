using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class CustomerAppoinmentFacade : BaseFacade
    {
        public CustomerAppoinmentFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerAppointmentDataAccess _CustomerAppoinmentDataAccess
        {
            get
            {
                return (CustomerAppointmentDataAccess)_ClientContext[typeof(CustomerAppointmentDataAccess)];
            }
        }
        RMRTagDataAccess _RMRTagDataAccess
        {
            get
            {
                return (RMRTagDataAccess)_ClientContext[typeof(RMRTagDataAccess)];
            }
        }
        KnowledgebaseRMRTagDataAccess _KnowledgebaseRMRTagDataAccess
        {
            get
            {
                return (KnowledgebaseRMRTagDataAccess)_ClientContext[typeof(KnowledgebaseRMRTagDataAccess)];
            }
        }
        KnowledgebaseRMRTagMapDataAccess _KnowledgebaseRMRTagMapDataAccess
        {
            get
            {
                return (KnowledgebaseRMRTagMapDataAccess)_ClientContext[typeof(KnowledgebaseRMRTagMapDataAccess)];
            }
        }
        RMRTagMapDataAccess _RMRTagMapDataAccess
        {
            get
            {
                return (RMRTagMapDataAccess)_ClientContext[typeof(RMRTagMapDataAccess)];
            }
        }
        KnowledgeSearchedKeywordDataAccess _KnowledgeSearchedKeywordDataAccess
        {
            get
            {
                return (KnowledgeSearchedKeywordDataAccess)_ClientContext[typeof(KnowledgeSearchedKeywordDataAccess)];
            }
        }
        KnowledgebaseAccessedHistoryDataAccess _KnowledgebaseAccessedHistoryDataAccess
        {
            get
            {
                return (KnowledgebaseAccessedHistoryDataAccess)_ClientContext[typeof(KnowledgebaseAccessedHistoryDataAccess)];
            }
        }
        EstimateImageDataAccess _EstimateImageDataAccess
        {
            get
            {
                return (EstimateImageDataAccess)_ClientContext[typeof(EstimateImageDataAccess)];
            }
        }
        EstimatorFileDataAccess _EstimatorFileDataAccess
        {
            get
            {
                return (EstimatorFileDataAccess)_ClientContext[typeof(EstimatorFileDataAccess)];
            }
        }
        KnowledgebaseDataAccess _KnowledgebaseDataAccess
        {
            get
            {
                return (KnowledgebaseDataAccess)_ClientContext[typeof(KnowledgebaseDataAccess)];
            }
        }
        KnowledgeBaseFlagUserDataAccess _KnowledgeBaseFlagUserDataAccess
        {
            get
            {
                return (KnowledgeBaseFlagUserDataAccess)_ClientContext[typeof(KnowledgeBaseFlagUserDataAccess)];
            }
        }
        KnowledgebaseWeblinkDataAccess _KnowledgebaseWeblinkDataAccess
        {
            get
            {
                return (KnowledgebaseWeblinkDataAccess)_ClientContext[typeof(KnowledgebaseWeblinkDataAccess)];
            }
        }
        DocumentLibraryWeblinkDataAccess _DocumentLibraryWeblinkDataAccess
        {
            get
            {
                return (DocumentLibraryWeblinkDataAccess)_ClientContext[typeof(DocumentLibraryWeblinkDataAccess)];
            }
        }
        DocumentLibraryDataAccess _DocumentLibraryDataAccess
        {
            get
            {
                return (DocumentLibraryDataAccess)_ClientContext[typeof(DocumentLibraryDataAccess)];
            }
        }
        KnowledgebaseGroupAccessDataAccess _KnowledgebaseGroupAccessDataAccess
        {
            get
            {
                return (KnowledgebaseGroupAccessDataAccess)_ClientContext[typeof(KnowledgebaseGroupAccessDataAccess)];
            }
        }
        KnowledgebaseAccountabilityDataAccess _KnowledgebaseAccountabilityDataAccess
        {
            get
            {
                return (KnowledgebaseAccountabilityDataAccess)_ClientContext[typeof(KnowledgebaseAccountabilityDataAccess)];
            }
        }
        CustomerSnapshotDataAccess _CustomerSnapshotDataAccess
        {
            get
            {
                return (CustomerSnapshotDataAccess)_ClientContext[typeof(CustomerSnapshotDataAccess)];
            }
        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        CustomerAppointmentEquipmentDataAccess _CustomerAppointmentEquipmentDataAccess
        {
            get
            {
                return (CustomerAppointmentEquipmentDataAccess)_ClientContext[typeof(CustomerAppointmentEquipmentDataAccess)];
            }
        }
        CustomerAppointmentDetailDataAccess _CustomerAppointmentDetailDataAccess
        {
            get
            {
                return (CustomerAppointmentDetailDataAccess)_ClientContext[typeof(CustomerAppointmentDetailDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        CustomerAppointmentTechnicianDataAccess _CustomerAppointmentTechnicianDataAccess
        {
            get
            {
                return (CustomerAppointmentTechnicianDataAccess)_ClientContext[typeof(CustomerAppointmentTechnicianDataAccess)];
            }
        }
        AdditionalMembersAppointmentDataAccess _AdditionalMembersAppointmentDataAccess
        {
            get
            {
                return (AdditionalMembersAppointmentDataAccess)_ClientContext[typeof(AdditionalMembersAppointmentDataAccess)];
            }
        }
        public List<CustomerAppointment> GetAllSalesAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllSalesAppoinmentByEmployeeId(customerId, companyId, employeeId);
            List<CustomerAppointment> CustomerAppointmentList = new List<CustomerAppointment>();
            CustomerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           SalesPerson = dr["SalesPerson"].ToString(),
                                           AppointmentId = (Guid)dr["AppointmentId"],
                                           CompanyId = (Guid)dr["CompanyId"],
                                           CustomerId = (Guid)dr["CustomerId"],
                                           EmployeeId = (Guid)dr["EmployeeId"],
                                           AppointmentType = dr["AppointmentType"].ToString(),
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                           Notes = dr["Notes"].ToString(),
                                           LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                           CreatedBy = dr["CreatedBy"].ToString(),
                                           LastUpdatedBy = dr["LastUpdatedBy"].ToString()
                                       }).ToList();
            return CustomerAppointmentList;
            //return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyId));
        }
        public List<CustomerAppointment> GetAllWorkOrderAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllWorkOrderAppoinmentByEmployeeId(customerId, companyId, employeeId);
            List<CustomerAppointment> CustomerAppointmentList = new List<CustomerAppointment>();
            CustomerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           WorkPerson = dr["WorkPerson"].ToString(),
                                           AppointmentId = (Guid)dr["AppointmentId"],
                                           CompanyId = (Guid)dr["CompanyId"],
                                           CustomerId = (Guid)dr["CustomerId"],
                                           EmployeeId = (Guid)dr["EmployeeId"],
                                           AppointmentType = dr["AppointmentType"].ToString(),
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                           Notes = dr["Notes"].ToString(),
                                           LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                           CreatedBy = dr["CreatedBy"].ToString(),
                                           LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                           Idstring = "Product Installation",
                                           Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,

                                       }).ToList();
            return CustomerAppointmentList;
            //return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyId));
        }

        public List<CustomerAppointment> GetAllWorkOrderAppoinmentByCustomerIdAndCompanyId(Guid customerId, Guid companyId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllWorkOrderAppoinmentByCustomerIdAndCompanyId(customerId, companyId);
            List<CustomerAppointment> CustomerAppointmentList = new List<CustomerAppointment>();
            CustomerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           WorkPerson = dr["WorkPerson"].ToString(),
                                           AppointmentId = (Guid)dr["AppointmentId"],
                                           CompanyId = (Guid)dr["CompanyId"],
                                           CustomerId = (Guid)dr["CustomerId"],
                                           EmployeeId = (Guid)dr["EmployeeId"],
                                           AppointmentType = dr["AppointmentType"].ToString(),
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                           Notes = dr["Notes"].ToString(),
                                           LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                           CreatedBy = dr["CreatedBy"].ToString(),
                                           LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                           Idstring = "Product Installation",
                                           Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,

                                       }).ToList();
            return CustomerAppointmentList;
            //return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyId));
        }


        public CustomerAppointment GetAppointmentById(int value)
        {
            return _CustomerAppoinmentDataAccess.Get(value);
        }
        public bool UpdateCustomerAppoinment(CustomerAppointment ca)
        {
            return _CustomerAppoinmentDataAccess.Update(ca) > 0;
        }
        public bool UpdateCustomerAppoinmentEquipment(CustomerAppointmentEquipment cae)
        {
            return _CustomerAppointmentEquipmentDataAccess.Update(cae) > 0;
        }
        public Knowledgebase GetKnowledgebase(int Id)
        {
            return _KnowledgebaseDataAccess.Get(Id);
        }
        public bool InsertFlagUserForKnowledgebase(KnowledgeBaseFlagUser model)
        {
            return _KnowledgeBaseFlagUserDataAccess.Insert(model) > 0;
        }
        public KnowledgeBaseFlagUser GetFlagUserForKnowledgebase(Guid userid, int knowledgebaseid, bool IsDocument, bool isadmin = false)
        {
            string DocumentQuery = "";
            if (IsDocument)
            {
                DocumentQuery = string.Format("and IsDocument = 1");
            }
            else
            {
                DocumentQuery = string.Format("and IsDocument = 0");
            }

            if (isadmin)
            {
                return _KnowledgeBaseFlagUserDataAccess.GetByQuery(string.Format("KnowledgebaseId={1} AND IsFlag=1 {2}", userid, knowledgebaseid, DocumentQuery)).LastOrDefault();
            }
            else
            {
                return _KnowledgeBaseFlagUserDataAccess.GetByQuery(string.Format("UserId = '{0}' AND KnowledgebaseId={1} AND IsFlag=1 {2}", userid, knowledgebaseid, DocumentQuery)).FirstOrDefault();
            }
        }
        public bool DeleteDocumentLibraryWeblinkByKnowledgebaseId(int id)
        {
            return true;//_DocumentLibraryWeblinkDataAccess.DeleteDocumentLibraryWeblinkByKnowledgebaseId(id);
        }
        public bool UnFlagUserForKnowledgebase(int KnowledgebaseId, Guid uid)
        {
            return _KnowledgebaseWeblinkDataAccess.UnFlagUserForKnowledgebase(KnowledgebaseId, uid);
        }
        public bool UpdateDocumentLibrary(DocumentLibrary model)
        {
            return _DocumentLibraryDataAccess.Update(model) > 0;
        }
        public bool UpdateKnowledgebase(Knowledgebase model)
        {
            return _KnowledgebaseDataAccess.Update(model) > 0;
        }
        public bool DeleteKnowledgebaseWeblinkByKnowledgebaseId(int id)
        {
            return _KnowledgebaseWeblinkDataAccess.DeleteKnowledgebaseWeblinkByKnowledgebaseId(id);
        }
        public long InsertKnowledgebaseWeblinkClass(KnowledgebaseWeblink model)
        {
            return _KnowledgebaseWeblinkDataAccess.Insert(model);
        }
        public long InsertDocumentLibraryWeblink(DocumentLibraryWeblink model)
        {
            return _DocumentLibraryWeblinkDataAccess.Insert(model);
        }

        //public bool DeleteEstimateImageByKnowledgeId(string id, bool IsDocument)
        //{
        //    return _EstimateImageDataAccess.DeleteEstimateImageByKnowledgeId(id, IsDocument);
        //}
        //public long InsertEstimateImage(EstimateImage model)
        //{
        //    return _EstimateImageDataAccess.Insert(model);
        //}
        public long InsertDocumentLibrary(DocumentLibrary model)
        {
            return _DocumentLibraryDataAccess.Insert(model);
        }
        public List<KnowledgebaseWeblink> GetWeblinksListByKnowledgebaseId(int id)
        {
            return _KnowledgebaseWeblinkDataAccess.GetWeblinksListByKnowledgebaseId(id);
        }
        public Knowledgebase GetKnowledgebaseWithTagName(int Id)
        {
            DataSet ds = _KnowledgebaseDataAccess.GetKnowledgebaseWithTagName(Id);
            Knowledgebase model = new Knowledgebase();
            if (ds != null)
            {
                model = (from DataRow dr in ds.Tables[0].Rows
                         select new Knowledgebase()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             Title = dr["Title"].ToString(),
                             Tags = dr["Tags"].ToString(),
                             Answer = dr["Answer"].ToString(),
                             IsDocumentLibrary = dr["IsDocumentLibrary"] != DBNull.Value ? Convert.ToBoolean(dr["IsDocumentLibrary"]) : false,
                             IsHidden = dr["IsHidden"] != DBNull.Value ? Convert.ToBoolean(dr["IsHidden"]) : false,
                             IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false
                         }).FirstOrDefault();
            }
            return model;
        }
        public DocumentLibrary GetDocumentLibrary(int Id)
        {
            return _DocumentLibraryDataAccess.Get(Id);
        }
        public List<KnowledgeBaseFlagUserCustom> GetFlagUserCommentForKnowledgebase(int KnowledgebaseId, bool IsDocument)
        {
            DataTable dt = _KnowledgebaseWeblinkDataAccess.GetFlagUserCommentForKnowledgebase(KnowledgebaseId, IsDocument);
            List<KnowledgeBaseFlagUserCustom> result = new List<KnowledgeBaseFlagUserCustom>();
            if (dt != null)
            {
                result = (from DataRow dr in dt.Rows
                          select new KnowledgeBaseFlagUserCustom()
                          {
                              Name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                              Comment = dr["Comment"] != DBNull.Value ? dr["Comment"].ToString() : "",
                              Date = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).UTCToClientTime() : new DateTime(),
                          }).ToList();
            }
            return result;
        }
        public List<KnowledgeBaseFlagUserCustom> GetFlagUserCommentForKnowledgebase(int KnowledgebaseId, Guid uid, bool IsDocument)
        {
            DataTable dt = _KnowledgebaseWeblinkDataAccess.GetFlagUserCommentForKnowledgebase(KnowledgebaseId, uid, IsDocument);
            List<KnowledgeBaseFlagUserCustom> result = new List<KnowledgeBaseFlagUserCustom>();
            if (dt != null)
            {
                result = (from DataRow dr in dt.Rows
                          select new KnowledgeBaseFlagUserCustom()
                          {
                              Name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                              Comment = dr["Comment"] != DBNull.Value ? dr["Comment"].ToString() : "",
                              Date = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).UTCToClientTime() : new DateTime(),
                          }).ToList();
            }
            return result;
        }
        public List<Knowledgebase> SearchListOfKnowledgebase(string key, string tag, int Id, bool IsDeleted)
        {
            string tagquery = "";
            string deletedquerry = "";
            if (!string.IsNullOrWhiteSpace(tag) && tag != "null")
            {
                var array = tag.Split(",");
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrWhiteSpace(tagquery))
                        {
                            tagquery += string.Format("and (Tags like '%{0}%'", item);
                        }
                        else
                        {
                            tagquery += string.Format(" or Tags like '%{0}%'", item);
                        }
                    }
                    tagquery = tagquery + ")";
                }
            }
            if (!IsDeleted)
            {
                deletedquerry += string.Format("and IsDeleted = 0");
            }
            else
            {
                deletedquerry += string.Format("and IsDeleted = 1");
            }
            return _KnowledgebaseDataAccess.GetByQuery(string.Format("(Title like '%{0}%' or Tags like '%{0}%') {1} and IsDocumentLibrary = 0 and Id != {2} {3}", key, tagquery, Id, deletedquerry)).ToList();
        }
        public List<EstimateImage> GetEstimateImage(string Id, bool IsDocument)
        {
            string docCheck = "";
            if (IsDocument)
            {
                docCheck = "and IsDocument = 1";
            }
            else
            {
                docCheck = "and IsDocument = 0";
            }
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}' {1} order by ImageType asc", Id, docCheck)).ToList();
        }
        public List<EstimateImage> GetSortListOfEstimateImage(int Id, string Order, bool IsDocument)
        {
            string docCheck = "";
            string orderQuery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "descending/image")
                {
                    orderQuery = "order by ImageType desc";
                }
                else if (Order == "descending/name")
                {
                    orderQuery = "order by ImageType desc";
                }
                else if (Order == "ascending/name")
                {
                    orderQuery = "order by ImageType asc";
                }
                else if (Order == "ascending/uploaddate")
                {
                    orderQuery = "order by UploadedDate desc";
                }
                else if (Order == "descending/uploaddate")
                {
                    orderQuery = "order by UploadedDate asc";
                }
                else if (Order == "descending/size")
                {
                    orderQuery = "order by Size desc";
                }
                else if (Order == "ascending/size")
                {
                    orderQuery = "order by Size asc";
                }
                else
                {
                    orderQuery = "order by ImageType asc";
                }
            }
            else
            {
                orderQuery = "order by ImageType asc";
            }
            #endregion
            if (IsDocument)
            {
                docCheck = "and IsDocument = 1";
            }
            else
            {
                docCheck = "and IsDocument = 0";
            }
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}' {1} {2}", Id, docCheck, orderQuery)).ToList();
        }
        public bool DeleteEstimateImageByKnowledgeId(string id, bool IsDocument)
        {
            return _EstimateImageDataAccess.DeleteEstimateImageByKnowledgeId(id, IsDocument);
        }
        public List<DocumentLibraryWeblink> GetWeblinksListByDocumentId(int id)
        {
            return _DocumentLibraryWeblinkDataAccess.GetWeblinkByDocumentLibraryId(id);
        }
        public bool DeleteKnowledgebaseGroupAccess(int KnowledgebaseId, bool IsDocument)
        {
            return _KnowledgebaseGroupAccessDataAccess.DeleteKnowledgebaseGroupAccess(KnowledgebaseId, IsDocument);
        }
        public long InsertKnowledgebaseGroupAccess(KnowledgebaseGroupAccess access)
        {
            return _KnowledgebaseGroupAccessDataAccess.Insert(access);
        }
        public bool UpdateKnowledgebaseGroupAccess(KnowledgebaseGroupAccess model)
        {
            return _KnowledgebaseGroupAccessDataAccess.Update(model) > 0;
        }
        public List<KnowledgebaseGroupAccess> GetKnowledgebaseGroupAccess(int KnowledgeId)
        {
            return _KnowledgebaseGroupAccessDataAccess.GetByQuery(string.Format("KnowledgeBaseId = '{0}'", KnowledgeId)).ToList();
        } 
        //public List<AccessedKnowledgebase> GetAllAccessedKnowledgebase(string UserRole)
        //{
        //    List<AccessedKnowledgebase> Model = new List<AccessedKnowledgebase>();
        //    DataSet ds = _KnowledgebaseGroupAccessDataAccess.GetAllAccessedKnowledgebase(UserRole);
        //    Model = (from DataRow dr in ds.Tables[0].Rows
        //             select new AccessedKnowledgebase()
        //             {
        //                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                 Title = dr["Title"].ToString(),
        //                 IsDefault = dr["IsDefault"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefault"]) : false
        //             }).ToList();
        //    return Model;
        //} 
        public bool InsertKnowledgebaseAccountability(KnowledgebaseAccountability model)
        {
            return _KnowledgebaseAccountabilityDataAccess.Insert(model) > 0;
        }
        public KnowledgebaseAccountability GetKnowledgebaseAccountability(int Id, string UserId)
        {
            return _KnowledgebaseAccountabilityDataAccess.GetByQuery(string.Format("KnowledgebaseId = {0} and AssignedUser = '{1}' order by id desc", Id, UserId)).FirstOrDefault();
        }
        public bool UpdateKnowledgebaseAccountability(KnowledgebaseAccountability model)
        {
            return _KnowledgebaseAccountabilityDataAccess.Update(model) > 0;
        }
        public KnowledgebaseAccountability GetKnowledgebaseAccountabilityChecked(int Id, string UserId)
        {
            return _KnowledgebaseAccountabilityDataAccess.GetByQuery(string.Format("KnowledgebaseId = {0} and AssignedUser = '{1}' order by id desc", Id, UserId)).FirstOrDefault();
        }
        public List<KnowledgebaseAccountability> GetKnowledgebaseAccountabilityCount(string UserId)
        {
            return _KnowledgebaseAccountabilityDataAccess.GetByQuery(string.Format("AssignedUser = '{0}' and IsRead = 0", UserId)).ToList();
        }
        public List<KnowledgebaseAccountability> GetAllAccessedKnowledgebaseListForUser(Guid UserId)
        {
            List<KnowledgebaseAccountability> Model = new List<KnowledgebaseAccountability>();
            DataSet ds = _KnowledgebaseAccountabilityDataAccess.GetAllAccessedKnowledgebaseListForUser(UserId);
            Model = (from DataRow dr in ds.Tables[0].Rows
                     select new KnowledgebaseAccountability()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         KnowledgebaseId = dr["KnowledgebaseId"] != DBNull.Value ? Convert.ToInt32(dr["KnowledgebaseId"]) : 0,
                         AssignedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                         AssignedByUserName = dr["AssignedByUserName"].ToString(),
                         Title = dr["Title"].ToString()
                     }).ToList();
            return Model;
        }

        public List<KnowledgebaseAccountability> GetKnowledgebaseAccountabilityByKnowledgebaseId(int Id)
        {
            return _KnowledgebaseAccountabilityDataAccess.GetByQuery(string.Format("KnowledgebaseId = '{0}'", Id)).ToList();
        }
        public List<KnowledgebaseAccountability> GetKnowledgebaseAccountabilityByAssignedUser(Guid UserId)
        {
            return _KnowledgebaseAccountabilityDataAccess.GetByQuery(string.Format("AssignedUser = '{0}' and IsRead = 0", UserId)).ToList();
        }
        public List<KnowledgebaseAccountability> GetKnowledgebaseAccountabilityByAssignedUserForAdmin(Guid UserId)
        {
            List<KnowledgebaseAccountability> Model = new List<KnowledgebaseAccountability>();
            DataSet ds = _KnowledgebaseAccountabilityDataAccess.GetAllUnreadKnowledgebaseList(UserId);
            Model = (from DataRow dr in ds.Tables[0].Rows
                     select new KnowledgebaseAccountability()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         KnowledgebaseId = dr["KnowledgebaseId"] != DBNull.Value ? Convert.ToInt32(dr["KnowledgebaseId"]) : 0,
                         AssignedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                         Title = dr["Title"].ToString()
                     }).ToList();
            return Model;
        }
        public bool DeleteKnowledgebaseAccountability(int Id)
        {
            return _KnowledgebaseAccountabilityDataAccess.Delete(Id) > 0;
        }
        public long InsertKnowledgebase(Knowledgebase model)
        {
            return _KnowledgebaseDataAccess.Insert(model);
        } 
        public KnowledgebaseListModel GetKnowledgebasebyFilter(QtiFilter filter)
        {
            DataSet ds = _KnowledgebaseDataAccess.GetKnowledgebasebyFilter(filter);
            KnowledgebaseListModel model = new KnowledgebaseListModel();
            if (ds != null)
            {
                model.KnowledgebaseList = (from DataRow dr in ds.Tables[0].Rows
                                           select new Knowledgebase()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               AttachmentCount = dr["AttachmentCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentCount"]) : 0,
                                               Title = dr["Title"].ToString(),
                                               Tags = dr["Tags"].ToString(),
                                               Answer = dr["Answer"].ToString(),
                                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]).UTCToClientTime() : new DateTime(),
                                               UpadtedBy = dr["UpdatedBy"].ToString(),
                                               IsDocumentLibrary = dr["IsDocumentLibrary"] != DBNull.Value ? Convert.ToBoolean(dr["IsDocumentLibrary"]) : false,
                                               IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                                               IsFavourite = dr["fav"] != DBNull.Value ? Convert.ToBoolean(dr["fav"]) : false,
                                               KnowledgeWeblinkList = _KnowledgebaseWeblinkDataAccess.GetWeblinksListByKnowledgebaseId((dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0)),
                                               DeletedKnowledgebaseList = _KnowledgebaseDataAccess.GetListOfDeletedKnowledgebase((dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0))
                                           }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
                model.DeletedCount = ds.Tables[2].Rows[0]["DeletedCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["DeletedCount"]) : 0;
                model.TotalKnFlagCount = ds.Tables[3].Rows[0]["TotalKnFlagCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[0]["TotalKnFlagCount"]) : 0;
                model.TotalKnFavoriteCount = ds.Tables[4].Rows[0]["TotalKnFavoriteCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[4].Rows[0]["TotalKnFavoriteCount"]) : 0;
                model.TotalKnDeleteFavoriteCount = ds.Tables[5].Rows[0]["TotalKnDeleteFavoriteCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[0]["TotalKnDeleteFavoriteCount"]) : 0;
            }
            return model;
        }
        public KnowledgebaseListModel GetKnowledgebasebyFilterForClassroom(QtiFilter filter)
        {
            DataSet ds = _KnowledgebaseDataAccess.GetKnowledgebasebyFilterForClassroom(filter);
            KnowledgebaseListModel model = new KnowledgebaseListModel();
            if (ds != null)
            {
                model.KnowledgebaseList = (from DataRow dr in ds.Tables[0].Rows
                                           select new Knowledgebase()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               AttachmentCount = dr["AttachmentCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentCount"]) : 0,
                                               Title = dr["Title"].ToString(),
                                               Tags = dr["TagName"].ToString(),
                                               Answer = dr["Answer"].ToString(),
                                               AssignTo = dr["AssignedTo"].ToString(),
                                               LastUpdatedDate = dr["AssignedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AssignedDate"]).UTCToClientTime() : new DateTime(),
                                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]).UTCToClientTime() : new DateTime(),
                                               EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).UTCToClientTime() : new DateTime(),
                                               UpadtedBy = dr["AssignedBy"].ToString(),
                                               IsDocumentLibrary = dr["IsDocumentLibrary"] != DBNull.Value ? Convert.ToBoolean(dr["IsDocumentLibrary"]) : false,
                                               IsDefault = dr["IsRead"] != DBNull.Value ? Convert.ToBoolean(dr["IsRead"]) : false,
                                               IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                                               IsFavourite = dr["fav"] != DBNull.Value ? Convert.ToBoolean(dr["fav"]) : false,
                                               KnowledgeWeblinkList = _KnowledgebaseWeblinkDataAccess.GetWeblinksListByKnowledgebaseId((dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0)),
                                               DeletedKnowledgebaseList = _KnowledgebaseDataAccess.GetListOfDeletedKnowledgebase((dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0))
                                           }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
            }
            return model;
        }
        public DataTable DownloadKnowledgebasebyFilter(QtiFilter filter)
        {
            DataSet ds = _KnowledgebaseDataAccess.GetKnowledgebasebyFilter(filter);
            DataTable dt = new DataTable();
            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            return dt;
        } 
        public bool DeleteTagMapByKnowladgeId(int id)
        {
            return _RMRTagDataAccess.DeleteTagMapByKnowladgeId(id);
        }

        public long InsertKnowledgeSearchedKeyword(KnowledgeSearchedKeyword model)
        {
            return _KnowledgeSearchedKeywordDataAccess.Insert(model);
        }
        public KnowledgebaseReportModel GetKnowledgebaseSearchList(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            DataSet ds = _KnowledgeSearchedKeywordDataAccess.GetKnowledgebaseSearchList(CompanyId, start, end, searchtext, pageno, pagesize, order);
            KnowledgebaseReportModel model = new KnowledgebaseReportModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new KnowledgebaseReport()
                              {
                                  Searchkey = dr["Keyword"].ToString(),
                                  Count = dr["Count"] != DBNull.Value ? Convert.ToInt32(dr["Count"]) : 0,
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }
        public DataTable GetKnowledgebaseSearchListForDownload(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            return _KnowledgeSearchedKeywordDataAccess.GetKnowledgebaseSearchListForDownload(CompanyId, start, end, searchtext, pageno, pagesize, order);
        } 
        public KnowledgebaseSearchedHistoryModel GetKnowledgebaseSearchedHistoryList(DateTime start, DateTime end, string searchtext, string Order)
        {
            DataSet ds = _KnowledgeSearchedKeywordDataAccess.GetKnowledgebaseSearchedHistoryList(start, end, searchtext, Order);
            KnowledgebaseSearchedHistoryModel model = new KnowledgebaseSearchedHistoryModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new KnowledgebaseSearchedHistory()
                              {
                                  SearchBy = dr["SearchedByName"].ToString(),
                                  SearchDate = dr["SearchedDate"] != DBNull.Value ? Convert.ToDateTime(dr["SearchedDate"]).UTCToClientTime() : new DateTime(),
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }

        public long InsertKnowledgebaseAccessedHistory(KnowledgebaseAccessedHistory model)
        {
            return _KnowledgebaseAccessedHistoryDataAccess.Insert(model);
        }
        public AccessedKnowledgebaseHistoryModel GetKnowledgebaseAccessedHistoryList(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            DataSet ds = _KnowledgebaseAccessedHistoryDataAccess.GetKnowledgebaseAccessedHistoryList(CompanyId, start, end, searchtext, pageno, pagesize, order);
            AccessedKnowledgebaseHistoryModel model = new AccessedKnowledgebaseHistoryModel();
            if (ds != null)
            {
                model.List = (from DataRow dr in ds.Tables[0].Rows
                              select new AccessedKnowledgebaseHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  Title = dr["Title"].ToString(),
                                  IsDocumentLibrary = dr["IsDocumentLibrary"] != DBNull.Value ? Convert.ToBoolean(dr["IsDocumentLibrary"]) : false,
                                  Count = dr["Visited"] != DBNull.Value ? Convert.ToInt32(dr["Visited"]) : 0,
                                  IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false
                              }).ToList();
                model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;
            }
            return model;
        }
        public KnowledgebaseGroupAccess GetKnowledgebaseGroupAccess(int Id, int KnowledgeId)
        {
            return _KnowledgebaseGroupAccessDataAccess.GetByQuery(string.Format("UserGroupId = {0} and KnowledgebaseId = {1} and IsDocumentLibrary = 0", Id, KnowledgeId)).FirstOrDefault();
        }
        public List<AccessedKnowledgebase> GetAllAccessedKnowledgebase(string UserRole)
        {
            List<AccessedKnowledgebase> Model = new List<AccessedKnowledgebase>();
            DataSet ds = _KnowledgebaseGroupAccessDataAccess.GetAllAccessedKnowledgebase(UserRole);
            Model = (from DataRow dr in ds.Tables[0].Rows
                     select new AccessedKnowledgebase()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         Title = dr["Title"].ToString(),
                         IsDefault = dr["IsDefault"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefault"]) : false
                     }).ToList();
            return Model;
        }

        public List<RMRTag> GetAllTag()
        {
            return _RMRTagDataAccess.GetAll();
        }

        public KnowledgebaseRMRTag GetTagByName(string value)
        {
            return _KnowledgebaseRMRTagDataAccess.GetByQuery(string.Format("TagName = '{0}' and IsDeleted = 0", value)).FirstOrDefault();
        }
        public List<KnowledgebaseRMRTag> GetAllFavouriteTags()
        {
            return _KnowledgebaseRMRTagDataAccess.GetByQuery(string.Format("IsFavourite = 1 and IsDeleted = 0 Order by TagName asc")).ToList();
        }
        public List<KnowledgebaseRMRTag> GetAllFavouriteAndKnowledgeNavTags()
        {
            return _KnowledgebaseRMRTagDataAccess.GetByQuery(string.Format("( IsFavourite = 1 Or IsKnowledgebaseNav = 1 ) and IsDeleted = 0 Order by TagName asc")).ToList();
        }
        public List<KnowledgebaseRMRTagMap> GetAllTagMapByContactid(Guid id)
        {
            return _KnowledgebaseRMRTagMapDataAccess.GetByQuery(string.Format("ContactId = '{0}'", id)).ToList();
        }

        public bool DeleteTagMapById(int value)
        {
            return _KnowledgebaseRMRTagMapDataAccess.Delete(value) > 0;
        } 
        public long InsertEstimateImage(EstimateImage model)
        {
            return _EstimateImageDataAccess.Insert(model);
        }
        public long InsertEstimatorFile(EstimatorFile model)
        {
            return _EstimatorFileDataAccess.Insert(model);
        }
        public long UpdateEstimatorFile(EstimatorFile model)
        {
            return _EstimatorFileDataAccess.Update(model);
        }
        public bool DeleteEstimatorFileById(string EstimatorId,string EstimatorType)
        {
            return _EstimatorFileDataAccess.DeleteEstimatorFileByEstimatorId(EstimatorId, EstimatorType);
        }

         

        public KnowledgebaseHomeModel GetRecentViewedKnowledgebaseList(Guid UserId)
        {
            DataSet dt = _KnowledgebaseDataAccess.GetRecentViewedKnowledgebaseList(UserId);
            KnowledgebaseHomeModel model = new KnowledgebaseHomeModel();
            model.RecentViewedlist = (from DataRow dr in dt.Tables[0].Rows
                                      select new Knowledgebase()
                                      {
                                          Id = dr["KnowledgebaseId"] != DBNull.Value ? Convert.ToInt32(dr["KnowledgebaseId"]) : 0,
                                          Title = dr["Title"].ToString()
                                      }).ToList();
            model.MostViewedlist = (from DataRow dr in dt.Tables[1].Rows
                                    select new Knowledgebase()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                        Title = dr["Title"].ToString()
                                    }).ToList();
            model.KnowledgeSearchedKeywordList = (from DataRow dr in dt.Tables[2].Rows
                                                  select new KnowledgeSearchedKeyword()
                                                  {
                                                      Id = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                                      Keyword = dr["Keyword"].ToString()
                                                  }).ToList();
            model.RMRTagList = (from DataRow dr in dt.Tables[3].Rows
                                select new KnowledgebaseRMRTag()
                                {
                                    TagName = dr["TagName"].ToString()
                                }).ToList();
            model.FlaggedCount = dt.Tables[4].Rows[0]["TotalKnFlagCount"] != DBNull.Value ? Convert.ToInt32(dt.Tables[4].Rows[0]["TotalKnFlagCount"]) : 0;
            model.FavoriteCount = dt.Tables[5].Rows[0]["TotalKnFavoriteCount"] != DBNull.Value ? Convert.ToInt32(dt.Tables[5].Rows[0]["TotalKnFavoriteCount"]) : 0;
            return model;
        }
        public List<Knowledgebase> GetKnowledgebaseList(string tag, string order, string UserRole)
        {
            DataSet ds = _KnowledgebaseDataAccess.GetKnowledgebaseListByTag(tag, order, UserRole);
            List<Knowledgebase> model = new List<Knowledgebase>();
            if (ds != null)
            {
                model = (from DataRow dr in ds.Tables[0].Rows
                         select new Knowledgebase()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             AttachmentCount = dr["AttachmentCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentCount"]) : 0,
                             Title = dr["Title"].ToString(),
                             Tags = dr["TagName"].ToString(),
                             Answer = dr["Answer"].ToString(),
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]).UTCToClientTime() : new DateTime(),
                             UpadtedBy = dr["UpdatedBy"].ToString(),
                             IsDocumentLibrary = dr["IsDocumentLibrary"] != DBNull.Value ? Convert.ToBoolean(dr["IsDocumentLibrary"]) : false
                         }).ToList();
            }
            return model;
        }

        public long InsertCustomerAppoinment(CustomerAppointment ca)
        {
            return _CustomerAppoinmentDataAccess.Insert(ca);
        }

        public int InsertAppoinment(CustomerAppointment ca)
        {
            return (int)_CustomerAppoinmentDataAccess.Insert(ca);
        }

        public CustomerAppointment GetById(int id)
        {
            return _CustomerAppoinmentDataAccess.Get(id);
        }
        public bool DeleteSalesAppointment(int sale)
        {
            return _CustomerAppoinmentDataAccess.Delete(sale) > 0;
        }
        public List<CustomerAppointment> GetAllServiceAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllServiceAppoinmentByEmployeeId(customerId, companyId, employeeId);
            List<CustomerAppointment> CustomerAppointmentList = new List<CustomerAppointment>();
            CustomerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           ServicePerson = dr["ServicePerson"].ToString(),
                                           AppointmentId = (Guid)dr["AppointmentId"],
                                           CompanyId = (Guid)dr["CompanyId"],
                                           CustomerId = (Guid)dr["CustomerId"],
                                           EmployeeId = (Guid)dr["EmployeeId"],
                                           AppointmentType = dr["AppointmentType"].ToString(),
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                           Notes = dr["Notes"].ToString(),
                                           LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                           CreatedBy = dr["CreatedBy"].ToString(),
                                           LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                           Idstring = "Service Order #" + dr["Id"].ToString(),
                                           Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false
                                       }).ToList();
            return CustomerAppointmentList;
            //return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyId));
        }
        public bool DeleteServiceAppointment(int service)
        {
            return _CustomerAppoinmentDataAccess.Delete(service) > 0;
        }
        public long InsertSnapshot(CustomerSnapshot CustomerSnapshot)
        {
            return _CustomerSnapshotDataAccess.Insert(CustomerSnapshot);
        }
        public CustomerAppointment GetCustomerAppointmentinfoByAppointmentIdandCustomerIdandCompanyId(Guid AppointmentId, Guid CustomerId, Guid CompanyId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format(" AppointmentId = '{0}' and  CustomerId = '{1}' and  CompanyId = '{2}'", AppointmentId, CustomerId, CompanyId)).FirstOrDefault();
        }
        public CustomerAppointment GetCustomerAppointmentDetailByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format(" AppointmentId = '{0}'", AppointmentId)).FirstOrDefault();
        }
        public CustomerAppointment GetAppoinmentByAppoinmentId(int appoinmentid)
        {
            return _CustomerAppoinmentDataAccess.Get(appoinmentid);
        }
        public CustomerAppointment GetAppoinmentidByAppoinmentId(Guid Appointmentid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", Appointmentid)).FirstOrDefault();
        }
        public bool InsertCustomerAppointmentEquipmentDetails(List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList)
        {
            bool result = false;
            foreach (var items in CustomerAppointmentEquipmentList)
            {
                result = false;
                bool insertSuccessful = _CustomerAppointmentEquipmentDataAccess.Insert(items) > 0;
                if (insertSuccessful == true)
                {
                    result = insertSuccessful;
                }
            }
            return result;
        }
        public int InsertCustomerAppointmentEquipmentDetail(CustomerAppointmentEquipment CustomerAppointmentEquipment)
        {
            return (int)_CustomerAppointmentEquipmentDataAccess.Insert(CustomerAppointmentEquipment);
        }

        public List<CustomerAppointmentEquipment> IsAppointmentEquipmentExistCheck(Guid AppointmentId)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }
        public bool DeletePreviousCustomerDetailsEquiptmentByEquipmentId(List<CustomerAppointmentEquipment> PreviousList)
        {
            bool result = false;

            foreach (var items in PreviousList)
            {
                result = _CustomerAppointmentEquipmentDataAccess.Delete(items.Id) > 0;
            }

            return result;
        }
        public List<CustomerAppointmentDetail> IsAppointmentIdDetail(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }
        public CustomerAppointment GetCustomerAppointmentObjectByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId)).FirstOrDefault();
        }
        public bool UpdateCustomerAppointmentInformation(CustomerAppointment customerAppointment)
        {
            return _CustomerAppoinmentDataAccess.Update(customerAppointment) > 0;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipmentListByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId= '{0}'", AppointmentId));
        }

        public CustomerAppointment GetCompanyByComapnyId(Guid CompanyId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).FirstOrDefault();
        }
        public List<CustomerAppointmentDetail> GetAllCustomerAppointmentDetailListByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppointmentDetailDataAccess.GetByQuery(string.Format("AppointmentId= '{0}'", AppointmentId));
        }

        public CustomerAppointment GetCustomerAppointmentDetailinfoByAppointmentId(Guid AppointmentId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format(" AppointmentId = '{0}'", AppointmentId)).FirstOrDefault();
        }

        public CustomerAppointment GetAllCustomerAppointmentByAppIdCusId(Guid appid, Guid cusid)
        {

            DataTable dt = _CustomerAppoinmentDataAccess.GetAllCustomerAppointmentByAppIdCusId(appid, cusid);
            CustomerAppointment CustomerAppointment = new CustomerAppointment();
            CustomerAppointment = (from DataRow dr in dt.Rows
                                   select new CustomerAppointment()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       AppointmentId = (Guid)dr["AppointmentId"],
                                       CreatedBy = dr["CreatedBy"].ToString(),
                                       AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                       AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                       AppointmentEndTimeVal = dr["AppointmentEndTimeVal"].ToString(),
                                       AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                       AppointmentStartTimeVal = dr["AppointmentStartTimeVal"].ToString(),
                                       AppointmentType = dr["AppointmentType"].ToString(),
                                       CompanyId = (Guid)dr["CompanyId"],
                                       CustomerId = (Guid)dr["CustomerId"],
                                       EmployeeId = (Guid)dr["EmployeeId"],
                                       IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                       LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                       TotalAmountTax = dr["TotalAmountTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmountTax"]) : 0,
                                       TaxTotal = dr["TaxTotal"] != DBNull.Value ? Convert.ToDouble(dr["TaxTotal"]) : 0,
                                       TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                       Notes = dr["Notes"].ToString(),
                                       TaxPercent = dr["TaxPercent"] != DBNull.Value ? Convert.ToDouble(dr["TaxPercent"]) : 0,
                                       TaxType = dr["TaxType"].ToString(),
                                       Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                       LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),

                                   }).ToList().FirstOrDefault();
            return CustomerAppointment;
        }
        public CustomerAppointment GetAllInfo(Guid Appid, Guid Cusid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and CustomerId ='{1}'", Appid, Cusid)).FirstOrDefault(); ;
        }

        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipListByAppointmentId(Guid appointmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentListByAppointmentId(appointmentid);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        AppointmentId = (Guid)dr["AppointmentId"],
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedBy = dr["CreatedByName"].ToString(),
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        EquipmentServiceName = dr["EquipmentName"].ToString(),
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        FileDescription = dr["FileDescription"].ToString(),
                                        FileFullName = dr["FileFullName"].ToString(),
                                        FileName = dr["Filename"].ToString(),
                                        FileType = dr["FileType"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                        QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                        IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                        IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                        IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                        IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                        QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        IsInvoiceCreate = dr["IsInvoiceCreate"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceCreate"]) : false,
                                        ReferenceInvoiceId = dr["ReferenceInvoiceId"].ToString(),
                                        ReferenceInvDetailId = dr["ReferenceInvDetailId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceInvDetailId"]) : 0,
                                        IsBilling = dr["IsBilling"] != DBNull.Value ? Convert.ToBoolean(dr["IsBilling"]) : false,
                                        IsCopied = dr["IsCopied"] != DBNull.Value ? Convert.ToBoolean(dr["IsCopied"]) : false,
                                        IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                                        TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0,
                                        Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false,
                                        IsBillingProcess = dr["IsBillingProcess"] != DBNull.Value ? Convert.ToBoolean(dr["IsBillingProcess"]) : false,

                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<CustomerAppointmentEquipment> NewGetAllCustomerAppointmentEquipListByAppointmentId(Guid appointmentid, string order)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.NewGetAllCustomerAppointmentEquipmentListByAppointmentId(appointmentid, order);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        AppointmentId = (Guid)dr["AppointmentId"],
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedBy = dr["CreatedByName"].ToString(),
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        EquipmentServiceName = dr["EquipmentName"].ToString(),
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        FileDescription = dr["FileDescription"].ToString(),
                                        FileFullName = dr["FileFullName"].ToString(),
                                        FileName = dr["Filename"].ToString(),
                                        FileType = dr["FileType"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                        QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                        IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                        IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                        IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                        IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                        QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        IsInvoiceCreate = dr["IsInvoiceCreate"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceCreate"]) : false,
                                        ReferenceInvoiceId = dr["ReferenceInvoiceId"].ToString(),
                                        ReferenceInvDetailId = dr["ReferenceInvDetailId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceInvDetailId"]) : 0,
                                        IsBilling = dr["IsBilling"] != DBNull.Value ? Convert.ToBoolean(dr["IsBilling"]) : false,
                                        IsCopied = dr["IsCopied"] != DBNull.Value ? Convert.ToBoolean(dr["IsCopied"]) : false,
                                        IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                                        TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0,
                                        Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false,
                                        IsBillingProcess = dr["IsBillingProcess"] != DBNull.Value ? Convert.ToBoolean(dr["IsBillingProcess"]) : false,

                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(Guid appointmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentListByAppointmentIdForCommission(appointmentid);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        AppointmentId = (Guid)dr["AppointmentId"],
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedBy = dr["CreatedByName"].ToString(),
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        InstalledByUid = dr["InstalledByUid"] != DBNull.Value ? (Guid)dr["InstalledByUid"] : Guid.Empty,
                                        EquipmentServiceName = dr["EquipmentName"].ToString(),
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        FileDescription = dr["FileDescription"].ToString(),
                                        FileFullName = dr["FileFullName"].ToString(),
                                        FileName = dr["Filename"].ToString(),
                                        FileType = dr["FileType"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                        IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                        QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                        IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                        IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                        IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                        IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                        QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        IsInvoiceCreate = dr["IsInvoiceCreate"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceCreate"]) : false,
                                        ReferenceInvoiceId = dr["ReferenceInvoiceId"].ToString(),
                                        ReferenceInvDetailId = dr["ReferenceInvDetailId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceInvDetailId"]) : 0,
                                        IsBilling = dr["IsBilling"] != DBNull.Value ? Convert.ToBoolean(dr["IsBilling"]) : false,
                                        IsCopied = dr["IsCopied"] != DBNull.Value ? Convert.ToBoolean(dr["IsCopied"]) : false,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0
                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipListByAppointmentIdForPoint(Guid appointmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipListByAppointmentIdForPoint(appointmentid);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        AppointmentId = (Guid)dr["AppointmentId"],
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedBy = dr["CreatedByName"].ToString(),
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        InstalledByUid = dr["InstalledByUid"] != DBNull.Value ? (Guid)dr["InstalledByUid"] : Guid.Empty,
                                        EquipmentServiceName = dr["EquipmentName"].ToString(),
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        FileDescription = dr["FileDescription"].ToString(),
                                        FileFullName = dr["FileFullName"].ToString(),
                                        FileName = dr["Filename"].ToString(),
                                        FileType = dr["FileType"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                        IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                        QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                        IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                        IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                        IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                        IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                        QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        IsInvoiceCreate = dr["IsInvoiceCreate"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceCreate"]) : false,
                                        ReferenceInvoiceId = dr["ReferenceInvoiceId"].ToString(),
                                        ReferenceInvDetailId = dr["ReferenceInvDetailId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceInvDetailId"]) : 0,
                                        IsBilling = dr["IsBilling"] != DBNull.Value ? Convert.ToBoolean(dr["IsBilling"]) : false,
                                        IsCopied = dr["IsCopied"] != DBNull.Value ? Convert.ToBoolean(dr["IsCopied"]) : false,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0
                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<CustomerAppointmentEquipmentPoint> GetCustomerAppointmentEquipmentPointByAppointmentId(Guid appointmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerAppointmentEquipmentPointByAppointmentId(appointmentid);
            List<CustomerAppointmentEquipmentPoint> AppointmentEquipPointList = new List<CustomerAppointmentEquipmentPoint>();
            AppointmentEquipPointList = (from DataRow dr in dt.Rows
                                         select new CustomerAppointmentEquipmentPoint()
                                         {
                                             EmployeeName = dr["EmployeeName"].ToString(),
                                             Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                                         }).ToList();
            return AppointmentEquipPointList;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipListByCustomerId(Guid CustomerId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentByCusId(CustomerId);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        //SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        //RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        //Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        //OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        AppointmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)dr["AppointmentId"] : Guid.Empty,
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        //FileDescription = dr["FileDescription"].ToString(),



                                    }).ToList();
            return AppointmentEquipList;
        }
        public string GetCustomerNameByCompanyIdandCustomerId(Guid Companyid, Guid Customerid)
        {
            string cusname = "";
            DataTable dt = _CustomerDataAccess.GetCustomerNameByCompanyIdandCustomerId(Companyid, Customerid);
            cusname = dt.Rows[0]["customerName"].ToString();
            return cusname;
        }
        public CustomerAppointment GetCustomerWorkOrderbyCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and CompanyId = '{1}' and AppointmentType = 'WorkOrder' ", CustomerId, CompanyId)).FirstOrDefault();
        }

        public ServiceOrderMaildetails GetServiceOrderMailDetailsByAppointmentIdAndCompanyId(Guid AppointmentId, Guid CompanyId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetServiceOrderMailDetailsByAppointmentIdAndCompanyId(AppointmentId, CompanyId);
            List<ServiceOrderMaildetails> ServiceOrderMaildetailsList = new List<ServiceOrderMaildetails>();
            ServiceOrderMaildetailsList = (from DataRow dr in dt.Rows
                                           select new ServiceOrderMaildetails()
                                           {
                                               EmployeeEmail = dr["EmpEmail"].ToString(),
                                               EmployeeName = dr["EmployeeName"].ToString(),
                                               CustomerFirstName = dr["CustomerFirstName"].ToString(),
                                               CustomerLastName = dr["CustomerLastName"].ToString(),
                                               CustomerMiddleName = dr["CustomerMiddleName"].ToString(),
                                               AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                               AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                               AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                               Notes = dr["Notes"].ToString(),
                                               CreatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                               CreatedBy = dr["LastUpdatedBy"].ToString()
                                           }).ToList();
            return ServiceOrderMaildetailsList[0];
        }

        public List<CustomerAppointment> GetAllWorkOrderByCompanyId(Guid companyid)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllWorkOrderByCompanyId(companyid);
            List<CustomerAppointment> WorkList = new List<CustomerAppointment>();
            WorkList = (from DataRow dr in dt.Rows
                        select new CustomerAppointment()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            //ServicePerson = dr["ServicePerson"].ToString(),
                            AppointmentId = (Guid)dr["AppointmentId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CustomerId = (Guid)dr["CustomerId"],
                            EmployeeId = (Guid)dr["EmployeeId"],
                            AppointmentType = dr["AppointmentType"].ToString(),
                            AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                            AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                            AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                            IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                            Notes = dr["Notes"].ToString(),
                            LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                            Idstring = "Product Installation",
                            Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false
                        }).ToList();
            return WorkList;
        }

        public List<CustomScheduleCaneldar> GetAllScheduleByComoanyIdandCustomerId(Guid CompanyId, Guid EmployeeId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetAllScheduleByCompanyIdAndCustomerId(CompanyId, EmployeeId);
            List<CustomScheduleCaneldar> SchedualList = new List<CustomScheduleCaneldar>();
            SchedualList = (from DataRow dr in dt.Rows
                            select new CustomScheduleCaneldar()
                            {
                                inedtity = dr["inedtity"].ToString(),
                                EventCustomerId = dr["EventCustomerId"].ToString(),
                                EventType = dr["EventType"].ToString(),
                                EventStartDate = dr["EventStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventStartDate"]) : new DateTime(),
                                EventEndDate = dr["EventEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventEndDate"]) : new DateTime(),
                                EventName = dr["EventName"].ToString(),
                                EventLeadId = dr["EventLeadId"] != DBNull.Value ? Convert.ToInt32(dr["EventLeadId"]) : 0
                            }).ToList();
            return SchedualList;
        }


        public Guid GetCustomerLastAppointmentIdByCustomerIdAndCompanyId(Guid CusId, Guid ComId)
        {
            var AppoinmentId = new Guid();
            var result = _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' order by Id desc", CusId, ComId)).FirstOrDefault();
            if (result != null)
            {
                AppoinmentId = result.AppointmentId;
            }
            return AppoinmentId;
        }
        public bool DeleteCustomerAppoinmentEquipmentByTicketId(Guid TicketId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteCustomerAppoinmentEquipmentByTicketId(TicketId);
            return result;
        }
        public bool DeleteCustomerAppoinmentEquipmentByTicketIdEquipment(Guid TicketId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteCustomerAppoinmentEquipmentByTicketIdEquipment(TicketId);
            return result;
        }
        public bool DeleteCustomerAppoinmentEquipmentByTicketIdService(Guid TicketId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteCustomerAppoinmentEquipmentByTicketIdService(TicketId);
            return result;
        }
        public bool DeleteAllCustomerAppointmentEquipmentByAppointmentId(Guid AppointmentId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteAllCustomerAppointmentEquipmentByAppointmentId(AppointmentId);
            return result;
        }
        public bool DeleteAllCustomerAppointmentServiceByAppointmentId(Guid AppointmentId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteAllCustomerAppointmentServiceByAppointmentId(AppointmentId);
            return result;
        }
        public Customer GetCustomerIdByCustomerAppointmentCustomerId(Guid customerid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }

        public Employee GteEmployeeNameByCustomerAppointmentEmployeeId(Guid employeeid)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GteEmployeeNameByCustomerAppointmentEmployeeId(employeeid);
            Employee EmpNameList = new Employee();
            EmpNameList = (from DataRow dr in dt.Rows
                           select new Employee()
                           {
                               EMPName = dr["EMPName"].ToString()
                           }).ToList().FirstOrDefault();
            return EmpNameList;
        }

        public int GetScheduleIdByCustomerAppointmentId(Guid appid)
        {
            int result = 0;
            DataTable dt = _CustomerAppoinmentDataAccess.GetScheduleIdByCustomerAppointmentId(appid);
            Schedule SchedualList = new Schedule();
            SchedualList = (from DataRow dr in dt.Rows
                            select new Schedule()
                            {
                                Id = dr["scheduleid"] != DBNull.Value ? Convert.ToInt32(dr["scheduleid"]) : 0
                            }).FirstOrDefault();
            if (SchedualList != null)
            {
                result = SchedualList.Id;
            }
            return result;
        }

        public List<CusEquipmentList> GetCustomerAppointmentEquipmentForTechCall(Guid AppointmentId, Guid CustomerId)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetCustomerAppointmentEquipmentByCustomerIdAndAppointmentId(AppointmentId, CustomerId);
            List<CusEquipmentList> CustomerEquipmentList = new List<CusEquipmentList>();
            CustomerEquipmentList = (from DataRow dr in dt.Rows
                                     select new CusEquipmentList()
                                     {
                                         EquipmentName = dr["Name"].ToString(),
                                         EquipmentQuantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                         EquipmentUnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                         YourPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,

                                     }).ToList();
            return CustomerEquipmentList;
        }

        public CustomerAppointment GetAllAppointmentByCustomerId(Guid cusid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).FirstOrDefault();
        }

        public CustomerAppointment GetAppointmentIdByEmployeeId(Guid employeeid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("EmployeeId = '{0}'", employeeid)).FirstOrDefault();
        }

        public CustomerAppointment GetCustomerIdByAppointmentIdAndAppointmentType(Guid appointid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and AppointmentType = 'WorkOrder'", appointid)).FirstOrDefault();
        }

        public CustomerAppointment GetServiceCustomerIdByAppointmentIdAndAppointmentType(Guid appointid)
        {
            return _CustomerAppoinmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and AppointmentType = 'ServiceOrder'", appointid)).FirstOrDefault();
        }
        public List<DashboardServiceBoardListViewModel> GetDashBoardServiceBoardList(Guid CompanyId, DateTime StartTime, DateTime EndTime)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetDashBoardServiceBoardList(CompanyId, StartTime, EndTime);
            List<DashboardServiceBoardListViewModel> ServiceList = new List<DashboardServiceBoardListViewModel>();
            if (dt != null)
            {
                ServiceList = (from DataRow dr in dt.Rows
                               select new DashboardServiceBoardListViewModel()
                               {
                                   AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                   AppointmentTime = dr["AppointmentTime"].ToString(),
                                   CustomerFirstName = dr["CustomerFirstName"].ToString(),
                                   CustomerMiddleName = dr["CustomerMiddleName"].ToString(),
                                   CustomerLastName = dr["CustomerLastName"].ToString(),
                                   EmployeeName = dr["EmployeeName"].ToString(),
                                   CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                   CustomerGuidId = dr["CustomerGuidId"] != DBNull.Value ? (Guid)dr["CustomerGuidId"] : new Guid(),
                                   ServiceOrderId = dr["ServiceOrderId"] != DBNull.Value ? (Guid)dr["ServiceOrderId"] : new Guid()
                               }).ToList();
            }
            return ServiceList;
        }

        public List<DashboardInstallationListViewModel> GetDashBoardInstallationList(Guid CompanyId, DateTime StartTime, DateTime EndTime)
        {
            DataTable dt = _CustomerAppoinmentDataAccess.GetDashBoardInstallationList(CompanyId, StartTime, EndTime);
            List<DashboardInstallationListViewModel> ServiceList = new List<DashboardInstallationListViewModel>();
            if (dt != null)
            {
                ServiceList = (from DataRow dr in dt.Rows
                               select new DashboardInstallationListViewModel()
                               {
                                   AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                   AppointmentTime = dr["AppointmentTime"].ToString(),
                                   CustomerFirstName = dr["CustomerFirstName"].ToString(),
                                   CustomerMiddleName = dr["CustomerMiddleName"].ToString(),
                                   CustomerLastName = dr["CustomerLastName"].ToString(),
                                   EmployeeName = dr["EmployeeName"].ToString(),
                                   CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                   CustomerGuidId = dr["CustomerGuidId"] != DBNull.Value ? (Guid)dr["CustomerGuidId"] : new Guid(),
                                   ServiceOrderId = dr["ServiceOrderId"] != DBNull.Value ? (Guid)dr["ServiceOrderId"] : new Guid()
                               }).ToList();
            }
            return ServiceList;
        }

        public long InsertAppointmentEquipment(CustomerAppointmentEquipment cae)
        {
            return _CustomerAppointmentEquipmentDataAccess.Insert(cae);
        }

        public int InsertCustomerAppointmentEquipment(CustomerAppointmentEquipment cae)
        {
            return (int)_CustomerAppointmentEquipmentDataAccess.Insert(cae);
        }

        public List<LeadEquipmentDetail> GetAllLeadEquipmentDetailByLeadIdandCompanyId(Guid companyid, int id)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllLeadEquipmentDetailByLeadIdandCompanyId(companyid, id);
            List<LeadEquipmentDetail> Pdetail = new List<LeadEquipmentDetail>();
            Pdetail = (from DataRow dr in dt.Rows
                       select new LeadEquipmentDetail()
                       {
                           LeadEquipmentName = dr["LeadEquipmentName"].ToString(),
                           LeadEquipmentQuantity = dr["LeadEquipmentQuantity"].ToString(),
                           LeadEquipmentPrice = dr["LeadEquipmentPrice"].ToString()
                       }).ToList();
            return Pdetail;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipmentByTicketId(Guid companyid, Guid TicketId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentByTicketId(companyid, TicketId);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    AppointmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)dr["AppointmentId"] : Guid.Empty,
                                                    EquipmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : Guid.Empty,
                                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                    UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                                    OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                                    TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                                    QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                    CreatedBy = dr["CreatedBy"].ToString(),
                                                    EquipName = dr["EquipName"].ToString(),
                                                    EquipDetail = dr["EquipDetail"].ToString(),
                                                    IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                                    IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                                    CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                                    InstalledByUid = dr["InstalledByUid"] != DBNull.Value ? (Guid)dr["InstalledByUid"] : Guid.Empty,
                                                    IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                                    IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                                    IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                                    IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                                    IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                                    IsNonCommissionable = dr["IsNonCommissionable"] != DBNull.Value ? Convert.ToBoolean(dr["IsNonCommissionable"]) : false,
                                                    IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }
        public CustomerAppointmentEquipment GetAppoinmentEquipmentByIdAppoinmentIdAndEquipmentId(int id, Guid appoinmentId, Guid equipmentId)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("Id = {0} and EquipmentId = '{1}' and AppointmentId ='{2}'", id, equipmentId, appoinmentId)).FirstOrDefault();
        }
        public CustomerAppointmentEquipment GetAppoinmentEquipmentByAppoinmentIdAndEquipmentId(Guid appoinmentId, Guid equipmentId)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and AppointmentId ='{1}'", equipmentId, appoinmentId)).FirstOrDefault();
        }

        public CustomerAppointmentEquipment GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(Guid appoinmentId, Guid equipmentId, int Id)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and AppointmentId ='{1}' and Id = {2}", equipmentId, appoinmentId, Id)).FirstOrDefault();
        }
        public bool DeleteCustomerAppoinmentEquipment(int id)
        {
            return _CustomerAppointmentEquipmentDataAccess.Delete(id) > 0;
        }

        public List<CustomerAppointmentEquipment> GetAllAppointmentEquipmentByEquipmentId(Guid equipid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", equipid)).ToList();
        }
        public List<CustomerAppointmentEquipment> GetAllAppointmentEquipmentByAppointmentIdandEquipmentId(Guid equipid, Guid AppointmentId)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format(" AppointmentId ='{1}' AND EquipmentId = '{0}'", equipid, AppointmentId)).ToList();
        }
        public long InsertAppointmentTechnician(CustomerAppointmentTechnician cat)
        {
            return _CustomerAppointmentTechnicianDataAccess.Insert(cat);
        }

        public List<CustomerAppointmentTechnician> GetAllAppointmentTechnicianByAppointmentId(int id)
        {
            return _CustomerAppointmentTechnicianDataAccess.GetByQuery(string.Format("CustomerAppointmentId = '{0}'", id)).ToList();
        }

        public void DeleteAppointmentTech(int value)
        {
            _CustomerAppointmentTechnicianDataAccess.Delete(value);
        }
        public bool DeleteAppointmentAllTech(int AppId)
        {
            return _CustomerAppointmentTechnicianDataAccess.DeleteAppointmentAllTech(AppId);
        }

        public CustomerAppointmentEquipment GetCustomerAppointmentEquipmentByAppointmentIdAndEquipmentId(Guid appid, Guid eqpid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and EquipmentId = '{1}'", appid, eqpid)).FirstOrDefault();
        }
        public CustomerAppointmentEquipment GetCustomerAppointmentEquipmentByAppointmentIdAndEquipmentIdAndId(Guid appid, Guid eqpid, int Id)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and EquipmentId = '{1}' and Id = '{2}'", appid, eqpid, Id)).FirstOrDefault();
        }
        public CustomerAppointmentEquipment GetCustomerAppointmentEquipmentById(int value)
        {
            return _CustomerAppointmentEquipmentDataAccess.Get(value);
        }

        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(Guid appointmentid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsBilling = 1 and IsService = 1", appointmentid)).ToList();
        }

        public List<CustomerAppointmentEquipment> GetCAEListByTicketIdUserId(Guid ticketId, Guid UserId, bool withNonCommission, string Type,int CommissionIntId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCAEListByTicketIdUserId(ticketId, UserId, withNonCommission, Type, CommissionIntId);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    EquipName = dr["EquipName"].ToString(),
                                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }
        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentListByAppointmentId(Guid appointid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", appointid)).ToList();
        }
        public bool UpdateSoldByCustomerAppointmentEqp(Guid AppointmentId, string CreatedByUid)
        {
            return _CustomerAppointmentEquipmentDataAccess.UpdateSoldByCustomerAppointmentEqp(AppointmentId, CreatedByUid);
        }
        public bool UpdateInstalledByCustomerAppointmentEqp(Guid AppointmentId, Guid InstalledByUid)
        {
            return _CustomerAppointmentEquipmentDataAccess.UpdateInstalledByCustomerAppointmentEqp(AppointmentId, InstalledByUid);
        }
        public List<CustomerAppointmentEquipment> GetAllTicketEquipmentsDetailByCustomerIdAndCompanyId(Guid companyid, Guid customerid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllTicketEquipmentsDetailByCustomerIdAndCompanyId(companyid, customerid);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    EquipmentId = (Guid)dr["EquipmentId"],
                                                    EquipName = dr["EquipName"].ToString(),
                                                    QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                                    //TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }

        public List<CustomerAppointmentEquipment> GetAllTicketServicesDetailByCustomerIdAndCompanyId(Guid companyid, Guid customerid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllTicketServicesDetailByCustomerIdAndCompanyId(companyid, customerid);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    EquipmentId = (Guid)dr["EquipmentId"],
                                                    EquipName = dr["EquipName"].ToString(),
                                                    TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                                    //TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }

        public List<CustomerAppointmentEquipment> GetTicketEquipmentsDetailByEqpID(Guid companyid, Guid customerid, Guid EqpId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetTicketEquipmentsDetailByCustomerIdAndCompanyIdAndEqpId(companyid, customerid, EqpId);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    EquipName = dr["EquipName"].ToString(),
                                                    QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                                    TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }

        public BillingCheckModel GetBillingCheckCustomerAppointmentEquipmentByEquipmentIdAndCustomerId(Guid customerid, Guid equipmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetBillingCheckCustomerAppointmentEquipmentByEquipmentIdAndCustomerId(customerid, equipmentid);
            BillingCheckModel model = new BillingCheckModel();
            model = (from DataRow dr in dt.Rows
                     select new BillingCheckModel()
                     {
                         IsBillingCheck = dr["IsBillingCheck"] != DBNull.Value ? Convert.ToBoolean(dr["IsBillingCheck"]) : false,
                         BillEquipmentId = (Guid)dr["BillEquipmentId"]
                     }).FirstOrDefault();
            return model;
        }

        public AdditionalMembersAppointment GetAdditionalMembersAppointmentByAppointmentIdAndUserId(Guid appointid, Guid empid)
        {
            return _AdditionalMembersAppointmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and EmployeeId = '{1}' order by Id desc", appointid, empid)).FirstOrDefault();
        }

        public bool UpdateAdditionalMembersAppointment(AdditionalMembersAppointment amp)
        {
            return _AdditionalMembersAppointmentDataAccess.Update(amp) > 0;
        }

        public bool DeleteAdditionalMembersAppointment(int value)
        {
            return _AdditionalMembersAppointmentDataAccess.Delete(value) > 0;
        }

        public long InsertAdditionalMembersAppointment(AdditionalMembersAppointment amp)
        {
            return _AdditionalMembersAppointmentDataAccess.Insert(amp);
        }

        public bool DeleteAdditionalMembersAppointmentByAppointmentId(Guid appointid)
        {
            return _CustomerAppoinmentDataAccess.DeleteAdditionalMembersAppointmentByAppointmentId(appointid);
        }

        public List<AdditionalMembersAppointment> GetAllAdditionalMembersAppointmentByAppointmentId(Guid appointid)
        {
            return _AdditionalMembersAppointmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", appointid)).ToList();
        }

        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(Guid appid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsBilling = 1", appid)).ToList();
        }

        public List<CustomerAppointmentEquipment> GetCustomerServiceListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerServiceListByCustomerIdAndCompanyId(customerid, companyid);
            List<CustomerAppointmentEquipment> model = new List<CustomerAppointmentEquipment>();
            model = (from DataRow dr in dt.Rows
                     select new CustomerAppointmentEquipment()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         AppointmentId = (Guid)dr["AppointmentId"],
                         EquipmentId = (Guid)dr["EquipmentId"],
                         Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                         UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                         TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                         EquipName = dr["EquipName"].ToString(),
                     }).ToList();
            return model;
        }

        public List<CustomerAppointmentEquipment> GetCustomerEquipmentListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerEquipmentListByCustomerIdAndCompanyId(customerid, companyid);
            List<CustomerAppointmentEquipment> model = new List<CustomerAppointmentEquipment>();
            model = (from DataRow dr in dt.Rows
                     select new CustomerAppointmentEquipment()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         AppointmentId = (Guid)dr["AppointmentId"],
                         EquipmentId = (Guid)dr["EquipmentId"],
                         Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                         UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                         TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                         EquipName = dr["EquipName"].ToString(),
                     }).ToList();
            return model;
        }
        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentByAppointmentId(Guid appointmentid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' ", appointmentid)).ToList();
        }
        public EquipmentType GetEquipmentTypeByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetEquipmentTypeByAppointmentIdAndEquipmentId(AppointmentId, EquipmentId);
            EquipmentType model = new EquipmentType();
            model = (from DataRow dr in dt.Rows
                     select new EquipmentType()
                     {
                         Name = dr["Name"].ToString(),
                     }).FirstOrDefault();
            return model;
        }
        public Manufacturer GetManufacturerByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetManufacturerByAppointmentIdAndEquipmentId(AppointmentId, EquipmentId);
            Manufacturer model = new Manufacturer();
            model = (from DataRow dr in dt.Rows
                     select new Manufacturer()
                     {
                         Name = dr["Name"].ToString(),
                     }).FirstOrDefault();
            return model;
        }
        public InstallerModel GetInstallerByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetInstallerTypeByAppointmentIdAndEquipmentId(AppointmentId, EquipmentId);
            InstallerModel model = new InstallerModel();
            model = (from DataRow dr in dt.Rows
                     select new InstallerModel()
                     {
                         Id= (Guid)dr["InstalledByUid"],
                         Name = dr["Name"].ToString(),
                     }).FirstOrDefault();
            return model;
        }
        public CompanyCost GetCompanyCostByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCompanyCostByEquipmentId(AppointmentId, EquipmentId);
            CompanyCost model = new CompanyCost();
            if (dt.Rows.Count > 0)
            {
                model = (from DataRow dr in dt.Rows
                         select new CompanyCost()
                         {
                             Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0
                         }).FirstOrDefault();
            }
            else
            {
                model.Cost = 0.0;
            }
            return model;
        }
        public EquipmentSKU GetEquipmentSKUByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetEquipmentSKUAndPointByAppointmentIdAndEquipmentId(AppointmentId, EquipmentId);
            EquipmentSKU model = new EquipmentSKU();
            model = (from DataRow dr in dt.Rows
                     select new EquipmentSKU()
                     {
                         Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                         SKU = dr["SKU"].ToString()
                     }).FirstOrDefault();
            return model;
        }
        public Customer GetCustomerByAppointmentId(Guid AppointmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerByAppointmentId(AppointmentId);
            Customer model = new Customer();
            model = (from DataRow dr in dt.Rows
                     select new Customer()
                     {
                         Name = dr["Name"].ToString(),
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                     }).FirstOrDefault();
            return model;
        }
        public TicketType GetTicketTypeByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetTicketTypeByAppointmentIdAndEquipmentId(AppointmentId, EquipmentId);
            TicketType model = new TicketType();
            model = (from DataRow dr in dt.Rows
                     select new TicketType()
                     {
                         Type = dr["TicketType"].ToString(),
                     }).FirstOrDefault();
            return model;
        }
        public Count GetAttachmentsCountAppointmentId(Guid AppointmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAttachmentsCountByAppointmentId(AppointmentId);
            Count model = new Count();
            model = (from DataRow dr in dt.Rows
                     select new Count()
                     {
                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            return model;
        }
        public Count GetRepliesCountAppointmentId(Guid AppointmentId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetRepliesCountByAppointmentId(AppointmentId);
            Count model = new Count();
            model = (from DataRow dr in dt.Rows
                     select new Count()
                     {
                         TotalCount = dr["Total"] != DBNull.Value ? Convert.ToInt32(dr["Total"]) : 0
                     }).FirstOrDefault();
            return model;
        }
    }
}
