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
	public partial class AccountHolderDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTACCOUNTHOLDER = "InsertAccountHolder";
		private const string UPDATEACCOUNTHOLDER = "UpdateAccountHolder";
		private const string DELETEACCOUNTHOLDER = "DeleteAccountHolder";
		private const string GETACCOUNTHOLDERBYID = "GetAccountHolderById";
		private const string GETALLACCOUNTHOLDER = "GetAllAccountHolder";
		private const string GETPAGEDACCOUNTHOLDER = "GetPagedAccountHolder";
		private const string GETACCOUNTHOLDERMAXIMUMID = "GetAccountHolderMaximumId";
		private const string GETACCOUNTHOLDERROWCOUNT = "GetAccountHolderRowCount";	
		private const string GETACCOUNTHOLDERBYQUERY = "GetAccountHolderByQuery";
		#endregion
		
		#region Constructors
		public AccountHolderDataAccess(ClientContext context) : base(context) { }
		public AccountHolderDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="accountHolderObject"></param>
		private void AddCommonParams(SqlCommand cmd, AccountHolderBase accountHolderObject)
		{	
			AddParameter(cmd, pGuid(AccountHolderBase.Property_CompanyId, accountHolderObject.CompanyId));
			AddParameter(cmd, pNVarChar(AccountHolderBase.Property_Name, 250, accountHolderObject.Name));
			AddParameter(cmd, pBool(AccountHolderBase.Property_IsActive, accountHolderObject.IsActive));
			AddParameter(cmd, pBool(AccountHolderBase.Property_InHouse, accountHolderObject.InHouse));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AccountHolder
        /// </summary>
        /// <param name="accountHolderObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AccountHolderBase accountHolderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTACCOUNTHOLDER);
	
				AddParameter(cmd, pInt32Out(AccountHolderBase.Property_Id));
				AddCommonParams(cmd, accountHolderObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					accountHolderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					accountHolderObject.Id = (Int32)GetOutParameter(cmd, AccountHolderBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(accountHolderObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AccountHolder
        /// </summary>
        /// <param name="accountHolderObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AccountHolderBase accountHolderObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEACCOUNTHOLDER);
				
				AddParameter(cmd, pInt32(AccountHolderBase.Property_Id, accountHolderObject.Id));
				AddCommonParams(cmd, accountHolderObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					accountHolderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(accountHolderObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AccountHolder
        /// </summary>
        /// <param name="Id">Id of the AccountHolder object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEACCOUNTHOLDER);	
				
				AddParameter(cmd, pInt32(AccountHolderBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AccountHolder), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AccountHolder object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AccountHolder object to retrieve</param>
        /// <returns>AccountHolder object, null if not found</returns>
		public AccountHolder Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETACCOUNTHOLDERBYID))
			{
				AddParameter( cmd, pInt32(AccountHolderBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AccountHolder objects 
        /// </summary>
        /// <returns>A list of AccountHolder objects</returns>
		public AccountHolderList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLACCOUNTHOLDER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AccountHolder objects by PageRequest
        /// </summary>
        /// <returns>A list of AccountHolder objects</returns>
		public AccountHolderList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDACCOUNTHOLDER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AccountHolderList _AccountHolderList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AccountHolderList;
			}
		}
		
		/// <summary>
        /// Retrieves all AccountHolder objects by query String
        /// </summary>
        /// <returns>A list of AccountHolder objects</returns>
		public AccountHolderList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETACCOUNTHOLDERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AccountHolder Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AccountHolder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACCOUNTHOLDERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AccountHolder Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AccountHolder
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AccountHolderRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETACCOUNTHOLDERROWCOUNT))
			{
				SqlDataReader reader;
				_AccountHolderRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AccountHolderRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AccountHolder object
        /// </summary>
        /// <param name="accountHolderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AccountHolderBase accountHolderObject, SqlDataReader reader, int start)
		{
			
				accountHolderObject.Id = reader.GetInt32( start + 0 );			
				accountHolderObject.CompanyId = reader.GetGuid( start + 1 );			
				accountHolderObject.Name = reader.GetString( start + 2 );			
				accountHolderObject.IsActive = reader.GetBoolean( start + 3 );			
				accountHolderObject.InHouse = reader.GetBoolean( start + 4 );			
			FillBaseObject(accountHolderObject, reader, (start + 5));

			
			accountHolderObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AccountHolder object
        /// </summary>
        /// <param name="accountHolderObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AccountHolderBase accountHolderObject, SqlDataReader reader)
		{
			FillObject(accountHolderObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AccountHolder object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AccountHolder object</returns>
		private AccountHolder GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AccountHolder accountHolderObject= new AccountHolder();
					FillObject(accountHolderObject, reader);
					return accountHolderObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AccountHolder objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AccountHolder objects</returns>
		private AccountHolderList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AccountHolder list
			AccountHolderList list = new AccountHolderList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AccountHolder accountHolderObject = new AccountHolder();
					FillObject(accountHolderObject, reader);

					list.Add(accountHolderObject);
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
