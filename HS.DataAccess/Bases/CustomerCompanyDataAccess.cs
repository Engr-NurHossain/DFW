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
	public partial class CustomerCompanyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCOMPANY = "InsertCustomerCompany";
		private const string UPDATECUSTOMERCOMPANY = "UpdateCustomerCompany";
		private const string DELETECUSTOMERCOMPANY = "DeleteCustomerCompany";
		private const string GETCUSTOMERCOMPANYBYID = "GetCustomerCompanyById";
		private const string GETALLCUSTOMERCOMPANY = "GetAllCustomerCompany";
		private const string GETPAGEDCUSTOMERCOMPANY = "GetPagedCustomerCompany";
		private const string GETCUSTOMERCOMPANYMAXIMUMID = "GetCustomerCompanyMaximumId";
		private const string GETCUSTOMERCOMPANYROWCOUNT = "GetCustomerCompanyRowCount";	
		private const string GETCUSTOMERCOMPANYBYQUERY = "GetCustomerCompanyByQuery";
		#endregion
		
		#region Constructors
		public CustomerCompanyDataAccess(ClientContext context) : base(context) { }
		public CustomerCompanyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCompanyObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCompanyBase customerCompanyObject)
		{	
			AddParameter(cmd, pGuid(CustomerCompanyBase.Property_CustomerId, customerCompanyObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerCompanyBase.Property_CompanyId, customerCompanyObject.CompanyId));
			AddParameter(cmd, pBool(CustomerCompanyBase.Property_IsLead, customerCompanyObject.IsLead));
			AddParameter(cmd, pDateTime(CustomerCompanyBase.Property_ConvertionDate, customerCompanyObject.ConvertionDate));
			AddParameter(cmd, pNVarChar(CustomerCompanyBase.Property_ConvertionType, 50, customerCompanyObject.ConvertionType));
			AddParameter(cmd, pBool(CustomerCompanyBase.Property_IsActive, customerCompanyObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCompany
        /// </summary>
        /// <param name="customerCompanyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCompanyBase customerCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCOMPANY);
	
				AddParameter(cmd, pInt32Out(CustomerCompanyBase.Property_Id));
				AddCommonParams(cmd, customerCompanyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCompanyObject.Id = (Int32)GetOutParameter(cmd, CustomerCompanyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCompanyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCompany
        /// </summary>
        /// <param name="customerCompanyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCompanyBase customerCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCOMPANY);
				
				AddParameter(cmd, pInt32(CustomerCompanyBase.Property_Id, customerCompanyObject.Id));
				AddCommonParams(cmd, customerCompanyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCompanyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCompany
        /// </summary>
        /// <param name="Id">Id of the CustomerCompany object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCOMPANY);	
				
				AddParameter(cmd, pInt32(CustomerCompanyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCompany), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCompany object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCompany object to retrieve</param>
        /// <returns>CustomerCompany object, null if not found</returns>
		public CustomerCompany Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCOMPANYBYID))
			{
				AddParameter( cmd, pInt32(CustomerCompanyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCompany objects 
        /// </summary>
        /// <returns>A list of CustomerCompany objects</returns>
		public CustomerCompanyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCOMPANY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCompany objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCompany objects</returns>
		public CustomerCompanyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCOMPANY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCompanyList _CustomerCompanyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCompanyList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCompany objects by query String
        /// </summary>
        /// <returns>A list of CustomerCompany objects</returns>
		public CustomerCompanyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCOMPANYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCompany Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCOMPANYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCompany Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCompanyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCOMPANYROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCompanyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCompanyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCompany object
        /// </summary>
        /// <param name="customerCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCompanyBase customerCompanyObject, SqlDataReader reader, int start)
		{
			
				customerCompanyObject.Id = reader.GetInt32( start + 0 );			
				customerCompanyObject.CustomerId = reader.GetGuid( start + 1 );			
				customerCompanyObject.CompanyId = reader.GetGuid( start + 2 );			
				customerCompanyObject.IsLead = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) customerCompanyObject.ConvertionDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) customerCompanyObject.ConvertionType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerCompanyObject.IsActive = reader.GetBoolean( start + 6 );			
			FillBaseObject(customerCompanyObject, reader, (start + 7));

			
			customerCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCompany object
        /// </summary>
        /// <param name="customerCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCompanyBase customerCompanyObject, SqlDataReader reader)
		{
			FillObject(customerCompanyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCompany object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCompany object</returns>
		private CustomerCompany GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCompany customerCompanyObject= new CustomerCompany();
					FillObject(customerCompanyObject, reader);
					return customerCompanyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCompany objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCompany objects</returns>
		private CustomerCompanyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCompany list
			CustomerCompanyList list = new CustomerCompanyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCompany customerCompanyObject = new CustomerCompany();
					FillObject(customerCompanyObject, reader);

					list.Add(customerCompanyObject);
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
