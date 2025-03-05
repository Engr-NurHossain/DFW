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
	public partial class CustomerProratedBillDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERPRORATEDBILL = "InsertCustomerProratedBill";
		private const string UPDATECUSTOMERPRORATEDBILL = "UpdateCustomerProratedBill";
		private const string DELETECUSTOMERPRORATEDBILL = "DeleteCustomerProratedBill";
		private const string GETCUSTOMERPRORATEDBILLBYID = "GetCustomerProratedBillById";
		private const string GETALLCUSTOMERPRORATEDBILL = "GetAllCustomerProratedBill";
		private const string GETPAGEDCUSTOMERPRORATEDBILL = "GetPagedCustomerProratedBill";
		private const string GETCUSTOMERPRORATEDBILLMAXIMUMID = "GetCustomerProratedBillMaximumId";
		private const string GETCUSTOMERPRORATEDBILLROWCOUNT = "GetCustomerProratedBillRowCount";	
		private const string GETCUSTOMERPRORATEDBILLBYQUERY = "GetCustomerProratedBillByQuery";
		#endregion
		
		#region Constructors
		public CustomerProratedBillDataAccess(ClientContext context) : base(context) { }
		public CustomerProratedBillDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerProratedBillObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerProratedBillBase customerProratedBillObject)
		{	
			AddParameter(cmd, pGuid(CustomerProratedBillBase.Property_CompanyId, customerProratedBillObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerProratedBillBase.Property_CustomerId, customerProratedBillObject.CustomerId));
			AddParameter(cmd, pDateTime(CustomerProratedBillBase.Property_StartDate, customerProratedBillObject.StartDate));
			AddParameter(cmd, pDateTime(CustomerProratedBillBase.Property_EndDate, customerProratedBillObject.EndDate));
			AddParameter(cmd, pInt32(CustomerProratedBillBase.Property_DayCount, customerProratedBillObject.DayCount));
			AddParameter(cmd, pDouble(CustomerProratedBillBase.Property_Amount, customerProratedBillObject.Amount));
			AddParameter(cmd, pNVarChar(CustomerProratedBillBase.Property_InvoiceId, 50, customerProratedBillObject.InvoiceId));
			AddParameter(cmd, pGuid(CustomerProratedBillBase.Property_CreatedBy, customerProratedBillObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerProratedBillBase.Property_CreatedDate, customerProratedBillObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerProratedBill
        /// </summary>
        /// <param name="customerProratedBillObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerProratedBillBase customerProratedBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERPRORATEDBILL);
	
				AddParameter(cmd, pInt32Out(CustomerProratedBillBase.Property_Id));
				AddCommonParams(cmd, customerProratedBillObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerProratedBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerProratedBillObject.Id = (Int32)GetOutParameter(cmd, CustomerProratedBillBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerProratedBillObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerProratedBill
        /// </summary>
        /// <param name="customerProratedBillObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerProratedBillBase customerProratedBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERPRORATEDBILL);
				
				AddParameter(cmd, pInt32(CustomerProratedBillBase.Property_Id, customerProratedBillObject.Id));
				AddCommonParams(cmd, customerProratedBillObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerProratedBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerProratedBillObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerProratedBill
        /// </summary>
        /// <param name="Id">Id of the CustomerProratedBill object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERPRORATEDBILL);	
				
				AddParameter(cmd, pInt32(CustomerProratedBillBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerProratedBill), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerProratedBill object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerProratedBill object to retrieve</param>
        /// <returns>CustomerProratedBill object, null if not found</returns>
		public CustomerProratedBill Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPRORATEDBILLBYID))
			{
				AddParameter( cmd, pInt32(CustomerProratedBillBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerProratedBill objects 
        /// </summary>
        /// <returns>A list of CustomerProratedBill objects</returns>
		public CustomerProratedBillList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERPRORATEDBILL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerProratedBill objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerProratedBill objects</returns>
		public CustomerProratedBillList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERPRORATEDBILL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerProratedBillList _CustomerProratedBillList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerProratedBillList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerProratedBill objects by query String
        /// </summary>
        /// <returns>A list of CustomerProratedBill objects</returns>
		public CustomerProratedBillList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPRORATEDBILLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerProratedBill Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerProratedBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPRORATEDBILLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerProratedBill Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerProratedBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerProratedBillRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPRORATEDBILLROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerProratedBillRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerProratedBillRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerProratedBill object
        /// </summary>
        /// <param name="customerProratedBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerProratedBillBase customerProratedBillObject, SqlDataReader reader, int start)
		{
			
				customerProratedBillObject.Id = reader.GetInt32( start + 0 );			
				customerProratedBillObject.CompanyId = reader.GetGuid( start + 1 );			
				customerProratedBillObject.CustomerId = reader.GetGuid( start + 2 );			
				customerProratedBillObject.StartDate = reader.GetDateTime( start + 3 );			
				customerProratedBillObject.EndDate = reader.GetDateTime( start + 4 );			
				customerProratedBillObject.DayCount = reader.GetInt32( start + 5 );			
				customerProratedBillObject.Amount = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) customerProratedBillObject.InvoiceId = reader.GetString( start + 7 );			
				customerProratedBillObject.CreatedBy = reader.GetGuid( start + 8 );			
				customerProratedBillObject.CreatedDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(customerProratedBillObject, reader, (start + 10));

			
			customerProratedBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerProratedBill object
        /// </summary>
        /// <param name="customerProratedBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerProratedBillBase customerProratedBillObject, SqlDataReader reader)
		{
			FillObject(customerProratedBillObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerProratedBill object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerProratedBill object</returns>
		private CustomerProratedBill GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerProratedBill customerProratedBillObject= new CustomerProratedBill();
					FillObject(customerProratedBillObject, reader);
					return customerProratedBillObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerProratedBill objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerProratedBill objects</returns>
		private CustomerProratedBillList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerProratedBill list
			CustomerProratedBillList list = new CustomerProratedBillList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerProratedBill customerProratedBillObject = new CustomerProratedBill();
					FillObject(customerProratedBillObject, reader);

					list.Add(customerProratedBillObject);
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
