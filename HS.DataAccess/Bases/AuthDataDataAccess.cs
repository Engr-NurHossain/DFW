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
	public partial class AuthDataDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAUTHDATA = "InsertAuthData";
		private const string UPDATEAUTHDATA = "UpdateAuthData";
		private const string DELETEAUTHDATA = "DeleteAuthData";
		private const string GETAUTHDATABYID = "GetAuthDataById";
		private const string GETALLAUTHDATA = "GetAllAuthData";
		private const string GETPAGEDAUTHDATA = "GetPagedAuthData";
		private const string GETAUTHDATAMAXIMUMID = "GetAuthDataMaximumId";
		private const string GETAUTHDATAROWCOUNT = "GetAuthDataRowCount";	
		private const string GETAUTHDATABYQUERY = "GetAuthDataByQuery";
		#endregion
		
		#region Constructors
		public AuthDataDataAccess(ClientContext context) : base(context) { }
		public AuthDataDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="authDataObject"></param>
		private void AddCommonParams(SqlCommand cmd, AuthDataBase authDataObject)
		{	
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_AuthRefId, 50, authDataObject.AuthRefId));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_FirstName, 50, authDataObject.FirstName));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_Lastname, 50, authDataObject.Lastname));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_Name, 50, authDataObject.Name));
			AddParameter(cmd, pDouble(AuthDataBase.Property_Amount, authDataObject.Amount));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_CustomerProfileId, 50, authDataObject.CustomerProfileId));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_PaymentprofileId, 50, authDataObject.PaymentprofileId));
			AddParameter(cmd, pNVarChar(AuthDataBase.Property_CustomerNo, 50, authDataObject.CustomerNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AuthData
        /// </summary>
        /// <param name="authDataObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AuthDataBase authDataObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAUTHDATA);
	
				AddParameter(cmd, pInt64Out(AuthDataBase.Property_Id, authDataObject.Id));
				AddCommonParams(cmd, authDataObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					authDataObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					authDataObject.Id = (Int64)GetOutParameter(cmd, AuthDataBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(authDataObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AuthData
        /// </summary>
        /// <param name="authDataObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AuthDataBase authDataObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAUTHDATA);
				
				AddParameter(cmd, pInt64(AuthDataBase.Property_Id, authDataObject.Id));
				AddCommonParams(cmd, authDataObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					authDataObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(authDataObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AuthData
        /// </summary>
        /// <param name="Id">Id of the AuthData object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int64 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAUTHDATA);	
				
				AddParameter(cmd, pInt64(AuthDataBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AuthData), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AuthData object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AuthData object to retrieve</param>
        /// <returns>AuthData object, null if not found</returns>
		public AuthData Get(Int64 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETAUTHDATABYID))
			{
				AddParameter( cmd, pInt64(AuthDataBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AuthData objects 
        /// </summary>
        /// <returns>A list of AuthData objects</returns>
		public AuthDataList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAUTHDATA))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AuthData objects by PageRequest
        /// </summary>
        /// <returns>A list of AuthData objects</returns>
		public AuthDataList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAUTHDATA))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AuthDataList _AuthDataList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AuthDataList;
			}
		}
		
		/// <summary>
        /// Retrieves all AuthData objects by query String
        /// </summary>
        /// <returns>A list of AuthData objects</returns>
		public AuthDataList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAUTHDATABYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AuthData Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AuthData
        /// </summary>
        /// <returns>Int64 type object</returns>
		public Int64 GetMaxId()
		{
			Int64 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAUTHDATAMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int64) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AuthData Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AuthData
        /// </summary>
        /// <returns>Int64 type object</returns>
		public Int64 GetRowCount()
		{
			Int64 _AuthDataRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAUTHDATAROWCOUNT))
			{
				SqlDataReader reader;
				_AuthDataRowCount = (Int64) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AuthDataRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AuthData object
        /// </summary>
        /// <param name="authDataObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AuthDataBase authDataObject, SqlDataReader reader, int start)
		{
			
				authDataObject.Id = reader.GetInt64( start + 0 );			
				if(!reader.IsDBNull(1)) authDataObject.AuthRefId = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) authDataObject.FirstName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) authDataObject.Lastname = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) authDataObject.Name = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) authDataObject.Amount = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) authDataObject.CustomerProfileId = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) authDataObject.PaymentprofileId = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) authDataObject.CustomerNo = reader.GetString( start + 8 );			
			FillBaseObject(authDataObject, reader, (start + 9));

			
			authDataObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AuthData object
        /// </summary>
        /// <param name="authDataObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AuthDataBase authDataObject, SqlDataReader reader)
		{
			FillObject(authDataObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AuthData object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AuthData object</returns>
		private AuthData GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AuthData authDataObject= new AuthData();
					FillObject(authDataObject, reader);
					return authDataObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AuthData objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AuthData objects</returns>
		private AuthDataList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AuthData list
			AuthDataList list = new AuthDataList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AuthData authDataObject = new AuthData();
					FillObject(authDataObject, reader);

					list.Add(authDataObject);
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
