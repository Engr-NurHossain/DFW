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
	public partial class CustomerCreditDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCREDIT = "InsertCustomerCredit";
		private const string UPDATECUSTOMERCREDIT = "UpdateCustomerCredit";
		private const string DELETECUSTOMERCREDIT = "DeleteCustomerCredit";
		private const string GETCUSTOMERCREDITBYID = "GetCustomerCreditById";
		private const string GETALLCUSTOMERCREDIT = "GetAllCustomerCredit";
		private const string GETPAGEDCUSTOMERCREDIT = "GetPagedCustomerCredit";
		private const string GETCUSTOMERCREDITMAXIMUMID = "GetCustomerCreditMaximumId";
		private const string GETCUSTOMERCREDITROWCOUNT = "GetCustomerCreditRowCount";	
		private const string GETCUSTOMERCREDITBYQUERY = "GetCustomerCreditByQuery";
		#endregion
		
		#region Constructors
		public CustomerCreditDataAccess(ClientContext context) : base(context) { }
		public CustomerCreditDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCreditObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCreditBase customerCreditObject)
		{	
			AddParameter(cmd, pGuid(CustomerCreditBase.Property_CompanyId, customerCreditObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerCreditBase.Property_CustomerId, customerCreditObject.CustomerId));
			AddParameter(cmd, pInt32(CustomerCreditBase.Property_TransactionId, customerCreditObject.TransactionId));
			AddParameter(cmd, pNVarChar(CustomerCreditBase.Property_Type, 50, customerCreditObject.Type));
			AddParameter(cmd, pDouble(CustomerCreditBase.Property_Amount, customerCreditObject.Amount));
			AddParameter(cmd, pGuid(CustomerCreditBase.Property_CreatedBy, customerCreditObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerCreditBase.Property_CreatedDate, customerCreditObject.CreatedDate));
			AddParameter(cmd, pBool(CustomerCreditBase.Property_IsRefund, customerCreditObject.IsRefund));
			AddParameter(cmd, pNVarChar(CustomerCreditBase.Property_Note, customerCreditObject.Note));
			AddParameter(cmd, pBool(CustomerCreditBase.Property_IsRMRCredit, customerCreditObject.IsRMRCredit));
			AddParameter(cmd, pBool(CustomerCreditBase.Property_IsDeleted, customerCreditObject.IsDeleted));
			AddParameter(cmd, pNVarChar(CustomerCreditBase.Property_CreditReason, 500, customerCreditObject.CreditReason));
			AddParameter(cmd, pInt32(CustomerCreditBase.Property_DebitRefId, customerCreditObject.DebitRefId));
			AddParameter(cmd, pNVarChar(CustomerCreditBase.Property_CreditType, 50, customerCreditObject.CreditType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCredit
        /// </summary>
        /// <param name="customerCreditObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCreditBase customerCreditObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCREDIT);
	
				AddParameter(cmd, pInt32Out(CustomerCreditBase.Property_Id));
				AddCommonParams(cmd, customerCreditObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCreditObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCreditObject.Id = (Int32)GetOutParameter(cmd, CustomerCreditBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCreditObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCredit
        /// </summary>
        /// <param name="customerCreditObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCreditBase customerCreditObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCREDIT);
				
				AddParameter(cmd, pInt32(CustomerCreditBase.Property_Id, customerCreditObject.Id));
				AddCommonParams(cmd, customerCreditObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCreditObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCreditObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCredit
        /// </summary>
        /// <param name="Id">Id of the CustomerCredit object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCREDIT);	
				
				AddParameter(cmd, pInt32(CustomerCreditBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCredit), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCredit object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCredit object to retrieve</param>
        /// <returns>CustomerCredit object, null if not found</returns>
		public CustomerCredit Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITBYID))
			{
				AddParameter( cmd, pInt32(CustomerCreditBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCredit objects 
        /// </summary>
        /// <returns>A list of CustomerCredit objects</returns>
		public CustomerCreditList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCREDIT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCredit objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCredit objects</returns>
		public CustomerCreditList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCREDIT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCreditList _CustomerCreditList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCreditList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCredit objects by query String
        /// </summary>
        /// <returns>A list of CustomerCredit objects</returns>
		public CustomerCreditList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCredit Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCredit
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCredit Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCredit
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCreditRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCREDITROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCreditRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCreditRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCredit object
        /// </summary>
        /// <param name="customerCreditObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCreditBase customerCreditObject, SqlDataReader reader, int start)
		{
			
				customerCreditObject.Id = reader.GetInt32( start + 0 );			
				customerCreditObject.CompanyId = reader.GetGuid( start + 1 );			
				customerCreditObject.CustomerId = reader.GetGuid( start + 2 );			
				customerCreditObject.TransactionId = reader.GetInt32( start + 3 );			
				customerCreditObject.Type = reader.GetString( start + 4 );			
				customerCreditObject.Amount = reader.GetDouble( start + 5 );			
				customerCreditObject.CreatedBy = reader.GetGuid( start + 6 );			
				customerCreditObject.CreatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) customerCreditObject.IsRefund = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) customerCreditObject.Note = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerCreditObject.IsRMRCredit = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) customerCreditObject.IsDeleted = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) customerCreditObject.CreditReason = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerCreditObject.DebitRefId = reader.GetInt32( start + 13 );			
				if(!reader.IsDBNull(14)) customerCreditObject.CreditType = reader.GetString( start + 14 );			
			FillBaseObject(customerCreditObject, reader, (start + 15));

			
			customerCreditObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCredit object
        /// </summary>
        /// <param name="customerCreditObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCreditBase customerCreditObject, SqlDataReader reader)
		{
			FillObject(customerCreditObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCredit object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCredit object</returns>
		private CustomerCredit GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCredit customerCreditObject= new CustomerCredit();
					FillObject(customerCreditObject, reader);
					return customerCreditObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCredit objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCredit objects</returns>
		private CustomerCreditList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCredit list
			CustomerCreditList list = new CustomerCreditList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCredit customerCreditObject = new CustomerCredit();
					FillObject(customerCreditObject, reader);

					list.Add(customerCreditObject);
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
