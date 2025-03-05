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
	public partial class SetupAlarmDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSETUPALARM = "InsertSetupAlarm";
		private const string UPDATESETUPALARM = "UpdateSetupAlarm";
		private const string DELETESETUPALARM = "DeleteSetupAlarm";
		private const string GETSETUPALARMBYID = "GetSetupAlarmById";
		private const string GETALLSETUPALARM = "GetAllSetupAlarm";
		private const string GETPAGEDSETUPALARM = "GetPagedSetupAlarm";
		private const string GETSETUPALARMMAXIMUMID = "GetSetupAlarmMaximumId";
		private const string GETSETUPALARMROWCOUNT = "GetSetupAlarmRowCount";	
		private const string GETSETUPALARMBYQUERY = "GetSetupAlarmByQuery";
		#endregion
		
		#region Constructors
		public SetupAlarmDataAccess(ClientContext context) : base(context) { }
		public SetupAlarmDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="setupAlarmObject"></param>
		private void AddCommonParams(SqlCommand cmd, SetupAlarmBase setupAlarmObject)
		{	
			AddParameter(cmd, pGuid(SetupAlarmBase.Property_CustomerId, setupAlarmObject.CustomerId));
			AddParameter(cmd, pGuid(SetupAlarmBase.Property_CompanyId, setupAlarmObject.CompanyId));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_PropertyType, 50, setupAlarmObject.PropertyType));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_EmailAddress, 50, setupAlarmObject.EmailAddress));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_Phone, 50, setupAlarmObject.Phone));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_DealerCustomer, 50, setupAlarmObject.DealerCustomer));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_LoginName, 50, setupAlarmObject.LoginName));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_Password, 50, setupAlarmObject.Password));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsStreet, 250, setupAlarmObject.InsStreet));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsState, 50, setupAlarmObject.InsState));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsCity, 50, setupAlarmObject.InsCity));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsZip, 50, setupAlarmObject.InsZip));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsCountry, 50, setupAlarmObject.InsCountry));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_InsTimeZone, 50, setupAlarmObject.InsTimeZone));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_Culture, 50, setupAlarmObject.Culture));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_PanelType, 50, setupAlarmObject.PanelType));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_PanelVersion, 50, setupAlarmObject.PanelVersion));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_ModelSerialNumber, 50, setupAlarmObject.ModelSerialNumber));
			AddParameter(cmd, pBool(SetupAlarmBase.Property_PhoneLinePresent, setupAlarmObject.PhoneLinePresent));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_CentrastationForwardingOption, 50, setupAlarmObject.CentrastationForwardingOption));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_CentralStationAccountNo, 50, setupAlarmObject.CentralStationAccountNo));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_CentralStationRecieverNumber, 50, setupAlarmObject.CentralStationRecieverNumber));
			AddParameter(cmd, pInt32(SetupAlarmBase.Property_PackageId, setupAlarmObject.PackageId));
			AddParameter(cmd, pBool(SetupAlarmBase.Property_IgnoreLowCoverageError, setupAlarmObject.IgnoreLowCoverageError));
			AddParameter(cmd, pNVarChar(SetupAlarmBase.Property_CustomerStatus, 50, setupAlarmObject.CustomerStatus));
			AddParameter(cmd, pGuid(SetupAlarmBase.Property_CreatedBy, setupAlarmObject.CreatedBy));
			AddParameter(cmd, pDateTime(SetupAlarmBase.Property_CreatedDate, setupAlarmObject.CreatedDate));
			AddParameter(cmd, pGuid(SetupAlarmBase.Property_LastUpdatedBy, setupAlarmObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(SetupAlarmBase.Property_LastUpdatedDate, setupAlarmObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SetupAlarm
        /// </summary>
        /// <param name="setupAlarmObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SetupAlarmBase setupAlarmObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSETUPALARM);
	
				AddParameter(cmd, pInt32Out(SetupAlarmBase.Property_Id));
				AddCommonParams(cmd, setupAlarmObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					setupAlarmObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					setupAlarmObject.Id = (Int32)GetOutParameter(cmd, SetupAlarmBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(setupAlarmObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SetupAlarm
        /// </summary>
        /// <param name="setupAlarmObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SetupAlarmBase setupAlarmObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESETUPALARM);
				
				AddParameter(cmd, pInt32(SetupAlarmBase.Property_Id, setupAlarmObject.Id));
				AddCommonParams(cmd, setupAlarmObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					setupAlarmObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(setupAlarmObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SetupAlarm
        /// </summary>
        /// <param name="Id">Id of the SetupAlarm object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESETUPALARM);	
				
				AddParameter(cmd, pInt32(SetupAlarmBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SetupAlarm), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SetupAlarm object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SetupAlarm object to retrieve</param>
        /// <returns>SetupAlarm object, null if not found</returns>
		public SetupAlarm Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSETUPALARMBYID))
			{
				AddParameter( cmd, pInt32(SetupAlarmBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SetupAlarm objects 
        /// </summary>
        /// <returns>A list of SetupAlarm objects</returns>
		public SetupAlarmList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSETUPALARM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SetupAlarm objects by PageRequest
        /// </summary>
        /// <returns>A list of SetupAlarm objects</returns>
		public SetupAlarmList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSETUPALARM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SetupAlarmList _SetupAlarmList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SetupAlarmList;
			}
		}
		
		/// <summary>
        /// Retrieves all SetupAlarm objects by query String
        /// </summary>
        /// <returns>A list of SetupAlarm objects</returns>
		public SetupAlarmList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSETUPALARMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SetupAlarm Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SetupAlarm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSETUPALARMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SetupAlarm Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SetupAlarm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SetupAlarmRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSETUPALARMROWCOUNT))
			{
				SqlDataReader reader;
				_SetupAlarmRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SetupAlarmRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SetupAlarm object
        /// </summary>
        /// <param name="setupAlarmObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SetupAlarmBase setupAlarmObject, SqlDataReader reader, int start)
		{
			
				setupAlarmObject.Id = reader.GetInt32( start + 0 );			
				setupAlarmObject.CustomerId = reader.GetGuid( start + 1 );			
				setupAlarmObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) setupAlarmObject.PropertyType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) setupAlarmObject.EmailAddress = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) setupAlarmObject.Phone = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) setupAlarmObject.DealerCustomer = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) setupAlarmObject.LoginName = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) setupAlarmObject.Password = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) setupAlarmObject.InsStreet = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) setupAlarmObject.InsState = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) setupAlarmObject.InsCity = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) setupAlarmObject.InsZip = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) setupAlarmObject.InsCountry = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) setupAlarmObject.InsTimeZone = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) setupAlarmObject.Culture = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) setupAlarmObject.PanelType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) setupAlarmObject.PanelVersion = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) setupAlarmObject.ModelSerialNumber = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) setupAlarmObject.PhoneLinePresent = reader.GetBoolean( start + 19 );			
				if(!reader.IsDBNull(20)) setupAlarmObject.CentrastationForwardingOption = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) setupAlarmObject.CentralStationAccountNo = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) setupAlarmObject.CentralStationRecieverNumber = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) setupAlarmObject.PackageId = reader.GetInt32( start + 23 );			
				if(!reader.IsDBNull(24)) setupAlarmObject.IgnoreLowCoverageError = reader.GetBoolean( start + 24 );			
				if(!reader.IsDBNull(25)) setupAlarmObject.CustomerStatus = reader.GetString( start + 25 );			
				setupAlarmObject.CreatedBy = reader.GetGuid( start + 26 );			
				setupAlarmObject.CreatedDate = reader.GetDateTime( start + 27 );			
				setupAlarmObject.LastUpdatedBy = reader.GetGuid( start + 28 );			
				setupAlarmObject.LastUpdatedDate = reader.GetDateTime( start + 29 );			
			FillBaseObject(setupAlarmObject, reader, (start + 30));

			
			setupAlarmObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SetupAlarm object
        /// </summary>
        /// <param name="setupAlarmObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SetupAlarmBase setupAlarmObject, SqlDataReader reader)
		{
			FillObject(setupAlarmObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SetupAlarm object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SetupAlarm object</returns>
		private SetupAlarm GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SetupAlarm setupAlarmObject= new SetupAlarm();
					FillObject(setupAlarmObject, reader);
					return setupAlarmObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SetupAlarm objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SetupAlarm objects</returns>
		private SetupAlarmList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SetupAlarm list
			SetupAlarmList list = new SetupAlarmList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SetupAlarm setupAlarmObject = new SetupAlarm();
					FillObject(setupAlarmObject, reader);

					list.Add(setupAlarmObject);
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
