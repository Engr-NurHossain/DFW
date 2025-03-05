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
	public partial class CustomerAppointmentTechnicianDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERAPPOINTMENTTECHNICIAN = "InsertCustomerAppointmentTechnician";
		private const string UPDATECUSTOMERAPPOINTMENTTECHNICIAN = "UpdateCustomerAppointmentTechnician";
		private const string DELETECUSTOMERAPPOINTMENTTECHNICIAN = "DeleteCustomerAppointmentTechnician";
		private const string GETCUSTOMERAPPOINTMENTTECHNICIANBYID = "GetCustomerAppointmentTechnicianById";
		private const string GETALLCUSTOMERAPPOINTMENTTECHNICIAN = "GetAllCustomerAppointmentTechnician";
		private const string GETPAGEDCUSTOMERAPPOINTMENTTECHNICIAN = "GetPagedCustomerAppointmentTechnician";
		private const string GETCUSTOMERAPPOINTMENTTECHNICIANMAXIMUMID = "GetCustomerAppointmentTechnicianMaximumId";
		private const string GETCUSTOMERAPPOINTMENTTECHNICIANROWCOUNT = "GetCustomerAppointmentTechnicianRowCount";	
		private const string GETCUSTOMERAPPOINTMENTTECHNICIANBYQUERY = "GetCustomerAppointmentTechnicianByQuery";
		#endregion
		
		#region Constructors
		public CustomerAppointmentTechnicianDataAccess(ClientContext context) : base(context) { }
		public CustomerAppointmentTechnicianDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAppointmentTechnicianObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAppointmentTechnicianBase customerAppointmentTechnicianObject)
		{	
			AddParameter(cmd, pGuid(CustomerAppointmentTechnicianBase.Property_EmployeeId, customerAppointmentTechnicianObject.EmployeeId));
			AddParameter(cmd, pInt32(CustomerAppointmentTechnicianBase.Property_CustomerAppointmentId, customerAppointmentTechnicianObject.CustomerAppointmentId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAppointmentTechnician
        /// </summary>
        /// <param name="customerAppointmentTechnicianObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAppointmentTechnicianBase customerAppointmentTechnicianObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERAPPOINTMENTTECHNICIAN);
	
				AddParameter(cmd, pInt32Out(CustomerAppointmentTechnicianBase.Property_Id));
				AddCommonParams(cmd, customerAppointmentTechnicianObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAppointmentTechnicianObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAppointmentTechnicianObject.Id = (Int32)GetOutParameter(cmd, CustomerAppointmentTechnicianBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAppointmentTechnicianObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAppointmentTechnician
        /// </summary>
        /// <param name="customerAppointmentTechnicianObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAppointmentTechnicianBase customerAppointmentTechnicianObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERAPPOINTMENTTECHNICIAN);
				
				AddParameter(cmd, pInt32(CustomerAppointmentTechnicianBase.Property_Id, customerAppointmentTechnicianObject.Id));
				AddCommonParams(cmd, customerAppointmentTechnicianObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAppointmentTechnicianObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAppointmentTechnicianObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAppointmentTechnician
        /// </summary>
        /// <param name="Id">Id of the CustomerAppointmentTechnician object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERAPPOINTMENTTECHNICIAN);	
				
				AddParameter(cmd, pInt32(CustomerAppointmentTechnicianBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAppointmentTechnician), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAppointmentTechnician object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAppointmentTechnician object to retrieve</param>
        /// <returns>CustomerAppointmentTechnician object, null if not found</returns>
		public CustomerAppointmentTechnician Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTTECHNICIANBYID))
			{
				AddParameter( cmd, pInt32(CustomerAppointmentTechnicianBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAppointmentTechnician objects 
        /// </summary>
        /// <returns>A list of CustomerAppointmentTechnician objects</returns>
		public CustomerAppointmentTechnicianList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERAPPOINTMENTTECHNICIAN))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAppointmentTechnician objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAppointmentTechnician objects</returns>
		public CustomerAppointmentTechnicianList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERAPPOINTMENTTECHNICIAN))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAppointmentTechnicianList _CustomerAppointmentTechnicianList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAppointmentTechnicianList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAppointmentTechnician objects by query String
        /// </summary>
        /// <returns>A list of CustomerAppointmentTechnician objects</returns>
		public CustomerAppointmentTechnicianList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTTECHNICIANBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAppointmentTechnician Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAppointmentTechnician
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTTECHNICIANMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAppointmentTechnician Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAppointmentTechnician
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAppointmentTechnicianRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERAPPOINTMENTTECHNICIANROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAppointmentTechnicianRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAppointmentTechnicianRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAppointmentTechnician object
        /// </summary>
        /// <param name="customerAppointmentTechnicianObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAppointmentTechnicianBase customerAppointmentTechnicianObject, SqlDataReader reader, int start)
		{
			
				customerAppointmentTechnicianObject.Id = reader.GetInt32( start + 0 );			
				customerAppointmentTechnicianObject.EmployeeId = reader.GetGuid( start + 1 );			
				customerAppointmentTechnicianObject.CustomerAppointmentId = reader.GetInt32( start + 2 );			
			FillBaseObject(customerAppointmentTechnicianObject, reader, (start + 3));

			
			customerAppointmentTechnicianObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAppointmentTechnician object
        /// </summary>
        /// <param name="customerAppointmentTechnicianObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAppointmentTechnicianBase customerAppointmentTechnicianObject, SqlDataReader reader)
		{
			FillObject(customerAppointmentTechnicianObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAppointmentTechnician object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAppointmentTechnician object</returns>
		private CustomerAppointmentTechnician GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAppointmentTechnician customerAppointmentTechnicianObject= new CustomerAppointmentTechnician();
					FillObject(customerAppointmentTechnicianObject, reader);
					return customerAppointmentTechnicianObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAppointmentTechnician objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAppointmentTechnician objects</returns>
		private CustomerAppointmentTechnicianList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAppointmentTechnician list
			CustomerAppointmentTechnicianList list = new CustomerAppointmentTechnicianList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAppointmentTechnician customerAppointmentTechnicianObject = new CustomerAppointmentTechnician();
					FillObject(customerAppointmentTechnicianObject, reader);

					list.Add(customerAppointmentTechnicianObject);
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
