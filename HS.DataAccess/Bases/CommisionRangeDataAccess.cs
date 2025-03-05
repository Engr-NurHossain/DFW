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
	public partial class CommisionRangeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMMISIONRANGE = "InsertCommisionRange";
		private const string UPDATECOMMISIONRANGE = "UpdateCommisionRange";
		private const string DELETECOMMISIONRANGE = "DeleteCommisionRange";
		private const string GETCOMMISIONRANGEBYID = "GetCommisionRangeById";
		private const string GETALLCOMMISIONRANGE = "GetAllCommisionRange";
		private const string GETPAGEDCOMMISIONRANGE = "GetPagedCommisionRange";
		private const string GETCOMMISIONRANGEMAXIMUMID = "GetCommisionRangeMaximumId";
		private const string GETCOMMISIONRANGEROWCOUNT = "GetCommisionRangeRowCount";	
		private const string GETCOMMISIONRANGEBYQUERY = "GetCommisionRangeByQuery";
		#endregion
		
		#region Constructors
		public CommisionRangeDataAccess(ClientContext context) : base(context) { }
		public CommisionRangeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="commisionRangeObject"></param>
		private void AddCommonParams(SqlCommand cmd, CommisionRangeBase commisionRangeObject)
		{	
			AddParameter(cmd, pInt32(CommisionRangeBase.Property_CommisionTypeId, commisionRangeObject.CommisionTypeId));
			AddParameter(cmd, pInt32(CommisionRangeBase.Property_CommisionSessionId, commisionRangeObject.CommisionSessionId));
			AddParameter(cmd, pInt32(CommisionRangeBase.Property_RangeStart, commisionRangeObject.RangeStart));
			AddParameter(cmd, pInt32(CommisionRangeBase.Property_RangeEnd, commisionRangeObject.RangeEnd));
			AddParameter(cmd, pDouble(CommisionRangeBase.Property_Upfront, commisionRangeObject.Upfront));
			AddParameter(cmd, pDouble(CommisionRangeBase.Property_Backend, commisionRangeObject.Backend));
			AddParameter(cmd, pDouble(CommisionRangeBase.Property_Bonus, commisionRangeObject.Bonus));
			AddParameter(cmd, pDouble(CommisionRangeBase.Property_RentBonus, commisionRangeObject.RentBonus));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CommisionRange
        /// </summary>
        /// <param name="commisionRangeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CommisionRangeBase commisionRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMMISIONRANGE);
	
				AddParameter(cmd, pInt32Out(CommisionRangeBase.Property_Id));
				AddCommonParams(cmd, commisionRangeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					commisionRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					commisionRangeObject.Id = (Int32)GetOutParameter(cmd, CommisionRangeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(commisionRangeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CommisionRange
        /// </summary>
        /// <param name="commisionRangeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CommisionRangeBase commisionRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMMISIONRANGE);
				
				AddParameter(cmd, pInt32(CommisionRangeBase.Property_Id, commisionRangeObject.Id));
				AddCommonParams(cmd, commisionRangeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					commisionRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(commisionRangeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CommisionRange
        /// </summary>
        /// <param name="Id">Id of the CommisionRange object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMMISIONRANGE);	
				
				AddParameter(cmd, pInt32(CommisionRangeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CommisionRange), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CommisionRange object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CommisionRange object to retrieve</param>
        /// <returns>CommisionRange object, null if not found</returns>
		public CommisionRange Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONRANGEBYID))
			{
				AddParameter( cmd, pInt32(CommisionRangeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CommisionRange objects 
        /// </summary>
        /// <returns>A list of CommisionRange objects</returns>
		public CommisionRangeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMMISIONRANGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CommisionRange objects by PageRequest
        /// </summary>
        /// <returns>A list of CommisionRange objects</returns>
		public CommisionRangeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMMISIONRANGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CommisionRangeList _CommisionRangeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CommisionRangeList;
			}
		}
		
		/// <summary>
        /// Retrieves all CommisionRange objects by query String
        /// </summary>
        /// <returns>A list of CommisionRange objects</returns>
		public CommisionRangeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONRANGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CommisionRange Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CommisionRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONRANGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CommisionRange Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CommisionRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CommisionRangeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONRANGEROWCOUNT))
			{
				SqlDataReader reader;
				_CommisionRangeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CommisionRangeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CommisionRange object
        /// </summary>
        /// <param name="commisionRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CommisionRangeBase commisionRangeObject, SqlDataReader reader, int start)
		{
			
				commisionRangeObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) commisionRangeObject.CommisionTypeId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) commisionRangeObject.CommisionSessionId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) commisionRangeObject.RangeStart = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) commisionRangeObject.RangeEnd = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) commisionRangeObject.Upfront = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) commisionRangeObject.Backend = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) commisionRangeObject.Bonus = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) commisionRangeObject.RentBonus = reader.GetDouble( start + 8 );			
			FillBaseObject(commisionRangeObject, reader, (start + 9));

			
			commisionRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CommisionRange object
        /// </summary>
        /// <param name="commisionRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CommisionRangeBase commisionRangeObject, SqlDataReader reader)
		{
			FillObject(commisionRangeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CommisionRange object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CommisionRange object</returns>
		private CommisionRange GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CommisionRange commisionRangeObject= new CommisionRange();
					FillObject(commisionRangeObject, reader);
					return commisionRangeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CommisionRange objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CommisionRange objects</returns>
		private CommisionRangeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CommisionRange list
			CommisionRangeList list = new CommisionRangeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CommisionRange commisionRangeObject = new CommisionRange();
					FillObject(commisionRangeObject, reader);

					list.Add(commisionRangeObject);
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
