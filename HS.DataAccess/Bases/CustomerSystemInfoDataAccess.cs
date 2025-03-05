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
	public partial class CustomerSystemInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSYSTEMINFO = "InsertCustomerSystemInfo";
		private const string UPDATECUSTOMERSYSTEMINFO = "UpdateCustomerSystemInfo";
		private const string DELETECUSTOMERSYSTEMINFO = "DeleteCustomerSystemInfo";
		private const string GETCUSTOMERSYSTEMINFOBYID = "GetCustomerSystemInfoById";
		private const string GETALLCUSTOMERSYSTEMINFO = "GetAllCustomerSystemInfo";
		private const string GETPAGEDCUSTOMERSYSTEMINFO = "GetPagedCustomerSystemInfo";
		private const string GETCUSTOMERSYSTEMINFOMAXIMUMID = "GetCustomerSystemInfoMaximumId";
		private const string GETCUSTOMERSYSTEMINFOROWCOUNT = "GetCustomerSystemInfoRowCount";	
		private const string GETCUSTOMERSYSTEMINFOBYQUERY = "GetCustomerSystemInfoByQuery";
		#endregion
		
		#region Constructors
		public CustomerSystemInfoDataAccess(ClientContext context) : base(context) { }
		public CustomerSystemInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSystemInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSystemInfoBase customerSystemInfoObject)
		{	
			AddParameter(cmd, pGuid(CustomerSystemInfoBase.Property_CustomerId, customerSystemInfoObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerSystemInfoBase.Property_CompanyId, customerSystemInfoObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_PanelType, 250, customerSystemInfoObject.PanelType));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_InstallType, 250, customerSystemInfoObject.InstallType));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_CellularBackup, 250, customerSystemInfoObject.CellularBackup));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone1, 250, customerSystemInfoObject.Zone1));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone2, 250, customerSystemInfoObject.Zone2));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone3, 250, customerSystemInfoObject.Zone3));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone4, 250, customerSystemInfoObject.Zone4));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone5, 250, customerSystemInfoObject.Zone5));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone6, 250, customerSystemInfoObject.Zone6));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone7, 250, customerSystemInfoObject.Zone7));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone8, 250, customerSystemInfoObject.Zone8));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoBase.Property_Zone9, 250, customerSystemInfoObject.Zone9));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSystemInfo
        /// </summary>
        /// <param name="customerSystemInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSystemInfoBase customerSystemInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSYSTEMINFO);
	
				AddParameter(cmd, pInt32Out(CustomerSystemInfoBase.Property_Id));
				AddCommonParams(cmd, customerSystemInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSystemInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSystemInfoObject.Id = (Int32)GetOutParameter(cmd, CustomerSystemInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSystemInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSystemInfo
        /// </summary>
        /// <param name="customerSystemInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSystemInfoBase customerSystemInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSYSTEMINFO);
				
				AddParameter(cmd, pInt32(CustomerSystemInfoBase.Property_Id, customerSystemInfoObject.Id));
				AddCommonParams(cmd, customerSystemInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSystemInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSystemInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSystemInfo
        /// </summary>
        /// <param name="Id">Id of the CustomerSystemInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSYSTEMINFO);	
				
				AddParameter(cmd, pInt32(CustomerSystemInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSystemInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSystemInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSystemInfo object to retrieve</param>
        /// <returns>CustomerSystemInfo object, null if not found</returns>
		public CustomerSystemInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFOBYID))
			{
				AddParameter( cmd, pInt32(CustomerSystemInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSystemInfo objects 
        /// </summary>
        /// <returns>A list of CustomerSystemInfo objects</returns>
		public CustomerSystemInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSYSTEMINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSystemInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSystemInfo objects</returns>
		public CustomerSystemInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSYSTEMINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSystemInfoList _CustomerSystemInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSystemInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSystemInfo objects by query String
        /// </summary>
        /// <returns>A list of CustomerSystemInfo objects</returns>
		public CustomerSystemInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSystemInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSystemInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSystemInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSystemInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSystemInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFOROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSystemInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSystemInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSystemInfo object
        /// </summary>
        /// <param name="customerSystemInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSystemInfoBase customerSystemInfoObject, SqlDataReader reader, int start)
		{
			
				customerSystemInfoObject.Id = reader.GetInt32( start + 0 );			
				customerSystemInfoObject.CustomerId = reader.GetGuid( start + 1 );			
				customerSystemInfoObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerSystemInfoObject.PanelType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerSystemInfoObject.InstallType = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerSystemInfoObject.CellularBackup = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerSystemInfoObject.Zone1 = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerSystemInfoObject.Zone2 = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerSystemInfoObject.Zone3 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerSystemInfoObject.Zone4 = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerSystemInfoObject.Zone5 = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerSystemInfoObject.Zone6 = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerSystemInfoObject.Zone7 = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerSystemInfoObject.Zone8 = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerSystemInfoObject.Zone9 = reader.GetString( start + 14 );			
			FillBaseObject(customerSystemInfoObject, reader, (start + 15));

			
			customerSystemInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSystemInfo object
        /// </summary>
        /// <param name="customerSystemInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSystemInfoBase customerSystemInfoObject, SqlDataReader reader)
		{
			FillObject(customerSystemInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSystemInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSystemInfo object</returns>
		private CustomerSystemInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSystemInfo customerSystemInfoObject= new CustomerSystemInfo();
					FillObject(customerSystemInfoObject, reader);
					return customerSystemInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSystemInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSystemInfo objects</returns>
		private CustomerSystemInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSystemInfo list
			CustomerSystemInfoList list = new CustomerSystemInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSystemInfo customerSystemInfoObject = new CustomerSystemInfo();
					FillObject(customerSystemInfoObject, reader);

					list.Add(customerSystemInfoObject);
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
