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
	public partial class KnowledgebaseRMRTagMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASERMRTAGMAP = "InsertKnowledgebaseRMRTagMap";
		private const string UPDATEKNOWLEDGEBASERMRTAGMAP = "UpdateKnowledgebaseRMRTagMap";
		private const string DELETEKNOWLEDGEBASERMRTAGMAP = "DeleteKnowledgebaseRMRTagMap";
		private const string GETKNOWLEDGEBASERMRTAGMAPBYID = "GetKnowledgebaseRMRTagMapById";
		private const string GETALLKNOWLEDGEBASERMRTAGMAP = "GetAllKnowledgebaseRMRTagMap";
		private const string GETPAGEDKNOWLEDGEBASERMRTAGMAP = "GetPagedKnowledgebaseRMRTagMap";
		private const string GETKNOWLEDGEBASERMRTAGMAPMAXIMUMID = "GetKnowledgebaseRMRTagMapMaximumId";
		private const string GETKNOWLEDGEBASERMRTAGMAPROWCOUNT = "GetKnowledgebaseRMRTagMapRowCount";	
		private const string GETKNOWLEDGEBASERMRTAGMAPBYQUERY = "GetKnowledgebaseRMRTagMapByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseRMRTagMapDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseRMRTagMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseRMRTagMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseRMRTagMapBase knowledgebaseRMRTagMapObject)
		{	
			AddParameter(cmd, pInt32(KnowledgebaseRMRTagMapBase.Property_TagId, knowledgebaseRMRTagMapObject.TagId));
			AddParameter(cmd, pInt32(KnowledgebaseRMRTagMapBase.Property_KnowledgebaseId, knowledgebaseRMRTagMapObject.KnowledgebaseId));
			AddParameter(cmd, pGuid(KnowledgebaseRMRTagMapBase.Property_CreatedBy, knowledgebaseRMRTagMapObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseRMRTagMapBase.Property_CreatedDate, knowledgebaseRMRTagMapObject.CreatedDate));
			AddParameter(cmd, pGuid(KnowledgebaseRMRTagMapBase.Property_LastUpdatedBy, knowledgebaseRMRTagMapObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseRMRTagMapBase.Property_LastUpdatedDate, knowledgebaseRMRTagMapObject.LastUpdatedDate));
			AddParameter(cmd, pBool(KnowledgebaseRMRTagMapBase.Property_IsDeleted, knowledgebaseRMRTagMapObject.IsDeleted));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseRMRTagMap
        /// </summary>
        /// <param name="knowledgebaseRMRTagMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseRMRTagMapBase knowledgebaseRMRTagMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASERMRTAGMAP);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseRMRTagMapBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseRMRTagMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseRMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseRMRTagMapObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseRMRTagMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseRMRTagMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseRMRTagMap
        /// </summary>
        /// <param name="knowledgebaseRMRTagMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseRMRTagMapBase knowledgebaseRMRTagMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASERMRTAGMAP);
				
				AddParameter(cmd, pInt32(KnowledgebaseRMRTagMapBase.Property_Id, knowledgebaseRMRTagMapObject.Id));
				AddCommonParams(cmd, knowledgebaseRMRTagMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseRMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseRMRTagMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseRMRTagMap
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseRMRTagMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASERMRTAGMAP);	
				
				AddParameter(cmd, pInt32(KnowledgebaseRMRTagMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseRMRTagMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseRMRTagMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseRMRTagMap object to retrieve</param>
        /// <returns>KnowledgebaseRMRTagMap object, null if not found</returns>
		public KnowledgebaseRMRTagMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGMAPBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseRMRTagMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTagMap objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTagMap objects</returns>
		public KnowledgebaseRMRTagMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASERMRTAGMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTagMap objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTagMap objects</returns>
		public KnowledgebaseRMRTagMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASERMRTAGMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseRMRTagMapList _KnowledgebaseRMRTagMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseRMRTagMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTagMap objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTagMap objects</returns>
		public KnowledgebaseRMRTagMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseRMRTagMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseRMRTagMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseRMRTagMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseRMRTagMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseRMRTagMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGMAPROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseRMRTagMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseRMRTagMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseRMRTagMap object
        /// </summary>
        /// <param name="knowledgebaseRMRTagMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseRMRTagMapBase knowledgebaseRMRTagMapObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseRMRTagMapObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseRMRTagMapObject.TagId = reader.GetInt32( start + 1 );			
				knowledgebaseRMRTagMapObject.KnowledgebaseId = reader.GetInt32( start + 2 );			
				knowledgebaseRMRTagMapObject.CreatedBy = reader.GetGuid( start + 3 );			
				knowledgebaseRMRTagMapObject.CreatedDate = reader.GetDateTime( start + 4 );			
				knowledgebaseRMRTagMapObject.LastUpdatedBy = reader.GetGuid( start + 5 );			
				knowledgebaseRMRTagMapObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
				knowledgebaseRMRTagMapObject.IsDeleted = reader.GetBoolean( start + 7 );			
			FillBaseObject(knowledgebaseRMRTagMapObject, reader, (start + 8));

			
			knowledgebaseRMRTagMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseRMRTagMap object
        /// </summary>
        /// <param name="knowledgebaseRMRTagMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseRMRTagMapBase knowledgebaseRMRTagMapObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseRMRTagMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseRMRTagMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseRMRTagMap object</returns>
		private KnowledgebaseRMRTagMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseRMRTagMap knowledgebaseRMRTagMapObject= new KnowledgebaseRMRTagMap();
					FillObject(knowledgebaseRMRTagMapObject, reader);
					return knowledgebaseRMRTagMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseRMRTagMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseRMRTagMap objects</returns>
		private KnowledgebaseRMRTagMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseRMRTagMap list
			KnowledgebaseRMRTagMapList list = new KnowledgebaseRMRTagMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseRMRTagMap knowledgebaseRMRTagMapObject = new KnowledgebaseRMRTagMap();
					FillObject(knowledgebaseRMRTagMapObject, reader);

					list.Add(knowledgebaseRMRTagMapObject);
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