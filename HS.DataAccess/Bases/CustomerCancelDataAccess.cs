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
	public partial class CustomerCancelDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCANCEL = "InsertCustomerCancel";
		private const string UPDATECUSTOMERCANCEL = "UpdateCustomerCancel";
		private const string DELETECUSTOMERCANCEL = "DeleteCustomerCancel";
		private const string GETCUSTOMERCANCELBYID = "GetCustomerCancelById";
		private const string GETALLCUSTOMERCANCEL = "GetAllCustomerCancel";
		private const string GETPAGEDCUSTOMERCANCEL = "GetPagedCustomerCancel";
		private const string GETCUSTOMERCANCELMAXIMUMID = "GetCustomerCancelMaximumId";
		private const string GETCUSTOMERCANCELROWCOUNT = "GetCustomerCancelRowCount";	
		private const string GETCUSTOMERCANCELBYQUERY = "GetCustomerCancelByQuery";
		#endregion
		
		#region Constructors
		public CustomerCancelDataAccess(ClientContext context) : base(context) { }
		public CustomerCancelDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCancelObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCancelBase customerCancelObject)
		{	
			AddParameter(cmd, pGuid(CustomerCancelBase.Property_CompanyId, customerCancelObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerCancelBase.Property_CustomerId, customerCancelObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerCancelBase.Property_EmployeeId, customerCancelObject.EmployeeId));
			AddParameter(cmd, pDateTime(CustomerCancelBase.Property_CancelDatet, customerCancelObject.CancelDatet));
			AddParameter(cmd, pNVarChar(CustomerCancelBase.Property_CancelReason, customerCancelObject.CancelReason));
			AddParameter(cmd, pBool(CustomerCancelBase.Property_IsActivated, customerCancelObject.IsActivated));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCancel
        /// </summary>
        /// <param name="customerCancelObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCancelBase customerCancelObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCANCEL);
	
				AddParameter(cmd, pInt32Out(CustomerCancelBase.Property_Id));
				AddCommonParams(cmd, customerCancelObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCancelObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCancelObject.Id = (Int32)GetOutParameter(cmd, CustomerCancelBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCancelObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCancel
        /// </summary>
        /// <param name="customerCancelObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCancelBase customerCancelObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCANCEL);
				
				AddParameter(cmd, pInt32(CustomerCancelBase.Property_Id, customerCancelObject.Id));
				AddCommonParams(cmd, customerCancelObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCancelObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCancelObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCancel
        /// </summary>
        /// <param name="Id">Id of the CustomerCancel object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCANCEL);	
				
				AddParameter(cmd, pInt32(CustomerCancelBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCancel), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCancel object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCancel object to retrieve</param>
        /// <returns>CustomerCancel object, null if not found</returns>
		public CustomerCancel Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELBYID))
			{
				AddParameter( cmd, pInt32(CustomerCancelBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCancel objects 
        /// </summary>
        /// <returns>A list of CustomerCancel objects</returns>
		public CustomerCancelList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCANCEL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCancel objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCancel objects</returns>
		public CustomerCancelList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCANCEL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCancelList _CustomerCancelList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCancelList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCancel objects by query String
        /// </summary>
        /// <returns>A list of CustomerCancel objects</returns>
		public CustomerCancelList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCancel Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCancel
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCancel Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCancel
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCancelRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCancelRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCancelRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCancel object
        /// </summary>
        /// <param name="customerCancelObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCancelBase customerCancelObject, SqlDataReader reader, int start)
		{
			
				customerCancelObject.Id = reader.GetInt32( start + 0 );			
				customerCancelObject.CompanyId = reader.GetGuid( start + 1 );			
				customerCancelObject.CustomerId = reader.GetGuid( start + 2 );			
				customerCancelObject.EmployeeId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) customerCancelObject.CancelDatet = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) customerCancelObject.CancelReason = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerCancelObject.IsActivated = reader.GetBoolean( start + 6 );			
			FillBaseObject(customerCancelObject, reader, (start + 7));

			
			customerCancelObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCancel object
        /// </summary>
        /// <param name="customerCancelObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCancelBase customerCancelObject, SqlDataReader reader)
		{
			FillObject(customerCancelObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCancel object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCancel object</returns>
		private CustomerCancel GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCancel customerCancelObject= new CustomerCancel();
					FillObject(customerCancelObject, reader);
					return customerCancelObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCancel objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCancel objects</returns>
		private CustomerCancelList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCancel list
			CustomerCancelList list = new CustomerCancelList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCancel customerCancelObject = new CustomerCancel();
					FillObject(customerCancelObject, reader);

					list.Add(customerCancelObject);
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
