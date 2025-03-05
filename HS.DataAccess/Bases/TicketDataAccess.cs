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
	public partial class TicketDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKET = "InsertTicket";
		private const string UPDATETICKET = "UpdateTicket";
		private const string DELETETICKET = "DeleteTicket";
		private const string GETTICKETBYID = "GetTicketById";
		private const string GETALLTICKET = "GetAllTicket";
		private const string GETPAGEDTICKET = "GetPagedTicket";
		private const string GETTICKETMAXIMUMID = "GetTicketMaximumId";
		private const string GETTICKETROWCOUNT = "GetTicketRowCount";	
		private const string GETTICKETBYQUERY = "GetTicketByQuery";
		#endregion
		
		#region Constructors
		public TicketDataAccess(ClientContext context) : base(context) { }
		public TicketDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketBase ticketObject)
		{	
			AddParameter(cmd, pGuid(TicketBase.Property_TicketId, ticketObject.TicketId));
			AddParameter(cmd, pGuid(TicketBase.Property_CompanyId, ticketObject.CompanyId));
			AddParameter(cmd, pGuid(TicketBase.Property_CustomerId, ticketObject.CustomerId));
			AddParameter(cmd, pNVarChar(TicketBase.Property_TicketType, 50, ticketObject.TicketType));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Subject, 500, ticketObject.Subject));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Message, ticketObject.Message));
			AddParameter(cmd, pGuid(TicketBase.Property_CreatedBy, ticketObject.CreatedBy));
			AddParameter(cmd, pDateTime(TicketBase.Property_CreatedDate, ticketObject.CreatedDate));
			AddParameter(cmd, pDateTime(TicketBase.Property_CompletionDate, ticketObject.CompletionDate));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Status, 50, ticketObject.Status));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Priority, 50, ticketObject.Priority));
			AddParameter(cmd, pGuid(TicketBase.Property_LastUpdatedBy, ticketObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(TicketBase.Property_LastUpdatedDate, ticketObject.LastUpdatedDate));
			AddParameter(cmd, pBool(TicketBase.Property_HasInvoice, ticketObject.HasInvoice));
			AddParameter(cmd, pBool(TicketBase.Property_HasSurvey, ticketObject.HasSurvey));
			AddParameter(cmd, pBool(TicketBase.Property_IsClosed, ticketObject.IsClosed));
			AddParameter(cmd, pBool(TicketBase.Property_IsAgreementTicket, ticketObject.IsAgreementTicket));
			AddParameter(cmd, pDateTime(TicketBase.Property_CompletedDate, ticketObject.CompletedDate));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Signature, ticketObject.Signature));
			AddParameter(cmd, pBool(TicketBase.Property_IsDispatch, ticketObject.IsDispatch));
			AddParameter(cmd, pInt32(TicketBase.Property_ReferenceTicketId, ticketObject.ReferenceTicketId));
			AddParameter(cmd, pNVarChar(TicketBase.Property_BookingId, 50, ticketObject.BookingId));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Reason, 150, ticketObject.Reason));
			AddParameter(cmd, pNVarChar(TicketBase.Property_RackNo, 50, ticketObject.RackNo));
			AddParameter(cmd, pNVarChar(TicketBase.Property_Locations, 150, ticketObject.Locations));
			AddParameter(cmd, pInt32(TicketBase.Property_RescheduleTicketId, ticketObject.RescheduleTicketId));
			AddParameter(cmd, pBool(TicketBase.Property_IsImportedTicket, ticketObject.IsImportedTicket));
			AddParameter(cmd, pDateTime(TicketBase.Property_TicketSignatureDate, ticketObject.TicketSignatureDate));
			AddParameter(cmd, pDateTime(TicketBase.Property_TechOnsiteDate, ticketObject.TechOnsiteDate));
			AddParameter(cmd, pNVarChar(TicketBase.Property_WorkToBePerformed, ticketObject.WorkToBePerformed));
			AddParameter(cmd, pBool(TicketBase.Property_EquipmentPriceChanged, ticketObject.EquipmentPriceChanged));
			AddParameter(cmd, pBool(TicketBase.Property_ServicePriceChanged, ticketObject.ServicePriceChanged));
			AddParameter(cmd, pBool(TicketBase.Property_EquipmentQTYChanged, ticketObject.EquipmentQTYChanged));
			AddParameter(cmd, pBool(TicketBase.Property_ServiceQTYChanged, ticketObject.ServiceQTYChanged));
			AddParameter(cmd, pBool(TicketBase.Property_IsPayrollClosed, ticketObject.IsPayrollClosed));
			AddParameter(cmd, pNVarChar(TicketBase.Property_MiscName, 500, ticketObject.MiscName));
			AddParameter(cmd, pDecimal(TicketBase.Property_MiscValue, ticketObject.MiscValue));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Ticket
        /// </summary>
        /// <param name="ticketObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketBase ticketObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKET);
	
				AddParameter(cmd, pInt32Out(TicketBase.Property_Id));
				AddCommonParams(cmd, ticketObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketObject.Id = (Int32)GetOutParameter(cmd, TicketBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Ticket
        /// </summary>
        /// <param name="ticketObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketBase ticketObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKET);
				
				AddParameter(cmd, pInt32(TicketBase.Property_Id, ticketObject.Id));
				AddCommonParams(cmd, ticketObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Ticket
        /// </summary>
        /// <param name="Id">Id of the Ticket object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKET);	
				
				AddParameter(cmd, pInt32(TicketBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Ticket), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Ticket object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Ticket object to retrieve</param>
        /// <returns>Ticket object, null if not found</returns>
		public Ticket Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBYID))
			{
				AddParameter( cmd, pInt32(TicketBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Ticket objects 
        /// </summary>
        /// <returns>A list of Ticket objects</returns>
		public TicketList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKET))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Ticket objects by PageRequest
        /// </summary>
        /// <returns>A list of Ticket objects</returns>
		public TicketList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKET))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketList _TicketList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketList;
			}
		}
		
		/// <summary>
        /// Retrieves all Ticket objects by query String
        /// </summary>
        /// <returns>A list of Ticket objects</returns>
		public TicketList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
        public DataTable GetJobBookingItemsByTicketId(Guid ticketid)
        {
            string sqlQuery = @"select tbd.*, (select lp.DisplayText from [LookUp] lp where  lp.DataKey = 'ServiceItems' + tbd.ServiceType and lp.DataValue = tbd.Extras) as DisplayText, tk.TicketId
                                from Ticket tk
                                left join TicketBookingDetails tbd on tk.BookingId = tbd.BookingId and tk.Subject = tbd.ServiceType
                                where tk.TicketId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllCustomerAppointmentEquipmentListByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select						   
                               CAE.[Id]
                              ,CAE.[AppointmentId]
                              ,CAE.[EquipmentId]
                              ,CAE.[Quantity]
                              ,CAE.[IsEquipmentRelease]
                              ,ep.SupplierCost
							  ,ep.Retail
                              ,ep.RepCost
                              ,ep.SKU
							  ,ep.Barcode
                              ,ep.EquipmentClassId as EquipmentClassId
                              ,CAE.[UnitPrice]
                              ,CAE.[OriginalUnitPrice]
                              ,CAE.[TotalPrice]
                              ,CAE.[CreatedDate]
                              ,CAE.IsService
                              ,CAE.IsAgreementItem
                              ,CAE.[CreatedBy]
                              ,CAE.[CreatedByUid]
	                          ,CAE.[EquipName]
                              ,CAE.[EquipName] as EquipmentName
                              ,CAE.[EquipDetail]
                              ,CAE.IsDefaultService,
                               CAE.IsInvoiceCreate
                              ,CAE.ReferenceInvoiceId,
                              CAE.ReferenceInvDetailId
                                ,CAE.IsBilling
							  ,EF.[FileDescription]
							  ,EF.Filename
							  ,EF.FileFullName
							  ,EF.FileType
							  ,EF.EquipmentId
                              ,tu.UserId as TechnicianId
                              ,emp.FirstName + ' ' +emp.LastName as CreatedByName
                              ,CAE.IsBaseItem
                                ,CAE.IsBadInventory
                              ,CASE 
									WHEN tu.UserId='22222222-2222-2222-2222-222222222222' AND CAE.QuantityLeftEquipment=0 THEN 0
									WHEN tu.UserId='22222222-2222-2222-2222-222222222221' AND CAE.QuantityLeftEquipment=0 THEN 0
									ELSE 
                                    (SELECT 
                                    ISNULL(SUM(Quantity),0) AS TotalQty
                                    FROM
                                    (SELECT 
                                    ISNULL(SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END),0) AS Quantity
                                    FROM 
                                    InventoryTech
                                    WHERE 
                                    EquipmentId =ep.EquipmentId AND
                                    TechnicianId = tu.UserId
                                    GROUP BY 
                                    TechnicianId										  
                               HAVING 
                               SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END) >= 0) AS Qty)
                               - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = ep.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 And b.TechnicianId=tu.UserId),0)END QuantityOnHand
                             ,ISNULL(IsCheckedEquipment, 0) as IsCheckedEquipment
                             ,CAE.QuantityLeftEquipment
                             ,CAE.IsCopied
                             ,CAE.isEquipmentExist
                             ,ep.Point as  Point
                             ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = ep.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost
							 ,format(CAE.Quantity*ep.Point,'N2') as  TotalPoint
                             , CAE.IsBillingProcess
                             ,ep.IsARBEnabled
                             from CustomerAppointmentEquipment CAE
                             LEFT JOIN Equipment ep on ep.EquipmentId=CAE.EquipmentId
                       --      Left Join EquipmentVendor ev on ev.EquipmentId = CAE.EquipmentId
                             LEFT JOIN TicketUser tu on tu.TiketId=CAE.AppointmentId and tu.IsPrimary=1
                             LEFT JOIN Employee emp on CAE.CreatedByUid = emp.UserId
                            Left Join EquipmentFile EF on CAE.EquipmentId = EF.EquipmentId AND EF.FileType = '{1}'
                            where CAE.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, "ProfilePicture");
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteAdditionalMembersByTicketId(Guid ticketid)
        {
            string sqlQuery = @"delete from TicketUser where TiketId = '{0}' and IsPrimary = 0 and NotificationOnly = 0
                                delete from AdditionalMembersAppointment where AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable GetJobFilesByTicketId(Guid ticketid)
        {
            string sqlQuery = @"select tf.*
                                from TicketFile tf
                                left join Ticket tk on tf.TicketId = tk.TicketId
                                where tf.TicketId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteNotifyingMembersByTicketId(Guid ticketid)
        {
            string sqlQuery = @"delete from TicketUser where TiketId = '{0}' and IsPrimary = 0 and NotificationOnly = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable GetJobNotesByTicketId(Guid ticketid)
        {
            string sqlQuery = @"select tr.*, emp.FirstName + ' ' + emp.LastName as UserName, emp.ProfilePicture
                                from TicketReply tr
                                left join Employee emp on emp.UserId = tr.UserId
                                where tr.TicketId = '{0}'
								and CHARINDEX('<p>', tr.[Message]) > 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion


        #region Get Ticket Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of Ticket
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Ticket Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Ticket
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETROWCOUNT))
			{
				SqlDataReader reader;
				_TicketRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Ticket object
        /// </summary>
        /// <param name="ticketObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketBase ticketObject, SqlDataReader reader, int start)
		{
			
				ticketObject.Id = reader.GetInt32( start + 0 );			
				ticketObject.TicketId = reader.GetGuid( start + 1 );			
				ticketObject.CompanyId = reader.GetGuid( start + 2 );			
				ticketObject.CustomerId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) ticketObject.TicketType = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketObject.Subject = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) ticketObject.Message = reader.GetString( start + 6 );			
				ticketObject.CreatedBy = reader.GetGuid( start + 7 );			
				ticketObject.CreatedDate = reader.GetDateTime( start + 8 );			
				ticketObject.CompletionDate = reader.GetDateTime( start + 9 );			
				ticketObject.Status = reader.GetString( start + 10 );			
				ticketObject.Priority = reader.GetString( start + 11 );			
				ticketObject.LastUpdatedBy = reader.GetGuid( start + 12 );			
				ticketObject.LastUpdatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) ticketObject.HasInvoice = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) ticketObject.HasSurvey = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) ticketObject.IsClosed = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) ticketObject.IsAgreementTicket = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) ticketObject.CompletedDate = reader.GetDateTime( start + 18 );			
				if(!reader.IsDBNull(19)) ticketObject.Signature = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) ticketObject.IsDispatch = reader.GetBoolean( start + 20 );			
				if(!reader.IsDBNull(21)) ticketObject.ReferenceTicketId = reader.GetInt32( start + 21 );			
				if(!reader.IsDBNull(22)) ticketObject.BookingId = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) ticketObject.Reason = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) ticketObject.RackNo = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) ticketObject.Locations = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) ticketObject.RescheduleTicketId = reader.GetInt32( start + 26 );			
				if(!reader.IsDBNull(27)) ticketObject.IsImportedTicket = reader.GetBoolean( start + 27 );			
				if(!reader.IsDBNull(28)) ticketObject.TicketSignatureDate = reader.GetDateTime( start + 28 );			
				if(!reader.IsDBNull(29)) ticketObject.TechOnsiteDate = reader.GetDateTime( start + 29 );			
				if(!reader.IsDBNull(30)) ticketObject.WorkToBePerformed = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) ticketObject.EquipmentPriceChanged = reader.GetBoolean( start + 31 );			
				if(!reader.IsDBNull(32)) ticketObject.ServicePriceChanged = reader.GetBoolean( start + 32 );			
				if(!reader.IsDBNull(33)) ticketObject.EquipmentQTYChanged = reader.GetBoolean( start + 33 );			
				if(!reader.IsDBNull(34)) ticketObject.ServiceQTYChanged = reader.GetBoolean( start + 34 );			
				if(!reader.IsDBNull(35)) ticketObject.IsPayrollClosed = reader.GetBoolean( start + 35 );			
				if(!reader.IsDBNull(36)) ticketObject.MiscName = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) ticketObject.MiscValue = reader.GetDecimal( start + 37 );			
			FillBaseObject(ticketObject, reader, (start + 38));

			
			ticketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Ticket object
        /// </summary>
        /// <param name="ticketObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketBase ticketObject, SqlDataReader reader)
		{
			FillObject(ticketObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Ticket object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Ticket object</returns>
		private Ticket GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Ticket ticketObject= new Ticket();
					FillObject(ticketObject, reader);
					return ticketObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Ticket objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Ticket objects</returns>
		private TicketList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Ticket list
			TicketList list = new TicketList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Ticket ticketObject = new Ticket();
					FillObject(ticketObject, reader);

					list.Add(ticketObject);
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
