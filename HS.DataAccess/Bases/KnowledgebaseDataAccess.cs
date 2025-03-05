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
	public partial class KnowledgebaseDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASE = "InsertKnowledgebase";
		private const string UPDATEKNOWLEDGEBASE = "UpdateKnowledgebase";
		private const string DELETEKNOWLEDGEBASE = "DeleteKnowledgebase";
		private const string GETKNOWLEDGEBASEBYID = "GetKnowledgebaseById";
		private const string GETALLKNOWLEDGEBASE = "GetAllKnowledgebase";
		private const string GETPAGEDKNOWLEDGEBASE = "GetPagedKnowledgebase";
		private const string GETKNOWLEDGEBASEMAXIMUMID = "GetKnowledgebaseMaximumId";
		private const string GETKNOWLEDGEBASEROWCOUNT = "GetKnowledgebaseRowCount";	
		private const string GETKNOWLEDGEBASEBYQUERY = "GetKnowledgebaseByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseBase knowledgebaseObject)
		{	
			AddParameter(cmd, pNVarChar(KnowledgebaseBase.Property_Title, 500, knowledgebaseObject.Title));
			AddParameter(cmd, pNVarChar(KnowledgebaseBase.Property_Answer, knowledgebaseObject.Answer));
			AddParameter(cmd, pNVarChar(KnowledgebaseBase.Property_Tags, 100, knowledgebaseObject.Tags));
			AddParameter(cmd, pGuid(KnowledgebaseBase.Property_CreatedBy, knowledgebaseObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseBase.Property_CreatedDate, knowledgebaseObject.CreatedDate));
			AddParameter(cmd, pGuid(KnowledgebaseBase.Property_LastUpdatedBy, knowledgebaseObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseBase.Property_LastUpdatedDate, knowledgebaseObject.LastUpdatedDate));
			AddParameter(cmd, pBool(KnowledgebaseBase.Property_IsDeleted, knowledgebaseObject.IsDeleted));
			AddParameter(cmd, pBool(KnowledgebaseBase.Property_IsDocumentLibrary, knowledgebaseObject.IsDocumentLibrary));
			AddParameter(cmd, pBool(KnowledgebaseBase.Property_IsFlag, knowledgebaseObject.IsFlag));
			AddParameter(cmd, pGuid(KnowledgebaseBase.Property_FlagBy, knowledgebaseObject.FlagBy));
			AddParameter(cmd, pDateTime(KnowledgebaseBase.Property_FlagDate, knowledgebaseObject.FlagDate));
			AddParameter(cmd, pBool(KnowledgebaseBase.Property_IsHidden, knowledgebaseObject.IsHidden));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Knowledgebase
        /// </summary>
        /// <param name="knowledgebaseObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseBase knowledgebaseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASE);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Knowledgebase
        /// </summary>
        /// <param name="knowledgebaseObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseBase knowledgebaseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASE);
				
				AddParameter(cmd, pInt32(KnowledgebaseBase.Property_Id, knowledgebaseObject.Id));
				AddCommonParams(cmd, knowledgebaseObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Knowledgebase
        /// </summary>
        /// <param name="Id">Id of the Knowledgebase object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASE);	
				
				AddParameter(cmd, pInt32(KnowledgebaseBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Knowledgebase), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Knowledgebase object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Knowledgebase object to retrieve</param>
        /// <returns>Knowledgebase object, null if not found</returns>
		public Knowledgebase Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Knowledgebase objects 
        /// </summary>
        /// <returns>A list of Knowledgebase objects</returns>
		public KnowledgebaseList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Knowledgebase objects by PageRequest
        /// </summary>
        /// <returns>A list of Knowledgebase objects</returns>
		public KnowledgebaseList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseList _KnowledgebaseList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseList;
			}
		}
		
		/// <summary>
        /// Retrieves all Knowledgebase objects by query String
        /// </summary>
        /// <returns>A list of Knowledgebase objects</returns>
		public KnowledgebaseList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Knowledgebase Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Knowledgebase
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Knowledgebase Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Knowledgebase
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Knowledgebase object
        /// </summary>
        /// <param name="knowledgebaseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseBase knowledgebaseObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseObject.Title = reader.GetString( start + 1 );			
				knowledgebaseObject.Answer = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) knowledgebaseObject.Tags = reader.GetString( start + 3 );			
				knowledgebaseObject.CreatedBy = reader.GetGuid( start + 4 );			
				knowledgebaseObject.CreatedDate = reader.GetDateTime( start + 5 );			
				knowledgebaseObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				knowledgebaseObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				knowledgebaseObject.IsDeleted = reader.GetBoolean( start + 8 );			
				knowledgebaseObject.IsDocumentLibrary = reader.GetBoolean( start + 9 );			
				knowledgebaseObject.IsFlag = reader.GetBoolean( start + 10 );			
				knowledgebaseObject.FlagBy = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) knowledgebaseObject.FlagDate = reader.GetDateTime( start + 12 );			
				knowledgebaseObject.IsHidden = reader.GetBoolean( start + 13 );			
			FillBaseObject(knowledgebaseObject, reader, (start + 14));

			
			knowledgebaseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Knowledgebase object
        /// </summary>
        /// <param name="knowledgebaseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseBase knowledgebaseObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Knowledgebase object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Knowledgebase object</returns>
		private Knowledgebase GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Knowledgebase knowledgebaseObject= new Knowledgebase();
					FillObject(knowledgebaseObject, reader);
					return knowledgebaseObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Knowledgebase objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Knowledgebase objects</returns>
		private KnowledgebaseList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Knowledgebase list
			KnowledgebaseList list = new KnowledgebaseList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Knowledgebase knowledgebaseObject = new Knowledgebase();
					FillObject(knowledgebaseObject, reader);

					list.Add(knowledgebaseObject);
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