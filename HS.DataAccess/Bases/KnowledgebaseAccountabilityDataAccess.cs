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
	public partial class KnowledgebaseAccountabilityDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEACCOUNTABILITY = "InsertKnowledgebaseAccountability";
		private const string UPDATEKNOWLEDGEBASEACCOUNTABILITY = "UpdateKnowledgebaseAccountability";
		private const string DELETEKNOWLEDGEBASEACCOUNTABILITY = "DeleteKnowledgebaseAccountability";
		private const string GETKNOWLEDGEBASEACCOUNTABILITYBYID = "GetKnowledgebaseAccountabilityById";
		private const string GETALLKNOWLEDGEBASEACCOUNTABILITY = "GetAllKnowledgebaseAccountability";
		private const string GETPAGEDKNOWLEDGEBASEACCOUNTABILITY = "GetPagedKnowledgebaseAccountability";
		private const string GETKNOWLEDGEBASEACCOUNTABILITYMAXIMUMID = "GetKnowledgebaseAccountabilityMaximumId";
		private const string GETKNOWLEDGEBASEACCOUNTABILITYROWCOUNT = "GetKnowledgebaseAccountabilityRowCount";	
		private const string GETKNOWLEDGEBASEACCOUNTABILITYBYQUERY = "GetKnowledgebaseAccountabilityByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseAccountabilityDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseAccountabilityDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseAccountabilityObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseAccountabilityBase knowledgebaseAccountabilityObject)
		{	
			AddParameter(cmd, pInt32(KnowledgebaseAccountabilityBase.Property_KnowledgebaseId, knowledgebaseAccountabilityObject.KnowledgebaseId));
			AddParameter(cmd, pGuid(KnowledgebaseAccountabilityBase.Property_AssignedUser, knowledgebaseAccountabilityObject.AssignedUser));
			AddParameter(cmd, pDateTime(KnowledgebaseAccountabilityBase.Property_AssignedDate, knowledgebaseAccountabilityObject.AssignedDate));
			AddParameter(cmd, pDateTime(KnowledgebaseAccountabilityBase.Property_EndDate, knowledgebaseAccountabilityObject.EndDate));
			AddParameter(cmd, pBool(KnowledgebaseAccountabilityBase.Property_IsRead, knowledgebaseAccountabilityObject.IsRead));
			AddParameter(cmd, pGuid(KnowledgebaseAccountabilityBase.Property_AssignedBy, knowledgebaseAccountabilityObject.AssignedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseAccountability
        /// </summary>
        /// <param name="knowledgebaseAccountabilityObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseAccountabilityBase knowledgebaseAccountabilityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEACCOUNTABILITY);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseAccountabilityBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseAccountabilityObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseAccountabilityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseAccountabilityObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseAccountabilityBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseAccountabilityObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseAccountability
        /// </summary>
        /// <param name="knowledgebaseAccountabilityObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseAccountabilityBase knowledgebaseAccountabilityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEACCOUNTABILITY);
				
				AddParameter(cmd, pInt32(KnowledgebaseAccountabilityBase.Property_Id, knowledgebaseAccountabilityObject.Id));
				AddCommonParams(cmd, knowledgebaseAccountabilityObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseAccountabilityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseAccountabilityObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseAccountability
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseAccountability object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEACCOUNTABILITY);	
				
				AddParameter(cmd, pInt32(KnowledgebaseAccountabilityBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseAccountability), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseAccountability object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseAccountability object to retrieve</param>
        /// <returns>KnowledgebaseAccountability object, null if not found</returns>
		public KnowledgebaseAccountability Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCOUNTABILITYBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseAccountabilityBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseAccountability objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseAccountability objects</returns>
		public KnowledgebaseAccountabilityList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEACCOUNTABILITY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseAccountability objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseAccountability objects</returns>
		public KnowledgebaseAccountabilityList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEACCOUNTABILITY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseAccountabilityList _KnowledgebaseAccountabilityList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseAccountabilityList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseAccountability objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseAccountability objects</returns>
		public KnowledgebaseAccountabilityList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCOUNTABILITYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseAccountability Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseAccountability
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCOUNTABILITYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseAccountability Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseAccountability
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseAccountabilityRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCOUNTABILITYROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseAccountabilityRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseAccountabilityRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseAccountability object
        /// </summary>
        /// <param name="knowledgebaseAccountabilityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseAccountabilityBase knowledgebaseAccountabilityObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseAccountabilityObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseAccountabilityObject.KnowledgebaseId = reader.GetInt32( start + 1 );			
				knowledgebaseAccountabilityObject.AssignedUser = reader.GetGuid( start + 2 );			
				knowledgebaseAccountabilityObject.AssignedDate = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) knowledgebaseAccountabilityObject.EndDate = reader.GetDateTime( start + 4 );			
				knowledgebaseAccountabilityObject.IsRead = reader.GetBoolean( start + 5 );			
				knowledgebaseAccountabilityObject.AssignedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(knowledgebaseAccountabilityObject, reader, (start + 7));

			
			knowledgebaseAccountabilityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseAccountability object
        /// </summary>
        /// <param name="knowledgebaseAccountabilityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseAccountabilityBase knowledgebaseAccountabilityObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseAccountabilityObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseAccountability object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseAccountability object</returns>
		private KnowledgebaseAccountability GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseAccountability knowledgebaseAccountabilityObject= new KnowledgebaseAccountability();
					FillObject(knowledgebaseAccountabilityObject, reader);
					return knowledgebaseAccountabilityObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseAccountability objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseAccountability objects</returns>
		private KnowledgebaseAccountabilityList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseAccountability list
			KnowledgebaseAccountabilityList list = new KnowledgebaseAccountabilityList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseAccountability knowledgebaseAccountabilityObject = new KnowledgebaseAccountability();
					FillObject(knowledgebaseAccountabilityObject, reader);

					list.Add(knowledgebaseAccountabilityObject);
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