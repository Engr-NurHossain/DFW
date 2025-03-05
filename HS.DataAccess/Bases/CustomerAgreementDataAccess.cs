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
	public partial class CustomerAgreementDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAGREEMENT = "InsertCustomerAgreement";
		private const string UPDATECUSTOMERAGREEMENT = "UpdateCustomerAgreement";
		private const string DELETECUSTOMERAGREEMENT = "DeleteCustomerAgreement";
		private const string GETCUSTOMERAGREEMENTBYID = "GetCustomerAgreementById";
		private const string GETALLCUSTOMERAGREEMENT = "GetAllCustomerAgreement";
		private const string GETPAGEDCUSTOMERAGREEMENT = "GetPagedCustomerAgreement";
		private const string GETCUSTOMERAGREEMENTMAXIMUMID = "GetCustomerAgreementMaximumId";
		private const string GETCUSTOMERAGREEMENTROWCOUNT = "GetCustomerAgreementRowCount";	
		private const string GETCUSTOMERAGREEMENTBYQUERY = "GetCustomerAgreementByQuery";
		#endregion
		
		#region Constructors
		public CustomerAgreementDataAccess(ClientContext context) : base(context) { }
		public CustomerAgreementDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAgreementObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAgreementBase customerAgreementObject)
		{	
			AddParameter(cmd, pGuid(CustomerAgreementBase.Property_CustomerId, customerAgreementObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerAgreementBase.Property_CompanyId, customerAgreementObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerAgreementBase.Property_InvoiceId, 50, customerAgreementObject.InvoiceId));
			AddParameter(cmd, pNVarChar(CustomerAgreementBase.Property_IP, 100, customerAgreementObject.IP));
			AddParameter(cmd, pNVarChar(CustomerAgreementBase.Property_UserAgent, 1000, customerAgreementObject.UserAgent));
			AddParameter(cmd, pNVarChar(CustomerAgreementBase.Property_Type, 50, customerAgreementObject.Type));
			AddParameter(cmd, pDateTime(CustomerAgreementBase.Property_AddedDate, customerAgreementObject.AddedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAgreement
        /// </summary>
        /// <param name="customerAgreementObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAgreementBase customerAgreementObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAGREEMENT);
	
				AddParameter(cmd, pInt32Out(CustomerAgreementBase.Property_Id));
				AddCommonParams(cmd, customerAgreementObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAgreementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAgreementObject.Id = (Int32)GetOutParameter(cmd, CustomerAgreementBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAgreementObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAgreement
        /// </summary>
        /// <param name="customerAgreementObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAgreementBase customerAgreementObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAGREEMENT);
				
				AddParameter(cmd, pInt32(CustomerAgreementBase.Property_Id, customerAgreementObject.Id));
				AddCommonParams(cmd, customerAgreementObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAgreementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAgreementObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAgreement
        /// </summary>
        /// <param name="Id">Id of the CustomerAgreement object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAGREEMENT);	
				
				AddParameter(cmd, pInt32(CustomerAgreementBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAgreement), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAgreement object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAgreement object to retrieve</param>
        /// <returns>CustomerAgreement object, null if not found</returns>
		public CustomerAgreement Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTBYID))
			{
				AddParameter( cmd, pInt32(CustomerAgreementBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAgreement objects 
        /// </summary>
        /// <returns>A list of CustomerAgreement objects</returns>
		public CustomerAgreementList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAGREEMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAgreement objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAgreement objects</returns>
		public CustomerAgreementList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAGREEMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAgreementList _CustomerAgreementList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAgreementList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAgreement objects by query String
        /// </summary>
        /// <returns>A list of CustomerAgreement objects</returns>
		public CustomerAgreementList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAgreement Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAgreement
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAgreement Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAgreement
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAgreementRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAGREEMENTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAgreementRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAgreementRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAgreement object
        /// </summary>
        /// <param name="customerAgreementObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAgreementBase customerAgreementObject, SqlDataReader reader, int start)
		{
			
				customerAgreementObject.Id = reader.GetInt32( start + 0 );			
				customerAgreementObject.CustomerId = reader.GetGuid( start + 1 );			
				customerAgreementObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerAgreementObject.InvoiceId = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerAgreementObject.IP = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerAgreementObject.UserAgent = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerAgreementObject.Type = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerAgreementObject.AddedDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(customerAgreementObject, reader, (start + 8));

			
			customerAgreementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAgreement object
        /// </summary>
        /// <param name="customerAgreementObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAgreementBase customerAgreementObject, SqlDataReader reader)
		{
			FillObject(customerAgreementObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAgreement object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAgreement object</returns>
		private CustomerAgreement GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAgreement customerAgreementObject= new CustomerAgreement();
					FillObject(customerAgreementObject, reader);
					return customerAgreementObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAgreement objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAgreement objects</returns>
		private CustomerAgreementList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAgreement list
			CustomerAgreementList list = new CustomerAgreementList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAgreement customerAgreementObject = new CustomerAgreement();
					FillObject(customerAgreementObject, reader);

					list.Add(customerAgreementObject);
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
