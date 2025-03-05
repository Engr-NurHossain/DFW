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
	public partial class ResturantSystemSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTSYSTEMSETTING = "InsertResturantSystemSetting";
		private const string UPDATERESTURANTSYSTEMSETTING = "UpdateResturantSystemSetting";
		private const string DELETERESTURANTSYSTEMSETTING = "DeleteResturantSystemSetting";
		private const string GETRESTURANTSYSTEMSETTINGBYID = "GetResturantSystemSettingById";
		private const string GETALLRESTURANTSYSTEMSETTING = "GetAllResturantSystemSetting";
		private const string GETPAGEDRESTURANTSYSTEMSETTING = "GetPagedResturantSystemSetting";
		private const string GETRESTURANTSYSTEMSETTINGMAXIMUMID = "GetResturantSystemSettingMaximumId";
		private const string GETRESTURANTSYSTEMSETTINGROWCOUNT = "GetResturantSystemSettingRowCount";	
		private const string GETRESTURANTSYSTEMSETTINGBYQUERY = "GetResturantSystemSettingByQuery";
		#endregion
		
		#region Constructors
		public ResturantSystemSettingDataAccess(ClientContext context) : base(context) { }
		public ResturantSystemSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantSystemSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantSystemSettingBase resturantSystemSettingObject)
		{	
			AddParameter(cmd, pGuid(ResturantSystemSettingBase.Property_CompanyId, resturantSystemSettingObject.CompanyId));
			AddParameter(cmd, pNVarChar(ResturantSystemSettingBase.Property_Restaurant, resturantSystemSettingObject.Restaurant));
			AddParameter(cmd, pNVarChar(ResturantSystemSettingBase.Property_Logo, resturantSystemSettingObject.Logo));
			AddParameter(cmd, pDouble(ResturantSystemSettingBase.Property_TaxRate, resturantSystemSettingObject.TaxRate));
			AddParameter(cmd, pNVarChar(ResturantSystemSettingBase.Property_PrimaryContact, resturantSystemSettingObject.PrimaryContact));
			AddParameter(cmd, pDateTime(ResturantSystemSettingBase.Property_CreatedDate, resturantSystemSettingObject.CreatedDate));
			AddParameter(cmd, pGuid(ResturantSystemSettingBase.Property_CreatedBy, resturantSystemSettingObject.CreatedBy));
			AddParameter(cmd, pNVarChar(ResturantSystemSettingBase.Property_AuthApiLoginKey, 250, resturantSystemSettingObject.AuthApiLoginKey));
			AddParameter(cmd, pNVarChar(ResturantSystemSettingBase.Property_AuthApiTransactionKey, 250, resturantSystemSettingObject.AuthApiTransactionKey));
			AddParameter(cmd, pDouble(ResturantSystemSettingBase.Property_MinimumOrderValue, resturantSystemSettingObject.MinimumOrderValue));
			AddParameter(cmd, pBool(ResturantSystemSettingBase.Property_AutoConfirmOrder, resturantSystemSettingObject.AutoConfirmOrder));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantSystemSetting
        /// </summary>
        /// <param name="resturantSystemSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantSystemSettingBase resturantSystemSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTSYSTEMSETTING);
	
				AddParameter(cmd, pInt32Out(ResturantSystemSettingBase.Property_Id));
				AddCommonParams(cmd, resturantSystemSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantSystemSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantSystemSettingObject.Id = (Int32)GetOutParameter(cmd, ResturantSystemSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantSystemSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantSystemSetting
        /// </summary>
        /// <param name="resturantSystemSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantSystemSettingBase resturantSystemSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTSYSTEMSETTING);
				
				AddParameter(cmd, pInt32(ResturantSystemSettingBase.Property_Id, resturantSystemSettingObject.Id));
				AddCommonParams(cmd, resturantSystemSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantSystemSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantSystemSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantSystemSetting
        /// </summary>
        /// <param name="Id">Id of the ResturantSystemSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTSYSTEMSETTING);	
				
				AddParameter(cmd, pInt32(ResturantSystemSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantSystemSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantSystemSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantSystemSetting object to retrieve</param>
        /// <returns>ResturantSystemSetting object, null if not found</returns>
		public ResturantSystemSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTSYSTEMSETTINGBYID))
			{
				AddParameter( cmd, pInt32(ResturantSystemSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantSystemSetting objects 
        /// </summary>
        /// <returns>A list of ResturantSystemSetting objects</returns>
		public ResturantSystemSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTSYSTEMSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantSystemSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantSystemSetting objects</returns>
		public ResturantSystemSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTSYSTEMSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantSystemSettingList _ResturantSystemSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantSystemSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantSystemSetting objects by query String
        /// </summary>
        /// <returns>A list of ResturantSystemSetting objects</returns>
		public ResturantSystemSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTSYSTEMSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantSystemSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantSystemSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTSYSTEMSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantSystemSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantSystemSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantSystemSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTSYSTEMSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantSystemSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantSystemSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantSystemSetting object
        /// </summary>
        /// <param name="resturantSystemSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantSystemSettingBase resturantSystemSettingObject, SqlDataReader reader, int start)
		{
			
				resturantSystemSettingObject.Id = reader.GetInt32( start + 0 );			
				resturantSystemSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) resturantSystemSettingObject.Restaurant = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) resturantSystemSettingObject.Logo = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) resturantSystemSettingObject.TaxRate = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) resturantSystemSettingObject.PrimaryContact = reader.GetString( start + 5 );			
				resturantSystemSettingObject.CreatedDate = reader.GetDateTime( start + 6 );			
				resturantSystemSettingObject.CreatedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) resturantSystemSettingObject.AuthApiLoginKey = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) resturantSystemSettingObject.AuthApiTransactionKey = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) resturantSystemSettingObject.MinimumOrderValue = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) resturantSystemSettingObject.AutoConfirmOrder = reader.GetBoolean( start + 11 );			
			FillBaseObject(resturantSystemSettingObject, reader, (start + 12));

			
			resturantSystemSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantSystemSetting object
        /// </summary>
        /// <param name="resturantSystemSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantSystemSettingBase resturantSystemSettingObject, SqlDataReader reader)
		{
			FillObject(resturantSystemSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantSystemSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantSystemSetting object</returns>
		private ResturantSystemSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantSystemSetting resturantSystemSettingObject= new ResturantSystemSetting();
					FillObject(resturantSystemSettingObject, reader);
					return resturantSystemSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantSystemSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantSystemSetting objects</returns>
		private ResturantSystemSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantSystemSetting list
			ResturantSystemSettingList list = new ResturantSystemSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantSystemSetting resturantSystemSettingObject = new ResturantSystemSetting();
					FillObject(resturantSystemSettingObject, reader);

					list.Add(resturantSystemSettingObject);
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
