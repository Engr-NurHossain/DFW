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
	public partial class CredentialSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCREDENTIALSETTING = "InsertCredentialSetting";
		private const string UPDATECREDENTIALSETTING = "UpdateCredentialSetting";
		private const string DELETECREDENTIALSETTING = "DeleteCredentialSetting";
		private const string GETCREDENTIALSETTINGBYID = "GetCredentialSettingById";
		private const string GETALLCREDENTIALSETTING = "GetAllCredentialSetting";
		private const string GETPAGEDCREDENTIALSETTING = "GetPagedCredentialSetting";
		private const string GETCREDENTIALSETTINGMAXIMUMID = "GetCredentialSettingMaximumId";
		private const string GETCREDENTIALSETTINGROWCOUNT = "GetCredentialSettingRowCount";	
		private const string GETCREDENTIALSETTINGBYQUERY = "GetCredentialSettingByQuery";
		#endregion
		
		#region Constructors
		public CredentialSettingDataAccess(ClientContext context) : base(context) { }
		public CredentialSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="credentialSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, CredentialSettingBase credentialSettingObject)
		{	
			AddParameter(cmd, pGuid(CredentialSettingBase.Property_CompanyId, credentialSettingObject.CompanyId));
			AddParameter(cmd, pInt32(CredentialSettingBase.Property_AcountHolderId, credentialSettingObject.AcountHolderId));
			AddParameter(cmd, pNVarChar(CredentialSettingBase.Property_UserName, 250, credentialSettingObject.UserName));
			AddParameter(cmd, pNVarChar(CredentialSettingBase.Property_Password, 250, credentialSettingObject.Password));
			AddParameter(cmd, pNVarChar(CredentialSettingBase.Property_Description, 250, credentialSettingObject.Description));
			AddParameter(cmd, pBool(CredentialSettingBase.Property_IsActive, credentialSettingObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CredentialSetting
        /// </summary>
        /// <param name="credentialSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CredentialSettingBase credentialSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCREDENTIALSETTING);
	
				AddParameter(cmd, pInt32Out(CredentialSettingBase.Property_Id));
				AddCommonParams(cmd, credentialSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					credentialSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					credentialSettingObject.Id = (Int32)GetOutParameter(cmd, CredentialSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(credentialSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CredentialSetting
        /// </summary>
        /// <param name="credentialSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CredentialSettingBase credentialSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECREDENTIALSETTING);
				
				AddParameter(cmd, pInt32(CredentialSettingBase.Property_Id, credentialSettingObject.Id));
				AddCommonParams(cmd, credentialSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					credentialSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(credentialSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CredentialSetting
        /// </summary>
        /// <param name="Id">Id of the CredentialSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECREDENTIALSETTING);	
				
				AddParameter(cmd, pInt32(CredentialSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CredentialSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CredentialSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CredentialSetting object to retrieve</param>
        /// <returns>CredentialSetting object, null if not found</returns>
		public CredentialSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDENTIALSETTINGBYID))
			{
				AddParameter( cmd, pInt32(CredentialSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CredentialSetting objects 
        /// </summary>
        /// <returns>A list of CredentialSetting objects</returns>
		public CredentialSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCREDENTIALSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CredentialSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of CredentialSetting objects</returns>
		public CredentialSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCREDENTIALSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CredentialSettingList _CredentialSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CredentialSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all CredentialSetting objects by query String
        /// </summary>
        /// <returns>A list of CredentialSetting objects</returns>
		public CredentialSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCREDENTIALSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CredentialSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CredentialSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDENTIALSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CredentialSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CredentialSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CredentialSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCREDENTIALSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_CredentialSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CredentialSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CredentialSetting object
        /// </summary>
        /// <param name="credentialSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CredentialSettingBase credentialSettingObject, SqlDataReader reader, int start)
		{
			
				credentialSettingObject.Id = reader.GetInt32( start + 0 );			
				credentialSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				credentialSettingObject.AcountHolderId = reader.GetInt32( start + 2 );			
				credentialSettingObject.UserName = reader.GetString( start + 3 );			
				credentialSettingObject.Password = reader.GetString( start + 4 );			
				credentialSettingObject.Description = reader.GetString( start + 5 );			
				credentialSettingObject.IsActive = reader.GetBoolean( start + 6 );			
			FillBaseObject(credentialSettingObject, reader, (start + 7));

			
			credentialSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CredentialSetting object
        /// </summary>
        /// <param name="credentialSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CredentialSettingBase credentialSettingObject, SqlDataReader reader)
		{
			FillObject(credentialSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CredentialSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CredentialSetting object</returns>
		private CredentialSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CredentialSetting credentialSettingObject= new CredentialSetting();
					FillObject(credentialSettingObject, reader);
					return credentialSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CredentialSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CredentialSetting objects</returns>
		private CredentialSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CredentialSetting list
			CredentialSettingList list = new CredentialSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CredentialSetting credentialSettingObject = new CredentialSetting();
					FillObject(credentialSettingObject, reader);

					list.Add(credentialSettingObject);
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
