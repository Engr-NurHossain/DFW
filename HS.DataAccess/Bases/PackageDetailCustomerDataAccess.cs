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
	public partial class PackageDetailCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGEDETAILCUSTOMER = "InsertPackageDetailCustomer";
		private const string UPDATEPACKAGEDETAILCUSTOMER = "UpdatePackageDetailCustomer";
		private const string DELETEPACKAGEDETAILCUSTOMER = "DeletePackageDetailCustomer";
		private const string GETPACKAGEDETAILCUSTOMERBYID = "GetPackageDetailCustomerById";
		private const string GETALLPACKAGEDETAILCUSTOMER = "GetAllPackageDetailCustomer";
		private const string GETPAGEDPACKAGEDETAILCUSTOMER = "GetPagedPackageDetailCustomer";
		private const string GETPACKAGEDETAILCUSTOMERMAXIMUMID = "GetPackageDetailCustomerMaximumId";
		private const string GETPACKAGEDETAILCUSTOMERROWCOUNT = "GetPackageDetailCustomerRowCount";	
		private const string GETPACKAGEDETAILCUSTOMERBYQUERY = "GetPackageDetailCustomerByQuery";
		#endregion
		
		#region Constructors
		public PackageDetailCustomerDataAccess(ClientContext context) : base(context) { }
		public PackageDetailCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageDetailCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageDetailCustomerBase packageDetailCustomerObject)
		{	
			AddParameter(cmd, pGuid(PackageDetailCustomerBase.Property_CompanyId, packageDetailCustomerObject.CompanyId));
			AddParameter(cmd, pGuid(PackageDetailCustomerBase.Property_CustomerId, packageDetailCustomerObject.CustomerId));
			AddParameter(cmd, pNVarChar(PackageDetailCustomerBase.Property_Type, 50, packageDetailCustomerObject.Type));
			AddParameter(cmd, pBool(PackageDetailCustomerBase.Property_IsIncluded, packageDetailCustomerObject.IsIncluded));
			AddParameter(cmd, pInt32(PackageDetailCustomerBase.Property_PackageEqpId, packageDetailCustomerObject.PackageEqpId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageDetailCustomer
        /// </summary>
        /// <param name="packageDetailCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageDetailCustomerBase packageDetailCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGEDETAILCUSTOMER);
	
				AddParameter(cmd, pInt32Out(PackageDetailCustomerBase.Property_Id));
				AddCommonParams(cmd, packageDetailCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageDetailCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageDetailCustomerObject.Id = (Int32)GetOutParameter(cmd, PackageDetailCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageDetailCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageDetailCustomer
        /// </summary>
        /// <param name="packageDetailCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageDetailCustomerBase packageDetailCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGEDETAILCUSTOMER);
				
				AddParameter(cmd, pInt32(PackageDetailCustomerBase.Property_Id, packageDetailCustomerObject.Id));
				AddCommonParams(cmd, packageDetailCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageDetailCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageDetailCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageDetailCustomer
        /// </summary>
        /// <param name="Id">Id of the PackageDetailCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGEDETAILCUSTOMER);	
				
				AddParameter(cmd, pInt32(PackageDetailCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageDetailCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageDetailCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageDetailCustomer object to retrieve</param>
        /// <returns>PackageDetailCustomer object, null if not found</returns>
		public PackageDetailCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDETAILCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(PackageDetailCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageDetailCustomer objects 
        /// </summary>
        /// <returns>A list of PackageDetailCustomer objects</returns>
		public PackageDetailCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGEDETAILCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageDetailCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageDetailCustomer objects</returns>
		public PackageDetailCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGEDETAILCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageDetailCustomerList _PackageDetailCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageDetailCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageDetailCustomer objects by query String
        /// </summary>
        /// <returns>A list of PackageDetailCustomer objects</returns>
		public PackageDetailCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDETAILCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageDetailCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageDetailCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDETAILCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageDetailCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageDetailCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageDetailCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGEDETAILCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_PackageDetailCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageDetailCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageDetailCustomer object
        /// </summary>
        /// <param name="packageDetailCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageDetailCustomerBase packageDetailCustomerObject, SqlDataReader reader, int start)
		{
			
				packageDetailCustomerObject.Id = reader.GetInt32( start + 0 );			
				packageDetailCustomerObject.CompanyId = reader.GetGuid( start + 1 );			
				packageDetailCustomerObject.CustomerId = reader.GetGuid( start + 2 );			
				packageDetailCustomerObject.Type = reader.GetString( start + 3 );			
				packageDetailCustomerObject.IsIncluded = reader.GetBoolean( start + 4 );			
				packageDetailCustomerObject.PackageEqpId = reader.GetInt32( start + 5 );			
			FillBaseObject(packageDetailCustomerObject, reader, (start + 6));

			
			packageDetailCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageDetailCustomer object
        /// </summary>
        /// <param name="packageDetailCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageDetailCustomerBase packageDetailCustomerObject, SqlDataReader reader)
		{
			FillObject(packageDetailCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageDetailCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageDetailCustomer object</returns>
		private PackageDetailCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageDetailCustomer packageDetailCustomerObject= new PackageDetailCustomer();
					FillObject(packageDetailCustomerObject, reader);
					return packageDetailCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageDetailCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageDetailCustomer objects</returns>
		private PackageDetailCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageDetailCustomer list
			PackageDetailCustomerList list = new PackageDetailCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageDetailCustomer packageDetailCustomerObject = new PackageDetailCustomer();
					FillObject(packageDetailCustomerObject, reader);

					list.Add(packageDetailCustomerObject);
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
