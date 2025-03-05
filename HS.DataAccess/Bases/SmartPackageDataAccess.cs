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
	public partial class SmartPackageDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSMARTPACKAGE = "InsertSmartPackage";
		private const string UPDATESMARTPACKAGE = "UpdateSmartPackage";
		private const string DELETESMARTPACKAGE = "DeleteSmartPackage";
		private const string GETSMARTPACKAGEBYID = "GetSmartPackageById";
		private const string GETALLSMARTPACKAGE = "GetAllSmartPackage";
		private const string GETPAGEDSMARTPACKAGE = "GetPagedSmartPackage";
		private const string GETSMARTPACKAGEMAXIMUMID = "GetSmartPackageMaximumId";
		private const string GETSMARTPACKAGEROWCOUNT = "GetSmartPackageRowCount";	
		private const string GETSMARTPACKAGEBYQUERY = "GetSmartPackageByQuery";
		#endregion
		
		#region Constructors
		public SmartPackageDataAccess(ClientContext context) : base(context) { }        
        public SmartPackageDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="smartPackageObject"></param>
		private void AddCommonParams(SqlCommand cmd, SmartPackageBase smartPackageObject)
		{	
			AddParameter(cmd, pGuid(SmartPackageBase.Property_PackageId, smartPackageObject.PackageId));
			AddParameter(cmd, pGuid(SmartPackageBase.Property_CompanyId, smartPackageObject.CompanyId));
			AddParameter(cmd, pInt32(SmartPackageBase.Property_SmartSystemTypeId, smartPackageObject.SmartSystemTypeId));
			AddParameter(cmd, pInt32(SmartPackageBase.Property_SmartInstallTypeId, smartPackageObject.SmartInstallTypeId));
			AddParameter(cmd, pNVarChar(SmartPackageBase.Property_PackageName, 150, smartPackageObject.PackageName));
			AddParameter(cmd, pInt32(SmartPackageBase.Property_EquipmentMaxLimit, smartPackageObject.EquipmentMaxLimit));
			AddParameter(cmd, pDouble(SmartPackageBase.Property_ActivationFee, smartPackageObject.ActivationFee));
			AddParameter(cmd, pBool(SmartPackageBase.Property_IsActive, smartPackageObject.IsActive));
			AddParameter(cmd, pBool(SmartPackageBase.Property_IsPromo, smartPackageObject.IsPromo));
			AddParameter(cmd, pDateTime(SmartPackageBase.Property_StartDate, smartPackageObject.StartDate));
			AddParameter(cmd, pDateTime(SmartPackageBase.Property_EndDate, smartPackageObject.EndDate));
			AddParameter(cmd, pDouble(SmartPackageBase.Property_TotalRMR, smartPackageObject.TotalRMR));
			AddParameter(cmd, pGuid(SmartPackageBase.Property_LastUpdatedBy, smartPackageObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(SmartPackageBase.Property_LastUpdatedDate, smartPackageObject.LastUpdatedDate));
			AddParameter(cmd, pBool(SmartPackageBase.Property_NonConforming, smartPackageObject.NonConforming));
			AddParameter(cmd, pDouble(SmartPackageBase.Property_MinCredit, smartPackageObject.MinCredit));
			AddParameter(cmd, pDouble(SmartPackageBase.Property_MaxCredit, smartPackageObject.MaxCredit));
			AddParameter(cmd, pGuid(SmartPackageBase.Property_ManufacturerId, smartPackageObject.ManufacturerId));
			AddParameter(cmd, pNVarChar(SmartPackageBase.Property_PackageCode, 50, smartPackageObject.PackageCode));
			AddParameter(cmd, pNVarChar(SmartPackageBase.Property_UserType, 50, smartPackageObject.UserType));
			AddParameter(cmd, pDouble(SmartPackageBase.Property_ConformingFee, smartPackageObject.ConformingFee));
			AddParameter(cmd, pNVarChar(SmartPackageBase.Property_PackageType, 50, smartPackageObject.PackageType));
			AddParameter(cmd, pBool(SmartPackageBase.Property_IsDelete, smartPackageObject.IsDelete));
			AddParameter(cmd, pNVarChar(SmartPackageBase.Property_CustomerNumber, 50, smartPackageObject.CustomerNumber));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SmartPackage
        /// </summary>
        /// <param name="smartPackageObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SmartPackageBase smartPackageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSMARTPACKAGE);
	
				AddParameter(cmd, pInt32Out(SmartPackageBase.Property_Id));
				AddCommonParams(cmd, smartPackageObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					smartPackageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					smartPackageObject.Id = (Int32)GetOutParameter(cmd, SmartPackageBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(smartPackageObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SmartPackage
        /// </summary>
        /// <param name="smartPackageObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SmartPackageBase smartPackageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESMARTPACKAGE);
				
				AddParameter(cmd, pInt32(SmartPackageBase.Property_Id, smartPackageObject.Id));
				AddCommonParams(cmd, smartPackageObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					smartPackageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(smartPackageObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SmartPackage
        /// </summary>
        /// <param name="Id">Id of the SmartPackage object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESMARTPACKAGE);	
				
				AddParameter(cmd, pInt32(SmartPackageBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SmartPackage), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SmartPackage object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SmartPackage object to retrieve</param>
        /// <returns>SmartPackage object, null if not found</returns>
		public SmartPackage Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEBYID))
			{
				AddParameter( cmd, pInt32(SmartPackageBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SmartPackage objects 
        /// </summary>
        /// <returns>A list of SmartPackage objects</returns>
		public SmartPackageList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSMARTPACKAGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SmartPackage objects by PageRequest
        /// </summary>
        /// <returns>A list of SmartPackage objects</returns>
		public SmartPackageList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSMARTPACKAGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SmartPackageList _SmartPackageList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SmartPackageList;
			}
		}
		
		/// <summary>
        /// Retrieves all SmartPackage objects by query String
        /// </summary>
        /// <returns>A list of SmartPackage objects</returns>
		public SmartPackageList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SmartPackage Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SmartPackage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SmartPackage Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SmartPackage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SmartPackageRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSMARTPACKAGEROWCOUNT))
			{
				SqlDataReader reader;
				_SmartPackageRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SmartPackageRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SmartPackage object
        /// </summary>
        /// <param name="smartPackageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SmartPackageBase smartPackageObject, SqlDataReader reader, int start)
		{
			
				smartPackageObject.Id = reader.GetInt32( start + 0 );			
				smartPackageObject.PackageId = reader.GetGuid( start + 1 );			
				smartPackageObject.CompanyId = reader.GetGuid( start + 2 );			
				smartPackageObject.SmartSystemTypeId = reader.GetInt32( start + 3 );			
				smartPackageObject.SmartInstallTypeId = reader.GetInt32( start + 4 );			
				smartPackageObject.PackageName = reader.GetString( start + 5 );			
				smartPackageObject.EquipmentMaxLimit = reader.GetInt32( start + 6 );			
				if(!reader.IsDBNull(7)) smartPackageObject.ActivationFee = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) smartPackageObject.IsActive = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) smartPackageObject.IsPromo = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) smartPackageObject.StartDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) smartPackageObject.EndDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) smartPackageObject.TotalRMR = reader.GetDouble( start + 12 );			
				smartPackageObject.LastUpdatedBy = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) smartPackageObject.LastUpdatedDate = reader.GetDateTime( start + 14 );			
				if(!reader.IsDBNull(15)) smartPackageObject.NonConforming = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) smartPackageObject.MinCredit = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) smartPackageObject.MaxCredit = reader.GetDouble( start + 17 );			
				smartPackageObject.ManufacturerId = reader.GetGuid( start + 18 );			
				if(!reader.IsDBNull(19)) smartPackageObject.PackageCode = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) smartPackageObject.UserType = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) smartPackageObject.ConformingFee = reader.GetDouble( start + 21 );			
				if(!reader.IsDBNull(22)) smartPackageObject.PackageType = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) smartPackageObject.IsDelete = reader.GetBoolean( start + 23 );			
				if(!reader.IsDBNull(24)) smartPackageObject.CustomerNumber = reader.GetString( start + 24 );			
			FillBaseObject(smartPackageObject, reader, (start + 25));

			
			smartPackageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SmartPackage object
        /// </summary>
        /// <param name="smartPackageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SmartPackageBase smartPackageObject, SqlDataReader reader)
		{
			FillObject(smartPackageObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SmartPackage object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SmartPackage object</returns>
		private SmartPackage GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SmartPackage smartPackageObject= new SmartPackage();
					FillObject(smartPackageObject, reader);
					return smartPackageObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SmartPackage objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SmartPackage objects</returns>
		private SmartPackageList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SmartPackage list
			SmartPackageList list = new SmartPackageList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SmartPackage smartPackageObject = new SmartPackage();
					FillObject(smartPackageObject, reader);

					list.Add(smartPackageObject);
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
