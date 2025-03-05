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
	public partial class PackageIncludeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGEINCLUDE = "InsertPackageInclude";
		private const string UPDATEPACKAGEINCLUDE = "UpdatePackageInclude";
		private const string DELETEPACKAGEINCLUDE = "DeletePackageInclude";
		private const string GETPACKAGEINCLUDEBYID = "GetPackageIncludeById";
		private const string GETALLPACKAGEINCLUDE = "GetAllPackageInclude";
		private const string GETPAGEDPACKAGEINCLUDE = "GetPagedPackageInclude";
		private const string GETPACKAGEINCLUDEMAXIMUMID = "GetPackageIncludeMaximumId";
		private const string GETPACKAGEINCLUDEROWCOUNT = "GetPackageIncludeRowCount";	
		private const string GETPACKAGEINCLUDEBYQUERY = "GetPackageIncludeByQuery";
		#endregion
		
		#region Constructors
		public PackageIncludeDataAccess(ClientContext context) : base(context) { }
		public PackageIncludeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageIncludeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageIncludeBase packageIncludeObject)
		{	
			AddParameter(cmd, pGuid(PackageIncludeBase.Property_CompanyId, packageIncludeObject.CompanyId));
			AddParameter(cmd, pInt32(PackageIncludeBase.Property_PackageId, packageIncludeObject.PackageId));
			AddParameter(cmd, pGuid(PackageIncludeBase.Property_EquipmentId, packageIncludeObject.EquipmentId));
			AddParameter(cmd, pBool(PackageIncludeBase.Property_IsFree, packageIncludeObject.IsFree));
			AddParameter(cmd, pInt32(PackageIncludeBase.Property_EptNo, packageIncludeObject.EptNo));
			AddParameter(cmd, pInt32(PackageIncludeBase.Property_OrderBy, packageIncludeObject.OrderBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageInclude
        /// </summary>
        /// <param name="packageIncludeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageIncludeBase packageIncludeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGEINCLUDE);
	
				AddParameter(cmd, pInt32Out(PackageIncludeBase.Property_Id));
				AddCommonParams(cmd, packageIncludeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageIncludeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageIncludeObject.Id = (Int32)GetOutParameter(cmd, PackageIncludeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageIncludeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageInclude
        /// </summary>
        /// <param name="packageIncludeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageIncludeBase packageIncludeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGEINCLUDE);
				
				AddParameter(cmd, pInt32(PackageIncludeBase.Property_Id, packageIncludeObject.Id));
				AddCommonParams(cmd, packageIncludeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageIncludeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageIncludeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageInclude
        /// </summary>
        /// <param name="Id">Id of the PackageInclude object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGEINCLUDE);	
				
				AddParameter(cmd, pInt32(PackageIncludeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageInclude), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageInclude object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageInclude object to retrieve</param>
        /// <returns>PackageInclude object, null if not found</returns>
		public PackageInclude Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEINCLUDEBYID))
			{
				AddParameter( cmd, pInt32(PackageIncludeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageInclude objects 
        /// </summary>
        /// <returns>A list of PackageInclude objects</returns>
		public PackageIncludeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGEINCLUDE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageInclude objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageInclude objects</returns>
		public PackageIncludeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGEINCLUDE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageIncludeList _PackageIncludeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageIncludeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageInclude objects by query String
        /// </summary>
        /// <returns>A list of PackageInclude objects</returns>
		public PackageIncludeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEINCLUDEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageInclude Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageInclude
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEINCLUDEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageInclude Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageInclude
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageIncludeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEINCLUDEROWCOUNT))
			{
				SqlDataReader reader;
				_PackageIncludeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageIncludeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageInclude object
        /// </summary>
        /// <param name="packageIncludeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageIncludeBase packageIncludeObject, SqlDataReader reader, int start)
		{
			
				packageIncludeObject.Id = reader.GetInt32( start + 0 );			
				packageIncludeObject.CompanyId = reader.GetGuid( start + 1 );			
				packageIncludeObject.PackageId = reader.GetInt32( start + 2 );			
				packageIncludeObject.EquipmentId = reader.GetGuid( start + 3 );			
				packageIncludeObject.IsFree = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) packageIncludeObject.EptNo = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) packageIncludeObject.OrderBy = reader.GetInt32( start + 6 );			
			FillBaseObject(packageIncludeObject, reader, (start + 7));

			
			packageIncludeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageInclude object
        /// </summary>
        /// <param name="packageIncludeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageIncludeBase packageIncludeObject, SqlDataReader reader)
		{
			FillObject(packageIncludeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageInclude object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageInclude object</returns>
		private PackageInclude GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageInclude packageIncludeObject= new PackageInclude();
					FillObject(packageIncludeObject, reader);
					return packageIncludeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageInclude objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageInclude objects</returns>
		private PackageIncludeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageInclude list
			PackageIncludeList list = new PackageIncludeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageInclude packageIncludeObject = new PackageInclude();
					FillObject(packageIncludeObject, reader);

					list.Add(packageIncludeObject);
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
