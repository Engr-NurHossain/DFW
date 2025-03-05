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
	public partial class PackageCommissionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGECOMMISSION = "InsertPackageCommission";
		private const string UPDATEPACKAGECOMMISSION = "UpdatePackageCommission";
		private const string DELETEPACKAGECOMMISSION = "DeletePackageCommission";
		private const string GETPACKAGECOMMISSIONBYID = "GetPackageCommissionById";
		private const string GETALLPACKAGECOMMISSION = "GetAllPackageCommission";
		private const string GETPAGEDPACKAGECOMMISSION = "GetPagedPackageCommission";
		private const string GETPACKAGECOMMISSIONMAXIMUMID = "GetPackageCommissionMaximumId";
		private const string GETPACKAGECOMMISSIONROWCOUNT = "GetPackageCommissionRowCount";	
		private const string GETPACKAGECOMMISSIONBYQUERY = "GetPackageCommissionByQuery";
		#endregion
		
		#region Constructors
		public PackageCommissionDataAccess(ClientContext context) : base(context) { }
		public PackageCommissionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageCommissionObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageCommissionBase packageCommissionObject)
		{	
			AddParameter(cmd, pGuid(PackageCommissionBase.Property_PackageCommissionId, packageCommissionObject.PackageCommissionId));
			AddParameter(cmd, pNVarChar(PackageCommissionBase.Property_Type, 50, packageCommissionObject.Type));
			AddParameter(cmd, pNVarChar(PackageCommissionBase.Property_LeadType, 50, packageCommissionObject.LeadType));
			AddParameter(cmd, pNVarChar(PackageCommissionBase.Property_PackageType, 50, packageCommissionObject.PackageType));
			AddParameter(cmd, pNVarChar(PackageCommissionBase.Property_CommissionType, 50, packageCommissionObject.CommissionType));
			AddParameter(cmd, pDouble(PackageCommissionBase.Property_Commission, packageCommissionObject.Commission));
			AddParameter(cmd, pGuid(PackageCommissionBase.Property_CreatedBy, packageCommissionObject.CreatedBy));
			AddParameter(cmd, pDateTime(PackageCommissionBase.Property_CreatedDate, packageCommissionObject.CreatedDate));
			AddParameter(cmd, pGuid(PackageCommissionBase.Property_LastUpdatedBy, packageCommissionObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(PackageCommissionBase.Property_LastUpdatedDate, packageCommissionObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageCommission
        /// </summary>
        /// <param name="packageCommissionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageCommissionBase packageCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGECOMMISSION);
	
				AddParameter(cmd, pInt32Out(PackageCommissionBase.Property_Id));
				AddCommonParams(cmd, packageCommissionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageCommissionObject.Id = (Int32)GetOutParameter(cmd, PackageCommissionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageCommissionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageCommission
        /// </summary>
        /// <param name="packageCommissionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageCommissionBase packageCommissionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGECOMMISSION);
				
				AddParameter(cmd, pInt32(PackageCommissionBase.Property_Id, packageCommissionObject.Id));
				AddCommonParams(cmd, packageCommissionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageCommissionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageCommission
        /// </summary>
        /// <param name="Id">Id of the PackageCommission object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGECOMMISSION);	
				
				AddParameter(cmd, pInt32(PackageCommissionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageCommission), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageCommission object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageCommission object to retrieve</param>
        /// <returns>PackageCommission object, null if not found</returns>
		public PackageCommission Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECOMMISSIONBYID))
			{
				AddParameter( cmd, pInt32(PackageCommissionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageCommission objects 
        /// </summary>
        /// <returns>A list of PackageCommission objects</returns>
		public PackageCommissionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGECOMMISSION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageCommission objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageCommission objects</returns>
		public PackageCommissionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGECOMMISSION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageCommissionList _PackageCommissionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageCommissionList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageCommission objects by query String
        /// </summary>
        /// <returns>A list of PackageCommission objects</returns>
		public PackageCommissionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECOMMISSIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageCommission Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECOMMISSIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageCommission Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageCommission
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageCommissionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECOMMISSIONROWCOUNT))
			{
				SqlDataReader reader;
				_PackageCommissionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageCommissionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageCommission object
        /// </summary>
        /// <param name="packageCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageCommissionBase packageCommissionObject, SqlDataReader reader, int start)
		{
			
				packageCommissionObject.Id = reader.GetInt32( start + 0 );			
				packageCommissionObject.PackageCommissionId = reader.GetGuid( start + 1 );			
				packageCommissionObject.Type = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) packageCommissionObject.LeadType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) packageCommissionObject.PackageType = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) packageCommissionObject.CommissionType = reader.GetString( start + 5 );			
				packageCommissionObject.Commission = reader.GetDouble( start + 6 );			
				packageCommissionObject.CreatedBy = reader.GetGuid( start + 7 );			
				packageCommissionObject.CreatedDate = reader.GetDateTime( start + 8 );			
				packageCommissionObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				packageCommissionObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(packageCommissionObject, reader, (start + 11));

			
			packageCommissionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageCommission object
        /// </summary>
        /// <param name="packageCommissionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageCommissionBase packageCommissionObject, SqlDataReader reader)
		{
			FillObject(packageCommissionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageCommission object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageCommission object</returns>
		private PackageCommission GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageCommission packageCommissionObject= new PackageCommission();
					FillObject(packageCommissionObject, reader);
					return packageCommissionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageCommission objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageCommission objects</returns>
		private PackageCommissionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageCommission list
			PackageCommissionList list = new PackageCommissionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageCommission packageCommissionObject = new PackageCommission();
					FillObject(packageCommissionObject, reader);

					list.Add(packageCommissionObject);
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
