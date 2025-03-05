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
	public partial class PackageSystemInstallTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGESYSTEMINSTALLTYPE = "InsertPackageSystemInstallType";
		private const string UPDATEPACKAGESYSTEMINSTALLTYPE = "UpdatePackageSystemInstallType";
		private const string DELETEPACKAGESYSTEMINSTALLTYPE = "DeletePackageSystemInstallType";
		private const string GETPACKAGESYSTEMINSTALLTYPEBYID = "GetPackageSystemInstallTypeById";
		private const string GETALLPACKAGESYSTEMINSTALLTYPE = "GetAllPackageSystemInstallType";
		private const string GETPAGEDPACKAGESYSTEMINSTALLTYPE = "GetPagedPackageSystemInstallType";
		private const string GETPACKAGESYSTEMINSTALLTYPEMAXIMUMID = "GetPackageSystemInstallTypeMaximumId";
		private const string GETPACKAGESYSTEMINSTALLTYPEROWCOUNT = "GetPackageSystemInstallTypeRowCount";	
		private const string GETPACKAGESYSTEMINSTALLTYPEBYQUERY = "GetPackageSystemInstallTypeByQuery";
		#endregion
		
		#region Constructors
		public PackageSystemInstallTypeDataAccess(ClientContext context) : base(context) { }
		public PackageSystemInstallTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageSystemInstallTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageSystemInstallTypeBase packageSystemInstallTypeObject)
		{	
			AddParameter(cmd, pGuid(PackageSystemInstallTypeBase.Property_CompanyId, packageSystemInstallTypeObject.CompanyId));
			AddParameter(cmd, pInt32(PackageSystemInstallTypeBase.Property_SystemId, packageSystemInstallTypeObject.SystemId));
			AddParameter(cmd, pNVarChar(PackageSystemInstallTypeBase.Property_Installtypevalue, 50, packageSystemInstallTypeObject.Installtypevalue));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageSystemInstallType
        /// </summary>
        /// <param name="packageSystemInstallTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageSystemInstallTypeBase packageSystemInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGESYSTEMINSTALLTYPE);
	
				AddParameter(cmd, pInt32Out(PackageSystemInstallTypeBase.Property_Id));
				AddCommonParams(cmd, packageSystemInstallTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageSystemInstallTypeObject.Id = (Int32)GetOutParameter(cmd, PackageSystemInstallTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageSystemInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageSystemInstallType
        /// </summary>
        /// <param name="packageSystemInstallTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageSystemInstallTypeBase packageSystemInstallTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGESYSTEMINSTALLTYPE);
				
				AddParameter(cmd, pInt32(PackageSystemInstallTypeBase.Property_Id, packageSystemInstallTypeObject.Id));
				AddCommonParams(cmd, packageSystemInstallTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageSystemInstallTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageSystemInstallType
        /// </summary>
        /// <param name="Id">Id of the PackageSystemInstallType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGESYSTEMINSTALLTYPE);	
				
				AddParameter(cmd, pInt32(PackageSystemInstallTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageSystemInstallType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageSystemInstallType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageSystemInstallType object to retrieve</param>
        /// <returns>PackageSystemInstallType object, null if not found</returns>
		public PackageSystemInstallType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMINSTALLTYPEBYID))
			{
				AddParameter( cmd, pInt32(PackageSystemInstallTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageSystemInstallType objects 
        /// </summary>
        /// <returns>A list of PackageSystemInstallType objects</returns>
		public PackageSystemInstallTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGESYSTEMINSTALLTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageSystemInstallType objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageSystemInstallType objects</returns>
		public PackageSystemInstallTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGESYSTEMINSTALLTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageSystemInstallTypeList _PackageSystemInstallTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageSystemInstallTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageSystemInstallType objects by query String
        /// </summary>
        /// <returns>A list of PackageSystemInstallType objects</returns>
		public PackageSystemInstallTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMINSTALLTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageSystemInstallType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageSystemInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMINSTALLTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageSystemInstallType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageSystemInstallType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageSystemInstallTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMINSTALLTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_PackageSystemInstallTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageSystemInstallTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageSystemInstallType object
        /// </summary>
        /// <param name="packageSystemInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageSystemInstallTypeBase packageSystemInstallTypeObject, SqlDataReader reader, int start)
		{
			
				packageSystemInstallTypeObject.Id = reader.GetInt32( start + 0 );			
				packageSystemInstallTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				packageSystemInstallTypeObject.SystemId = reader.GetInt32( start + 2 );			
				packageSystemInstallTypeObject.Installtypevalue = reader.GetString( start + 3 );			
			FillBaseObject(packageSystemInstallTypeObject, reader, (start + 4));

			
			packageSystemInstallTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageSystemInstallType object
        /// </summary>
        /// <param name="packageSystemInstallTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageSystemInstallTypeBase packageSystemInstallTypeObject, SqlDataReader reader)
		{
			FillObject(packageSystemInstallTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageSystemInstallType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageSystemInstallType object</returns>
		private PackageSystemInstallType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageSystemInstallType packageSystemInstallTypeObject= new PackageSystemInstallType();
					FillObject(packageSystemInstallTypeObject, reader);
					return packageSystemInstallTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageSystemInstallType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageSystemInstallType objects</returns>
		private PackageSystemInstallTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageSystemInstallType list
			PackageSystemInstallTypeList list = new PackageSystemInstallTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageSystemInstallType packageSystemInstallTypeObject = new PackageSystemInstallType();
					FillObject(packageSystemInstallTypeObject, reader);

					list.Add(packageSystemInstallTypeObject);
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
