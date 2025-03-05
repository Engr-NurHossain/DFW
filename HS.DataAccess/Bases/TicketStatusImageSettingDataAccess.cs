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
	public partial class TicketStatusImageSettingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETSTATUSIMAGESETTING = "InsertTicketStatusImageSetting";
		private const string UPDATETICKETSTATUSIMAGESETTING = "UpdateTicketStatusImageSetting";
		private const string DELETETICKETSTATUSIMAGESETTING = "DeleteTicketStatusImageSetting";
		private const string GETTICKETSTATUSIMAGESETTINGBYID = "GetTicketStatusImageSettingById";
		private const string GETALLTICKETSTATUSIMAGESETTING = "GetAllTicketStatusImageSetting";
		private const string GETPAGEDTICKETSTATUSIMAGESETTING = "GetPagedTicketStatusImageSetting";
		private const string GETTICKETSTATUSIMAGESETTINGMAXIMUMID = "GetTicketStatusImageSettingMaximumId";
		private const string GETTICKETSTATUSIMAGESETTINGROWCOUNT = "GetTicketStatusImageSettingRowCount";	
		private const string GETTICKETSTATUSIMAGESETTINGBYQUERY = "GetTicketStatusImageSettingByQuery";
		#endregion
		
		#region Constructors
		public TicketStatusImageSettingDataAccess(ClientContext context) : base(context) { }
		public TicketStatusImageSettingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketStatusImageSettingObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketStatusImageSettingBase ticketStatusImageSettingObject)
		{	
			AddParameter(cmd, pGuid(TicketStatusImageSettingBase.Property_CompanyId, ticketStatusImageSettingObject.CompanyId));
			AddParameter(cmd, pNVarChar(TicketStatusImageSettingBase.Property_TicketStatus, 150, ticketStatusImageSettingObject.TicketStatus));
			AddParameter(cmd, pNVarChar(TicketStatusImageSettingBase.Property_FileDescription, ticketStatusImageSettingObject.FileDescription));
			AddParameter(cmd, pNVarChar(TicketStatusImageSettingBase.Property_Filename, 500, ticketStatusImageSettingObject.Filename));
			AddParameter(cmd, pNVarChar(TicketStatusImageSettingBase.Property_FileFullName, 500, ticketStatusImageSettingObject.FileFullName));
			AddParameter(cmd, pDateTime(TicketStatusImageSettingBase.Property_Uploadeddate, ticketStatusImageSettingObject.Uploadeddate));
			AddParameter(cmd, pBool(TicketStatusImageSettingBase.Property_IsActive, ticketStatusImageSettingObject.IsActive));
			AddParameter(cmd, pDouble(TicketStatusImageSettingBase.Property_FileSize, ticketStatusImageSettingObject.FileSize));
			AddParameter(cmd, pNVarChar(TicketStatusImageSettingBase.Property_TicketStatusColor, 50, ticketStatusImageSettingObject.TicketStatusColor));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketStatusImageSetting
        /// </summary>
        /// <param name="ticketStatusImageSettingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketStatusImageSettingBase ticketStatusImageSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETSTATUSIMAGESETTING);
	
				AddParameter(cmd, pInt32Out(TicketStatusImageSettingBase.Property_Id));
				AddCommonParams(cmd, ticketStatusImageSettingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketStatusImageSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketStatusImageSettingObject.Id = (Int32)GetOutParameter(cmd, TicketStatusImageSettingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketStatusImageSettingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketStatusImageSetting
        /// </summary>
        /// <param name="ticketStatusImageSettingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketStatusImageSettingBase ticketStatusImageSettingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETSTATUSIMAGESETTING);
				
				AddParameter(cmd, pInt32(TicketStatusImageSettingBase.Property_Id, ticketStatusImageSettingObject.Id));
				AddCommonParams(cmd, ticketStatusImageSettingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketStatusImageSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketStatusImageSettingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketStatusImageSetting
        /// </summary>
        /// <param name="Id">Id of the TicketStatusImageSetting object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETSTATUSIMAGESETTING);	
				
				AddParameter(cmd, pInt32(TicketStatusImageSettingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketStatusImageSetting), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketStatusImageSetting object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketStatusImageSetting object to retrieve</param>
        /// <returns>TicketStatusImageSetting object, null if not found</returns>
		public TicketStatusImageSetting Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETSTATUSIMAGESETTINGBYID))
			{
				AddParameter( cmd, pInt32(TicketStatusImageSettingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketStatusImageSetting objects 
        /// </summary>
        /// <returns>A list of TicketStatusImageSetting objects</returns>
		public TicketStatusImageSettingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETSTATUSIMAGESETTING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketStatusImageSetting objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketStatusImageSetting objects</returns>
		public TicketStatusImageSettingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETSTATUSIMAGESETTING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketStatusImageSettingList _TicketStatusImageSettingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketStatusImageSettingList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketStatusImageSetting objects by query String
        /// </summary>
        /// <returns>A list of TicketStatusImageSetting objects</returns>
		public TicketStatusImageSettingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETSTATUSIMAGESETTINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketStatusImageSetting Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketStatusImageSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETSTATUSIMAGESETTINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketStatusImageSetting Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketStatusImageSetting
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketStatusImageSettingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETSTATUSIMAGESETTINGROWCOUNT))
			{
				SqlDataReader reader;
				_TicketStatusImageSettingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketStatusImageSettingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketStatusImageSetting object
        /// </summary>
        /// <param name="ticketStatusImageSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketStatusImageSettingBase ticketStatusImageSettingObject, SqlDataReader reader, int start)
		{
			
				ticketStatusImageSettingObject.Id = reader.GetInt32( start + 0 );			
				ticketStatusImageSettingObject.CompanyId = reader.GetGuid( start + 1 );			
				ticketStatusImageSettingObject.TicketStatus = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) ticketStatusImageSettingObject.FileDescription = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) ticketStatusImageSettingObject.Filename = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketStatusImageSettingObject.FileFullName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) ticketStatusImageSettingObject.Uploadeddate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) ticketStatusImageSettingObject.IsActive = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) ticketStatusImageSettingObject.FileSize = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) ticketStatusImageSettingObject.TicketStatusColor = reader.GetString( start + 9 );			
			FillBaseObject(ticketStatusImageSettingObject, reader, (start + 10));

			
			ticketStatusImageSettingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketStatusImageSetting object
        /// </summary>
        /// <param name="ticketStatusImageSettingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketStatusImageSettingBase ticketStatusImageSettingObject, SqlDataReader reader)
		{
			FillObject(ticketStatusImageSettingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketStatusImageSetting object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketStatusImageSetting object</returns>
		private TicketStatusImageSetting GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketStatusImageSetting ticketStatusImageSettingObject= new TicketStatusImageSetting();
					FillObject(ticketStatusImageSettingObject, reader);
					return ticketStatusImageSettingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketStatusImageSetting objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketStatusImageSetting objects</returns>
		private TicketStatusImageSettingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketStatusImageSetting list
			TicketStatusImageSettingList list = new TicketStatusImageSettingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketStatusImageSetting ticketStatusImageSettingObject = new TicketStatusImageSetting();
					FillObject(ticketStatusImageSettingObject, reader);

					list.Add(ticketStatusImageSettingObject);
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
