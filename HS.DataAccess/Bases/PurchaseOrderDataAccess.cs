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
	public partial class PurchaseOrderDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDER = "InsertPurchaseOrder";
		private const string UPDATEPURCHASEORDER = "UpdatePurchaseOrder";
		private const string DELETEPURCHASEORDER = "DeletePurchaseOrder";
		private const string GETPURCHASEORDERBYID = "GetPurchaseOrderById";
		private const string GETALLPURCHASEORDER = "GetAllPurchaseOrder";
		private const string GETPAGEDPURCHASEORDER = "GetPagedPurchaseOrder";
		private const string GETPURCHASEORDERMAXIMUMID = "GetPurchaseOrderMaximumId";
		private const string GETPURCHASEORDERROWCOUNT = "GetPurchaseOrderRowCount";	
		private const string GETPURCHASEORDERBYQUERY = "GetPurchaseOrderByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderBase purchaseOrderObject)
		{	
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_PurchaseOrderId, 15, purchaseOrderObject.PurchaseOrderId));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_SuplierId, purchaseOrderObject.SuplierId));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_CompanyId, purchaseOrderObject.CompanyId));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_SoldBy, purchaseOrderObject.SoldBy));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_DiscountCode, 50, purchaseOrderObject.DiscountCode));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_DiscountType, 50, purchaseOrderObject.DiscountType));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_DiscountAmount, purchaseOrderObject.DiscountAmount));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_Discountpercent, purchaseOrderObject.Discountpercent));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_Amount, purchaseOrderObject.Amount));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_Tax, purchaseOrderObject.Tax));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_Deposit, purchaseOrderObject.Deposit));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_TotalAmount, purchaseOrderObject.TotalAmount));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_BalanceDue, purchaseOrderObject.BalanceDue));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_Status, 50, purchaseOrderObject.Status));
			AddParameter(cmd, pDateTime(PurchaseOrderBase.Property_OrderDate, purchaseOrderObject.OrderDate));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_BillingAddress, purchaseOrderObject.BillingAddress));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_ShippingAddress, purchaseOrderObject.ShippingAddress));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_ShippingVia, 50, purchaseOrderObject.ShippingVia));
			AddParameter(cmd, pDateTime(PurchaseOrderBase.Property_ShippingDate, purchaseOrderObject.ShippingDate));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_ShippingCost, purchaseOrderObject.ShippingCost));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_TrackingNo, 50, purchaseOrderObject.TrackingNo));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_Message, purchaseOrderObject.Message));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_TaxType, 50, purchaseOrderObject.TaxType));
			AddParameter(cmd, pDouble(PurchaseOrderBase.Property_Balance, purchaseOrderObject.Balance));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_Description, purchaseOrderObject.Description));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_Signature, 250, purchaseOrderObject.Signature));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_CancelReason, purchaseOrderObject.CancelReason));
			AddParameter(cmd, pDateTime(PurchaseOrderBase.Property_CreatedDate, purchaseOrderObject.CreatedDate));
			AddParameter(cmd, pNVarChar(PurchaseOrderBase.Property_CreatedBy, 50, purchaseOrderObject.CreatedBy));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_CreatedByUid, purchaseOrderObject.CreatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderBase.Property_LastUpdatedDate, purchaseOrderObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_LastUpdatedByUid, purchaseOrderObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderBase.Property_RecieveDate, purchaseOrderObject.RecieveDate));
			AddParameter(cmd, pGuid(PurchaseOrderBase.Property_RecieveByUid, purchaseOrderObject.RecieveByUid));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrder
        /// </summary>
        /// <param name="purchaseOrderObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderBase purchaseOrderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDER);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrder
        /// </summary>
        /// <param name="purchaseOrderObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderBase purchaseOrderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDER);
				
				AddParameter(cmd, pInt32(PurchaseOrderBase.Property_Id, purchaseOrderObject.Id));
				AddCommonParams(cmd, purchaseOrderObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrder
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrder object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDER);	
				
				AddParameter(cmd, pInt32(PurchaseOrderBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrder), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrder object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrder object to retrieve</param>
        /// <returns>PurchaseOrder object, null if not found</returns>
		public PurchaseOrder Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrder objects 
        /// </summary>
        /// <returns>A list of PurchaseOrder objects</returns>
		public PurchaseOrderList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrder objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrder objects</returns>
		public PurchaseOrderList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderList _PurchaseOrderList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrder objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrder objects</returns>
		public PurchaseOrderList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrder Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrder Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrder object
        /// </summary>
        /// <param name="purchaseOrderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderBase purchaseOrderObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderObject.PurchaseOrderId = reader.GetString( start + 1 );			
				purchaseOrderObject.SuplierId = reader.GetGuid( start + 2 );			
				purchaseOrderObject.CompanyId = reader.GetGuid( start + 3 );			
				purchaseOrderObject.SoldBy = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) purchaseOrderObject.DiscountCode = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) purchaseOrderObject.DiscountType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) purchaseOrderObject.DiscountAmount = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) purchaseOrderObject.Discountpercent = reader.GetDouble( start + 8 );			
				purchaseOrderObject.Amount = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) purchaseOrderObject.Tax = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) purchaseOrderObject.Deposit = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) purchaseOrderObject.TotalAmount = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) purchaseOrderObject.BalanceDue = reader.GetDouble( start + 13 );			
				purchaseOrderObject.Status = reader.GetString( start + 14 );			
				purchaseOrderObject.OrderDate = reader.GetDateTime( start + 15 );			
				if(!reader.IsDBNull(16)) purchaseOrderObject.BillingAddress = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) purchaseOrderObject.ShippingAddress = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) purchaseOrderObject.ShippingVia = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) purchaseOrderObject.ShippingDate = reader.GetDateTime( start + 19 );			
				if(!reader.IsDBNull(20)) purchaseOrderObject.ShippingCost = reader.GetDouble( start + 20 );			
				if(!reader.IsDBNull(21)) purchaseOrderObject.TrackingNo = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) purchaseOrderObject.Message = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) purchaseOrderObject.TaxType = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) purchaseOrderObject.Balance = reader.GetDouble( start + 24 );			
				if(!reader.IsDBNull(25)) purchaseOrderObject.Description = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) purchaseOrderObject.Signature = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) purchaseOrderObject.CancelReason = reader.GetString( start + 27 );			
				purchaseOrderObject.CreatedDate = reader.GetDateTime( start + 28 );			
				purchaseOrderObject.CreatedBy = reader.GetString( start + 29 );			
				purchaseOrderObject.CreatedByUid = reader.GetGuid( start + 30 );			
				purchaseOrderObject.LastUpdatedDate = reader.GetDateTime( start + 31 );			
				purchaseOrderObject.LastUpdatedByUid = reader.GetGuid( start + 32 );			
				if(!reader.IsDBNull(33)) purchaseOrderObject.RecieveDate = reader.GetDateTime( start + 33 );			
				purchaseOrderObject.RecieveByUid = reader.GetGuid( start + 34 );			
			FillBaseObject(purchaseOrderObject, reader, (start + 35));

			
			purchaseOrderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrder object
        /// </summary>
        /// <param name="purchaseOrderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderBase purchaseOrderObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrder object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrder object</returns>
		private PurchaseOrder GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrder purchaseOrderObject= new PurchaseOrder();
					FillObject(purchaseOrderObject, reader);
					return purchaseOrderObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrder objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrder objects</returns>
		private PurchaseOrderList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrder list
			PurchaseOrderList list = new PurchaseOrderList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrder purchaseOrderObject = new PurchaseOrder();
					FillObject(purchaseOrderObject, reader);

					list.Add(purchaseOrderObject);
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
