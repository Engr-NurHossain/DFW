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
	public partial class KnowledgeBaseFlagUserDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEFLAGUSER = "InsertKnowledgeBaseFlagUser";
		private const string UPDATEKNOWLEDGEBASEFLAGUSER = "UpdateKnowledgeBaseFlagUser";
		private const string DELETEKNOWLEDGEBASEFLAGUSER = "DeleteKnowledgeBaseFlagUser";
		private const string GETKNOWLEDGEBASEFLAGUSERBYID = "GetKnowledgeBaseFlagUserById";
		private const string GETALLKNOWLEDGEBASEFLAGUSER = "GetAllKnowledgeBaseFlagUser";
		private const string GETPAGEDKNOWLEDGEBASEFLAGUSER = "GetPagedKnowledgeBaseFlagUser";
		private const string GETKNOWLEDGEBASEFLAGUSERMAXIMUMID = "GetKnowledgeBaseFlagUserMaximumId";
		private const string GETKNOWLEDGEBASEFLAGUSERROWCOUNT = "GetKnowledgeBaseFlagUserRowCount";	
		private const string GETKNOWLEDGEBASEFLAGUSERBYQUERY = "GetKnowledgeBaseFlagUserByQuery";
		#endregion
		
		#region Constructors
		public KnowledgeBaseFlagUserDataAccess(ClientContext context) : base(context) { }
		public KnowledgeBaseFlagUserDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgeBaseFlagUserObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgeBaseFlagUserBase knowledgeBaseFlagUserObject)
		{	
			AddParameter(cmd, pGuid(KnowledgeBaseFlagUserBase.Property_UserId, knowledgeBaseFlagUserObject.UserId));
			AddParameter(cmd, pInt32(KnowledgeBaseFlagUserBase.Property_KnowledgebaseId, knowledgeBaseFlagUserObject.KnowledgebaseId));
			AddParameter(cmd, pBool(KnowledgeBaseFlagUserBase.Property_IsFlag, knowledgeBaseFlagUserObject.IsFlag));
			AddParameter(cmd, pNVarChar(KnowledgeBaseFlagUserBase.Property_Comment, knowledgeBaseFlagUserObject.Comment));
			AddParameter(cmd, pGuid(KnowledgeBaseFlagUserBase.Property_CreatedBy, knowledgeBaseFlagUserObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgeBaseFlagUserBase.Property_CreatedDate, knowledgeBaseFlagUserObject.CreatedDate));
			AddParameter(cmd, pGuid(KnowledgeBaseFlagUserBase.Property_LastUpdatedBy, knowledgeBaseFlagUserObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(KnowledgeBaseFlagUserBase.Property_LastUpdatedDate, knowledgeBaseFlagUserObject.LastUpdatedDate));
			AddParameter(cmd, pBool(KnowledgeBaseFlagUserBase.Property_IsDocument, knowledgeBaseFlagUserObject.IsDocument));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgeBaseFlagUser
        /// </summary>
        /// <param name="knowledgeBaseFlagUserObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgeBaseFlagUserBase knowledgeBaseFlagUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEFLAGUSER);
	
				AddParameter(cmd, pInt32Out(KnowledgeBaseFlagUserBase.Property_Id));
				AddCommonParams(cmd, knowledgeBaseFlagUserObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgeBaseFlagUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgeBaseFlagUserObject.Id = (Int32)GetOutParameter(cmd, KnowledgeBaseFlagUserBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgeBaseFlagUserObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgeBaseFlagUser
        /// </summary>
        /// <param name="knowledgeBaseFlagUserObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgeBaseFlagUserBase knowledgeBaseFlagUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEFLAGUSER);
				
				AddParameter(cmd, pInt32(KnowledgeBaseFlagUserBase.Property_Id, knowledgeBaseFlagUserObject.Id));
				AddCommonParams(cmd, knowledgeBaseFlagUserObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgeBaseFlagUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgeBaseFlagUserObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgeBaseFlagUser
        /// </summary>
        /// <param name="Id">Id of the KnowledgeBaseFlagUser object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEFLAGUSER);	
				
				AddParameter(cmd, pInt32(KnowledgeBaseFlagUserBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgeBaseFlagUser), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgeBaseFlagUser object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgeBaseFlagUser object to retrieve</param>
        /// <returns>KnowledgeBaseFlagUser object, null if not found</returns>
		public KnowledgeBaseFlagUser Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFLAGUSERBYID))
			{
				AddParameter( cmd, pInt32(KnowledgeBaseFlagUserBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgeBaseFlagUser objects 
        /// </summary>
        /// <returns>A list of KnowledgeBaseFlagUser objects</returns>
		public KnowledgeBaseFlagUserList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEFLAGUSER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgeBaseFlagUser objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgeBaseFlagUser objects</returns>
		public KnowledgeBaseFlagUserList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEFLAGUSER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgeBaseFlagUserList _KnowledgeBaseFlagUserList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgeBaseFlagUserList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgeBaseFlagUser objects by query String
        /// </summary>
        /// <returns>A list of KnowledgeBaseFlagUser objects</returns>
		public KnowledgeBaseFlagUserList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFLAGUSERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgeBaseFlagUser Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgeBaseFlagUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFLAGUSERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgeBaseFlagUser Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgeBaseFlagUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgeBaseFlagUserRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFLAGUSERROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgeBaseFlagUserRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgeBaseFlagUserRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgeBaseFlagUser object
        /// </summary>
        /// <param name="knowledgeBaseFlagUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgeBaseFlagUserBase knowledgeBaseFlagUserObject, SqlDataReader reader, int start)
		{
			
				knowledgeBaseFlagUserObject.Id = reader.GetInt32( start + 0 );			
				knowledgeBaseFlagUserObject.UserId = reader.GetGuid( start + 1 );			
				knowledgeBaseFlagUserObject.KnowledgebaseId = reader.GetInt32( start + 2 );			
				knowledgeBaseFlagUserObject.IsFlag = reader.GetBoolean( start + 3 );			
				knowledgeBaseFlagUserObject.Comment = reader.GetString( start + 4 );			
				knowledgeBaseFlagUserObject.CreatedBy = reader.GetGuid( start + 5 );			
				knowledgeBaseFlagUserObject.CreatedDate = reader.GetDateTime( start + 6 );			
				knowledgeBaseFlagUserObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				knowledgeBaseFlagUserObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
				knowledgeBaseFlagUserObject.IsDocument = reader.GetBoolean( start + 9 );			
			FillBaseObject(knowledgeBaseFlagUserObject, reader, (start + 10));

			
			knowledgeBaseFlagUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgeBaseFlagUser object
        /// </summary>
        /// <param name="knowledgeBaseFlagUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgeBaseFlagUserBase knowledgeBaseFlagUserObject, SqlDataReader reader)
		{
			FillObject(knowledgeBaseFlagUserObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgeBaseFlagUser object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgeBaseFlagUser object</returns>
		private KnowledgeBaseFlagUser GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgeBaseFlagUser knowledgeBaseFlagUserObject= new KnowledgeBaseFlagUser();
					FillObject(knowledgeBaseFlagUserObject, reader);
					return knowledgeBaseFlagUserObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgeBaseFlagUser objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgeBaseFlagUser objects</returns>
		private KnowledgeBaseFlagUserList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgeBaseFlagUser list
			KnowledgeBaseFlagUserList list = new KnowledgeBaseFlagUserList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgeBaseFlagUser knowledgeBaseFlagUserObject = new KnowledgeBaseFlagUser();
					FillObject(knowledgeBaseFlagUserObject, reader);

					list.Add(knowledgeBaseFlagUserObject);
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