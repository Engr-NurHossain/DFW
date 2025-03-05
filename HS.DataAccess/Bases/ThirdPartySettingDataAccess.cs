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
	public partial class ThirdPartySettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTHIRDPARTYSETTING = "InsertThirdPartySetting";
		private const string UPDATETHIRDPARTYSETTING = "UpdateThirdPartySetting";
		private const string DELETETHIRDPARTYSETTING = "DeleteThirdPartySetting";
		private const string GETTHIRDPARTYSETTINGBYID = "GetThirdPartySettingById";
		private const string GETALLTHIRDPARTYSETTING = "GetAllThirdPartySetting";
		private const string GETPAGEDTHIRDPARTYSETTING = "GetPagedThirdPartySetting";
		private const string GETTHIRDPARTYSETTINGMAXIMUMID = "GetThirdPartySettingMaximumId";
		private const string GETTHIRDPARTYSETTINGROWCOUNT = "GetThirdPartySettingRowCount";	
		private const string GETTHIRDPARTYSETTINGBYQUERY = "GetThirdPartySettingByQuery";
		#endregion
		
		#region Constructors
		public ThirdPartySettingDataAccess(ClientContext context) : base(context) { }
		public ThirdPartySettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="thirdPartySettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, ThirdPartySettingBase thirdPartySettingObject)
		{	
			AddParameter(cmd, pGuid(ThirdPartySettingBase.Property_CompanyId, thirdPartySettingObject.CompanyId));
			AddParameter(cmd, pNVarChar(ThirdPartySettingBase.Property_Type, 50, thirdPartySettingObject.Type));
			AddParameter(cmd, pNVarChar(ThirdPartySettingBase.Property_Name, 50, thirdPartySettingObject.Name));
			AddParameter(cmd, pNVarChar(ThirdPartySettingBase.Property_Value, 50, thirdPartySettingObject.Value));
			AddParameter(cmd, pBool(ThirdPartySettingBase.Property_IsActive, thirdPartySettingObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ThirdPartySetting
        /// </summary>
        /// <param name="thirdPartySettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ThirdPartySettingBase thirdPartySettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTHIRDPARTYSETTING);
	
				AddParameter(cmd, pInt32Out(ThirdPartySettingBase.Property_Id));
				AddCommonParams(cmd, thirdPartySettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					thirdPartySettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					thirdPartySettingObject.Id = (Int32)GetOutParameter(cmd, ThirdPartySettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(thirdPartySettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ThirdPartySetting
        /// </summary>
        /// <param name="thirdPartySettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ThirdPartySettingBase thirdPartySettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETHIRDPARTYSETTING);
				
				AddParameter(cmd, pInt32(ThirdPartySettingBase.Property_Id, thirdPartySettingObject.Id));
				AddCommonParams(cmd, thirdPartySettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					thirdPartySettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(thirdPartySettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ThirdPartySetting
        /// </summary>
        /// <param name="Id">Id of the ThirdPartySetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETHIRDPARTYSETTING);	
				
				AddParameter(cmd, pInt32(ThirdPartySettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ThirdPartySetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ThirdPartySetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ThirdPartySetting object to retrieve</param>
        /// <returns>ThirdPartySetting object, null if not found</returns>
		public ThirdPartySetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYSETTINGBYID))
			{
				AddParameter( cmd, pInt32(ThirdPartySettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ThirdPartySetting objects 
        /// </summary>
        /// <returns>A list of ThirdPartySetting objects</returns>
		public ThirdPartySettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTHIRDPARTYSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ThirdPartySetting objects by PageRequest
        /// </summary>
        /// <returns>A list of ThirdPartySetting objects</returns>
		public ThirdPartySettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTHIRDPARTYSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ThirdPartySettingList _ThirdPartySettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ThirdPartySettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all ThirdPartySetting objects by query String
        /// </summary>
        /// <returns>A list of ThirdPartySetting objects</returns>
		public ThirdPartySettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ThirdPartySetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ThirdPartySetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ThirdPartySetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ThirdPartySetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ThirdPartySettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTHIRDPARTYSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_ThirdPartySettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ThirdPartySettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ThirdPartySetting object
        /// </summary>
        /// <param name="thirdPartySettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ThirdPartySettingBase thirdPartySettingObject, SqlDataReader reader, int start)
		{
			
				thirdPartySettingObject.Id = reader.GetInt32( start + 0 );			
				thirdPartySettingObject.CompanyId = reader.GetGuid( start + 1 );			
				thirdPartySettingObject.Type = reader.GetString( start + 2 );			
				thirdPartySettingObject.Name = reader.GetString( start + 3 );			
				thirdPartySettingObject.Value = reader.GetString( start + 4 );			
				thirdPartySettingObject.IsActive = reader.GetBoolean( start + 5 );			
			FillBaseObject(thirdPartySettingObject, reader, (start + 6));

			
			thirdPartySettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ThirdPartySetting object
        /// </summary>
        /// <param name="thirdPartySettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ThirdPartySettingBase thirdPartySettingObject, SqlDataReader reader)
		{
			FillObject(thirdPartySettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ThirdPartySetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ThirdPartySetting object</returns>
		private ThirdPartySetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ThirdPartySetting thirdPartySettingObject= new ThirdPartySetting();
					FillObject(thirdPartySettingObject, reader);
					return thirdPartySettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ThirdPartySetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ThirdPartySetting objects</returns>
		private ThirdPartySettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ThirdPartySetting list
			ThirdPartySettingList list = new ThirdPartySettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ThirdPartySetting thirdPartySettingObject = new ThirdPartySetting();
					FillObject(thirdPartySettingObject, reader);

					list.Add(thirdPartySettingObject);
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
