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
	public partial class CustomerBillDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERBILLDETAIL = "InsertCustomerBillDetail";
		private const string UPDATECUSTOMERBILLDETAIL = "UpdateCustomerBillDetail";
		private const string DELETECUSTOMERBILLDETAIL = "DeleteCustomerBillDetail";
		private const string GETCUSTOMERBILLDETAILBYID = "GetCustomerBillDetailById";
		private const string GETALLCUSTOMERBILLDETAIL = "GetAllCustomerBillDetail";
		private const string GETPAGEDCUSTOMERBILLDETAIL = "GetPagedCustomerBillDetail";
		private const string GETCUSTOMERBILLDETAILMAXIMUMID = "GetCustomerBillDetailMaximumId";
		private const string GETCUSTOMERBILLDETAILROWCOUNT = "GetCustomerBillDetailRowCount";	
		private const string GETCUSTOMERBILLDETAILBYQUERY = "GetCustomerBillDetailByQuery";
		#endregion
		
		#region Constructors
		public CustomerBillDetailDataAccess(ClientContext context) : base(context) { }
		public CustomerBillDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerBillDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerBillDetailBase customerBillDetailObject)
		{	
			AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_CustomerBillId, customerBillDetailObject.CustomerBillId));
			AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_EquipmentId, customerBillDetailObject.EquipmentId));
			AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_AccoutTypeId, customerBillDetailObject.AccoutTypeId));
			AddParameter(cmd, pNVarChar(CustomerBillDetailBase.Property_Dscription, customerBillDetailObject.Dscription));
			AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_Quantity, customerBillDetailObject.Quantity));
			AddParameter(cmd, pDouble(CustomerBillDetailBase.Property_Rate, customerBillDetailObject.Rate));
			AddParameter(cmd, pDouble(CustomerBillDetailBase.Property_Amount, customerBillDetailObject.Amount));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerBillDetail
        /// </summary>
        /// <param name="customerBillDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerBillDetailBase customerBillDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERBILLDETAIL);
	
				AddParameter(cmd, pInt32Out(CustomerBillDetailBase.Property_Id));
				AddCommonParams(cmd, customerBillDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerBillDetailObject.Id = (Int32)GetOutParameter(cmd, CustomerBillDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerBillDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerBillDetail
        /// </summary>
        /// <param name="customerBillDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerBillDetailBase customerBillDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERBILLDETAIL);
				
				AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_Id, customerBillDetailObject.Id));
				AddCommonParams(cmd, customerBillDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerBillDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerBillDetail
        /// </summary>
        /// <param name="Id">Id of the CustomerBillDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERBILLDETAIL);	
				
				AddParameter(cmd, pInt32(CustomerBillDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerBillDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerBillDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerBillDetail object to retrieve</param>
        /// <returns>CustomerBillDetail object, null if not found</returns>
		public CustomerBillDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLDETAILBYID))
			{
				AddParameter( cmd, pInt32(CustomerBillDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerBillDetail objects 
        /// </summary>
        /// <returns>A list of CustomerBillDetail objects</returns>
		public CustomerBillDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERBILLDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerBillDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerBillDetail objects</returns>
		public CustomerBillDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERBILLDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerBillDetailList _CustomerBillDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerBillDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerBillDetail objects by query String
        /// </summary>
        /// <returns>A list of CustomerBillDetail objects</returns>
		public CustomerBillDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerBillDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerBillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerBillDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerBillDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerBillDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBILLDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerBillDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerBillDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerBillDetail object
        /// </summary>
        /// <param name="customerBillDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerBillDetailBase customerBillDetailObject, SqlDataReader reader, int start)
		{
			
				customerBillDetailObject.Id = reader.GetInt32( start + 0 );			
				customerBillDetailObject.CustomerBillId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) customerBillDetailObject.EquipmentId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) customerBillDetailObject.AccoutTypeId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) customerBillDetailObject.Dscription = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerBillDetailObject.Quantity = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) customerBillDetailObject.Rate = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) customerBillDetailObject.Amount = reader.GetDouble( start + 7 );			
			FillBaseObject(customerBillDetailObject, reader, (start + 8));

			
			customerBillDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerBillDetail object
        /// </summary>
        /// <param name="customerBillDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerBillDetailBase customerBillDetailObject, SqlDataReader reader)
		{
			FillObject(customerBillDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerBillDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerBillDetail object</returns>
		private CustomerBillDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerBillDetail customerBillDetailObject= new CustomerBillDetail();
					FillObject(customerBillDetailObject, reader);
					return customerBillDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerBillDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerBillDetail objects</returns>
		private CustomerBillDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerBillDetail list
			CustomerBillDetailList list = new CustomerBillDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerBillDetail customerBillDetailObject = new CustomerBillDetail();
					FillObject(customerBillDetailObject, reader);

					list.Add(customerBillDetailObject);
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
