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
	public partial class CustomerExistingItemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMEREXISTINGITEM = "InsertCustomerExistingItem";
		private const string UPDATECUSTOMEREXISTINGITEM = "UpdateCustomerExistingItem";
		private const string DELETECUSTOMEREXISTINGITEM = "DeleteCustomerExistingItem";
		private const string GETCUSTOMEREXISTINGITEMBYID = "GetCustomerExistingItemById";
		private const string GETALLCUSTOMEREXISTINGITEM = "GetAllCustomerExistingItem";
		private const string GETPAGEDCUSTOMEREXISTINGITEM = "GetPagedCustomerExistingItem";
		private const string GETCUSTOMEREXISTINGITEMMAXIMUMID = "GetCustomerExistingItemMaximumId";
		private const string GETCUSTOMEREXISTINGITEMROWCOUNT = "GetCustomerExistingItemRowCount";	
		private const string GETCUSTOMEREXISTINGITEMBYQUERY = "GetCustomerExistingItemByQuery";
		#endregion
		
		#region Constructors
		public CustomerExistingItemDataAccess(ClientContext context) : base(context) { }
		public CustomerExistingItemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerExistingItemObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerExistingItemBase customerExistingItemObject)
		{	
			AddParameter(cmd, pGuid(CustomerExistingItemBase.Property_CustomerId, customerExistingItemObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerExistingItemBase.Property_ItemName, 500, customerExistingItemObject.ItemName));
			AddParameter(cmd, pInt32(CustomerExistingItemBase.Property_Quantity, customerExistingItemObject.Quantity));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerExistingItem
        /// </summary>
        /// <param name="customerExistingItemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerExistingItemBase customerExistingItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMEREXISTINGITEM);
	
				AddParameter(cmd, pInt32Out(CustomerExistingItemBase.Property_Id));
				AddCommonParams(cmd, customerExistingItemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerExistingItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerExistingItemObject.Id = (Int32)GetOutParameter(cmd, CustomerExistingItemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerExistingItemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerExistingItem
        /// </summary>
        /// <param name="customerExistingItemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerExistingItemBase customerExistingItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMEREXISTINGITEM);
				
				AddParameter(cmd, pInt32(CustomerExistingItemBase.Property_Id, customerExistingItemObject.Id));
				AddCommonParams(cmd, customerExistingItemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerExistingItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerExistingItemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerExistingItem
        /// </summary>
        /// <param name="Id">Id of the CustomerExistingItem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMEREXISTINGITEM);	
				
				AddParameter(cmd, pInt32(CustomerExistingItemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerExistingItem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerExistingItem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerExistingItem object to retrieve</param>
        /// <returns>CustomerExistingItem object, null if not found</returns>
		public CustomerExistingItem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMEREXISTINGITEMBYID))
			{
				AddParameter( cmd, pInt32(CustomerExistingItemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerExistingItem objects 
        /// </summary>
        /// <returns>A list of CustomerExistingItem objects</returns>
		public CustomerExistingItemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMEREXISTINGITEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerExistingItem objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerExistingItem objects</returns>
		public CustomerExistingItemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMEREXISTINGITEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerExistingItemList _CustomerExistingItemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerExistingItemList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerExistingItem objects by query String
        /// </summary>
        /// <returns>A list of CustomerExistingItem objects</returns>
		public CustomerExistingItemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMEREXISTINGITEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerExistingItem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerExistingItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMEREXISTINGITEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerExistingItem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerExistingItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerExistingItemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMEREXISTINGITEMROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerExistingItemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerExistingItemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerExistingItem object
        /// </summary>
        /// <param name="customerExistingItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerExistingItemBase customerExistingItemObject, SqlDataReader reader, int start)
		{
			
				customerExistingItemObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) customerExistingItemObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerExistingItemObject.ItemName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerExistingItemObject.Quantity = reader.GetInt32( start + 3 );			
			FillBaseObject(customerExistingItemObject, reader, (start + 4));

			
			customerExistingItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerExistingItem object
        /// </summary>
        /// <param name="customerExistingItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerExistingItemBase customerExistingItemObject, SqlDataReader reader)
		{
			FillObject(customerExistingItemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerExistingItem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerExistingItem object</returns>
		private CustomerExistingItem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerExistingItem customerExistingItemObject= new CustomerExistingItem();
					FillObject(customerExistingItemObject, reader);
					return customerExistingItemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerExistingItem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerExistingItem objects</returns>
		private CustomerExistingItemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerExistingItem list
			CustomerExistingItemList list = new CustomerExistingItemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerExistingItem customerExistingItemObject = new CustomerExistingItem();
					FillObject(customerExistingItemObject, reader);

					list.Add(customerExistingItemObject);
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
