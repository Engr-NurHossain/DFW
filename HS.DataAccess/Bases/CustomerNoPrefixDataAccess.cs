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
	public partial class CustomerNoPrefixDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERNOPREFIX = "InsertCustomerNoPrefix";
		private const string UPDATECUSTOMERNOPREFIX = "UpdateCustomerNoPrefix";
		private const string DELETECUSTOMERNOPREFIX = "DeleteCustomerNoPrefix";
		private const string GETCUSTOMERNOPREFIXBYID = "GetCustomerNoPrefixById";
		private const string GETALLCUSTOMERNOPREFIX = "GetAllCustomerNoPrefix";
		private const string GETPAGEDCUSTOMERNOPREFIX = "GetPagedCustomerNoPrefix";
		private const string GETCUSTOMERNOPREFIXMAXIMUMID = "GetCustomerNoPrefixMaximumId";
		private const string GETCUSTOMERNOPREFIXROWCOUNT = "GetCustomerNoPrefixRowCount";	
		private const string GETCUSTOMERNOPREFIXBYQUERY = "GetCustomerNoPrefixByQuery";
		#endregion
		
		#region Constructors
		public CustomerNoPrefixDataAccess(ClientContext context) : base(context) { }
		public CustomerNoPrefixDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerNoPrefixObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerNoPrefixBase customerNoPrefixObject)
		{	
			AddParameter(cmd, pGuid(CustomerNoPrefixBase.Property_CompanyId, customerNoPrefixObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerNoPrefixBase.Property_Name, 50, customerNoPrefixObject.Name));
			AddParameter(cmd, pNVarChar(CustomerNoPrefixBase.Property_CentralstationName, 100, customerNoPrefixObject.CentralstationName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerNoPrefix
        /// </summary>
        /// <param name="customerNoPrefixObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerNoPrefixBase customerNoPrefixObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERNOPREFIX);
	
				AddParameter(cmd, pInt32Out(CustomerNoPrefixBase.Property_Id));
				AddCommonParams(cmd, customerNoPrefixObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerNoPrefixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerNoPrefixObject.Id = (Int32)GetOutParameter(cmd, CustomerNoPrefixBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerNoPrefixObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerNoPrefix
        /// </summary>
        /// <param name="customerNoPrefixObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerNoPrefixBase customerNoPrefixObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERNOPREFIX);
				
				AddParameter(cmd, pInt32(CustomerNoPrefixBase.Property_Id, customerNoPrefixObject.Id));
				AddCommonParams(cmd, customerNoPrefixObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerNoPrefixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerNoPrefixObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerNoPrefix
        /// </summary>
        /// <param name="Id">Id of the CustomerNoPrefix object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERNOPREFIX);	
				
				AddParameter(cmd, pInt32(CustomerNoPrefixBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerNoPrefix), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerNoPrefix object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerNoPrefix object to retrieve</param>
        /// <returns>CustomerNoPrefix object, null if not found</returns>
		public CustomerNoPrefix Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOPREFIXBYID))
			{
				AddParameter( cmd, pInt32(CustomerNoPrefixBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerNoPrefix objects 
        /// </summary>
        /// <returns>A list of CustomerNoPrefix objects</returns>
		public CustomerNoPrefixList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERNOPREFIX))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerNoPrefix objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerNoPrefix objects</returns>
		public CustomerNoPrefixList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERNOPREFIX))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerNoPrefixList _CustomerNoPrefixList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerNoPrefixList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerNoPrefix objects by query String
        /// </summary>
        /// <returns>A list of CustomerNoPrefix objects</returns>
		public CustomerNoPrefixList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOPREFIXBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerNoPrefix Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerNoPrefix
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOPREFIXMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerNoPrefix Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerNoPrefix
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerNoPrefixRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERNOPREFIXROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerNoPrefixRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerNoPrefixRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerNoPrefix object
        /// </summary>
        /// <param name="customerNoPrefixObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerNoPrefixBase customerNoPrefixObject, SqlDataReader reader, int start)
		{
			
				customerNoPrefixObject.Id = reader.GetInt32( start + 0 );			
				customerNoPrefixObject.CompanyId = reader.GetGuid( start + 1 );			
				customerNoPrefixObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerNoPrefixObject.CentralstationName = reader.GetString( start + 3 );			
			FillBaseObject(customerNoPrefixObject, reader, (start + 4));

			
			customerNoPrefixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerNoPrefix object
        /// </summary>
        /// <param name="customerNoPrefixObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerNoPrefixBase customerNoPrefixObject, SqlDataReader reader)
		{
			FillObject(customerNoPrefixObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerNoPrefix object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerNoPrefix object</returns>
		private CustomerNoPrefix GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerNoPrefix customerNoPrefixObject= new CustomerNoPrefix();
					FillObject(customerNoPrefixObject, reader);
					return customerNoPrefixObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerNoPrefix objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerNoPrefix objects</returns>
		private CustomerNoPrefixList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerNoPrefix list
			CustomerNoPrefixList list = new CustomerNoPrefixList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerNoPrefix customerNoPrefixObject = new CustomerNoPrefix();
					FillObject(customerNoPrefixObject, reader);

					list.Add(customerNoPrefixObject);
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
