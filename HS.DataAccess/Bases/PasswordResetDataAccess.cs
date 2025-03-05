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
	public partial class PasswordResetDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPASSWORDRESET = "InsertPasswordReset";
		private const string UPDATEPASSWORDRESET = "UpdatePasswordReset";
		private const string DELETEPASSWORDRESET = "DeletePasswordReset";
		private const string GETPASSWORDRESETBYID = "GetPasswordResetById";
		private const string GETALLPASSWORDRESET = "GetAllPasswordReset";
		private const string GETPAGEDPASSWORDRESET = "GetPagedPasswordReset";
		private const string GETPASSWORDRESETMAXIMUMID = "GetPasswordResetMaximumId";
		private const string GETPASSWORDRESETROWCOUNT = "GetPasswordResetRowCount";	
		private const string GETPASSWORDRESETBYQUERY = "GetPasswordResetByQuery";
		#endregion
		
		#region Constructors
		public PasswordResetDataAccess(ClientContext context) : base(context) { }
		public PasswordResetDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="passwordResetObject"></param>
		private void AddCommonParams(SqlCommand cmd, PasswordResetBase passwordResetObject)
		{	
			AddParameter(cmd, pGuid(PasswordResetBase.Property_CompanyId, passwordResetObject.CompanyId));
			AddParameter(cmd, pNVarChar(PasswordResetBase.Property_UserName, 250, passwordResetObject.UserName));
			AddParameter(cmd, pNVarChar(PasswordResetBase.Property_PasswordSalt, 250, passwordResetObject.PasswordSalt));
			AddParameter(cmd, pBool(PasswordResetBase.Property_IsUsed, passwordResetObject.IsUsed));
			AddParameter(cmd, pInt32(PasswordResetBase.Property_ResetCounter, passwordResetObject.ResetCounter));
			AddParameter(cmd, pNVarChar(PasswordResetBase.Property_UserIP, 50, passwordResetObject.UserIP));
			AddParameter(cmd, pNVarChar(PasswordResetBase.Property_UserAgent, passwordResetObject.UserAgent));
			AddParameter(cmd, pNVarChar(PasswordResetBase.Property_PageUrl, passwordResetObject.PageUrl));
			AddParameter(cmd, pBool(PasswordResetBase.Property_IsActive, passwordResetObject.IsActive));
			AddParameter(cmd, pDateTime(PasswordResetBase.Property_LastUpdatedDate, passwordResetObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PasswordReset
        /// </summary>
        /// <param name="passwordResetObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PasswordResetBase passwordResetObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPASSWORDRESET);
	
				AddParameter(cmd, pInt32Out(PasswordResetBase.Property_Id));
				AddCommonParams(cmd, passwordResetObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					passwordResetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					passwordResetObject.Id = (Int32)GetOutParameter(cmd, PasswordResetBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(passwordResetObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PasswordReset
        /// </summary>
        /// <param name="passwordResetObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PasswordResetBase passwordResetObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPASSWORDRESET);
				
				AddParameter(cmd, pInt32(PasswordResetBase.Property_Id, passwordResetObject.Id));
				AddCommonParams(cmd, passwordResetObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					passwordResetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(passwordResetObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PasswordReset
        /// </summary>
        /// <param name="Id">Id of the PasswordReset object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPASSWORDRESET);	
				
				AddParameter(cmd, pInt32(PasswordResetBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PasswordReset), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PasswordReset object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PasswordReset object to retrieve</param>
        /// <returns>PasswordReset object, null if not found</returns>
		public PasswordReset Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPASSWORDRESETBYID))
			{
				AddParameter( cmd, pInt32(PasswordResetBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PasswordReset objects 
        /// </summary>
        /// <returns>A list of PasswordReset objects</returns>
		public PasswordResetList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPASSWORDRESET))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PasswordReset objects by PageRequest
        /// </summary>
        /// <returns>A list of PasswordReset objects</returns>
		public PasswordResetList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPASSWORDRESET))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PasswordResetList _PasswordResetList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PasswordResetList;
			}
		}
		
		/// <summary>
        /// Retrieves all PasswordReset objects by query String
        /// </summary>
        /// <returns>A list of PasswordReset objects</returns>
		public PasswordResetList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPASSWORDRESETBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PasswordReset Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PasswordReset
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPASSWORDRESETMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PasswordReset Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PasswordReset
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PasswordResetRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPASSWORDRESETROWCOUNT))
			{
				SqlDataReader reader;
				_PasswordResetRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PasswordResetRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PasswordReset object
        /// </summary>
        /// <param name="passwordResetObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PasswordResetBase passwordResetObject, SqlDataReader reader, int start)
		{
			
				passwordResetObject.Id = reader.GetInt32( start + 0 );			
				passwordResetObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) passwordResetObject.UserName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) passwordResetObject.PasswordSalt = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) passwordResetObject.IsUsed = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) passwordResetObject.ResetCounter = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) passwordResetObject.UserIP = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) passwordResetObject.UserAgent = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) passwordResetObject.PageUrl = reader.GetString( start + 8 );			
				passwordResetObject.IsActive = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) passwordResetObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(passwordResetObject, reader, (start + 11));

			
			passwordResetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PasswordReset object
        /// </summary>
        /// <param name="passwordResetObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PasswordResetBase passwordResetObject, SqlDataReader reader)
		{
			FillObject(passwordResetObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PasswordReset object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PasswordReset object</returns>
		private PasswordReset GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PasswordReset passwordResetObject= new PasswordReset();
					FillObject(passwordResetObject, reader);
					return passwordResetObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PasswordReset objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PasswordReset objects</returns>
		private PasswordResetList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PasswordReset list
			PasswordResetList list = new PasswordResetList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PasswordReset passwordResetObject = new PasswordReset();
					FillObject(passwordResetObject, reader);

					list.Add(passwordResetObject);
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
