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
	public partial class CustomerSystemInfoDraftDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSYSTEMINFODRAFT = "InsertCustomerSystemInfoDraft";
		private const string UPDATECUSTOMERSYSTEMINFODRAFT = "UpdateCustomerSystemInfoDraft";
		private const string DELETECUSTOMERSYSTEMINFODRAFT = "DeleteCustomerSystemInfoDraft";
		private const string GETCUSTOMERSYSTEMINFODRAFTBYID = "GetCustomerSystemInfoDraftById";
		private const string GETALLCUSTOMERSYSTEMINFODRAFT = "GetAllCustomerSystemInfoDraft";
		private const string GETPAGEDCUSTOMERSYSTEMINFODRAFT = "GetPagedCustomerSystemInfoDraft";
		private const string GETCUSTOMERSYSTEMINFODRAFTMAXIMUMID = "GetCustomerSystemInfoDraftMaximumId";
		private const string GETCUSTOMERSYSTEMINFODRAFTROWCOUNT = "GetCustomerSystemInfoDraftRowCount";	
		private const string GETCUSTOMERSYSTEMINFODRAFTBYQUERY = "GetCustomerSystemInfoDraftByQuery";
		#endregion
		
		#region Constructors
		public CustomerSystemInfoDraftDataAccess(ClientContext context) : base(context) { }
		public CustomerSystemInfoDraftDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSystemInfoDraftObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSystemInfoDraftBase customerSystemInfoDraftObject)
		{	
			AddParameter(cmd, pGuid(CustomerSystemInfoDraftBase.Property_CustomerId, customerSystemInfoDraftObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerSystemInfoDraftBase.Property_CompanyId, customerSystemInfoDraftObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_PanelType, 250, customerSystemInfoDraftObject.PanelType));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_InstallType, 250, customerSystemInfoDraftObject.InstallType));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_CellularBackup, 250, customerSystemInfoDraftObject.CellularBackup));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone1, 250, customerSystemInfoDraftObject.Zone1));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone2, 250, customerSystemInfoDraftObject.Zone2));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone3, 250, customerSystemInfoDraftObject.Zone3));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone4, 250, customerSystemInfoDraftObject.Zone4));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone5, 250, customerSystemInfoDraftObject.Zone5));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone6, 250, customerSystemInfoDraftObject.Zone6));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone7, 250, customerSystemInfoDraftObject.Zone7));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone8, 250, customerSystemInfoDraftObject.Zone8));
			AddParameter(cmd, pNVarChar(CustomerSystemInfoDraftBase.Property_Zone9, 250, customerSystemInfoDraftObject.Zone9));
			AddParameter(cmd, pInt32(CustomerSystemInfoDraftBase.Property_SmartSystemTypeId, customerSystemInfoDraftObject.SmartSystemTypeId));
			AddParameter(cmd, pInt32(CustomerSystemInfoDraftBase.Property_SmartInstallTypeId, customerSystemInfoDraftObject.SmartInstallTypeId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSystemInfoDraft
        /// </summary>
        /// <param name="customerSystemInfoDraftObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSystemInfoDraftBase customerSystemInfoDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSYSTEMINFODRAFT);
	
				AddParameter(cmd, pInt32Out(CustomerSystemInfoDraftBase.Property_Id));
				AddCommonParams(cmd, customerSystemInfoDraftObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSystemInfoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSystemInfoDraftObject.Id = (Int32)GetOutParameter(cmd, CustomerSystemInfoDraftBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSystemInfoDraftObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSystemInfoDraft
        /// </summary>
        /// <param name="customerSystemInfoDraftObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSystemInfoDraftBase customerSystemInfoDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSYSTEMINFODRAFT);
				
				AddParameter(cmd, pInt32(CustomerSystemInfoDraftBase.Property_Id, customerSystemInfoDraftObject.Id));
				AddCommonParams(cmd, customerSystemInfoDraftObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSystemInfoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSystemInfoDraftObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSystemInfoDraft
        /// </summary>
        /// <param name="Id">Id of the CustomerSystemInfoDraft object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSYSTEMINFODRAFT);	
				
				AddParameter(cmd, pInt32(CustomerSystemInfoDraftBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSystemInfoDraft), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSystemInfoDraft object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSystemInfoDraft object to retrieve</param>
        /// <returns>CustomerSystemInfoDraft object, null if not found</returns>
		public CustomerSystemInfoDraft Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFODRAFTBYID))
			{
				AddParameter( cmd, pInt32(CustomerSystemInfoDraftBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSystemInfoDraft objects 
        /// </summary>
        /// <returns>A list of CustomerSystemInfoDraft objects</returns>
		public CustomerSystemInfoDraftList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSYSTEMINFODRAFT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSystemInfoDraft objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSystemInfoDraft objects</returns>
		public CustomerSystemInfoDraftList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSYSTEMINFODRAFT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSystemInfoDraftList _CustomerSystemInfoDraftList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSystemInfoDraftList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSystemInfoDraft objects by query String
        /// </summary>
        /// <returns>A list of CustomerSystemInfoDraft objects</returns>
		public CustomerSystemInfoDraftList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFODRAFTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSystemInfoDraft Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSystemInfoDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFODRAFTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSystemInfoDraft Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSystemInfoDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSystemInfoDraftRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSYSTEMINFODRAFTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSystemInfoDraftRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSystemInfoDraftRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSystemInfoDraft object
        /// </summary>
        /// <param name="customerSystemInfoDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSystemInfoDraftBase customerSystemInfoDraftObject, SqlDataReader reader, int start)
		{
			
				customerSystemInfoDraftObject.Id = reader.GetInt32( start + 0 );			
				customerSystemInfoDraftObject.CustomerId = reader.GetGuid( start + 1 );			
				customerSystemInfoDraftObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerSystemInfoDraftObject.PanelType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerSystemInfoDraftObject.InstallType = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerSystemInfoDraftObject.CellularBackup = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerSystemInfoDraftObject.Zone1 = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerSystemInfoDraftObject.Zone2 = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerSystemInfoDraftObject.Zone3 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerSystemInfoDraftObject.Zone4 = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerSystemInfoDraftObject.Zone5 = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerSystemInfoDraftObject.Zone6 = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerSystemInfoDraftObject.Zone7 = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerSystemInfoDraftObject.Zone8 = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerSystemInfoDraftObject.Zone9 = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerSystemInfoDraftObject.SmartSystemTypeId = reader.GetInt32( start + 15 );			
				if(!reader.IsDBNull(16)) customerSystemInfoDraftObject.SmartInstallTypeId = reader.GetInt32( start + 16 );			
			FillBaseObject(customerSystemInfoDraftObject, reader, (start + 17));

			
			customerSystemInfoDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSystemInfoDraft object
        /// </summary>
        /// <param name="customerSystemInfoDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSystemInfoDraftBase customerSystemInfoDraftObject, SqlDataReader reader)
		{
			FillObject(customerSystemInfoDraftObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSystemInfoDraft object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSystemInfoDraft object</returns>
		private CustomerSystemInfoDraft GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSystemInfoDraft customerSystemInfoDraftObject= new CustomerSystemInfoDraft();
					FillObject(customerSystemInfoDraftObject, reader);
					return customerSystemInfoDraftObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSystemInfoDraft objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSystemInfoDraft objects</returns>
		private CustomerSystemInfoDraftList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSystemInfoDraft list
			CustomerSystemInfoDraftList list = new CustomerSystemInfoDraftList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSystemInfoDraft customerSystemInfoDraftObject = new CustomerSystemInfoDraft();
					FillObject(customerSystemInfoDraftObject, reader);

					list.Add(customerSystemInfoDraftObject);
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
