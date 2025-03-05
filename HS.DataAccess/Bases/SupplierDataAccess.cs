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
	public partial class SupplierDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSUPPLIER = "InsertSupplier";
		private const string UPDATESUPPLIER = "UpdateSupplier";
		private const string DELETESUPPLIER = "DeleteSupplier";
		private const string GETSUPPLIERBYID = "GetSupplierById";
		private const string GETALLSUPPLIER = "GetAllSupplier";
		private const string GETPAGEDSUPPLIER = "GetPagedSupplier";
		private const string GETSUPPLIERMAXIMUMID = "GetSupplierMaximumId";
		private const string GETSUPPLIERROWCOUNT = "GetSupplierRowCount";	
		private const string GETSUPPLIERBYQUERY = "GetSupplierByQuery";
		#endregion
		
		#region Constructors
		public SupplierDataAccess(ClientContext context) : base(context) { }
		public SupplierDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="supplierObject"></param>
		private void AddCommonParams(SqlCommand cmd, SupplierBase supplierObject)
		{	
			AddParameter(cmd, pGuid(SupplierBase.Property_SupplierId, supplierObject.SupplierId));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_CompanyName, 250, supplierObject.CompanyName));
			AddParameter(cmd, pGuid(SupplierBase.Property_CompanyId, supplierObject.CompanyId));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Name, 50, supplierObject.Name));
			AddParameter(cmd, pInt32(SupplierBase.Property_OrderBy, supplierObject.OrderBy));
			AddParameter(cmd, pGuid(SupplierBase.Property_ContactPerson, supplierObject.ContactPerson));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Phone, 50, supplierObject.Phone));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_EmailAddress, 50, supplierObject.EmailAddress));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Street, 500, supplierObject.Street));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_City, 50, supplierObject.City));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_State, 50, supplierObject.State));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Zipcode, 50, supplierObject.Zipcode));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Country, 50, supplierObject.Country));
			AddParameter(cmd, pBool(SupplierBase.Property_IsActive, supplierObject.IsActive));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Note, supplierObject.Note));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_Website, 50, supplierObject.Website));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_TaxId, 50, supplierObject.TaxId));
			AddParameter(cmd, pNVarChar(SupplierBase.Property_SalesRepName, 250, supplierObject.SalesRepName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Supplier
        /// </summary>
        /// <param name="supplierObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SupplierBase supplierObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSUPPLIER);
	
				AddParameter(cmd, pInt32Out(SupplierBase.Property_Id));
				AddCommonParams(cmd, supplierObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					supplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					supplierObject.Id = (Int32)GetOutParameter(cmd, SupplierBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(supplierObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Supplier
        /// </summary>
        /// <param name="supplierObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SupplierBase supplierObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESUPPLIER);
				
				AddParameter(cmd, pInt32(SupplierBase.Property_Id, supplierObject.Id));
				AddCommonParams(cmd, supplierObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					supplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(supplierObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Supplier
        /// </summary>
        /// <param name="Id">Id of the Supplier object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESUPPLIER);	
				
				AddParameter(cmd, pInt32(SupplierBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Supplier), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Supplier object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Supplier object to retrieve</param>
        /// <returns>Supplier object, null if not found</returns>
		public Supplier Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBYID))
			{
				AddParameter( cmd, pInt32(SupplierBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Supplier objects 
        /// </summary>
        /// <returns>A list of Supplier objects</returns>
		public SupplierList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSUPPLIER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Supplier objects by PageRequest
        /// </summary>
        /// <returns>A list of Supplier objects</returns>
		public SupplierList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSUPPLIER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SupplierList _SupplierList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SupplierList;
			}
		}
		
		/// <summary>
        /// Retrieves all Supplier objects by query String
        /// </summary>
        /// <returns>A list of Supplier objects</returns>
		public SupplierList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Supplier Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Supplier
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Supplier Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Supplier
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SupplierRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSUPPLIERROWCOUNT))
			{
				SqlDataReader reader;
				_SupplierRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SupplierRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Supplier object
        /// </summary>
        /// <param name="supplierObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SupplierBase supplierObject, SqlDataReader reader, int start)
		{
			
				supplierObject.Id = reader.GetInt32( start + 0 );			
				supplierObject.SupplierId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) supplierObject.CompanyName = reader.GetString( start + 2 );			
				supplierObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) supplierObject.Name = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) supplierObject.OrderBy = reader.GetInt32( start + 5 );			
				supplierObject.ContactPerson = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) supplierObject.Phone = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) supplierObject.EmailAddress = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) supplierObject.Street = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) supplierObject.City = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) supplierObject.State = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) supplierObject.Zipcode = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) supplierObject.Country = reader.GetString( start + 13 );			
				supplierObject.IsActive = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) supplierObject.Note = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) supplierObject.Website = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) supplierObject.TaxId = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) supplierObject.SalesRepName = reader.GetString( start + 18 );			
			FillBaseObject(supplierObject, reader, (start + 19));

			
			supplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Supplier object
        /// </summary>
        /// <param name="supplierObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SupplierBase supplierObject, SqlDataReader reader)
		{
			FillObject(supplierObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Supplier object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Supplier object</returns>
		private Supplier GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Supplier supplierObject= new Supplier();
					FillObject(supplierObject, reader);
					return supplierObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Supplier objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Supplier objects</returns>
		private SupplierList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Supplier list
			SupplierList list = new SupplierList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Supplier supplierObject = new Supplier();
					FillObject(supplierObject, reader);

					list.Add(supplierObject);
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
