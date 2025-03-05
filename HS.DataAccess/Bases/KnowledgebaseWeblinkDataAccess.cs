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
	public partial class KnowledgebaseWeblinkDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEWEBLINK = "InsertKnowledgebaseWeblink";
		private const string UPDATEKNOWLEDGEBASEWEBLINK = "UpdateKnowledgebaseWeblink";
		private const string DELETEKNOWLEDGEBASEWEBLINK = "DeleteKnowledgebaseWeblink";
		private const string GETKNOWLEDGEBASEWEBLINKBYID = "GetKnowledgebaseWeblinkById";
		private const string GETALLKNOWLEDGEBASEWEBLINK = "GetAllKnowledgebaseWeblink";
		private const string GETPAGEDKNOWLEDGEBASEWEBLINK = "GetPagedKnowledgebaseWeblink";
		private const string GETKNOWLEDGEBASEWEBLINKMAXIMUMID = "GetKnowledgebaseWeblinkMaximumId";
		private const string GETKNOWLEDGEBASEWEBLINKROWCOUNT = "GetKnowledgebaseWeblinkRowCount";	
		private const string GETKNOWLEDGEBASEWEBLINKBYQUERY = "GetKnowledgebaseWeblinkByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseWeblinkDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseWeblinkDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseWeblinkObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseWeblinkBase knowledgebaseWeblinkObject)
		{	
			AddParameter(cmd, pInt32(KnowledgebaseWeblinkBase.Property_KnowledgebaseId, knowledgebaseWeblinkObject.KnowledgebaseId));
			AddParameter(cmd, pNVarChar(KnowledgebaseWeblinkBase.Property_Title, 100, knowledgebaseWeblinkObject.Title));
			AddParameter(cmd, pNVarChar(KnowledgebaseWeblinkBase.Property_Link, 100, knowledgebaseWeblinkObject.Link));
			AddParameter(cmd, pBool(KnowledgebaseWeblinkBase.Property_IsRelated, knowledgebaseWeblinkObject.IsRelated));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseWeblink
        /// </summary>
        /// <param name="knowledgebaseWeblinkObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseWeblinkBase knowledgebaseWeblinkObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEWEBLINK);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseWeblinkBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseWeblinkObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseWeblinkObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseWeblinkBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseWeblinkObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseWeblink
        /// </summary>
        /// <param name="knowledgebaseWeblinkObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseWeblinkBase knowledgebaseWeblinkObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEWEBLINK);
				
				AddParameter(cmd, pInt32(KnowledgebaseWeblinkBase.Property_Id, knowledgebaseWeblinkObject.Id));
				AddCommonParams(cmd, knowledgebaseWeblinkObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseWeblinkObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseWeblink
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseWeblink object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEWEBLINK);	
				
				AddParameter(cmd, pInt32(KnowledgebaseWeblinkBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseWeblink), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseWeblink object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseWeblink object to retrieve</param>
        /// <returns>KnowledgebaseWeblink object, null if not found</returns>
		public KnowledgebaseWeblink Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEWEBLINKBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseWeblinkBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseWeblink objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseWeblink objects</returns>
		public KnowledgebaseWeblinkList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEWEBLINK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseWeblink objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseWeblink objects</returns>
		public KnowledgebaseWeblinkList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEWEBLINK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseWeblinkList _KnowledgebaseWeblinkList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseWeblinkList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseWeblink objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseWeblink objects</returns>
		public KnowledgebaseWeblinkList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEWEBLINKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseWeblink Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseWeblink
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEWEBLINKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseWeblink Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseWeblink
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseWeblinkRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEWEBLINKROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseWeblinkRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseWeblinkRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseWeblink object
        /// </summary>
        /// <param name="knowledgebaseWeblinkObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseWeblinkBase knowledgebaseWeblinkObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseWeblinkObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseWeblinkObject.KnowledgebaseId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) knowledgebaseWeblinkObject.Title = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) knowledgebaseWeblinkObject.Link = reader.GetString( start + 3 );			
				knowledgebaseWeblinkObject.IsRelated = reader.GetBoolean( start + 4 );			
			FillBaseObject(knowledgebaseWeblinkObject, reader, (start + 5));

			
			knowledgebaseWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseWeblink object
        /// </summary>
        /// <param name="knowledgebaseWeblinkObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseWeblinkBase knowledgebaseWeblinkObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseWeblinkObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseWeblink object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseWeblink object</returns>
		private KnowledgebaseWeblink GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseWeblink knowledgebaseWeblinkObject= new KnowledgebaseWeblink();
					FillObject(knowledgebaseWeblinkObject, reader);
					return knowledgebaseWeblinkObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseWeblink objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseWeblink objects</returns>
		private KnowledgebaseWeblinkList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseWeblink list
			KnowledgebaseWeblinkList list = new KnowledgebaseWeblinkList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseWeblink knowledgebaseWeblinkObject = new KnowledgebaseWeblink();
					FillObject(knowledgebaseWeblinkObject, reader);

					list.Add(knowledgebaseWeblinkObject);
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