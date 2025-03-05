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
	public partial class CustomerAppointmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAPPOINTMENT = "InsertCustomerAppointment";
		private const string UPDATECUSTOMERAPPOINTMENT = "UpdateCustomerAppointment";
		private const string DELETECUSTOMERAPPOINTMENT = "DeleteCustomerAppointment";
		private const string GETCUSTOMERAPPOINTMENTBYID = "GetCustomerAppointmentById";
		private const string GETALLCUSTOMERAPPOINTMENT = "GetAllCustomerAppointment";
		private const string GETPAGEDCUSTOMERAPPOINTMENT = "GetPagedCustomerAppointment";
		private const string GETCUSTOMERAPPOINTMENTMAXIMUMID = "GetCustomerAppointmentMaximumId";
		private const string GETCUSTOMERAPPOINTMENTROWCOUNT = "GetCustomerAppointmentRowCount";	
		private const string GETCUSTOMERAPPOINTMENTBYQUERY = "GetCustomerAppointmentByQuery";
		#endregion
		
		#region Constructors
		public CustomerAppointmentDataAccess(ClientContext context) : base(context) { }
		public CustomerAppointmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAppointmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAppointmentBase customerAppointmentObject)
		{	
			AddParameter(cmd, pGuid(CustomerAppointmentBase.Property_AppointmentId, customerAppointmentObject.AppointmentId));
			AddParameter(cmd, pGuid(CustomerAppointmentBase.Property_CompanyId, customerAppointmentObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerAppointmentBase.Property_CustomerId, customerAppointmentObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerAppointmentBase.Property_EmployeeId, customerAppointmentObject.EmployeeId));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_AppointmentType, 50, customerAppointmentObject.AppointmentType));
			AddParameter(cmd, pDateTime(CustomerAppointmentBase.Property_AppointmentDate, customerAppointmentObject.AppointmentDate));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_AppointmentStartTime, 50, customerAppointmentObject.AppointmentStartTime));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_AppointmentEndTime, 50, customerAppointmentObject.AppointmentEndTime));
			AddParameter(cmd, pBool(CustomerAppointmentBase.Property_IsAllDay, customerAppointmentObject.IsAllDay));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_Notes, 50, customerAppointmentObject.Notes));
			AddParameter(cmd, pBool(CustomerAppointmentBase.Property_Status, customerAppointmentObject.Status));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_TaxType, 50, customerAppointmentObject.TaxType));
			AddParameter(cmd, pDouble(CustomerAppointmentBase.Property_TaxPercent, customerAppointmentObject.TaxPercent));
			AddParameter(cmd, pDouble(CustomerAppointmentBase.Property_TaxTotal, customerAppointmentObject.TaxTotal));
			AddParameter(cmd, pDouble(CustomerAppointmentBase.Property_TotalAmount, customerAppointmentObject.TotalAmount));
			AddParameter(cmd, pDouble(CustomerAppointmentBase.Property_TotalAmountTax, customerAppointmentObject.TotalAmountTax));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_CreatedBy, 50, customerAppointmentObject.CreatedBy));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_LastUpdatedBy, 50, customerAppointmentObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(CustomerAppointmentBase.Property_LastUpdatedDate, customerAppointmentObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(CustomerAppointmentBase.Property_Address, customerAppointmentObject.Address));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAppointment
        /// </summary>
        /// <param name="customerAppointmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAppointmentBase customerAppointmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAPPOINTMENT);
	
				AddParameter(cmd, pInt32Out(CustomerAppointmentBase.Property_Id));
				AddCommonParams(cmd, customerAppointmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAppointmentObject.Id = (Int32)GetOutParameter(cmd, CustomerAppointmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAppointmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAppointment
        /// </summary>
        /// <param name="customerAppointmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAppointmentBase customerAppointmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAPPOINTMENT);
				
				AddParameter(cmd, pInt32(CustomerAppointmentBase.Property_Id, customerAppointmentObject.Id));
				AddCommonParams(cmd, customerAppointmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAppointmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAppointment
        /// </summary>
        /// <param name="Id">Id of the CustomerAppointment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAPPOINTMENT);	
				
				AddParameter(cmd, pInt32(CustomerAppointmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAppointment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAppointment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAppointment object to retrieve</param>
        /// <returns>CustomerAppointment object, null if not found</returns>
		public CustomerAppointment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTBYID))
			{
				AddParameter( cmd, pInt32(CustomerAppointmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAppointment objects 
        /// </summary>
        /// <returns>A list of CustomerAppointment objects</returns>
		public CustomerAppointmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAPPOINTMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAppointment objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAppointment objects</returns>
		public CustomerAppointmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAPPOINTMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAppointmentList _CustomerAppointmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAppointmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAppointment objects by query String
        /// </summary>
        /// <returns>A list of CustomerAppointment objects</returns>
		public CustomerAppointmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAppointment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAppointment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAppointment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAppointment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAppointmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAppointmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAppointmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAppointment object
        /// </summary>
        /// <param name="customerAppointmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAppointmentBase customerAppointmentObject, SqlDataReader reader, int start)
		{
			
				customerAppointmentObject.Id = reader.GetInt32( start + 0 );			
				customerAppointmentObject.AppointmentId = reader.GetGuid( start + 1 );			
				customerAppointmentObject.CompanyId = reader.GetGuid( start + 2 );			
				customerAppointmentObject.CustomerId = reader.GetGuid( start + 3 );			
				customerAppointmentObject.EmployeeId = reader.GetGuid( start + 4 );			
				customerAppointmentObject.AppointmentType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerAppointmentObject.AppointmentDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customerAppointmentObject.AppointmentStartTime = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerAppointmentObject.AppointmentEndTime = reader.GetString( start + 8 );			
				customerAppointmentObject.IsAllDay = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) customerAppointmentObject.Notes = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerAppointmentObject.Status = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) customerAppointmentObject.TaxType = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerAppointmentObject.TaxPercent = reader.GetDouble( start + 13 );			
				if(!reader.IsDBNull(14)) customerAppointmentObject.TaxTotal = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) customerAppointmentObject.TotalAmount = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) customerAppointmentObject.TotalAmountTax = reader.GetDouble( start + 16 );			
				customerAppointmentObject.CreatedBy = reader.GetString( start + 17 );			
				customerAppointmentObject.LastUpdatedBy = reader.GetString( start + 18 );			
				customerAppointmentObject.LastUpdatedDate = reader.GetDateTime( start + 19 );			
				if(!reader.IsDBNull(20)) customerAppointmentObject.Address = reader.GetString( start + 20 );			
			FillBaseObject(customerAppointmentObject, reader, (start + 21));

			
			customerAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAppointment object
        /// </summary>
        /// <param name="customerAppointmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAppointmentBase customerAppointmentObject, SqlDataReader reader)
		{
			FillObject(customerAppointmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAppointment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAppointment object</returns>
		private CustomerAppointment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAppointment customerAppointmentObject= new CustomerAppointment();
					FillObject(customerAppointmentObject, reader);
					return customerAppointmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAppointment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAppointment objects</returns>
		private CustomerAppointmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAppointment list
			CustomerAppointmentList list = new CustomerAppointmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAppointment customerAppointmentObject = new CustomerAppointment();
					FillObject(customerAppointmentObject, reader);

					list.Add(customerAppointmentObject);
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
