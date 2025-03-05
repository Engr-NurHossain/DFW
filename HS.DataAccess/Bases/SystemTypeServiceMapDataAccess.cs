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
	public partial class SystemTypeServiceMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSYSTEMTYPESERVICEMAP = "InsertSystemTypeServiceMap";
		private const string UPDATESYSTEMTYPESERVICEMAP = "UpdateSystemTypeServiceMap";
		private const string DELETESYSTEMTYPESERVICEMAP = "DeleteSystemTypeServiceMap";
		private const string GETSYSTEMTYPESERVICEMAPBYID = "GetSystemTypeServiceMapById";
		private const string GETALLSYSTEMTYPESERVICEMAP = "GetAllSystemTypeServiceMap";
		private const string GETPAGEDSYSTEMTYPESERVICEMAP = "GetPagedSystemTypeServiceMap";
		private const string GETSYSTEMTYPESERVICEMAPMAXIMUMID = "GetSystemTypeServiceMapMaximumId";
		private const string GETSYSTEMTYPESERVICEMAPROWCOUNT = "GetSystemTypeServiceMapRowCount";	
		private const string GETSYSTEMTYPESERVICEMAPBYQUERY = "GetSystemTypeServiceMapByQuery";
		#endregion
		
		#region Constructors
		public SystemTypeServiceMapDataAccess(ClientContext context) : base(context) { }
		public SystemTypeServiceMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="systemTypeServiceMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, SystemTypeServiceMapBase systemTypeServiceMapObject)
		{	
			AddParameter(cmd, pInt32(SystemTypeServiceMapBase.Property_SystemTypeId, systemTypeServiceMapObject.SystemTypeId));
			AddParameter(cmd, pGuid(SystemTypeServiceMapBase.Property_EquipmentId, systemTypeServiceMapObject.EquipmentId));
			AddParameter(cmd, pGuid(SystemTypeServiceMapBase.Property_PackageId, systemTypeServiceMapObject.PackageId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SystemTypeServiceMap
        /// </summary>
        /// <param name="systemTypeServiceMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SystemTypeServiceMapBase systemTypeServiceMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSYSTEMTYPESERVICEMAP);
	
				AddParameter(cmd, pInt32Out(SystemTypeServiceMapBase.Property_Id));
				AddCommonParams(cmd, systemTypeServiceMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					systemTypeServiceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					systemTypeServiceMapObject.Id = (Int32)GetOutParameter(cmd, SystemTypeServiceMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(systemTypeServiceMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SystemTypeServiceMap
        /// </summary>
        /// <param name="systemTypeServiceMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SystemTypeServiceMapBase systemTypeServiceMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESYSTEMTYPESERVICEMAP);
				
				AddParameter(cmd, pInt32(SystemTypeServiceMapBase.Property_Id, systemTypeServiceMapObject.Id));
				AddCommonParams(cmd, systemTypeServiceMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					systemTypeServiceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(systemTypeServiceMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SystemTypeServiceMap
        /// </summary>
        /// <param name="Id">Id of the SystemTypeServiceMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESYSTEMTYPESERVICEMAP);	
				
				AddParameter(cmd, pInt32(SystemTypeServiceMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SystemTypeServiceMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SystemTypeServiceMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SystemTypeServiceMap object to retrieve</param>
        /// <returns>SystemTypeServiceMap object, null if not found</returns>
		public SystemTypeServiceMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPESERVICEMAPBYID))
			{
				AddParameter( cmd, pInt32(SystemTypeServiceMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SystemTypeServiceMap objects 
        /// </summary>
        /// <returns>A list of SystemTypeServiceMap objects</returns>
		public SystemTypeServiceMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSYSTEMTYPESERVICEMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SystemTypeServiceMap objects by PageRequest
        /// </summary>
        /// <returns>A list of SystemTypeServiceMap objects</returns>
		public SystemTypeServiceMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSYSTEMTYPESERVICEMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SystemTypeServiceMapList _SystemTypeServiceMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SystemTypeServiceMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all SystemTypeServiceMap objects by query String
        /// </summary>
        /// <returns>A list of SystemTypeServiceMap objects</returns>
		public SystemTypeServiceMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPESERVICEMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SystemTypeServiceMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SystemTypeServiceMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPESERVICEMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SystemTypeServiceMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SystemTypeServiceMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SystemTypeServiceMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPESERVICEMAPROWCOUNT))
			{
				SqlDataReader reader;
				_SystemTypeServiceMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SystemTypeServiceMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SystemTypeServiceMap object
        /// </summary>
        /// <param name="systemTypeServiceMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SystemTypeServiceMapBase systemTypeServiceMapObject, SqlDataReader reader, int start)
		{
			
				systemTypeServiceMapObject.Id = reader.GetInt32( start + 0 );			
				systemTypeServiceMapObject.SystemTypeId = reader.GetInt32( start + 1 );			
				systemTypeServiceMapObject.EquipmentId = reader.GetGuid( start + 2 );			
				systemTypeServiceMapObject.PackageId = reader.GetGuid( start + 3 );			
			FillBaseObject(systemTypeServiceMapObject, reader, (start + 4));

			
			systemTypeServiceMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SystemTypeServiceMap object
        /// </summary>
        /// <param name="systemTypeServiceMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SystemTypeServiceMapBase systemTypeServiceMapObject, SqlDataReader reader)
		{
			FillObject(systemTypeServiceMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SystemTypeServiceMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SystemTypeServiceMap object</returns>
		private SystemTypeServiceMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SystemTypeServiceMap systemTypeServiceMapObject= new SystemTypeServiceMap();
					FillObject(systemTypeServiceMapObject, reader);
					return systemTypeServiceMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SystemTypeServiceMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SystemTypeServiceMap objects</returns>
		private SystemTypeServiceMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SystemTypeServiceMap list
			SystemTypeServiceMapList list = new SystemTypeServiceMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SystemTypeServiceMap systemTypeServiceMapObject = new SystemTypeServiceMap();
					FillObject(systemTypeServiceMapObject, reader);

					list.Add(systemTypeServiceMapObject);
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
