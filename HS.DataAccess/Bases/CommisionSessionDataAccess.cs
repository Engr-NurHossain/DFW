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
	public partial class CommisionSessionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMMISIONSESSION = "InsertCommisionSession";
		private const string UPDATECOMMISIONSESSION = "UpdateCommisionSession";
		private const string DELETECOMMISIONSESSION = "DeleteCommisionSession";
		private const string GETCOMMISIONSESSIONBYID = "GetCommisionSessionById";
		private const string GETALLCOMMISIONSESSION = "GetAllCommisionSession";
		private const string GETPAGEDCOMMISIONSESSION = "GetPagedCommisionSession";
		private const string GETCOMMISIONSESSIONMAXIMUMID = "GetCommisionSessionMaximumId";
		private const string GETCOMMISIONSESSIONROWCOUNT = "GetCommisionSessionRowCount";	
		private const string GETCOMMISIONSESSIONBYQUERY = "GetCommisionSessionByQuery";
		#endregion
		
		#region Constructors
		public CommisionSessionDataAccess(ClientContext context) : base(context) { }
		public CommisionSessionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="commisionSessionObject"></param>
		private void AddCommonParams(SqlCommand cmd, CommisionSessionBase commisionSessionObject)
		{	
			AddParameter(cmd, pNVarChar(CommisionSessionBase.Property_Name, 50, commisionSessionObject.Name));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CommisionSession
        /// </summary>
        /// <param name="commisionSessionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CommisionSessionBase commisionSessionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMMISIONSESSION);
	
				AddParameter(cmd, pInt32Out(CommisionSessionBase.Property_Id));
				AddCommonParams(cmd, commisionSessionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					commisionSessionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					commisionSessionObject.Id = (Int32)GetOutParameter(cmd, CommisionSessionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(commisionSessionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CommisionSession
        /// </summary>
        /// <param name="commisionSessionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CommisionSessionBase commisionSessionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMMISIONSESSION);
				
				AddParameter(cmd, pInt32(CommisionSessionBase.Property_Id, commisionSessionObject.Id));
				AddCommonParams(cmd, commisionSessionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					commisionSessionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(commisionSessionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CommisionSession
        /// </summary>
        /// <param name="Id">Id of the CommisionSession object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMMISIONSESSION);	
				
				AddParameter(cmd, pInt32(CommisionSessionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CommisionSession), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CommisionSession object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CommisionSession object to retrieve</param>
        /// <returns>CommisionSession object, null if not found</returns>
		public CommisionSession Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONSESSIONBYID))
			{
				AddParameter( cmd, pInt32(CommisionSessionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CommisionSession objects 
        /// </summary>
        /// <returns>A list of CommisionSession objects</returns>
		public CommisionSessionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMMISIONSESSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CommisionSession objects by PageRequest
        /// </summary>
        /// <returns>A list of CommisionSession objects</returns>
		public CommisionSessionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMMISIONSESSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CommisionSessionList _CommisionSessionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CommisionSessionList;
			}
		}
		
		/// <summary>
        /// Retrieves all CommisionSession objects by query String
        /// </summary>
        /// <returns>A list of CommisionSession objects</returns>
		public CommisionSessionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONSESSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CommisionSession Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CommisionSession
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONSESSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CommisionSession Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CommisionSession
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CommisionSessionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONSESSIONROWCOUNT))
			{
				SqlDataReader reader;
				_CommisionSessionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CommisionSessionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CommisionSession object
        /// </summary>
        /// <param name="commisionSessionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CommisionSessionBase commisionSessionObject, SqlDataReader reader, int start)
		{
			
				commisionSessionObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) commisionSessionObject.Name = reader.GetString( start + 1 );			
			FillBaseObject(commisionSessionObject, reader, (start + 2));

			
			commisionSessionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CommisionSession object
        /// </summary>
        /// <param name="commisionSessionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CommisionSessionBase commisionSessionObject, SqlDataReader reader)
		{
			FillObject(commisionSessionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CommisionSession object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CommisionSession object</returns>
		private CommisionSession GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CommisionSession commisionSessionObject= new CommisionSession();
					FillObject(commisionSessionObject, reader);
					return commisionSessionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CommisionSession objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CommisionSession objects</returns>
		private CommisionSessionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CommisionSession list
			CommisionSessionList list = new CommisionSessionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CommisionSession commisionSessionObject = new CommisionSession();
					FillObject(commisionSessionObject, reader);

					list.Add(commisionSessionObject);
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
