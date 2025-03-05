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
	public partial class PtoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPTO = "InsertPto";
		private const string UPDATEPTO = "UpdatePto";
		private const string DELETEPTO = "DeletePto";
		private const string GETPTOBYID = "GetPtoById";
		private const string GETALLPTO = "GetAllPto";
		private const string GETPAGEDPTO = "GetPagedPto";
		private const string GETPTOMAXIMUMID = "GetPtoMaximumId";
		private const string GETPTOROWCOUNT = "GetPtoRowCount";	
		private const string GETPTOBYQUERY = "GetPtoByQuery";
		#endregion
		
		#region Constructors
		public PtoDataAccess(ClientContext context) : base(context) { }
		public PtoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ptoObject"></param>
		private void AddCommonParams(SqlCommand cmd, PtoBase ptoObject)
		{	
			AddParameter(cmd, pGuid(PtoBase.Property_UserId, ptoObject.UserId));
			AddParameter(cmd, pNVarChar(PtoBase.Property_Type, 50, ptoObject.Type));
			AddParameter(cmd, pDateTime(PtoBase.Property_StartDate, ptoObject.StartDate));
			AddParameter(cmd, pDateTime(PtoBase.Property_EndDate, ptoObject.EndDate));
			AddParameter(cmd, pNVarChar(PtoBase.Property_LeaveTime, 50, ptoObject.LeaveTime));
			AddParameter(cmd, pNVarChar(PtoBase.Property_TimeFrom, 50, ptoObject.TimeFrom));
			AddParameter(cmd, pNVarChar(PtoBase.Property_TimeTo, 50, ptoObject.TimeTo));
			AddParameter(cmd, pNVarChar(PtoBase.Property_Notes, ptoObject.Notes));
			AddParameter(cmd, pNVarChar(PtoBase.Property_Status, 50, ptoObject.Status));
			AddParameter(cmd, pGuid(PtoBase.Property_CreatedBy, ptoObject.CreatedBy));
			AddParameter(cmd, pDateTime(PtoBase.Property_CreatedDate, ptoObject.CreatedDate));
			AddParameter(cmd, pGuid(PtoBase.Property_LastUpdatedBy, ptoObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(PtoBase.Property_LastUpdatedDate, ptoObject.LastUpdatedDate));
			AddParameter(cmd, pBool(PtoBase.Property_Payable, ptoObject.Payable));
			AddParameter(cmd, pNVarChar(PtoBase.Property_RejectNote, ptoObject.RejectNote));
			AddParameter(cmd, pInt32(PtoBase.Property_Minute, ptoObject.Minute));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Pto
        /// </summary>
        /// <param name="ptoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PtoBase ptoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPTO);
	
				AddParameter(cmd, pInt32Out(PtoBase.Property_Id));
				AddCommonParams(cmd, ptoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ptoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ptoObject.Id = (Int32)GetOutParameter(cmd, PtoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ptoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Pto
        /// </summary>
        /// <param name="ptoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PtoBase ptoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPTO);
				
				AddParameter(cmd, pInt32(PtoBase.Property_Id, ptoObject.Id));
				AddCommonParams(cmd, ptoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ptoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ptoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Pto
        /// </summary>
        /// <param name="Id">Id of the Pto object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPTO);	
				
				AddParameter(cmd, pInt32(PtoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Pto), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Pto object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Pto object to retrieve</param>
        /// <returns>Pto object, null if not found</returns>
		public Pto Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPTOBYID))
			{
				AddParameter( cmd, pInt32(PtoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Pto objects 
        /// </summary>
        /// <returns>A list of Pto objects</returns>
		public PtoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPTO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Pto objects by PageRequest
        /// </summary>
        /// <returns>A list of Pto objects</returns>
		public PtoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPTO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PtoList _PtoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PtoList;
			}
		}
		
		/// <summary>
        /// Retrieves all Pto objects by query String
        /// </summary>
        /// <returns>A list of Pto objects</returns>
		public PtoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPTOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Pto Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Pto
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPTOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Pto Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Pto
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PtoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPTOROWCOUNT))
			{
				SqlDataReader reader;
				_PtoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PtoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Pto object
        /// </summary>
        /// <param name="ptoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PtoBase ptoObject, SqlDataReader reader, int start)
		{
			
				ptoObject.Id = reader.GetInt32( start + 0 );			
				ptoObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) ptoObject.Type = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) ptoObject.StartDate = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) ptoObject.EndDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) ptoObject.LeaveTime = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) ptoObject.TimeFrom = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) ptoObject.TimeTo = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) ptoObject.Notes = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) ptoObject.Status = reader.GetString( start + 9 );			
				ptoObject.CreatedBy = reader.GetGuid( start + 10 );			
				ptoObject.CreatedDate = reader.GetDateTime( start + 11 );			
				ptoObject.LastUpdatedBy = reader.GetGuid( start + 12 );			
				ptoObject.LastUpdatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) ptoObject.Payable = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) ptoObject.RejectNote = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) ptoObject.Minute = reader.GetInt32( start + 16 );			
			FillBaseObject(ptoObject, reader, (start + 17));

			
			ptoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Pto object
        /// </summary>
        /// <param name="ptoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PtoBase ptoObject, SqlDataReader reader)
		{
			FillObject(ptoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Pto object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Pto object</returns>
		private Pto GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Pto ptoObject= new Pto();
					FillObject(ptoObject, reader);
					return ptoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Pto objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Pto objects</returns>
		private PtoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Pto list
			PtoList list = new PtoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows != 0 )
				{
					Pto ptoObject = new Pto();
					FillObject(ptoObject, reader);

					list.Add(ptoObject);
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
