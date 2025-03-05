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
	public partial class CustomerAppointmentEquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAPPOINTMENTEQUIPMENT = "InsertCustomerAppointmentEquipment";
		private const string UPDATECUSTOMERAPPOINTMENTEQUIPMENT = "UpdateCustomerAppointmentEquipment";
		private const string DELETECUSTOMERAPPOINTMENTEQUIPMENT = "DeleteCustomerAppointmentEquipment";
		private const string GETCUSTOMERAPPOINTMENTEQUIPMENTBYID = "GetCustomerAppointmentEquipmentById";
		private const string GETALLCUSTOMERAPPOINTMENTEQUIPMENT = "GetAllCustomerAppointmentEquipment";
		private const string GETPAGEDCUSTOMERAPPOINTMENTEQUIPMENT = "GetPagedCustomerAppointmentEquipment";
		private const string GETCUSTOMERAPPOINTMENTEQUIPMENTMAXIMUMID = "GetCustomerAppointmentEquipmentMaximumId";
		private const string GETCUSTOMERAPPOINTMENTEQUIPMENTROWCOUNT = "GetCustomerAppointmentEquipmentRowCount";	
		private const string GETCUSTOMERAPPOINTMENTEQUIPMENTBYQUERY = "GetCustomerAppointmentEquipmentByQuery";
		#endregion
		
		#region Constructors
		public CustomerAppointmentEquipmentDataAccess(ClientContext context) : base(context) { }
		public CustomerAppointmentEquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAppointmentEquipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAppointmentEquipmentBase customerAppointmentEquipmentObject)
		{	
			AddParameter(cmd, pGuid(CustomerAppointmentEquipmentBase.Property_AppointmentId, customerAppointmentEquipmentObject.AppointmentId));
			AddParameter(cmd, pGuid(CustomerAppointmentEquipmentBase.Property_EquipmentId, customerAppointmentEquipmentObject.EquipmentId));
			AddParameter(cmd, pInt32(CustomerAppointmentEquipmentBase.Property_Quantity, customerAppointmentEquipmentObject.Quantity));
			AddParameter(cmd, pDouble(CustomerAppointmentEquipmentBase.Property_UnitPrice, customerAppointmentEquipmentObject.UnitPrice));
			AddParameter(cmd, pDouble(CustomerAppointmentEquipmentBase.Property_TotalPrice, customerAppointmentEquipmentObject.TotalPrice));
			AddParameter(cmd, pDateTime(CustomerAppointmentEquipmentBase.Property_CreatedDate, customerAppointmentEquipmentObject.CreatedDate));
			AddParameter(cmd, pNVarChar(CustomerAppointmentEquipmentBase.Property_CreatedBy, 50, customerAppointmentEquipmentObject.CreatedBy));
			AddParameter(cmd, pNVarChar(CustomerAppointmentEquipmentBase.Property_EquipName, 500, customerAppointmentEquipmentObject.EquipName));
			AddParameter(cmd, pNVarChar(CustomerAppointmentEquipmentBase.Property_EquipDetail, customerAppointmentEquipmentObject.EquipDetail));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsEquipmentRelease, customerAppointmentEquipmentObject.IsEquipmentRelease));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsService, customerAppointmentEquipmentObject.IsService));
			AddParameter(cmd, pGuid(CustomerAppointmentEquipmentBase.Property_CreatedByUid, customerAppointmentEquipmentObject.CreatedByUid));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsAgreementItem, customerAppointmentEquipmentObject.IsAgreementItem));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsBaseItem, customerAppointmentEquipmentObject.IsBaseItem));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsBadInventory, customerAppointmentEquipmentObject.IsBadInventory));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsDefaultService, customerAppointmentEquipmentObject.IsDefaultService));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsCheckedEquipment, customerAppointmentEquipmentObject.IsCheckedEquipment));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsTransfered, customerAppointmentEquipmentObject.IsTransfered));
			AddParameter(cmd, pInt32(CustomerAppointmentEquipmentBase.Property_QuantityLeftEquipment, customerAppointmentEquipmentObject.QuantityLeftEquipment));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsEquipmentExist, customerAppointmentEquipmentObject.IsEquipmentExist));
			AddParameter(cmd, pDouble(CustomerAppointmentEquipmentBase.Property_OriginalUnitPrice, customerAppointmentEquipmentObject.OriginalUnitPrice));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsInvoiceCreate, customerAppointmentEquipmentObject.IsInvoiceCreate));
			AddParameter(cmd, pNVarChar(CustomerAppointmentEquipmentBase.Property_ReferenceInvoiceId, 150, customerAppointmentEquipmentObject.ReferenceInvoiceId));
			AddParameter(cmd, pInt32(CustomerAppointmentEquipmentBase.Property_ReferenceInvDetailId, customerAppointmentEquipmentObject.ReferenceInvDetailId));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsBilling, customerAppointmentEquipmentObject.IsBilling));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsCopied, customerAppointmentEquipmentObject.IsCopied));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsNonCommissionable, customerAppointmentEquipmentObject.IsNonCommissionable));
			AddParameter(cmd, pGuid(CustomerAppointmentEquipmentBase.Property_InstalledByUid, customerAppointmentEquipmentObject.InstalledByUid));
			AddParameter(cmd, pBool(CustomerAppointmentEquipmentBase.Property_IsBillingProcess, customerAppointmentEquipmentObject.IsBillingProcess));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAppointmentEquipment
        /// </summary>
        /// <param name="customerAppointmentEquipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAppointmentEquipmentBase customerAppointmentEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAPPOINTMENTEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(CustomerAppointmentEquipmentBase.Property_Id));
				AddCommonParams(cmd, customerAppointmentEquipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAppointmentEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAppointmentEquipmentObject.Id = (Int32)GetOutParameter(cmd, CustomerAppointmentEquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAppointmentEquipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAppointmentEquipment
        /// </summary>
        /// <param name="customerAppointmentEquipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAppointmentEquipmentBase customerAppointmentEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAPPOINTMENTEQUIPMENT);
				
				AddParameter(cmd, pInt32(CustomerAppointmentEquipmentBase.Property_Id, customerAppointmentEquipmentObject.Id));
				AddCommonParams(cmd, customerAppointmentEquipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAppointmentEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAppointmentEquipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAppointmentEquipment
        /// </summary>
        /// <param name="Id">Id of the CustomerAppointmentEquipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAPPOINTMENTEQUIPMENT);	
				
				AddParameter(cmd, pInt32(CustomerAppointmentEquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAppointmentEquipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAppointmentEquipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAppointmentEquipment object to retrieve</param>
        /// <returns>CustomerAppointmentEquipment object, null if not found</returns>
		public CustomerAppointmentEquipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(CustomerAppointmentEquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAppointmentEquipment objects 
        /// </summary>
        /// <returns>A list of CustomerAppointmentEquipment objects</returns>
		public CustomerAppointmentEquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAPPOINTMENTEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAppointmentEquipment objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAppointmentEquipment objects</returns>
		public CustomerAppointmentEquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAPPOINTMENTEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAppointmentEquipmentList _CustomerAppointmentEquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAppointmentEquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAppointmentEquipment objects by query String
        /// </summary>
        /// <returns>A list of CustomerAppointmentEquipment objects</returns>
		public CustomerAppointmentEquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAppointmentEquipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAppointmentEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAppointmentEquipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAppointmentEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAppointmentEquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAppointmentEquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAppointmentEquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAppointmentEquipment object
        /// </summary>
        /// <param name="customerAppointmentEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAppointmentEquipmentBase customerAppointmentEquipmentObject, SqlDataReader reader, int start)
		{
			
				customerAppointmentEquipmentObject.Id = reader.GetInt32( start + 0 );			
				customerAppointmentEquipmentObject.AppointmentId = reader.GetGuid( start + 1 );			
				customerAppointmentEquipmentObject.EquipmentId = reader.GetGuid( start + 2 );			
				customerAppointmentEquipmentObject.Quantity = reader.GetInt32( start + 3 );			
				customerAppointmentEquipmentObject.UnitPrice = reader.GetDouble( start + 4 );			
				customerAppointmentEquipmentObject.TotalPrice = reader.GetDouble( start + 5 );			
				customerAppointmentEquipmentObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customerAppointmentEquipmentObject.CreatedBy = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerAppointmentEquipmentObject.EquipName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerAppointmentEquipmentObject.EquipDetail = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerAppointmentEquipmentObject.IsEquipmentRelease = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) customerAppointmentEquipmentObject.IsService = reader.GetBoolean( start + 11 );			
				customerAppointmentEquipmentObject.CreatedByUid = reader.GetGuid( start + 12 );			
				if(!reader.IsDBNull(13)) customerAppointmentEquipmentObject.IsAgreementItem = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) customerAppointmentEquipmentObject.IsBaseItem = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) customerAppointmentEquipmentObject.IsBadInventory = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) customerAppointmentEquipmentObject.IsDefaultService = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) customerAppointmentEquipmentObject.IsCheckedEquipment = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) customerAppointmentEquipmentObject.IsTransfered = reader.GetBoolean( start + 18 );			
				if(!reader.IsDBNull(19)) customerAppointmentEquipmentObject.QuantityLeftEquipment = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) customerAppointmentEquipmentObject.IsEquipmentExist = reader.GetBoolean( start + 20 );			
				if(!reader.IsDBNull(21)) customerAppointmentEquipmentObject.OriginalUnitPrice = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) customerAppointmentEquipmentObject.IsInvoiceCreate = reader.GetBoolean( start + 22 );			
				if(!reader.IsDBNull(23)) customerAppointmentEquipmentObject.ReferenceInvoiceId = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerAppointmentEquipmentObject.ReferenceInvDetailId = reader.GetInt32( start + 24 );			
				if(!reader.IsDBNull(25)) customerAppointmentEquipmentObject.IsBilling = reader.GetBoolean( start + 25 );			
				if(!reader.IsDBNull(26)) customerAppointmentEquipmentObject.IsCopied = reader.GetBoolean( start + 26 );			
				if(!reader.IsDBNull(27)) customerAppointmentEquipmentObject.IsNonCommissionable = reader.GetBoolean( start + 27 );			
				customerAppointmentEquipmentObject.InstalledByUid = reader.GetGuid( start + 28 );			
				if(!reader.IsDBNull(29)) customerAppointmentEquipmentObject.IsBillingProcess = reader.GetBoolean( start + 29 );			
			FillBaseObject(customerAppointmentEquipmentObject, reader, (start + 30));

			
			customerAppointmentEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAppointmentEquipment object
        /// </summary>
        /// <param name="customerAppointmentEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAppointmentEquipmentBase customerAppointmentEquipmentObject, SqlDataReader reader)
		{
			FillObject(customerAppointmentEquipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAppointmentEquipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAppointmentEquipment object</returns>
		private CustomerAppointmentEquipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAppointmentEquipment customerAppointmentEquipmentObject= new CustomerAppointmentEquipment();
					FillObject(customerAppointmentEquipmentObject, reader);
					return customerAppointmentEquipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAppointmentEquipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAppointmentEquipment objects</returns>
		private CustomerAppointmentEquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAppointmentEquipment list
			CustomerAppointmentEquipmentList list = new CustomerAppointmentEquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAppointmentEquipment customerAppointmentEquipmentObject = new CustomerAppointmentEquipment();
					FillObject(customerAppointmentEquipmentObject, reader);

					list.Add(customerAppointmentEquipmentObject);
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
