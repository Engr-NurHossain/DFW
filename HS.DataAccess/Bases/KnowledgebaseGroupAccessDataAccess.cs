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
	public partial class KnowledgebaseGroupAccessDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEGROUPACCESS = "InsertKnowledgebaseGroupAccess";
		private const string UPDATEKNOWLEDGEBASEGROUPACCESS = "UpdateKnowledgebaseGroupAccess";
		private const string DELETEKNOWLEDGEBASEGROUPACCESS = "DeleteKnowledgebaseGroupAccess";
		private const string GETKNOWLEDGEBASEGROUPACCESSBYID = "GetKnowledgebaseGroupAccessById";
		private const string GETALLKNOWLEDGEBASEGROUPACCESS = "GetAllKnowledgebaseGroupAccess";
		private const string GETPAGEDKNOWLEDGEBASEGROUPACCESS = "GetPagedKnowledgebaseGroupAccess";
		private const string GETKNOWLEDGEBASEGROUPACCESSMAXIMUMID = "GetKnowledgebaseGroupAccessMaximumId";
		private const string GETKNOWLEDGEBASEGROUPACCESSROWCOUNT = "GetKnowledgebaseGroupAccessRowCount";	
		private const string GETKNOWLEDGEBASEGROUPACCESSBYQUERY = "GetKnowledgebaseGroupAccessByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseGroupAccessDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseGroupAccessDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseGroupAccessObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseGroupAccessBase knowledgebaseGroupAccessObject)
		{	
			AddParameter(cmd, pInt32(KnowledgebaseGroupAccessBase.Property_KnowledgebaseId, knowledgebaseGroupAccessObject.KnowledgebaseId));
			AddParameter(cmd, pBool(KnowledgebaseGroupAccessBase.Property_IsDocumentLibrary, knowledgebaseGroupAccessObject.IsDocumentLibrary));
			AddParameter(cmd, pBool(KnowledgebaseGroupAccessBase.Property_IsDefault, knowledgebaseGroupAccessObject.IsDefault));
			AddParameter(cmd, pInt32(KnowledgebaseGroupAccessBase.Property_UserGroupId, knowledgebaseGroupAccessObject.UserGroupId));
			AddParameter(cmd, pGuid(KnowledgebaseGroupAccessBase.Property_CreatedBy, knowledgebaseGroupAccessObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseGroupAccessBase.Property_CreatedDate, knowledgebaseGroupAccessObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseGroupAccess
        /// </summary>
        /// <param name="knowledgebaseGroupAccessObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseGroupAccessBase knowledgebaseGroupAccessObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEGROUPACCESS);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseGroupAccessBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseGroupAccessObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseGroupAccessObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseGroupAccessObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseGroupAccessBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseGroupAccessObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseGroupAccess
        /// </summary>
        /// <param name="knowledgebaseGroupAccessObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseGroupAccessBase knowledgebaseGroupAccessObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEGROUPACCESS);
				
				AddParameter(cmd, pInt32(KnowledgebaseGroupAccessBase.Property_Id, knowledgebaseGroupAccessObject.Id));
				AddCommonParams(cmd, knowledgebaseGroupAccessObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseGroupAccessObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseGroupAccessObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseGroupAccess
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseGroupAccess object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEGROUPACCESS);	
				
				AddParameter(cmd, pInt32(KnowledgebaseGroupAccessBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseGroupAccess), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseGroupAccess object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseGroupAccess object to retrieve</param>
        /// <returns>KnowledgebaseGroupAccess object, null if not found</returns>
		public KnowledgebaseGroupAccess Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEGROUPACCESSBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseGroupAccessBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseGroupAccess objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseGroupAccess objects</returns>
		public KnowledgebaseGroupAccessList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEGROUPACCESS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseGroupAccess objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseGroupAccess objects</returns>
		public KnowledgebaseGroupAccessList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEGROUPACCESS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseGroupAccessList _KnowledgebaseGroupAccessList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseGroupAccessList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseGroupAccess objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseGroupAccess objects</returns>
		public KnowledgebaseGroupAccessList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEGROUPACCESSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseGroupAccess Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseGroupAccess
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEGROUPACCESSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseGroupAccess Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseGroupAccess
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseGroupAccessRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEGROUPACCESSROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseGroupAccessRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseGroupAccessRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseGroupAccess object
        /// </summary>
        /// <param name="knowledgebaseGroupAccessObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseGroupAccessBase knowledgebaseGroupAccessObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseGroupAccessObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseGroupAccessObject.KnowledgebaseId = reader.GetInt32( start + 1 );			
				knowledgebaseGroupAccessObject.IsDocumentLibrary = reader.GetBoolean( start + 2 );			
				knowledgebaseGroupAccessObject.IsDefault = reader.GetBoolean( start + 3 );			
				knowledgebaseGroupAccessObject.UserGroupId = reader.GetInt32( start + 4 );			
				knowledgebaseGroupAccessObject.CreatedBy = reader.GetGuid( start + 5 );			
				knowledgebaseGroupAccessObject.CreatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(knowledgebaseGroupAccessObject, reader, (start + 7));

			
			knowledgebaseGroupAccessObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseGroupAccess object
        /// </summary>
        /// <param name="knowledgebaseGroupAccessObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseGroupAccessBase knowledgebaseGroupAccessObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseGroupAccessObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseGroupAccess object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseGroupAccess object</returns>
		private KnowledgebaseGroupAccess GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseGroupAccess knowledgebaseGroupAccessObject= new KnowledgebaseGroupAccess();
					FillObject(knowledgebaseGroupAccessObject, reader);
					return knowledgebaseGroupAccessObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseGroupAccess objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseGroupAccess objects</returns>
		private KnowledgebaseGroupAccessList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseGroupAccess list
			KnowledgebaseGroupAccessList list = new KnowledgebaseGroupAccessList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseGroupAccess knowledgebaseGroupAccessObject = new KnowledgebaseGroupAccess();
					FillObject(knowledgebaseGroupAccessObject, reader);

					list.Add(knowledgebaseGroupAccessObject);
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