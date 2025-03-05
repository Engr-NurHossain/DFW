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
	public partial class CompanyBranchDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMPANYBRANCH = "InsertCompanyBranch";
		private const string UPDATECOMPANYBRANCH = "UpdateCompanyBranch";
		private const string DELETECOMPANYBRANCH = "DeleteCompanyBranch";
		private const string GETCOMPANYBRANCHBYID = "GetCompanyBranchById";
		private const string GETALLCOMPANYBRANCH = "GetAllCompanyBranch";
		private const string GETPAGEDCOMPANYBRANCH = "GetPagedCompanyBranch";
		private const string GETCOMPANYBRANCHMAXIMUMID = "GetCompanyBranchMaximumId";
		private const string GETCOMPANYBRANCHROWCOUNT = "GetCompanyBranchRowCount";	
		private const string GETCOMPANYBRANCHBYQUERY = "GetCompanyBranchByQuery";
		#endregion
		
		#region Constructors
		public CompanyBranchDataAccess(ClientContext context) : base(context) { }
		public CompanyBranchDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="companyBranchObject"></param>
		private void AddCommonParams(SqlCommand cmd, CompanyBranchBase companyBranchObject)
		{	
			AddParameter(cmd, pGuid(CompanyBranchBase.Property_CompanyId, companyBranchObject.CompanyId));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_Name, 150, companyBranchObject.Name));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_Street, 250, companyBranchObject.Street));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_City, 50, companyBranchObject.City));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_State, 50, companyBranchObject.State));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_ZipCode, 50, companyBranchObject.ZipCode));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_Logo, 250, companyBranchObject.Logo));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_ColorLogo, 250, companyBranchObject.ColorLogo));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_EmailLogo, 250, companyBranchObject.EmailLogo));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_TimeZone, 250, companyBranchObject.TimeZone));
			AddParameter(cmd, pDouble(CompanyBranchBase.Property_Tax, companyBranchObject.Tax));
			AddParameter(cmd, pBool(CompanyBranchBase.Property_IsMainBranch, companyBranchObject.IsMainBranch));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_Division, 50, companyBranchObject.Division));
			AddParameter(cmd, pNVarChar(CompanyBranchBase.Property_Region, 50, companyBranchObject.Region));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CompanyBranch
        /// </summary>
        /// <param name="companyBranchObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CompanyBranchBase companyBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMPANYBRANCH);
	
				AddParameter(cmd, pInt32Out(CompanyBranchBase.Property_Id));
				AddCommonParams(cmd, companyBranchObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					companyBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					companyBranchObject.Id = (Int32)GetOutParameter(cmd, CompanyBranchBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(companyBranchObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CompanyBranch
        /// </summary>
        /// <param name="companyBranchObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CompanyBranchBase companyBranchObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMPANYBRANCH);
				
				AddParameter(cmd, pInt32(CompanyBranchBase.Property_Id, companyBranchObject.Id));
				AddCommonParams(cmd, companyBranchObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					companyBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(companyBranchObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CompanyBranch
        /// </summary>
        /// <param name="Id">Id of the CompanyBranch object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMPANYBRANCH);	
				
				AddParameter(cmd, pInt32(CompanyBranchBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CompanyBranch), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CompanyBranch object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CompanyBranch object to retrieve</param>
        /// <returns>CompanyBranch object, null if not found</returns>
		public CompanyBranch Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYBRANCHBYID))
			{
				AddParameter( cmd, pInt32(CompanyBranchBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CompanyBranch objects 
        /// </summary>
        /// <returns>A list of CompanyBranch objects</returns>
		public CompanyBranchList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMPANYBRANCH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CompanyBranch objects by PageRequest
        /// </summary>
        /// <returns>A list of CompanyBranch objects</returns>
		public CompanyBranchList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMPANYBRANCH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CompanyBranchList _CompanyBranchList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CompanyBranchList;
			}
		}
		
		/// <summary>
        /// Retrieves all CompanyBranch objects by query String
        /// </summary>
        /// <returns>A list of CompanyBranch objects</returns>
		public CompanyBranchList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYBRANCHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CompanyBranch Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CompanyBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYBRANCHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CompanyBranch Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CompanyBranch
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CompanyBranchRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYBRANCHROWCOUNT))
			{
				SqlDataReader reader;
				_CompanyBranchRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CompanyBranchRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CompanyBranch object
        /// </summary>
        /// <param name="companyBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CompanyBranchBase companyBranchObject, SqlDataReader reader, int start)
		{
			
				companyBranchObject.Id = reader.GetInt32( start + 0 );			
				companyBranchObject.CompanyId = reader.GetGuid( start + 1 );			
				companyBranchObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) companyBranchObject.Street = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) companyBranchObject.City = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) companyBranchObject.State = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) companyBranchObject.ZipCode = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) companyBranchObject.Logo = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) companyBranchObject.ColorLogo = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) companyBranchObject.EmailLogo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) companyBranchObject.TimeZone = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) companyBranchObject.Tax = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) companyBranchObject.IsMainBranch = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) companyBranchObject.Division = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) companyBranchObject.Region = reader.GetString( start + 14 );			
			FillBaseObject(companyBranchObject, reader, (start + 15));

			
			companyBranchObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CompanyBranch object
        /// </summary>
        /// <param name="companyBranchObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CompanyBranchBase companyBranchObject, SqlDataReader reader)
		{
			FillObject(companyBranchObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CompanyBranch object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CompanyBranch object</returns>
		private CompanyBranch GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CompanyBranch companyBranchObject= new CompanyBranch();
					FillObject(companyBranchObject, reader);
					return companyBranchObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CompanyBranch objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CompanyBranch objects</returns>
		private CompanyBranchList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CompanyBranch list
			CompanyBranchList list = new CompanyBranchList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CompanyBranch companyBranchObject = new CompanyBranch();
					FillObject(companyBranchObject, reader);

					list.Add(companyBranchObject);
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
