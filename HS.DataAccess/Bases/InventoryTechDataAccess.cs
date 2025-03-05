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
	public partial class InventoryTechDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINVENTORYTECH = "InsertInventoryTech";
		private const string UPDATEINVENTORYTECH = "UpdateInventoryTech";
		private const string DELETEINVENTORYTECH = "DeleteInventoryTech";
		private const string GETINVENTORYTECHBYID = "GetInventoryTechById";
		private const string GETALLINVENTORYTECH = "GetAllInventoryTech";
		private const string GETPAGEDINVENTORYTECH = "GetPagedInventoryTech";
		private const string GETINVENTORYTECHMAXIMUMID = "GetInventoryTechMaximumId";
		private const string GETINVENTORYTECHROWCOUNT = "GetInventoryTechRowCount";	
		private const string GETINVENTORYTECHBYQUERY = "GetInventoryTechByQuery";
		#endregion
		
		#region Constructors
		public InventoryTechDataAccess(ClientContext context) : base(context) { }
		public InventoryTechDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="inventoryTechObject"></param>
		private void AddCommonParams(SqlCommand cmd, InventoryTechBase inventoryTechObject)
		{	
			AddParameter(cmd, pGuid(InventoryTechBase.Property_CompanyId, inventoryTechObject.CompanyId));
			AddParameter(cmd, pGuid(InventoryTechBase.Property_TechnicianId, inventoryTechObject.TechnicianId));
			AddParameter(cmd, pGuid(InventoryTechBase.Property_EquipmentId, inventoryTechObject.EquipmentId));
			AddParameter(cmd, pNVarChar(InventoryTechBase.Property_Type, 50, inventoryTechObject.Type));
			AddParameter(cmd, pInt32(InventoryTechBase.Property_Quantity, inventoryTechObject.Quantity));
			AddParameter(cmd, pNVarChar(InventoryTechBase.Property_PurchaseOrderId, 50, inventoryTechObject.PurchaseOrderId));
			AddParameter(cmd, pGuid(InventoryTechBase.Property_LastUpdatedBy, inventoryTechObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(InventoryTechBase.Property_LastUpdatedDate, inventoryTechObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(InventoryTechBase.Property_Description, inventoryTechObject.Description));
			AddParameter(cmd, pInt32(InventoryTechBase.Property_CustomerAppointmentEquipmentId, inventoryTechObject.CustomerAppointmentEquipmentId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts InventoryTech
        /// </summary>
        /// <param name="inventoryTechObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(InventoryTechBase inventoryTechObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINVENTORYTECH);
	
				AddParameter(cmd, pInt32Out(InventoryTechBase.Property_Id));
				AddCommonParams(cmd, inventoryTechObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					inventoryTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					inventoryTechObject.Id = (Int32)GetOutParameter(cmd, InventoryTechBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(inventoryTechObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates InventoryTech
        /// </summary>
        /// <param name="inventoryTechObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(InventoryTechBase inventoryTechObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINVENTORYTECH);
				
				AddParameter(cmd, pInt32(InventoryTechBase.Property_Id, inventoryTechObject.Id));
				AddCommonParams(cmd, inventoryTechObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					inventoryTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(inventoryTechObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes InventoryTech
        /// </summary>
        /// <param name="Id">Id of the InventoryTech object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINVENTORYTECH);	
				
				AddParameter(cmd, pInt32(InventoryTechBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(InventoryTech), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves InventoryTech object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the InventoryTech object to retrieve</param>
        /// <returns>InventoryTech object, null if not found</returns>
		public InventoryTech Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYTECHBYID))
			{
				AddParameter( cmd, pInt32(InventoryTechBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all InventoryTech objects 
        /// </summary>
        /// <returns>A list of InventoryTech objects</returns>
		public InventoryTechList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINVENTORYTECH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all InventoryTech objects by PageRequest
        /// </summary>
        /// <returns>A list of InventoryTech objects</returns>
		public InventoryTechList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINVENTORYTECH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				InventoryTechList _InventoryTechList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _InventoryTechList;
			}
		}
		
		/// <summary>
        /// Retrieves all InventoryTech objects by query String
        /// </summary>
        /// <returns>A list of InventoryTech objects</returns>
		public InventoryTechList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYTECHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get InventoryTech Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of InventoryTech
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYTECHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get InventoryTech Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of InventoryTech
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _InventoryTechRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINVENTORYTECHROWCOUNT))
			{
				SqlDataReader reader;
				_InventoryTechRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _InventoryTechRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills InventoryTech object
        /// </summary>
        /// <param name="inventoryTechObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(InventoryTechBase inventoryTechObject, SqlDataReader reader, int start)
		{
			
				inventoryTechObject.Id = reader.GetInt32( start + 0 );			
				inventoryTechObject.CompanyId = reader.GetGuid( start + 1 );			
				inventoryTechObject.TechnicianId = reader.GetGuid( start + 2 );			
				inventoryTechObject.EquipmentId = reader.GetGuid( start + 3 );			
				inventoryTechObject.Type = reader.GetString( start + 4 );			
				inventoryTechObject.Quantity = reader.GetInt32( start + 5 );			
				inventoryTechObject.PurchaseOrderId = reader.GetString( start + 6 );			
				inventoryTechObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				inventoryTechObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) inventoryTechObject.Description = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) inventoryTechObject.CustomerAppointmentEquipmentId = reader.GetInt32( start + 10 );			
			FillBaseObject(inventoryTechObject, reader, (start + 11));

			
			inventoryTechObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills InventoryTech object
        /// </summary>
        /// <param name="inventoryTechObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(InventoryTechBase inventoryTechObject, SqlDataReader reader)
		{
			FillObject(inventoryTechObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves InventoryTech object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>InventoryTech object</returns>
		private InventoryTech GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					InventoryTech inventoryTechObject= new InventoryTech();
					FillObject(inventoryTechObject, reader);
					return inventoryTechObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of InventoryTech objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of InventoryTech objects</returns>
		private InventoryTechList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//InventoryTech list
			InventoryTechList list = new InventoryTechList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					InventoryTech inventoryTechObject = new InventoryTech();
					FillObject(inventoryTechObject, reader);

					list.Add(inventoryTechObject);
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
