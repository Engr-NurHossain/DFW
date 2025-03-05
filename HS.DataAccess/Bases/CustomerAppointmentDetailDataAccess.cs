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
	public partial class CustomerAppointmentDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAPPOINTMENTDETAIL = "InsertCustomerAppointmentDetail";
		private const string UPDATECUSTOMERAPPOINTMENTDETAIL = "UpdateCustomerAppointmentDetail";
		private const string DELETECUSTOMERAPPOINTMENTDETAIL = "DeleteCustomerAppointmentDetail";
		private const string GETCUSTOMERAPPOINTMENTDETAILBYID = "GetCustomerAppointmentDetailById";
		private const string GETALLCUSTOMERAPPOINTMENTDETAIL = "GetAllCustomerAppointmentDetail";
		private const string GETPAGEDCUSTOMERAPPOINTMENTDETAIL = "GetPagedCustomerAppointmentDetail";
		private const string GETCUSTOMERAPPOINTMENTDETAILMAXIMUMID = "GetCustomerAppointmentDetailMaximumId";
		private const string GETCUSTOMERAPPOINTMENTDETAILROWCOUNT = "GetCustomerAppointmentDetailRowCount";	
		private const string GETCUSTOMERAPPOINTMENTDETAILBYQUERY = "GetCustomerAppointmentDetailByQuery";
		#endregion
		
		#region Constructors
		public CustomerAppointmentDetailDataAccess(ClientContext context) : base(context) { }
		public CustomerAppointmentDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAppointmentDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAppointmentDetailBase customerAppointmentDetailObject)
		{	
			AddParameter(cmd, pGuid(CustomerAppointmentDetailBase.Property_AppointmentId, customerAppointmentDetailObject.AppointmentId));
			AddParameter(cmd, pNVarChar(CustomerAppointmentDetailBase.Property_InstallType, 50, customerAppointmentDetailObject.InstallType));
			AddParameter(cmd, pDouble(CustomerAppointmentDetailBase.Property_CollectedAmount, customerAppointmentDetailObject.CollectedAmount));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAppointmentDetail
        /// </summary>
        /// <param name="customerAppointmentDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAppointmentDetailBase customerAppointmentDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAPPOINTMENTDETAIL);
	
				AddParameter(cmd, pInt32Out(CustomerAppointmentDetailBase.Property_Id));
				AddCommonParams(cmd, customerAppointmentDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAppointmentDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAppointmentDetailObject.Id = (Int32)GetOutParameter(cmd, CustomerAppointmentDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAppointmentDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAppointmentDetail
        /// </summary>
        /// <param name="customerAppointmentDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAppointmentDetailBase customerAppointmentDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAPPOINTMENTDETAIL);
				
				AddParameter(cmd, pInt32(CustomerAppointmentDetailBase.Property_Id, customerAppointmentDetailObject.Id));
				AddCommonParams(cmd, customerAppointmentDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAppointmentDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAppointmentDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAppointmentDetail
        /// </summary>
        /// <param name="Id">Id of the CustomerAppointmentDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAPPOINTMENTDETAIL);	
				
				AddParameter(cmd, pInt32(CustomerAppointmentDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAppointmentDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAppointmentDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAppointmentDetail object to retrieve</param>
        /// <returns>CustomerAppointmentDetail object, null if not found</returns>
		public CustomerAppointmentDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTDETAILBYID))
			{
				AddParameter( cmd, pInt32(CustomerAppointmentDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAppointmentDetail objects 
        /// </summary>
        /// <returns>A list of CustomerAppointmentDetail objects</returns>
		public CustomerAppointmentDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAPPOINTMENTDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAppointmentDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAppointmentDetail objects</returns>
		public CustomerAppointmentDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAPPOINTMENTDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAppointmentDetailList _CustomerAppointmentDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAppointmentDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAppointmentDetail objects by query String
        /// </summary>
        /// <returns>A list of CustomerAppointmentDetail objects</returns>
		public CustomerAppointmentDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAppointmentDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAppointmentDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAppointmentDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAppointmentDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAppointmentDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAppointmentDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAppointmentDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAppointmentDetail object
        /// </summary>
        /// <param name="customerAppointmentDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAppointmentDetailBase customerAppointmentDetailObject, SqlDataReader reader, int start)
		{
			
				customerAppointmentDetailObject.Id = reader.GetInt32( start + 0 );			
				customerAppointmentDetailObject.AppointmentId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerAppointmentDetailObject.InstallType = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerAppointmentDetailObject.CollectedAmount = reader.GetDouble( start + 3 );			
			FillBaseObject(customerAppointmentDetailObject, reader, (start + 4));

			
			customerAppointmentDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAppointmentDetail object
        /// </summary>
        /// <param name="customerAppointmentDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAppointmentDetailBase customerAppointmentDetailObject, SqlDataReader reader)
		{
			FillObject(customerAppointmentDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAppointmentDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAppointmentDetail object</returns>
		private CustomerAppointmentDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAppointmentDetail customerAppointmentDetailObject= new CustomerAppointmentDetail();
					FillObject(customerAppointmentDetailObject, reader);
					return customerAppointmentDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAppointmentDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAppointmentDetail objects</returns>
		private CustomerAppointmentDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAppointmentDetail list
			CustomerAppointmentDetailList list = new CustomerAppointmentDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAppointmentDetail customerAppointmentDetailObject = new CustomerAppointmentDetail();
					FillObject(customerAppointmentDetailObject, reader);

					list.Add(customerAppointmentDetailObject);
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
