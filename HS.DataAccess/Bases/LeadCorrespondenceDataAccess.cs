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
	public partial class LeadCorrespondenceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTLEADCORRESPONDENCE = "InsertLeadCorrespondence";
		private const string UPDATELEADCORRESPONDENCE = "UpdateLeadCorrespondence";
		private const string DELETELEADCORRESPONDENCE = "DeleteLeadCorrespondence";
		private const string GETLEADCORRESPONDENCEBYID = "GetLeadCorrespondenceById";
		private const string GETALLLEADCORRESPONDENCE = "GetAllLeadCorrespondence";
		private const string GETPAGEDLEADCORRESPONDENCE = "GetPagedLeadCorrespondence";
		private const string GETLEADCORRESPONDENCEMAXIMUMID = "GetLeadCorrespondenceMaximumId";
		private const string GETLEADCORRESPONDENCEROWCOUNT = "GetLeadCorrespondenceRowCount";	
		private const string GETLEADCORRESPONDENCEBYQUERY = "GetLeadCorrespondenceByQuery";
		#endregion
		
		#region Constructors
		public LeadCorrespondenceDataAccess(ClientContext context) : base(context) { }
		public LeadCorrespondenceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="leadCorrespondenceObject"></param>
		private void AddCommonParams(SqlCommand cmd, LeadCorrespondenceBase leadCorrespondenceObject)
		{	
			AddParameter(cmd, pGuid(LeadCorrespondenceBase.Property_CompanyId, leadCorrespondenceObject.CompanyId));
			AddParameter(cmd, pGuid(LeadCorrespondenceBase.Property_CustomerId, leadCorrespondenceObject.CustomerId));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_TemplateKey, 250, leadCorrespondenceObject.TemplateKey));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_Type, 50, leadCorrespondenceObject.Type));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_ToEmail, 250, leadCorrespondenceObject.ToEmail));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_CcEmail, 250, leadCorrespondenceObject.CcEmail));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_BccEmail, 250, leadCorrespondenceObject.BccEmail));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_FromEmail, 250, leadCorrespondenceObject.FromEmail));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_ToMobileNo, 250, leadCorrespondenceObject.ToMobileNo));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_FromMobileNo, 250, leadCorrespondenceObject.FromMobileNo));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_FromName, 250, leadCorrespondenceObject.FromName));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_Subject, 250, leadCorrespondenceObject.Subject));
			AddParameter(cmd, pNVarChar(LeadCorrespondenceBase.Property_BodyContent, leadCorrespondenceObject.BodyContent));
			AddParameter(cmd, pDateTime(LeadCorrespondenceBase.Property_SentDate, leadCorrespondenceObject.SentDate));
			AddParameter(cmd, pBool(LeadCorrespondenceBase.Property_IsSystemAutoSent, leadCorrespondenceObject.IsSystemAutoSent));
			AddParameter(cmd, pBool(LeadCorrespondenceBase.Property_IsRead, leadCorrespondenceObject.IsRead));
			AddParameter(cmd, pInt32(LeadCorrespondenceBase.Property_ReadCount, leadCorrespondenceObject.ReadCount));
			AddParameter(cmd, pDateTime(LeadCorrespondenceBase.Property_LastUpdatedDate, leadCorrespondenceObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(LeadCorrespondenceBase.Property_SentBy, leadCorrespondenceObject.SentBy));
			AddParameter(cmd, pInt32(LeadCorrespondenceBase.Property_FileId, leadCorrespondenceObject.FileId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts LeadCorrespondence
        /// </summary>
        /// <param name="leadCorrespondenceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(LeadCorrespondenceBase leadCorrespondenceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTLEADCORRESPONDENCE);
	
				AddParameter(cmd, pInt32Out(LeadCorrespondenceBase.Property_Id));
				AddCommonParams(cmd, leadCorrespondenceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					leadCorrespondenceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					leadCorrespondenceObject.Id = (Int32)GetOutParameter(cmd, LeadCorrespondenceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(leadCorrespondenceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates LeadCorrespondence
        /// </summary>
        /// <param name="leadCorrespondenceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(LeadCorrespondenceBase leadCorrespondenceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATELEADCORRESPONDENCE);
				
				AddParameter(cmd, pInt32(LeadCorrespondenceBase.Property_Id, leadCorrespondenceObject.Id));
				AddCommonParams(cmd, leadCorrespondenceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					leadCorrespondenceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(leadCorrespondenceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes LeadCorrespondence
        /// </summary>
        /// <param name="Id">Id of the LeadCorrespondence object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETELEADCORRESPONDENCE);	
				
				AddParameter(cmd, pInt32(LeadCorrespondenceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(LeadCorrespondence), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves LeadCorrespondence object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the LeadCorrespondence object to retrieve</param>
        /// <returns>LeadCorrespondence object, null if not found</returns>
		public LeadCorrespondence Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETLEADCORRESPONDENCEBYID))
			{
				AddParameter( cmd, pInt32(LeadCorrespondenceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all LeadCorrespondence objects 
        /// </summary>
        /// <returns>A list of LeadCorrespondence objects</returns>
		public LeadCorrespondenceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLLEADCORRESPONDENCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all LeadCorrespondence objects by PageRequest
        /// </summary>
        /// <returns>A list of LeadCorrespondence objects</returns>
		public LeadCorrespondenceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDLEADCORRESPONDENCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				LeadCorrespondenceList _LeadCorrespondenceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _LeadCorrespondenceList;
			}
		}
		
		/// <summary>
        /// Retrieves all LeadCorrespondence objects by query String
        /// </summary>
        /// <returns>A list of LeadCorrespondence objects</returns>
		public LeadCorrespondenceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETLEADCORRESPONDENCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get LeadCorrespondence Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of LeadCorrespondence
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLEADCORRESPONDENCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get LeadCorrespondence Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of LeadCorrespondence
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _LeadCorrespondenceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETLEADCORRESPONDENCEROWCOUNT))
			{
				SqlDataReader reader;
				_LeadCorrespondenceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _LeadCorrespondenceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills LeadCorrespondence object
        /// </summary>
        /// <param name="leadCorrespondenceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(LeadCorrespondenceBase leadCorrespondenceObject, SqlDataReader reader, int start)
		{
			
				leadCorrespondenceObject.Id = reader.GetInt32( start + 0 );			
				leadCorrespondenceObject.CompanyId = reader.GetGuid( start + 1 );			
				leadCorrespondenceObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) leadCorrespondenceObject.TemplateKey = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) leadCorrespondenceObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) leadCorrespondenceObject.ToEmail = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) leadCorrespondenceObject.CcEmail = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) leadCorrespondenceObject.BccEmail = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) leadCorrespondenceObject.FromEmail = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) leadCorrespondenceObject.ToMobileNo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) leadCorrespondenceObject.FromMobileNo = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) leadCorrespondenceObject.FromName = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) leadCorrespondenceObject.Subject = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) leadCorrespondenceObject.BodyContent = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) leadCorrespondenceObject.SentDate = reader.GetDateTime( start + 14 );			
				if(!reader.IsDBNull(15)) leadCorrespondenceObject.IsSystemAutoSent = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) leadCorrespondenceObject.IsRead = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) leadCorrespondenceObject.ReadCount = reader.GetInt32( start + 17 );			
				if(!reader.IsDBNull(18)) leadCorrespondenceObject.LastUpdatedDate = reader.GetDateTime( start + 18 );			
				leadCorrespondenceObject.SentBy = reader.GetGuid( start + 19 );			
				if(!reader.IsDBNull(20)) leadCorrespondenceObject.FileId = reader.GetInt32( start + 20 );			
			FillBaseObject(leadCorrespondenceObject, reader, (start + 21));

			
			leadCorrespondenceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills LeadCorrespondence object
        /// </summary>
        /// <param name="leadCorrespondenceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(LeadCorrespondenceBase leadCorrespondenceObject, SqlDataReader reader)
		{
			FillObject(leadCorrespondenceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves LeadCorrespondence object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>LeadCorrespondence object</returns>
		private LeadCorrespondence GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					LeadCorrespondence leadCorrespondenceObject= new LeadCorrespondence();
					FillObject(leadCorrespondenceObject, reader);
					return leadCorrespondenceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of LeadCorrespondence objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of LeadCorrespondence objects</returns>
		private LeadCorrespondenceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//LeadCorrespondence list
			LeadCorrespondenceList list = new LeadCorrespondenceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					LeadCorrespondence leadCorrespondenceObject = new LeadCorrespondence();
					FillObject(leadCorrespondenceObject, reader);

					list.Add(leadCorrespondenceObject);
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
