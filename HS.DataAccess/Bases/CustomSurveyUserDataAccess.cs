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
	public partial class CustomSurveyUserDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMSURVEYUSER = "InsertCustomSurveyUser";
		private const string UPDATECUSTOMSURVEYUSER = "UpdateCustomSurveyUser";
		private const string DELETECUSTOMSURVEYUSER = "DeleteCustomSurveyUser";
		private const string GETCUSTOMSURVEYUSERBYID = "GetCustomSurveyUserById";
		private const string GETALLCUSTOMSURVEYUSER = "GetAllCustomSurveyUser";
		private const string GETPAGEDCUSTOMSURVEYUSER = "GetPagedCustomSurveyUser";
		private const string GETCUSTOMSURVEYUSERMAXIMUMID = "GetCustomSurveyUserMaximumId";
		private const string GETCUSTOMSURVEYUSERROWCOUNT = "GetCustomSurveyUserRowCount";	
		private const string GETCUSTOMSURVEYUSERBYQUERY = "GetCustomSurveyUserByQuery";
		#endregion
		
		#region Constructors
		public CustomSurveyUserDataAccess(ClientContext context) : base(context) { }
		public CustomSurveyUserDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customSurveyUserObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomSurveyUserBase customSurveyUserObject)
		{	
			AddParameter(cmd, pGuid(CustomSurveyUserBase.Property_SurveyId, customSurveyUserObject.SurveyId));
			AddParameter(cmd, pGuid(CustomSurveyUserBase.Property_UserId, customSurveyUserObject.UserId));
			AddParameter(cmd, pGuid(CustomSurveyUserBase.Property_SurveyUserId, customSurveyUserObject.SurveyUserId));
			AddParameter(cmd, pGuid(CustomSurveyUserBase.Property_AddedBy, customSurveyUserObject.AddedBy));
			AddParameter(cmd, pDateTime(CustomSurveyUserBase.Property_AddedDate, customSurveyUserObject.AddedDate));
			AddParameter(cmd, pNVarChar(CustomSurveyUserBase.Property_Status, 50, customSurveyUserObject.Status));
			AddParameter(cmd, pNVarChar(CustomSurveyUserBase.Property_ReferenceId, 50, customSurveyUserObject.ReferenceId));
			AddParameter(cmd, pDateTime(CustomSurveyUserBase.Property_ViewedDate, customSurveyUserObject.ViewedDate));
			AddParameter(cmd, pNVarChar(CustomSurveyUserBase.Property_UserIP, 50, customSurveyUserObject.UserIP));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomSurveyUser
        /// </summary>
        /// <param name="customSurveyUserObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomSurveyUserBase customSurveyUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMSURVEYUSER);
	
				AddParameter(cmd, pInt32Out(CustomSurveyUserBase.Property_Id));
				AddCommonParams(cmd, customSurveyUserObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customSurveyUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customSurveyUserObject.Id = (Int32)GetOutParameter(cmd, CustomSurveyUserBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customSurveyUserObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomSurveyUser
        /// </summary>
        /// <param name="customSurveyUserObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomSurveyUserBase customSurveyUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMSURVEYUSER);
				
				AddParameter(cmd, pInt32(CustomSurveyUserBase.Property_Id, customSurveyUserObject.Id));
				AddCommonParams(cmd, customSurveyUserObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customSurveyUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customSurveyUserObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomSurveyUser
        /// </summary>
        /// <param name="Id">Id of the CustomSurveyUser object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMSURVEYUSER);	
				
				AddParameter(cmd, pInt32(CustomSurveyUserBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomSurveyUser), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomSurveyUser object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomSurveyUser object to retrieve</param>
        /// <returns>CustomSurveyUser object, null if not found</returns>
		public CustomSurveyUser Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERBYID))
			{
				AddParameter( cmd, pInt32(CustomSurveyUserBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomSurveyUser objects 
        /// </summary>
        /// <returns>A list of CustomSurveyUser objects</returns>
		public CustomSurveyUserList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMSURVEYUSER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomSurveyUser objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomSurveyUser objects</returns>
		public CustomSurveyUserList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMSURVEYUSER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomSurveyUserList _CustomSurveyUserList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomSurveyUserList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomSurveyUser objects by query String
        /// </summary>
        /// <returns>A list of CustomSurveyUser objects</returns>
		public CustomSurveyUserList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomSurveyUser Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomSurveyUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomSurveyUser Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomSurveyUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomSurveyUserRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMSURVEYUSERROWCOUNT))
			{
				SqlDataReader reader;
				_CustomSurveyUserRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomSurveyUserRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomSurveyUser object
        /// </summary>
        /// <param name="customSurveyUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomSurveyUserBase customSurveyUserObject, SqlDataReader reader, int start)
		{
			
				customSurveyUserObject.Id = reader.GetInt32( start + 0 );			
				customSurveyUserObject.SurveyId = reader.GetGuid( start + 1 );			
				customSurveyUserObject.UserId = reader.GetGuid( start + 2 );			
				customSurveyUserObject.SurveyUserId = reader.GetGuid( start + 3 );			
				customSurveyUserObject.AddedBy = reader.GetGuid( start + 4 );			
				customSurveyUserObject.AddedDate = reader.GetDateTime( start + 5 );			
				customSurveyUserObject.Status = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customSurveyUserObject.ReferenceId = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customSurveyUserObject.ViewedDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) customSurveyUserObject.UserIP = reader.GetString( start + 9 );			
			FillBaseObject(customSurveyUserObject, reader, (start + 10));

			
			customSurveyUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomSurveyUser object
        /// </summary>
        /// <param name="customSurveyUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomSurveyUserBase customSurveyUserObject, SqlDataReader reader)
		{
			FillObject(customSurveyUserObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomSurveyUser object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomSurveyUser object</returns>
		private CustomSurveyUser GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomSurveyUser customSurveyUserObject= new CustomSurveyUser();
					FillObject(customSurveyUserObject, reader);
					return customSurveyUserObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomSurveyUser objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomSurveyUser objects</returns>
		private CustomSurveyUserList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomSurveyUser list
			CustomSurveyUserList list = new CustomSurveyUserList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomSurveyUser customSurveyUserObject = new CustomSurveyUser();
					FillObject(customSurveyUserObject, reader);

					list.Add(customSurveyUserObject);
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
