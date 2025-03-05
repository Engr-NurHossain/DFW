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
	public partial class SMSHistoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMSHISTORY = "InsertSMSHistory";
		private const string UPDATESMSHISTORY = "UpdateSMSHistory";
		private const string DELETESMSHISTORY = "DeleteSMSHistory";
		private const string GETSMSHISTORYBYID = "GetSMSHistoryById";
		private const string GETALLSMSHISTORY = "GetAllSMSHistory";
		private const string GETPAGEDSMSHISTORY = "GetPagedSMSHistory";
		private const string GETSMSHISTORYMAXIMUMID = "GetSMSHistoryMaximumId";
		private const string GETSMSHISTORYROWCOUNT = "GetSMSHistoryRowCount";	
		private const string GETSMSHISTORYBYQUERY = "GetSMSHistoryByQuery";
        #endregion

        #region Constructors
        public SMSHistoryDataAccess(string ConStr) : base(ConStr) { }
        public SMSHistoryDataAccess(ClientContext context) : base(context) { }
		public SMSHistoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="sMSHistoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, SMSHistoryBase sMSHistoryObject)
		{	
			AddParameter(cmd, pNVarChar(SMSHistoryBase.Property_TemplateKey, 250, sMSHistoryObject.TemplateKey));
			AddParameter(cmd, pNVarChar(SMSHistoryBase.Property_ToMobileNo, 250, sMSHistoryObject.ToMobileNo));
			AddParameter(cmd, pNVarChar(SMSHistoryBase.Property_FromMobileNo, 250, sMSHistoryObject.FromMobileNo));
			AddParameter(cmd, pNVarChar(SMSHistoryBase.Property_FromName, 250, sMSHistoryObject.FromName));
			AddParameter(cmd, pNVarChar(SMSHistoryBase.Property_SMSBodyContent, sMSHistoryObject.SMSBodyContent));
			AddParameter(cmd, pDateTime(SMSHistoryBase.Property_SMSSentDate, sMSHistoryObject.SMSSentDate));
			AddParameter(cmd, pBool(SMSHistoryBase.Property_IsSystemAutoSent, sMSHistoryObject.IsSystemAutoSent));
			AddParameter(cmd, pBool(SMSHistoryBase.Property_IsRead, sMSHistoryObject.IsRead));
			AddParameter(cmd, pInt32(SMSHistoryBase.Property_ReadCount, sMSHistoryObject.ReadCount));
			AddParameter(cmd, pDateTime(SMSHistoryBase.Property_LastUpdatedDate, sMSHistoryObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(SMSHistoryBase.Property_CompanyId, sMSHistoryObject.CompanyId));
			AddParameter(cmd, pGuid(SMSHistoryBase.Property_CreatedBy, sMSHistoryObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SMSHistory
        /// </summary>
        /// <param name="sMSHistoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SMSHistoryBase sMSHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMSHISTORY);
	
				AddParameter(cmd, pInt32Out(SMSHistoryBase.Property_Id));
				AddCommonParams(cmd, sMSHistoryObject);

                long result = InsertRecord(cmd);
				if (result > 0)
				{
					sMSHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					sMSHistoryObject.Id = (Int32)GetOutParameter(cmd, SMSHistoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(sMSHistoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SMSHistory
        /// </summary>
        /// <param name="sMSHistoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SMSHistoryBase sMSHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMSHISTORY);
				
				AddParameter(cmd, pInt32(SMSHistoryBase.Property_Id, sMSHistoryObject.Id));
				AddCommonParams(cmd, sMSHistoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					sMSHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(sMSHistoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SMSHistory
        /// </summary>
        /// <param name="Id">Id of the SMSHistory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMSHISTORY);	
				
				AddParameter(cmd, pInt32(SMSHistoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SMSHistory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SMSHistory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SMSHistory object to retrieve</param>
        /// <returns>SMSHistory object, null if not found</returns>
		public SMSHistory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMSHISTORYBYID))
			{
				AddParameter( cmd, pInt32(SMSHistoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SMSHistory objects 
        /// </summary>
        /// <returns>A list of SMSHistory objects</returns>
		public SMSHistoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMSHISTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SMSHistory objects by PageRequest
        /// </summary>
        /// <returns>A list of SMSHistory objects</returns>
		public SMSHistoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMSHISTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SMSHistoryList _SMSHistoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SMSHistoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all SMSHistory objects by query String
        /// </summary>
        /// <returns>A list of SMSHistory objects</returns>
		public SMSHistoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMSHISTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SMSHistory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SMSHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMSHISTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SMSHistory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SMSHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SMSHistoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMSHISTORYROWCOUNT))
			{
				SqlDataReader reader;
				_SMSHistoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SMSHistoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SMSHistory object
        /// </summary>
        /// <param name="sMSHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SMSHistoryBase sMSHistoryObject, SqlDataReader reader, int start)
		{
			
				sMSHistoryObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) sMSHistoryObject.TemplateKey = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) sMSHistoryObject.ToMobileNo = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) sMSHistoryObject.FromMobileNo = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) sMSHistoryObject.FromName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) sMSHistoryObject.SMSBodyContent = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) sMSHistoryObject.SMSSentDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) sMSHistoryObject.IsSystemAutoSent = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) sMSHistoryObject.IsRead = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) sMSHistoryObject.ReadCount = reader.GetInt32( start + 9 );			
				if(!reader.IsDBNull(10)) sMSHistoryObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				sMSHistoryObject.CompanyId = reader.GetGuid( start + 11 );			
				sMSHistoryObject.CreatedBy = reader.GetGuid( start + 12 );			
			FillBaseObject(sMSHistoryObject, reader, (start + 13));

			
			sMSHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SMSHistory object
        /// </summary>
        /// <param name="sMSHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SMSHistoryBase sMSHistoryObject, SqlDataReader reader)
		{
			FillObject(sMSHistoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SMSHistory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SMSHistory object</returns>
		private SMSHistory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SMSHistory sMSHistoryObject= new SMSHistory();
					FillObject(sMSHistoryObject, reader);
					return sMSHistoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SMSHistory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SMSHistory objects</returns>
		private SMSHistoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SMSHistory list
			SMSHistoryList list = new SMSHistoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SMSHistory sMSHistoryObject = new SMSHistory();
					FillObject(sMSHistoryObject, reader);

					list.Add(sMSHistoryObject);
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
