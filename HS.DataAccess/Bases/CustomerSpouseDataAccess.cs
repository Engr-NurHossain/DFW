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
	public partial class CustomerSpouseDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSPOUSE = "InsertCustomerSpouse";
		private const string UPDATECUSTOMERSPOUSE = "UpdateCustomerSpouse";
		private const string DELETECUSTOMERSPOUSE = "DeleteCustomerSpouse";
		private const string GETCUSTOMERSPOUSEBYID = "GetCustomerSpouseById";
		private const string GETALLCUSTOMERSPOUSE = "GetAllCustomerSpouse";
		private const string GETPAGEDCUSTOMERSPOUSE = "GetPagedCustomerSpouse";
		private const string GETCUSTOMERSPOUSEMAXIMUMID = "GetCustomerSpouseMaximumId";
		private const string GETCUSTOMERSPOUSEROWCOUNT = "GetCustomerSpouseRowCount";	
		private const string GETCUSTOMERSPOUSEBYQUERY = "GetCustomerSpouseByQuery";
		#endregion
		
		#region Constructors
		public CustomerSpouseDataAccess(ClientContext context) : base(context) { }
		public CustomerSpouseDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSpouseObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSpouseBase customerSpouseObject)
		{	
			AddParameter(cmd, pGuid(CustomerSpouseBase.Property_CustomerId, customerSpouseObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerSpouseBase.Property_CompanyId, customerSpouseObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSpouseBase.Property_FirstName, 50, customerSpouseObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerSpouseBase.Property_LastName, 50, customerSpouseObject.LastName));
			AddParameter(cmd, pDateTime(CustomerSpouseBase.Property_DateofBirth, customerSpouseObject.DateofBirth));
			AddParameter(cmd, pNVarChar(CustomerSpouseBase.Property_SSN, 50, customerSpouseObject.SSN));
			AddParameter(cmd, pDateTime(CustomerSpouseBase.Property_AddedDate, customerSpouseObject.AddedDate));
			AddParameter(cmd, pDateTime(CustomerSpouseBase.Property_CreditCheckDate, customerSpouseObject.CreditCheckDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSpouse
        /// </summary>
        /// <param name="customerSpouseObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSpouseBase customerSpouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSPOUSE);
	
				AddParameter(cmd, pInt32Out(CustomerSpouseBase.Property_Id));
				AddCommonParams(cmd, customerSpouseObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSpouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSpouseObject.Id = (Int32)GetOutParameter(cmd, CustomerSpouseBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSpouseObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSpouse
        /// </summary>
        /// <param name="customerSpouseObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSpouseBase customerSpouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSPOUSE);
				
				AddParameter(cmd, pInt32(CustomerSpouseBase.Property_Id, customerSpouseObject.Id));
				AddCommonParams(cmd, customerSpouseObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSpouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSpouseObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSpouse
        /// </summary>
        /// <param name="Id">Id of the CustomerSpouse object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSPOUSE);	
				
				AddParameter(cmd, pInt32(CustomerSpouseBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSpouse), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSpouse object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSpouse object to retrieve</param>
        /// <returns>CustomerSpouse object, null if not found</returns>
		public CustomerSpouse Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSPOUSEBYID))
			{
				AddParameter( cmd, pInt32(CustomerSpouseBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSpouse objects 
        /// </summary>
        /// <returns>A list of CustomerSpouse objects</returns>
		public CustomerSpouseList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSPOUSE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSpouse objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSpouse objects</returns>
		public CustomerSpouseList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSPOUSE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSpouseList _CustomerSpouseList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSpouseList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSpouse objects by query String
        /// </summary>
        /// <returns>A list of CustomerSpouse objects</returns>
		public CustomerSpouseList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSPOUSEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSpouse Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSpouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSPOUSEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSpouse Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSpouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSpouseRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSPOUSEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSpouseRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSpouseRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSpouse object
        /// </summary>
        /// <param name="customerSpouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSpouseBase customerSpouseObject, SqlDataReader reader, int start)
		{
			
				customerSpouseObject.Id = reader.GetInt32( start + 0 );			
				customerSpouseObject.CustomerId = reader.GetGuid( start + 1 );			
				customerSpouseObject.CompanyId = reader.GetGuid( start + 2 );			
				customerSpouseObject.FirstName = reader.GetString( start + 3 );			
				customerSpouseObject.LastName = reader.GetString( start + 4 );			
				customerSpouseObject.DateofBirth = reader.GetDateTime( start + 5 );			
				customerSpouseObject.SSN = reader.GetString( start + 6 );			
				customerSpouseObject.AddedDate = reader.GetDateTime( start + 7 );			
				customerSpouseObject.CreditCheckDate = reader.GetDateTime( start + 8 );			
			FillBaseObject(customerSpouseObject, reader, (start + 9));

			
			customerSpouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSpouse object
        /// </summary>
        /// <param name="customerSpouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSpouseBase customerSpouseObject, SqlDataReader reader)
		{
			FillObject(customerSpouseObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSpouse object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSpouse object</returns>
		private CustomerSpouse GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSpouse customerSpouseObject= new CustomerSpouse();
					FillObject(customerSpouseObject, reader);
					return customerSpouseObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSpouse objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSpouse objects</returns>
		private CustomerSpouseList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSpouse list
			CustomerSpouseList list = new CustomerSpouseList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSpouse customerSpouseObject = new CustomerSpouse();
					FillObject(customerSpouseObject, reader);

					list.Add(customerSpouseObject);
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
