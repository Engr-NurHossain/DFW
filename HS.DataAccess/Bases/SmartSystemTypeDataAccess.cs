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
	public partial class SmartSystemTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTSYSTEMTYPE = "InsertSmartSystemType";
		private const string UPDATESMARTSYSTEMTYPE = "UpdateSmartSystemType";
		private const string DELETESMARTSYSTEMTYPE = "DeleteSmartSystemType";
		private const string GETSMARTSYSTEMTYPEBYID = "GetSmartSystemTypeById";
		private const string GETALLSMARTSYSTEMTYPE = "GetAllSmartSystemType";
		private const string GETPAGEDSMARTSYSTEMTYPE = "GetPagedSmartSystemType";
		private const string GETSMARTSYSTEMTYPEMAXIMUMID = "GetSmartSystemTypeMaximumId";
		private const string GETSMARTSYSTEMTYPEROWCOUNT = "GetSmartSystemTypeRowCount";	
		private const string GETSMARTSYSTEMTYPEBYQUERY = "GetSmartSystemTypeByQuery";
		#endregion
		
		#region Constructors
		public SmartSystemTypeDataAccess(ClientContext context) : base(context) { }
		public SmartSystemTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartSystemTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartSystemTypeBase smartSystemTypeObject)
		{	
			AddParameter(cmd, pGuid(SmartSystemTypeBase.Property_CompanyId, smartSystemTypeObject.CompanyId));
			AddParameter(cmd, pNVarChar(SmartSystemTypeBase.Property_Name, 50, smartSystemTypeObject.Name));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartSystemType
        /// </summary>
        /// <param name="smartSystemTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartSystemTypeBase smartSystemTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTSYSTEMTYPE);
	
				AddParameter(cmd, pInt32Out(SmartSystemTypeBase.Property_Id));
				AddCommonParams(cmd, smartSystemTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartSystemTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartSystemTypeObject.Id = (Int32)GetOutParameter(cmd, SmartSystemTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartSystemTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartSystemType
        /// </summary>
        /// <param name="smartSystemTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartSystemTypeBase smartSystemTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTSYSTEMTYPE);
				
				AddParameter(cmd, pInt32(SmartSystemTypeBase.Property_Id, smartSystemTypeObject.Id));
				AddCommonParams(cmd, smartSystemTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartSystemTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartSystemTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartSystemType
        /// </summary>
        /// <param name="Id">Id of the SmartSystemType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTSYSTEMTYPE);	
				
				AddParameter(cmd, pInt32(SmartSystemTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartSystemType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartSystemType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartSystemType object to retrieve</param>
        /// <returns>SmartSystemType object, null if not found</returns>
		public SmartSystemType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMTYPEBYID))
			{
				AddParameter( cmd, pInt32(SmartSystemTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartSystemType objects 
        /// </summary>
        /// <returns>A list of SmartSystemType objects</returns>
		public SmartSystemTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTSYSTEMTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartSystemType objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartSystemType objects</returns>
		public SmartSystemTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTSYSTEMTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartSystemTypeList _SmartSystemTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartSystemTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartSystemType objects by query String
        /// </summary>
        /// <returns>A list of SmartSystemType objects</returns>
		public SmartSystemTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartSystemType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartSystemType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartSystemType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartSystemType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartSystemTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTSYSTEMTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_SmartSystemTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartSystemTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartSystemType object
        /// </summary>
        /// <param name="smartSystemTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartSystemTypeBase smartSystemTypeObject, SqlDataReader reader, int start)
		{
			
				smartSystemTypeObject.Id = reader.GetInt32( start + 0 );			
				smartSystemTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				smartSystemTypeObject.Name = reader.GetString( start + 2 );			
			FillBaseObject(smartSystemTypeObject, reader, (start + 3));

			
			smartSystemTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartSystemType object
        /// </summary>
        /// <param name="smartSystemTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartSystemTypeBase smartSystemTypeObject, SqlDataReader reader)
		{
			FillObject(smartSystemTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartSystemType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartSystemType object</returns>
		private SmartSystemType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartSystemType smartSystemTypeObject= new SmartSystemType();
					FillObject(smartSystemTypeObject, reader);
					return smartSystemTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartSystemType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartSystemType objects</returns>
		private SmartSystemTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartSystemType list
			SmartSystemTypeList list = new SmartSystemTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartSystemType smartSystemTypeObject = new SmartSystemType();
					FillObject(smartSystemTypeObject, reader);

					list.Add(smartSystemTypeObject);
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
