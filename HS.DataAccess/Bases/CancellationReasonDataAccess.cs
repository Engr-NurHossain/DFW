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
	public partial class CancellationReasonDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCANCELLATIONREASON = "InsertCancellationReason";
		private const string UPDATECANCELLATIONREASON = "UpdateCancellationReason";
		private const string DELETECANCELLATIONREASON = "DeleteCancellationReason";
		private const string GETCANCELLATIONREASONBYID = "GetCancellationReasonById";
		private const string GETALLCANCELLATIONREASON = "GetAllCancellationReason";
		private const string GETPAGEDCANCELLATIONREASON = "GetPagedCancellationReason";
		private const string GETCANCELLATIONREASONMAXIMUMID = "GetCancellationReasonMaximumId";
		private const string GETCANCELLATIONREASONROWCOUNT = "GetCancellationReasonRowCount";	
		private const string GETCANCELLATIONREASONBYQUERY = "GetCancellationReasonByQuery";
		#endregion
		
		#region Constructors
		public CancellationReasonDataAccess(ClientContext context) : base(context) { }
		public CancellationReasonDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="cancellationReasonObject"></param>
		private void AddCommonParams(SqlCommand cmd, CancellationReasonBase cancellationReasonObject)
		{	
			AddParameter(cmd, pNVarChar(CancellationReasonBase.Property_Reason, 50, cancellationReasonObject.Reason));
			AddParameter(cmd, pInt32(CancellationReasonBase.Property_OrderBy, cancellationReasonObject.OrderBy));
			AddParameter(cmd, pGuid(CancellationReasonBase.Property_CompanyId, cancellationReasonObject.CompanyId));
			AddParameter(cmd, pBool(CancellationReasonBase.Property_IsActive, cancellationReasonObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CancellationReason
        /// </summary>
        /// <param name="cancellationReasonObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CancellationReasonBase cancellationReasonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCANCELLATIONREASON);
	
				AddParameter(cmd, pInt32Out(CancellationReasonBase.Property_Id));
				AddCommonParams(cmd, cancellationReasonObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					cancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					cancellationReasonObject.Id = (Int32)GetOutParameter(cmd, CancellationReasonBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(cancellationReasonObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CancellationReason
        /// </summary>
        /// <param name="cancellationReasonObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CancellationReasonBase cancellationReasonObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECANCELLATIONREASON);
				
				AddParameter(cmd, pInt32(CancellationReasonBase.Property_Id, cancellationReasonObject.Id));
				AddCommonParams(cmd, cancellationReasonObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					cancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(cancellationReasonObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CancellationReason
        /// </summary>
        /// <param name="Id">Id of the CancellationReason object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECANCELLATIONREASON);	
				
				AddParameter(cmd, pInt32(CancellationReasonBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CancellationReason), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CancellationReason object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CancellationReason object to retrieve</param>
        /// <returns>CancellationReason object, null if not found</returns>
		public CancellationReason Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCANCELLATIONREASONBYID))
			{
				AddParameter( cmd, pInt32(CancellationReasonBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CancellationReason objects 
        /// </summary>
        /// <returns>A list of CancellationReason objects</returns>
		public CancellationReasonList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCANCELLATIONREASON))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CancellationReason objects by PageRequest
        /// </summary>
        /// <returns>A list of CancellationReason objects</returns>
		public CancellationReasonList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCANCELLATIONREASON))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CancellationReasonList _CancellationReasonList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CancellationReasonList;
			}
		}
		
		/// <summary>
        /// Retrieves all CancellationReason objects by query String
        /// </summary>
        /// <returns>A list of CancellationReason objects</returns>
		public CancellationReasonList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCANCELLATIONREASONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CancellationReason Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CancellationReason
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCANCELLATIONREASONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CancellationReason Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CancellationReason
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CancellationReasonRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCANCELLATIONREASONROWCOUNT))
			{
				SqlDataReader reader;
				_CancellationReasonRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CancellationReasonRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CancellationReason object
        /// </summary>
        /// <param name="cancellationReasonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CancellationReasonBase cancellationReasonObject, SqlDataReader reader, int start)
		{
			
				cancellationReasonObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) cancellationReasonObject.Reason = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) cancellationReasonObject.OrderBy = reader.GetInt32( start + 2 );			
				cancellationReasonObject.CompanyId = reader.GetGuid( start + 3 );			
				cancellationReasonObject.IsActive = reader.GetBoolean( start + 4 );			
			FillBaseObject(cancellationReasonObject, reader, (start + 5));

			
			cancellationReasonObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CancellationReason object
        /// </summary>
        /// <param name="cancellationReasonObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CancellationReasonBase cancellationReasonObject, SqlDataReader reader)
		{
			FillObject(cancellationReasonObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CancellationReason object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CancellationReason object</returns>
		private CancellationReason GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CancellationReason cancellationReasonObject= new CancellationReason();
					FillObject(cancellationReasonObject, reader);
					return cancellationReasonObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CancellationReason objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CancellationReason objects</returns>
		private CancellationReasonList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CancellationReason list
			CancellationReasonList list = new CancellationReasonList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CancellationReason cancellationReasonObject = new CancellationReason();
					FillObject(cancellationReasonObject, reader);

					list.Add(cancellationReasonObject);
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
