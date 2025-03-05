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
	public partial class RMRTagMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRMRTAGMAP = "InsertRMRTagMap";
		private const string UPDATERMRTAGMAP = "UpdateRMRTagMap";
		private const string DELETERMRTAGMAP = "DeleteRMRTagMap";
		private const string GETRMRTAGMAPBYID = "GetRMRTagMapById";
		private const string GETALLRMRTAGMAP = "GetAllRMRTagMap";
		private const string GETPAGEDRMRTAGMAP = "GetPagedRMRTagMap";
		private const string GETRMRTAGMAPMAXIMUMID = "GetRMRTagMapMaximumId";
		private const string GETRMRTAGMAPROWCOUNT = "GetRMRTagMapRowCount";	
		private const string GETRMRTAGMAPBYQUERY = "GetRMRTagMapByQuery";
		#endregion
		
		#region Constructors
		public RMRTagMapDataAccess(ClientContext context) : base(context) { }
		public RMRTagMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="rMRTagMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, RMRTagMapBase rMRTagMapObject)
		{	
			AddParameter(cmd, pGuid(RMRTagMapBase.Property_TagId, rMRTagMapObject.TagId));
			AddParameter(cmd, pGuid(RMRTagMapBase.Property_ContactId, rMRTagMapObject.ContactId));
			AddParameter(cmd, pGuid(RMRTagMapBase.Property_CreatedBy, rMRTagMapObject.CreatedBy));
			AddParameter(cmd, pDateTime(RMRTagMapBase.Property_CreatedDate, rMRTagMapObject.CreatedDate));
			AddParameter(cmd, pGuid(RMRTagMapBase.Property_LastUpdatedBy, rMRTagMapObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RMRTagMapBase.Property_LastUpdatedDate, rMRTagMapObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RMRTagMap
        /// </summary>
        /// <param name="rMRTagMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RMRTagMapBase rMRTagMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRMRTAGMAP);
	
				AddParameter(cmd, pInt32Out(RMRTagMapBase.Property_Id));
				AddCommonParams(cmd, rMRTagMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					rMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					rMRTagMapObject.Id = (Int32)GetOutParameter(cmd, RMRTagMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(rMRTagMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RMRTagMap
        /// </summary>
        /// <param name="rMRTagMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RMRTagMapBase rMRTagMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERMRTAGMAP);
				
				AddParameter(cmd, pInt32(RMRTagMapBase.Property_Id, rMRTagMapObject.Id));
				AddCommonParams(cmd, rMRTagMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					rMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(rMRTagMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RMRTagMap
        /// </summary>
        /// <param name="Id">Id of the RMRTagMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERMRTAGMAP);	
				
				AddParameter(cmd, pInt32(RMRTagMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RMRTagMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RMRTagMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RMRTagMap object to retrieve</param>
        /// <returns>RMRTagMap object, null if not found</returns>
		public RMRTagMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGMAPBYID))
			{
				AddParameter( cmd, pInt32(RMRTagMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RMRTagMap objects 
        /// </summary>
        /// <returns>A list of RMRTagMap objects</returns>
		public RMRTagMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRMRTAGMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RMRTagMap objects by PageRequest
        /// </summary>
        /// <returns>A list of RMRTagMap objects</returns>
		public RMRTagMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRMRTAGMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RMRTagMapList _RMRTagMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RMRTagMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all RMRTagMap objects by query String
        /// </summary>
        /// <returns>A list of RMRTagMap objects</returns>
		public RMRTagMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RMRTagMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RMRTagMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RMRTagMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RMRTagMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RMRTagMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRMRTAGMAPROWCOUNT))
			{
				SqlDataReader reader;
				_RMRTagMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RMRTagMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RMRTagMap object
        /// </summary>
        /// <param name="rMRTagMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RMRTagMapBase rMRTagMapObject, SqlDataReader reader, int start)
		{
			
				rMRTagMapObject.Id = reader.GetInt32( start + 0 );			
				rMRTagMapObject.TagId = reader.GetGuid( start + 1 );			
				rMRTagMapObject.ContactId = reader.GetGuid( start + 2 );			
				rMRTagMapObject.CreatedBy = reader.GetGuid( start + 3 );			
				rMRTagMapObject.CreatedDate = reader.GetDateTime( start + 4 );			
				rMRTagMapObject.LastUpdatedBy = reader.GetGuid( start + 5 );			
				rMRTagMapObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(rMRTagMapObject, reader, (start + 7));

			
			rMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RMRTagMap object
        /// </summary>
        /// <param name="rMRTagMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RMRTagMapBase rMRTagMapObject, SqlDataReader reader)
		{
			FillObject(rMRTagMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RMRTagMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RMRTagMap object</returns>
		private RMRTagMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RMRTagMap rMRTagMapObject= new RMRTagMap();
					FillObject(rMRTagMapObject, reader);
					return rMRTagMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RMRTagMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RMRTagMap objects</returns>
		private RMRTagMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RMRTagMap list
			RMRTagMapList list = new RMRTagMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RMRTagMap rMRTagMapObject = new RMRTagMap();
					FillObject(rMRTagMapObject, reader);

					list.Add(rMRTagMapObject);
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
