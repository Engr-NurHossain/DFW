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
	public partial class KnowledgebaseFavouriteUserDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEFAVOURITEUSER = "InsertKnowledgebaseFavouriteUser";
		private const string UPDATEKNOWLEDGEBASEFAVOURITEUSER = "UpdateKnowledgebaseFavouriteUser";
		private const string DELETEKNOWLEDGEBASEFAVOURITEUSER = "DeleteKnowledgebaseFavouriteUser";
		private const string GETKNOWLEDGEBASEFAVOURITEUSERBYID = "GetKnowledgebaseFavouriteUserById";
		private const string GETALLKNOWLEDGEBASEFAVOURITEUSER = "GetAllKnowledgebaseFavouriteUser";
		private const string GETPAGEDKNOWLEDGEBASEFAVOURITEUSER = "GetPagedKnowledgebaseFavouriteUser";
		private const string GETKNOWLEDGEBASEFAVOURITEUSERMAXIMUMID = "GetKnowledgebaseFavouriteUserMaximumId";
		private const string GETKNOWLEDGEBASEFAVOURITEUSERROWCOUNT = "GetKnowledgebaseFavouriteUserRowCount";	
		private const string GETKNOWLEDGEBASEFAVOURITEUSERBYQUERY = "GetKnowledgebaseFavouriteUserByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseFavouriteUserDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseFavouriteUserDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseFavouriteUserObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseFavouriteUserBase knowledgebaseFavouriteUserObject)
		{	
			AddParameter(cmd, pGuid(KnowledgebaseFavouriteUserBase.Property_UserId, knowledgebaseFavouriteUserObject.UserId));
			AddParameter(cmd, pInt32(KnowledgebaseFavouriteUserBase.Property_KnowledgebaseId, knowledgebaseFavouriteUserObject.KnowledgebaseId));
			AddParameter(cmd, pBool(KnowledgebaseFavouriteUserBase.Property_IsFavourite, knowledgebaseFavouriteUserObject.IsFavourite));
			AddParameter(cmd, pNVarChar(KnowledgebaseFavouriteUserBase.Property_Comment, knowledgebaseFavouriteUserObject.Comment));
			AddParameter(cmd, pGuid(KnowledgebaseFavouriteUserBase.Property_CreatedBy, knowledgebaseFavouriteUserObject.CreatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseFavouriteUserBase.Property_CreatedDate, knowledgebaseFavouriteUserObject.CreatedDate));
			AddParameter(cmd, pGuid(KnowledgebaseFavouriteUserBase.Property_LastUpdatedBy, knowledgebaseFavouriteUserObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseFavouriteUserBase.Property_LastUpdatedDate, knowledgebaseFavouriteUserObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseFavouriteUser
        /// </summary>
        /// <param name="knowledgebaseFavouriteUserObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseFavouriteUserBase knowledgebaseFavouriteUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEFAVOURITEUSER);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseFavouriteUserBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseFavouriteUserObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseFavouriteUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseFavouriteUserObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseFavouriteUserBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseFavouriteUserObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseFavouriteUser
        /// </summary>
        /// <param name="knowledgebaseFavouriteUserObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseFavouriteUserBase knowledgebaseFavouriteUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEFAVOURITEUSER);
				
				AddParameter(cmd, pInt32(KnowledgebaseFavouriteUserBase.Property_Id, knowledgebaseFavouriteUserObject.Id));
				AddCommonParams(cmd, knowledgebaseFavouriteUserObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseFavouriteUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseFavouriteUserObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseFavouriteUser
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseFavouriteUser object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEFAVOURITEUSER);	
				
				AddParameter(cmd, pInt32(KnowledgebaseFavouriteUserBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseFavouriteUser), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseFavouriteUser object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseFavouriteUser object to retrieve</param>
        /// <returns>KnowledgebaseFavouriteUser object, null if not found</returns>
		public KnowledgebaseFavouriteUser Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFAVOURITEUSERBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseFavouriteUserBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseFavouriteUser objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseFavouriteUser objects</returns>
		public KnowledgebaseFavouriteUserList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEFAVOURITEUSER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseFavouriteUser objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseFavouriteUser objects</returns>
		public KnowledgebaseFavouriteUserList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEFAVOURITEUSER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseFavouriteUserList _KnowledgebaseFavouriteUserList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseFavouriteUserList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseFavouriteUser objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseFavouriteUser objects</returns>
		public KnowledgebaseFavouriteUserList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFAVOURITEUSERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseFavouriteUser Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseFavouriteUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFAVOURITEUSERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseFavouriteUser Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseFavouriteUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseFavouriteUserRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEFAVOURITEUSERROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseFavouriteUserRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseFavouriteUserRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseFavouriteUser object
        /// </summary>
        /// <param name="knowledgebaseFavouriteUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseFavouriteUserBase knowledgebaseFavouriteUserObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseFavouriteUserObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseFavouriteUserObject.UserId = reader.GetGuid( start + 1 );			
				knowledgebaseFavouriteUserObject.KnowledgebaseId = reader.GetInt32( start + 2 );			
				knowledgebaseFavouriteUserObject.IsFavourite = reader.GetBoolean( start + 3 );			
				knowledgebaseFavouriteUserObject.Comment = reader.GetString( start + 4 );			
				knowledgebaseFavouriteUserObject.CreatedBy = reader.GetGuid( start + 5 );			
				knowledgebaseFavouriteUserObject.CreatedDate = reader.GetDateTime( start + 6 );			
				knowledgebaseFavouriteUserObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				knowledgebaseFavouriteUserObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
			FillBaseObject(knowledgebaseFavouriteUserObject, reader, (start + 9));

			
			knowledgebaseFavouriteUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseFavouriteUser object
        /// </summary>
        /// <param name="knowledgebaseFavouriteUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseFavouriteUserBase knowledgebaseFavouriteUserObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseFavouriteUserObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseFavouriteUser object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseFavouriteUser object</returns>
		private KnowledgebaseFavouriteUser GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseFavouriteUser knowledgebaseFavouriteUserObject= new KnowledgebaseFavouriteUser();
					FillObject(knowledgebaseFavouriteUserObject, reader);
					return knowledgebaseFavouriteUserObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseFavouriteUser objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseFavouriteUser objects</returns>
		private KnowledgebaseFavouriteUserList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseFavouriteUser list
			KnowledgebaseFavouriteUserList list = new KnowledgebaseFavouriteUserList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseFavouriteUser knowledgebaseFavouriteUserObject = new KnowledgebaseFavouriteUser();
					FillObject(knowledgebaseFavouriteUserObject, reader);

					list.Add(knowledgebaseFavouriteUserObject);
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