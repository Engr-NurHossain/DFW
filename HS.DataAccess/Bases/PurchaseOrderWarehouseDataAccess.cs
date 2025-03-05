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
	public partial class PurchaseOrderWarehouseDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERWAREHOUSE = "InsertPurchaseOrderWarehouse";
		private const string UPDATEPURCHASEORDERWAREHOUSE = "UpdatePurchaseOrderWarehouse";
		private const string DELETEPURCHASEORDERWAREHOUSE = "DeletePurchaseOrderWarehouse";
		private const string GETPURCHASEORDERWAREHOUSEBYID = "GetPurchaseOrderWarehouseById";
		private const string GETALLPURCHASEORDERWAREHOUSE = "GetAllPurchaseOrderWarehouse";
		private const string GETPAGEDPURCHASEORDERWAREHOUSE = "GetPagedPurchaseOrderWarehouse";
		private const string GETPURCHASEORDERWAREHOUSEMAXIMUMID = "GetPurchaseOrderWarehouseMaximumId";
		private const string GETPURCHASEORDERWAREHOUSEROWCOUNT = "GetPurchaseOrderWarehouseRowCount";	
		private const string GETPURCHASEORDERWAREHOUSEBYQUERY = "GetPurchaseOrderWarehouseByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderWarehouseDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderWarehouseDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderWarehouseObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderWarehouseBase purchaseOrderWarehouseObject)
		{	
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_CompanyId, purchaseOrderWarehouseObject.CompanyId));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_PurchaseOrderId, 50, purchaseOrderWarehouseObject.PurchaseOrderId));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_Action, 50, purchaseOrderWarehouseObject.Action));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_Status, 50, purchaseOrderWarehouseObject.Status));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_Location, 50, purchaseOrderWarehouseObject.Location));
			AddParameter(cmd, pBool(PurchaseOrderWarehouseBase.Property_IsReceived, purchaseOrderWarehouseObject.IsReceived));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_CreatedByUid, purchaseOrderWarehouseObject.CreatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderWarehouseBase.Property_LastUpdatedDate, purchaseOrderWarehouseObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_LastUpdatedByUid, purchaseOrderWarehouseObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderWarehouseBase.Property_RecieveDate, purchaseOrderWarehouseObject.RecieveDate));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_RecieveByUid, purchaseOrderWarehouseObject.RecieveByUid));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_BranchDemandOrderId, 50, purchaseOrderWarehouseObject.BranchDemandOrderId));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_SuplierId, purchaseOrderWarehouseObject.SuplierId));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_SoldBy, purchaseOrderWarehouseObject.SoldBy));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_Amount, purchaseOrderWarehouseObject.Amount));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_TaxType, 50, purchaseOrderWarehouseObject.TaxType));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_Tax, purchaseOrderWarehouseObject.Tax));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_Deposit, purchaseOrderWarehouseObject.Deposit));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_TotalAmount, purchaseOrderWarehouseObject.TotalAmount));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_BalanceDue, purchaseOrderWarehouseObject.BalanceDue));
			AddParameter(cmd, pDateTime(PurchaseOrderWarehouseBase.Property_OrderDate, purchaseOrderWarehouseObject.OrderDate));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_BillingAddress, purchaseOrderWarehouseObject.BillingAddress));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_ShippingAddress, purchaseOrderWarehouseObject.ShippingAddress));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_ShippingVia, 50, purchaseOrderWarehouseObject.ShippingVia));
			AddParameter(cmd, pDateTime(PurchaseOrderWarehouseBase.Property_ShippingDate, purchaseOrderWarehouseObject.ShippingDate));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_ShippingCost, purchaseOrderWarehouseObject.ShippingCost));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_TrackingNo, 50, purchaseOrderWarehouseObject.TrackingNo));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_Message, purchaseOrderWarehouseObject.Message));
			AddParameter(cmd, pDouble(PurchaseOrderWarehouseBase.Property_Balance, purchaseOrderWarehouseObject.Balance));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_Description, purchaseOrderWarehouseObject.Description));
			AddParameter(cmd, pDateTime(PurchaseOrderWarehouseBase.Property_CreatedDate, purchaseOrderWarehouseObject.CreatedDate));
			AddParameter(cmd, pBool(PurchaseOrderWarehouseBase.Property_IsBulkPO, purchaseOrderWarehouseObject.IsBulkPO));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_RecieveForUid, purchaseOrderWarehouseObject.RecieveForUid));
			AddParameter(cmd, pGuid(PurchaseOrderWarehouseBase.Property_POFor, purchaseOrderWarehouseObject.POFor));
			AddParameter(cmd, pNVarChar(PurchaseOrderWarehouseBase.Property_EstimatorId, 50, purchaseOrderWarehouseObject.EstimatorId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderWarehouse
        /// </summary>
        /// <param name="purchaseOrderWarehouseObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderWarehouseBase purchaseOrderWarehouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERWAREHOUSE);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderWarehouseBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderWarehouseObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderWarehouseObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderWarehouseBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderWarehouseObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderWarehouse
        /// </summary>
        /// <param name="purchaseOrderWarehouseObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderWarehouseBase purchaseOrderWarehouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERWAREHOUSE);
				
				AddParameter(cmd, pInt32(PurchaseOrderWarehouseBase.Property_Id, purchaseOrderWarehouseObject.Id));
				AddCommonParams(cmd, purchaseOrderWarehouseObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderWarehouseObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderWarehouse
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderWarehouse object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERWAREHOUSE);	
				
				AddParameter(cmd, pInt32(PurchaseOrderWarehouseBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderWarehouse), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderWarehouse object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderWarehouse object to retrieve</param>
        /// <returns>PurchaseOrderWarehouse object, null if not found</returns>
		public PurchaseOrderWarehouse Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERWAREHOUSEBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderWarehouseBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderWarehouse objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderWarehouse objects</returns>
		public PurchaseOrderWarehouseList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERWAREHOUSE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderWarehouse objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderWarehouse objects</returns>
		public PurchaseOrderWarehouseList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERWAREHOUSE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderWarehouseList _PurchaseOrderWarehouseList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderWarehouseList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderWarehouse objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderWarehouse objects</returns>
		public PurchaseOrderWarehouseList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERWAREHOUSEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrderWarehouse Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrderWarehouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERWAREHOUSEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderWarehouse Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderWarehouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderWarehouseRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERWAREHOUSEROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderWarehouseRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderWarehouseRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderWarehouse object
        /// </summary>
        /// <param name="purchaseOrderWarehouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderWarehouseBase purchaseOrderWarehouseObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderWarehouseObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderWarehouseObject.CompanyId = reader.GetGuid( start + 1 );			
				purchaseOrderWarehouseObject.PurchaseOrderId = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) purchaseOrderWarehouseObject.Action = reader.GetString( start + 3 );			
				purchaseOrderWarehouseObject.Status = reader.GetString( start + 4 );			
				purchaseOrderWarehouseObject.Location = reader.GetString( start + 5 );			
				purchaseOrderWarehouseObject.IsReceived = reader.GetBoolean( start + 6 );			
				purchaseOrderWarehouseObject.CreatedByUid = reader.GetGuid( start + 7 );			
				purchaseOrderWarehouseObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				purchaseOrderWarehouseObject.LastUpdatedByUid = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) purchaseOrderWarehouseObject.RecieveDate = reader.GetDateTime( start + 10 );			
				purchaseOrderWarehouseObject.RecieveByUid = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) purchaseOrderWarehouseObject.BranchDemandOrderId = reader.GetString( start + 12 );			
				purchaseOrderWarehouseObject.SuplierId = reader.GetGuid( start + 13 );			
				purchaseOrderWarehouseObject.SoldBy = reader.GetGuid( start + 14 );			
				purchaseOrderWarehouseObject.Amount = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) purchaseOrderWarehouseObject.TaxType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) purchaseOrderWarehouseObject.Tax = reader.GetDouble( start + 17 );			
				if(!reader.IsDBNull(18)) purchaseOrderWarehouseObject.Deposit = reader.GetDouble( start + 18 );			
				if(!reader.IsDBNull(19)) purchaseOrderWarehouseObject.TotalAmount = reader.GetDouble( start + 19 );			
				if(!reader.IsDBNull(20)) purchaseOrderWarehouseObject.BalanceDue = reader.GetDouble( start + 20 );			
				purchaseOrderWarehouseObject.OrderDate = reader.GetDateTime( start + 21 );			
				if(!reader.IsDBNull(22)) purchaseOrderWarehouseObject.BillingAddress = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) purchaseOrderWarehouseObject.ShippingAddress = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) purchaseOrderWarehouseObject.ShippingVia = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) purchaseOrderWarehouseObject.ShippingDate = reader.GetDateTime( start + 25 );			
				if(!reader.IsDBNull(26)) purchaseOrderWarehouseObject.ShippingCost = reader.GetDouble( start + 26 );			
				if(!reader.IsDBNull(27)) purchaseOrderWarehouseObject.TrackingNo = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) purchaseOrderWarehouseObject.Message = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) purchaseOrderWarehouseObject.Balance = reader.GetDouble( start + 29 );			
				if(!reader.IsDBNull(30)) purchaseOrderWarehouseObject.Description = reader.GetString( start + 30 );			
				purchaseOrderWarehouseObject.CreatedDate = reader.GetDateTime( start + 31 );			
				if(!reader.IsDBNull(32)) purchaseOrderWarehouseObject.IsBulkPO = reader.GetBoolean( start + 32 );			
				purchaseOrderWarehouseObject.RecieveForUid = reader.GetGuid( start + 33 );			
				purchaseOrderWarehouseObject.POFor = reader.GetGuid( start + 34 );			
				if(!reader.IsDBNull(35)) purchaseOrderWarehouseObject.EstimatorId = reader.GetString( start + 35 );			
			FillBaseObject(purchaseOrderWarehouseObject, reader, (start + 36));

			
			purchaseOrderWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderWarehouse object
        /// </summary>
        /// <param name="purchaseOrderWarehouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderWarehouseBase purchaseOrderWarehouseObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderWarehouseObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderWarehouse object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderWarehouse object</returns>
		private PurchaseOrderWarehouse GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderWarehouse purchaseOrderWarehouseObject= new PurchaseOrderWarehouse();
					FillObject(purchaseOrderWarehouseObject, reader);
					return purchaseOrderWarehouseObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderWarehouse objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderWarehouse objects</returns>
		private PurchaseOrderWarehouseList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderWarehouse list
			PurchaseOrderWarehouseList list = new PurchaseOrderWarehouseList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderWarehouse purchaseOrderWarehouseObject = new PurchaseOrderWarehouse();
					FillObject(purchaseOrderWarehouseObject, reader);

					list.Add(purchaseOrderWarehouseObject);
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
