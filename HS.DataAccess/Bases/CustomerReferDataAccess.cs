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
	public partial class CustomerReferDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERREFER = "InsertCustomerRefer";
		private const string UPDATECUSTOMERREFER = "UpdateCustomerRefer";
		private const string DELETECUSTOMERREFER = "DeleteCustomerRefer";
		private const string GETCUSTOMERREFERBYID = "GetCustomerReferById";
		private const string GETALLCUSTOMERREFER = "GetAllCustomerRefer";
		private const string GETPAGEDCUSTOMERREFER = "GetPagedCustomerRefer";
		private const string GETCUSTOMERREFERMAXIMUMID = "GetCustomerReferMaximumId";
		private const string GETCUSTOMERREFERROWCOUNT = "GetCustomerReferRowCount";	
		private const string GETCUSTOMERREFERBYQUERY = "GetCustomerReferByQuery";
		#endregion
		
		#region Constructors
		public CustomerReferDataAccess(ClientContext context) : base(context) { }
		public CustomerReferDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerReferObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerReferBase customerReferObject)
		{	
			AddParameter(cmd, pGuid(CustomerReferBase.Property_CustomerId, customerReferObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerReferBase.Property_Name, 50, customerReferObject.Name));
			AddParameter(cmd, pNVarChar(CustomerReferBase.Property_Email, 50, customerReferObject.Email));
			AddParameter(cmd, pNVarChar(CustomerReferBase.Property_Phone, 50, customerReferObject.Phone));
			AddParameter(cmd, pBool(CustomerReferBase.Property_IsViewd, customerReferObject.IsViewd));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerRefer
        /// </summary>
        /// <param name="customerReferObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerReferBase customerReferObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERREFER);
	
				AddParameter(cmd, pInt32Out(CustomerReferBase.Property_Id));
				AddCommonParams(cmd, customerReferObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerReferObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerReferObject.Id = (Int32)GetOutParameter(cmd, CustomerReferBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerReferObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerRefer
        /// </summary>
        /// <param name="customerReferObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerReferBase customerReferObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERREFER);
				
				AddParameter(cmd, pInt32(CustomerReferBase.Property_Id, customerReferObject.Id));
				AddCommonParams(cmd, customerReferObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerReferObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerReferObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerRefer
        /// </summary>
        /// <param name="Id">Id of the CustomerRefer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERREFER);	
				
				AddParameter(cmd, pInt32(CustomerReferBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerRefer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerRefer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerRefer object to retrieve</param>
        /// <returns>CustomerRefer object, null if not found</returns>
		public CustomerRefer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERREFERBYID))
			{
				AddParameter( cmd, pInt32(CustomerReferBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerRefer objects 
        /// </summary>
        /// <returns>A list of CustomerRefer objects</returns>
		public CustomerReferList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERREFER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerRefer objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerRefer objects</returns>
		public CustomerReferList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERREFER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerReferList _CustomerReferList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerReferList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerRefer objects by query String
        /// </summary>
        /// <returns>A list of CustomerRefer objects</returns>
		public CustomerReferList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERREFERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerRefer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerRefer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERREFERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerRefer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerRefer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerReferRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERREFERROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerReferRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerReferRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerRefer object
        /// </summary>
        /// <param name="customerReferObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerReferBase customerReferObject, SqlDataReader reader, int start)
		{
			
				customerReferObject.Id = reader.GetInt32( start + 0 );			
				customerReferObject.CustomerId = reader.GetGuid( start + 1 );			
				customerReferObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerReferObject.Email = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerReferObject.Phone = reader.GetString( start + 4 );			
				customerReferObject.IsViewd = reader.GetBoolean( start + 5 );			
			FillBaseObject(customerReferObject, reader, (start + 6));

			
			customerReferObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerRefer object
        /// </summary>
        /// <param name="customerReferObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerReferBase customerReferObject, SqlDataReader reader)
		{
			FillObject(customerReferObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerRefer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerRefer object</returns>
		private CustomerRefer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerRefer customerReferObject= new CustomerRefer();
					FillObject(customerReferObject, reader);
					return customerReferObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerRefer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerRefer objects</returns>
		private CustomerReferList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerRefer list
			CustomerReferList list = new CustomerReferList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerRefer customerReferObject = new CustomerRefer();
					FillObject(customerReferObject, reader);

					list.Add(customerReferObject);
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
