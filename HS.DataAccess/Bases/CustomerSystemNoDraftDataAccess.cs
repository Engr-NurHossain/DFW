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
	public partial class CustomerSystemNoDraftDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSYSTEMNODRAFT = "InsertCustomerSystemNoDraft";
		private const string UPDATECUSTOMERSYSTEMNODRAFT = "UpdateCustomerSystemNoDraft";
		private const string DELETECUSTOMERSYSTEMNODRAFT = "DeleteCustomerSystemNoDraft";
		private const string GETCUSTOMERSYSTEMNODRAFTBYID = "GetCustomerSystemNoDraftById";
		private const string GETALLCUSTOMERSYSTEMNODRAFT = "GetAllCustomerSystemNoDraft";
		private const string GETPAGEDCUSTOMERSYSTEMNODRAFT = "GetPagedCustomerSystemNoDraft";
		private const string GETCUSTOMERSYSTEMNODRAFTMAXIMUMID = "GetCustomerSystemNoDraftMaximumId";
		private const string GETCUSTOMERSYSTEMNODRAFTROWCOUNT = "GetCustomerSystemNoDraftRowCount";	
		private const string GETCUSTOMERSYSTEMNODRAFTBYQUERY = "GetCustomerSystemNoDraftByQuery";
		#endregion
		
		#region Constructors
		public CustomerSystemNoDraftDataAccess(ClientContext context) : base(context) { }
		public CustomerSystemNoDraftDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSystemNoDraftObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSystemNoDraftBase customerSystemNoDraftObject)
		{	
			AddParameter(cmd, pGuid(CustomerSystemNoDraftBase.Property_CompanyId, customerSystemNoDraftObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSystemNoDraftBase.Property_CustomerNo, 50, customerSystemNoDraftObject.CustomerNo));
			AddParameter(cmd, pBool(CustomerSystemNoDraftBase.Property_IsUsed, customerSystemNoDraftObject.IsUsed));
			AddParameter(cmd, pBool(CustomerSystemNoDraftBase.Property_IsReserved, customerSystemNoDraftObject.IsReserved));
			AddParameter(cmd, pDateTime(CustomerSystemNoDraftBase.Property_GenerateDate, customerSystemNoDraftObject.GenerateDate));
			AddParameter(cmd, pDateTime(CustomerSystemNoDraftBase.Property_ReserveDate, customerSystemNoDraftObject.ReserveDate));
			AddParameter(cmd, pDateTime(CustomerSystemNoDraftBase.Property_UsedDate, customerSystemNoDraftObject.UsedDate));
			AddParameter(cmd, pInt32(CustomerSystemNoDraftBase.Property_CustomerId, customerSystemNoDraftObject.CustomerId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSystemNoDraft
        /// </summary>
        /// <param name="customerSystemNoDraftObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSystemNoDraftBase customerSystemNoDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSYSTEMNODRAFT);
	
				AddParameter(cmd, pInt32Out(CustomerSystemNoDraftBase.Property_Id));
				AddCommonParams(cmd, customerSystemNoDraftObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSystemNoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSystemNoDraftObject.Id = (Int32)GetOutParameter(cmd, CustomerSystemNoDraftBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSystemNoDraftObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSystemNoDraft
        /// </summary>
        /// <param name="customerSystemNoDraftObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSystemNoDraftBase customerSystemNoDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSYSTEMNODRAFT);
				
				AddParameter(cmd, pInt32(CustomerSystemNoDraftBase.Property_Id, customerSystemNoDraftObject.Id));
				AddCommonParams(cmd, customerSystemNoDraftObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSystemNoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSystemNoDraftObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSystemNoDraft
        /// </summary>
        /// <param name="Id">Id of the CustomerSystemNoDraft object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSYSTEMNODRAFT);	
				
				AddParameter(cmd, pInt32(CustomerSystemNoDraftBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSystemNoDraft), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSystemNoDraft object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSystemNoDraft object to retrieve</param>
        /// <returns>CustomerSystemNoDraft object, null if not found</returns>
		public CustomerSystemNoDraft Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNODRAFTBYID))
			{
				AddParameter( cmd, pInt32(CustomerSystemNoDraftBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSystemNoDraft objects 
        /// </summary>
        /// <returns>A list of CustomerSystemNoDraft objects</returns>
		public CustomerSystemNoDraftList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSYSTEMNODRAFT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSystemNoDraft objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSystemNoDraft objects</returns>
		public CustomerSystemNoDraftList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSYSTEMNODRAFT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSystemNoDraftList _CustomerSystemNoDraftList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSystemNoDraftList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSystemNoDraft objects by query String
        /// </summary>
        /// <returns>A list of CustomerSystemNoDraft objects</returns>
		public CustomerSystemNoDraftList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNODRAFTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSystemNoDraft Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSystemNoDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNODRAFTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSystemNoDraft Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSystemNoDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSystemNoDraftRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNODRAFTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSystemNoDraftRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSystemNoDraftRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSystemNoDraft object
        /// </summary>
        /// <param name="customerSystemNoDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSystemNoDraftBase customerSystemNoDraftObject, SqlDataReader reader, int start)
		{
			
				customerSystemNoDraftObject.Id = reader.GetInt32( start + 0 );			
				customerSystemNoDraftObject.CompanyId = reader.GetGuid( start + 1 );			
				customerSystemNoDraftObject.CustomerNo = reader.GetString( start + 2 );			
				customerSystemNoDraftObject.IsUsed = reader.GetBoolean( start + 3 );			
				customerSystemNoDraftObject.IsReserved = reader.GetBoolean( start + 4 );			
				customerSystemNoDraftObject.GenerateDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) customerSystemNoDraftObject.ReserveDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customerSystemNoDraftObject.UsedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) customerSystemNoDraftObject.CustomerId = reader.GetInt32( start + 8 );			
			FillBaseObject(customerSystemNoDraftObject, reader, (start + 9));

			
			customerSystemNoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSystemNoDraft object
        /// </summary>
        /// <param name="customerSystemNoDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSystemNoDraftBase customerSystemNoDraftObject, SqlDataReader reader)
		{
			FillObject(customerSystemNoDraftObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSystemNoDraft object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSystemNoDraft object</returns>
		private CustomerSystemNoDraft GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSystemNoDraft customerSystemNoDraftObject= new CustomerSystemNoDraft();
					FillObject(customerSystemNoDraftObject, reader);
					return customerSystemNoDraftObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSystemNoDraft objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSystemNoDraft objects</returns>
		private CustomerSystemNoDraftList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSystemNoDraft list
			CustomerSystemNoDraftList list = new CustomerSystemNoDraftList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSystemNoDraft customerSystemNoDraftObject = new CustomerSystemNoDraft();
					FillObject(customerSystemNoDraftObject, reader);

					list.Add(customerSystemNoDraftObject);
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
