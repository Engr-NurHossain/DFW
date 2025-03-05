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
	public partial class AnnouncementDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTANNOUNCEMENT = "InsertAnnouncement";
		private const string UPDATEANNOUNCEMENT = "UpdateAnnouncement";
		private const string DELETEANNOUNCEMENT = "DeleteAnnouncement";
		private const string GETANNOUNCEMENTBYID = "GetAnnouncementById";
		private const string GETALLANNOUNCEMENT = "GetAllAnnouncement";
		private const string GETPAGEDANNOUNCEMENT = "GetPagedAnnouncement";
		private const string GETANNOUNCEMENTMAXIMUMID = "GetAnnouncementMaximumId";
		private const string GETANNOUNCEMENTROWCOUNT = "GetAnnouncementRowCount";	
		private const string GETANNOUNCEMENTBYQUERY = "GetAnnouncementByQuery";
		#endregion
		
		#region Constructors
		public AnnouncementDataAccess(ClientContext context) : base(context) { }
		public AnnouncementDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="announcementObject"></param>
		private void AddCommonParams(SqlCommand cmd, AnnouncementBase announcementObject)
		{	
			AddParameter(cmd, pGuid(AnnouncementBase.Property_CompanyId, announcementObject.CompanyId));
			AddParameter(cmd, pNVarChar(AnnouncementBase.Property_Title, 250, announcementObject.Title));
			AddParameter(cmd, pNVarChar(AnnouncementBase.Property_Message, announcementObject.Message));
			AddParameter(cmd, pDateTime(AnnouncementBase.Property_StartTime, announcementObject.StartTime));
			AddParameter(cmd, pDateTime(AnnouncementBase.Property_EndTime, announcementObject.EndTime));
			AddParameter(cmd, pGuid(AnnouncementBase.Property_CreatedBy, announcementObject.CreatedBy));
			AddParameter(cmd, pDateTime(AnnouncementBase.Property_CreatedDate, announcementObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Announcement
        /// </summary>
        /// <param name="announcementObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AnnouncementBase announcementObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTANNOUNCEMENT);
	
				AddParameter(cmd, pInt32Out(AnnouncementBase.Property_Id));
				AddCommonParams(cmd, announcementObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					announcementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					announcementObject.Id = (Int32)GetOutParameter(cmd, AnnouncementBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(announcementObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Announcement
        /// </summary>
        /// <param name="announcementObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AnnouncementBase announcementObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEANNOUNCEMENT);
				
				AddParameter(cmd, pInt32(AnnouncementBase.Property_Id, announcementObject.Id));
				AddCommonParams(cmd, announcementObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					announcementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(announcementObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Announcement
        /// </summary>
        /// <param name="Id">Id of the Announcement object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEANNOUNCEMENT);	
				
				AddParameter(cmd, pInt32(AnnouncementBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Announcement), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Announcement object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Announcement object to retrieve</param>
        /// <returns>Announcement object, null if not found</returns>
		public Announcement Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETANNOUNCEMENTBYID))
			{
				AddParameter( cmd, pInt32(AnnouncementBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Announcement objects 
        /// </summary>
        /// <returns>A list of Announcement objects</returns>
		public AnnouncementList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLANNOUNCEMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Announcement objects by PageRequest
        /// </summary>
        /// <returns>A list of Announcement objects</returns>
		public AnnouncementList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDANNOUNCEMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AnnouncementList _AnnouncementList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AnnouncementList;
			}
		}
		
		/// <summary>
        /// Retrieves all Announcement objects by query String
        /// </summary>
        /// <returns>A list of Announcement objects</returns>
		public AnnouncementList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETANNOUNCEMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Announcement Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Announcement
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETANNOUNCEMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Announcement Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Announcement
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AnnouncementRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETANNOUNCEMENTROWCOUNT))
			{
				SqlDataReader reader;
				_AnnouncementRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AnnouncementRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Announcement object
        /// </summary>
        /// <param name="announcementObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AnnouncementBase announcementObject, SqlDataReader reader, int start)
		{
			
				announcementObject.Id = reader.GetInt32( start + 0 );			
				announcementObject.CompanyId = reader.GetGuid( start + 1 );			
				announcementObject.Title = reader.GetString( start + 2 );			
				announcementObject.Message = reader.GetString( start + 3 );			
				announcementObject.StartTime = reader.GetDateTime( start + 4 );			
				announcementObject.EndTime = reader.GetDateTime( start + 5 );			
				announcementObject.CreatedBy = reader.GetGuid( start + 6 );			
				announcementObject.CreatedDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(announcementObject, reader, (start + 8));

			
			announcementObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Announcement object
        /// </summary>
        /// <param name="announcementObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AnnouncementBase announcementObject, SqlDataReader reader)
		{
			FillObject(announcementObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Announcement object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Announcement object</returns>
		private Announcement GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Announcement announcementObject= new Announcement();
					FillObject(announcementObject, reader);
					return announcementObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Announcement objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Announcement objects</returns>
		private AnnouncementList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Announcement list
			AnnouncementList list = new AnnouncementList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Announcement announcementObject = new Announcement();
					FillObject(announcementObject, reader);

					list.Add(announcementObject);
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
