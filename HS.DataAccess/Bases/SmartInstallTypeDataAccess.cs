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
	public partial class SmartInstallTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTINSTALLTYPE = "InsertSmartInstallType";
		private const string UPDATESMARTINSTALLTYPE = "UpdateSmartInstallType";
		private const string DELETESMARTINSTALLTYPE = "DeleteSmartInstallType";
		private const string GETSMARTINSTALLTYPEBYID = "GetSmartInstallTypeById";
		private const string GETALLSMARTINSTALLTYPE = "GetAllSmartInstallType";
		private const string GETPAGEDSMARTINSTALLTYPE = "GetPagedSmartInstallType";
		private const string GETSMARTINSTALLTYPEMAXIMUMID = "GetSmartInstallTypeMaximumId";
		private const string GETSMARTINSTALLTYPEROWCOUNT = "GetSmartInstallTypeRowCount";	
		private const string GETSMARTINSTALLTYPEBYQUERY = "GetSmartInstallTypeByQuery";
		#endregion
		
		#region Constructors
		public SmartInstallTypeDataAccess(ClientContext context) : base(context) { }
		public SmartInstallTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartInstallTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartInstallTypeBase smartInstallTypeObject)
		{	
			AddParameter(cmd, pGuid(SmartInstallTypeBase.Property_CompanyId, smartInstallTypeObject.CompanyId));
			AddParameter(cmd, pNVarChar(SmartInstallTypeBase.Property_Name, 50, smartInstallTypeObject.Name));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartInstallType
        /// </summary>
        /// <param name="smartInstallTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartInstallTypeBase smartInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTINSTALLTYPE);
	
				AddParameter(cmd, pInt32Out(SmartInstallTypeBase.Property_Id));
				AddCommonParams(cmd, smartInstallTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartInstallTypeObject.Id = (Int32)GetOutParameter(cmd, SmartInstallTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartInstallType
        /// </summary>
        /// <param name="smartInstallTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartInstallTypeBase smartInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTINSTALLTYPE);
				
				AddParameter(cmd, pInt32(SmartInstallTypeBase.Property_Id, smartInstallTypeObject.Id));
				AddCommonParams(cmd, smartInstallTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartInstallType
        /// </summary>
        /// <param name="Id">Id of the SmartInstallType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTINSTALLTYPE);	
				
				AddParameter(cmd, pInt32(SmartInstallTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartInstallType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartInstallType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartInstallType object to retrieve</param>
        /// <returns>SmartInstallType object, null if not found</returns>
		public SmartInstallType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTINSTALLTYPEBYID))
			{
				AddParameter( cmd, pInt32(SmartInstallTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartInstallType objects 
        /// </summary>
        /// <returns>A list of SmartInstallType objects</returns>
		public SmartInstallTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTINSTALLTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartInstallType objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartInstallType objects</returns>
		public SmartInstallTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTINSTALLTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartInstallTypeList _SmartInstallTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartInstallTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartInstallType objects by query String
        /// </summary>
        /// <returns>A list of SmartInstallType objects</returns>
		public SmartInstallTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTINSTALLTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartInstallType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTINSTALLTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartInstallType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartInstallTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTINSTALLTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_SmartInstallTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartInstallTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartInstallType object
        /// </summary>
        /// <param name="smartInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartInstallTypeBase smartInstallTypeObject, SqlDataReader reader, int start)
		{
			
				smartInstallTypeObject.Id = reader.GetInt32( start + 0 );			
				smartInstallTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				smartInstallTypeObject.Name = reader.GetString( start + 2 );			
			FillBaseObject(smartInstallTypeObject, reader, (start + 3));

			
			smartInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartInstallType object
        /// </summary>
        /// <param name="smartInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartInstallTypeBase smartInstallTypeObject, SqlDataReader reader)
		{
			FillObject(smartInstallTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartInstallType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartInstallType object</returns>
		private SmartInstallType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartInstallType smartInstallTypeObject= new SmartInstallType();
					FillObject(smartInstallTypeObject, reader);
					return smartInstallTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartInstallType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartInstallType objects</returns>
		private SmartInstallTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartInstallType list
			SmartInstallTypeList list = new SmartInstallTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartInstallType smartInstallTypeObject = new SmartInstallType();
					FillObject(smartInstallTypeObject, reader);

					list.Add(smartInstallTypeObject);
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
