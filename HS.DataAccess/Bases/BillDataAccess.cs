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
	public partial class BillDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBILL = "InsertBill";
		private const string UPDATEBILL = "UpdateBill";
		private const string DELETEBILL = "DeleteBill";
		private const string GETBILLBYID = "GetBillById";
		private const string GETALLBILL = "GetAllBill";
		private const string GETPAGEDBILL = "GetPagedBill";
		private const string GETBILLMAXIMUMID = "GetBillMaximumId";
		private const string GETBILLROWCOUNT = "GetBillRowCount";	
		private const string GETBILLBYQUERY = "GetBillByQuery";
		#endregion
		
		#region Constructors
		public BillDataAccess(ClientContext context) : base(context) { }
		public BillDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="billObject"></param>
		private void AddCommonParams(SqlCommand cmd, BillBase billObject)
		{	
			AddParameter(cmd, pNVarChar(BillBase.Property_BillNo, 50, billObject.BillNo));
			AddParameter(cmd, pGuid(BillBase.Property_CompanyId, billObject.CompanyId));
			AddParameter(cmd, pInt32(BillBase.Property_SupplierId, billObject.SupplierId));
			AddParameter(cmd, pNVarChar(BillBase.Property_Type, 50, billObject.Type));
			AddParameter(cmd, pDouble(BillBase.Property_Amount, billObject.Amount));
			AddParameter(cmd, pNVarChar(BillBase.Property_PaymentMethod, 50, billObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(BillBase.Property_PaymentStatus, 50, billObject.PaymentStatus));
			AddParameter(cmd, pDateTime(BillBase.Property_PaymentDate, billObject.PaymentDate));
			AddParameter(cmd, pDateTime(BillBase.Property_PaymentDueDate, billObject.PaymentDueDate));
			AddParameter(cmd, pNVarChar(BillBase.Property_BillCycle, 50, billObject.BillCycle));
			AddParameter(cmd, pNVarChar(BillBase.Property_Notes, billObject.Notes));
			AddParameter(cmd, pNVarChar(BillBase.Property_UpdatedBy, 50, billObject.UpdatedBy));
			AddParameter(cmd, pDateTime(BillBase.Property_UpdatedDate, billObject.UpdatedDate));
			AddParameter(cmd, pDouble(BillBase.Property_PaymentDue, billObject.PaymentDue));
			AddParameter(cmd, pNVarChar(BillBase.Property_SupplierAddress, billObject.SupplierAddress));
			AddParameter(cmd, pNVarChar(BillBase.Property_BillFor, 50, billObject.BillFor));
			AddParameter(cmd, pGuid(BillBase.Property_EmployeeId, billObject.EmployeeId));
			AddParameter(cmd, pNVarChar(BillBase.Property_InvoiceId, 50, billObject.InvoiceId));
			AddParameter(cmd, pNVarChar(BillBase.Property_PurchaseOrderId, 50, billObject.PurchaseOrderId));
			AddParameter(cmd, pNVarChar(BillBase.Property_PaymentTerm, 250, billObject.PaymentTerm));
			AddParameter(cmd, pNVarChar(BillBase.Property_JobName, 250, billObject.JobName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Bill
        /// </summary>
        /// <param name="billObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BillBase billObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBILL);
	
				AddParameter(cmd, pInt32Out(BillBase.Property_Id));
				AddCommonParams(cmd, billObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					billObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					billObject.Id = (Int32)GetOutParameter(cmd, BillBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(billObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Bill
        /// </summary>
        /// <param name="billObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BillBase billObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBILL);
				
				AddParameter(cmd, pInt32(BillBase.Property_Id, billObject.Id));
				AddCommonParams(cmd, billObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					billObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(billObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Bill
        /// </summary>
        /// <param name="Id">Id of the Bill object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBILL);	
				
				AddParameter(cmd, pInt32(BillBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Bill), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Bill object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Bill object to retrieve</param>
        /// <returns>Bill object, null if not found</returns>
		public Bill Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLBYID))
			{
				AddParameter( cmd, pInt32(BillBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Bill objects 
        /// </summary>
        /// <returns>A list of Bill objects</returns>
		public BillList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBILL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Bill objects by PageRequest
        /// </summary>
        /// <returns>A list of Bill objects</returns>
		public BillList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBILL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BillList _BillList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BillList;
			}
		}
		
		/// <summary>
        /// Retrieves all Bill objects by query String
        /// </summary>
        /// <returns>A list of Bill objects</returns>
		public BillList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Bill Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Bill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Bill Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Bill
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BillRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLROWCOUNT))
			{
				SqlDataReader reader;
				_BillRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BillRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Bill object
        /// </summary>
        /// <param name="billObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BillBase billObject, SqlDataReader reader, int start)
		{
			
				billObject.Id = reader.GetInt32( start + 0 );			
				billObject.BillNo = reader.GetString( start + 1 );			
				billObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) billObject.SupplierId = reader.GetInt32( start + 3 );			
				billObject.Type = reader.GetString( start + 4 );			
				billObject.Amount = reader.GetDouble( start + 5 );			
				billObject.PaymentMethod = reader.GetString( start + 6 );			
				billObject.PaymentStatus = reader.GetString( start + 7 );			
				billObject.PaymentDate = reader.GetDateTime( start + 8 );			
				billObject.PaymentDueDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) billObject.BillCycle = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) billObject.Notes = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) billObject.UpdatedBy = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) billObject.UpdatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) billObject.PaymentDue = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) billObject.SupplierAddress = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) billObject.BillFor = reader.GetString( start + 16 );			
				billObject.EmployeeId = reader.GetGuid( start + 17 );			
				if(!reader.IsDBNull(18)) billObject.InvoiceId = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) billObject.PurchaseOrderId = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) billObject.PaymentTerm = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) billObject.JobName = reader.GetString( start + 21 );			
			FillBaseObject(billObject, reader, (start + 22));

			
			billObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Bill object
        /// </summary>
        /// <param name="billObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BillBase billObject, SqlDataReader reader)
		{
			FillObject(billObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Bill object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Bill object</returns>
		private Bill GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Bill billObject= new Bill();
					FillObject(billObject, reader);
					return billObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Bill objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Bill objects</returns>
		private BillList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Bill list
			BillList list = new BillList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Bill billObject = new Bill();
					FillObject(billObject, reader);

					list.Add(billObject);
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
