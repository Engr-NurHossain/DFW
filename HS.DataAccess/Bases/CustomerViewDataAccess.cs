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
	public partial class CustomerViewDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERVIEW = "InsertCustomerView";
		private const string UPDATECUSTOMERVIEW = "UpdateCustomerView";
		private const string DELETECUSTOMERVIEW = "DeleteCustomerView";
		private const string GETCUSTOMERVIEWBYID = "GetCustomerViewById";
		private const string GETALLCUSTOMERVIEW = "GetAllCustomerView";
		private const string GETPAGEDCUSTOMERVIEW = "GetPagedCustomerView";
		private const string GETCUSTOMERVIEWMAXIMUMID = "GetCustomerViewMaximumId";
		private const string GETCUSTOMERVIEWROWCOUNT = "GetCustomerViewRowCount";	
		private const string GETCUSTOMERVIEWBYQUERY = "GetCustomerViewByQuery";
		#endregion
		
		#region Constructors
		public CustomerViewDataAccess(ClientContext context) : base(context) { }
		public CustomerViewDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerViewObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerViewBase customerViewObject)
		{	
			AddParameter(cmd, pGuid(CustomerViewBase.Property_CompanyId, customerViewObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerViewBase.Property_CustomerId, customerViewObject.CustomerId));
			AddParameter(cmd, pDateTime(CustomerViewBase.Property_LastVistited, customerViewObject.LastVistited));
			AddParameter(cmd, pNVarChar(CustomerViewBase.Property_LastVisitedBy, 50, customerViewObject.LastVisitedBy));
			AddParameter(cmd, pGuid(CustomerViewBase.Property_LastVisitedByUId, customerViewObject.LastVisitedByUId));
			AddParameter(cmd, pBool(CustomerViewBase.Property_IsLead, customerViewObject.IsLead));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerView
        /// </summary>
        /// <param name="customerViewObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerViewBase customerViewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERVIEW);
	
				AddParameter(cmd, pInt32Out(CustomerViewBase.Property_Id));
				AddCommonParams(cmd, customerViewObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerViewObject.Id = (Int32)GetOutParameter(cmd, CustomerViewBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerViewObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerView
        /// </summary>
        /// <param name="customerViewObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerViewBase customerViewObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERVIEW);
				
				AddParameter(cmd, pInt32(CustomerViewBase.Property_Id, customerViewObject.Id));
				AddCommonParams(cmd, customerViewObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerViewObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerView
        /// </summary>
        /// <param name="Id">Id of the CustomerView object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERVIEW);	
				
				AddParameter(cmd, pInt32(CustomerViewBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerView), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerView object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerView object to retrieve</param>
        /// <returns>CustomerView object, null if not found</returns>
		public CustomerView Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVIEWBYID))
			{
				AddParameter( cmd, pInt32(CustomerViewBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerView objects 
        /// </summary>
        /// <returns>A list of CustomerView objects</returns>
		public CustomerViewList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERVIEW))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerView objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerView objects</returns>
		public CustomerViewList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERVIEW))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerViewList _CustomerViewList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerViewList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerView objects by query String
        /// </summary>
        /// <returns>A list of CustomerView objects</returns>
		public CustomerViewList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVIEWBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerView Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerView
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVIEWMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerView Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerView
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerViewRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERVIEWROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerViewRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerViewRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerView object
        /// </summary>
        /// <param name="customerViewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerViewBase customerViewObject, SqlDataReader reader, int start)
		{
			
				customerViewObject.Id = reader.GetInt32( start + 0 );			
				customerViewObject.CompanyId = reader.GetGuid( start + 1 );			
				customerViewObject.CustomerId = reader.GetGuid( start + 2 );			
				customerViewObject.LastVistited = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) customerViewObject.LastVisitedBy = reader.GetString( start + 4 );			
				customerViewObject.LastVisitedByUId = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) customerViewObject.IsLead = reader.GetBoolean( start + 6 );			
			FillBaseObject(customerViewObject, reader, (start + 7));

			
			customerViewObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerView object
        /// </summary>
        /// <param name="customerViewObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerViewBase customerViewObject, SqlDataReader reader)
		{
			FillObject(customerViewObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerView object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerView object</returns>
		private CustomerView GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerView customerViewObject= new CustomerView();
					FillObject(customerViewObject, reader);
					return customerViewObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerView objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerView objects</returns>
		private CustomerViewList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerView list
			CustomerViewList list = new CustomerViewList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerView customerViewObject = new CustomerView();
					FillObject(customerViewObject, reader);

					list.Add(customerViewObject);
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
