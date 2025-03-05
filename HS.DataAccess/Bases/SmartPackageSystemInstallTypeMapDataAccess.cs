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
	public partial class SmartPackageSystemInstallTypeMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTPACKAGESYSTEMINSTALLTYPEMAP = "InsertSmartPackageSystemInstallTypeMap";
		private const string UPDATESMARTPACKAGESYSTEMINSTALLTYPEMAP = "UpdateSmartPackageSystemInstallTypeMap";
		private const string DELETESMARTPACKAGESYSTEMINSTALLTYPEMAP = "DeleteSmartPackageSystemInstallTypeMap";
		private const string GETSMARTPACKAGESYSTEMINSTALLTYPEMAPBYID = "GetSmartPackageSystemInstallTypeMapById";
		private const string GETALLSMARTPACKAGESYSTEMINSTALLTYPEMAP = "GetAllSmartPackageSystemInstallTypeMap";
		private const string GETPAGEDSMARTPACKAGESYSTEMINSTALLTYPEMAP = "GetPagedSmartPackageSystemInstallTypeMap";
		private const string GETSMARTPACKAGESYSTEMINSTALLTYPEMAPMAXIMUMID = "GetSmartPackageSystemInstallTypeMapMaximumId";
		private const string GETSMARTPACKAGESYSTEMINSTALLTYPEMAPROWCOUNT = "GetSmartPackageSystemInstallTypeMapRowCount";	
		private const string GETSMARTPACKAGESYSTEMINSTALLTYPEMAPBYQUERY = "GetSmartPackageSystemInstallTypeMapByQuery";
		#endregion
		
		#region Constructors
		public SmartPackageSystemInstallTypeMapDataAccess(ClientContext context) : base(context) { }
		public SmartPackageSystemInstallTypeMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartPackageSystemInstallTypeMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartPackageSystemInstallTypeMapBase smartPackageSystemInstallTypeMapObject)
		{	
			AddParameter(cmd, pGuid(SmartPackageSystemInstallTypeMapBase.Property_PackageId, smartPackageSystemInstallTypeMapObject.PackageId));
			AddParameter(cmd, pInt32(SmartPackageSystemInstallTypeMapBase.Property_SmartSystemTypeId, smartPackageSystemInstallTypeMapObject.SmartSystemTypeId));
			AddParameter(cmd, pInt32(SmartPackageSystemInstallTypeMapBase.Property_SmartInstallTypeId, smartPackageSystemInstallTypeMapObject.SmartInstallTypeId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartPackageSystemInstallTypeMap
        /// </summary>
        /// <param name="smartPackageSystemInstallTypeMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartPackageSystemInstallTypeMapBase smartPackageSystemInstallTypeMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTPACKAGESYSTEMINSTALLTYPEMAP);
	
				AddParameter(cmd, pInt32Out(SmartPackageSystemInstallTypeMapBase.Property_Id));
				AddCommonParams(cmd, smartPackageSystemInstallTypeMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartPackageSystemInstallTypeMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartPackageSystemInstallTypeMapObject.Id = (Int32)GetOutParameter(cmd, SmartPackageSystemInstallTypeMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartPackageSystemInstallTypeMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartPackageSystemInstallTypeMap
        /// </summary>
        /// <param name="smartPackageSystemInstallTypeMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartPackageSystemInstallTypeMapBase smartPackageSystemInstallTypeMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTPACKAGESYSTEMINSTALLTYPEMAP);
				
				AddParameter(cmd, pInt32(SmartPackageSystemInstallTypeMapBase.Property_Id, smartPackageSystemInstallTypeMapObject.Id));
				AddCommonParams(cmd, smartPackageSystemInstallTypeMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartPackageSystemInstallTypeMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartPackageSystemInstallTypeMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartPackageSystemInstallTypeMap
        /// </summary>
        /// <param name="Id">Id of the SmartPackageSystemInstallTypeMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTPACKAGESYSTEMINSTALLTYPEMAP);	
				
				AddParameter(cmd, pInt32(SmartPackageSystemInstallTypeMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartPackageSystemInstallTypeMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartPackageSystemInstallTypeMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartPackageSystemInstallTypeMap object to retrieve</param>
        /// <returns>SmartPackageSystemInstallTypeMap object, null if not found</returns>
		public SmartPackageSystemInstallTypeMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGESYSTEMINSTALLTYPEMAPBYID))
			{
				AddParameter( cmd, pInt32(SmartPackageSystemInstallTypeMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartPackageSystemInstallTypeMap objects 
        /// </summary>
        /// <returns>A list of SmartPackageSystemInstallTypeMap objects</returns>
		public SmartPackageSystemInstallTypeMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTPACKAGESYSTEMINSTALLTYPEMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartPackageSystemInstallTypeMap objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartPackageSystemInstallTypeMap objects</returns>
		public SmartPackageSystemInstallTypeMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTPACKAGESYSTEMINSTALLTYPEMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartPackageSystemInstallTypeMapList _SmartPackageSystemInstallTypeMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartPackageSystemInstallTypeMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartPackageSystemInstallTypeMap objects by query String
        /// </summary>
        /// <returns>A list of SmartPackageSystemInstallTypeMap objects</returns>
		public SmartPackageSystemInstallTypeMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGESYSTEMINSTALLTYPEMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartPackageSystemInstallTypeMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartPackageSystemInstallTypeMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGESYSTEMINSTALLTYPEMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartPackageSystemInstallTypeMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartPackageSystemInstallTypeMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartPackageSystemInstallTypeMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGESYSTEMINSTALLTYPEMAPROWCOUNT))
			{
				SqlDataReader reader;
				_SmartPackageSystemInstallTypeMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartPackageSystemInstallTypeMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartPackageSystemInstallTypeMap object
        /// </summary>
        /// <param name="smartPackageSystemInstallTypeMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartPackageSystemInstallTypeMapBase smartPackageSystemInstallTypeMapObject, SqlDataReader reader, int start)
		{
			
				smartPackageSystemInstallTypeMapObject.Id = reader.GetInt32( start + 0 );			
				smartPackageSystemInstallTypeMapObject.PackageId = reader.GetGuid( start + 1 );			
				smartPackageSystemInstallTypeMapObject.SmartSystemTypeId = reader.GetInt32( start + 2 );			
				smartPackageSystemInstallTypeMapObject.SmartInstallTypeId = reader.GetInt32( start + 3 );			
			FillBaseObject(smartPackageSystemInstallTypeMapObject, reader, (start + 4));

			
			smartPackageSystemInstallTypeMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartPackageSystemInstallTypeMap object
        /// </summary>
        /// <param name="smartPackageSystemInstallTypeMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartPackageSystemInstallTypeMapBase smartPackageSystemInstallTypeMapObject, SqlDataReader reader)
		{
			FillObject(smartPackageSystemInstallTypeMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartPackageSystemInstallTypeMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartPackageSystemInstallTypeMap object</returns>
		private SmartPackageSystemInstallTypeMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartPackageSystemInstallTypeMap smartPackageSystemInstallTypeMapObject= new SmartPackageSystemInstallTypeMap();
					FillObject(smartPackageSystemInstallTypeMapObject, reader);
					return smartPackageSystemInstallTypeMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartPackageSystemInstallTypeMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartPackageSystemInstallTypeMap objects</returns>
		private SmartPackageSystemInstallTypeMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartPackageSystemInstallTypeMap list
			SmartPackageSystemInstallTypeMapList list = new SmartPackageSystemInstallTypeMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartPackageSystemInstallTypeMap smartPackageSystemInstallTypeMapObject = new SmartPackageSystemInstallTypeMap();
					FillObject(smartPackageSystemInstallTypeMapObject, reader);

					list.Add(smartPackageSystemInstallTypeMapObject);
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
