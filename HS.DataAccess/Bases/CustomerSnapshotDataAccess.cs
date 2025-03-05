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
	public partial class CustomerSnapshotDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSNAPSHOT = "InsertCustomerSnapshot";
		private const string UPDATECUSTOMERSNAPSHOT = "UpdateCustomerSnapshot";
		private const string DELETECUSTOMERSNAPSHOT = "DeleteCustomerSnapshot";
		private const string GETCUSTOMERSNAPSHOTBYID = "GetCustomerSnapshotById";
		private const string GETALLCUSTOMERSNAPSHOT = "GetAllCustomerSnapshot";
		private const string GETPAGEDCUSTOMERSNAPSHOT = "GetPagedCustomerSnapshot";
		private const string GETCUSTOMERSNAPSHOTMAXIMUMID = "GetCustomerSnapshotMaximumId";
		private const string GETCUSTOMERSNAPSHOTROWCOUNT = "GetCustomerSnapshotRowCount";	
		private const string GETCUSTOMERSNAPSHOTBYQUERY = "GetCustomerSnapshotByQuery";
        private const string PushCustomerSnapshot = "PushCustomerSnapshot";
        #endregion

        #region Constructors
        public CustomerSnapshotDataAccess(ClientContext context) : base(context) { }
		public CustomerSnapshotDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSnapshotObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSnapshotBase customerSnapshotObject)
		{	
			AddParameter(cmd, pGuid(CustomerSnapshotBase.Property_CustomerId, customerSnapshotObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerSnapshotBase.Property_CompanyId, customerSnapshotObject.CompanyId));
			AddParameter(cmd, pNVarChar(CustomerSnapshotBase.Property_Description, 500, customerSnapshotObject.Description));
			AddParameter(cmd, pDateTime(CustomerSnapshotBase.Property_Logdate, customerSnapshotObject.Logdate));
			AddParameter(cmd, pNVarChar(CustomerSnapshotBase.Property_Updatedby, 50, customerSnapshotObject.Updatedby));
			AddParameter(cmd, pNVarChar(CustomerSnapshotBase.Property_Type, 50, customerSnapshotObject.Type));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSnapshot
        /// </summary>
        /// <param name="customerSnapshotObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSnapshotBase customerSnapshotObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSNAPSHOT);
	
				AddParameter(cmd, pInt32Out(CustomerSnapshotBase.Property_Id));
				AddCommonParams(cmd, customerSnapshotObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSnapshotObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSnapshotObject.Id = (Int32)GetOutParameter(cmd, CustomerSnapshotBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSnapshotObject, x);
			}
		}

        public long Push(CustomerSnapshotBase customerSnapshotObject, string InvoiceId)
        {
			long result = 0;
            try
            {
                SqlCommand cmd = GetSPCommand(PushCustomerSnapshot);

                AddParameter(cmd, pInt32Out(CustomerSnapshotBase.Property_Id));
                AddCommonParams(cmd, customerSnapshotObject);
				AddParameter(cmd, pVarChar("EntityId",InvoiceId));

                result = InsertRecord(cmd);
                if (result > 0)
                {
                    customerSnapshotObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                    customerSnapshotObject.Id = (Int32)GetOutParameter(cmd, CustomerSnapshotBase.Property_Id);
                }
            }
            catch (SqlException x)
            {
				logger.Error(x);
                //throw new ObjectInsertException(customerSnapshotObject, x);
            }
            return result;
        }

        #endregion

        #region Update Method
        /// <summary>
        /// Updates CustomerSnapshot
        /// </summary>
        /// <param name="customerSnapshotObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
        public long Update(CustomerSnapshotBase customerSnapshotObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSNAPSHOT);
				
				AddParameter(cmd, pInt32(CustomerSnapshotBase.Property_Id, customerSnapshotObject.Id));
				AddCommonParams(cmd, customerSnapshotObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSnapshotObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSnapshotObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSnapshot
        /// </summary>
        /// <param name="Id">Id of the CustomerSnapshot object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSNAPSHOT);	
				
				AddParameter(cmd, pInt32(CustomerSnapshotBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSnapshot), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerSnapshot object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerSnapshot object to retrieve</param>
        /// <returns>CustomerSnapshot object, null if not found</returns>
		public CustomerSnapshot Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSNAPSHOTBYID))
			{
				AddParameter( cmd, pInt32(CustomerSnapshotBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSnapshot objects 
        /// </summary>
        /// <returns>A list of CustomerSnapshot objects</returns>
		public CustomerSnapshotList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSNAPSHOT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSnapshot objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSnapshot objects</returns>
		public CustomerSnapshotList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSNAPSHOT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSnapshotList _CustomerSnapshotList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSnapshotList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSnapshot objects by query String
        /// </summary>
        /// <returns>A list of CustomerSnapshot objects</returns>
		public CustomerSnapshotList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSNAPSHOTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSnapshot Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerSnapshot
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSNAPSHOTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerSnapshot Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSnapshot
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSnapshotRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSNAPSHOTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSnapshotRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSnapshotRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSnapshot object
        /// </summary>
        /// <param name="customerSnapshotObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSnapshotBase customerSnapshotObject, SqlDataReader reader, int start)
		{
			
				customerSnapshotObject.Id = reader.GetInt32( start + 0 );			
				customerSnapshotObject.CustomerId = reader.GetGuid( start + 1 );			
				customerSnapshotObject.CompanyId = reader.GetGuid( start + 2 );			
				customerSnapshotObject.Description = reader.GetString( start + 3 );			
				customerSnapshotObject.Logdate = reader.GetDateTime( start + 4 );			
				customerSnapshotObject.Updatedby = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerSnapshotObject.Type = reader.GetString( start + 6 );			
			FillBaseObject(customerSnapshotObject, reader, (start + 7));

			
			customerSnapshotObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSnapshot object
        /// </summary>
        /// <param name="customerSnapshotObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSnapshotBase customerSnapshotObject, SqlDataReader reader)
		{
			FillObject(customerSnapshotObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSnapshot object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSnapshot object</returns>
		private CustomerSnapshot GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSnapshot customerSnapshotObject= new CustomerSnapshot();
					FillObject(customerSnapshotObject, reader);
					return customerSnapshotObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSnapshot objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSnapshot objects</returns>
		private CustomerSnapshotList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSnapshot list
			CustomerSnapshotList list = new CustomerSnapshotList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSnapshot customerSnapshotObject = new CustomerSnapshot();
					FillObject(customerSnapshotObject, reader);

					list.Add(customerSnapshotObject);
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
