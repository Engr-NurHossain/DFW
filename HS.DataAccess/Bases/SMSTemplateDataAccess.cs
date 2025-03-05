using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
    public partial class SMSTemplateDataAccess : BaseDataAccess
    {
        #region Constants
        private const string INSERTSMSTEMPLATE = "InsertSMSTemplate";
        private const string UPDATESMSTEMPLATE = "UpdateSMSTemplate";
        private const string DELETESMSTEMPLATE = "DeleteSMSTemplate";
        private const string GETSMSTEMPLATEBYID = "GetSMSTemplateById";
        private const string GETALLSMSTEMPLATE = "GetAllSMSTemplate";
        private const string GETPAGEDSMSTEMPLATE = "GetPagedSMSTemplate";
        private const string GETSMSTEMPLATEMAXIMUMID = "GetSMSTemplateMaximumId";
        private const string GETSMSTEMPLATEROWCOUNT = "GetSMSTemplateRowCount";
        private const string GETSMSTEMPLATEBYQUERY = "GetSMSTemplateByQuery";
        #endregion

        #region Constructors
        public SMSTemplateDataAccess(string ConStr) : base(ConStr) { }
        public SMSTemplateDataAccess(ClientContext context) : base(context) { }
        public SMSTemplateDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion

        #region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="sMSTemplateObject"></param>
        private void AddCommonParams(SqlCommand cmd, SMSTemplateBase sMSTemplateObject)
        {
            AddParameter(cmd, pGuid(SMSTemplateBase.Property_CompanyId, sMSTemplateObject.CompanyId));
            AddParameter(cmd, pNVarChar(SMSTemplateBase.Property_TemplateKey, 150, sMSTemplateObject.TemplateKey));
            AddParameter(cmd, pNVarChar(SMSTemplateBase.Property_Name, 150, sMSTemplateObject.Name));
            AddParameter(cmd, pNVarChar(SMSTemplateBase.Property_Description, sMSTemplateObject.Description));
            AddParameter(cmd, pNVarChar(SMSTemplateBase.Property_ToNumber, 150, sMSTemplateObject.ToNumber));
            AddParameter(cmd, pNVarChar(SMSTemplateBase.Property_Body, sMSTemplateObject.Body));
            AddParameter(cmd, pBool(SMSTemplateBase.Property_IsActive, sMSTemplateObject.IsActive));
            AddParameter(cmd, pGuid(SMSTemplateBase.Property_LastUpdatedBy, sMSTemplateObject.LastUpdatedBy));
            AddParameter(cmd, pDateTime(SMSTemplateBase.Property_LastUpdatedDate, sMSTemplateObject.LastUpdatedDate));
        }
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts SMSTemplate
        /// </summary>
        /// <param name="sMSTemplateObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
        public long Insert(SMSTemplateBase sMSTemplateObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(INSERTSMSTEMPLATE);

                AddParameter(cmd, pInt32Out(SMSTemplateBase.Property_Id));
                AddCommonParams(cmd, sMSTemplateObject);

                long result = InsertRecord(cmd);
                if (result > 0)
                {
                    sMSTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                    sMSTemplateObject.Id = (Int32)GetOutParameter(cmd, SMSTemplateBase.Property_Id);
                }
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectInsertException(sMSTemplateObject, x);
            }
        }
        #endregion

        #region Update Method
        /// <summary>
        /// Updates SMSTemplate
        /// </summary>
        /// <param name="sMSTemplateObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
        public long Update(SMSTemplateBase sMSTemplateObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(UPDATESMSTEMPLATE);

                AddParameter(cmd, pInt32(SMSTemplateBase.Property_Id, sMSTemplateObject.Id));
                AddCommonParams(cmd, sMSTemplateObject);

                long result = UpdateRecord(cmd);
                if (result > 0)
                    sMSTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectUpdateException(sMSTemplateObject, x);
            }
        }
        #endregion

        #region Delete Method
        /// <summary>
        /// Deletes SMSTemplate
        /// </summary>
        /// <param name="Id">Id of the SMSTemplate object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
        public long Delete(Int32 _Id)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(DELETESMSTEMPLATE);

                AddParameter(cmd, pInt32(SMSTemplateBase.Property_Id, _Id));

                return DeleteRecord(cmd);
            }
            catch (SqlException x)
            {
                throw new ObjectDeleteException(typeof(SMSTemplate), _Id, x);
            }

        }
        #endregion

        #region Get By Id Method
        /// <summary>
        /// Retrieves SMSTemplate object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SMSTemplate object to retrieve</param>
        /// <returns>SMSTemplate object, null if not found</returns>
        public SMSTemplate Get(Int32 _Id)
        {
            using (SqlCommand cmd = GetSPCommand(GETSMSTEMPLATEBYID))
            {
                AddParameter(cmd, pInt32(SMSTemplateBase.Property_Id, _Id));

                return GetObject(cmd);
            }
        }
        #endregion

        #region GetAll Method
        /// <summary>
        /// Retrieves all SMSTemplate objects 
        /// </summary>
        /// <returns>A list of SMSTemplate objects</returns>
        public SMSTemplateList GetAll()
        {
            using (SqlCommand cmd = GetSPCommand(GETALLSMSTEMPLATE))
            {
                return GetList(cmd, ALL_AVAILABLE_RECORDS);
            }
        }


        /// <summary>
        /// Retrieves all SMSTemplate objects by PageRequest
        /// </summary>
        /// <returns>A list of SMSTemplate objects</returns>
        public SMSTemplateList GetPaged(PagedRequest request)
        {
            using (SqlCommand cmd = GetSPCommand(GETPAGEDSMSTEMPLATE))
            {
                AddParameter(cmd, pInt32Out("TotalRows"));
                AddParameter(cmd, pInt32("PageIndex", request.PageIndex));
                AddParameter(cmd, pInt32("RowPerPage", request.RowPerPage));
                AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause));
                AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn));
                AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder));

                SMSTemplateList _SMSTemplateList = GetList(cmd, ALL_AVAILABLE_RECORDS);
                request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
                return _SMSTemplateList;
            }
        }

        /// <summary>
        /// Retrieves all SMSTemplate objects by query String
        /// </summary>
        /// <returns>A list of SMSTemplate objects</returns>
        public SMSTemplateList GetByQuery(String query)
        {
            using (SqlCommand cmd = GetSPCommand(GETSMSTEMPLATEBYQUERY))
            {
                AddParameter(cmd, pNVarChar("Query", 4000, query));
                return GetList(cmd, ALL_AVAILABLE_RECORDS); ;
            }
        }

        #endregion


        #region Get SMSTemplate Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of SMSTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
        {
            Int32 _MaximumId = 0;
            using (SqlCommand cmd = GetSPCommand(GETSMSTEMPLATEMAXIMUMID))
            {
                SqlDataReader reader;
                _MaximumId = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _MaximumId;
        }

        #endregion

        #region Get SMSTemplate Row Count Method
        /// <summary>
        /// Retrieves Get Total Rows of SMSTemplate
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetRowCount()
        {
            Int32 _SMSTemplateRowCount = 0;
            using (SqlCommand cmd = GetSPCommand(GETSMSTEMPLATEROWCOUNT))
            {
                SqlDataReader reader;
                _SMSTemplateRowCount = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _SMSTemplateRowCount;
        }

        #endregion

        #region Fill Methods
        /// <summary>
        /// Fills SMSTemplate object
        /// </summary>
        /// <param name="sMSTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
        protected void FillObject(SMSTemplateBase sMSTemplateObject, SqlDataReader reader, int start)
        {

            sMSTemplateObject.Id = reader.GetInt32(start + 0);
            sMSTemplateObject.CompanyId = reader.GetGuid(start + 1);
            sMSTemplateObject.TemplateKey = reader.GetString(start + 2);
            sMSTemplateObject.Name = reader.GetString(start + 3);
            sMSTemplateObject.Description = reader.GetString(start + 4);
            sMSTemplateObject.ToNumber = reader.GetString(start + 5);
            sMSTemplateObject.Body = reader.GetString(start + 6);
            sMSTemplateObject.IsActive = reader.GetBoolean(start + 7);
            sMSTemplateObject.LastUpdatedBy = reader.GetGuid(start + 8);
            sMSTemplateObject.LastUpdatedDate = reader.GetDateTime(start + 9);
            FillBaseObject(sMSTemplateObject, reader, (start + 10));


            sMSTemplateObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
        }

        /// <summary>
        /// Fills SMSTemplate object
        /// </summary>
        /// <param name="sMSTemplateObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        protected void FillObject(SMSTemplateBase sMSTemplateObject, SqlDataReader reader)
        {
            FillObject(sMSTemplateObject, reader, 0);
        }

        /// <summary>
        /// Retrieves SMSTemplate object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SMSTemplate object</returns>
        private SMSTemplate GetObject(SqlCommand cmd)
        {
            SqlDataReader reader;
            long rows = SelectRecords(cmd, out reader);

            using (reader)
            {
                if (reader.Read())
                {
                    SMSTemplate sMSTemplateObject = new SMSTemplate();
                    FillObject(sMSTemplateObject, reader);
                    return sMSTemplateObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves list of SMSTemplate objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SMSTemplate objects</returns>
        private SMSTemplateList GetList(SqlCommand cmd, long rows)
        {
            // Select multiple records
            SqlDataReader reader;
            long result = SelectRecords(cmd, out reader);

            //SMSTemplate list
            SMSTemplateList list = new SMSTemplateList();

            using (reader)
            {
                // Read rows until end of result or number of rows specified is reached
                while (reader.Read() && rows-- != 0)
                {
                    SMSTemplate sMSTemplateObject = new SMSTemplate();
                    FillObject(sMSTemplateObject, reader);

                    list.Add(sMSTemplateObject);
                }

                // Close the reader in order to receive output parameters
                // Output parameters are not available until reader is closed.
                reader.Close();
            }

            return list;
        }

        #endregion
    }
}