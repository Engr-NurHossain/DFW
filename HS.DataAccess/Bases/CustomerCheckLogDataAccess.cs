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
	public partial class CustomerCheckLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCHECKLOG = "InsertCustomerCheckLog";
		private const string UPDATECUSTOMERCHECKLOG = "UpdateCustomerCheckLog";
		private const string DELETECUSTOMERCHECKLOG = "DeleteCustomerCheckLog";
		private const string GETCUSTOMERCHECKLOGBYID = "GetCustomerCheckLogById";
		private const string GETALLCUSTOMERCHECKLOG = "GetAllCustomerCheckLog";
		private const string GETPAGEDCUSTOMERCHECKLOG = "GetPagedCustomerCheckLog";
		private const string GETCUSTOMERCHECKLOGMAXIMUMID = "GetCustomerCheckLogMaximumId";
		private const string GETCUSTOMERCHECKLOGROWCOUNT = "GetCustomerCheckLogRowCount";	
		private const string GETCUSTOMERCHECKLOGBYQUERY = "GetCustomerCheckLogByQuery";
		#endregion
		
		#region Constructors
		public CustomerCheckLogDataAccess(ClientContext context) : base(context) { }
		public CustomerCheckLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCheckLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCheckLogBase customerCheckLogObject)
		{	
			AddParameter(cmd, pGuid(CustomerCheckLogBase.Property_CustomerId, customerCheckLogObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerCheckLogBase.Property_RouteId, customerCheckLogObject.RouteId));
			AddParameter(cmd, pDateTime(CustomerCheckLogBase.Property_CheckInTime, customerCheckLogObject.CheckInTime));
			AddParameter(cmd, pDateTime(CustomerCheckLogBase.Property_CheckOutTime, customerCheckLogObject.CheckOutTime));
			AddParameter(cmd, pNVarChar(CustomerCheckLogBase.Property_CheckType, 50, customerCheckLogObject.CheckType));
			AddParameter(cmd, pDouble(CustomerCheckLogBase.Property_ToatlTime, customerCheckLogObject.ToatlTime));
			AddParameter(cmd, pGuid(CustomerCheckLogBase.Property_CreadtedBy, customerCheckLogObject.CreadtedBy));
			AddParameter(cmd, pDateTime(CustomerCheckLogBase.Property_CreatedDate, customerCheckLogObject.CreatedDate));
			AddParameter(cmd, pInt32(CustomerCheckLogBase.Property_GeeseCount, customerCheckLogObject.GeeseCount));
			AddParameter(cmd, pGuid(CustomerCheckLogBase.Property_UserId, customerCheckLogObject.UserId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCheckLog
        /// </summary>
        /// <param name="customerCheckLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCheckLogBase customerCheckLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCHECKLOG);
	
				AddParameter(cmd, pInt32Out(CustomerCheckLogBase.Property_Id));
				AddCommonParams(cmd, customerCheckLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCheckLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCheckLogObject.Id = (Int32)GetOutParameter(cmd, CustomerCheckLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCheckLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCheckLog
        /// </summary>
        /// <param name="customerCheckLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCheckLogBase customerCheckLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCHECKLOG);
				
				AddParameter(cmd, pInt32(CustomerCheckLogBase.Property_Id, customerCheckLogObject.Id));
				AddCommonParams(cmd, customerCheckLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCheckLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCheckLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCheckLog
        /// </summary>
        /// <param name="Id">Id of the CustomerCheckLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCHECKLOG);	
				
				AddParameter(cmd, pInt32(CustomerCheckLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCheckLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCheckLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCheckLog object to retrieve</param>
        /// <returns>CustomerCheckLog object, null if not found</returns>
		public CustomerCheckLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCHECKLOGBYID))
			{
				AddParameter( cmd, pInt32(CustomerCheckLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCheckLog objects 
        /// </summary>
        /// <returns>A list of CustomerCheckLog objects</returns>
		public CustomerCheckLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCHECKLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCheckLog objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCheckLog objects</returns>
		public CustomerCheckLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCHECKLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCheckLogList _CustomerCheckLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCheckLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCheckLog objects by query String
        /// </summary>
        /// <returns>A list of CustomerCheckLog objects</returns>
		public CustomerCheckLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCHECKLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCheckLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCheckLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCHECKLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCheckLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCheckLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCheckLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCHECKLOGROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCheckLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCheckLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCheckLog object
        /// </summary>
        /// <param name="customerCheckLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCheckLogBase customerCheckLogObject, SqlDataReader reader, int start)
		{
			
				customerCheckLogObject.Id = reader.GetInt32( start + 0 );			
				customerCheckLogObject.CustomerId = reader.GetGuid( start + 1 );			
				customerCheckLogObject.RouteId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerCheckLogObject.CheckInTime = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) customerCheckLogObject.CheckOutTime = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) customerCheckLogObject.CheckType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerCheckLogObject.ToatlTime = reader.GetDouble( start + 6 );			
				customerCheckLogObject.CreadtedBy = reader.GetGuid( start + 7 );			
				customerCheckLogObject.CreatedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) customerCheckLogObject.GeeseCount = reader.GetInt32( start + 9 );			
				customerCheckLogObject.UserId = reader.GetGuid( start + 10 );			
			FillBaseObject(customerCheckLogObject, reader, (start + 11));

			
			customerCheckLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCheckLog object
        /// </summary>
        /// <param name="customerCheckLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCheckLogBase customerCheckLogObject, SqlDataReader reader)
		{
			FillObject(customerCheckLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCheckLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCheckLog object</returns>
		private CustomerCheckLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCheckLog customerCheckLogObject= new CustomerCheckLog();
					FillObject(customerCheckLogObject, reader);
					return customerCheckLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCheckLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCheckLog objects</returns>
		private CustomerCheckLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCheckLog list
			CustomerCheckLogList list = new CustomerCheckLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCheckLog customerCheckLogObject = new CustomerCheckLog();
					FillObject(customerCheckLogObject, reader);

					list.Add(customerCheckLogObject);
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
