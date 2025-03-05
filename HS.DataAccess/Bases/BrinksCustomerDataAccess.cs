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
	public partial class BrinksCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBRINKSCUSTOMER = "InsertBrinksCustomer";
		private const string UPDATEBRINKSCUSTOMER = "UpdateBrinksCustomer";
		private const string DELETEBRINKSCUSTOMER = "DeleteBrinksCustomer";
		private const string GETBRINKSCUSTOMERBYID = "GetBrinksCustomerByID";
		private const string GETALLBRINKSCUSTOMER = "GetAllBrinksCustomer";
		private const string GETPAGEDBRINKSCUSTOMER = "GetPagedBrinksCustomer";
		private const string GETBRINKSCUSTOMERMAXIMUMID = "GetBrinksCustomerMaximumID";
		private const string GETBRINKSCUSTOMERROWCOUNT = "GetBrinksCustomerRowCount";	
		private const string GETBRINKSCUSTOMERBYQUERY = "GetBrinksCustomerByQuery";
		#endregion
		
		#region Constructors
		public BrinksCustomerDataAccess(ClientContext context) : base(context) { }
		public BrinksCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="brinksCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, BrinksCustomerBase brinksCustomerObject)
		{	
			//AddParameter(cmd, pGuid(BrinksCustomerBase.Property_CustomerId, brinksCustomerObject.CustomerId));
			AddParameter(cmd, pInt32(BrinksCustomerBase.Property_CustomerNumber, brinksCustomerObject.CustomerNumber));
			AddParameter(cmd, pNVarChar(BrinksCustomerBase.Property_FirstName, brinksCustomerObject.FirstName));
			AddParameter(cmd, pNVarChar(BrinksCustomerBase.Property_LastName, brinksCustomerObject.LastName));
			AddParameter(cmd, pNVarChar(BrinksCustomerBase.Property_TransectionID, brinksCustomerObject.TransectionID));
			AddParameter(cmd, pDateTime(BrinksCustomerBase.Property_DateSubmitted, brinksCustomerObject.DateSubmitted));
			AddParameter(cmd, pNVarChar(BrinksCustomerBase.Property_Contact, brinksCustomerObject.Contact));
			AddParameter(cmd, pNVarChar(BrinksCustomerBase.Property_DealerNumber, brinksCustomerObject.DealerNumber));
			AddParameter(cmd, pDateTime(BrinksCustomerBase.Property_AccountOnlineDate, brinksCustomerObject.AccountOnlineDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BrinksCustomer
        /// </summary>
        /// <param name="brinksCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BrinksCustomerBase brinksCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBRINKSCUSTOMER);
	
				AddParameter(cmd, pInt32Out(BrinksCustomerBase.Property_ID));
				AddCommonParams(cmd, brinksCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					brinksCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					brinksCustomerObject.ID = (Int32)GetOutParameter(cmd, BrinksCustomerBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(brinksCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BrinksCustomer
        /// </summary>
        /// <param name="brinksCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BrinksCustomerBase brinksCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBRINKSCUSTOMER);
				
				AddParameter(cmd, pInt32(BrinksCustomerBase.Property_ID, brinksCustomerObject.ID));
				AddCommonParams(cmd, brinksCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					brinksCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(brinksCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BrinksCustomer
        /// </summary>
        /// <param name="ID">ID of the BrinksCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBRINKSCUSTOMER);	
				
				AddParameter(cmd, pInt32(BrinksCustomerBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BrinksCustomer), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves BrinksCustomer object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the BrinksCustomer object to retrieve</param>
        /// <returns>BrinksCustomer object, null if not found</returns>
		public BrinksCustomer Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETBRINKSCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(BrinksCustomerBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BrinksCustomer objects 
        /// </summary>
        /// <returns>A list of BrinksCustomer objects</returns>
		public BrinksCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBRINKSCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BrinksCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of BrinksCustomer objects</returns>
		public BrinksCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBRINKSCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BrinksCustomerList _BrinksCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BrinksCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all BrinksCustomer objects by query String
        /// </summary>
        /// <returns>A list of BrinksCustomer objects</returns>
		public BrinksCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBRINKSCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BrinksCustomer Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of BrinksCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBRINKSCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get BrinksCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BrinksCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BrinksCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBRINKSCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_BrinksCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BrinksCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BrinksCustomer object
        /// </summary>
        /// <param name="brinksCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BrinksCustomerBase brinksCustomerObject, SqlDataReader reader, int start)
		{
			
				brinksCustomerObject.ID = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) brinksCustomerObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) brinksCustomerObject.CustomerNumber = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) brinksCustomerObject.FirstName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) brinksCustomerObject.LastName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) brinksCustomerObject.TransectionID = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) brinksCustomerObject.DateSubmitted = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) brinksCustomerObject.Contact = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) brinksCustomerObject.DealerNumber = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) brinksCustomerObject.AccountOnlineDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(brinksCustomerObject, reader, (start + 10));

			
			brinksCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BrinksCustomer object
        /// </summary>
        /// <param name="brinksCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BrinksCustomerBase brinksCustomerObject, SqlDataReader reader)
		{
			FillObject(brinksCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BrinksCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BrinksCustomer object</returns>
		private BrinksCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BrinksCustomer brinksCustomerObject= new BrinksCustomer();
					FillObject(brinksCustomerObject, reader);
					return brinksCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BrinksCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BrinksCustomer objects</returns>
		private BrinksCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BrinksCustomer list
			BrinksCustomerList list = new BrinksCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BrinksCustomer brinksCustomerObject = new BrinksCustomer();
					FillObject(brinksCustomerObject, reader);

					list.Add(brinksCustomerObject);
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
