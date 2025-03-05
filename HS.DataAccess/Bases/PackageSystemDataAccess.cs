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
	public partial class PackageSystemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGESYSTEM = "InsertPackageSystem";
		private const string UPDATEPACKAGESYSTEM = "UpdatePackageSystem";
		private const string DELETEPACKAGESYSTEM = "DeletePackageSystem";
		private const string GETPACKAGESYSTEMBYID = "GetPackageSystemById";
		private const string GETALLPACKAGESYSTEM = "GetAllPackageSystem";
		private const string GETPAGEDPACKAGESYSTEM = "GetPagedPackageSystem";
		private const string GETPACKAGESYSTEMMAXIMUMID = "GetPackageSystemMaximumId";
		private const string GETPACKAGESYSTEMROWCOUNT = "GetPackageSystemRowCount";	
		private const string GETPACKAGESYSTEMBYQUERY = "GetPackageSystemByQuery";
		#endregion
		
		#region Constructors
		public PackageSystemDataAccess(ClientContext context) : base(context) { }
		public PackageSystemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageSystemObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageSystemBase packageSystemObject)
		{	
			AddParameter(cmd, pGuid(PackageSystemBase.Property_CompanyId, packageSystemObject.CompanyId));
			AddParameter(cmd, pNVarChar(PackageSystemBase.Property_Name, 500, packageSystemObject.Name));
			AddParameter(cmd, pNVarChar(PackageSystemBase.Property_Value, 500, packageSystemObject.Value));
			AddParameter(cmd, pBool(PackageSystemBase.Property_IsActive, packageSystemObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageSystem
        /// </summary>
        /// <param name="packageSystemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageSystemBase packageSystemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGESYSTEM);
	
				AddParameter(cmd, pInt32Out(PackageSystemBase.Property_Id));
				AddCommonParams(cmd, packageSystemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageSystemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageSystemObject.Id = (Int32)GetOutParameter(cmd, PackageSystemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageSystemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageSystem
        /// </summary>
        /// <param name="packageSystemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageSystemBase packageSystemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGESYSTEM);
				
				AddParameter(cmd, pInt32(PackageSystemBase.Property_Id, packageSystemObject.Id));
				AddCommonParams(cmd, packageSystemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageSystemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageSystemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageSystem
        /// </summary>
        /// <param name="Id">Id of the PackageSystem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGESYSTEM);	
				
				AddParameter(cmd, pInt32(PackageSystemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageSystem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageSystem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageSystem object to retrieve</param>
        /// <returns>PackageSystem object, null if not found</returns>
		public PackageSystem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMBYID))
			{
				AddParameter( cmd, pInt32(PackageSystemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageSystem objects 
        /// </summary>
        /// <returns>A list of PackageSystem objects</returns>
		public PackageSystemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGESYSTEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageSystem objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageSystem objects</returns>
		public PackageSystemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGESYSTEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageSystemList _PackageSystemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageSystemList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageSystem objects by query String
        /// </summary>
        /// <returns>A list of PackageSystem objects</returns>
		public PackageSystemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageSystem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageSystem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageSystem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageSystem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageSystemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMROWCOUNT))
			{
				SqlDataReader reader;
				_PackageSystemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageSystemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageSystem object
        /// </summary>
        /// <param name="packageSystemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageSystemBase packageSystemObject, SqlDataReader reader, int start)
		{
			
				packageSystemObject.Id = reader.GetInt32( start + 0 );			
				packageSystemObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) packageSystemObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) packageSystemObject.Value = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) packageSystemObject.IsActive = reader.GetBoolean( start + 4 );			
			FillBaseObject(packageSystemObject, reader, (start + 5));

			
			packageSystemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageSystem object
        /// </summary>
        /// <param name="packageSystemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageSystemBase packageSystemObject, SqlDataReader reader)
		{
			FillObject(packageSystemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageSystem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageSystem object</returns>
		private PackageSystem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageSystem packageSystemObject= new PackageSystem();
					FillObject(packageSystemObject, reader);
					return packageSystemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageSystem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageSystem objects</returns>
		private PackageSystemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageSystem list
			PackageSystemList list = new PackageSystemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageSystem packageSystemObject = new PackageSystem();
					FillObject(packageSystemObject, reader);

					list.Add(packageSystemObject);
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
