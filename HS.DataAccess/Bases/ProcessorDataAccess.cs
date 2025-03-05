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
	public partial class ProcessorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPROCESSOR = "InsertProcessor";
		private const string UPDATEPROCESSOR = "UpdateProcessor";
		private const string DELETEPROCESSOR = "DeleteProcessor";
		private const string GETPROCESSORBYID = "GetProcessorById";
		private const string GETALLPROCESSOR = "GetAllProcessor";
		private const string GETPAGEDPROCESSOR = "GetPagedProcessor";
		private const string GETPROCESSORMAXIMUMID = "GetProcessorMaximumId";
		private const string GETPROCESSORROWCOUNT = "GetProcessorRowCount";	
		private const string GETPROCESSORBYQUERY = "GetProcessorByQuery";
		#endregion
		
		#region Constructors
		public ProcessorDataAccess(ClientContext context) : base(context) { }
		public ProcessorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="processorObject"></param>
		private void AddCommonParams(SqlCommand cmd, ProcessorBase processorObject)
		{	
			AddParameter(cmd, pGuid(ProcessorBase.Property_CompanyId, processorObject.CompanyId));
			AddParameter(cmd, pNVarChar(ProcessorBase.Property_Name, 50, processorObject.Name));
			AddParameter(cmd, pInt32(ProcessorBase.Property_OrderBy, processorObject.OrderBy));
			AddParameter(cmd, pBool(ProcessorBase.Property_IsActive, processorObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Processor
        /// </summary>
        /// <param name="processorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ProcessorBase processorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPROCESSOR);
	
				AddParameter(cmd, pInt32Out(ProcessorBase.Property_Id));
				AddCommonParams(cmd, processorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					processorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					processorObject.Id = (Int32)GetOutParameter(cmd, ProcessorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(processorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Processor
        /// </summary>
        /// <param name="processorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ProcessorBase processorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPROCESSOR);
				
				AddParameter(cmd, pInt32(ProcessorBase.Property_Id, processorObject.Id));
				AddCommonParams(cmd, processorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					processorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(processorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Processor
        /// </summary>
        /// <param name="Id">Id of the Processor object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPROCESSOR);	
				
				AddParameter(cmd, pInt32(ProcessorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Processor), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Processor object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Processor object to retrieve</param>
        /// <returns>Processor object, null if not found</returns>
		public Processor Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPROCESSORBYID))
			{
				AddParameter( cmd, pInt32(ProcessorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Processor objects 
        /// </summary>
        /// <returns>A list of Processor objects</returns>
		public ProcessorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPROCESSOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Processor objects by PageRequest
        /// </summary>
        /// <returns>A list of Processor objects</returns>
		public ProcessorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPROCESSOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ProcessorList _ProcessorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ProcessorList;
			}
		}
		
		/// <summary>
        /// Retrieves all Processor objects by query String
        /// </summary>
        /// <returns>A list of Processor objects</returns>
		public ProcessorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPROCESSORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Processor Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Processor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPROCESSORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Processor Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Processor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ProcessorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPROCESSORROWCOUNT))
			{
				SqlDataReader reader;
				_ProcessorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ProcessorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Processor object
        /// </summary>
        /// <param name="processorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ProcessorBase processorObject, SqlDataReader reader, int start)
		{
			
				processorObject.Id = reader.GetInt32( start + 0 );			
				processorObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) processorObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) processorObject.OrderBy = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) processorObject.IsActive = reader.GetBoolean( start + 4 );			
			FillBaseObject(processorObject, reader, (start + 5));

			
			processorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Processor object
        /// </summary>
        /// <param name="processorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ProcessorBase processorObject, SqlDataReader reader)
		{
			FillObject(processorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Processor object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Processor object</returns>
		private Processor GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Processor processorObject= new Processor();
					FillObject(processorObject, reader);
					return processorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Processor objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Processor objects</returns>
		private ProcessorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Processor list
			ProcessorList list = new ProcessorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Processor processorObject = new Processor();
					FillObject(processorObject, reader);

					list.Add(processorObject);
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
