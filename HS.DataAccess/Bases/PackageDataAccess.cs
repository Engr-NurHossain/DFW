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
	public partial class PackageDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGE = "InsertPackage";
		private const string UPDATEPACKAGE = "UpdatePackage";
		private const string DELETEPACKAGE = "DeletePackage";
		private const string GETPACKAGEBYID = "GetPackageById";
		private const string GETALLPACKAGE = "GetAllPackage";
		private const string GETPAGEDPACKAGE = "GetPagedPackage";
		private const string GETPACKAGEMAXIMUMID = "GetPackageMaximumId";
		private const string GETPACKAGEROWCOUNT = "GetPackageRowCount";	
		private const string GETPACKAGEBYQUERY = "GetPackageByQuery";
		#endregion
		
		#region Constructors
		public PackageDataAccess(ClientContext context) : base(context) { }
		public PackageDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageBase packageObject)
		{	
			AddParameter(cmd, pGuid(PackageBase.Property_PackageId, packageObject.PackageId));
			AddParameter(cmd, pNVarChar(PackageBase.Property_Name, 250, packageObject.Name));
			AddParameter(cmd, pGuid(PackageBase.Property_CompanyId, packageObject.CompanyId));
			AddParameter(cmd, pInt32(PackageBase.Property_OptionEqpMaxLimit, packageObject.OptionEqpMaxLimit));
			AddParameter(cmd, pDouble(PackageBase.Property_Rate, packageObject.Rate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Package
        /// </summary>
        /// <param name="packageObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageBase packageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGE);
	
				AddParameter(cmd, pInt32Out(PackageBase.Property_Id));
				AddCommonParams(cmd, packageObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageObject.Id = (Int32)GetOutParameter(cmd, PackageBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Package
        /// </summary>
        /// <param name="packageObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageBase packageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGE);
				
				AddParameter(cmd, pInt32(PackageBase.Property_Id, packageObject.Id));
				AddCommonParams(cmd, packageObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Package
        /// </summary>
        /// <param name="Id">Id of the Package object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGE);	
				
				AddParameter(cmd, pInt32(PackageBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Package), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Package object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Package object to retrieve</param>
        /// <returns>Package object, null if not found</returns>
		public Package Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEBYID))
			{
				AddParameter( cmd, pInt32(PackageBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Package objects 
        /// </summary>
        /// <returns>A list of Package objects</returns>
		public PackageList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Package objects by PageRequest
        /// </summary>
        /// <returns>A list of Package objects</returns>
		public PackageList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageList _PackageList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageList;
			}
		}
		
		/// <summary>
        /// Retrieves all Package objects by query String
        /// </summary>
        /// <returns>A list of Package objects</returns>
		public PackageList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Package Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Package
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Package Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Package
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEROWCOUNT))
			{
				SqlDataReader reader;
				_PackageRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Package object
        /// </summary>
        /// <param name="packageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageBase packageObject, SqlDataReader reader, int start)
		{
			
				packageObject.Id = reader.GetInt32( start + 0 );			
				packageObject.PackageId = reader.GetGuid( start + 1 );			
				packageObject.Name = reader.GetString( start + 2 );			
				packageObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) packageObject.OptionEqpMaxLimit = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) packageObject.Rate = reader.GetDouble( start + 5 );			
			FillBaseObject(packageObject, reader, (start + 6));

			
			packageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Package object
        /// </summary>
        /// <param name="packageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageBase packageObject, SqlDataReader reader)
		{
			FillObject(packageObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Package object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Package object</returns>
		private Package GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Package packageObject= new Package();
					FillObject(packageObject, reader);
					return packageObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Package objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Package objects</returns>
		private PackageList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Package list
			PackageList list = new PackageList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Package packageObject = new Package();
					FillObject(packageObject, reader);

					list.Add(packageObject);
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
