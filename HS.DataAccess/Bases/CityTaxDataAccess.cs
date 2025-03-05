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
	public partial class CityTaxDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCITYTAX = "InsertCityTax";
		private const string UPDATECITYTAX = "UpdateCityTax";
		private const string DELETECITYTAX = "DeleteCityTax";
		private const string GETCITYTAXBYID = "GetCityTaxById";
		private const string GETALLCITYTAX = "GetAllCityTax";
		private const string GETPAGEDCITYTAX = "GetPagedCityTax";
		private const string GETCITYTAXMAXIMUMID = "GetCityTaxMaximumId";
		private const string GETCITYTAXROWCOUNT = "GetCityTaxRowCount";	
		private const string GETCITYTAXBYQUERY = "GetCityTaxByQuery";
		#endregion
		
		#region Constructors
		public CityTaxDataAccess(ClientContext context) : base(context) { }
		public CityTaxDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="cityTaxObject"></param>
		private void AddCommonParams(SqlCommand cmd, CityTaxBase cityTaxObject)
		{	
			AddParameter(cmd, pGuid(CityTaxBase.Property_CompanyId, cityTaxObject.CompanyId));
			AddParameter(cmd, pNVarChar(CityTaxBase.Property_City, 50, cityTaxObject.City));
			AddParameter(cmd, pNVarChar(CityTaxBase.Property_Country, 50, cityTaxObject.Country));
			AddParameter(cmd, pNVarChar(CityTaxBase.Property_State, 50, cityTaxObject.State));
			AddParameter(cmd, pNVarChar(CityTaxBase.Property_ZipCode, 50, cityTaxObject.ZipCode));
			AddParameter(cmd, pDouble(CityTaxBase.Property_Rate, cityTaxObject.Rate));
			AddParameter(cmd, pBool(CityTaxBase.Property_IsActive, cityTaxObject.IsActive));
			AddParameter(cmd, pNVarChar(CityTaxBase.Property_TaxText, 50, cityTaxObject.TaxText));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CityTax
        /// </summary>
        /// <param name="cityTaxObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CityTaxBase cityTaxObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCITYTAX);
	
				AddParameter(cmd, pInt32Out(CityTaxBase.Property_Id));
				AddCommonParams(cmd, cityTaxObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					cityTaxObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					cityTaxObject.Id = (Int32)GetOutParameter(cmd, CityTaxBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(cityTaxObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CityTax
        /// </summary>
        /// <param name="cityTaxObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CityTaxBase cityTaxObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECITYTAX);
				
				AddParameter(cmd, pInt32(CityTaxBase.Property_Id, cityTaxObject.Id));
				AddCommonParams(cmd, cityTaxObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					cityTaxObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(cityTaxObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CityTax
        /// </summary>
        /// <param name="Id">Id of the CityTax object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECITYTAX);	
				
				AddParameter(cmd, pInt32(CityTaxBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CityTax), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CityTax object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CityTax object to retrieve</param>
        /// <returns>CityTax object, null if not found</returns>
		public CityTax Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYTAXBYID))
			{
				AddParameter( cmd, pInt32(CityTaxBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CityTax objects 
        /// </summary>
        /// <returns>A list of CityTax objects</returns>
		public CityTaxList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCITYTAX))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CityTax objects by PageRequest
        /// </summary>
        /// <returns>A list of CityTax objects</returns>
		public CityTaxList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCITYTAX))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CityTaxList _CityTaxList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CityTaxList;
			}
		}
		
		/// <summary>
        /// Retrieves all CityTax objects by query String
        /// </summary>
        /// <returns>A list of CityTax objects</returns>
		public CityTaxList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYTAXBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CityTax Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CityTax
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYTAXMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CityTax Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CityTax
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CityTaxRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYTAXROWCOUNT))
			{
				SqlDataReader reader;
				_CityTaxRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CityTaxRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CityTax object
        /// </summary>
        /// <param name="cityTaxObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CityTaxBase cityTaxObject, SqlDataReader reader, int start)
		{
			
				cityTaxObject.Id = reader.GetInt32( start + 0 );			
				cityTaxObject.CompanyId = reader.GetGuid( start + 1 );			
				cityTaxObject.City = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) cityTaxObject.Country = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) cityTaxObject.State = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) cityTaxObject.ZipCode = reader.GetString( start + 5 );			
				cityTaxObject.Rate = reader.GetDouble( start + 6 );			
				cityTaxObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) cityTaxObject.TaxText = reader.GetString( start + 8 );			
			FillBaseObject(cityTaxObject, reader, (start + 9));

			
			cityTaxObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CityTax object
        /// </summary>
        /// <param name="cityTaxObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CityTaxBase cityTaxObject, SqlDataReader reader)
		{
			FillObject(cityTaxObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CityTax object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CityTax object</returns>
		private CityTax GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CityTax cityTaxObject= new CityTax();
					FillObject(cityTaxObject, reader);
					return cityTaxObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CityTax objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CityTax objects</returns>
		private CityTaxList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CityTax list
			CityTaxList list = new CityTaxList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CityTax cityTaxObject = new CityTax();
					FillObject(cityTaxObject, reader);

					list.Add(cityTaxObject);
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
