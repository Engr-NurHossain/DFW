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
	public partial class TicketFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETFILE = "InsertTicketFile";
		private const string UPDATETICKETFILE = "UpdateTicketFile";
		private const string DELETETICKETFILE = "DeleteTicketFile";
		private const string GETTICKETFILEBYID = "GetTicketFileById";
		private const string GETALLTICKETFILE = "GetAllTicketFile";
		private const string GETPAGEDTICKETFILE = "GetPagedTicketFile";
		private const string GETTICKETFILEMAXIMUMID = "GetTicketFileMaximumId";
		private const string GETTICKETFILEROWCOUNT = "GetTicketFileRowCount";	
		private const string GETTICKETFILEBYQUERY = "GetTicketFileByQuery";
		#endregion
		
		#region Constructors
		public TicketFileDataAccess(ClientContext context) : base(context) { }
		public TicketFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketFileBase ticketFileObject)
		{	
			AddParameter(cmd, pGuid(TicketFileBase.Property_TicketId, ticketFileObject.TicketId));
			AddParameter(cmd, pNVarChar(TicketFileBase.Property_FileName, 500, ticketFileObject.FileName));
			AddParameter(cmd, pInt32(TicketFileBase.Property_Filesize, ticketFileObject.Filesize));
			AddParameter(cmd, pNVarChar(TicketFileBase.Property_FileLocation, 500, ticketFileObject.FileLocation));
			AddParameter(cmd, pNVarChar(TicketFileBase.Property_Description, ticketFileObject.Description));
			AddParameter(cmd, pGuid(TicketFileBase.Property_FileAddedBy, ticketFileObject.FileAddedBy));
			AddParameter(cmd, pDateTime(TicketFileBase.Property_FileAddedDate, ticketFileObject.FileAddedDate));
			AddParameter(cmd, pInt32(TicketFileBase.Property_TicketBookingDetailsId, ticketFileObject.TicketBookingDetailsId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketFile
        /// </summary>
        /// <param name="ticketFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketFileBase ticketFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETFILE);
	
				AddParameter(cmd, pInt32Out(TicketFileBase.Property_Id));
				AddCommonParams(cmd, ticketFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketFileObject.Id = (Int32)GetOutParameter(cmd, TicketFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketFile
        /// </summary>
        /// <param name="ticketFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketFileBase ticketFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETFILE);
				
				AddParameter(cmd, pInt32(TicketFileBase.Property_Id, ticketFileObject.Id));
				AddCommonParams(cmd, ticketFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketFile
        /// </summary>
        /// <param name="Id">Id of the TicketFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETFILE);	
				
				AddParameter(cmd, pInt32(TicketFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketFile object to retrieve</param>
        /// <returns>TicketFile object, null if not found</returns>
		public TicketFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETFILEBYID))
			{
				AddParameter( cmd, pInt32(TicketFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketFile objects 
        /// </summary>
        /// <returns>A list of TicketFile objects</returns>
		public TicketFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketFile objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketFile objects</returns>
		public TicketFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketFileList _TicketFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketFile objects by query String
        /// </summary>
        /// <returns>A list of TicketFile objects</returns>
		public TicketFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETFILEROWCOUNT))
			{
				SqlDataReader reader;
				_TicketFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketFile object
        /// </summary>
        /// <param name="ticketFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketFileBase ticketFileObject, SqlDataReader reader, int start)
		{
			
				ticketFileObject.Id = reader.GetInt32( start + 0 );			
				ticketFileObject.TicketId = reader.GetGuid( start + 1 );			
				ticketFileObject.FileName = reader.GetString( start + 2 );			
				ticketFileObject.Filesize = reader.GetInt32( start + 3 );			
				ticketFileObject.FileLocation = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketFileObject.Description = reader.GetString( start + 5 );			
				ticketFileObject.FileAddedBy = reader.GetGuid( start + 6 );			
				ticketFileObject.FileAddedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) ticketFileObject.TicketBookingDetailsId = reader.GetInt32( start + 8 );			
			FillBaseObject(ticketFileObject, reader, (start + 9));

			
			ticketFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketFile object
        /// </summary>
        /// <param name="ticketFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketFileBase ticketFileObject, SqlDataReader reader)
		{
			FillObject(ticketFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketFile object</returns>
		private TicketFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketFile ticketFileObject= new TicketFile();
					FillObject(ticketFileObject, reader);
					return ticketFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketFile objects</returns>
		private TicketFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketFile list
			TicketFileList list = new TicketFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketFile ticketFileObject = new TicketFile();
					FillObject(ticketFileObject, reader);

					list.Add(ticketFileObject);
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
