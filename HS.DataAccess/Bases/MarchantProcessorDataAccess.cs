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
	public partial class MarchantProcessorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMARCHANTPROCESSOR = "InsertMarchantProcessor";
		private const string UPDATEMARCHANTPROCESSOR = "UpdateMarchantProcessor";
		private const string DELETEMARCHANTPROCESSOR = "DeleteMarchantProcessor";
		private const string GETMARCHANTPROCESSORBYID = "GetMarchantProcessorById";
		private const string GETALLMARCHANTPROCESSOR = "GetAllMarchantProcessor";
		private const string GETPAGEDMARCHANTPROCESSOR = "GetPagedMarchantProcessor";
		private const string GETMARCHANTPROCESSORMAXIMUMID = "GetMarchantProcessorMaximumId";
		private const string GETMARCHANTPROCESSORROWCOUNT = "GetMarchantProcessorRowCount";	
		private const string GETMARCHANTPROCESSORBYQUERY = "GetMarchantProcessorByQuery";
		#endregion
		
		#region Constructors
		public MarchantProcessorDataAccess(ClientContext context) : base(context) { }
		public MarchantProcessorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="marchantProcessorObject"></param>
		private void AddCommonParams(SqlCommand cmd, MarchantProcessorBase marchantProcessorObject)
		{	
			AddParameter(cmd, pGuid(MarchantProcessorBase.Property_CompanyId, marchantProcessorObject.CompanyId));
			AddParameter(cmd, pInt32(MarchantProcessorBase.Property_MarchantId, marchantProcessorObject.MarchantId));
			AddParameter(cmd, pInt32(MarchantProcessorBase.Property_ProcessorId, marchantProcessorObject.ProcessorId));
			AddParameter(cmd, pNVarChar(MarchantProcessorBase.Property_Method, 50, marchantProcessorObject.Method));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MarchantProcessor
        /// </summary>
        /// <param name="marchantProcessorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MarchantProcessorBase marchantProcessorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMARCHANTPROCESSOR);
	
				AddParameter(cmd, pInt32Out(MarchantProcessorBase.Property_Id));
				AddCommonParams(cmd, marchantProcessorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					marchantProcessorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					marchantProcessorObject.Id = (Int32)GetOutParameter(cmd, MarchantProcessorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(marchantProcessorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MarchantProcessor
        /// </summary>
        /// <param name="marchantProcessorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MarchantProcessorBase marchantProcessorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMARCHANTPROCESSOR);
				
				AddParameter(cmd, pInt32(MarchantProcessorBase.Property_Id, marchantProcessorObject.Id));
				AddCommonParams(cmd, marchantProcessorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					marchantProcessorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(marchantProcessorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MarchantProcessor
        /// </summary>
        /// <param name="Id">Id of the MarchantProcessor object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMARCHANTPROCESSOR);	
				
				AddParameter(cmd, pInt32(MarchantProcessorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MarchantProcessor), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MarchantProcessor object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MarchantProcessor object to retrieve</param>
        /// <returns>MarchantProcessor object, null if not found</returns>
		public MarchantProcessor Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTPROCESSORBYID))
			{
				AddParameter( cmd, pInt32(MarchantProcessorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MarchantProcessor objects 
        /// </summary>
        /// <returns>A list of MarchantProcessor objects</returns>
		public MarchantProcessorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMARCHANTPROCESSOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MarchantProcessor objects by PageRequest
        /// </summary>
        /// <returns>A list of MarchantProcessor objects</returns>
		public MarchantProcessorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMARCHANTPROCESSOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MarchantProcessorList _MarchantProcessorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MarchantProcessorList;
			}
		}
		
		/// <summary>
        /// Retrieves all MarchantProcessor objects by query String
        /// </summary>
        /// <returns>A list of MarchantProcessor objects</returns>
		public MarchantProcessorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTPROCESSORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MarchantProcessor Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MarchantProcessor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTPROCESSORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MarchantProcessor Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MarchantProcessor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MarchantProcessorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMARCHANTPROCESSORROWCOUNT))
			{
				SqlDataReader reader;
				_MarchantProcessorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MarchantProcessorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MarchantProcessor object
        /// </summary>
        /// <param name="marchantProcessorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MarchantProcessorBase marchantProcessorObject, SqlDataReader reader, int start)
		{
			
				marchantProcessorObject.Id = reader.GetInt32( start + 0 );			
				marchantProcessorObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) marchantProcessorObject.MarchantId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) marchantProcessorObject.ProcessorId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) marchantProcessorObject.Method = reader.GetString( start + 4 );			
			FillBaseObject(marchantProcessorObject, reader, (start + 5));

			
			marchantProcessorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MarchantProcessor object
        /// </summary>
        /// <param name="marchantProcessorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MarchantProcessorBase marchantProcessorObject, SqlDataReader reader)
		{
			FillObject(marchantProcessorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MarchantProcessor object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MarchantProcessor object</returns>
		private MarchantProcessor GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MarchantProcessor marchantProcessorObject= new MarchantProcessor();
					FillObject(marchantProcessorObject, reader);
					return marchantProcessorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MarchantProcessor objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MarchantProcessor objects</returns>
		private MarchantProcessorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MarchantProcessor list
			MarchantProcessorList list = new MarchantProcessorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MarchantProcessor marchantProcessorObject = new MarchantProcessor();
					FillObject(marchantProcessorObject, reader);

					list.Add(marchantProcessorObject);
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
