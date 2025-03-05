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
	public partial class CustomerMigrationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERMIGRATION = "InsertCustomerMigration";
		private const string UPDATECUSTOMERMIGRATION = "UpdateCustomerMigration";
		private const string DELETECUSTOMERMIGRATION = "DeleteCustomerMigration";
		private const string GETCUSTOMERMIGRATIONBYID = "GetCustomerMigrationById";
		private const string GETALLCUSTOMERMIGRATION = "GetAllCustomerMigration";
		private const string GETPAGEDCUSTOMERMIGRATION = "GetPagedCustomerMigration";
		private const string GETCUSTOMERMIGRATIONMAXIMUMID = "GetCustomerMigrationMaximumId";
		private const string GETCUSTOMERMIGRATIONROWCOUNT = "GetCustomerMigrationRowCount";	
		private const string GETCUSTOMERMIGRATIONBYQUERY = "GetCustomerMigrationByQuery";
		#endregion
		
		#region Constructors
		public CustomerMigrationDataAccess(ClientContext context) : base(context) { }
		public CustomerMigrationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerMigrationObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerMigrationBase customerMigrationObject)
		{	
			AddParameter(cmd, pGuid(CustomerMigrationBase.Property_CompanyId, customerMigrationObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerMigrationBase.Property_CustomerId, customerMigrationObject.CustomerId));
			AddParameter(cmd, pInt32(CustomerMigrationBase.Property_RefenrenceId, customerMigrationObject.RefenrenceId));
			AddParameter(cmd, pNVarChar(CustomerMigrationBase.Property_Platform, 50, customerMigrationObject.Platform));
			AddParameter(cmd, pNVarChar(CustomerMigrationBase.Property_Note, customerMigrationObject.Note));
			AddParameter(cmd, pDateTime(CustomerMigrationBase.Property_CreatedDate, customerMigrationObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerMigrationBase.Property_CreatedBy, customerMigrationObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerMigration
        /// </summary>
        /// <param name="customerMigrationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerMigrationBase customerMigrationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERMIGRATION);
	
				AddParameter(cmd, pInt32Out(CustomerMigrationBase.Property_Id));
				AddCommonParams(cmd, customerMigrationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerMigrationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerMigrationObject.Id = (Int32)GetOutParameter(cmd, CustomerMigrationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerMigrationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerMigration
        /// </summary>
        /// <param name="customerMigrationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerMigrationBase customerMigrationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERMIGRATION);
				
				AddParameter(cmd, pInt32(CustomerMigrationBase.Property_Id, customerMigrationObject.Id));
				AddCommonParams(cmd, customerMigrationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerMigrationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerMigrationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerMigration
        /// </summary>
        /// <param name="Id">Id of the CustomerMigration object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERMIGRATION);	
				
				AddParameter(cmd, pInt32(CustomerMigrationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerMigration), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerMigration object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerMigration object to retrieve</param>
        /// <returns>CustomerMigration object, null if not found</returns>
		public CustomerMigration Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERMIGRATIONBYID))
			{
				AddParameter( cmd, pInt32(CustomerMigrationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerMigration objects 
        /// </summary>
        /// <returns>A list of CustomerMigration objects</returns>
		public CustomerMigrationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERMIGRATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerMigration objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerMigration objects</returns>
		public CustomerMigrationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERMIGRATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerMigrationList _CustomerMigrationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerMigrationList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerMigration objects by query String
        /// </summary>
        /// <returns>A list of CustomerMigration objects</returns>
		public CustomerMigrationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERMIGRATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerMigration Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerMigration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERMIGRATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerMigration Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerMigration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerMigrationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERMIGRATIONROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerMigrationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerMigrationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerMigration object
        /// </summary>
        /// <param name="customerMigrationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerMigrationBase customerMigrationObject, SqlDataReader reader, int start)
		{
			
				customerMigrationObject.Id = reader.GetInt32( start + 0 );			
				customerMigrationObject.CompanyId = reader.GetGuid( start + 1 );			
				customerMigrationObject.CustomerId = reader.GetGuid( start + 2 );			
				customerMigrationObject.RefenrenceId = reader.GetInt32( start + 3 );			
				customerMigrationObject.Platform = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerMigrationObject.Note = reader.GetString( start + 5 );			
				customerMigrationObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customerMigrationObject.CreatedBy = reader.GetGuid( start + 7 );			
			FillBaseObject(customerMigrationObject, reader, (start + 8));

			
			customerMigrationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerMigration object
        /// </summary>
        /// <param name="customerMigrationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerMigrationBase customerMigrationObject, SqlDataReader reader)
		{
			FillObject(customerMigrationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerMigration object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerMigration object</returns>
		private CustomerMigration GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerMigration customerMigrationObject= new CustomerMigration();
					FillObject(customerMigrationObject, reader);
					return customerMigrationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerMigration objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerMigration objects</returns>
		private CustomerMigrationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerMigration list
			CustomerMigrationList list = new CustomerMigrationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerMigration customerMigrationObject = new CustomerMigration();
					FillObject(customerMigrationObject, reader);

					list.Add(customerMigrationObject);
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
