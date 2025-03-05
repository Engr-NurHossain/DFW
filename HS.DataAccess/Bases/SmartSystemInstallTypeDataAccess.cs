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
	public partial class SmartSystemInstallTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTSYSTEMINSTALLTYPE = "InsertSmartSystemInstallType";
		private const string UPDATESMARTSYSTEMINSTALLTYPE = "UpdateSmartSystemInstallType";
		private const string DELETESMARTSYSTEMINSTALLTYPE = "DeleteSmartSystemInstallType";
		private const string GETSMARTSYSTEMINSTALLTYPEBYID = "GetSmartSystemInstallTypeById";
		private const string GETALLSMARTSYSTEMINSTALLTYPE = "GetAllSmartSystemInstallType";
		private const string GETPAGEDSMARTSYSTEMINSTALLTYPE = "GetPagedSmartSystemInstallType";
		private const string GETSMARTSYSTEMINSTALLTYPEMAXIMUMID = "GetSmartSystemInstallTypeMaximumId";
		private const string GETSMARTSYSTEMINSTALLTYPEROWCOUNT = "GetSmartSystemInstallTypeRowCount";	
		private const string GETSMARTSYSTEMINSTALLTYPEBYQUERY = "GetSmartSystemInstallTypeByQuery";
		#endregion
		
		#region Constructors
		public SmartSystemInstallTypeDataAccess(ClientContext context) : base(context) { }
		public SmartSystemInstallTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartSystemInstallTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartSystemInstallTypeBase smartSystemInstallTypeObject)
		{	
			AddParameter(cmd, pGuid(SmartSystemInstallTypeBase.Property_CompanyId, smartSystemInstallTypeObject.CompanyId));
			AddParameter(cmd, pInt32(SmartSystemInstallTypeBase.Property_SystemId, smartSystemInstallTypeObject.SystemId));
			AddParameter(cmd, pInt32(SmartSystemInstallTypeBase.Property_InstallTypeId, smartSystemInstallTypeObject.InstallTypeId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartSystemInstallType
        /// </summary>
        /// <param name="smartSystemInstallTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartSystemInstallTypeBase smartSystemInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTSYSTEMINSTALLTYPE);
	
				AddParameter(cmd, pInt32Out(SmartSystemInstallTypeBase.Property_Id));
				AddCommonParams(cmd, smartSystemInstallTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartSystemInstallTypeObject.Id = (Int32)GetOutParameter(cmd, SmartSystemInstallTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartSystemInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartSystemInstallType
        /// </summary>
        /// <param name="smartSystemInstallTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartSystemInstallTypeBase smartSystemInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTSYSTEMINSTALLTYPE);
				
				AddParameter(cmd, pInt32(SmartSystemInstallTypeBase.Property_Id, smartSystemInstallTypeObject.Id));
				AddCommonParams(cmd, smartSystemInstallTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartSystemInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartSystemInstallType
        /// </summary>
        /// <param name="Id">Id of the SmartSystemInstallType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTSYSTEMINSTALLTYPE);	
				
				AddParameter(cmd, pInt32(SmartSystemInstallTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartSystemInstallType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartSystemInstallType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartSystemInstallType object to retrieve</param>
        /// <returns>SmartSystemInstallType object, null if not found</returns>
		public SmartSystemInstallType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMINSTALLTYPEBYID))
			{
				AddParameter( cmd, pInt32(SmartSystemInstallTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartSystemInstallType objects 
        /// </summary>
        /// <returns>A list of SmartSystemInstallType objects</returns>
		public SmartSystemInstallTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTSYSTEMINSTALLTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartSystemInstallType objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartSystemInstallType objects</returns>
		public SmartSystemInstallTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTSYSTEMINSTALLTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartSystemInstallTypeList _SmartSystemInstallTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartSystemInstallTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartSystemInstallType objects by query String
        /// </summary>
        /// <returns>A list of SmartSystemInstallType objects</returns>
		public SmartSystemInstallTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMINSTALLTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartSystemInstallType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartSystemInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMINSTALLTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartSystemInstallType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartSystemInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartSystemInstallTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMINSTALLTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_SmartSystemInstallTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartSystemInstallTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartSystemInstallType object
        /// </summary>
        /// <param name="smartSystemInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartSystemInstallTypeBase smartSystemInstallTypeObject, SqlDataReader reader, int start)
		{
			
				smartSystemInstallTypeObject.Id = reader.GetInt32( start + 0 );			
				smartSystemInstallTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				smartSystemInstallTypeObject.SystemId = reader.GetInt32( start + 2 );			
				smartSystemInstallTypeObject.InstallTypeId = reader.GetInt32( start + 3 );			
			FillBaseObject(smartSystemInstallTypeObject, reader, (start + 4));

			
			smartSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartSystemInstallType object
        /// </summary>
        /// <param name="smartSystemInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartSystemInstallTypeBase smartSystemInstallTypeObject, SqlDataReader reader)
		{
			FillObject(smartSystemInstallTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartSystemInstallType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartSystemInstallType object</returns>
		private SmartSystemInstallType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartSystemInstallType smartSystemInstallTypeObject= new SmartSystemInstallType();
					FillObject(smartSystemInstallTypeObject, reader);
					return smartSystemInstallTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartSystemInstallType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartSystemInstallType objects</returns>
		private SmartSystemInstallTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartSystemInstallType list
			SmartSystemInstallTypeList list = new SmartSystemInstallTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartSystemInstallType smartSystemInstallTypeObject = new SmartSystemInstallType();
					FillObject(smartSystemInstallTypeObject, reader);

					list.Add(smartSystemInstallTypeObject);
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
