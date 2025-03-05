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
	public partial class MMRRangeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMMRRANGE = "InsertMMRRange";
		private const string UPDATEMMRRANGE = "UpdateMMRRange";
		private const string DELETEMMRRANGE = "DeleteMMRRange";
		private const string GETMMRRANGEBYID = "GetMMRRangeById";
		private const string GETALLMMRRANGE = "GetAllMMRRange";
		private const string GETPAGEDMMRRANGE = "GetPagedMMRRange";
		private const string GETMMRRANGEMAXIMUMID = "GetMMRRangeMaximumId";
		private const string GETMMRRANGEROWCOUNT = "GetMMRRangeRowCount";	
		private const string GETMMRRANGEBYQUERY = "GetMMRRangeByQuery";
		#endregion
		
		#region Constructors
		public MMRRangeDataAccess(ClientContext context) : base(context) { }
		public MMRRangeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="mMRRangeObject"></param>
		private void AddCommonParams(SqlCommand cmd, MMRRangeBase mMRRangeObject)
		{	
			AddParameter(cmd, pGuid(MMRRangeBase.Property_CompanyId, mMRRangeObject.CompanyId));
			AddParameter(cmd, pGuid(MMRRangeBase.Property_PackageId, mMRRangeObject.PackageId));
			AddParameter(cmd, pDouble(MMRRangeBase.Property_MinMMR, mMRRangeObject.MinMMR));
			AddParameter(cmd, pDouble(MMRRangeBase.Property_MaxMMR, mMRRangeObject.MaxMMR));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MMRRange
        /// </summary>
        /// <param name="mMRRangeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MMRRangeBase mMRRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMMRRANGE);
	
				AddParameter(cmd, pInt32Out(MMRRangeBase.Property_Id));
				AddCommonParams(cmd, mMRRangeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					mMRRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					mMRRangeObject.Id = (Int32)GetOutParameter(cmd, MMRRangeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(mMRRangeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MMRRange
        /// </summary>
        /// <param name="mMRRangeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MMRRangeBase mMRRangeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMMRRANGE);
				
				AddParameter(cmd, pInt32(MMRRangeBase.Property_Id, mMRRangeObject.Id));
				AddCommonParams(cmd, mMRRangeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					mMRRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(mMRRangeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MMRRange
        /// </summary>
        /// <param name="Id">Id of the MMRRange object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMMRRANGE);	
				
				AddParameter(cmd, pInt32(MMRRangeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MMRRange), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MMRRange object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MMRRange object to retrieve</param>
        /// <returns>MMRRange object, null if not found</returns>
		public MMRRange Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMMRRANGEBYID))
			{
				AddParameter( cmd, pInt32(MMRRangeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MMRRange objects 
        /// </summary>
        /// <returns>A list of MMRRange objects</returns>
		public MMRRangeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMMRRANGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MMRRange objects by PageRequest
        /// </summary>
        /// <returns>A list of MMRRange objects</returns>
		public MMRRangeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMMRRANGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MMRRangeList _MMRRangeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MMRRangeList;
			}
		}
		
		/// <summary>
        /// Retrieves all MMRRange objects by query String
        /// </summary>
        /// <returns>A list of MMRRange objects</returns>
		public MMRRangeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMMRRANGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MMRRange Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MMRRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMMRRANGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MMRRange Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MMRRange
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MMRRangeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMMRRANGEROWCOUNT))
			{
				SqlDataReader reader;
				_MMRRangeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MMRRangeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MMRRange object
        /// </summary>
        /// <param name="mMRRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MMRRangeBase mMRRangeObject, SqlDataReader reader, int start)
		{
			
				mMRRangeObject.Id = reader.GetInt32( start + 0 );			
				mMRRangeObject.CompanyId = reader.GetGuid( start + 1 );			
				mMRRangeObject.PackageId = reader.GetGuid( start + 2 );			
				mMRRangeObject.MinMMR = reader.GetDouble( start + 3 );			
				mMRRangeObject.MaxMMR = reader.GetDouble( start + 4 );			
			FillBaseObject(mMRRangeObject, reader, (start + 5));

			
			mMRRangeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MMRRange object
        /// </summary>
        /// <param name="mMRRangeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MMRRangeBase mMRRangeObject, SqlDataReader reader)
		{
			FillObject(mMRRangeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MMRRange object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MMRRange object</returns>
		private MMRRange GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MMRRange mMRRangeObject= new MMRRange();
					FillObject(mMRRangeObject, reader);
					return mMRRangeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MMRRange objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MMRRange objects</returns>
		private MMRRangeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MMRRange list
			MMRRangeList list = new MMRRangeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MMRRange mMRRangeObject = new MMRRange();
					FillObject(mMRRangeObject, reader);

					list.Add(mMRRangeObject);
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
