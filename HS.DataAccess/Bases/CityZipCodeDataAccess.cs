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
	public partial class CityZipCodeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCITYZIPCODE = "InsertCityZipCode";
		private const string UPDATECITYZIPCODE = "UpdateCityZipCode";
		private const string DELETECITYZIPCODE = "DeleteCityZipCode";
		private const string GETCITYZIPCODEBYID = "GetCityZipCodeById";
		private const string GETALLCITYZIPCODE = "GetAllCityZipCode";
		private const string GETPAGEDCITYZIPCODE = "GetPagedCityZipCode";
		private const string GETCITYZIPCODEMAXIMUMID = "GetCityZipCodeMaximumId";
		private const string GETCITYZIPCODEROWCOUNT = "GetCityZipCodeRowCount";	
		private const string GETCITYZIPCODEBYQUERY = "GetCityZipCodeByQuery";
		#endregion
		
		#region Constructors
		public CityZipCodeDataAccess(ClientContext context) : base(context) { }
		public CityZipCodeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="cityZipCodeObject"></param>
		private void AddCommonParams(SqlCommand cmd, CityZipCodeBase cityZipCodeObject)
		{	
			AddParameter(cmd, pNVarChar(CityZipCodeBase.Property_ZipCode, 6, cityZipCodeObject.ZipCode));
			AddParameter(cmd, pVarChar(CityZipCodeBase.Property_City, 35, cityZipCodeObject.City));
			AddParameter(cmd, pNVarChar(CityZipCodeBase.Property_State, 50, cityZipCodeObject.State));
			AddParameter(cmd, pVarChar(CityZipCodeBase.Property_County, 45, cityZipCodeObject.County));
			AddParameter(cmd, pVarChar(CityZipCodeBase.Property_AreaCode, 55, cityZipCodeObject.AreaCode));
			AddParameter(cmd, pDouble(CityZipCodeBase.Property_Latitude, cityZipCodeObject.Latitude));
			AddParameter(cmd, pDouble(CityZipCodeBase.Property_Longitude, cityZipCodeObject.Longitude));
			AddParameter(cmd, pNVarChar(CityZipCodeBase.Property_TimeZone, 50, cityZipCodeObject.TimeZone));
			AddParameter(cmd, pInt32(CityZipCodeBase.Property_Elevation, cityZipCodeObject.Elevation));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CityZipCode
        /// </summary>
        /// <param name="cityZipCodeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CityZipCodeBase cityZipCodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCITYZIPCODE);
	
				AddParameter(cmd, pInt32Out(CityZipCodeBase.Property_Id));
				AddCommonParams(cmd, cityZipCodeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					cityZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					cityZipCodeObject.Id = (Int32)GetOutParameter(cmd, CityZipCodeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(cityZipCodeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CityZipCode
        /// </summary>
        /// <param name="cityZipCodeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CityZipCodeBase cityZipCodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECITYZIPCODE);
				
				AddParameter(cmd, pInt32(CityZipCodeBase.Property_Id, cityZipCodeObject.Id));
				AddCommonParams(cmd, cityZipCodeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					cityZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(cityZipCodeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CityZipCode
        /// </summary>
        /// <param name="Id">Id of the CityZipCode object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECITYZIPCODE);	
				
				AddParameter(cmd, pInt32(CityZipCodeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CityZipCode), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CityZipCode object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CityZipCode object to retrieve</param>
        /// <returns>CityZipCode object, null if not found</returns>
		public CityZipCode Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODEBYID))
			{
				AddParameter( cmd, pInt32(CityZipCodeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CityZipCode objects 
        /// </summary>
        /// <returns>A list of CityZipCode objects</returns>
		public CityZipCodeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCITYZIPCODE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CityZipCode objects by PageRequest
        /// </summary>
        /// <returns>A list of CityZipCode objects</returns>
		public CityZipCodeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCITYZIPCODE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CityZipCodeList _CityZipCodeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CityZipCodeList;
			}
		}
		
		/// <summary>
        /// Retrieves all CityZipCode objects by query String
        /// </summary>
        /// <returns>A list of CityZipCode objects</returns>
		public CityZipCodeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CityZipCode Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CityZipCode
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CityZipCode Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CityZipCode
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CityZipCodeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCITYZIPCODEROWCOUNT))
			{
				SqlDataReader reader;
				_CityZipCodeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CityZipCodeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CityZipCode object
        /// </summary>
        /// <param name="cityZipCodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CityZipCodeBase cityZipCodeObject, SqlDataReader reader, int start)
		{
			
				cityZipCodeObject.Id = reader.GetInt32( start + 0 );			
				cityZipCodeObject.ZipCode = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) cityZipCodeObject.City = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) cityZipCodeObject.State = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) cityZipCodeObject.County = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) cityZipCodeObject.AreaCode = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) cityZipCodeObject.Latitude = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) cityZipCodeObject.Longitude = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) cityZipCodeObject.TimeZone = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) cityZipCodeObject.Elevation = reader.GetInt32( start + 9 );			
			FillBaseObject(cityZipCodeObject, reader, (start + 10));

			
			cityZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CityZipCode object
        /// </summary>
        /// <param name="cityZipCodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CityZipCodeBase cityZipCodeObject, SqlDataReader reader)
		{
			FillObject(cityZipCodeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CityZipCode object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CityZipCode object</returns>
		private CityZipCode GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CityZipCode cityZipCodeObject= new CityZipCode();
					FillObject(cityZipCodeObject, reader);
					return cityZipCodeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CityZipCode objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CityZipCode objects</returns>
		private CityZipCodeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CityZipCode list
			CityZipCodeList list = new CityZipCodeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CityZipCode cityZipCodeObject = new CityZipCode();
					FillObject(cityZipCodeObject, reader);

					list.Add(cityZipCodeObject);
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
