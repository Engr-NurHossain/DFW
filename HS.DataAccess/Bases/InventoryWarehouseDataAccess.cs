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
	public partial class InventoryWarehouseDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVENTORYWAREHOUSE = "InsertInventoryWarehouse_v2";
		private const string UPDATEINVENTORYWAREHOUSE = "UpdateInventoryWarehouse";
		private const string DELETEINVENTORYWAREHOUSE = "DeleteInventoryWarehouse";
		private const string GETINVENTORYWAREHOUSEBYID = "GetInventoryWarehouseById";
		private const string GETALLINVENTORYWAREHOUSE = "GetAllInventoryWarehouse";
		private const string GETPAGEDINVENTORYWAREHOUSE = "GetPagedInventoryWarehouse";
		private const string GETINVENTORYWAREHOUSEMAXIMUMID = "GetInventoryWarehouseMaximumId";
		private const string GETINVENTORYWAREHOUSEROWCOUNT = "GetInventoryWarehouseRowCount";	
		private const string GETINVENTORYWAREHOUSEBYQUERY = "GetInventoryWarehouseByQuery";
		#endregion
		
		#region Constructors
		public InventoryWarehouseDataAccess(ClientContext context) : base(context) { }
		public InventoryWarehouseDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="inventoryWarehouseObject"></param>
		private void AddCommonParams(SqlCommand cmd, InventoryWarehouseBase inventoryWarehouseObject)
		{	
			AddParameter(cmd, pGuid(InventoryWarehouseBase.Property_CompanyId, inventoryWarehouseObject.CompanyId));
			AddParameter(cmd, pGuid(InventoryWarehouseBase.Property_EquipmentId, inventoryWarehouseObject.EquipmentId));
			AddParameter(cmd, pNVarChar(InventoryWarehouseBase.Property_Type, 50, inventoryWarehouseObject.Type));
			AddParameter(cmd, pInt32(InventoryWarehouseBase.Property_Quantity, inventoryWarehouseObject.Quantity));
			AddParameter(cmd, pNVarChar(InventoryWarehouseBase.Property_PurchaseOrderId, 50, inventoryWarehouseObject.PurchaseOrderId));
			AddParameter(cmd, pGuid(InventoryWarehouseBase.Property_LastUpdatedBy, inventoryWarehouseObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(InventoryWarehouseBase.Property_LastUpdatedDate, inventoryWarehouseObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(InventoryWarehouseBase.Property_Description, inventoryWarehouseObject.Description));
            AddParameter(cmd, pGuid(InventoryWarehouseBase.Property_LocationId, inventoryWarehouseObject.LocationId));
        }
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts InventoryWarehouse
        /// </summary>
        /// <param name="inventoryWarehouseObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InventoryWarehouseBase inventoryWarehouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVENTORYWAREHOUSE);
	
				AddParameter(cmd, pInt32Out(InventoryWarehouseBase.Property_Id));
				AddCommonParams(cmd, inventoryWarehouseObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					inventoryWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					inventoryWarehouseObject.Id = (Int32)GetOutParameter(cmd, InventoryWarehouseBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(inventoryWarehouseObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates InventoryWarehouse
        /// </summary>
        /// <param name="inventoryWarehouseObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(InventoryWarehouseBase inventoryWarehouseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVENTORYWAREHOUSE);
				
				AddParameter(cmd, pInt32(InventoryWarehouseBase.Property_Id, inventoryWarehouseObject.Id));
				AddCommonParams(cmd, inventoryWarehouseObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					inventoryWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(inventoryWarehouseObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes InventoryWarehouse
        /// </summary>
        /// <param name="Id">Id of the InventoryWarehouse object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVENTORYWAREHOUSE);	
				
				AddParameter(cmd, pInt32(InventoryWarehouseBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(InventoryWarehouse), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves InventoryWarehouse object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the InventoryWarehouse object to retrieve</param>
        /// <returns>InventoryWarehouse object, null if not found</returns>
		public InventoryWarehouse Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYWAREHOUSEBYID))
			{
				AddParameter( cmd, pInt32(InventoryWarehouseBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all InventoryWarehouse objects 
        /// </summary>
        /// <returns>A list of InventoryWarehouse objects</returns>
		public InventoryWarehouseList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVENTORYWAREHOUSE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all InventoryWarehouse objects by PageRequest
        /// </summary>
        /// <returns>A list of InventoryWarehouse objects</returns>
		public InventoryWarehouseList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVENTORYWAREHOUSE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InventoryWarehouseList _InventoryWarehouseList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InventoryWarehouseList;
			}
		}
		
		/// <summary>
        /// Retrieves all InventoryWarehouse objects by query String
        /// </summary>
        /// <returns>A list of InventoryWarehouse objects</returns>
		public InventoryWarehouseList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYWAREHOUSEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get InventoryWarehouse Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of InventoryWarehouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYWAREHOUSEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get InventoryWarehouse Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of InventoryWarehouse
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InventoryWarehouseRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYWAREHOUSEROWCOUNT))
			{
				SqlDataReader reader;
				_InventoryWarehouseRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InventoryWarehouseRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills InventoryWarehouse object
        /// </summary>
        /// <param name="inventoryWarehouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InventoryWarehouseBase inventoryWarehouseObject, SqlDataReader reader, int start)
		{
			
				inventoryWarehouseObject.Id = reader.GetInt32( start + 0 );			
				inventoryWarehouseObject.CompanyId = reader.GetGuid( start + 1 );			
				inventoryWarehouseObject.EquipmentId = reader.GetGuid( start + 2 );			
				inventoryWarehouseObject.Type = reader.GetString( start + 3 );			
				inventoryWarehouseObject.Quantity = reader.GetInt32( start + 4 );			
				inventoryWarehouseObject.PurchaseOrderId = reader.GetString( start + 5 );			
				inventoryWarehouseObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				inventoryWarehouseObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) inventoryWarehouseObject.Description = reader.GetString( start + 8 );			
			FillBaseObject(inventoryWarehouseObject, reader, (start + 9));

			
			inventoryWarehouseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills InventoryWarehouse object
        /// </summary>
        /// <param name="inventoryWarehouseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InventoryWarehouseBase inventoryWarehouseObject, SqlDataReader reader)
		{
			FillObject(inventoryWarehouseObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves InventoryWarehouse object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>InventoryWarehouse object</returns>
		private InventoryWarehouse GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					InventoryWarehouse inventoryWarehouseObject= new InventoryWarehouse();
					FillObject(inventoryWarehouseObject, reader);
					return inventoryWarehouseObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of InventoryWarehouse objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of InventoryWarehouse objects</returns>
		private InventoryWarehouseList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//InventoryWarehouse list
			InventoryWarehouseList list = new InventoryWarehouseList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					InventoryWarehouse inventoryWarehouseObject = new InventoryWarehouse();
					FillObject(inventoryWarehouseObject, reader);

					list.Add(inventoryWarehouseObject);
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
