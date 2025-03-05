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
	public partial class AdjustmentRuleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADJUSTMENTRULE = "InsertAdjustmentRule";
		private const string UPDATEADJUSTMENTRULE = "UpdateAdjustmentRule";
		private const string DELETEADJUSTMENTRULE = "DeleteAdjustmentRule";
		private const string GETADJUSTMENTRULEBYID = "GetAdjustmentRuleById";
		private const string GETALLADJUSTMENTRULE = "GetAllAdjustmentRule";
		private const string GETPAGEDADJUSTMENTRULE = "GetPagedAdjustmentRule";
		private const string GETADJUSTMENTRULEMAXIMUMID = "GetAdjustmentRuleMaximumId";
		private const string GETADJUSTMENTRULEROWCOUNT = "GetAdjustmentRuleRowCount";	
		private const string GETADJUSTMENTRULEBYQUERY = "GetAdjustmentRuleByQuery";
		#endregion
		
		#region Constructors
		public AdjustmentRuleDataAccess(ClientContext context) : base(context) { }
		public AdjustmentRuleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="adjustmentRuleObject"></param>
		private void AddCommonParams(SqlCommand cmd, AdjustmentRuleBase adjustmentRuleObject)
		{	
			AddParameter(cmd, pInt32(AdjustmentRuleBase.Property_ComissionSessionId, adjustmentRuleObject.ComissionSessionId));
			AddParameter(cmd, pInt32(AdjustmentRuleBase.Property_AdjustSchemeId, adjustmentRuleObject.AdjustSchemeId));
			AddParameter(cmd, pNVarChar(AdjustmentRuleBase.Property_TableName, 50, adjustmentRuleObject.TableName));
			AddParameter(cmd, pNVarChar(AdjustmentRuleBase.Property_ColumnName, 50, adjustmentRuleObject.ColumnName));
			AddParameter(cmd, pNVarChar(AdjustmentRuleBase.Property_ColumnValue, 50, adjustmentRuleObject.ColumnValue));
			AddParameter(cmd, pNVarChar(AdjustmentRuleBase.Property_DataType, 50, adjustmentRuleObject.DataType));
			AddParameter(cmd, pNVarChar(AdjustmentRuleBase.Property_CommandType, 50, adjustmentRuleObject.CommandType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AdjustmentRule
        /// </summary>
        /// <param name="adjustmentRuleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AdjustmentRuleBase adjustmentRuleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADJUSTMENTRULE);
	
				AddParameter(cmd, pInt32Out(AdjustmentRuleBase.Property_Id));
				AddCommonParams(cmd, adjustmentRuleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					adjustmentRuleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					adjustmentRuleObject.Id = (Int32)GetOutParameter(cmd, AdjustmentRuleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(adjustmentRuleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AdjustmentRule
        /// </summary>
        /// <param name="adjustmentRuleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AdjustmentRuleBase adjustmentRuleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADJUSTMENTRULE);
				
				AddParameter(cmd, pInt32(AdjustmentRuleBase.Property_Id, adjustmentRuleObject.Id));
				AddCommonParams(cmd, adjustmentRuleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					adjustmentRuleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(adjustmentRuleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AdjustmentRule
        /// </summary>
        /// <param name="Id">Id of the AdjustmentRule object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADJUSTMENTRULE);	
				
				AddParameter(cmd, pInt32(AdjustmentRuleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AdjustmentRule), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AdjustmentRule object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AdjustmentRule object to retrieve</param>
        /// <returns>AdjustmentRule object, null if not found</returns>
		public AdjustmentRule Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTRULEBYID))
			{
				AddParameter( cmd, pInt32(AdjustmentRuleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AdjustmentRule objects 
        /// </summary>
        /// <returns>A list of AdjustmentRule objects</returns>
		public AdjustmentRuleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADJUSTMENTRULE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AdjustmentRule objects by PageRequest
        /// </summary>
        /// <returns>A list of AdjustmentRule objects</returns>
		public AdjustmentRuleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADJUSTMENTRULE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AdjustmentRuleList _AdjustmentRuleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AdjustmentRuleList;
			}
		}
		
		/// <summary>
        /// Retrieves all AdjustmentRule objects by query String
        /// </summary>
        /// <returns>A list of AdjustmentRule objects</returns>
		public AdjustmentRuleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTRULEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AdjustmentRule Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AdjustmentRule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTRULEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AdjustmentRule Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AdjustmentRule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AdjustmentRuleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTRULEROWCOUNT))
			{
				SqlDataReader reader;
				_AdjustmentRuleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AdjustmentRuleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AdjustmentRule object
        /// </summary>
        /// <param name="adjustmentRuleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AdjustmentRuleBase adjustmentRuleObject, SqlDataReader reader, int start)
		{
			
				adjustmentRuleObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) adjustmentRuleObject.ComissionSessionId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) adjustmentRuleObject.AdjustSchemeId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) adjustmentRuleObject.TableName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) adjustmentRuleObject.ColumnName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) adjustmentRuleObject.ColumnValue = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) adjustmentRuleObject.DataType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) adjustmentRuleObject.CommandType = reader.GetString( start + 7 );			
			FillBaseObject(adjustmentRuleObject, reader, (start + 8));

			
			adjustmentRuleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AdjustmentRule object
        /// </summary>
        /// <param name="adjustmentRuleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AdjustmentRuleBase adjustmentRuleObject, SqlDataReader reader)
		{
			FillObject(adjustmentRuleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AdjustmentRule object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AdjustmentRule object</returns>
		private AdjustmentRule GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AdjustmentRule adjustmentRuleObject= new AdjustmentRule();
					FillObject(adjustmentRuleObject, reader);
					return adjustmentRuleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AdjustmentRule objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AdjustmentRule objects</returns>
		private AdjustmentRuleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AdjustmentRule list
			AdjustmentRuleList list = new AdjustmentRuleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AdjustmentRule adjustmentRuleObject = new AdjustmentRule();
					FillObject(adjustmentRuleObject, reader);

					list.Add(adjustmentRuleObject);
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
