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
	public partial class InventoryBranchDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVENTORYBRANCH = "InsertInventoryBranch";
		private const string UPDATEINVENTORYBRANCH = "UpdateInventoryBranch";
		private const string DELETEINVENTORYBRANCH = "DeleteInventoryBranch";
		private const string GETINVENTORYBRANCHBYID = "GetInventoryBranchById";
		private const string GETALLINVENTORYBRANCH = "GetAllInventoryBranch";
		private const string GETPAGEDINVENTORYBRANCH = "GetPagedInventoryBranch";
		private const string GETINVENTORYBRANCHMAXIMUMID = "GetInventoryBranchMaximumId";
		private const string GETINVENTORYBRANCHROWCOUNT = "GetInventoryBranchRowCount";	
		private const string GETINVENTORYBRANCHBYQUERY = "GetInventoryBranchByQuery";
		#endregion
		
		#region Constructors
		public InventoryBranchDataAccess(ClientContext context) : base(context) { }
		public InventoryBranchDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="inventoryBranchObject"></param>
		private void AddCommonParams(SqlCommand cmd, InventoryBranchBase inventoryBranchObject)
		{	
			AddParameter(cmd, pGuid(InventoryBranchBase.Property_CompanyId, inventoryBranchObject.CompanyId));
			AddParameter(cmd, pInt32(InventoryBranchBase.Property_BranchId, inventoryBranchObject.BranchId));
			AddParameter(cmd, pGuid(InventoryBranchBase.Property_EquipmentId, inventoryBranchObject.EquipmentId));
			AddParameter(cmd, pNVarChar(InventoryBranchBase.Property_Type, 50, inventoryBranchObject.Type));
			AddParameter(cmd, pInt32(InventoryBranchBase.Property_Quantity, inventoryBranchObject.Quantity));
			AddParameter(cmd, pNVarChar(InventoryBranchBase.Property_PurchaseOrderId, 50, inventoryBranchObject.PurchaseOrderId));
			AddParameter(cmd, pGuid(InventoryBranchBase.Property_LastUpdatedBy, inventoryBranchObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(InventoryBranchBase.Property_LastUpdatedDate, inventoryBranchObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(InventoryBranchBase.Property_Description, inventoryBranchObject.Description));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts InventoryBranch
        /// </summary>
        /// <param name="inventoryBranchObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InventoryBranchBase inventoryBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVENTORYBRANCH);
	
				AddParameter(cmd, pInt32Out(InventoryBranchBase.Property_Id));
				AddCommonParams(cmd, inventoryBranchObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					inventoryBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					inventoryBranchObject.Id = (Int32)GetOutParameter(cmd, InventoryBranchBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(inventoryBranchObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates InventoryBranch
        /// </summary>
        /// <param name="inventoryBranchObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(InventoryBranchBase inventoryBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVENTORYBRANCH);
				
				AddParameter(cmd, pInt32(InventoryBranchBase.Property_Id, inventoryBranchObject.Id));
				AddCommonParams(cmd, inventoryBranchObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					inventoryBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(inventoryBranchObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes InventoryBranch
        /// </summary>
        /// <param name="Id">Id of the InventoryBranch object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVENTORYBRANCH);	
				
				AddParameter(cmd, pInt32(InventoryBranchBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(InventoryBranch), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves InventoryBranch object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the InventoryBranch object to retrieve</param>
        /// <returns>InventoryBranch object, null if not found</returns>
		public InventoryBranch Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBRANCHBYID))
			{
				AddParameter( cmd, pInt32(InventoryBranchBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all InventoryBranch objects 
        /// </summary>
        /// <returns>A list of InventoryBranch objects</returns>
		public InventoryBranchList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVENTORYBRANCH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all InventoryBranch objects by PageRequest
        /// </summary>
        /// <returns>A list of InventoryBranch objects</returns>
		public InventoryBranchList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVENTORYBRANCH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InventoryBranchList _InventoryBranchList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InventoryBranchList;
			}
		}
		
		/// <summary>
        /// Retrieves all InventoryBranch objects by query String
        /// </summary>
        /// <returns>A list of InventoryBranch objects</returns>
		public InventoryBranchList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBRANCHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get InventoryBranch Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of InventoryBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBRANCHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get InventoryBranch Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of InventoryBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InventoryBranchRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBRANCHROWCOUNT))
			{
				SqlDataReader reader;
				_InventoryBranchRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InventoryBranchRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills InventoryBranch object
        /// </summary>
        /// <param name="inventoryBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InventoryBranchBase inventoryBranchObject, SqlDataReader reader, int start)
		{
			
				inventoryBranchObject.Id = reader.GetInt32( start + 0 );			
				inventoryBranchObject.CompanyId = reader.GetGuid( start + 1 );			
				inventoryBranchObject.BranchId = reader.GetInt32( start + 2 );			
				inventoryBranchObject.EquipmentId = reader.GetGuid( start + 3 );			
				inventoryBranchObject.Type = reader.GetString( start + 4 );			
				inventoryBranchObject.Quantity = reader.GetInt32( start + 5 );			
				inventoryBranchObject.PurchaseOrderId = reader.GetString( start + 6 );			
				inventoryBranchObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				inventoryBranchObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) inventoryBranchObject.Description = reader.GetString( start + 9 );			
			FillBaseObject(inventoryBranchObject, reader, (start + 10));

			
			inventoryBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills InventoryBranch object
        /// </summary>
        /// <param name="inventoryBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InventoryBranchBase inventoryBranchObject, SqlDataReader reader)
		{
			FillObject(inventoryBranchObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves InventoryBranch object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>InventoryBranch object</returns>
		private InventoryBranch GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					InventoryBranch inventoryBranchObject= new InventoryBranch();
					FillObject(inventoryBranchObject, reader);
					return inventoryBranchObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of InventoryBranch objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of InventoryBranch objects</returns>
		private InventoryBranchList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//InventoryBranch list
			InventoryBranchList list = new InventoryBranchList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					InventoryBranch inventoryBranchObject = new InventoryBranch();
					FillObject(inventoryBranchObject, reader);

					list.Add(inventoryBranchObject);
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
