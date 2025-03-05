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
	public partial class GridSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTGRIDSETTING = "InsertGridSetting";
		private const string UPDATEGRIDSETTING = "UpdateGridSetting";
		private const string DELETEGRIDSETTING = "DeleteGridSetting";
		private const string GETGRIDSETTINGBYID = "GetGridSettingById";
		private const string GETALLGRIDSETTING = "GetAllGridSetting";
		private const string GETPAGEDGRIDSETTING = "GetPagedGridSetting";
		private const string GETGRIDSETTINGMAXIMUMID = "GetGridSettingMaximumId";
		private const string GETGRIDSETTINGROWCOUNT = "GetGridSettingRowCount";	
		private const string GETGRIDSETTINGBYQUERY = "GetGridSettingByQuery";
		#endregion
		
		#region Constructors
		public GridSettingDataAccess(ClientContext context) : base(context) { }
		public GridSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="gridSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, GridSettingBase gridSettingObject)
		{	
			AddParameter(cmd, pGuid(GridSettingBase.Property_CompanyId, gridSettingObject.CompanyId));
			AddParameter(cmd, pNVarChar(GridSettingBase.Property_ListKeyName, 50, gridSettingObject.ListKeyName));
			AddParameter(cmd, pNVarChar(GridSettingBase.Property_SelectedColumn, gridSettingObject.SelectedColumn));
			AddParameter(cmd, pNVarChar(GridSettingBase.Property_ColumnGroup, 250, gridSettingObject.ColumnGroup));
			AddParameter(cmd, pInt32(GridSettingBase.Property_GroupOrder, gridSettingObject.GroupOrder));
			AddParameter(cmd, pInt32(GridSettingBase.Property_OrderBy, gridSettingObject.OrderBy));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsActive, gridSettingObject.IsActive));
			AddParameter(cmd, pBool(GridSettingBase.Property_GridActive, gridSettingObject.GridActive));
			AddParameter(cmd, pBool(GridSettingBase.Property_FormActive, gridSettingObject.FormActive));
			AddParameter(cmd, pNVarChar(GridSettingBase.Property_InputType, 50, gridSettingObject.InputType));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsFilter, gridSettingObject.IsFilter));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsCustomerRequired, gridSettingObject.IsCustomerRequired));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsLeadRequired, gridSettingObject.IsLeadRequired));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsCustomerLabel, gridSettingObject.IsCustomerLabel));
			AddParameter(cmd, pBool(GridSettingBase.Property_IsLeadLabel, gridSettingObject.IsLeadLabel));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts GridSetting
        /// </summary>
        /// <param name="gridSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(GridSettingBase gridSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTGRIDSETTING);
	
				AddParameter(cmd, pInt32Out(GridSettingBase.Property_Id));
				AddCommonParams(cmd, gridSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					gridSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					gridSettingObject.Id = (Int32)GetOutParameter(cmd, GridSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(gridSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates GridSetting
        /// </summary>
        /// <param name="gridSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(GridSettingBase gridSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEGRIDSETTING);
				
				AddParameter(cmd, pInt32(GridSettingBase.Property_Id, gridSettingObject.Id));
				AddCommonParams(cmd, gridSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					gridSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(gridSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes GridSetting
        /// </summary>
        /// <param name="Id">Id of the GridSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEGRIDSETTING);	
				
				AddParameter(cmd, pInt32(GridSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(GridSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves GridSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the GridSetting object to retrieve</param>
        /// <returns>GridSetting object, null if not found</returns>
		public GridSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETGRIDSETTINGBYID))
			{
				AddParameter( cmd, pInt32(GridSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all GridSetting objects 
        /// </summary>
        /// <returns>A list of GridSetting objects</returns>
		public GridSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLGRIDSETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all GridSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of GridSetting objects</returns>
		public GridSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDGRIDSETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				GridSettingList _GridSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _GridSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all GridSetting objects by query String
        /// </summary>
        /// <returns>A list of GridSetting objects</returns>
		public GridSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETGRIDSETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get GridSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of GridSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETGRIDSETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get GridSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of GridSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _GridSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETGRIDSETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_GridSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _GridSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills GridSetting object
        /// </summary>
        /// <param name="gridSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(GridSettingBase gridSettingObject, SqlDataReader reader, int start)
		{
			
				gridSettingObject.Id = reader.GetInt32( start + 0 );			
				gridSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				gridSettingObject.ListKeyName = reader.GetString( start + 2 );			
				gridSettingObject.SelectedColumn = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) gridSettingObject.ColumnGroup = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) gridSettingObject.GroupOrder = reader.GetInt32( start + 5 );			
				gridSettingObject.OrderBy = reader.GetInt32( start + 6 );			
				gridSettingObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) gridSettingObject.GridActive = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) gridSettingObject.FormActive = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) gridSettingObject.InputType = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) gridSettingObject.IsFilter = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) gridSettingObject.IsCustomerRequired = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) gridSettingObject.IsLeadRequired = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) gridSettingObject.IsCustomerLabel = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) gridSettingObject.IsLeadLabel = reader.GetBoolean( start + 15 );			
			FillBaseObject(gridSettingObject, reader, (start + 16));

			
			gridSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills GridSetting object
        /// </summary>
        /// <param name="gridSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(GridSettingBase gridSettingObject, SqlDataReader reader)
		{
			FillObject(gridSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves GridSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>GridSetting object</returns>
		private GridSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					GridSetting gridSettingObject= new GridSetting();
					FillObject(gridSettingObject, reader);
					return gridSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of GridSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of GridSetting objects</returns>
		private GridSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//GridSetting list
			GridSettingList list = new GridSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					GridSetting gridSettingObject = new GridSetting();
					FillObject(gridSettingObject, reader);

					list.Add(gridSettingObject);
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
