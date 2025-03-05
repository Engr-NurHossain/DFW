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
	public partial class CustomerThirdPartyAgencyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERTHIRDPARTYAGENCY = "InsertCustomerThirdPartyAgency";
		private const string UPDATECUSTOMERTHIRDPARTYAGENCY = "UpdateCustomerThirdPartyAgency";
		private const string DELETECUSTOMERTHIRDPARTYAGENCY = "DeleteCustomerThirdPartyAgency";
		private const string GETCUSTOMERTHIRDPARTYAGENCYBYID = "GetCustomerThirdPartyAgencyById";
		private const string GETALLCUSTOMERTHIRDPARTYAGENCY = "GetAllCustomerThirdPartyAgency";
		private const string GETPAGEDCUSTOMERTHIRDPARTYAGENCY = "GetPagedCustomerThirdPartyAgency";
		private const string GETCUSTOMERTHIRDPARTYAGENCYMAXIMUMID = "GetCustomerThirdPartyAgencyMaximumId";
		private const string GETCUSTOMERTHIRDPARTYAGENCYROWCOUNT = "GetCustomerThirdPartyAgencyRowCount";	
		private const string GETCUSTOMERTHIRDPARTYAGENCYBYQUERY = "GetCustomerThirdPartyAgencyByQuery";
		#endregion
		
		#region Constructors
		public CustomerThirdPartyAgencyDataAccess(ClientContext context) : base(context) { }
		public CustomerThirdPartyAgencyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerThirdPartyAgencyObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerThirdPartyAgencyBase customerThirdPartyAgencyObject)
		{	
			AddParameter(cmd, pGuid(CustomerThirdPartyAgencyBase.Property_CustomerId, customerThirdPartyAgencyObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_Agencytype, 50, customerThirdPartyAgencyObject.Agencytype));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_Phone, 50, customerThirdPartyAgencyObject.Phone));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_PermitNo, 50, customerThirdPartyAgencyObject.PermitNo));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_PermType, 50, customerThirdPartyAgencyObject.PermType));
			AddParameter(cmd, pDateTime(CustomerThirdPartyAgencyBase.Property_EffectiveDate, customerThirdPartyAgencyObject.EffectiveDate));
			AddParameter(cmd, pDateTime(CustomerThirdPartyAgencyBase.Property_ExpireDate, customerThirdPartyAgencyObject.ExpireDate));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_Platform, 50, customerThirdPartyAgencyObject.Platform));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_AgencyNo, 100, customerThirdPartyAgencyObject.AgencyNo));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_City, 100, customerThirdPartyAgencyObject.City));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_State, 100, customerThirdPartyAgencyObject.State));
			AddParameter(cmd, pNVarChar(CustomerThirdPartyAgencyBase.Property_AgencyName, 100, customerThirdPartyAgencyObject.AgencyName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerThirdPartyAgency
        /// </summary>
        /// <param name="customerThirdPartyAgencyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerThirdPartyAgencyBase customerThirdPartyAgencyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERTHIRDPARTYAGENCY);
	
				AddParameter(cmd, pInt32Out(CustomerThirdPartyAgencyBase.Property_Id));
				AddCommonParams(cmd, customerThirdPartyAgencyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerThirdPartyAgencyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerThirdPartyAgencyObject.Id = (Int32)GetOutParameter(cmd, CustomerThirdPartyAgencyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerThirdPartyAgencyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerThirdPartyAgency
        /// </summary>
        /// <param name="customerThirdPartyAgencyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerThirdPartyAgencyBase customerThirdPartyAgencyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERTHIRDPARTYAGENCY);
				
				AddParameter(cmd, pInt32(CustomerThirdPartyAgencyBase.Property_Id, customerThirdPartyAgencyObject.Id));
				AddCommonParams(cmd, customerThirdPartyAgencyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerThirdPartyAgencyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerThirdPartyAgencyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerThirdPartyAgency
        /// </summary>
        /// <param name="Id">Id of the CustomerThirdPartyAgency object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERTHIRDPARTYAGENCY);	
				
				AddParameter(cmd, pInt32(CustomerThirdPartyAgencyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerThirdPartyAgency), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerThirdPartyAgency object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerThirdPartyAgency object to retrieve</param>
        /// <returns>CustomerThirdPartyAgency object, null if not found</returns>
		public CustomerThirdPartyAgency Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERTHIRDPARTYAGENCYBYID))
			{
				AddParameter( cmd, pInt32(CustomerThirdPartyAgencyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerThirdPartyAgency objects 
        /// </summary>
        /// <returns>A list of CustomerThirdPartyAgency objects</returns>
		public CustomerThirdPartyAgencyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERTHIRDPARTYAGENCY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerThirdPartyAgency objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerThirdPartyAgency objects</returns>
		public CustomerThirdPartyAgencyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERTHIRDPARTYAGENCY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerThirdPartyAgencyList _CustomerThirdPartyAgencyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerThirdPartyAgencyList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerThirdPartyAgency objects by query String
        /// </summary>
        /// <returns>A list of CustomerThirdPartyAgency objects</returns>
		public CustomerThirdPartyAgencyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERTHIRDPARTYAGENCYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerThirdPartyAgency Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerThirdPartyAgency
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERTHIRDPARTYAGENCYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerThirdPartyAgency Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerThirdPartyAgency
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerThirdPartyAgencyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERTHIRDPARTYAGENCYROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerThirdPartyAgencyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerThirdPartyAgencyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerThirdPartyAgency object
        /// </summary>
        /// <param name="customerThirdPartyAgencyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerThirdPartyAgencyBase customerThirdPartyAgencyObject, SqlDataReader reader, int start)
		{
			
				customerThirdPartyAgencyObject.Id = reader.GetInt32( start + 0 );			
				customerThirdPartyAgencyObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerThirdPartyAgencyObject.Agencytype = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerThirdPartyAgencyObject.Phone = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerThirdPartyAgencyObject.PermitNo = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerThirdPartyAgencyObject.PermType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerThirdPartyAgencyObject.EffectiveDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) customerThirdPartyAgencyObject.ExpireDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) customerThirdPartyAgencyObject.Platform = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerThirdPartyAgencyObject.AgencyNo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerThirdPartyAgencyObject.City = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerThirdPartyAgencyObject.State = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerThirdPartyAgencyObject.AgencyName = reader.GetString( start + 12 );			
			FillBaseObject(customerThirdPartyAgencyObject, reader, (start + 13));

			
			customerThirdPartyAgencyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerThirdPartyAgency object
        /// </summary>
        /// <param name="customerThirdPartyAgencyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerThirdPartyAgencyBase customerThirdPartyAgencyObject, SqlDataReader reader)
		{
			FillObject(customerThirdPartyAgencyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerThirdPartyAgency object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerThirdPartyAgency object</returns>
		private CustomerThirdPartyAgency GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerThirdPartyAgency customerThirdPartyAgencyObject= new CustomerThirdPartyAgency();
					FillObject(customerThirdPartyAgencyObject, reader);
					return customerThirdPartyAgencyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerThirdPartyAgency objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerThirdPartyAgency objects</returns>
		private CustomerThirdPartyAgencyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerThirdPartyAgency list
			CustomerThirdPartyAgencyList list = new CustomerThirdPartyAgencyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerThirdPartyAgency customerThirdPartyAgencyObject = new CustomerThirdPartyAgency();
					FillObject(customerThirdPartyAgencyObject, reader);

					list.Add(customerThirdPartyAgencyObject);
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
