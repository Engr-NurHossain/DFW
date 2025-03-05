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
	public partial class CustomerAddressDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERADDRESS = "InsertCustomerAddress";
		private const string UPDATECUSTOMERADDRESS = "UpdateCustomerAddress";
		private const string DELETECUSTOMERADDRESS = "DeleteCustomerAddress";
		private const string GETCUSTOMERADDRESSBYID = "GetCustomerAddressById";
		private const string GETALLCUSTOMERADDRESS = "GetAllCustomerAddress";
		private const string GETPAGEDCUSTOMERADDRESS = "GetPagedCustomerAddress";
		private const string GETCUSTOMERADDRESSMAXIMUMID = "GetCustomerAddressMaximumId";
		private const string GETCUSTOMERADDRESSROWCOUNT = "GetCustomerAddressRowCount";	
		private const string GETCUSTOMERADDRESSBYQUERY = "GetCustomerAddressByQuery";
		#endregion
		
		#region Constructors
		public CustomerAddressDataAccess(ClientContext context) : base(context) { }
		public CustomerAddressDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerAddressObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerAddressBase customerAddressObject)
		{	
			AddParameter(cmd, pGuid(CustomerAddressBase.Property_CustomerId, customerAddressObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_RefId, 50, customerAddressObject.RefId));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_AddressType, 50, customerAddressObject.AddressType));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_Street, 350, customerAddressObject.Street));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_City, 350, customerAddressObject.City));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_State, 50, customerAddressObject.State));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_ZipCode, 50, customerAddressObject.ZipCode));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_County, 150, customerAddressObject.County));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_FirstName, 250, customerAddressObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_LastName, 250, customerAddressObject.LastName));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_BusinessName, 250, customerAddressObject.BusinessName));
			AddParameter(cmd, pBool(CustomerAddressBase.Property_IsDefault, customerAddressObject.IsDefault));
			AddParameter(cmd, pNVarChar(CustomerAddressBase.Property_Notes, customerAddressObject.Notes));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerAddress
        /// </summary>
        /// <param name="customerAddressObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerAddressBase customerAddressObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERADDRESS);
	
				AddParameter(cmd, pInt32Out(CustomerAddressBase.Property_Id));
				AddCommonParams(cmd, customerAddressObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerAddressObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerAddressObject.Id = (Int32)GetOutParameter(cmd, CustomerAddressBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerAddressObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerAddress
        /// </summary>
        /// <param name="customerAddressObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerAddressBase customerAddressObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERADDRESS);
				
				AddParameter(cmd, pInt32(CustomerAddressBase.Property_Id, customerAddressObject.Id));
				AddCommonParams(cmd, customerAddressObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerAddressObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerAddressObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerAddress
        /// </summary>
        /// <param name="Id">Id of the CustomerAddress object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERADDRESS);	
				
				AddParameter(cmd, pInt32(CustomerAddressBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerAddress), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerAddress object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerAddress object to retrieve</param>
        /// <returns>CustomerAddress object, null if not found</returns>
		public CustomerAddress Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDRESSBYID))
			{
				AddParameter( cmd, pInt32(CustomerAddressBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerAddress objects 
        /// </summary>
        /// <returns>A list of CustomerAddress objects</returns>
		public CustomerAddressList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERADDRESS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerAddress objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerAddress objects</returns>
		public CustomerAddressList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERADDRESS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerAddressList _CustomerAddressList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerAddressList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerAddress objects by query String
        /// </summary>
        /// <returns>A list of CustomerAddress objects</returns>
		public CustomerAddressList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDRESSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerAddress Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerAddress
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDRESSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerAddress Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerAddress
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerAddressRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERADDRESSROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerAddressRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerAddressRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerAddress object
        /// </summary>
        /// <param name="customerAddressObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerAddressBase customerAddressObject, SqlDataReader reader, int start)
		{
			
				customerAddressObject.Id = reader.GetInt32( start + 0 );			
				customerAddressObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerAddressObject.RefId = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerAddressObject.AddressType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerAddressObject.Street = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerAddressObject.City = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerAddressObject.State = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerAddressObject.ZipCode = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerAddressObject.County = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerAddressObject.FirstName = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerAddressObject.LastName = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerAddressObject.BusinessName = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerAddressObject.IsDefault = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) customerAddressObject.Notes = reader.GetString( start + 13 );			
			FillBaseObject(customerAddressObject, reader, (start + 14));

			
			customerAddressObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerAddress object
        /// </summary>
        /// <param name="customerAddressObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerAddressBase customerAddressObject, SqlDataReader reader)
		{
			FillObject(customerAddressObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerAddress object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerAddress object</returns>
		private CustomerAddress GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerAddress customerAddressObject= new CustomerAddress();
					FillObject(customerAddressObject, reader);
					return customerAddressObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerAddress objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerAddress objects</returns>
		private CustomerAddressList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerAddress list
			CustomerAddressList list = new CustomerAddressList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerAddress customerAddressObject = new CustomerAddress();
					FillObject(customerAddressObject, reader);

					list.Add(customerAddressObject);
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
