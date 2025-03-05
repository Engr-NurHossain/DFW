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
	public partial class PurchaseOrderTechReceivedDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERTECHRECEIVED = "InsertPurchaseOrderTechReceived";
		private const string UPDATEPURCHASEORDERTECHRECEIVED = "UpdatePurchaseOrderTechReceived";
		private const string DELETEPURCHASEORDERTECHRECEIVED = "DeletePurchaseOrderTechReceived";
		private const string GETPURCHASEORDERTECHRECEIVEDBYID = "GetPurchaseOrderTechReceivedById";
		private const string GETALLPURCHASEORDERTECHRECEIVED = "GetAllPurchaseOrderTechReceived";
		private const string GETPAGEDPURCHASEORDERTECHRECEIVED = "GetPagedPurchaseOrderTechReceived";
		private const string GETPURCHASEORDERTECHRECEIVEDMAXIMUMID = "GetPurchaseOrderTechReceivedMaximumId";
		private const string GETPURCHASEORDERTECHRECEIVEDROWCOUNT = "GetPurchaseOrderTechReceivedRowCount";	
		private const string GETPURCHASEORDERTECHRECEIVEDBYQUERY = "GetPurchaseOrderTechReceivedByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderTechReceivedDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderTechReceivedDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderTechReceivedObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderTechReceivedBase purchaseOrderTechReceivedObject)
		{	
			AddParameter(cmd, pNVarChar(PurchaseOrderTechReceivedBase.Property_BranchDemandOrderId, 15, purchaseOrderTechReceivedObject.BranchDemandOrderId));
			AddParameter(cmd, pGuid(PurchaseOrderTechReceivedBase.Property_EquipmentId, purchaseOrderTechReceivedObject.EquipmentId));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechReceivedBase.Property_EquipName, 500, purchaseOrderTechReceivedObject.EquipName));
			AddParameter(cmd, pNVarChar(PurchaseOrderTechReceivedBase.Property_EquipDetail, purchaseOrderTechReceivedObject.EquipDetail));
			AddParameter(cmd, pInt32(PurchaseOrderTechReceivedBase.Property_BundleId, purchaseOrderTechReceivedObject.BundleId));
			AddParameter(cmd, pInt32(PurchaseOrderTechReceivedBase.Property_Quantity, purchaseOrderTechReceivedObject.Quantity));
			AddParameter(cmd, pDouble(PurchaseOrderTechReceivedBase.Property_UnitPrice, purchaseOrderTechReceivedObject.UnitPrice));
			AddParameter(cmd, pDouble(PurchaseOrderTechReceivedBase.Property_TotalPrice, purchaseOrderTechReceivedObject.TotalPrice));
			AddParameter(cmd, pInt32(PurchaseOrderTechReceivedBase.Property_RecieveQty, purchaseOrderTechReceivedObject.RecieveQty));
			AddParameter(cmd, pBool(PurchaseOrderTechReceivedBase.Property_IsReceived, purchaseOrderTechReceivedObject.IsReceived));
			AddParameter(cmd, pDateTime(PurchaseOrderTechReceivedBase.Property_ReceivedDate, purchaseOrderTechReceivedObject.ReceivedDate));
			AddParameter(cmd, pGuid(PurchaseOrderTechReceivedBase.Property_ReceivedBy, purchaseOrderTechReceivedObject.ReceivedBy));
			AddParameter(cmd, pDateTime(PurchaseOrderTechReceivedBase.Property_CreatedDate, purchaseOrderTechReceivedObject.CreatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderTechReceivedBase.Property_CreatedBy, purchaseOrderTechReceivedObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderTechReceived
        /// </summary>
        /// <param name="purchaseOrderTechReceivedObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderTechReceivedBase purchaseOrderTechReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERTECHRECEIVED);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderTechReceivedBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderTechReceivedObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderTechReceivedObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderTechReceivedBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderTechReceivedObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderTechReceived
        /// </summary>
        /// <param name="purchaseOrderTechReceivedObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderTechReceivedBase purchaseOrderTechReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERTECHRECEIVED);
				
				AddParameter(cmd, pInt32(PurchaseOrderTechReceivedBase.Property_Id, purchaseOrderTechReceivedObject.Id));
				AddCommonParams(cmd, purchaseOrderTechReceivedObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderTechReceivedObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderTechReceived
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderTechReceived object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERTECHRECEIVED);	
				
				AddParameter(cmd, pInt32(PurchaseOrderTechReceivedBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderTechReceived), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderTechReceived object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderTechReceived object to retrieve</param>
        /// <returns>PurchaseOrderTechReceived object, null if not found</returns>
		public PurchaseOrderTechReceived Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHRECEIVEDBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderTechReceivedBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderTechReceived objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderTechReceived objects</returns>
		public PurchaseOrderTechReceivedList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERTECHRECEIVED))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderTechReceived objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderTechReceived objects</returns>
		public PurchaseOrderTechReceivedList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERTECHRECEIVED))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderTechReceivedList _PurchaseOrderTechReceivedList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderTechReceivedList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderTechReceived objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderTechReceived objects</returns>
		public PurchaseOrderTechReceivedList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHRECEIVEDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrderTechReceived Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrderTechReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHRECEIVEDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderTechReceived Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderTechReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderTechReceivedRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERTECHRECEIVEDROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderTechReceivedRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderTechReceivedRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderTechReceived object
        /// </summary>
        /// <param name="purchaseOrderTechReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderTechReceivedBase purchaseOrderTechReceivedObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderTechReceivedObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderTechReceivedObject.BranchDemandOrderId = reader.GetString( start + 1 );			
				purchaseOrderTechReceivedObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) purchaseOrderTechReceivedObject.EquipName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) purchaseOrderTechReceivedObject.EquipDetail = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) purchaseOrderTechReceivedObject.BundleId = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) purchaseOrderTechReceivedObject.Quantity = reader.GetInt32( start + 6 );			
				if(!reader.IsDBNull(7)) purchaseOrderTechReceivedObject.UnitPrice = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) purchaseOrderTechReceivedObject.TotalPrice = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) purchaseOrderTechReceivedObject.RecieveQty = reader.GetInt32( start + 9 );			
				if(!reader.IsDBNull(10)) purchaseOrderTechReceivedObject.IsReceived = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) purchaseOrderTechReceivedObject.ReceivedDate = reader.GetDateTime( start + 11 );			
				purchaseOrderTechReceivedObject.ReceivedBy = reader.GetGuid( start + 12 );			
				purchaseOrderTechReceivedObject.CreatedDate = reader.GetDateTime( start + 13 );			
				purchaseOrderTechReceivedObject.CreatedBy = reader.GetGuid( start + 14 );			
			FillBaseObject(purchaseOrderTechReceivedObject, reader, (start + 15));

			
			purchaseOrderTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderTechReceived object
        /// </summary>
        /// <param name="purchaseOrderTechReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderTechReceivedBase purchaseOrderTechReceivedObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderTechReceivedObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderTechReceived object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderTechReceived object</returns>
		private PurchaseOrderTechReceived GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderTechReceived purchaseOrderTechReceivedObject= new PurchaseOrderTechReceived();
					FillObject(purchaseOrderTechReceivedObject, reader);
					return purchaseOrderTechReceivedObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderTechReceived objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderTechReceived objects</returns>
		private PurchaseOrderTechReceivedList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderTechReceived list
			PurchaseOrderTechReceivedList list = new PurchaseOrderTechReceivedList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderTechReceived purchaseOrderTechReceivedObject = new PurchaseOrderTechReceived();
					FillObject(purchaseOrderTechReceivedObject, reader);

					list.Add(purchaseOrderTechReceivedObject);
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
