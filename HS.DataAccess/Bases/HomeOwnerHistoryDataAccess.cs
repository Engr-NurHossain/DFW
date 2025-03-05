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
	public partial class HomeOwnerHistoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTHOMEOWNERHISTORY = "InsertHomeOwnerHistory";
		private const string UPDATEHOMEOWNERHISTORY = "UpdateHomeOwnerHistory";
		private const string DELETEHOMEOWNERHISTORY = "DeleteHomeOwnerHistory";
		private const string GETHOMEOWNERHISTORYBYID = "GetHomeOwnerHistoryById";
		private const string GETALLHOMEOWNERHISTORY = "GetAllHomeOwnerHistory";
		private const string GETPAGEDHOMEOWNERHISTORY = "GetPagedHomeOwnerHistory";
		private const string GETHOMEOWNERHISTORYMAXIMUMID = "GetHomeOwnerHistoryMaximumId";
		private const string GETHOMEOWNERHISTORYROWCOUNT = "GetHomeOwnerHistoryRowCount";	
		private const string GETHOMEOWNERHISTORYBYQUERY = "GetHomeOwnerHistoryByQuery";
		#endregion
		
		#region Constructors
		public HomeOwnerHistoryDataAccess(ClientContext context) : base(context) { }
		public HomeOwnerHistoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="homeOwnerHistoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, HomeOwnerHistoryBase homeOwnerHistoryObject)
		{	
			AddParameter(cmd, pGuid(HomeOwnerHistoryBase.Property_CustomerId, homeOwnerHistoryObject.CustomerId));
			AddParameter(cmd, pNVarChar(HomeOwnerHistoryBase.Property_HomeOwnerName, 50, homeOwnerHistoryObject.HomeOwnerName));
			AddParameter(cmd, pNVarChar(HomeOwnerHistoryBase.Property_OwnerAddress, 100, homeOwnerHistoryObject.OwnerAddress));
			AddParameter(cmd, pDateTime(HomeOwnerHistoryBase.Property_RequestedDate, homeOwnerHistoryObject.RequestedDate));
			AddParameter(cmd, pGuid(HomeOwnerHistoryBase.Property_RequestedBy, homeOwnerHistoryObject.RequestedBy));
			AddParameter(cmd, pGuid(HomeOwnerHistoryBase.Property_CompanyId, homeOwnerHistoryObject.CompanyId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts HomeOwnerHistory
        /// </summary>
        /// <param name="homeOwnerHistoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(HomeOwnerHistoryBase homeOwnerHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTHOMEOWNERHISTORY);
	
				AddParameter(cmd, pInt32Out(HomeOwnerHistoryBase.Property_Id));
				AddCommonParams(cmd, homeOwnerHistoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					homeOwnerHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					homeOwnerHistoryObject.Id = (Int32)GetOutParameter(cmd, HomeOwnerHistoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(homeOwnerHistoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates HomeOwnerHistory
        /// </summary>
        /// <param name="homeOwnerHistoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(HomeOwnerHistoryBase homeOwnerHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEHOMEOWNERHISTORY);
				
				AddParameter(cmd, pInt32(HomeOwnerHistoryBase.Property_Id, homeOwnerHistoryObject.Id));
				AddCommonParams(cmd, homeOwnerHistoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					homeOwnerHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(homeOwnerHistoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes HomeOwnerHistory
        /// </summary>
        /// <param name="Id">Id of the HomeOwnerHistory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEHOMEOWNERHISTORY);	
				
				AddParameter(cmd, pInt32(HomeOwnerHistoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(HomeOwnerHistory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves HomeOwnerHistory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the HomeOwnerHistory object to retrieve</param>
        /// <returns>HomeOwnerHistory object, null if not found</returns>
		public HomeOwnerHistory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETHOMEOWNERHISTORYBYID))
			{
				AddParameter( cmd, pInt32(HomeOwnerHistoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all HomeOwnerHistory objects 
        /// </summary>
        /// <returns>A list of HomeOwnerHistory objects</returns>
		public HomeOwnerHistoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLHOMEOWNERHISTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all HomeOwnerHistory objects by PageRequest
        /// </summary>
        /// <returns>A list of HomeOwnerHistory objects</returns>
		public HomeOwnerHistoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDHOMEOWNERHISTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				HomeOwnerHistoryList _HomeOwnerHistoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _HomeOwnerHistoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all HomeOwnerHistory objects by query String
        /// </summary>
        /// <returns>A list of HomeOwnerHistory objects</returns>
		public HomeOwnerHistoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETHOMEOWNERHISTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get HomeOwnerHistory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of HomeOwnerHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETHOMEOWNERHISTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get HomeOwnerHistory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of HomeOwnerHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _HomeOwnerHistoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETHOMEOWNERHISTORYROWCOUNT))
			{
				SqlDataReader reader;
				_HomeOwnerHistoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _HomeOwnerHistoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills HomeOwnerHistory object
        /// </summary>
        /// <param name="homeOwnerHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(HomeOwnerHistoryBase homeOwnerHistoryObject, SqlDataReader reader, int start)
		{
			
				homeOwnerHistoryObject.Id = reader.GetInt32( start + 0 );			
				homeOwnerHistoryObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) homeOwnerHistoryObject.HomeOwnerName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) homeOwnerHistoryObject.OwnerAddress = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) homeOwnerHistoryObject.RequestedDate = reader.GetDateTime( start + 4 );			
				homeOwnerHistoryObject.RequestedBy = reader.GetGuid( start + 5 );			
				homeOwnerHistoryObject.CompanyId = reader.GetGuid( start + 6 );			
			FillBaseObject(homeOwnerHistoryObject, reader, (start + 7));

			
			homeOwnerHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills HomeOwnerHistory object
        /// </summary>
        /// <param name="homeOwnerHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(HomeOwnerHistoryBase homeOwnerHistoryObject, SqlDataReader reader)
		{
			FillObject(homeOwnerHistoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves HomeOwnerHistory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>HomeOwnerHistory object</returns>
		private HomeOwnerHistory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					HomeOwnerHistory homeOwnerHistoryObject= new HomeOwnerHistory();
					FillObject(homeOwnerHistoryObject, reader);
					return homeOwnerHistoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of HomeOwnerHistory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of HomeOwnerHistory objects</returns>
		private HomeOwnerHistoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//HomeOwnerHistory list
			HomeOwnerHistoryList list = new HomeOwnerHistoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					HomeOwnerHistory homeOwnerHistoryObject = new HomeOwnerHistory();
					FillObject(homeOwnerHistoryObject, reader);

					list.Add(homeOwnerHistoryObject);
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
