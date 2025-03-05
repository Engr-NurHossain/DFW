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
	public partial class SalesMatrixDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSALESMATRIX = "InsertSalesMatrix";
		private const string UPDATESALESMATRIX = "UpdateSalesMatrix";
		private const string DELETESALESMATRIX = "DeleteSalesMatrix";
		private const string GETSALESMATRIXBYID = "GetSalesMatrixById";
		private const string GETALLSALESMATRIX = "GetAllSalesMatrix";
		private const string GETPAGEDSALESMATRIX = "GetPagedSalesMatrix";
		private const string GETSALESMATRIXMAXIMUMID = "GetSalesMatrixMaximumId";
		private const string GETSALESMATRIXROWCOUNT = "GetSalesMatrixRowCount";	
		private const string GETSALESMATRIXBYQUERY = "GetSalesMatrixByQuery";
		#endregion
		
		#region Constructors
		public SalesMatrixDataAccess(ClientContext context) : base(context) { }
		public SalesMatrixDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="salesMatrixObject"></param>
		private void AddCommonParams(SqlCommand cmd, SalesMatrixBase salesMatrixObject)
		{	
			AddParameter(cmd, pGuid(SalesMatrixBase.Property_SalesMatrixId, salesMatrixObject.SalesMatrixId));
			AddParameter(cmd, pNVarChar(SalesMatrixBase.Property_Type, 500, salesMatrixObject.Type));
			AddParameter(cmd, pDouble(SalesMatrixBase.Property_Min, salesMatrixObject.Min));
			AddParameter(cmd, pDouble(SalesMatrixBase.Property_Max, salesMatrixObject.Max));
			AddParameter(cmd, pDouble(SalesMatrixBase.Property_UserX, salesMatrixObject.UserX));
			AddParameter(cmd, pDouble(SalesMatrixBase.Property_Difference, salesMatrixObject.Difference));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts SalesMatrix
        /// </summary>
        /// <param name="salesMatrixObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SalesMatrixBase salesMatrixObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSALESMATRIX);
	
				AddParameter(cmd, pInt32Out(SalesMatrixBase.Property_Id));
				AddCommonParams(cmd, salesMatrixObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					salesMatrixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					salesMatrixObject.Id = (Int32)GetOutParameter(cmd, SalesMatrixBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(salesMatrixObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates SalesMatrix
        /// </summary>
        /// <param name="salesMatrixObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SalesMatrixBase salesMatrixObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESALESMATRIX);
				
				AddParameter(cmd, pInt32(SalesMatrixBase.Property_Id, salesMatrixObject.Id));
				AddCommonParams(cmd, salesMatrixObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					salesMatrixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(salesMatrixObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes SalesMatrix
        /// </summary>
        /// <param name="Id">Id of the SalesMatrix object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESALESMATRIX);	
				
				AddParameter(cmd, pInt32(SalesMatrixBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(SalesMatrix), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves SalesMatrix object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the SalesMatrix object to retrieve</param>
        /// <returns>SalesMatrix object, null if not found</returns>
		public SalesMatrix Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESMATRIXBYID))
			{
				AddParameter( cmd, pInt32(SalesMatrixBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all SalesMatrix objects 
        /// </summary>
        /// <returns>A list of SalesMatrix objects</returns>
		public SalesMatrixList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSALESMATRIX))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all SalesMatrix objects by PageRequest
        /// </summary>
        /// <returns>A list of SalesMatrix objects</returns>
		public SalesMatrixList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSALESMATRIX))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SalesMatrixList _SalesMatrixList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SalesMatrixList;
			}
		}
		
		/// <summary>
        /// Retrieves all SalesMatrix objects by query String
        /// </summary>
        /// <returns>A list of SalesMatrix objects</returns>
		public SalesMatrixList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSALESMATRIXBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get SalesMatrix Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of SalesMatrix
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESMATRIXMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get SalesMatrix Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of SalesMatrix
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SalesMatrixRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSALESMATRIXROWCOUNT))
			{
				SqlDataReader reader;
				_SalesMatrixRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SalesMatrixRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills SalesMatrix object
        /// </summary>
        /// <param name="salesMatrixObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SalesMatrixBase salesMatrixObject, SqlDataReader reader, int start)
		{
			
				salesMatrixObject.Id = reader.GetInt32( start + 0 );			
				salesMatrixObject.SalesMatrixId = reader.GetGuid( start + 1 );			
				salesMatrixObject.Type = reader.GetString( start + 2 );			
				salesMatrixObject.Min = reader.GetDouble( start + 3 );			
				salesMatrixObject.Max = reader.GetDouble( start + 4 );			
				salesMatrixObject.UserX = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) salesMatrixObject.Difference = reader.GetDouble( start + 6 );			
			FillBaseObject(salesMatrixObject, reader, (start + 7));

			
			salesMatrixObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills SalesMatrix object
        /// </summary>
        /// <param name="salesMatrixObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SalesMatrixBase salesMatrixObject, SqlDataReader reader)
		{
			FillObject(salesMatrixObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves SalesMatrix object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>SalesMatrix object</returns>
		private SalesMatrix GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					SalesMatrix salesMatrixObject= new SalesMatrix();
					FillObject(salesMatrixObject, reader);
					return salesMatrixObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of SalesMatrix objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of SalesMatrix objects</returns>
		private SalesMatrixList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//SalesMatrix list
			SalesMatrixList list = new SalesMatrixList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					SalesMatrix salesMatrixObject = new SalesMatrix();
					FillObject(salesMatrixObject, reader);

					list.Add(salesMatrixObject);
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
