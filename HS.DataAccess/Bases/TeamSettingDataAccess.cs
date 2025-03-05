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
	public partial class TeamSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTEAMSETTING = "InsertTeamSetting";
		private const string UPDATETEAMSETTING = "UpdateTeamSetting";
		private const string DELETETEAMSETTING = "DeleteTeamSetting";
		private const string GETTEAMSETTINGBYID = "GetTeamSettingById";
		private const string GETALLTEAMSETTING = "GetAllTeamSetting";
		private const string GETPAGEDTEAMSETTING = "GetPagedTeamSetting";
		private const string GETTEAMSETTINGMAXIMUMID = "GetTeamSettingMaximumId";
		private const string GETTEAMSETTINGROWCOUNT = "GetTeamSettingRowCount";	
		private const string GETTEAMSETTINGBYQUERY = "GetTeamSettingByQuery";
		#endregion
		
		#region Constructors
		public TeamSettingDataAccess(ClientContext context) : base(context) { }
		public TeamSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="teamSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, TeamSettingBase teamSettingObject)
		{	
			AddParameter(cmd, pNVarChar(TeamSettingBase.Property_Name, 100, teamSettingObject.Name));
			AddParameter(cmd, pNVarChar(TeamSettingBase.Property_UserId, teamSettingObject.UserId));
			AddParameter(cmd, pGuid(TeamSettingBase.Property_CreatedBy, teamSettingObject.CreatedBy));
			AddParameter(cmd, pDateTime(TeamSettingBase.Property_CreatedDate, teamSettingObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TeamSetting
        /// </summary>
        /// <param name="teamSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TeamSettingBase teamSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTEAMSETTING);
	
				AddParameter(cmd, pInt32Out(TeamSettingBase.Property_Id));
				AddCommonParams(cmd, teamSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					teamSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					teamSettingObject.Id = (Int32)GetOutParameter(cmd, TeamSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(teamSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TeamSetting
        /// </summary>
        /// <param name="teamSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TeamSettingBase teamSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETEAMSETTING);
				
				AddParameter(cmd, pInt32(TeamSettingBase.Property_Id, teamSettingObject.Id));
				AddCommonParams(cmd, teamSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					teamSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(teamSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TeamSetting
        /// </summary>
        /// <param name="Id">Id of the TeamSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETEAMSETTING);	
				
				AddParameter(cmd, pInt32(TeamSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TeamSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TeamSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TeamSetting object to retrieve</param>
        /// <returns>TeamSetting object, null if not found</returns>
		public TeamSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTEAMSETTINGBYID))
			{
				AddParameter( cmd, pInt32(TeamSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TeamSetting objects 
        /// </summary>
        /// <returns>A list of TeamSetting objects</returns>
		public TeamSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTEAMSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TeamSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of TeamSetting objects</returns>
		public TeamSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTEAMSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TeamSettingList _TeamSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TeamSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all TeamSetting objects by query String
        /// </summary>
        /// <returns>A list of TeamSetting objects</returns>
		public TeamSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTEAMSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TeamSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TeamSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTEAMSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TeamSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TeamSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TeamSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTEAMSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_TeamSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TeamSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TeamSetting object
        /// </summary>
        /// <param name="teamSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TeamSettingBase teamSettingObject, SqlDataReader reader, int start)
		{
			
				teamSettingObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) teamSettingObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) teamSettingObject.UserId = reader.GetString( start + 2 );			
				teamSettingObject.CreatedBy = reader.GetGuid( start + 3 );			
				teamSettingObject.CreatedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(teamSettingObject, reader, (start + 5));

			
			teamSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TeamSetting object
        /// </summary>
        /// <param name="teamSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TeamSettingBase teamSettingObject, SqlDataReader reader)
		{
			FillObject(teamSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TeamSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TeamSetting object</returns>
		private TeamSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TeamSetting teamSettingObject= new TeamSetting();
					FillObject(teamSettingObject, reader);
					return teamSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TeamSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TeamSetting objects</returns>
		private TeamSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TeamSetting list
			TeamSettingList list = new TeamSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TeamSetting teamSettingObject = new TeamSetting();
					FillObject(teamSettingObject, reader);

					list.Add(teamSettingObject);
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
