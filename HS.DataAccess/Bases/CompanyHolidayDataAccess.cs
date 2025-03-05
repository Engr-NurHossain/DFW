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
	public partial class CompanyHolidayDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMPANYHOLIDAY = "InsertCompanyHoliday";
		private const string UPDATECOMPANYHOLIDAY = "UpdateCompanyHoliday";
		private const string DELETECOMPANYHOLIDAY = "DeleteCompanyHoliday";
		private const string GETCOMPANYHOLIDAYBYID = "GetCompanyHolidayById";
		private const string GETALLCOMPANYHOLIDAY = "GetAllCompanyHoliday";
		private const string GETPAGEDCOMPANYHOLIDAY = "GetPagedCompanyHoliday";
		private const string GETCOMPANYHOLIDAYMAXIMUMID = "GetCompanyHolidayMaximumId";
		private const string GETCOMPANYHOLIDAYROWCOUNT = "GetCompanyHolidayRowCount";	
		private const string GETCOMPANYHOLIDAYBYQUERY = "GetCompanyHolidayByQuery";
		#endregion
		
		#region Constructors
		public CompanyHolidayDataAccess(ClientContext context) : base(context) { }
		public CompanyHolidayDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="companyHolidayObject"></param>
		private void AddCommonParams(SqlCommand cmd, CompanyHolidayBase companyHolidayObject)
		{	
			AddParameter(cmd, pGuid(CompanyHolidayBase.Property_CompanyId, companyHolidayObject.CompanyId));
			AddParameter(cmd, pNVarChar(CompanyHolidayBase.Property_Year, 50, companyHolidayObject.Year));
			AddParameter(cmd, pDateTime(CompanyHolidayBase.Property_Holiday, companyHolidayObject.Holiday));
			AddParameter(cmd, pNVarChar(CompanyHolidayBase.Property_HolidayDetails, 150, companyHolidayObject.HolidayDetails));
			AddParameter(cmd, pBool(CompanyHolidayBase.Property_IsActive, companyHolidayObject.IsActive));
			AddParameter(cmd, pDateTime(CompanyHolidayBase.Property_CreatedDate, companyHolidayObject.CreatedDate));
			AddParameter(cmd, pGuid(CompanyHolidayBase.Property_CreatedBy, companyHolidayObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CompanyHoliday
        /// </summary>
        /// <param name="companyHolidayObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CompanyHolidayBase companyHolidayObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMPANYHOLIDAY);
	
				AddParameter(cmd, pInt32Out(CompanyHolidayBase.Property_Id));
				AddCommonParams(cmd, companyHolidayObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					companyHolidayObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					companyHolidayObject.Id = (Int32)GetOutParameter(cmd, CompanyHolidayBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(companyHolidayObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CompanyHoliday
        /// </summary>
        /// <param name="companyHolidayObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CompanyHolidayBase companyHolidayObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMPANYHOLIDAY);
				
				AddParameter(cmd, pInt32(CompanyHolidayBase.Property_Id, companyHolidayObject.Id));
				AddCommonParams(cmd, companyHolidayObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					companyHolidayObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(companyHolidayObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CompanyHoliday
        /// </summary>
        /// <param name="Id">Id of the CompanyHoliday object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMPANYHOLIDAY);	
				
				AddParameter(cmd, pInt32(CompanyHolidayBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CompanyHoliday), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CompanyHoliday object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CompanyHoliday object to retrieve</param>
        /// <returns>CompanyHoliday object, null if not found</returns>
		public CompanyHoliday Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYHOLIDAYBYID))
			{
				AddParameter( cmd, pInt32(CompanyHolidayBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CompanyHoliday objects 
        /// </summary>
        /// <returns>A list of CompanyHoliday objects</returns>
		public CompanyHolidayList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMPANYHOLIDAY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CompanyHoliday objects by PageRequest
        /// </summary>
        /// <returns>A list of CompanyHoliday objects</returns>
		public CompanyHolidayList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMPANYHOLIDAY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CompanyHolidayList _CompanyHolidayList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CompanyHolidayList;
			}
		}
		
		/// <summary>
        /// Retrieves all CompanyHoliday objects by query String
        /// </summary>
        /// <returns>A list of CompanyHoliday objects</returns>
		public CompanyHolidayList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYHOLIDAYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CompanyHoliday Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CompanyHoliday
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYHOLIDAYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CompanyHoliday Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CompanyHoliday
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CompanyHolidayRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMPANYHOLIDAYROWCOUNT))
			{
				SqlDataReader reader;
				_CompanyHolidayRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CompanyHolidayRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CompanyHoliday object
        /// </summary>
        /// <param name="companyHolidayObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CompanyHolidayBase companyHolidayObject, SqlDataReader reader, int start)
		{
			
				companyHolidayObject.Id = reader.GetInt32( start + 0 );			
				companyHolidayObject.CompanyId = reader.GetGuid( start + 1 );			
				companyHolidayObject.Year = reader.GetString( start + 2 );			
				companyHolidayObject.Holiday = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) companyHolidayObject.HolidayDetails = reader.GetString( start + 4 );			
				companyHolidayObject.IsActive = reader.GetBoolean( start + 5 );			
				companyHolidayObject.CreatedDate = reader.GetDateTime( start + 6 );			
				companyHolidayObject.CreatedBy = reader.GetGuid( start + 7 );			
			FillBaseObject(companyHolidayObject, reader, (start + 8));

			
			companyHolidayObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CompanyHoliday object
        /// </summary>
        /// <param name="companyHolidayObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CompanyHolidayBase companyHolidayObject, SqlDataReader reader)
		{
			FillObject(companyHolidayObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CompanyHoliday object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CompanyHoliday object</returns>
		private CompanyHoliday GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CompanyHoliday companyHolidayObject= new CompanyHoliday();
					FillObject(companyHolidayObject, reader);
					return companyHolidayObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CompanyHoliday objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CompanyHoliday objects</returns>
		private CompanyHolidayList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CompanyHoliday list
			CompanyHolidayList list = new CompanyHolidayList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CompanyHoliday companyHolidayObject = new CompanyHoliday();
					FillObject(companyHolidayObject, reader);

					list.Add(companyHolidayObject);
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