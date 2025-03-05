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
	public partial class CustomerContactTrackDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERCONTACTTRACK = "InsertCustomerContactTrack";
		private const string UPDATECUSTOMERCONTACTTRACK = "UpdateCustomerContactTrack";
		private const string DELETECUSTOMERCONTACTTRACK = "DeleteCustomerContactTrack";
		private const string GETCUSTOMERCONTACTTRACKBYID = "GetCustomerContactTrackById";
		private const string GETALLCUSTOMERCONTACTTRACK = "GetAllCustomerContactTrack";
		private const string GETPAGEDCUSTOMERCONTACTTRACK = "GetPagedCustomerContactTrack";
		private const string GETCUSTOMERCONTACTTRACKMAXIMUMID = "GetCustomerContactTrackMaximumId";
		private const string GETCUSTOMERCONTACTTRACKROWCOUNT = "GetCustomerContactTrackRowCount";	
		private const string GETCUSTOMERCONTACTTRACKBYQUERY = "GetCustomerContactTrackByQuery";
		#endregion
		
		#region Constructors
		public CustomerContactTrackDataAccess(ClientContext context) : base(context) { }
		public CustomerContactTrackDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerContactTrackObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerContactTrackBase customerContactTrackObject)
		{	
			AddParameter(cmd, pGuid(CustomerContactTrackBase.Property_CustomerId, customerContactTrackObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerContactTrackBase.Property_CustomerPlatform, 100, customerContactTrackObject.CustomerPlatform));
			AddParameter(cmd, pNVarChar(CustomerContactTrackBase.Property_Note, customerContactTrackObject.Note));
			AddParameter(cmd, pDateTime(CustomerContactTrackBase.Property_CreatedDate, customerContactTrackObject.CreatedDate));
			AddParameter(cmd, pInt32(CustomerContactTrackBase.Property_PlatformId, customerContactTrackObject.PlatformId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerContactTrack
        /// </summary>
        /// <param name="customerContactTrackObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerContactTrackBase customerContactTrackObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERCONTACTTRACK);
	
				AddParameter(cmd, pInt32Out(CustomerContactTrackBase.Property_Id));
				AddCommonParams(cmd, customerContactTrackObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerContactTrackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerContactTrackObject.Id = (Int32)GetOutParameter(cmd, CustomerContactTrackBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerContactTrackObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerContactTrack
        /// </summary>
        /// <param name="customerContactTrackObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerContactTrackBase customerContactTrackObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERCONTACTTRACK);
				
				AddParameter(cmd, pInt32(CustomerContactTrackBase.Property_Id, customerContactTrackObject.Id));
				AddCommonParams(cmd, customerContactTrackObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerContactTrackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerContactTrackObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerContactTrack
        /// </summary>
        /// <param name="Id">Id of the CustomerContactTrack object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERCONTACTTRACK);	
				
				AddParameter(cmd, pInt32(CustomerContactTrackBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerContactTrack), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerContactTrack object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerContactTrack object to retrieve</param>
        /// <returns>CustomerContactTrack object, null if not found</returns>
		public CustomerContactTrack Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCONTACTTRACKBYID))
			{
				AddParameter( cmd, pInt32(CustomerContactTrackBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerContactTrack objects 
        /// </summary>
        /// <returns>A list of CustomerContactTrack objects</returns>
		public CustomerContactTrackList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERCONTACTTRACK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerContactTrack objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerContactTrack objects</returns>
		public CustomerContactTrackList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERCONTACTTRACK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerContactTrackList _CustomerContactTrackList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerContactTrackList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerContactTrack objects by query String
        /// </summary>
        /// <returns>A list of CustomerContactTrack objects</returns>
		public CustomerContactTrackList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCONTACTTRACKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerContactTrack Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerContactTrack
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCONTACTTRACKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerContactTrack Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerContactTrack
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerContactTrackRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERCONTACTTRACKROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerContactTrackRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerContactTrackRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerContactTrack object
        /// </summary>
        /// <param name="customerContactTrackObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerContactTrackBase customerContactTrackObject, SqlDataReader reader, int start)
		{
			
				customerContactTrackObject.Id = reader.GetInt32( start + 0 );			
				customerContactTrackObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerContactTrackObject.CustomerPlatform = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerContactTrackObject.Note = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerContactTrackObject.CreatedDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) customerContactTrackObject.PlatformId = reader.GetInt32( start + 5 );			
			FillBaseObject(customerContactTrackObject, reader, (start + 6));

			
			customerContactTrackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerContactTrack object
        /// </summary>
        /// <param name="customerContactTrackObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerContactTrackBase customerContactTrackObject, SqlDataReader reader)
		{
			FillObject(customerContactTrackObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerContactTrack object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerContactTrack object</returns>
		private CustomerContactTrack GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerContactTrack customerContactTrackObject= new CustomerContactTrack();
					FillObject(customerContactTrackObject, reader);
					return customerContactTrackObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerContactTrack objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerContactTrack objects</returns>
		private CustomerContactTrackList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerContactTrack list
			CustomerContactTrackList list = new CustomerContactTrackList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerContactTrack customerContactTrackObject = new CustomerContactTrack();
					FillObject(customerContactTrackObject, reader);

					list.Add(customerContactTrackObject);
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
