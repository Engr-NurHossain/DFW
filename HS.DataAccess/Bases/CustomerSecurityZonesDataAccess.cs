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
	public partial class CustomerSecurityZonesDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERSECURITYZONES = "InsertCustomerSecurityZones";
		private const string UPDATECUSTOMERSECURITYZONES = "UpdateCustomerSecurityZones";
		private const string DELETECUSTOMERSECURITYZONES = "DeleteCustomerSecurityZones";
		private const string GETCUSTOMERSECURITYZONESBYID = "GetCustomerSecurityZonesByID";
		private const string GETALLCUSTOMERSECURITYZONES = "GetAllCustomerSecurityZones";
		private const string GETPAGEDCUSTOMERSECURITYZONES = "GetPagedCustomerSecurityZones";
		private const string GETCUSTOMERSECURITYZONESMAXIMUMID = "GetCustomerSecurityZonesMaximumID";
		private const string GETCUSTOMERSECURITYZONESROWCOUNT = "GetCustomerSecurityZonesRowCount";	
		private const string GETCUSTOMERSECURITYZONESBYQUERY = "GetCustomerSecurityZonesByQuery";
		#endregion
		
		#region Constructors
		public CustomerSecurityZonesDataAccess(ClientContext context) : base(context) { }
		public CustomerSecurityZonesDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerSecurityZonesObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerSecurityZonesBase customerSecurityZonesObject)
		{	
			AddParameter(cmd, pGuid(CustomerSecurityZonesBase.Property_CustomerId, customerSecurityZonesObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_ZoneNumber, 50, customerSecurityZonesObject.ZoneNumber));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_EventCode, 50, customerSecurityZonesObject.EventCode));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_Location, customerSecurityZonesObject.Location));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_Platform, 50, customerSecurityZonesObject.Platform));
			AddParameter(cmd, pDateTime(CustomerSecurityZonesBase.Property_CreatedDate, customerSecurityZonesObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerSecurityZonesBase.Property_CreatedBy, customerSecurityZonesObject.CreatedBy));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_EquipmentType, 50, customerSecurityZonesObject.EquipmentType));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_ZoneComment, 100, customerSecurityZonesObject.ZoneComment));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_SignalCode, 100, customerSecurityZonesObject.SignalCode));
			AddParameter(cmd, pNVarChar(CustomerSecurityZonesBase.Property_SignalStatus, 100, customerSecurityZonesObject.SignalStatus));
			AddParameter(cmd, pInt32(CustomerSecurityZonesBase.Property_AreaNum, customerSecurityZonesObject.AreaNum));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerSecurityZones
        /// </summary>
        /// <param name="customerSecurityZonesObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerSecurityZonesBase customerSecurityZonesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSECURITYZONES);
	
				AddParameter(cmd, pInt32Out(CustomerSecurityZonesBase.Property_ID));
				AddCommonParams(cmd, customerSecurityZonesObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerSecurityZonesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerSecurityZonesObject.ID = (Int32)GetOutParameter(cmd, CustomerSecurityZonesBase.Property_ID);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerSecurityZonesObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerSecurityZones
        /// </summary>
        /// <param name="customerSecurityZonesObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerSecurityZonesBase customerSecurityZonesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERSECURITYZONES);
				
				AddParameter(cmd, pInt32(CustomerSecurityZonesBase.Property_ID, customerSecurityZonesObject.ID));
				AddCommonParams(cmd, customerSecurityZonesObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerSecurityZonesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerSecurityZonesObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerSecurityZones
        /// </summary>
        /// <param name="ID">ID of the CustomerSecurityZones object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _ID)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERSECURITYZONES);	
				
				AddParameter(cmd, pInt32(CustomerSecurityZonesBase.Property_ID, _ID));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerSecurityZones), _ID, x);
			}
			
		}
		#endregion
		
		#region Get By ID Method
		/// <summary>
        /// Retrieves CustomerSecurityZones object using it's ID
        /// </summary>
        /// <param name="ID">The ID of the CustomerSecurityZones object to retrieve</param>
        /// <returns>CustomerSecurityZones object, null if not found</returns>
		public CustomerSecurityZones Get(Int32 _ID)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSECURITYZONESBYID))
			{
				AddParameter( cmd, pInt32(CustomerSecurityZonesBase.Property_ID, _ID));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerSecurityZones objects 
        /// </summary>
        /// <returns>A list of CustomerSecurityZones objects</returns>
		public CustomerSecurityZonesList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERSECURITYZONES))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerSecurityZones objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerSecurityZones objects</returns>
		public CustomerSecurityZonesList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERSECURITYZONES))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerSecurityZonesList _CustomerSecurityZonesList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerSecurityZonesList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerSecurityZones objects by query String
        /// </summary>
        /// <returns>A list of CustomerSecurityZones objects</returns>
		public CustomerSecurityZonesList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSECURITYZONESBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerSecurityZones Maximum ID Method
		/// <summary>
        /// Retrieves Get Maximum ID of CustomerSecurityZones
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxID()
		{
			Int32 _MaximumID = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSECURITYZONESMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumID = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumID;
		}
		
		#endregion
		
		#region Get CustomerSecurityZones Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerSecurityZones
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerSecurityZonesRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERSECURITYZONESROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerSecurityZonesRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerSecurityZonesRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerSecurityZones object
        /// </summary>
        /// <param name="customerSecurityZonesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerSecurityZonesBase customerSecurityZonesObject, SqlDataReader reader, int start)
		{
			
				customerSecurityZonesObject.ID = reader.GetInt32( start + 0 );			
				customerSecurityZonesObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerSecurityZonesObject.ZoneNumber = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerSecurityZonesObject.EventCode = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerSecurityZonesObject.Location = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerSecurityZonesObject.Platform = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerSecurityZonesObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customerSecurityZonesObject.CreatedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) customerSecurityZonesObject.EquipmentType = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerSecurityZonesObject.ZoneComment = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) customerSecurityZonesObject.SignalCode = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerSecurityZonesObject.SignalStatus = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerSecurityZonesObject.AreaNum = reader.GetInt32( start + 12 );			
			FillBaseObject(customerSecurityZonesObject, reader, (start + 13));

			
			customerSecurityZonesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerSecurityZones object
        /// </summary>
        /// <param name="customerSecurityZonesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerSecurityZonesBase customerSecurityZonesObject, SqlDataReader reader)
		{
			FillObject(customerSecurityZonesObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerSecurityZones object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerSecurityZones object</returns>
		private CustomerSecurityZones GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerSecurityZones customerSecurityZonesObject= new CustomerSecurityZones();
					FillObject(customerSecurityZonesObject, reader);
					return customerSecurityZonesObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerSecurityZones objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerSecurityZones objects</returns>
		private CustomerSecurityZonesList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerSecurityZones list
			CustomerSecurityZonesList list = new CustomerSecurityZonesList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerSecurityZones customerSecurityZonesObject = new CustomerSecurityZones();
					FillObject(customerSecurityZonesObject, reader);

					list.Add(customerSecurityZonesObject);
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
