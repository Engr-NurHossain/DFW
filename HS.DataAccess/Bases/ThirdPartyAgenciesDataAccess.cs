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
	public partial class ThirdPartyAgenciesDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTHIRDPARTYAGENCIES = "InsertThirdPartyAgencies";
		private const string UPDATETHIRDPARTYAGENCIES = "UpdateThirdPartyAgencies";
		private const string DELETETHIRDPARTYAGENCIES = "DeleteThirdPartyAgencies";
		private const string GETTHIRDPARTYAGENCIESBYID = "GetThirdPartyAgenciesById";
		private const string GETALLTHIRDPARTYAGENCIES = "GetAllThirdPartyAgencies";
		private const string GETPAGEDTHIRDPARTYAGENCIES = "GetPagedThirdPartyAgencies";
		private const string GETTHIRDPARTYAGENCIESMAXIMUMID = "GetThirdPartyAgenciesMaximumId";
		private const string GETTHIRDPARTYAGENCIESROWCOUNT = "GetThirdPartyAgenciesRowCount";	
		private const string GETTHIRDPARTYAGENCIESBYQUERY = "GetThirdPartyAgenciesByQuery";
		#endregion
		
		#region Constructors
		public ThirdPartyAgenciesDataAccess(ClientContext context) : base(context) { }
		public ThirdPartyAgenciesDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="thirdPartyAgenciesObject"></param>
		private void AddCommonParams(SqlCommand cmd, ThirdPartyAgenciesBase thirdPartyAgenciesObject)
		{	
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_AgencyNo, 50, thirdPartyAgenciesObject.AgencyNo));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_AgencyType, 50, thirdPartyAgenciesObject.AgencyType));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_AgencyName, 100, thirdPartyAgenciesObject.AgencyName));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_City, 50, thirdPartyAgenciesObject.City));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_State, 50, thirdPartyAgenciesObject.State));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_Zipcode, 50, thirdPartyAgenciesObject.Zipcode));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_Phone1, 50, thirdPartyAgenciesObject.Phone1));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_Phone2, 50, thirdPartyAgenciesObject.Phone2));
			AddParameter(cmd, pDateTime(ThirdPartyAgenciesBase.Property_ChangeDate, thirdPartyAgenciesObject.ChangeDate));
			AddParameter(cmd, pNVarChar(ThirdPartyAgenciesBase.Property_Platform, 50, thirdPartyAgenciesObject.Platform));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ThirdPartyAgencies
        /// </summary>
        /// <param name="thirdPartyAgenciesObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ThirdPartyAgenciesBase thirdPartyAgenciesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTHIRDPARTYAGENCIES);
	
				AddParameter(cmd, pInt32Out(ThirdPartyAgenciesBase.Property_Id));
				AddCommonParams(cmd, thirdPartyAgenciesObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					thirdPartyAgenciesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					thirdPartyAgenciesObject.Id = (Int32)GetOutParameter(cmd, ThirdPartyAgenciesBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(thirdPartyAgenciesObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ThirdPartyAgencies
        /// </summary>
        /// <param name="thirdPartyAgenciesObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ThirdPartyAgenciesBase thirdPartyAgenciesObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETHIRDPARTYAGENCIES);
				
				AddParameter(cmd, pInt32(ThirdPartyAgenciesBase.Property_Id, thirdPartyAgenciesObject.Id));
				AddCommonParams(cmd, thirdPartyAgenciesObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					thirdPartyAgenciesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(thirdPartyAgenciesObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ThirdPartyAgencies
        /// </summary>
        /// <param name="Id">Id of the ThirdPartyAgencies object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETHIRDPARTYAGENCIES);	
				
				AddParameter(cmd, pInt32(ThirdPartyAgenciesBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ThirdPartyAgencies), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ThirdPartyAgencies object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ThirdPartyAgencies object to retrieve</param>
        /// <returns>ThirdPartyAgencies object, null if not found</returns>
		public ThirdPartyAgencies Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYAGENCIESBYID))
			{
				AddParameter( cmd, pInt32(ThirdPartyAgenciesBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ThirdPartyAgencies objects 
        /// </summary>
        /// <returns>A list of ThirdPartyAgencies objects</returns>
		public ThirdPartyAgenciesList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTHIRDPARTYAGENCIES))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ThirdPartyAgencies objects by PageRequest
        /// </summary>
        /// <returns>A list of ThirdPartyAgencies objects</returns>
		public ThirdPartyAgenciesList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTHIRDPARTYAGENCIES))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ThirdPartyAgenciesList _ThirdPartyAgenciesList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ThirdPartyAgenciesList;
			}
		}
		
		/// <summary>
        /// Retrieves all ThirdPartyAgencies objects by query String
        /// </summary>
        /// <returns>A list of ThirdPartyAgencies objects</returns>
		public ThirdPartyAgenciesList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYAGENCIESBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ThirdPartyAgencies Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ThirdPartyAgencies
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYAGENCIESMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ThirdPartyAgencies Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ThirdPartyAgencies
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ThirdPartyAgenciesRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYAGENCIESROWCOUNT))
			{
				SqlDataReader reader;
				_ThirdPartyAgenciesRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ThirdPartyAgenciesRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ThirdPartyAgencies object
        /// </summary>
        /// <param name="thirdPartyAgenciesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ThirdPartyAgenciesBase thirdPartyAgenciesObject, SqlDataReader reader, int start)
		{
			
				thirdPartyAgenciesObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) thirdPartyAgenciesObject.AgencyNo = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) thirdPartyAgenciesObject.AgencyType = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) thirdPartyAgenciesObject.AgencyName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) thirdPartyAgenciesObject.City = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) thirdPartyAgenciesObject.State = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) thirdPartyAgenciesObject.Zipcode = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) thirdPartyAgenciesObject.Phone1 = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) thirdPartyAgenciesObject.Phone2 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) thirdPartyAgenciesObject.ChangeDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) thirdPartyAgenciesObject.Platform = reader.GetString( start + 10 );			
			FillBaseObject(thirdPartyAgenciesObject, reader, (start + 11));

			
			thirdPartyAgenciesObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ThirdPartyAgencies object
        /// </summary>
        /// <param name="thirdPartyAgenciesObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ThirdPartyAgenciesBase thirdPartyAgenciesObject, SqlDataReader reader)
		{
			FillObject(thirdPartyAgenciesObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ThirdPartyAgencies object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ThirdPartyAgencies object</returns>
		private ThirdPartyAgencies GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ThirdPartyAgencies thirdPartyAgenciesObject= new ThirdPartyAgencies();
					FillObject(thirdPartyAgenciesObject, reader);
					return thirdPartyAgenciesObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ThirdPartyAgencies objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ThirdPartyAgencies objects</returns>
		private ThirdPartyAgenciesList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ThirdPartyAgencies list
			ThirdPartyAgenciesList list = new ThirdPartyAgenciesList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ThirdPartyAgencies thirdPartyAgenciesObject = new ThirdPartyAgencies();
					FillObject(thirdPartyAgenciesObject, reader);

					list.Add(thirdPartyAgenciesObject);
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
