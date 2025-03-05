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
	public partial class CustomerCancellationQueueDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCANCELLATIONQUEUE = "InsertCustomerCancellationQueue";
		private const string UPDATECUSTOMERCANCELLATIONQUEUE = "UpdateCustomerCancellationQueue";
		private const string DELETECUSTOMERCANCELLATIONQUEUE = "DeleteCustomerCancellationQueue";
		private const string GETCUSTOMERCANCELLATIONQUEUEBYID = "GetCustomerCancellationQueueById";
		private const string GETALLCUSTOMERCANCELLATIONQUEUE = "GetAllCustomerCancellationQueue";
		private const string GETPAGEDCUSTOMERCANCELLATIONQUEUE = "GetPagedCustomerCancellationQueue";
		private const string GETCUSTOMERCANCELLATIONQUEUEMAXIMUMID = "GetCustomerCancellationQueueMaximumId";
		private const string GETCUSTOMERCANCELLATIONQUEUEROWCOUNT = "GetCustomerCancellationQueueRowCount";	
		private const string GETCUSTOMERCANCELLATIONQUEUEBYQUERY = "GetCustomerCancellationQueueByQuery";
		#endregion
		
		#region Constructors
		public CustomerCancellationQueueDataAccess(ClientContext context) : base(context) { }
		public CustomerCancellationQueueDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCancellationQueueObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCancellationQueueBase customerCancellationQueueObject)
		{	
			AddParameter(cmd, pGuid(CustomerCancellationQueueBase.Property_CancellationId, customerCancellationQueueObject.CancellationId));
			AddParameter(cmd, pGuid(CustomerCancellationQueueBase.Property_CustomerId, customerCancellationQueueObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerCancellationQueueBase.Property_Reason, 100, customerCancellationQueueObject.Reason));
			AddParameter(cmd, pDouble(CustomerCancellationQueueBase.Property_RemainingBalance, customerCancellationQueueObject.RemainingBalance));
			AddParameter(cmd, pDateTime(CustomerCancellationQueueBase.Property_CancellationDate, customerCancellationQueueObject.CancellationDate));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsSigned, customerCancellationQueueObject.IsSigned));
			AddParameter(cmd, pDateTime(CustomerCancellationQueueBase.Property_CreatedDate, customerCancellationQueueObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerCancellationQueueBase.Property_CreatedBy, customerCancellationQueueObject.CreatedBy));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsActive, customerCancellationQueueObject.IsActive));
			AddParameter(cmd, pNVarChar(CustomerCancellationQueueBase.Property_Note, customerCancellationQueueObject.Note));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsCancelled, customerCancellationQueueObject.IsCancelled));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsInvoiceOff, customerCancellationQueueObject.IsInvoiceOff));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsBillingOff, customerCancellationQueueObject.IsBillingOff));
			AddParameter(cmd, pBool(CustomerCancellationQueueBase.Property_IsAlarmOff, customerCancellationQueueObject.IsAlarmOff));
			AddParameter(cmd, pNVarChar(CustomerCancellationQueueBase.Property_EmployeeReason, 250, customerCancellationQueueObject.EmployeeReason));
			AddParameter(cmd, pInt32(CustomerCancellationQueueBase.Property_ExpirationDays, customerCancellationQueueObject.ExpirationDays));
			AddParameter(cmd, pDateTime(CustomerCancellationQueueBase.Property_ExpirationDate, customerCancellationQueueObject.ExpirationDate));
			AddParameter(cmd, pGuid(CustomerCancellationQueueBase.Property_FileId, customerCancellationQueueObject.FileId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCancellationQueue
        /// </summary>
        /// <param name="customerCancellationQueueObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCancellationQueueBase customerCancellationQueueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCANCELLATIONQUEUE);
	
				AddParameter(cmd, pInt32Out(CustomerCancellationQueueBase.Property_Id));
				AddCommonParams(cmd, customerCancellationQueueObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCancellationQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCancellationQueueObject.Id = (Int32)GetOutParameter(cmd, CustomerCancellationQueueBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCancellationQueueObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCancellationQueue
        /// </summary>
        /// <param name="customerCancellationQueueObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCancellationQueueBase customerCancellationQueueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCANCELLATIONQUEUE);
				
				AddParameter(cmd, pInt32(CustomerCancellationQueueBase.Property_Id, customerCancellationQueueObject.Id));
				AddCommonParams(cmd, customerCancellationQueueObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCancellationQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCancellationQueueObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCancellationQueue
        /// </summary>
        /// <param name="Id">Id of the CustomerCancellationQueue object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCANCELLATIONQUEUE);	
				
				AddParameter(cmd, pInt32(CustomerCancellationQueueBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCancellationQueue), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCancellationQueue object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCancellationQueue object to retrieve</param>
        /// <returns>CustomerCancellationQueue object, null if not found</returns>
		public CustomerCancellationQueue Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONQUEUEBYID))
			{
				AddParameter( cmd, pInt32(CustomerCancellationQueueBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCancellationQueue objects 
        /// </summary>
        /// <returns>A list of CustomerCancellationQueue objects</returns>
		public CustomerCancellationQueueList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCANCELLATIONQUEUE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCancellationQueue objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCancellationQueue objects</returns>
		public CustomerCancellationQueueList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCANCELLATIONQUEUE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCancellationQueueList _CustomerCancellationQueueList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCancellationQueueList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCancellationQueue objects by query String
        /// </summary>
        /// <returns>A list of CustomerCancellationQueue objects</returns>
		public CustomerCancellationQueueList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONQUEUEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCancellationQueue Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCancellationQueue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONQUEUEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCancellationQueue Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCancellationQueue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCancellationQueueRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONQUEUEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCancellationQueueRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCancellationQueueRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCancellationQueue object
        /// </summary>
        /// <param name="customerCancellationQueueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCancellationQueueBase customerCancellationQueueObject, SqlDataReader reader, int start)
		{
			
				customerCancellationQueueObject.Id = reader.GetInt32( start + 0 );			
				customerCancellationQueueObject.CancellationId = reader.GetGuid( start + 1 );			
				customerCancellationQueueObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerCancellationQueueObject.Reason = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerCancellationQueueObject.RemainingBalance = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) customerCancellationQueueObject.CancellationDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) customerCancellationQueueObject.IsSigned = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) customerCancellationQueueObject.CreatedDate = reader.GetDateTime( start + 7 );			
				customerCancellationQueueObject.CreatedBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) customerCancellationQueueObject.IsActive = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) customerCancellationQueueObject.Note = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerCancellationQueueObject.IsCancelled = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) customerCancellationQueueObject.IsInvoiceOff = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) customerCancellationQueueObject.IsBillingOff = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) customerCancellationQueueObject.IsAlarmOff = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) customerCancellationQueueObject.EmployeeReason = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerCancellationQueueObject.ExpirationDays = reader.GetInt32( start + 16 );			
				if(!reader.IsDBNull(17)) customerCancellationQueueObject.ExpirationDate = reader.GetDateTime( start + 17 );			
				customerCancellationQueueObject.FileId = reader.GetGuid( start + 18 );			
			FillBaseObject(customerCancellationQueueObject, reader, (start + 19));

			
			customerCancellationQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCancellationQueue object
        /// </summary>
        /// <param name="customerCancellationQueueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCancellationQueueBase customerCancellationQueueObject, SqlDataReader reader)
		{
			FillObject(customerCancellationQueueObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCancellationQueue object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCancellationQueue object</returns>
		private CustomerCancellationQueue GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCancellationQueue customerCancellationQueueObject= new CustomerCancellationQueue();
					FillObject(customerCancellationQueueObject, reader);
					return customerCancellationQueueObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCancellationQueue objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCancellationQueue objects</returns>
		private CustomerCancellationQueueList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCancellationQueue list
			CustomerCancellationQueueList list = new CustomerCancellationQueueList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCancellationQueue customerCancellationQueueObject = new CustomerCancellationQueue();
					FillObject(customerCancellationQueueObject, reader);

					list.Add(customerCancellationQueueObject);
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
