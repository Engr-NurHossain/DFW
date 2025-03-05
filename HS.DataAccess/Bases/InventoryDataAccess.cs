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
	public partial class InventoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVENTORY = "InsertInventory";
		private const string UPDATEINVENTORY = "UpdateInventory";
		private const string DELETEINVENTORY = "DeleteInventory";
		private const string GETINVENTORYBYID = "GetInventoryById";
		private const string GETALLINVENTORY = "GetAllInventory";
		private const string GETPAGEDINVENTORY = "GetPagedInventory";
		private const string GETINVENTORYMAXIMUMID = "GetInventoryMaximumId";
		private const string GETINVENTORYROWCOUNT = "GetInventoryRowCount";	
		private const string GETINVENTORYBYQUERY = "GetInventoryByQuery";
		#endregion
		
		#region Constructors
		public InventoryDataAccess(ClientContext context) : base(context) { }
		public InventoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="inventoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, InventoryBase inventoryObject)
		{	
			AddParameter(cmd, pGuid(InventoryBase.Property_InventoryId, inventoryObject.InventoryId));
			AddParameter(cmd, pGuid(InventoryBase.Property_EquipmentId, inventoryObject.EquipmentId));
			AddParameter(cmd, pGuid(InventoryBase.Property_CompanyId, inventoryObject.CompanyId));
			AddParameter(cmd, pInt32(InventoryBase.Property_Quantity, inventoryObject.Quantity));
			AddParameter(cmd, pNVarChar(InventoryBase.Property_Type, 50, inventoryObject.Type));
			AddParameter(cmd, pDouble(InventoryBase.Property_SupplierCost, inventoryObject.SupplierCost));
			AddParameter(cmd, pDouble(InventoryBase.Property_Cost, inventoryObject.Cost));
			AddParameter(cmd, pDouble(InventoryBase.Property_Retail, inventoryObject.Retail));
			AddParameter(cmd, pDateTime(InventoryBase.Property_CreatedDate, inventoryObject.CreatedDate));
			AddParameter(cmd, pNVarChar(InventoryBase.Property_CreatedBy, 50, inventoryObject.CreatedBy));
			AddParameter(cmd, pGuid(InventoryBase.Property_CreatedByUid, inventoryObject.CreatedByUid));
			AddParameter(cmd, pNVarChar(InventoryBase.Property_PurchaseOrderId, 50, inventoryObject.PurchaseOrderId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Inventory
        /// </summary>
        /// <param name="inventoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InventoryBase inventoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVENTORY);
	
				AddParameter(cmd, pInt32Out(InventoryBase.Property_Id));
				AddCommonParams(cmd, inventoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					inventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					inventoryObject.Id = (Int32)GetOutParameter(cmd, InventoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(inventoryObject, x);
			}
		}
        public long InsertInventoryPO(InventoryAssignedPO inventoryObject)
        {
            try
            {	

                    string cmdText = @"
                INSERT INTO [dbo].[AssignedInventoryTechReceived]
                (
                    [TechnicianId],
                    [EquipmentId],
                    [Quantity],
                    [IsReceived],
                    [ReceivedDate],
                    [ReceivedBy],
                    [CreatedDate],
                    [CreatedBy],
                    [IsApprove],
                    [IsDecline],
                    [ReqSrc],
                    [ClosedBy]
                )
                VALUES
                (
                    @TechnicianId,
                    @EquipmentId,
                    @Quantity,
                    @IsReceived,
                    @ReceivedDate,
                    @ReceivedBy,
                    GETDATE(),
                    @CreatedBy,
                    @IsApprove,
                    @IsDecline,
                    @ReqSrc,
                    @ClosedBy
                );
            ";
				    SqlCommand cmd = GetSQLCommand(cmdText);
                    using (cmd)
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", new Guid(inventoryObject.TechnicianId));
                        cmd.Parameters.AddWithValue("@EquipmentId", new Guid(inventoryObject.EquipmentId));
                        cmd.Parameters.AddWithValue("@Quantity", inventoryObject.Quantity);
                        cmd.Parameters.AddWithValue("@IsReceived", inventoryObject.IsReceived);
                        cmd.Parameters.AddWithValue("@ReceivedDate", inventoryObject.ReceivedDate);
                        cmd.Parameters.AddWithValue("@ReceivedBy", new Guid(inventoryObject.ReceivedBy));
                 //       cmd.Parameters.AddWithValue("@CreatedBy", new Guid(inventoryObject.CreatedBy));
                        cmd.Parameters.AddWithValue("@IsApprove", inventoryObject.IsApprove);
                        cmd.Parameters.AddWithValue("@IsDecline", inventoryObject.IsDecline);
                        cmd.Parameters.AddWithValue("@ReqSrc", inventoryObject.ReqSrc??string.Empty);
                        cmd.Parameters.AddWithValue("@ClosedBy",new Guid(inventoryObject.ClosedBy));
					    cmd.Parameters.AddWithValue("@CreatedBy", new Guid(inventoryObject.CreatedBy));

                    long result = InsertRecord(cmd);
                        return result;
                    }
            }
            catch 
            {
                return -1; 
            }
        }

        #endregion

        #region Update Method
        /// <summary>
        /// Updates Inventory
        /// </summary>
        /// <param name="inventoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
        public long Update(InventoryBase inventoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVENTORY);
				
				AddParameter(cmd, pInt32(InventoryBase.Property_Id, inventoryObject.Id));
				AddCommonParams(cmd, inventoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					inventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(inventoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Inventory
        /// </summary>
        /// <param name="Id">Id of the Inventory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVENTORY);	
				
				AddParameter(cmd, pInt32(InventoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Inventory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Inventory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Inventory object to retrieve</param>
        /// <returns>Inventory object, null if not found</returns>
		public Inventory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBYID))
			{
				AddParameter( cmd, pInt32(InventoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Inventory objects 
        /// </summary>
        /// <returns>A list of Inventory objects</returns>
		public InventoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVENTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Inventory objects by PageRequest
        /// </summary>
        /// <returns>A list of Inventory objects</returns>
		public InventoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVENTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InventoryList _InventoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InventoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all Inventory objects by query String
        /// </summary>
        /// <returns>A list of Inventory objects</returns>
		public InventoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Inventory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Inventory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Inventory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Inventory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InventoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYROWCOUNT))
			{
				SqlDataReader reader;
				_InventoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InventoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Inventory object
        /// </summary>
        /// <param name="inventoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InventoryBase inventoryObject, SqlDataReader reader, int start)
		{
			
				inventoryObject.Id = reader.GetInt32( start + 0 );			
				inventoryObject.InventoryId = reader.GetGuid( start + 1 );			
				inventoryObject.EquipmentId = reader.GetGuid( start + 2 );			
				inventoryObject.CompanyId = reader.GetGuid( start + 3 );			
				inventoryObject.Quantity = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) inventoryObject.Type = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) inventoryObject.SupplierCost = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) inventoryObject.Cost = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) inventoryObject.Retail = reader.GetDouble( start + 8 );			
				inventoryObject.CreatedDate = reader.GetDateTime( start + 9 );			
				inventoryObject.CreatedBy = reader.GetString( start + 10 );			
				inventoryObject.CreatedByUid = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) inventoryObject.PurchaseOrderId = reader.GetString( start + 12 );			
			FillBaseObject(inventoryObject, reader, (start + 13));

			
			inventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Inventory object
        /// </summary>
        /// <param name="inventoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InventoryBase inventoryObject, SqlDataReader reader)
		{
			FillObject(inventoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Inventory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Inventory object</returns>
		private Inventory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Inventory inventoryObject= new Inventory();
					FillObject(inventoryObject, reader);
					return inventoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Inventory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Inventory objects</returns>
		private InventoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Inventory list
			InventoryList list = new InventoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Inventory inventoryObject = new Inventory();
					FillObject(inventoryObject, reader);

					list.Add(inventoryObject);
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
