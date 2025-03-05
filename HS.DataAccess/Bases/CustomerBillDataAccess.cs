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
	public partial class CustomerBillDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERBILL = "InsertCustomerBill";
		private const string UPDATECUSTOMERBILL = "UpdateCustomerBill";
		private const string DELETECUSTOMERBILL = "DeleteCustomerBill";
		private const string GETCUSTOMERBILLBYID = "GetCustomerBillById";
		private const string GETALLCUSTOMERBILL = "GetAllCustomerBill";
		private const string GETPAGEDCUSTOMERBILL = "GetPagedCustomerBill";
		private const string GETCUSTOMERBILLMAXIMUMID = "GetCustomerBillMaximumId";
		private const string GETCUSTOMERBILLROWCOUNT = "GetCustomerBillRowCount";	
		private const string GETCUSTOMERBILLBYQUERY = "GetCustomerBillByQuery";
		#endregion
		
		#region Constructors
		public CustomerBillDataAccess(ClientContext context) : base(context) { }
		public CustomerBillDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerBillObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerBillBase customerBillObject)
		{	
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_BillNo, 50, customerBillObject.BillNo));
			AddParameter(cmd, pGuid(CustomerBillBase.Property_CompanyId, customerBillObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerBillBase.Property_CustomerId, customerBillObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_Type, 50, customerBillObject.Type));
			AddParameter(cmd, pDouble(CustomerBillBase.Property_Amount, customerBillObject.Amount));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_PaymentMethod, 50, customerBillObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_PaymentStatus, 50, customerBillObject.PaymentStatus));
			AddParameter(cmd, pDateTime(CustomerBillBase.Property_PaymentDate, customerBillObject.PaymentDate));
			AddParameter(cmd, pDateTime(CustomerBillBase.Property_PaymentDueDate, customerBillObject.PaymentDueDate));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_BillCycle, 50, customerBillObject.BillCycle));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_Notes, customerBillObject.Notes));
			AddParameter(cmd, pNVarChar(CustomerBillBase.Property_UpdatedBy, 50, customerBillObject.UpdatedBy));
			AddParameter(cmd, pDateTime(CustomerBillBase.Property_UpdatedDate, customerBillObject.UpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerBill
        /// </summary>
        /// <param name="customerBillObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerBillBase customerBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERBILL);
	
				AddParameter(cmd, pInt32Out(CustomerBillBase.Property_Id));
				AddCommonParams(cmd, customerBillObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerBillObject.Id = (Int32)GetOutParameter(cmd, CustomerBillBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerBillObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerBill
        /// </summary>
        /// <param name="customerBillObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerBillBase customerBillObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERBILL);
				
				AddParameter(cmd, pInt32(CustomerBillBase.Property_Id, customerBillObject.Id));
				AddCommonParams(cmd, customerBillObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerBillObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerBill
        /// </summary>
        /// <param name="Id">Id of the CustomerBill object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERBILL);	
				
				AddParameter(cmd, pInt32(CustomerBillBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerBill), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerBill object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerBill object to retrieve</param>
        /// <returns>CustomerBill object, null if not found</returns>
		public CustomerBill Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLBYID))
			{
				AddParameter( cmd, pInt32(CustomerBillBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerBill objects 
        /// </summary>
        /// <returns>A list of CustomerBill objects</returns>
		public CustomerBillList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERBILL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerBill objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerBill objects</returns>
		public CustomerBillList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERBILL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerBillList _CustomerBillList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerBillList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerBill objects by query String
        /// </summary>
        /// <returns>A list of CustomerBill objects</returns>
		public CustomerBillList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerBill Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerBill Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerBill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerBillRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerBillRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerBillRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerBill object
        /// </summary>
        /// <param name="customerBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerBillBase customerBillObject, SqlDataReader reader, int start)
		{
			
				customerBillObject.Id = reader.GetInt32( start + 0 );			
				customerBillObject.BillNo = reader.GetString( start + 1 );			
				customerBillObject.CompanyId = reader.GetGuid( start + 2 );			
				customerBillObject.CustomerId = reader.GetGuid( start + 3 );			
				customerBillObject.Type = reader.GetString( start + 4 );			
				customerBillObject.Amount = reader.GetDouble( start + 5 );			
				customerBillObject.PaymentMethod = reader.GetString( start + 6 );			
				customerBillObject.PaymentStatus = reader.GetString( start + 7 );			
				customerBillObject.PaymentDate = reader.GetDateTime( start + 8 );			
				customerBillObject.PaymentDueDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) customerBillObject.BillCycle = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerBillObject.Notes = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerBillObject.UpdatedBy = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerBillObject.UpdatedDate = reader.GetDateTime( start + 13 );			
			FillBaseObject(customerBillObject, reader, (start + 14));

			
			customerBillObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerBill object
        /// </summary>
        /// <param name="customerBillObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerBillBase customerBillObject, SqlDataReader reader)
		{
			FillObject(customerBillObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerBill object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerBill object</returns>
		private CustomerBill GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerBill customerBillObject= new CustomerBill();
					FillObject(customerBillObject, reader);
					return customerBillObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerBill objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerBill objects</returns>
		private CustomerBillList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerBill list
			CustomerBillList list = new CustomerBillList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerBill customerBillObject = new CustomerBill();
					FillObject(customerBillObject, reader);

					list.Add(customerBillObject);
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
