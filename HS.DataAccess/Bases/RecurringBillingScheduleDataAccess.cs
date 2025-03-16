using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using System;
using System.Data.SqlClient;

namespace HS.DataAccess
{
    public partial class RecurringBillingScheduleDataAccess : BaseDataAccess
    {
        #region Constants
        private const string INSERTRECURRINGBILLINGSCHEDULE = "InsertRecurringBillingSchedule";
        private const string UPDATERECURRINGBILLINGSCHEDULE = "UpdateRecurringBillingSchedule";
        private const string DELETERECURRINGBILLINGSCHEDULE = "DeleteRecurringBillingSchedule";
        private const string GETRECURRINGBILLINGSCHEDULEBYID = "GetRecurringBillingScheduleById";
        private const string GETALLRECURRINGBILLINGSCHEDULE = "GetAllRecurringBillingSchedule";
        private const string GETPAGEDRECURRINGBILLINGSCHEDULE = "GetPagedRecurringBillingSchedule";
        private const string GETRECURRINGBILLINGSCHEDULEMAXIMUMID = "GetRecurringBillingScheduleMaximumId";
        private const string GETRECURRINGBILLINGSCHEDULEROWCOUNT = "GetRecurringBillingScheduleRowCount";
        private const string GETRECURRINGBILLINGSCHEDULEBYQUERY = "GetRecurringBillingScheduleByQuery";
        #endregion

        #region Constructors
        public RecurringBillingScheduleDataAccess(ClientContext context) : base(context) { }
        public RecurringBillingScheduleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion

        #region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recurringBillingScheduleObject"></param>
        private void AddCommonParams(SqlCommand cmd, RecurringBillingScheduleBase recurringBillingScheduleObject)
        {
            AddParameter(cmd, pGuid(RecurringBillingScheduleBase.Property_ScheduleId, recurringBillingScheduleObject.ScheduleId));
            AddParameter(cmd, pGuid(RecurringBillingScheduleBase.Property_CompanyId, recurringBillingScheduleObject.CompanyId));
            AddParameter(cmd, pGuid(RecurringBillingScheduleBase.Property_CustomerId, recurringBillingScheduleObject.CustomerId));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_TemplateName, 200, recurringBillingScheduleObject.TemplateName));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_EmailAddress, 200, recurringBillingScheduleObject.EmailAddress));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_CCEmail, 200, recurringBillingScheduleObject.CCEmail));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_BCCEmail, 200, recurringBillingScheduleObject.BCCEmail));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_Status, 50, recurringBillingScheduleObject.Status));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_AutomaticallySendEmail, recurringBillingScheduleObject.AutomaticallySendEmail));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IncludeOpenInvoices, recurringBillingScheduleObject.IncludeOpenInvoices));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_CollectOnline, recurringBillingScheduleObject.CollectOnline));
            AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_CustomerPaymentProfileId, recurringBillingScheduleObject.CustomerPaymentProfileId));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_BillCycle, 50, recurringBillingScheduleObject.BillCycle));
            AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_Interval, recurringBillingScheduleObject.Interval));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_StartDate, recurringBillingScheduleObject.StartDate));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_EndDate, recurringBillingScheduleObject.EndDate));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_BillingAddress, recurringBillingScheduleObject.BillingAddress));
            AddParameter(cmd, pDouble(RecurringBillingScheduleBase.Property_BillAmount, recurringBillingScheduleObject.BillAmount));
            AddParameter(cmd, pDouble(RecurringBillingScheduleBase.Property_TaxPercentage, recurringBillingScheduleObject.TaxPercentage));
            AddParameter(cmd, pDouble(RecurringBillingScheduleBase.Property_TaxAmount, recurringBillingScheduleObject.TaxAmount));
            AddParameter(cmd, pDouble(RecurringBillingScheduleBase.Property_TotalBillAmount, recurringBillingScheduleObject.TotalBillAmount));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_MessageOnInvoice, 1000, recurringBillingScheduleObject.MessageOnInvoice));
            AddParameter(cmd, pGuid(RecurringBillingScheduleBase.Property_CreatedBy, recurringBillingScheduleObject.CreatedBy));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_CreatedDate, recurringBillingScheduleObject.CreatedDate));
            AddParameter(cmd, pGuid(RecurringBillingScheduleBase.Property_LastUpdatedBy, recurringBillingScheduleObject.LastUpdatedBy));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_LastUpdatedDate, recurringBillingScheduleObject.LastUpdatedDate));
            AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_DayInAdvance, recurringBillingScheduleObject.DayInAdvance));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_PreviousDate, recurringBillingScheduleObject.PreviousDate));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_NextDate, recurringBillingScheduleObject.NextDate));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_OthersUnpaidBill, recurringBillingScheduleObject.OthersUnpaidBill));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_PaymentMethod, 50, recurringBillingScheduleObject.PaymentMethod));
            AddParameter(cmd, pDateTime(RecurringBillingScheduleBase.Property_PaymentCollectionDate, recurringBillingScheduleObject.PaymentCollectionDate));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsEInvoice, recurringBillingScheduleObject.IsEInvoice));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsEReceipt, recurringBillingScheduleObject.IsEReceipt));
            AddParameter(cmd, pNVarChar(RecurringBillingScheduleBase.Property_LastRMRInvoiceRefId, 50, recurringBillingScheduleObject.LastRMRInvoiceRefId));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsReplacement, recurringBillingScheduleObject.IsReplacement));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsTransfer, recurringBillingScheduleObject.IsTransfer));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsFCReplacement, recurringBillingScheduleObject.IsFCReplacement));
            AddParameter(cmd, pBool(RecurringBillingScheduleBase.Property_IsPOO, recurringBillingScheduleObject.IsPOO));
        }
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts RecurringBillingSchedule
        /// </summary>
        /// <param name="recurringBillingScheduleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
        public long Insert(RecurringBillingScheduleBase recurringBillingScheduleObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(INSERTRECURRINGBILLINGSCHEDULE);

                AddParameter(cmd, pInt32Out(RecurringBillingScheduleBase.Property_Id));
                AddCommonParams(cmd, recurringBillingScheduleObject);

                long result = InsertRecord(cmd);
                if (result > 0)
                {
                    recurringBillingScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                    recurringBillingScheduleObject.Id = (Int32)GetOutParameter(cmd, RecurringBillingScheduleBase.Property_Id);
                }
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectInsertException(recurringBillingScheduleObject, x);
            }
        }
        #endregion

        #region Update Method
        /// <summary>
        /// Updates RecurringBillingSchedule
        /// </summary>
        /// <param name="recurringBillingScheduleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
        public long Update(RecurringBillingScheduleBase recurringBillingScheduleObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(UPDATERECURRINGBILLINGSCHEDULE);

                AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_Id, recurringBillingScheduleObject.Id));
                AddCommonParams(cmd, recurringBillingScheduleObject);

                long result = UpdateRecord(cmd);
                if (result > 0)
                    recurringBillingScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectUpdateException(recurringBillingScheduleObject, x);
            }
        }
        #endregion

        #region Delete Method
        /// <summary>
        /// Deletes RecurringBillingSchedule
        /// </summary>
        /// <param name="Id">Id of the RecurringBillingSchedule object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
        public long Delete(Int32 _Id)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(DELETERECURRINGBILLINGSCHEDULE);

                AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_Id, _Id));

                return DeleteRecord(cmd);
            }
            catch (SqlException x)
            {
                throw new ObjectDeleteException(typeof(RecurringBillingSchedule), _Id, x);
            }

        }
        #endregion

        #region Get By Id Method
        /// <summary>
        /// Retrieves RecurringBillingSchedule object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecurringBillingSchedule object to retrieve</param>
        /// <returns>RecurringBillingSchedule object, null if not found</returns>
        public RecurringBillingSchedule Get(Int32 _Id)
        {
            using (SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEBYID))
            {
                AddParameter(cmd, pInt32(RecurringBillingScheduleBase.Property_Id, _Id));

                return GetObject(cmd);
            }
        }
        #endregion

        #region GetAll Method
        /// <summary>
        /// Retrieves all RecurringBillingSchedule objects 
        /// </summary>
        /// <returns>A list of RecurringBillingSchedule objects</returns>
        public RecurringBillingScheduleList GetAll()
        {
            using (SqlCommand cmd = GetSPCommand(GETALLRECURRINGBILLINGSCHEDULE))
            {
                return GetList(cmd, ALL_AVAILABLE_RECORDS);
            }
        }


        /// <summary>
        /// Retrieves all RecurringBillingSchedule objects by PageRequest
        /// </summary>
        /// <returns>A list of RecurringBillingSchedule objects</returns>
        public RecurringBillingScheduleList GetPaged(PagedRequest request)
        {
            using (SqlCommand cmd = GetSPCommand(GETPAGEDRECURRINGBILLINGSCHEDULE))
            {
                AddParameter(cmd, pInt32Out("TotalRows"));
                AddParameter(cmd, pInt32("PageIndex", request.PageIndex));
                AddParameter(cmd, pInt32("RowPerPage", request.RowPerPage));
                AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause));
                AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn));
                AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder));

                RecurringBillingScheduleList _RecurringBillingScheduleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
                request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
                return _RecurringBillingScheduleList;
            }
        }

        /// <summary>
        /// Retrieves all RecurringBillingSchedule objects by query String
        /// </summary>
        /// <returns>A list of RecurringBillingSchedule objects</returns>
        public RecurringBillingScheduleList GetByQuery(String query)
        {
            using (SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEBYQUERY))
            {
                AddParameter(cmd, pNVarChar("Query", 4000, query));
                return GetList(cmd, ALL_AVAILABLE_RECORDS); ;
            }
        }

        #endregion


        #region Get RecurringBillingSchedule Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of RecurringBillingSchedule
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
        {
            Int32 _MaximumId = 0;
            using (SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEMAXIMUMID))
            {
                SqlDataReader reader;
                _MaximumId = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _MaximumId;
        }

        #endregion

        #region Get RecurringBillingSchedule Row Count Method
        /// <summary>
        /// Retrieves Get Total Rows of RecurringBillingSchedule
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetRowCount()
        {
            Int32 _RecurringBillingScheduleRowCount = 0;
            using (SqlCommand cmd = GetSPCommand(GETRECURRINGBILLINGSCHEDULEROWCOUNT))
            {
                SqlDataReader reader;
                _RecurringBillingScheduleRowCount = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _RecurringBillingScheduleRowCount;
        }

        #endregion

        #region Fill Methods
        /// <summary>
        /// Fills RecurringBillingSchedule object
        /// </summary>
        /// <param name="recurringBillingScheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
        protected void FillObject(RecurringBillingScheduleBase recurringBillingScheduleObject, SqlDataReader reader, int start)
        {

            recurringBillingScheduleObject.Id = reader.GetInt32(start + 0);
            recurringBillingScheduleObject.ScheduleId = reader.GetGuid(start + 1);
            recurringBillingScheduleObject.CompanyId = reader.GetGuid(start + 2);
            recurringBillingScheduleObject.CustomerId = reader.GetGuid(start + 3);
            recurringBillingScheduleObject.TemplateName = reader.GetString(start + 4);
            if (!reader.IsDBNull(5)) recurringBillingScheduleObject.EmailAddress = reader.GetString(start + 5);
            if (!reader.IsDBNull(6)) recurringBillingScheduleObject.CCEmail = reader.GetString(start + 6);
            if (!reader.IsDBNull(7)) recurringBillingScheduleObject.BCCEmail = reader.GetString(start + 7);
            recurringBillingScheduleObject.Status = reader.GetString(start + 8);
            recurringBillingScheduleObject.AutomaticallySendEmail = reader.GetBoolean(start + 9);
            recurringBillingScheduleObject.IncludeOpenInvoices = reader.GetBoolean(start + 10);
            recurringBillingScheduleObject.CollectOnline = reader.GetBoolean(start + 11);
            if (!reader.IsDBNull(12)) recurringBillingScheduleObject.CustomerPaymentProfileId = reader.GetInt32(start + 12);
            recurringBillingScheduleObject.BillCycle = reader.GetString(start + 13);
            recurringBillingScheduleObject.Interval = reader.GetInt32(start + 14);
            if (!reader.IsDBNull(15)) recurringBillingScheduleObject.StartDate = reader.GetDateTime(start + 15);
            if (!reader.IsDBNull(16)) recurringBillingScheduleObject.EndDate = reader.GetDateTime(start + 16);
            if (!reader.IsDBNull(17)) recurringBillingScheduleObject.BillingAddress = reader.GetString(start + 17);
            recurringBillingScheduleObject.BillAmount = reader.GetDouble(start + 18);
            recurringBillingScheduleObject.TaxPercentage = reader.GetDouble(start + 19);
            recurringBillingScheduleObject.TaxAmount = reader.GetDouble(start + 20);
            recurringBillingScheduleObject.TotalBillAmount = reader.GetDouble(start + 21);
            recurringBillingScheduleObject.MessageOnInvoice = reader.GetString(start + 22);
            recurringBillingScheduleObject.CreatedBy = reader.GetGuid(start + 23);
            recurringBillingScheduleObject.CreatedDate = reader.GetDateTime(start + 24);
            recurringBillingScheduleObject.LastUpdatedBy = reader.GetGuid(start + 25);
            recurringBillingScheduleObject.LastUpdatedDate = reader.GetDateTime(start + 26);
            if (!reader.IsDBNull(27)) recurringBillingScheduleObject.DayInAdvance = reader.GetInt32(start + 27);
            if (!reader.IsDBNull(28)) recurringBillingScheduleObject.PreviousDate = reader.GetDateTime(start + 28);
            if (!reader.IsDBNull(29)) recurringBillingScheduleObject.NextDate = reader.GetDateTime(start + 29);
            if (!reader.IsDBNull(30)) recurringBillingScheduleObject.OthersUnpaidBill = reader.GetBoolean(start + 30);
            if (!reader.IsDBNull(31)) recurringBillingScheduleObject.PaymentMethod = reader.GetString(start + 31);
            if (!reader.IsDBNull(32)) recurringBillingScheduleObject.PaymentCollectionDate = reader.GetDateTime(start + 32);
            if (!reader.IsDBNull(33)) recurringBillingScheduleObject.IsEInvoice = reader.GetBoolean(start + 33);
            if (!reader.IsDBNull(34)) recurringBillingScheduleObject.IsEReceipt = reader.GetBoolean(start + 34);
            if (!reader.IsDBNull(35)) recurringBillingScheduleObject.LastRMRInvoiceRefId = reader.GetString(start + 35);
            if (!reader.IsDBNull(36)) recurringBillingScheduleObject.IsReplacement = reader.GetBoolean(start + 36);
            if (!reader.IsDBNull(37)) recurringBillingScheduleObject.IsTransfer = reader.GetBoolean(start + 37);
            if (!reader.IsDBNull(38)) recurringBillingScheduleObject.IsFCReplacement = reader.GetBoolean(start + 38);
            if (!reader.IsDBNull(39)) recurringBillingScheduleObject.IsPOO = reader.GetBoolean(start + 39);
            FillBaseObject(recurringBillingScheduleObject, reader, (start + 40));


            recurringBillingScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
        }

        /// <summary>
        /// Fills RecurringBillingSchedule object
        /// </summary>
        /// <param name="recurringBillingScheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        protected void FillObject(RecurringBillingScheduleBase recurringBillingScheduleObject, SqlDataReader reader)
        {
            FillObject(recurringBillingScheduleObject, reader, 0);
        }

        /// <summary>
        /// Retrieves RecurringBillingSchedule object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecurringBillingSchedule object</returns>
        private RecurringBillingSchedule GetObject(SqlCommand cmd)
        {
            SqlDataReader reader;
            long rows = SelectRecords(cmd, out reader);

            using (reader)
            {
                if (reader.Read())
                {
                    RecurringBillingSchedule recurringBillingScheduleObject = new RecurringBillingSchedule();
                    FillObject(recurringBillingScheduleObject, reader);
                    return recurringBillingScheduleObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves list of RecurringBillingSchedule objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecurringBillingSchedule objects</returns>
        private RecurringBillingScheduleList GetList(SqlCommand cmd, long rows)
        {
            // Select multiple records
            SqlDataReader reader;
            long result = SelectRecords(cmd, out reader);

            //RecurringBillingSchedule list
            RecurringBillingScheduleList list = new RecurringBillingScheduleList();

            using (reader)
            {
                // Read rows until end of result or number of rows specified is reached
                while (reader.Read() && rows-- != 0)
                {
                    RecurringBillingSchedule recurringBillingScheduleObject = new RecurringBillingSchedule();
                    FillObject(recurringBillingScheduleObject, reader);

                    list.Add(recurringBillingScheduleObject);
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