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
	public partial class CustomerApiSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAPISETTING = "InsertCustomerApiSetting";
		private const string UPDATECUSTOMERAPISETTING = "UpdateCustomerApiSetting";
		private const string DELETECUSTOMERAPISETTING = "DeleteCustomerApiSetting";
		private const string GETCUSTOMERAPISETTINGBYID = "GetCustomerApiSettingById";
		private const string GETALLCUSTOMERAPISETTING = "GetAllCustomerApiSetting";
		private const string GETPAGEDCUSTOMERAPISETTING = "GetPagedCustomerApiSetting";
		private const string GETCUSTOMERAPISETTINGMAXIMUMID = "GetCustomerApiSettingMaximumId";
		private const string GETCUSTOMERAPISETTINGROWCOUNT = "GetCustomerApiSettingRowCount";	
		private const string GETCUSTOMERAPISETTINGBYQUERY = "GetCustomerApiSettingByQuery";
		#endregion
		
		#region Constructors
		public CustomerApiSettingDataAccess(ClientContext context) : base(context) { }
		public CustomerApiSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerApiSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerApiSettingBase customerApiSettingObject)
		{	
			AddParameter(cmd, pGuid(CustomerApiSettingBase.Property_CompanyId, customerApiSettingObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerApiSettingBase.Property_CustomerId, customerApiSettingObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerApiSettingBase.Property_AccountName, 150, customerApiSettingObject.AccountName));
			AddParameter(cmd, pNVarChar(CustomerApiSettingBase.Property_Url, 150, customerApiSettingObject.Url));
			AddParameter(cmd, pNVarChar(CustomerApiSettingBase.Property_UserName, 150, customerApiSettingObject.UserName));
			AddParameter(cmd, pNVarChar(CustomerApiSettingBase.Property_Password, 150, customerApiSettingObject.Password));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerApiSetting
        /// </summary>
        /// <param name="customerApiSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerApiSettingBase customerApiSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAPISETTING);
	
				AddParameter(cmd, pInt32Out(CustomerApiSettingBase.Property_Id));
				AddCommonParams(cmd, customerApiSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerApiSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerApiSettingObject.Id = (Int32)GetOutParameter(cmd, CustomerApiSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerApiSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerApiSetting
        /// </summary>
        /// <param name="customerApiSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerApiSettingBase customerApiSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAPISETTING);
				
				AddParameter(cmd, pInt32(CustomerApiSettingBase.Property_Id, customerApiSettingObject.Id));
				AddCommonParams(cmd, customerApiSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerApiSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerApiSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerApiSetting
        /// </summary>
        /// <param name="Id">Id of the CustomerApiSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAPISETTING);	
				
				AddParameter(cmd, pInt32(CustomerApiSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerApiSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerApiSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerApiSetting object to retrieve</param>
        /// <returns>CustomerApiSetting object, null if not found</returns>
		public CustomerApiSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPISETTINGBYID))
			{
				AddParameter( cmd, pInt32(CustomerApiSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerApiSetting objects 
        /// </summary>
        /// <returns>A list of CustomerApiSetting objects</returns>
		public CustomerApiSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAPISETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerApiSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerApiSetting objects</returns>
		public CustomerApiSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAPISETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerApiSettingList _CustomerApiSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerApiSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerApiSetting objects by query String
        /// </summary>
        /// <returns>A list of CustomerApiSetting objects</returns>
		public CustomerApiSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPISETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerApiSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerApiSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPISETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerApiSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerApiSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerApiSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPISETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerApiSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerApiSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerApiSetting object
        /// </summary>
        /// <param name="customerApiSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerApiSettingBase customerApiSettingObject, SqlDataReader reader, int start)
		{
			
				customerApiSettingObject.Id = reader.GetInt32( start + 0 );			
				customerApiSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				customerApiSettingObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerApiSettingObject.AccountName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerApiSettingObject.Url = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerApiSettingObject.UserName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerApiSettingObject.Password = reader.GetString( start + 6 );			
			FillBaseObject(customerApiSettingObject, reader, (start + 7));

			
			customerApiSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerApiSetting object
        /// </summary>
        /// <param name="customerApiSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerApiSettingBase customerApiSettingObject, SqlDataReader reader)
		{
			FillObject(customerApiSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerApiSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerApiSetting object</returns>
		private CustomerApiSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerApiSetting customerApiSettingObject= new CustomerApiSetting();
					FillObject(customerApiSettingObject, reader);
					return customerApiSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerApiSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerApiSetting objects</returns>
		private CustomerApiSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerApiSetting list
			CustomerApiSettingList list = new CustomerApiSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerApiSetting customerApiSettingObject = new CustomerApiSetting();
					FillObject(customerApiSettingObject, reader);

					list.Add(customerApiSettingObject);
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
