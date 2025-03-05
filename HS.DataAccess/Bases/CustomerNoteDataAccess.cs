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
	public partial class CustomerNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERNOTE = "InsertCustomerNote";
		private const string UPDATECUSTOMERNOTE = "UpdateCustomerNote";
		private const string DELETECUSTOMERNOTE = "DeleteCustomerNote";
		private const string GETCUSTOMERNOTEBYID = "GetCustomerNoteById";
		private const string GETALLCUSTOMERNOTE = "GetAllCustomerNote";
		private const string GETPAGEDCUSTOMERNOTE = "GetPagedCustomerNote";
		private const string GETCUSTOMERNOTEMAXIMUMID = "GetCustomerNoteMaximumId";
		private const string GETCUSTOMERNOTEROWCOUNT = "GetCustomerNoteRowCount";	
		private const string GETCUSTOMERNOTEBYQUERY = "GetCustomerNoteByQuery";
		#endregion
		
		#region Constructors
		public CustomerNoteDataAccess(ClientContext context) : base(context) { }
		public CustomerNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerNoteBase customerNoteObject)
		{	
			AddParameter(cmd, pNVarChar(CustomerNoteBase.Property_Notes, customerNoteObject.Notes));
			AddParameter(cmd, pDateTime(CustomerNoteBase.Property_ReminderDate, customerNoteObject.ReminderDate));
			AddParameter(cmd, pDateTime(CustomerNoteBase.Property_ReminderEndDate, customerNoteObject.ReminderEndDate));
			AddParameter(cmd, pGuid(CustomerNoteBase.Property_CustomerId, customerNoteObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerNoteBase.Property_CompanyId, customerNoteObject.CompanyId));
			AddParameter(cmd, pDateTime(CustomerNoteBase.Property_CreatedDate, customerNoteObject.CreatedDate));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsEmail, customerNoteObject.IsEmail));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsText, customerNoteObject.IsText));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsShedule, customerNoteObject.IsShedule));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsFollowUp, customerNoteObject.IsFollowUp));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsActive, customerNoteObject.IsActive));
			AddParameter(cmd, pNVarChar(CustomerNoteBase.Property_CreatedBy, 50, customerNoteObject.CreatedBy));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsClose, customerNoteObject.IsClose));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsAllDay, customerNoteObject.IsAllDay));
			AddParameter(cmd, pGuid(CustomerNoteBase.Property_CreatedByUid, customerNoteObject.CreatedByUid));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsPin, customerNoteObject.IsPin));
			AddParameter(cmd, pNVarChar(CustomerNoteBase.Property_NoteType, 50, customerNoteObject.NoteType));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsOverview, customerNoteObject.IsOverview));
			AddParameter(cmd, pInt32(CustomerNoteBase.Property_OrderBy, customerNoteObject.OrderBy));
			AddParameter(cmd, pInt32(CustomerNoteBase.Property_ReferenceTicketId, customerNoteObject.ReferenceTicketId));
			AddParameter(cmd, pInt32(CustomerNoteBase.Property_ThirdPartyId, customerNoteObject.ThirdPartyId));
			AddParameter(cmd, pBool(CustomerNoteBase.Property_IsPrimaryNote, customerNoteObject.IsPrimaryNote));
			AddParameter(cmd, pInt32(CustomerNoteBase.Property_TeamSettingId, customerNoteObject.TeamSettingId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerNote
        /// </summary>
        /// <param name="customerNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerNoteBase customerNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERNOTE);
	
				AddParameter(cmd, pInt32Out(CustomerNoteBase.Property_Id));
				AddCommonParams(cmd, customerNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerNoteObject.Id = (Int32)GetOutParameter(cmd, CustomerNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerNote
        /// </summary>
        /// <param name="customerNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerNoteBase customerNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERNOTE);
				
				AddParameter(cmd, pInt32(CustomerNoteBase.Property_Id, customerNoteObject.Id));
				AddCommonParams(cmd, customerNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerNote
        /// </summary>
        /// <param name="Id">Id of the CustomerNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERNOTE);	
				
				AddParameter(cmd, pInt32(CustomerNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerNote object to retrieve</param>
        /// <returns>CustomerNote object, null if not found</returns>
		public CustomerNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOTEBYID))
			{
				AddParameter( cmd, pInt32(CustomerNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerNote objects 
        /// </summary>
        /// <returns>A list of CustomerNote objects</returns>
		public CustomerNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERNOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerNote objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerNote objects</returns>
		public CustomerNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERNOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerNoteList _CustomerNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerNote objects by query String
        /// </summary>
        /// <returns>A list of CustomerNote objects</returns>
		public CustomerNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOTEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerNote object
        /// </summary>
        /// <param name="customerNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerNoteBase customerNoteObject, SqlDataReader reader, int start)
		{
			
				customerNoteObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) customerNoteObject.Notes = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) customerNoteObject.ReminderDate = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) customerNoteObject.ReminderEndDate = reader.GetDateTime( start + 3 );			
				customerNoteObject.CustomerId = reader.GetGuid( start + 4 );			
				customerNoteObject.CompanyId = reader.GetGuid( start + 5 );			
				customerNoteObject.CreatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customerNoteObject.IsEmail = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) customerNoteObject.IsText = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) customerNoteObject.IsShedule = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) customerNoteObject.IsFollowUp = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) customerNoteObject.IsActive = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) customerNoteObject.CreatedBy = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerNoteObject.IsClose = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) customerNoteObject.IsAllDay = reader.GetBoolean( start + 14 );			
				customerNoteObject.CreatedByUid = reader.GetGuid( start + 15 );			
				if(!reader.IsDBNull(16)) customerNoteObject.IsPin = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) customerNoteObject.NoteType = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerNoteObject.IsOverview = reader.GetBoolean( start + 18 );			
				if(!reader.IsDBNull(19)) customerNoteObject.OrderBy = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) customerNoteObject.ReferenceTicketId = reader.GetInt32( start + 20 );			
				if(!reader.IsDBNull(21)) customerNoteObject.ThirdPartyId = reader.GetInt32( start + 21 );			
				if(!reader.IsDBNull(22)) customerNoteObject.IsPrimaryNote = reader.GetBoolean( start + 22 );			
				if(!reader.IsDBNull(23)) customerNoteObject.TeamSettingId = reader.GetInt32( start + 23 );			
			FillBaseObject(customerNoteObject, reader, (start + 24));

			
			customerNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerNote object
        /// </summary>
        /// <param name="customerNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerNoteBase customerNoteObject, SqlDataReader reader)
		{
			FillObject(customerNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerNote object</returns>
		private CustomerNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerNote customerNoteObject= new CustomerNote();
					FillObject(customerNoteObject, reader);
					return customerNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerNote objects</returns>
		private CustomerNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerNote list
			CustomerNoteList list = new CustomerNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerNote customerNoteObject = new CustomerNote();
					FillObject(customerNoteObject, reader);

					list.Add(customerNoteObject);
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
