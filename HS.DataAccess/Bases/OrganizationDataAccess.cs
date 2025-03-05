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
	public partial class OrganizationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTORGANIZATION = "InsertOrganization";
		private const string UPDATEORGANIZATION = "UpdateOrganization";
		private const string DELETEORGANIZATION = "DeleteOrganization";
		private const string GETORGANIZATIONBYID = "GetOrganizationById";
		private const string GETALLORGANIZATION = "GetAllOrganization";
		private const string GETPAGEDORGANIZATION = "GetPagedOrganization";
		private const string GETORGANIZATIONMAXIMUMID = "GetOrganizationMaximumId";
		private const string GETORGANIZATIONROWCOUNT = "GetOrganizationRowCount";	
		private const string GETORGANIZATIONBYQUERY = "GetOrganizationByQuery";
		#endregion
		
		#region Constructors
		public OrganizationDataAccess(ClientContext context) : base(context) { }
		public OrganizationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="organizationObject"></param>
		private void AddCommonParams(SqlCommand cmd, OrganizationBase organizationObject)
		{	
			AddParameter(cmd, pGuid(OrganizationBase.Property_CompanyId, organizationObject.CompanyId));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_CompanyName, 50, organizationObject.CompanyName));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_UserName, 150, organizationObject.UserName));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_EmailAdress, 150, organizationObject.EmailAdress));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_FirstName, 150, organizationObject.FirstName));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_LastName, 150, organizationObject.LastName));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Phone, 50, organizationObject.Phone));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Fax, 50, organizationObject.Fax));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Address, organizationObject.Address));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Street, 500, organizationObject.Street));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_City, 50, organizationObject.City));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_State, 50, organizationObject.State));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_ZipCode, 50, organizationObject.ZipCode));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Website, 50, organizationObject.Website));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_CompanyType, 50, organizationObject.CompanyType));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_Note, organizationObject.Note));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_CompanyLogo, 250, organizationObject.CompanyLogo));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_ConnectionString, organizationObject.ConnectionString));
			AddParameter(cmd, pNVarChar(OrganizationBase.Property_MasterPassword, 50, organizationObject.MasterPassword));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Organization
        /// </summary>
        /// <param name="organizationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(OrganizationBase organizationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTORGANIZATION);
	
				AddParameter(cmd, pInt32Out(OrganizationBase.Property_Id));
				AddCommonParams(cmd, organizationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					organizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					organizationObject.Id = (Int32)GetOutParameter(cmd, OrganizationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(organizationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Organization
        /// </summary>
        /// <param name="organizationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(OrganizationBase organizationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEORGANIZATION);
				
				AddParameter(cmd, pInt32(OrganizationBase.Property_Id, organizationObject.Id));
				AddCommonParams(cmd, organizationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					organizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(organizationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Organization
        /// </summary>
        /// <param name="Id">Id of the Organization object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEORGANIZATION);	
				
				AddParameter(cmd, pInt32(OrganizationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Organization), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Organization object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Organization object to retrieve</param>
        /// <returns>Organization object, null if not found</returns>
		public Organization Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETORGANIZATIONBYID))
			{
				AddParameter( cmd, pInt32(OrganizationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Organization objects 
        /// </summary>
        /// <returns>A list of Organization objects</returns>
		public OrganizationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLORGANIZATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Organization objects by PageRequest
        /// </summary>
        /// <returns>A list of Organization objects</returns>
		public OrganizationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDORGANIZATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				OrganizationList _OrganizationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _OrganizationList;
			}
		}
		
		/// <summary>
        /// Retrieves all Organization objects by query String
        /// </summary>
        /// <returns>A list of Organization objects</returns>
		public OrganizationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETORGANIZATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Organization Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Organization
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETORGANIZATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Organization Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Organization
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _OrganizationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETORGANIZATIONROWCOUNT))
			{
				SqlDataReader reader;
				_OrganizationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _OrganizationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Organization object
        /// </summary>
        /// <param name="organizationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(OrganizationBase organizationObject, SqlDataReader reader, int start)
		{
			
				organizationObject.Id = reader.GetInt32( start + 0 );			
				organizationObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) organizationObject.CompanyName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) organizationObject.UserName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) organizationObject.EmailAdress = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) organizationObject.FirstName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) organizationObject.LastName = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) organizationObject.Phone = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) organizationObject.Fax = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) organizationObject.Address = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) organizationObject.Street = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) organizationObject.City = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) organizationObject.State = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) organizationObject.ZipCode = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) organizationObject.Website = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) organizationObject.CompanyType = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) organizationObject.Note = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) organizationObject.CompanyLogo = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) organizationObject.ConnectionString = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) organizationObject.MasterPassword = reader.GetString( start + 19 );			
			FillBaseObject(organizationObject, reader, (start + 20));

			
			organizationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Organization object
        /// </summary>
        /// <param name="organizationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(OrganizationBase organizationObject, SqlDataReader reader)
		{
			FillObject(organizationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Organization object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Organization object</returns>
		private Organization GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Organization organizationObject= new Organization();
					FillObject(organizationObject, reader);
					return organizationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Organization objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Organization objects</returns>
		private OrganizationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Organization list
			OrganizationList list = new OrganizationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Organization organizationObject = new Organization();
					FillObject(organizationObject, reader);

					list.Add(organizationObject);
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
