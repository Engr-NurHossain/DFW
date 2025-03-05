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
	public partial class PackageSystemCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGESYSTEMCUSTOMER = "InsertPackageSystemCustomer";
		private const string UPDATEPACKAGESYSTEMCUSTOMER = "UpdatePackageSystemCustomer";
		private const string DELETEPACKAGESYSTEMCUSTOMER = "DeletePackageSystemCustomer";
		private const string GETPACKAGESYSTEMCUSTOMERBYID = "GetPackageSystemCustomerById";
		private const string GETALLPACKAGESYSTEMCUSTOMER = "GetAllPackageSystemCustomer";
		private const string GETPAGEDPACKAGESYSTEMCUSTOMER = "GetPagedPackageSystemCustomer";
		private const string GETPACKAGESYSTEMCUSTOMERMAXIMUMID = "GetPackageSystemCustomerMaximumId";
		private const string GETPACKAGESYSTEMCUSTOMERROWCOUNT = "GetPackageSystemCustomerRowCount";	
		private const string GETPACKAGESYSTEMCUSTOMERBYQUERY = "GetPackageSystemCustomerByQuery";
		#endregion
		
		#region Constructors
		public PackageSystemCustomerDataAccess(ClientContext context) : base(context) { }
		public PackageSystemCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageSystemCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageSystemCustomerBase packageSystemCustomerObject)
		{	
			AddParameter(cmd, pGuid(PackageSystemCustomerBase.Property_CompanyId, packageSystemCustomerObject.CompanyId));
			AddParameter(cmd, pGuid(PackageSystemCustomerBase.Property_CustomerId, packageSystemCustomerObject.CustomerId));
			AddParameter(cmd, pInt32(PackageSystemCustomerBase.Property_PackageSystemId, packageSystemCustomerObject.PackageSystemId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageSystemCustomer
        /// </summary>
        /// <param name="packageSystemCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageSystemCustomerBase packageSystemCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGESYSTEMCUSTOMER);
	
				AddParameter(cmd, pInt32Out(PackageSystemCustomerBase.Property_Id));
				AddCommonParams(cmd, packageSystemCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageSystemCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageSystemCustomerObject.Id = (Int32)GetOutParameter(cmd, PackageSystemCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageSystemCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageSystemCustomer
        /// </summary>
        /// <param name="packageSystemCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageSystemCustomerBase packageSystemCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGESYSTEMCUSTOMER);
				
				AddParameter(cmd, pInt32(PackageSystemCustomerBase.Property_Id, packageSystemCustomerObject.Id));
				AddCommonParams(cmd, packageSystemCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageSystemCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageSystemCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageSystemCustomer
        /// </summary>
        /// <param name="Id">Id of the PackageSystemCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGESYSTEMCUSTOMER);	
				
				AddParameter(cmd, pInt32(PackageSystemCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageSystemCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageSystemCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageSystemCustomer object to retrieve</param>
        /// <returns>PackageSystemCustomer object, null if not found</returns>
		public PackageSystemCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(PackageSystemCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageSystemCustomer objects 
        /// </summary>
        /// <returns>A list of PackageSystemCustomer objects</returns>
		public PackageSystemCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGESYSTEMCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageSystemCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageSystemCustomer objects</returns>
		public PackageSystemCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGESYSTEMCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageSystemCustomerList _PackageSystemCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageSystemCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageSystemCustomer objects by query String
        /// </summary>
        /// <returns>A list of PackageSystemCustomer objects</returns>
		public PackageSystemCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageSystemCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageSystemCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageSystemCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageSystemCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageSystemCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGESYSTEMCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_PackageSystemCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageSystemCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageSystemCustomer object
        /// </summary>
        /// <param name="packageSystemCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageSystemCustomerBase packageSystemCustomerObject, SqlDataReader reader, int start)
		{
			
				packageSystemCustomerObject.Id = reader.GetInt32( start + 0 );			
				packageSystemCustomerObject.CompanyId = reader.GetGuid( start + 1 );			
				packageSystemCustomerObject.CustomerId = reader.GetGuid( start + 2 );			
				packageSystemCustomerObject.PackageSystemId = reader.GetInt32( start + 3 );			
			FillBaseObject(packageSystemCustomerObject, reader, (start + 4));

			
			packageSystemCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageSystemCustomer object
        /// </summary>
        /// <param name="packageSystemCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageSystemCustomerBase packageSystemCustomerObject, SqlDataReader reader)
		{
			FillObject(packageSystemCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageSystemCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageSystemCustomer object</returns>
		private PackageSystemCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageSystemCustomer packageSystemCustomerObject= new PackageSystemCustomer();
					FillObject(packageSystemCustomerObject, reader);
					return packageSystemCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageSystemCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageSystemCustomer objects</returns>
		private PackageSystemCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageSystemCustomer list
			PackageSystemCustomerList list = new PackageSystemCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageSystemCustomer packageSystemCustomerObject = new PackageSystemCustomer();
					FillObject(packageSystemCustomerObject, reader);

					list.Add(packageSystemCustomerObject);
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
