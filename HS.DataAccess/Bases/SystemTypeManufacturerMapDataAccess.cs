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
	public partial class SystemTypeManufacturerMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSYSTEMTYPEMANUFACTURERMAP = "InsertSystemTypeManufacturerMap";
		private const string UPDATESYSTEMTYPEMANUFACTURERMAP = "UpdateSystemTypeManufacturerMap";
		private const string DELETESYSTEMTYPEMANUFACTURERMAP = "DeleteSystemTypeManufacturerMap";
		private const string GETSYSTEMTYPEMANUFACTURERMAPBYID = "GetSystemTypeManufacturerMapById";
		private const string GETALLSYSTEMTYPEMANUFACTURERMAP = "GetAllSystemTypeManufacturerMap";
		private const string GETPAGEDSYSTEMTYPEMANUFACTURERMAP = "GetPagedSystemTypeManufacturerMap";
		private const string GETSYSTEMTYPEMANUFACTURERMAPMAXIMUMID = "GetSystemTypeManufacturerMapMaximumId";
		private const string GETSYSTEMTYPEMANUFACTURERMAPROWCOUNT = "GetSystemTypeManufacturerMapRowCount";	
		private const string GETSYSTEMTYPEMANUFACTURERMAPBYQUERY = "GetSystemTypeManufacturerMapByQuery";
		#endregion
		
		#region Constructors
		public SystemTypeManufacturerMapDataAccess(ClientContext context) : base(context) { }
		public SystemTypeManufacturerMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="systemTypeManufacturerMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, SystemTypeManufacturerMapBase systemTypeManufacturerMapObject)
		{	
			AddParameter(cmd, pInt32(SystemTypeManufacturerMapBase.Property_SystemId, systemTypeManufacturerMapObject.SystemId));
			AddParameter(cmd, pGuid(SystemTypeManufacturerMapBase.Property_ManufacturerId, systemTypeManufacturerMapObject.ManufacturerId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SystemTypeManufacturerMap
        /// </summary>
        /// <param name="systemTypeManufacturerMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SystemTypeManufacturerMapBase systemTypeManufacturerMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSYSTEMTYPEMANUFACTURERMAP);
	
				AddParameter(cmd, pInt32Out(SystemTypeManufacturerMapBase.Property_Id));
				AddCommonParams(cmd, systemTypeManufacturerMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					systemTypeManufacturerMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					systemTypeManufacturerMapObject.Id = (Int32)GetOutParameter(cmd, SystemTypeManufacturerMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(systemTypeManufacturerMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SystemTypeManufacturerMap
        /// </summary>
        /// <param name="systemTypeManufacturerMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SystemTypeManufacturerMapBase systemTypeManufacturerMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESYSTEMTYPEMANUFACTURERMAP);
				
				AddParameter(cmd, pInt32(SystemTypeManufacturerMapBase.Property_Id, systemTypeManufacturerMapObject.Id));
				AddCommonParams(cmd, systemTypeManufacturerMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					systemTypeManufacturerMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(systemTypeManufacturerMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SystemTypeManufacturerMap
        /// </summary>
        /// <param name="Id">Id of the SystemTypeManufacturerMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESYSTEMTYPEMANUFACTURERMAP);	
				
				AddParameter(cmd, pInt32(SystemTypeManufacturerMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SystemTypeManufacturerMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SystemTypeManufacturerMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SystemTypeManufacturerMap object to retrieve</param>
        /// <returns>SystemTypeManufacturerMap object, null if not found</returns>
		public SystemTypeManufacturerMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPEMANUFACTURERMAPBYID))
			{
				AddParameter( cmd, pInt32(SystemTypeManufacturerMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SystemTypeManufacturerMap objects 
        /// </summary>
        /// <returns>A list of SystemTypeManufacturerMap objects</returns>
		public SystemTypeManufacturerMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSYSTEMTYPEMANUFACTURERMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SystemTypeManufacturerMap objects by PageRequest
        /// </summary>
        /// <returns>A list of SystemTypeManufacturerMap objects</returns>
		public SystemTypeManufacturerMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSYSTEMTYPEMANUFACTURERMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SystemTypeManufacturerMapList _SystemTypeManufacturerMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SystemTypeManufacturerMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all SystemTypeManufacturerMap objects by query String
        /// </summary>
        /// <returns>A list of SystemTypeManufacturerMap objects</returns>
		public SystemTypeManufacturerMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPEMANUFACTURERMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SystemTypeManufacturerMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SystemTypeManufacturerMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPEMANUFACTURERMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SystemTypeManufacturerMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SystemTypeManufacturerMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SystemTypeManufacturerMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSYSTEMTYPEMANUFACTURERMAPROWCOUNT))
			{
				SqlDataReader reader;
				_SystemTypeManufacturerMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SystemTypeManufacturerMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SystemTypeManufacturerMap object
        /// </summary>
        /// <param name="systemTypeManufacturerMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SystemTypeManufacturerMapBase systemTypeManufacturerMapObject, SqlDataReader reader, int start)
		{
			
				systemTypeManufacturerMapObject.Id = reader.GetInt32( start + 0 );			
				systemTypeManufacturerMapObject.SystemId = reader.GetInt32( start + 1 );			
				systemTypeManufacturerMapObject.ManufacturerId = reader.GetGuid( start + 2 );			
			FillBaseObject(systemTypeManufacturerMapObject, reader, (start + 3));

			
			systemTypeManufacturerMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SystemTypeManufacturerMap object
        /// </summary>
        /// <param name="systemTypeManufacturerMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SystemTypeManufacturerMapBase systemTypeManufacturerMapObject, SqlDataReader reader)
		{
			FillObject(systemTypeManufacturerMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SystemTypeManufacturerMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SystemTypeManufacturerMap object</returns>
		private SystemTypeManufacturerMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SystemTypeManufacturerMap systemTypeManufacturerMapObject= new SystemTypeManufacturerMap();
					FillObject(systemTypeManufacturerMapObject, reader);
					return systemTypeManufacturerMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SystemTypeManufacturerMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SystemTypeManufacturerMap objects</returns>
		private SystemTypeManufacturerMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SystemTypeManufacturerMap list
			SystemTypeManufacturerMapList list = new SystemTypeManufacturerMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SystemTypeManufacturerMap systemTypeManufacturerMapObject = new SystemTypeManufacturerMap();
					FillObject(systemTypeManufacturerMapObject, reader);

					list.Add(systemTypeManufacturerMapObject);
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
