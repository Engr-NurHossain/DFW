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
	public partial class PackageOptionalDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGEOPTIONAL = "InsertPackageOptional";
		private const string UPDATEPACKAGEOPTIONAL = "UpdatePackageOptional";
		private const string DELETEPACKAGEOPTIONAL = "DeletePackageOptional";
		private const string GETPACKAGEOPTIONALBYID = "GetPackageOptionalById";
		private const string GETALLPACKAGEOPTIONAL = "GetAllPackageOptional";
		private const string GETPAGEDPACKAGEOPTIONAL = "GetPagedPackageOptional";
		private const string GETPACKAGEOPTIONALMAXIMUMID = "GetPackageOptionalMaximumId";
		private const string GETPACKAGEOPTIONALROWCOUNT = "GetPackageOptionalRowCount";	
		private const string GETPACKAGEOPTIONALBYQUERY = "GetPackageOptionalByQuery";
		#endregion
		
		#region Constructors
		public PackageOptionalDataAccess(ClientContext context) : base(context) { }
		public PackageOptionalDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageOptionalObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageOptionalBase packageOptionalObject)
		{	
			AddParameter(cmd, pGuid(PackageOptionalBase.Property_CompanyId, packageOptionalObject.CompanyId));
			AddParameter(cmd, pInt32(PackageOptionalBase.Property_PackageId, packageOptionalObject.PackageId));
			AddParameter(cmd, pGuid(PackageOptionalBase.Property_EquipmentId, packageOptionalObject.EquipmentId));
			AddParameter(cmd, pBool(PackageOptionalBase.Property_IsFree, packageOptionalObject.IsFree));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageOptional
        /// </summary>
        /// <param name="packageOptionalObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageOptionalBase packageOptionalObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGEOPTIONAL);
	
				AddParameter(cmd, pInt32Out(PackageOptionalBase.Property_Id));
				AddCommonParams(cmd, packageOptionalObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageOptionalObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageOptionalObject.Id = (Int32)GetOutParameter(cmd, PackageOptionalBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageOptionalObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageOptional
        /// </summary>
        /// <param name="packageOptionalObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageOptionalBase packageOptionalObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGEOPTIONAL);
				
				AddParameter(cmd, pInt32(PackageOptionalBase.Property_Id, packageOptionalObject.Id));
				AddCommonParams(cmd, packageOptionalObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageOptionalObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageOptionalObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageOptional
        /// </summary>
        /// <param name="Id">Id of the PackageOptional object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGEOPTIONAL);	
				
				AddParameter(cmd, pInt32(PackageOptionalBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageOptional), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageOptional object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageOptional object to retrieve</param>
        /// <returns>PackageOptional object, null if not found</returns>
		public PackageOptional Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEOPTIONALBYID))
			{
				AddParameter( cmd, pInt32(PackageOptionalBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageOptional objects 
        /// </summary>
        /// <returns>A list of PackageOptional objects</returns>
		public PackageOptionalList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGEOPTIONAL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageOptional objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageOptional objects</returns>
		public PackageOptionalList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGEOPTIONAL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageOptionalList _PackageOptionalList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageOptionalList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageOptional objects by query String
        /// </summary>
        /// <returns>A list of PackageOptional objects</returns>
		public PackageOptionalList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEOPTIONALBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageOptional Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageOptional
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEOPTIONALMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageOptional Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageOptional
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageOptionalRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEOPTIONALROWCOUNT))
			{
				SqlDataReader reader;
				_PackageOptionalRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageOptionalRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageOptional object
        /// </summary>
        /// <param name="packageOptionalObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageOptionalBase packageOptionalObject, SqlDataReader reader, int start)
		{
			
				packageOptionalObject.Id = reader.GetInt32( start + 0 );			
				packageOptionalObject.CompanyId = reader.GetGuid( start + 1 );			
				packageOptionalObject.PackageId = reader.GetInt32( start + 2 );			
				packageOptionalObject.EquipmentId = reader.GetGuid( start + 3 );			
				packageOptionalObject.IsFree = reader.GetBoolean( start + 4 );			
			FillBaseObject(packageOptionalObject, reader, (start + 5));

			
			packageOptionalObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageOptional object
        /// </summary>
        /// <param name="packageOptionalObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageOptionalBase packageOptionalObject, SqlDataReader reader)
		{
			FillObject(packageOptionalObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageOptional object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageOptional object</returns>
		private PackageOptional GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageOptional packageOptionalObject= new PackageOptional();
					FillObject(packageOptionalObject, reader);
					return packageOptionalObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageOptional objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageOptional objects</returns>
		private PackageOptionalList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageOptional list
			PackageOptionalList list = new PackageOptionalList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageOptional packageOptionalObject = new PackageOptional();
					FillObject(packageOptionalObject, reader);

					list.Add(packageOptionalObject);
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
