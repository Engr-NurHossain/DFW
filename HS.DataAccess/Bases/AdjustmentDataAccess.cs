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
	public partial class AdjustmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADJUSTMENT = "InsertAdjustment";
		private const string UPDATEADJUSTMENT = "UpdateAdjustment";
		private const string DELETEADJUSTMENT = "DeleteAdjustment";
		private const string GETADJUSTMENTBYID = "GetAdjustmentById";
		private const string GETALLADJUSTMENT = "GetAllAdjustment";
		private const string GETPAGEDADJUSTMENT = "GetPagedAdjustment";
		private const string GETADJUSTMENTMAXIMUMID = "GetAdjustmentMaximumId";
		private const string GETADJUSTMENTROWCOUNT = "GetAdjustmentRowCount";	
		private const string GETADJUSTMENTBYQUERY = "GetAdjustmentByQuery";
		#endregion
		
		#region Constructors
		public AdjustmentDataAccess(ClientContext context) : base(context) { }
		public AdjustmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="adjustmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, AdjustmentBase adjustmentObject)
		{	
			AddParameter(cmd, pInt32(AdjustmentBase.Property_ComissionSessionId, adjustmentObject.ComissionSessionId));
			AddParameter(cmd, pInt32(AdjustmentBase.Property_AdjustSchemeId, adjustmentObject.AdjustSchemeId));
			AddParameter(cmd, pNVarChar(AdjustmentBase.Property_Description, adjustmentObject.Description));
			AddParameter(cmd, pNVarChar(AdjustmentBase.Property_Conduit, 50, adjustmentObject.Conduit));
			AddParameter(cmd, pDouble(AdjustmentBase.Property_Amount, adjustmentObject.Amount));
			AddParameter(cmd, pDouble(AdjustmentBase.Property_Multiple, adjustmentObject.Multiple));
			AddParameter(cmd, pNVarChar(AdjustmentBase.Property_AppliedTo, 50, adjustmentObject.AppliedTo));
			AddParameter(cmd, pDateTime(AdjustmentBase.Property_StartDate, adjustmentObject.StartDate));
			AddParameter(cmd, pDateTime(AdjustmentBase.Property_EndDate, adjustmentObject.EndDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Adjustment
        /// </summary>
        /// <param name="adjustmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AdjustmentBase adjustmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADJUSTMENT);
	
				AddParameter(cmd, pInt32Out(AdjustmentBase.Property_Id));
				AddCommonParams(cmd, adjustmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					adjustmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					adjustmentObject.Id = (Int32)GetOutParameter(cmd, AdjustmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(adjustmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Adjustment
        /// </summary>
        /// <param name="adjustmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AdjustmentBase adjustmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADJUSTMENT);
				
				AddParameter(cmd, pInt32(AdjustmentBase.Property_Id, adjustmentObject.Id));
				AddCommonParams(cmd, adjustmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					adjustmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(adjustmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Adjustment
        /// </summary>
        /// <param name="Id">Id of the Adjustment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADJUSTMENT);	
				
				AddParameter(cmd, pInt32(AdjustmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Adjustment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Adjustment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Adjustment object to retrieve</param>
        /// <returns>Adjustment object, null if not found</returns>
		public Adjustment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTBYID))
			{
				AddParameter( cmd, pInt32(AdjustmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Adjustment objects 
        /// </summary>
        /// <returns>A list of Adjustment objects</returns>
		public AdjustmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADJUSTMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Adjustment objects by PageRequest
        /// </summary>
        /// <returns>A list of Adjustment objects</returns>
		public AdjustmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADJUSTMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AdjustmentList _AdjustmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AdjustmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all Adjustment objects by query String
        /// </summary>
        /// <returns>A list of Adjustment objects</returns>
		public AdjustmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Adjustment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Adjustment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Adjustment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Adjustment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AdjustmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTROWCOUNT))
			{
				SqlDataReader reader;
				_AdjustmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AdjustmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Adjustment object
        /// </summary>
        /// <param name="adjustmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AdjustmentBase adjustmentObject, SqlDataReader reader, int start)
		{
			
				adjustmentObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) adjustmentObject.ComissionSessionId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) adjustmentObject.AdjustSchemeId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) adjustmentObject.Description = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) adjustmentObject.Conduit = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) adjustmentObject.Amount = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) adjustmentObject.Multiple = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) adjustmentObject.AppliedTo = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) adjustmentObject.StartDate = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) adjustmentObject.EndDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(adjustmentObject, reader, (start + 10));

			
			adjustmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Adjustment object
        /// </summary>
        /// <param name="adjustmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AdjustmentBase adjustmentObject, SqlDataReader reader)
		{
			FillObject(adjustmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Adjustment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Adjustment object</returns>
		private Adjustment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Adjustment adjustmentObject= new Adjustment();
					FillObject(adjustmentObject, reader);
					return adjustmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Adjustment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Adjustment objects</returns>
		private AdjustmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Adjustment list
			AdjustmentList list = new AdjustmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Adjustment adjustmentObject = new Adjustment();
					FillObject(adjustmentObject, reader);

					list.Add(adjustmentObject);
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
