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
	public partial class CustomerCancellationReasonDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCANCELLATIONREASON = "InsertCustomerCancellationReason";
		private const string UPDATECUSTOMERCANCELLATIONREASON = "UpdateCustomerCancellationReason";
		private const string DELETECUSTOMERCANCELLATIONREASON = "DeleteCustomerCancellationReason";
		private const string GETCUSTOMERCANCELLATIONREASONBYID = "GetCustomerCancellationReasonById";
		private const string GETALLCUSTOMERCANCELLATIONREASON = "GetAllCustomerCancellationReason";
		private const string GETPAGEDCUSTOMERCANCELLATIONREASON = "GetPagedCustomerCancellationReason";
		private const string GETCUSTOMERCANCELLATIONREASONMAXIMUMID = "GetCustomerCancellationReasonMaximumId";
		private const string GETCUSTOMERCANCELLATIONREASONROWCOUNT = "GetCustomerCancellationReasonRowCount";	
		private const string GETCUSTOMERCANCELLATIONREASONBYQUERY = "GetCustomerCancellationReasonByQuery";
		#endregion
		
		#region Constructors
		public CustomerCancellationReasonDataAccess(ClientContext context) : base(context) { }
		public CustomerCancellationReasonDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerCancellationReasonObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerCancellationReasonBase customerCancellationReasonObject)
		{	
			AddParameter(cmd, pGuid(CustomerCancellationReasonBase.Property_CustomerCancellationReasonId, customerCancellationReasonObject.CustomerCancellationReasonId));
			AddParameter(cmd, pGuid(CustomerCancellationReasonBase.Property_CompanyId, customerCancellationReasonObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerCancellationReasonBase.Property_CustomerId, customerCancellationReasonObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerCancellationReasonBase.Property_CancellationReason, 500, customerCancellationReasonObject.CancellationReason));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerCancellationReason
        /// </summary>
        /// <param name="customerCancellationReasonObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerCancellationReasonBase customerCancellationReasonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCANCELLATIONREASON);
	
				AddParameter(cmd, pInt32Out(CustomerCancellationReasonBase.Property_Id));
				AddCommonParams(cmd, customerCancellationReasonObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerCancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerCancellationReasonObject.Id = (Int32)GetOutParameter(cmd, CustomerCancellationReasonBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerCancellationReasonObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerCancellationReason
        /// </summary>
        /// <param name="customerCancellationReasonObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerCancellationReasonBase customerCancellationReasonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCANCELLATIONREASON);
				
				AddParameter(cmd, pInt32(CustomerCancellationReasonBase.Property_Id, customerCancellationReasonObject.Id));
				AddCommonParams(cmd, customerCancellationReasonObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerCancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerCancellationReasonObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerCancellationReason
        /// </summary>
        /// <param name="Id">Id of the CustomerCancellationReason object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCANCELLATIONREASON);	
				
				AddParameter(cmd, pInt32(CustomerCancellationReasonBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerCancellationReason), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerCancellationReason object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerCancellationReason object to retrieve</param>
        /// <returns>CustomerCancellationReason object, null if not found</returns>
		public CustomerCancellationReason Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONREASONBYID))
			{
				AddParameter( cmd, pInt32(CustomerCancellationReasonBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerCancellationReason objects 
        /// </summary>
        /// <returns>A list of CustomerCancellationReason objects</returns>
		public CustomerCancellationReasonList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCANCELLATIONREASON))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerCancellationReason objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerCancellationReason objects</returns>
		public CustomerCancellationReasonList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCANCELLATIONREASON))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerCancellationReasonList _CustomerCancellationReasonList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerCancellationReasonList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerCancellationReason objects by query String
        /// </summary>
        /// <returns>A list of CustomerCancellationReason objects</returns>
		public CustomerCancellationReasonList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONREASONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerCancellationReason Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerCancellationReason
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONREASONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerCancellationReason Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerCancellationReason
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerCancellationReasonRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCANCELLATIONREASONROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerCancellationReasonRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerCancellationReasonRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerCancellationReason object
        /// </summary>
        /// <param name="customerCancellationReasonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerCancellationReasonBase customerCancellationReasonObject, SqlDataReader reader, int start)
		{
			
				customerCancellationReasonObject.Id = reader.GetInt32( start + 0 );			
				customerCancellationReasonObject.CustomerCancellationReasonId = reader.GetGuid( start + 1 );			
				customerCancellationReasonObject.CompanyId = reader.GetGuid( start + 2 );			
				customerCancellationReasonObject.CustomerId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) customerCancellationReasonObject.CancellationReason = reader.GetString( start + 4 );			
			FillBaseObject(customerCancellationReasonObject, reader, (start + 5));

			
			customerCancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerCancellationReason object
        /// </summary>
        /// <param name="customerCancellationReasonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerCancellationReasonBase customerCancellationReasonObject, SqlDataReader reader)
		{
			FillObject(customerCancellationReasonObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerCancellationReason object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerCancellationReason object</returns>
		private CustomerCancellationReason GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerCancellationReason customerCancellationReasonObject= new CustomerCancellationReason();
					FillObject(customerCancellationReasonObject, reader);
					return customerCancellationReasonObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerCancellationReason objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerCancellationReason objects</returns>
		private CustomerCancellationReasonList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerCancellationReason list
			CustomerCancellationReasonList list = new CustomerCancellationReasonList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerCancellationReason customerCancellationReasonObject = new CustomerCancellationReason();
					FillObject(customerCancellationReasonObject, reader);

					list.Add(customerCancellationReasonObject);
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
