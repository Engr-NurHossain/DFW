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
	public partial class SiteContactDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSITECONTACT = "InsertSiteContact";
		private const string UPDATESITECONTACT = "UpdateSiteContact";
		private const string DELETESITECONTACT = "DeleteSiteContact";
		private const string GETSITECONTACTBYID = "GetSiteContactById";
		private const string GETALLSITECONTACT = "GetAllSiteContact";
		private const string GETPAGEDSITECONTACT = "GetPagedSiteContact";
		private const string GETSITECONTACTMAXIMUMID = "GetSiteContactMaximumId";
		private const string GETSITECONTACTROWCOUNT = "GetSiteContactRowCount";	
		private const string GETSITECONTACTBYQUERY = "GetSiteContactByQuery";
		#endregion
		
		#region Constructors
		public SiteContactDataAccess(ClientContext context) : base(context) { }
		public SiteContactDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="siteContactObject"></param>
		private void AddCommonParams(SqlCommand cmd, SiteContactBase siteContactObject)
		{	
			AddParameter(cmd, pGuid(SiteContactBase.Property_ContactId, siteContactObject.ContactId));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_FirstName, 50, siteContactObject.FirstName));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_LastName, 50, siteContactObject.LastName));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_CellPhone, 50, siteContactObject.CellPhone));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_WorkPhone, 50, siteContactObject.WorkPhone));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_Email, 50, siteContactObject.Email));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_Title, 50, siteContactObject.Title));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_ContactType, 50, siteContactObject.ContactType));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_AccessLevel, 50, siteContactObject.AccessLevel));
			AddParameter(cmd, pDateTime(SiteContactBase.Property_CreatedDate, siteContactObject.CreatedDate));
			AddParameter(cmd, pGuid(SiteContactBase.Property_CreatedBy, siteContactObject.CreatedBy));
			AddParameter(cmd, pGuid(SiteContactBase.Property_LastUpdatedBy, siteContactObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(SiteContactBase.Property_LastUpdatedDate, siteContactObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(SiteContactBase.Property_FileLocation, 500, siteContactObject.FileLocation));
			AddParameter(cmd, pGuid(SiteContactBase.Property_CompanyId, siteContactObject.CompanyId));
			AddParameter(cmd, pBool(SiteContactBase.Property_IsEmail, siteContactObject.IsEmail));
			AddParameter(cmd, pBool(SiteContactBase.Property_IsText, siteContactObject.IsText));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SiteContact
        /// </summary>
        /// <param name="siteContactObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SiteContactBase siteContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSITECONTACT);
	
				AddParameter(cmd, pInt32Out(SiteContactBase.Property_Id));
				AddCommonParams(cmd, siteContactObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					siteContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					siteContactObject.Id = (Int32)GetOutParameter(cmd, SiteContactBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(siteContactObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SiteContact
        /// </summary>
        /// <param name="siteContactObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SiteContactBase siteContactObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESITECONTACT);
				
				AddParameter(cmd, pInt32(SiteContactBase.Property_Id, siteContactObject.Id));
				AddCommonParams(cmd, siteContactObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					siteContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(siteContactObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SiteContact
        /// </summary>
        /// <param name="Id">Id of the SiteContact object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESITECONTACT);	
				
				AddParameter(cmd, pInt32(SiteContactBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SiteContact), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SiteContact object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SiteContact object to retrieve</param>
        /// <returns>SiteContact object, null if not found</returns>
		public SiteContact Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSITECONTACTBYID))
			{
				AddParameter( cmd, pInt32(SiteContactBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SiteContact objects 
        /// </summary>
        /// <returns>A list of SiteContact objects</returns>
		public SiteContactList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSITECONTACT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SiteContact objects by PageRequest
        /// </summary>
        /// <returns>A list of SiteContact objects</returns>
		public SiteContactList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSITECONTACT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SiteContactList _SiteContactList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SiteContactList;
			}
		}
		
		/// <summary>
        /// Retrieves all SiteContact objects by query String
        /// </summary>
        /// <returns>A list of SiteContact objects</returns>
		public SiteContactList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSITECONTACTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SiteContact Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SiteContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSITECONTACTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SiteContact Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SiteContact
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SiteContactRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSITECONTACTROWCOUNT))
			{
				SqlDataReader reader;
				_SiteContactRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SiteContactRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SiteContact object
        /// </summary>
        /// <param name="siteContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SiteContactBase siteContactObject, SqlDataReader reader, int start)
		{
			
				siteContactObject.Id = reader.GetInt32( start + 0 );			
				siteContactObject.ContactId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) siteContactObject.FirstName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) siteContactObject.LastName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) siteContactObject.CellPhone = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) siteContactObject.WorkPhone = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) siteContactObject.Email = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) siteContactObject.Title = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) siteContactObject.ContactType = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) siteContactObject.AccessLevel = reader.GetString( start + 9 );			
				siteContactObject.CreatedDate = reader.GetDateTime( start + 10 );			
				siteContactObject.CreatedBy = reader.GetGuid( start + 11 );			
				siteContactObject.LastUpdatedBy = reader.GetGuid( start + 12 );			
				siteContactObject.LastUpdatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) siteContactObject.FileLocation = reader.GetString( start + 14 );			
				siteContactObject.CompanyId = reader.GetGuid( start + 15 );			
				if(!reader.IsDBNull(16)) siteContactObject.IsEmail = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) siteContactObject.IsText = reader.GetBoolean( start + 17 );			
			FillBaseObject(siteContactObject, reader, (start + 18));

			
			siteContactObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SiteContact object
        /// </summary>
        /// <param name="siteContactObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SiteContactBase siteContactObject, SqlDataReader reader)
		{
			FillObject(siteContactObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SiteContact object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SiteContact object</returns>
		private SiteContact GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SiteContact siteContactObject= new SiteContact();
					FillObject(siteContactObject, reader);
					return siteContactObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SiteContact objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SiteContact objects</returns>
		private SiteContactList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SiteContact list
			SiteContactList list = new SiteContactList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SiteContact siteContactObject = new SiteContact();
					FillObject(siteContactObject, reader);

					list.Add(siteContactObject);
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
