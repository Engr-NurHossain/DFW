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
	public partial class CustomerAddendumDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERADDENDUM = "InsertCustomerAddendum";
		private const string UPDATECUSTOMERADDENDUM = "UpdateCustomerAddendum";
		private const string DELETECUSTOMERADDENDUM = "DeleteCustomerAddendum";
		private const string GETCUSTOMERADDENDUMBYID = "GetCustomerAddendumById";
		private const string GETALLCUSTOMERADDENDUM = "GetAllCustomerAddendum";
		private const string GETPAGEDCUSTOMERADDENDUM = "GetPagedCustomerAddendum";
		private const string GETCUSTOMERADDENDUMMAXIMUMID = "GetCustomerAddendumMaximumId";
		private const string GETCUSTOMERADDENDUMROWCOUNT = "GetCustomerAddendumRowCount";	
		private const string GETCUSTOMERADDENDUMBYQUERY = "GetCustomerAddendumByQuery";
		#endregion
		
		#region Constructors
		public CustomerAddendumDataAccess(ClientContext context) : base(context) { }
		public CustomerAddendumDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAddendumObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAddendumBase customerAddendumObject)
		{	
			AddParameter(cmd, pGuid(CustomerAddendumBase.Property_CustomerId, customerAddendumObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerAddendumBase.Property_TicketId, customerAddendumObject.TicketId));
			AddParameter(cmd, pNVarChar(CustomerAddendumBase.Property_Signature, customerAddendumObject.Signature));
			AddParameter(cmd, pDateTime(CustomerAddendumBase.Property_CreatedDate, customerAddendumObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerAddendumBase.Property_CreatedBy, customerAddendumObject.CreatedBy));
			AddParameter(cmd, pBool(CustomerAddendumBase.Property_IsSigned, customerAddendumObject.IsSigned));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAddendum
        /// </summary>
        /// <param name="customerAddendumObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAddendumBase customerAddendumObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERADDENDUM);
	
				AddParameter(cmd, pInt32Out(CustomerAddendumBase.Property_Id));
				AddCommonParams(cmd, customerAddendumObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAddendumObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAddendumObject.Id = (Int32)GetOutParameter(cmd, CustomerAddendumBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAddendumObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAddendum
        /// </summary>
        /// <param name="customerAddendumObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAddendumBase customerAddendumObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERADDENDUM);
				
				AddParameter(cmd, pInt32(CustomerAddendumBase.Property_Id, customerAddendumObject.Id));
				AddCommonParams(cmd, customerAddendumObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAddendumObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAddendumObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAddendum
        /// </summary>
        /// <param name="Id">Id of the CustomerAddendum object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERADDENDUM);	
				
				AddParameter(cmd, pInt32(CustomerAddendumBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAddendum), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAddendum object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAddendum object to retrieve</param>
        /// <returns>CustomerAddendum object, null if not found</returns>
		public CustomerAddendum Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDENDUMBYID))
			{
				AddParameter( cmd, pInt32(CustomerAddendumBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAddendum objects 
        /// </summary>
        /// <returns>A list of CustomerAddendum objects</returns>
		public CustomerAddendumList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERADDENDUM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAddendum objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAddendum objects</returns>
		public CustomerAddendumList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERADDENDUM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAddendumList _CustomerAddendumList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAddendumList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAddendum objects by query String
        /// </summary>
        /// <returns>A list of CustomerAddendum objects</returns>
		public CustomerAddendumList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDENDUMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAddendum Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAddendum
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDENDUMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAddendum Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAddendum
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAddendumRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDENDUMROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAddendumRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAddendumRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAddendum object
        /// </summary>
        /// <param name="customerAddendumObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAddendumBase customerAddendumObject, SqlDataReader reader, int start)
		{
			
				customerAddendumObject.Id = reader.GetInt32( start + 0 );			
				customerAddendumObject.CustomerId = reader.GetGuid( start + 1 );			
				customerAddendumObject.TicketId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerAddendumObject.Signature = reader.GetString( start + 3 );			
				customerAddendumObject.CreatedDate = reader.GetDateTime( start + 4 );			
				customerAddendumObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) customerAddendumObject.IsSigned = reader.GetBoolean( start + 6 );			
			FillBaseObject(customerAddendumObject, reader, (start + 7));

			
			customerAddendumObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAddendum object
        /// </summary>
        /// <param name="customerAddendumObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAddendumBase customerAddendumObject, SqlDataReader reader)
		{
			FillObject(customerAddendumObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAddendum object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAddendum object</returns>
		private CustomerAddendum GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAddendum customerAddendumObject= new CustomerAddendum();
					FillObject(customerAddendumObject, reader);
					return customerAddendumObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAddendum objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAddendum objects</returns>
		private CustomerAddendumList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAddendum list
			CustomerAddendumList list = new CustomerAddendumList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAddendum customerAddendumObject = new CustomerAddendum();
					FillObject(customerAddendumObject, reader);

					list.Add(customerAddendumObject);
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
