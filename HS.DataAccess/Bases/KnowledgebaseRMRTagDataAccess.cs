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
	public partial class KnowledgebaseRMRTagDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASERMRTAG = "InsertKnowledgebaseRMRTag";
		private const string UPDATEKNOWLEDGEBASERMRTAG = "UpdateKnowledgebaseRMRTag";
		private const string DELETEKNOWLEDGEBASERMRTAG = "DeleteKnowledgebaseRMRTag";
		private const string GETKNOWLEDGEBASERMRTAGBYID = "GetKnowledgebaseRMRTagById";
		private const string GETALLKNOWLEDGEBASERMRTAG = "GetAllKnowledgebaseRMRTag";
		private const string GETPAGEDKNOWLEDGEBASERMRTAG = "GetPagedKnowledgebaseRMRTag";
		private const string GETKNOWLEDGEBASERMRTAGMAXIMUMID = "GetKnowledgebaseRMRTagMaximumId";
		private const string GETKNOWLEDGEBASERMRTAGROWCOUNT = "GetKnowledgebaseRMRTagRowCount";	
		private const string GETKNOWLEDGEBASERMRTAGBYQUERY = "GetKnowledgebaseRMRTagByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseRMRTagDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseRMRTagDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseRMRTagObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseRMRTagBase knowledgebaseRMRTagObject)
		{	
			AddParameter(cmd, pNVarChar(KnowledgebaseRMRTagBase.Property_TagName, 150, knowledgebaseRMRTagObject.TagName));
			AddParameter(cmd, pDateTime(KnowledgebaseRMRTagBase.Property_CreatedDate, knowledgebaseRMRTagObject.CreatedDate));
			AddParameter(cmd, pGuid(KnowledgebaseRMRTagBase.Property_CreatedBy, knowledgebaseRMRTagObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseRMRTagBase.Property_LastUpdatedDate, knowledgebaseRMRTagObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(KnowledgebaseRMRTagBase.Property_LastUpdatedBy, knowledgebaseRMRTagObject.LastUpdatedBy));
			AddParameter(cmd, pBool(KnowledgebaseRMRTagBase.Property_IsDeleted, knowledgebaseRMRTagObject.IsDeleted));
			AddParameter(cmd, pBool(KnowledgebaseRMRTagBase.Property_IsFavourite, knowledgebaseRMRTagObject.IsFavourite));
			AddParameter(cmd, pBool(KnowledgebaseRMRTagBase.Property_IsKnowledgebaseNav, knowledgebaseRMRTagObject.IsKnowledgebaseNav));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseRMRTag
        /// </summary>
        /// <param name="knowledgebaseRMRTagObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseRMRTagBase knowledgebaseRMRTagObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASERMRTAG);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseRMRTagBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseRMRTagObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseRMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseRMRTagObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseRMRTagBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseRMRTagObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseRMRTag
        /// </summary>
        /// <param name="knowledgebaseRMRTagObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseRMRTagBase knowledgebaseRMRTagObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASERMRTAG);
				
				AddParameter(cmd, pInt32(KnowledgebaseRMRTagBase.Property_Id, knowledgebaseRMRTagObject.Id));
				AddCommonParams(cmd, knowledgebaseRMRTagObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseRMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseRMRTagObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseRMRTag
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseRMRTag object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASERMRTAG);	
				
				AddParameter(cmd, pInt32(KnowledgebaseRMRTagBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseRMRTag), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseRMRTag object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseRMRTag object to retrieve</param>
        /// <returns>KnowledgebaseRMRTag object, null if not found</returns>
		public KnowledgebaseRMRTag Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseRMRTagBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTag objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTag objects</returns>
		public KnowledgebaseRMRTagList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASERMRTAG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTag objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTag objects</returns>
		public KnowledgebaseRMRTagList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASERMRTAG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseRMRTagList _KnowledgebaseRMRTagList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseRMRTagList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseRMRTag objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseRMRTag objects</returns>
		public KnowledgebaseRMRTagList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseRMRTag Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseRMRTag
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseRMRTag Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseRMRTag
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseRMRTagRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASERMRTAGROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseRMRTagRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseRMRTagRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseRMRTag object
        /// </summary>
        /// <param name="knowledgebaseRMRTagObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseRMRTagBase knowledgebaseRMRTagObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseRMRTagObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseRMRTagObject.TagName = reader.GetString( start + 1 );			
				knowledgebaseRMRTagObject.CreatedDate = reader.GetDateTime( start + 2 );			
				knowledgebaseRMRTagObject.CreatedBy = reader.GetGuid( start + 3 );			
				knowledgebaseRMRTagObject.LastUpdatedDate = reader.GetDateTime( start + 4 );			
				knowledgebaseRMRTagObject.LastUpdatedBy = reader.GetGuid( start + 5 );			
				knowledgebaseRMRTagObject.IsDeleted = reader.GetBoolean( start + 6 );			
				knowledgebaseRMRTagObject.IsFavourite = reader.GetBoolean( start + 7 );			
				knowledgebaseRMRTagObject.IsKnowledgebaseNav = reader.GetBoolean( start + 8 );			
			FillBaseObject(knowledgebaseRMRTagObject, reader, (start + 9));

			
			knowledgebaseRMRTagObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseRMRTag object
        /// </summary>
        /// <param name="knowledgebaseRMRTagObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseRMRTagBase knowledgebaseRMRTagObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseRMRTagObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseRMRTag object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseRMRTag object</returns>
		private KnowledgebaseRMRTag GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseRMRTag knowledgebaseRMRTagObject= new KnowledgebaseRMRTag();
					FillObject(knowledgebaseRMRTagObject, reader);
					return knowledgebaseRMRTagObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseRMRTag objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseRMRTag objects</returns>
		private KnowledgebaseRMRTagList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseRMRTag list
			KnowledgebaseRMRTagList list = new KnowledgebaseRMRTagList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseRMRTag knowledgebaseRMRTagObject = new KnowledgebaseRMRTag();
					FillObject(knowledgebaseRMRTagObject, reader);

					list.Add(knowledgebaseRMRTagObject);
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