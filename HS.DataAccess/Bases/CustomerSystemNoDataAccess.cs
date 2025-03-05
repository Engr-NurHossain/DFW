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
	public partial class CustomerSystemNoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSYSTEMNO = "InsertCustomerSystemNo";
		private const string UPDATECUSTOMERSYSTEMNO = "UpdateCustomerSystemNo";
		private const string DELETECUSTOMERSYSTEMNO = "DeleteCustomerSystemNo";
		private const string GETCUSTOMERSYSTEMNOBYID = "GetCustomerSystemNoById";
		private const string GETALLCUSTOMERSYSTEMNO = "GetAllCustomerSystemNo";
		private const string GETPAGEDCUSTOMERSYSTEMNO = "GetPagedCustomerSystemNo";
		private const string GETCUSTOMERSYSTEMNOMAXIMUMID = "GetCustomerSystemNoMaximumId";
		private const string GETCUSTOMERSYSTEMNOROWCOUNT = "GetCustomerSystemNoRowCount";	
		private const string GETCUSTOMERSYSTEMNOBYQUERY = "GetCustomerSystemNoByQuery";
		#endregion
		
		#region Constructors
		public CustomerSystemNoDataAccess(ClientContext context) : base(context) { }
		public CustomerSystemNoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSystemNoObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSystemNoBase customerSystemNoObject)
		{	
			AddParameter(cmd, pGuid(CustomerSystemNoBase.Property_CompanyId, customerSystemNoObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSystemNoBase.Property_CustomerNo, 50, customerSystemNoObject.CustomerNo));
			AddParameter(cmd, pBool(CustomerSystemNoBase.Property_IsUsed, customerSystemNoObject.IsUsed));
			AddParameter(cmd, pBool(CustomerSystemNoBase.Property_IsReserved, customerSystemNoObject.IsReserved));
			AddParameter(cmd, pDateTime(CustomerSystemNoBase.Property_GenerateDate, customerSystemNoObject.GenerateDate));
			AddParameter(cmd, pDateTime(CustomerSystemNoBase.Property_ReserveDate, customerSystemNoObject.ReserveDate));
			AddParameter(cmd, pDateTime(CustomerSystemNoBase.Property_UsedDate, customerSystemNoObject.UsedDate));
			AddParameter(cmd, pInt32(CustomerSystemNoBase.Property_CustomerId, customerSystemNoObject.CustomerId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSystemNo
        /// </summary>
        /// <param name="customerSystemNoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSystemNoBase customerSystemNoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSYSTEMNO);
	
				AddParameter(cmd, pInt32Out(CustomerSystemNoBase.Property_Id));
				AddCommonParams(cmd, customerSystemNoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSystemNoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSystemNoObject.Id = (Int32)GetOutParameter(cmd, CustomerSystemNoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSystemNoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSystemNo
        /// </summary>
        /// <param name="customerSystemNoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSystemNoBase customerSystemNoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSYSTEMNO);
				
				AddParameter(cmd, pInt32(CustomerSystemNoBase.Property_Id, customerSystemNoObject.Id));
				AddCommonParams(cmd, customerSystemNoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSystemNoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSystemNoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSystemNo
        /// </summary>
        /// <param name="Id">Id of the CustomerSystemNo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSYSTEMNO);	
				
				AddParameter(cmd, pInt32(CustomerSystemNoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSystemNo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSystemNo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSystemNo object to retrieve</param>
        /// <returns>CustomerSystemNo object, null if not found</returns>
		public CustomerSystemNo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNOBYID))
			{
				AddParameter( cmd, pInt32(CustomerSystemNoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSystemNo objects 
        /// </summary>
        /// <returns>A list of CustomerSystemNo objects</returns>
		public CustomerSystemNoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSYSTEMNO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSystemNo objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSystemNo objects</returns>
		public CustomerSystemNoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSYSTEMNO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSystemNoList _CustomerSystemNoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSystemNoList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSystemNo objects by query String
        /// </summary>
        /// <returns>A list of CustomerSystemNo objects</returns>
		public CustomerSystemNoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSystemNo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSystemNo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSystemNo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSystemNo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSystemNoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMNOROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSystemNoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSystemNoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSystemNo object
        /// </summary>
        /// <param name="customerSystemNoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSystemNoBase customerSystemNoObject, SqlDataReader reader, int start)
		{
			
				customerSystemNoObject.Id = reader.GetInt32( start + 0 );			
				customerSystemNoObject.CompanyId = reader.GetGuid( start + 1 );			
				customerSystemNoObject.CustomerNo = reader.GetString( start + 2 );			
				customerSystemNoObject.IsUsed = reader.GetBoolean( start + 3 );			
				customerSystemNoObject.IsReserved = reader.GetBoolean( start + 4 );			
				customerSystemNoObject.GenerateDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) customerSystemNoObject.ReserveDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customerSystemNoObject.UsedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) customerSystemNoObject.CustomerId = reader.GetInt32( start + 8 );			
			FillBaseObject(customerSystemNoObject, reader, (start + 9));

			
			customerSystemNoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSystemNo object
        /// </summary>
        /// <param name="customerSystemNoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSystemNoBase customerSystemNoObject, SqlDataReader reader)
		{
			FillObject(customerSystemNoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSystemNo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSystemNo object</returns>
		private CustomerSystemNo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSystemNo customerSystemNoObject= new CustomerSystemNo();
					FillObject(customerSystemNoObject, reader);
					return customerSystemNoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSystemNo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSystemNo objects</returns>
		private CustomerSystemNoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSystemNo list
			CustomerSystemNoList list = new CustomerSystemNoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSystemNo customerSystemNoObject = new CustomerSystemNo();
					FillObject(customerSystemNoObject, reader);

					list.Add(customerSystemNoObject);
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
