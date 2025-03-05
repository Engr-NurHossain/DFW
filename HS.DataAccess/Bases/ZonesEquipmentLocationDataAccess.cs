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
	public partial class ZonesEquipmentLocationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTZONESEQUIPMENTLOCATION = "InsertZonesEquipmentLocation";
		private const string UPDATEZONESEQUIPMENTLOCATION = "UpdateZonesEquipmentLocation";
		private const string DELETEZONESEQUIPMENTLOCATION = "DeleteZonesEquipmentLocation";
		private const string GETZONESEQUIPMENTLOCATIONBYID = "GetZonesEquipmentLocationByID";
		private const string GETALLZONESEQUIPMENTLOCATION = "GetAllZonesEquipmentLocation";
		private const string GETPAGEDZONESEQUIPMENTLOCATION = "GetPagedZonesEquipmentLocation";
		private const string GETZONESEQUIPMENTLOCATIONMAXIMUMID = "GetZonesEquipmentLocationMaximumID";
		private const string GETZONESEQUIPMENTLOCATIONROWCOUNT = "GetZonesEquipmentLocationRowCount";	
		private const string GETZONESEQUIPMENTLOCATIONBYQUERY = "GetZonesEquipmentLocationByQuery";
		#endregion
		
		#region Constructors
		public ZonesEquipmentLocationDataAccess(ClientContext context) : base(context) { }
		public ZonesEquipmentLocationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="zonesEquipmentLocationObject"></param>
		private void AddCommonParams(SqlCommand cmd, ZonesEquipmentLocationBase zonesEquipmentLocationObject)
		{	
			AddParameter(cmd, pNVarChar(ZonesEquipmentLocationBase.Property_EquipmentLocationId, 50, zonesEquipmentLocationObject.EquipmentLocationId));
			AddParameter(cmd, pNVarChar(ZonesEquipmentLocationBase.Property_EquipmentLocation, 100, zonesEquipmentLocationObject.EquipmentLocation));
			AddParameter(cmd, pNVarChar(ZonesEquipmentLocationBase.Property_Platform, 50, zonesEquipmentLocationObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ZonesEquipmentLocation
        /// </summary>
        /// <param name="zonesEquipmentLocationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ZonesEquipmentLocationBase zonesEquipmentLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTZONESEQUIPMENTLOCATION);
	
				AddParameter(cmd, pInt32Out(ZonesEquipmentLocationBase.Property_ID));
				AddCommonParams(cmd, zonesEquipmentLocationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					zonesEquipmentLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					zonesEquipmentLocationObject.ID = (Int32)GetOutParameter(cmd, ZonesEquipmentLocationBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(zonesEquipmentLocationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ZonesEquipmentLocation
        /// </summary>
        /// <param name="zonesEquipmentLocationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ZonesEquipmentLocationBase zonesEquipmentLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEZONESEQUIPMENTLOCATION);
				
				AddParameter(cmd, pInt32(ZonesEquipmentLocationBase.Property_ID, zonesEquipmentLocationObject.ID));
				AddCommonParams(cmd, zonesEquipmentLocationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					zonesEquipmentLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(zonesEquipmentLocationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ZonesEquipmentLocation
        /// </summary>
        /// <param name="ID">ID of the ZonesEquipmentLocation object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEZONESEQUIPMENTLOCATION);	
				
				AddParameter(cmd, pInt32(ZonesEquipmentLocationBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ZonesEquipmentLocation), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves ZonesEquipmentLocation object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the ZonesEquipmentLocation object to retrieve</param>
        /// <returns>ZonesEquipmentLocation object, null if not found</returns>
		public ZonesEquipmentLocation Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTLOCATIONBYID))
			{
				AddParameter( cmd, pInt32(ZonesEquipmentLocationBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ZonesEquipmentLocation objects 
        /// </summary>
        /// <returns>A list of ZonesEquipmentLocation objects</returns>
		public ZonesEquipmentLocationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLZONESEQUIPMENTLOCATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ZonesEquipmentLocation objects by PageRequest
        /// </summary>
        /// <returns>A list of ZonesEquipmentLocation objects</returns>
		public ZonesEquipmentLocationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDZONESEQUIPMENTLOCATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ZonesEquipmentLocationList _ZonesEquipmentLocationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ZonesEquipmentLocationList;
			}
		}
		
		/// <summary>
        /// Retrieves all ZonesEquipmentLocation objects by query String
        /// </summary>
        /// <returns>A list of ZonesEquipmentLocation objects</returns>
		public ZonesEquipmentLocationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTLOCATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ZonesEquipmentLocation Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of ZonesEquipmentLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTLOCATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get ZonesEquipmentLocation Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ZonesEquipmentLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ZonesEquipmentLocationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETZONESEQUIPMENTLOCATIONROWCOUNT))
			{
				SqlDataReader reader;
				_ZonesEquipmentLocationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ZonesEquipmentLocationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ZonesEquipmentLocation object
        /// </summary>
        /// <param name="zonesEquipmentLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ZonesEquipmentLocationBase zonesEquipmentLocationObject, SqlDataReader reader, int start)
		{
			
				zonesEquipmentLocationObject.ID = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) zonesEquipmentLocationObject.EquipmentLocationId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) zonesEquipmentLocationObject.EquipmentLocation = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) zonesEquipmentLocationObject.Platform = reader.GetString( start + 3 );			
			FillBaseObject(zonesEquipmentLocationObject, reader, (start + 4));

			
			zonesEquipmentLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ZonesEquipmentLocation object
        /// </summary>
        /// <param name="zonesEquipmentLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ZonesEquipmentLocationBase zonesEquipmentLocationObject, SqlDataReader reader)
		{
			FillObject(zonesEquipmentLocationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ZonesEquipmentLocation object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ZonesEquipmentLocation object</returns>
		private ZonesEquipmentLocation GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ZonesEquipmentLocation zonesEquipmentLocationObject= new ZonesEquipmentLocation();
					FillObject(zonesEquipmentLocationObject, reader);
					return zonesEquipmentLocationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ZonesEquipmentLocation objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ZonesEquipmentLocation objects</returns>
		private ZonesEquipmentLocationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ZonesEquipmentLocation list
			ZonesEquipmentLocationList list = new ZonesEquipmentLocationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ZonesEquipmentLocation zonesEquipmentLocationObject = new ZonesEquipmentLocation();
					FillObject(zonesEquipmentLocationObject, reader);

					list.Add(zonesEquipmentLocationObject);
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
