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
	public partial class ZonesEquipmentTypeEventMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTZONESEQUIPMENTTYPEEVENTMAP = "InsertZonesEquipmentTypeEventMap";
		private const string UPDATEZONESEQUIPMENTTYPEEVENTMAP = "UpdateZonesEquipmentTypeEventMap";
		private const string DELETEZONESEQUIPMENTTYPEEVENTMAP = "DeleteZonesEquipmentTypeEventMap";
		private const string GETZONESEQUIPMENTTYPEEVENTMAPBYID = "GetZonesEquipmentTypeEventMapByID";
		private const string GETALLZONESEQUIPMENTTYPEEVENTMAP = "GetAllZonesEquipmentTypeEventMap";
		private const string GETPAGEDZONESEQUIPMENTTYPEEVENTMAP = "GetPagedZonesEquipmentTypeEventMap";
		private const string GETZONESEQUIPMENTTYPEEVENTMAPMAXIMUMID = "GetZonesEquipmentTypeEventMapMaximumID";
		private const string GETZONESEQUIPMENTTYPEEVENTMAPROWCOUNT = "GetZonesEquipmentTypeEventMapRowCount";	
		private const string GETZONESEQUIPMENTTYPEEVENTMAPBYQUERY = "GetZonesEquipmentTypeEventMapByQuery";
		#endregion
		
		#region Constructors
		public ZonesEquipmentTypeEventMapDataAccess(ClientContext context) : base(context) { }
		public ZonesEquipmentTypeEventMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="zonesEquipmentTypeEventMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, ZonesEquipmentTypeEventMapBase zonesEquipmentTypeEventMapObject)
		{	
			AddParameter(cmd, pNVarChar(ZonesEquipmentTypeEventMapBase.Property_EquipmentTypeId, 50, zonesEquipmentTypeEventMapObject.EquipmentTypeId));
			AddParameter(cmd, pNVarChar(ZonesEquipmentTypeEventMapBase.Property_EventId, 50, zonesEquipmentTypeEventMapObject.EventId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ZonesEquipmentTypeEventMap
        /// </summary>
        /// <param name="zonesEquipmentTypeEventMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ZonesEquipmentTypeEventMapBase zonesEquipmentTypeEventMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTZONESEQUIPMENTTYPEEVENTMAP);
	
				AddParameter(cmd, pInt32Out(ZonesEquipmentTypeEventMapBase.Property_ID));
				AddCommonParams(cmd, zonesEquipmentTypeEventMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					zonesEquipmentTypeEventMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					zonesEquipmentTypeEventMapObject.ID = (Int32)GetOutParameter(cmd, ZonesEquipmentTypeEventMapBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(zonesEquipmentTypeEventMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ZonesEquipmentTypeEventMap
        /// </summary>
        /// <param name="zonesEquipmentTypeEventMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ZonesEquipmentTypeEventMapBase zonesEquipmentTypeEventMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEZONESEQUIPMENTTYPEEVENTMAP);
				
				AddParameter(cmd, pInt32(ZonesEquipmentTypeEventMapBase.Property_ID, zonesEquipmentTypeEventMapObject.ID));
				AddCommonParams(cmd, zonesEquipmentTypeEventMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					zonesEquipmentTypeEventMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(zonesEquipmentTypeEventMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ZonesEquipmentTypeEventMap
        /// </summary>
        /// <param name="ID">ID of the ZonesEquipmentTypeEventMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEZONESEQUIPMENTTYPEEVENTMAP);	
				
				AddParameter(cmd, pInt32(ZonesEquipmentTypeEventMapBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ZonesEquipmentTypeEventMap), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves ZonesEquipmentTypeEventMap object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the ZonesEquipmentTypeEventMap object to retrieve</param>
        /// <returns>ZonesEquipmentTypeEventMap object, null if not found</returns>
		public ZonesEquipmentTypeEventMap Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEEVENTMAPBYID))
			{
				AddParameter( cmd, pInt32(ZonesEquipmentTypeEventMapBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ZonesEquipmentTypeEventMap objects 
        /// </summary>
        /// <returns>A list of ZonesEquipmentTypeEventMap objects</returns>
		public ZonesEquipmentTypeEventMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLZONESEQUIPMENTTYPEEVENTMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ZonesEquipmentTypeEventMap objects by PageRequest
        /// </summary>
        /// <returns>A list of ZonesEquipmentTypeEventMap objects</returns>
		public ZonesEquipmentTypeEventMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDZONESEQUIPMENTTYPEEVENTMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ZonesEquipmentTypeEventMapList _ZonesEquipmentTypeEventMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ZonesEquipmentTypeEventMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all ZonesEquipmentTypeEventMap objects by query String
        /// </summary>
        /// <returns>A list of ZonesEquipmentTypeEventMap objects</returns>
		public ZonesEquipmentTypeEventMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEEVENTMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ZonesEquipmentTypeEventMap Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of ZonesEquipmentTypeEventMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEEVENTMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get ZonesEquipmentTypeEventMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ZonesEquipmentTypeEventMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ZonesEquipmentTypeEventMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTTYPEEVENTMAPROWCOUNT))
			{
				SqlDataReader reader;
				_ZonesEquipmentTypeEventMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ZonesEquipmentTypeEventMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ZonesEquipmentTypeEventMap object
        /// </summary>
        /// <param name="zonesEquipmentTypeEventMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ZonesEquipmentTypeEventMapBase zonesEquipmentTypeEventMapObject, SqlDataReader reader, int start)
		{
			
				zonesEquipmentTypeEventMapObject.ID = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) zonesEquipmentTypeEventMapObject.EquipmentTypeId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) zonesEquipmentTypeEventMapObject.EventId = reader.GetString( start + 2 );			
			FillBaseObject(zonesEquipmentTypeEventMapObject, reader, (start + 3));

			
			zonesEquipmentTypeEventMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ZonesEquipmentTypeEventMap object
        /// </summary>
        /// <param name="zonesEquipmentTypeEventMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ZonesEquipmentTypeEventMapBase zonesEquipmentTypeEventMapObject, SqlDataReader reader)
		{
			FillObject(zonesEquipmentTypeEventMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ZonesEquipmentTypeEventMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ZonesEquipmentTypeEventMap object</returns>
		private ZonesEquipmentTypeEventMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ZonesEquipmentTypeEventMap zonesEquipmentTypeEventMapObject= new ZonesEquipmentTypeEventMap();
					FillObject(zonesEquipmentTypeEventMapObject, reader);
					return zonesEquipmentTypeEventMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ZonesEquipmentTypeEventMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ZonesEquipmentTypeEventMap objects</returns>
		private ZonesEquipmentTypeEventMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ZonesEquipmentTypeEventMap list
			ZonesEquipmentTypeEventMapList list = new ZonesEquipmentTypeEventMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ZonesEquipmentTypeEventMap zonesEquipmentTypeEventMapObject = new ZonesEquipmentTypeEventMap();
					FillObject(zonesEquipmentTypeEventMapObject, reader);

					list.Add(zonesEquipmentTypeEventMapObject);
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
