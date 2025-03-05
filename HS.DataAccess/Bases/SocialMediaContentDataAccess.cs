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
	public partial class SocialMediaContentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSOCIALMEDIACONTENT = "InsertSocialMediaContent";
		private const string UPDATESOCIALMEDIACONTENT = "UpdateSocialMediaContent";
		private const string DELETESOCIALMEDIACONTENT = "DeleteSocialMediaContent";
		private const string GETSOCIALMEDIACONTENTBYID = "GetSocialMediaContentById";
		private const string GETALLSOCIALMEDIACONTENT = "GetAllSocialMediaContent";
		private const string GETPAGEDSOCIALMEDIACONTENT = "GetPagedSocialMediaContent";
		private const string GETSOCIALMEDIACONTENTMAXIMUMID = "GetSocialMediaContentMaximumId";
		private const string GETSOCIALMEDIACONTENTROWCOUNT = "GetSocialMediaContentRowCount";	
		private const string GETSOCIALMEDIACONTENTBYQUERY = "GetSocialMediaContentByQuery";
		#endregion
		
		#region Constructors
		public SocialMediaContentDataAccess(ClientContext context) : base(context) { }
		public SocialMediaContentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="socialMediaContentObject"></param>
		private void AddCommonParams(SqlCommand cmd, SocialMediaContentBase socialMediaContentObject)
		{	
			AddParameter(cmd, pGuid(SocialMediaContentBase.Property_CompanyId, socialMediaContentObject.CompanyId));
			AddParameter(cmd, pNVarChar(SocialMediaContentBase.Property_Name, 250, socialMediaContentObject.Name));
			AddParameter(cmd, pNVarChar(SocialMediaContentBase.Property_FollowUpLink, socialMediaContentObject.FollowUpLink));
			AddParameter(cmd, pNVarChar(SocialMediaContentBase.Property_ShareLink, socialMediaContentObject.ShareLink));
			AddParameter(cmd, pNVarChar(SocialMediaContentBase.Property_ImageLink, socialMediaContentObject.ImageLink));
			AddParameter(cmd, pDateTime(SocialMediaContentBase.Property_CreatedDate, socialMediaContentObject.CreatedDate));
			AddParameter(cmd, pGuid(SocialMediaContentBase.Property_CreatedBy, socialMediaContentObject.CreatedBy));
			AddParameter(cmd, pGuid(SocialMediaContentBase.Property_LastUpdatedBy, socialMediaContentObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(SocialMediaContentBase.Property_LastUpdatedDate, socialMediaContentObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SocialMediaContent
        /// </summary>
        /// <param name="socialMediaContentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SocialMediaContentBase socialMediaContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSOCIALMEDIACONTENT);
	
				AddParameter(cmd, pInt32Out(SocialMediaContentBase.Property_Id));
				AddCommonParams(cmd, socialMediaContentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					socialMediaContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					socialMediaContentObject.Id = (Int32)GetOutParameter(cmd, SocialMediaContentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(socialMediaContentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SocialMediaContent
        /// </summary>
        /// <param name="socialMediaContentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SocialMediaContentBase socialMediaContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESOCIALMEDIACONTENT);
				
				AddParameter(cmd, pInt32(SocialMediaContentBase.Property_Id, socialMediaContentObject.Id));
				AddCommonParams(cmd, socialMediaContentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					socialMediaContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(socialMediaContentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SocialMediaContent
        /// </summary>
        /// <param name="Id">Id of the SocialMediaContent object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESOCIALMEDIACONTENT);	
				
				AddParameter(cmd, pInt32(SocialMediaContentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SocialMediaContent), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SocialMediaContent object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SocialMediaContent object to retrieve</param>
        /// <returns>SocialMediaContent object, null if not found</returns>
		public SocialMediaContent Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSOCIALMEDIACONTENTBYID))
			{
				AddParameter( cmd, pInt32(SocialMediaContentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SocialMediaContent objects 
        /// </summary>
        /// <returns>A list of SocialMediaContent objects</returns>
		public SocialMediaContentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSOCIALMEDIACONTENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SocialMediaContent objects by PageRequest
        /// </summary>
        /// <returns>A list of SocialMediaContent objects</returns>
		public SocialMediaContentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSOCIALMEDIACONTENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SocialMediaContentList _SocialMediaContentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SocialMediaContentList;
			}
		}
		
		/// <summary>
        /// Retrieves all SocialMediaContent objects by query String
        /// </summary>
        /// <returns>A list of SocialMediaContent objects</returns>
		public SocialMediaContentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSOCIALMEDIACONTENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SocialMediaContent Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SocialMediaContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSOCIALMEDIACONTENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SocialMediaContent Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SocialMediaContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SocialMediaContentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSOCIALMEDIACONTENTROWCOUNT))
			{
				SqlDataReader reader;
				_SocialMediaContentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SocialMediaContentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SocialMediaContent object
        /// </summary>
        /// <param name="socialMediaContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SocialMediaContentBase socialMediaContentObject, SqlDataReader reader, int start)
		{
			
				socialMediaContentObject.Id = reader.GetInt32( start + 0 );			
				socialMediaContentObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) socialMediaContentObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) socialMediaContentObject.FollowUpLink = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) socialMediaContentObject.ShareLink = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) socialMediaContentObject.ImageLink = reader.GetString( start + 5 );			
				socialMediaContentObject.CreatedDate = reader.GetDateTime( start + 6 );			
				socialMediaContentObject.CreatedBy = reader.GetGuid( start + 7 );			
				socialMediaContentObject.LastUpdatedBy = reader.GetGuid( start + 8 );			
				socialMediaContentObject.LastUpdatedDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(socialMediaContentObject, reader, (start + 10));

			
			socialMediaContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SocialMediaContent object
        /// </summary>
        /// <param name="socialMediaContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SocialMediaContentBase socialMediaContentObject, SqlDataReader reader)
		{
			FillObject(socialMediaContentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SocialMediaContent object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SocialMediaContent object</returns>
		private SocialMediaContent GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SocialMediaContent socialMediaContentObject= new SocialMediaContent();
					FillObject(socialMediaContentObject, reader);
					return socialMediaContentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SocialMediaContent objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SocialMediaContent objects</returns>
		private SocialMediaContentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SocialMediaContent list
			SocialMediaContentList list = new SocialMediaContentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SocialMediaContent socialMediaContentObject = new SocialMediaContent();
					FillObject(socialMediaContentObject, reader);

					list.Add(socialMediaContentObject);
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
