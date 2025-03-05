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
	public partial class PurchaseOrderDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERDETAIL = "InsertPurchaseOrderDetail";
		private const string UPDATEPURCHASEORDERDETAIL = "UpdatePurchaseOrderDetail";
		private const string DELETEPURCHASEORDERDETAIL = "DeletePurchaseOrderDetail";
		private const string GETPURCHASEORDERDETAILBYID = "GetPurchaseOrderDetailById";
		private const string GETALLPURCHASEORDERDETAIL = "GetAllPurchaseOrderDetail";
		private const string GETPAGEDPURCHASEORDERDETAIL = "GetPagedPurchaseOrderDetail";
		private const string GETPURCHASEORDERDETAILMAXIMUMID = "GetPurchaseOrderDetailMaximumId";
		private const string GETPURCHASEORDERDETAILROWCOUNT = "GetPurchaseOrderDetailRowCount";	
		private const string GETPURCHASEORDERDETAILBYQUERY = "GetPurchaseOrderDetailByQuery";
		private const string CHECKMASSRESTOCKORDERDETAIL = "CheckMassRestockOrderDetail";
		#endregion
		
		#region Constructors
		public PurchaseOrderDetailDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderDetailBase purchaseOrderDetailObject)
		{	
			AddParameter(cmd, pNVarChar(PurchaseOrderDetailBase.Property_PurchaseOrderId, 15, purchaseOrderDetailObject.PurchaseOrderId));
			AddParameter(cmd, pGuid(PurchaseOrderDetailBase.Property_EquipmentId, purchaseOrderDetailObject.EquipmentId));
			AddParameter(cmd, pNVarChar(PurchaseOrderDetailBase.Property_EquipName, 500, purchaseOrderDetailObject.EquipName));
			AddParameter(cmd, pNVarChar(PurchaseOrderDetailBase.Property_EquipDetail, purchaseOrderDetailObject.EquipDetail));
			AddParameter(cmd, pInt32(PurchaseOrderDetailBase.Property_BundleId, purchaseOrderDetailObject.BundleId));
			AddParameter(cmd, pInt32(PurchaseOrderDetailBase.Property_Quantity, purchaseOrderDetailObject.Quantity));
			AddParameter(cmd, pDouble(PurchaseOrderDetailBase.Property_UnitPrice, purchaseOrderDetailObject.UnitPrice));
			AddParameter(cmd, pDouble(PurchaseOrderDetailBase.Property_TotalPrice, purchaseOrderDetailObject.TotalPrice));
			AddParameter(cmd, pDateTime(PurchaseOrderDetailBase.Property_CreatedDate, purchaseOrderDetailObject.CreatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderDetailBase.Property_CreatedBy, purchaseOrderDetailObject.CreatedBy));
			AddParameter(cmd, pInt32(PurchaseOrderDetailBase.Property_RecieveQty, purchaseOrderDetailObject.RecieveQty));
			AddParameter(cmd, pBool(PurchaseOrderDetailBase.Property_BulkStatus, purchaseOrderDetailObject.BulkStatus));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderDetail
        /// </summary>
        /// <param name="purchaseOrderDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderDetailBase purchaseOrderDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERDETAIL);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderDetailBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderDetailObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderDetail
        /// </summary>
        /// <param name="purchaseOrderDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderDetailBase purchaseOrderDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERDETAIL);
				
				AddParameter(cmd, pInt32(PurchaseOrderDetailBase.Property_Id, purchaseOrderDetailObject.Id));
				AddCommonParams(cmd, purchaseOrderDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderDetail
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERDETAIL);	
				
				AddParameter(cmd, pInt32(PurchaseOrderDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderDetail object to retrieve</param>
        /// <returns>PurchaseOrderDetail object, null if not found</returns>
		public PurchaseOrderDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERDETAILBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderDetail objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderDetail objects</returns>
		public PurchaseOrderDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderDetail objects</returns>
		public PurchaseOrderDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderDetailList _PurchaseOrderDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderDetail objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderDetail objects</returns>
		public PurchaseOrderDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}


		public long CheckMassRestockOrderDetail(Guid EquipmentId, Guid TechnicianId)
		{
			PurchaseOrderDetailBase purchaseOrderDetailObject = new PurchaseOrderDetailBase();

			try
			{

				SqlCommand cmd = GetSPCommand(CHECKMASSRESTOCKORDERDETAIL);

				AddParameter(cmd, pInt32Out(PurchaseOrderDetailBase.Property_IsExists));
				AddParameter(cmd, pGuid(PurchaseOrderDetailBase.Property_EquipmentId, EquipmentId));
				AddParameter(cmd, pGuid(PurchaseOrderDetailBase.Property_TechnicianId, TechnicianId));

				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderDetailObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderDetailBase.Property_IsExists);
				}
				return result;
			}
			catch (SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderDetailObject, x);
			}
		}
		#endregion


		#region Get PurchaseOrderDetail Maximum Id Method
		/// <summary>
		/// Retrieves Get Maximum Id of PurchaseOrderDetail
		/// </summary>
		/// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderDetail object
        /// </summary>
        /// <param name="purchaseOrderDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderDetailBase purchaseOrderDetailObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderDetailObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderDetailObject.PurchaseOrderId = reader.GetString( start + 1 );			
				purchaseOrderDetailObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) purchaseOrderDetailObject.EquipName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) purchaseOrderDetailObject.EquipDetail = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) purchaseOrderDetailObject.BundleId = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) purchaseOrderDetailObject.Quantity = reader.GetInt32( start + 6 );			
				if(!reader.IsDBNull(7)) purchaseOrderDetailObject.UnitPrice = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) purchaseOrderDetailObject.TotalPrice = reader.GetDouble( start + 8 );			
				purchaseOrderDetailObject.CreatedDate = reader.GetDateTime( start + 9 );			
				purchaseOrderDetailObject.CreatedBy = reader.GetGuid( start + 10 );			
				if(!reader.IsDBNull(11)) purchaseOrderDetailObject.RecieveQty = reader.GetInt32( start + 11 );			
				if(!reader.IsDBNull(12)) purchaseOrderDetailObject.BulkStatus = reader.GetBoolean( start + 12 );			
			FillBaseObject(purchaseOrderDetailObject, reader, (start + 13));

			
			purchaseOrderDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderDetail object
        /// </summary>
        /// <param name="purchaseOrderDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderDetailBase purchaseOrderDetailObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderDetail object</returns>
		private PurchaseOrderDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderDetail purchaseOrderDetailObject= new PurchaseOrderDetail();
					FillObject(purchaseOrderDetailObject, reader);
					return purchaseOrderDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderDetail objects</returns>
		private PurchaseOrderDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderDetail list
			PurchaseOrderDetailList list = new PurchaseOrderDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderDetail purchaseOrderDetailObject = new PurchaseOrderDetail();
					FillObject(purchaseOrderDetailObject, reader);

					list.Add(purchaseOrderDetailObject);
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
