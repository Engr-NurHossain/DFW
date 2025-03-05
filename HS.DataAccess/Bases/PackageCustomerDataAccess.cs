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
	public partial class PackageCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPACKAGECUSTOMER = "InsertPackageCustomer";
		private const string UPDATEPACKAGECUSTOMER = "UpdatePackageCustomer";
		private const string DELETEPACKAGECUSTOMER = "DeletePackageCustomer";
		private const string GETPACKAGECUSTOMERBYID = "GetPackageCustomerById";
		private const string GETALLPACKAGECUSTOMER = "GetAllPackageCustomer";
		private const string GETPAGEDPACKAGECUSTOMER = "GetPagedPackageCustomer";
		private const string GETPACKAGECUSTOMERMAXIMUMID = "GetPackageCustomerMaximumId";
		private const string GETPACKAGECUSTOMERROWCOUNT = "GetPackageCustomerRowCount";	
		private const string GETPACKAGECUSTOMERBYQUERY = "GetPackageCustomerByQuery";
		#endregion
		
		#region Constructors
		public PackageCustomerDataAccess(ClientContext context) : base(context) { }
		public PackageCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="packageCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PackageCustomerBase packageCustomerObject)
		{	
			AddParameter(cmd, pGuid(PackageCustomerBase.Property_CompanyId, packageCustomerObject.CompanyId));
			AddParameter(cmd, pGuid(PackageCustomerBase.Property_CustomerId, packageCustomerObject.CustomerId));
			AddParameter(cmd, pGuid(PackageCustomerBase.Property_PackageId, packageCustomerObject.PackageId));
			AddParameter(cmd, pInt32(PackageCustomerBase.Property_SmartSystemTypeId, packageCustomerObject.SmartSystemTypeId));
			AddParameter(cmd, pInt32(PackageCustomerBase.Property_SmartInstallTypeId, packageCustomerObject.SmartInstallTypeId));
			AddParameter(cmd, pGuid(PackageCustomerBase.Property_ManufacturerId, packageCustomerObject.ManufacturerId));
			AddParameter(cmd, pDouble(PackageCustomerBase.Property_NonConformingFee, packageCustomerObject.NonConformingFee));
			AddParameter(cmd, pDouble(PackageCustomerBase.Property_ActivationFee, packageCustomerObject.ActivationFee));
			AddParameter(cmd, pBool(PackageCustomerBase.Property_WarrentyAvailable, packageCustomerObject.WarrentyAvailable));
			AddParameter(cmd, pNVarChar(PackageCustomerBase.Property_SmartSystemType, 250, packageCustomerObject.SmartSystemType));
			AddParameter(cmd, pDouble(PackageCustomerBase.Property_LabourFee, packageCustomerObject.LabourFee));
			AddParameter(cmd, pBool(PackageCustomerBase.Property_IsNFTTicket, packageCustomerObject.IsNFTTicket));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PackageCustomer
        /// </summary>
        /// <param name="packageCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PackageCustomerBase packageCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPACKAGECUSTOMER);
	
				AddParameter(cmd, pInt32Out(PackageCustomerBase.Property_Id));
				AddCommonParams(cmd, packageCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					packageCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					packageCustomerObject.Id = (Int32)GetOutParameter(cmd, PackageCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(packageCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PackageCustomer
        /// </summary>
        /// <param name="packageCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PackageCustomerBase packageCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPACKAGECUSTOMER);
				
				AddParameter(cmd, pInt32(PackageCustomerBase.Property_Id, packageCustomerObject.Id));
				AddCommonParams(cmd, packageCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					packageCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(packageCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PackageCustomer
        /// </summary>
        /// <param name="Id">Id of the PackageCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPACKAGECUSTOMER);	
				
				AddParameter(cmd, pInt32(PackageCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PackageCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PackageCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PackageCustomer object to retrieve</param>
        /// <returns>PackageCustomer object, null if not found</returns>
		public PackageCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(PackageCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PackageCustomer objects 
        /// </summary>
        /// <returns>A list of PackageCustomer objects</returns>
		public PackageCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPACKAGECUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PackageCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of PackageCustomer objects</returns>
		public PackageCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPACKAGECUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PackageCustomerList _PackageCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PackageCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all PackageCustomer objects by query String
        /// </summary>
        /// <returns>A list of PackageCustomer objects</returns>
		public PackageCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PackageCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PackageCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PackageCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PackageCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PackageCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPACKAGECUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_PackageCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PackageCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PackageCustomer object
        /// </summary>
        /// <param name="packageCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PackageCustomerBase packageCustomerObject, SqlDataReader reader, int start)
		{
			
				packageCustomerObject.Id = reader.GetInt32( start + 0 );			
				packageCustomerObject.CompanyId = reader.GetGuid( start + 1 );			
				packageCustomerObject.CustomerId = reader.GetGuid( start + 2 );			
				packageCustomerObject.PackageId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) packageCustomerObject.SmartSystemTypeId = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) packageCustomerObject.SmartInstallTypeId = reader.GetInt32( start + 5 );			
				packageCustomerObject.ManufacturerId = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) packageCustomerObject.NonConformingFee = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) packageCustomerObject.ActivationFee = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) packageCustomerObject.WarrentyAvailable = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) packageCustomerObject.SmartSystemType = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) packageCustomerObject.LabourFee = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) packageCustomerObject.IsNFTTicket = reader.GetBoolean( start + 12 );			
			FillBaseObject(packageCustomerObject, reader, (start + 13));

			
			packageCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PackageCustomer object
        /// </summary>
        /// <param name="packageCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PackageCustomerBase packageCustomerObject, SqlDataReader reader)
		{
			FillObject(packageCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PackageCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PackageCustomer object</returns>
		private PackageCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PackageCustomer packageCustomerObject= new PackageCustomer();
					FillObject(packageCustomerObject, reader);
					return packageCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PackageCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PackageCustomer objects</returns>
		private PackageCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PackageCustomer list
			PackageCustomerList list = new PackageCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PackageCustomer packageCustomerObject = new PackageCustomer();
					FillObject(packageCustomerObject, reader);

					list.Add(packageCustomerObject);
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
