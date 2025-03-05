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
	public partial class PurchaseOrderBranchDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPURCHASEORDERBRANCH = "InsertPurchaseOrderBranch";
		private const string UPDATEPURCHASEORDERBRANCH = "UpdatePurchaseOrderBranch";
		private const string DELETEPURCHASEORDERBRANCH = "DeletePurchaseOrderBranch";
		private const string GETPURCHASEORDERBRANCHBYID = "GetPurchaseOrderBranchById";
		private const string GETALLPURCHASEORDERBRANCH = "GetAllPurchaseOrderBranch";
		private const string GETPAGEDPURCHASEORDERBRANCH = "GetPagedPurchaseOrderBranch";
		private const string GETPURCHASEORDERBRANCHMAXIMUMID = "GetPurchaseOrderBranchMaximumId";
		private const string GETPURCHASEORDERBRANCHROWCOUNT = "GetPurchaseOrderBranchRowCount";	
		private const string GETPURCHASEORDERBRANCHBYQUERY = "GetPurchaseOrderBranchByQuery";
		#endregion
		
		#region Constructors
		public PurchaseOrderBranchDataAccess(ClientContext context) : base(context) { }
		public PurchaseOrderBranchDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="purchaseOrderBranchObject"></param>
		private void AddCommonParams(SqlCommand cmd, PurchaseOrderBranchBase purchaseOrderBranchObject)
		{	
			AddParameter(cmd, pGuid(PurchaseOrderBranchBase.Property_CompanyId, purchaseOrderBranchObject.CompanyId));
			AddParameter(cmd, pInt32(PurchaseOrderBranchBase.Property_BranchId, purchaseOrderBranchObject.BranchId));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_DemandOrderId, 50, purchaseOrderBranchObject.DemandOrderId));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_Action, 50, purchaseOrderBranchObject.Action));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_Status, 50, purchaseOrderBranchObject.Status));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_Location, 50, purchaseOrderBranchObject.Location));
			AddParameter(cmd, pBool(PurchaseOrderBranchBase.Property_IsReceived, purchaseOrderBranchObject.IsReceived));
			AddParameter(cmd, pGuid(PurchaseOrderBranchBase.Property_CreatedByUid, purchaseOrderBranchObject.CreatedByUid));
			AddParameter(cmd, pDateTime(PurchaseOrderBranchBase.Property_LastUpdatedDate, purchaseOrderBranchObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(PurchaseOrderBranchBase.Property_LastUpdatedByUid, purchaseOrderBranchObject.LastUpdatedByUid));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_TechDemandOrderId, 50, purchaseOrderBranchObject.TechDemandOrderId));
			AddParameter(cmd, pNVarChar(PurchaseOrderBranchBase.Property_Description, purchaseOrderBranchObject.Description));
			AddParameter(cmd, pDateTime(PurchaseOrderBranchBase.Property_CreatedDate, purchaseOrderBranchObject.CreatedDate));
			AddParameter(cmd, pBool(PurchaseOrderBranchBase.Property_IsBulkPO, purchaseOrderBranchObject.IsBulkPO));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PurchaseOrderBranch
        /// </summary>
        /// <param name="purchaseOrderBranchObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PurchaseOrderBranchBase purchaseOrderBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPURCHASEORDERBRANCH);
	
				AddParameter(cmd, pInt32Out(PurchaseOrderBranchBase.Property_Id));
				AddCommonParams(cmd, purchaseOrderBranchObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					purchaseOrderBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					purchaseOrderBranchObject.Id = (Int32)GetOutParameter(cmd, PurchaseOrderBranchBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(purchaseOrderBranchObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PurchaseOrderBranch
        /// </summary>
        /// <param name="purchaseOrderBranchObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PurchaseOrderBranchBase purchaseOrderBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPURCHASEORDERBRANCH);
				
				AddParameter(cmd, pInt32(PurchaseOrderBranchBase.Property_Id, purchaseOrderBranchObject.Id));
				AddCommonParams(cmd, purchaseOrderBranchObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					purchaseOrderBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(purchaseOrderBranchObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PurchaseOrderBranch
        /// </summary>
        /// <param name="Id">Id of the PurchaseOrderBranch object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPURCHASEORDERBRANCH);	
				
				AddParameter(cmd, pInt32(PurchaseOrderBranchBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PurchaseOrderBranch), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PurchaseOrderBranch object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PurchaseOrderBranch object to retrieve</param>
        /// <returns>PurchaseOrderBranch object, null if not found</returns>
		public PurchaseOrderBranch Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHBYID))
			{
				AddParameter( cmd, pInt32(PurchaseOrderBranchBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PurchaseOrderBranch objects 
        /// </summary>
        /// <returns>A list of PurchaseOrderBranch objects</returns>
		public PurchaseOrderBranchList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPURCHASEORDERBRANCH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PurchaseOrderBranch objects by PageRequest
        /// </summary>
        /// <returns>A list of PurchaseOrderBranch objects</returns>
		public PurchaseOrderBranchList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPURCHASEORDERBRANCH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PurchaseOrderBranchList _PurchaseOrderBranchList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PurchaseOrderBranchList;
			}
		}
		
		/// <summary>
        /// Retrieves all PurchaseOrderBranch objects by query String
        /// </summary>
        /// <returns>A list of PurchaseOrderBranch objects</returns>
		public PurchaseOrderBranchList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PurchaseOrderBranch Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PurchaseOrderBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PurchaseOrderBranch Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PurchaseOrderBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PurchaseOrderBranchRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPURCHASEORDERBRANCHROWCOUNT))
			{
				SqlDataReader reader;
				_PurchaseOrderBranchRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PurchaseOrderBranchRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PurchaseOrderBranch object
        /// </summary>
        /// <param name="purchaseOrderBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PurchaseOrderBranchBase purchaseOrderBranchObject, SqlDataReader reader, int start)
		{
			
				purchaseOrderBranchObject.Id = reader.GetInt32( start + 0 );			
				purchaseOrderBranchObject.CompanyId = reader.GetGuid( start + 1 );			
				purchaseOrderBranchObject.BranchId = reader.GetInt32( start + 2 );			
				purchaseOrderBranchObject.DemandOrderId = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) purchaseOrderBranchObject.Action = reader.GetString( start + 4 );			
				purchaseOrderBranchObject.Status = reader.GetString( start + 5 );			
				purchaseOrderBranchObject.Location = reader.GetString( start + 6 );			
				purchaseOrderBranchObject.IsReceived = reader.GetBoolean( start + 7 );			
				purchaseOrderBranchObject.CreatedByUid = reader.GetGuid( start + 8 );			
				purchaseOrderBranchObject.LastUpdatedDate = reader.GetDateTime( start + 9 );			
				purchaseOrderBranchObject.LastUpdatedByUid = reader.GetGuid( start + 10 );			
				if(!reader.IsDBNull(11)) purchaseOrderBranchObject.TechDemandOrderId = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) purchaseOrderBranchObject.Description = reader.GetString( start + 12 );			
				purchaseOrderBranchObject.CreatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) purchaseOrderBranchObject.IsBulkPO = reader.GetBoolean( start + 14 );			
			FillBaseObject(purchaseOrderBranchObject, reader, (start + 15));

			
			purchaseOrderBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PurchaseOrderBranch object
        /// </summary>
        /// <param name="purchaseOrderBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PurchaseOrderBranchBase purchaseOrderBranchObject, SqlDataReader reader)
		{
			FillObject(purchaseOrderBranchObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PurchaseOrderBranch object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PurchaseOrderBranch object</returns>
		private PurchaseOrderBranch GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PurchaseOrderBranch purchaseOrderBranchObject= new PurchaseOrderBranch();
					FillObject(purchaseOrderBranchObject, reader);
					return purchaseOrderBranchObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PurchaseOrderBranch objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PurchaseOrderBranch objects</returns>
		private PurchaseOrderBranchList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PurchaseOrderBranch list
			PurchaseOrderBranchList list = new PurchaseOrderBranchList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PurchaseOrderBranch purchaseOrderBranchObject = new PurchaseOrderBranch();
					FillObject(purchaseOrderBranchObject, reader);

					list.Add(purchaseOrderBranchObject);
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
