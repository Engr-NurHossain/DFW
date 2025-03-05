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
	public partial class TechnicianInventoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTECHNICIANINVENTORY = "InsertTechnicianInventory";
		private const string UPDATETECHNICIANINVENTORY = "UpdateTechnicianInventory";
		private const string DELETETECHNICIANINVENTORY = "DeleteTechnicianInventory";
		private const string GETTECHNICIANINVENTORYBYID = "GetTechnicianInventoryById";
		private const string GETALLTECHNICIANINVENTORY = "GetAllTechnicianInventory";
		private const string GETPAGEDTECHNICIANINVENTORY = "GetPagedTechnicianInventory";
		private const string GETTECHNICIANINVENTORYMAXIMUMID = "GetTechnicianInventoryMaximumId";
		private const string GETTECHNICIANINVENTORYROWCOUNT = "GetTechnicianInventoryRowCount";	
		private const string GETTECHNICIANINVENTORYBYQUERY = "GetTechnicianInventoryByQuery";
		#endregion
		
		#region Constructors
		public TechnicianInventoryDataAccess(ClientContext context) : base(context) { }
		public TechnicianInventoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="technicianInventoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, TechnicianInventoryBase technicianInventoryObject)
		{	
			AddParameter(cmd, pGuid(TechnicianInventoryBase.Property_CompanyId, technicianInventoryObject.CompanyId));
			AddParameter(cmd, pGuid(TechnicianInventoryBase.Property_TechnicianId, technicianInventoryObject.TechnicianId));
			AddParameter(cmd, pGuid(TechnicianInventoryBase.Property_EquipmentId, technicianInventoryObject.EquipmentId));
			AddParameter(cmd, pInt32(TechnicianInventoryBase.Property_Quantity, technicianInventoryObject.Quantity));
			AddParameter(cmd, pNVarChar(TechnicianInventoryBase.Property_LastUpdatedBy, 50, technicianInventoryObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(TechnicianInventoryBase.Property_LastUpdatedDate, technicianInventoryObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TechnicianInventory
        /// </summary>
        /// <param name="technicianInventoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TechnicianInventoryBase technicianInventoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTECHNICIANINVENTORY);
	
				AddParameter(cmd, pInt32Out(TechnicianInventoryBase.Property_Id));
				AddCommonParams(cmd, technicianInventoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					technicianInventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					technicianInventoryObject.Id = (Int32)GetOutParameter(cmd, TechnicianInventoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(technicianInventoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TechnicianInventory
        /// </summary>
        /// <param name="technicianInventoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TechnicianInventoryBase technicianInventoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETECHNICIANINVENTORY);
				
				AddParameter(cmd, pInt32(TechnicianInventoryBase.Property_Id, technicianInventoryObject.Id));
				AddCommonParams(cmd, technicianInventoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					technicianInventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(technicianInventoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TechnicianInventory
        /// </summary>
        /// <param name="Id">Id of the TechnicianInventory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETECHNICIANINVENTORY);	
				
				AddParameter(cmd, pInt32(TechnicianInventoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TechnicianInventory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TechnicianInventory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TechnicianInventory object to retrieve</param>
        /// <returns>TechnicianInventory object, null if not found</returns>
		public TechnicianInventory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHNICIANINVENTORYBYID))
			{
				AddParameter( cmd, pInt32(TechnicianInventoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TechnicianInventory objects 
        /// </summary>
        /// <returns>A list of TechnicianInventory objects</returns>
		public TechnicianInventoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTECHNICIANINVENTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TechnicianInventory objects by PageRequest
        /// </summary>
        /// <returns>A list of TechnicianInventory objects</returns>
		public TechnicianInventoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTECHNICIANINVENTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TechnicianInventoryList _TechnicianInventoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TechnicianInventoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all TechnicianInventory objects by query String
        /// </summary>
        /// <returns>A list of TechnicianInventory objects</returns>
		public TechnicianInventoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHNICIANINVENTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TechnicianInventory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TechnicianInventory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHNICIANINVENTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TechnicianInventory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TechnicianInventory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TechnicianInventoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHNICIANINVENTORYROWCOUNT))
			{
				SqlDataReader reader;
				_TechnicianInventoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TechnicianInventoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TechnicianInventory object
        /// </summary>
        /// <param name="technicianInventoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TechnicianInventoryBase technicianInventoryObject, SqlDataReader reader, int start)
		{
			
				technicianInventoryObject.Id = reader.GetInt32( start + 0 );			
				technicianInventoryObject.CompanyId = reader.GetGuid( start + 1 );			
				technicianInventoryObject.TechnicianId = reader.GetGuid( start + 2 );			
				technicianInventoryObject.EquipmentId = reader.GetGuid( start + 3 );			
				technicianInventoryObject.Quantity = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) technicianInventoryObject.LastUpdatedBy = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) technicianInventoryObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(technicianInventoryObject, reader, (start + 7));

			
			technicianInventoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TechnicianInventory object
        /// </summary>
        /// <param name="technicianInventoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TechnicianInventoryBase technicianInventoryObject, SqlDataReader reader)
		{
			FillObject(technicianInventoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TechnicianInventory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TechnicianInventory object</returns>
		private TechnicianInventory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TechnicianInventory technicianInventoryObject= new TechnicianInventory();
					FillObject(technicianInventoryObject, reader);
					return technicianInventoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TechnicianInventory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TechnicianInventory objects</returns>
		private TechnicianInventoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TechnicianInventory list
			TechnicianInventoryList list = new TechnicianInventoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TechnicianInventory technicianInventoryObject = new TechnicianInventory();
					FillObject(technicianInventoryObject, reader);

					list.Add(technicianInventoryObject);
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
