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
	public partial class PurchaseOrderBranchReceivedDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERBRANCHRECEIVED = "InsertPurchaseOrderBranchReceived";
		private const string UPDATEPURCHASEORDERBRANCHRECEIVED = "UpdatePurchaseOrderBranchReceived";
		private const string DELETEPURCHASEORDERBRANCHRECEIVED = "DeletePurchaseOrderBranchReceived";
		private const string GETPURCHASEORDERBRANCHRECEIVEDBYID = "GetPurchaseOrderBranchReceivedById";
		private const string GETALLPURCHASEORDERBRANCHRECEIVED = "GetAllPurchaseOrderBranchReceived";
		private const string GETPAGEDPURCHASEORDERBRANCHRECEIVED = "GetPagedPurchaseOrderBranchReceived";
		private const string GETPURCHASEORDERBRANCHRECEIVEDMAXIMUMID = "GetPurchaseOrderBranchReceivedMaximumId";
		private const string GETPURCHASEORDERBRANCHRECEIVEDROWCOUNT = "GetPurchaseOrderBranchReceivedRowCount";	
		private const string GETPURCHASEORDERBRANCHRECEIVEDBYQUERY = "GetPurchaseOrderBranchReceivedByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderBranchReceivedDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderBranchReceivedDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderBranchReceivedObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderBranchReceivedBase purchaseOrderBranchReceivedObject)
		{	
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchReceivedBase.Property_BranchDemandOrderId, 15, purchaseOrderBranchReceivedObject.BranchDemandOrderId));
			AddParameter(cmd, pGuid(PurchaseOrderBranchReceivedBase.Property_EquipmentId, purchaseOrderBranchReceivedObject.EquipmentId));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchReceivedBase.Property_EquipName, 500, purchaseOrderBranchReceivedObject.EquipName));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchReceivedBase.Property_EquipDetail, purchaseOrderBranchReceivedObject.EquipDetail));
			AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_BundleId, purchaseOrderBranchReceivedObject.BundleId));
			AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_Quantity, purchaseOrderBranchReceivedObject.Quantity));
			AddParameter(cmd, pDouble(PurchaseOrderBranchReceivedBase.Property_UnitPrice, purchaseOrderBranchReceivedObject.UnitPrice));
			AddParameter(cmd, pDouble(PurchaseOrderBranchReceivedBase.Property_TotalPrice, purchaseOrderBranchReceivedObject.TotalPrice));
			AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_RecieveQty, purchaseOrderBranchReceivedObject.RecieveQty));
			AddParameter(cmd, pBool(PurchaseOrderBranchReceivedBase.Property_IsReceived, purchaseOrderBranchReceivedObject.IsReceived));
			AddParameter(cmd, pDateTime(PurchaseOrderBranchReceivedBase.Property_ReceivedDate, purchaseOrderBranchReceivedObject.ReceivedDate));
			AddParameter(cmd, pGuid(PurchaseOrderBranchReceivedBase.Property_ReceivedBy, purchaseOrderBranchReceivedObject.ReceivedBy));
			AddParameter(cmd, pDateTime(PurchaseOrderBranchReceivedBase.Property_CreatedDate, purchaseOrderBranchReceivedObject.CreatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderBranchReceivedBase.Property_CreatedBy, purchaseOrderBranchReceivedObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderBranchReceived
        /// </summary>
        /// <param name="purchaseOrderBranchReceivedObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderBranchReceivedBase purchaseOrderBranchReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERBRANCHRECEIVED);
	
				AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_Id, purchaseOrderBranchReceivedObject.Id));
				AddCommonParams(cmd, purchaseOrderBranchReceivedObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderBranchReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderBranchReceivedObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderBranchReceived
        /// </summary>
        /// <param name="purchaseOrderBranchReceivedObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderBranchReceivedBase purchaseOrderBranchReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERBRANCHRECEIVED);
				
				AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_Id, purchaseOrderBranchReceivedObject.Id));
				AddCommonParams(cmd, purchaseOrderBranchReceivedObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderBranchReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderBranchReceivedObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderBranchReceived
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderBranchReceived object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERBRANCHRECEIVED);	
				
				AddParameter(cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderBranchReceived), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderBranchReceived object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderBranchReceived object to retrieve</param>
        /// <returns>PurchaseOrderBranchReceived object, null if not found</returns>
		public PurchaseOrderBranchReceived Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHRECEIVEDBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderBranchReceivedBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderBranchReceived objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderBranchReceived objects</returns>
		public PurchaseOrderBranchReceivedList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERBRANCHRECEIVED))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderBranchReceived objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderBranchReceived objects</returns>
		public PurchaseOrderBranchReceivedList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERBRANCHRECEIVED))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderBranchReceivedList _PurchaseOrderBranchReceivedList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderBranchReceivedList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderBranchReceived objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderBranchReceived objects</returns>
		public PurchaseOrderBranchReceivedList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHRECEIVEDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrderBranchReceived Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrderBranchReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHRECEIVEDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderBranchReceived Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderBranchReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderBranchReceivedRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHRECEIVEDROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderBranchReceivedRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderBranchReceivedRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderBranchReceived object
        /// </summary>
        /// <param name="purchaseOrderBranchReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderBranchReceivedBase purchaseOrderBranchReceivedObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderBranchReceivedObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderBranchReceivedObject.BranchDemandOrderId = reader.GetString( start + 1 );			
				purchaseOrderBranchReceivedObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) purchaseOrderBranchReceivedObject.EquipName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) purchaseOrderBranchReceivedObject.EquipDetail = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) purchaseOrderBranchReceivedObject.BundleId = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) purchaseOrderBranchReceivedObject.Quantity = reader.GetInt32( start + 6 );			
				if(!reader.IsDBNull(7)) purchaseOrderBranchReceivedObject.UnitPrice = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) purchaseOrderBranchReceivedObject.TotalPrice = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) purchaseOrderBranchReceivedObject.RecieveQty = reader.GetInt32( start + 9 );			
				if(!reader.IsDBNull(10)) purchaseOrderBranchReceivedObject.IsReceived = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) purchaseOrderBranchReceivedObject.ReceivedDate = reader.GetDateTime( start + 11 );			
				purchaseOrderBranchReceivedObject.ReceivedBy = reader.GetGuid( start + 12 );			
				purchaseOrderBranchReceivedObject.CreatedDate = reader.GetDateTime( start + 13 );			
				purchaseOrderBranchReceivedObject.CreatedBy = reader.GetGuid( start + 14 );			
			FillBaseObject(purchaseOrderBranchReceivedObject, reader, (start + 15));

			
			purchaseOrderBranchReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderBranchReceived object
        /// </summary>
        /// <param name="purchaseOrderBranchReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderBranchReceivedBase purchaseOrderBranchReceivedObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderBranchReceivedObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderBranchReceived object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderBranchReceived object</returns>
		private PurchaseOrderBranchReceived GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderBranchReceived purchaseOrderBranchReceivedObject= new PurchaseOrderBranchReceived();
					FillObject(purchaseOrderBranchReceivedObject, reader);
					return purchaseOrderBranchReceivedObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderBranchReceived objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderBranchReceived objects</returns>
		private PurchaseOrderBranchReceivedList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderBranchReceived list
			PurchaseOrderBranchReceivedList list = new PurchaseOrderBranchReceivedList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderBranchReceived purchaseOrderBranchReceivedObject = new PurchaseOrderBranchReceived();
					FillObject(purchaseOrderBranchReceivedObject, reader);

					list.Add(purchaseOrderBranchReceivedObject);
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
