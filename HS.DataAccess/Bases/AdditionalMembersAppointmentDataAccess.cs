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
	public partial class AdditionalMembersAppointmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADDITIONALMEMBERSAPPOINTMENT = "InsertAdditionalMembersAppointment";
		private const string UPDATEADDITIONALMEMBERSAPPOINTMENT = "UpdateAdditionalMembersAppointment";
		private const string DELETEADDITIONALMEMBERSAPPOINTMENT = "DeleteAdditionalMembersAppointment";
		private const string GETADDITIONALMEMBERSAPPOINTMENTBYID = "GetAdditionalMembersAppointmentById";
		private const string GETALLADDITIONALMEMBERSAPPOINTMENT = "GetAllAdditionalMembersAppointment";
		private const string GETPAGEDADDITIONALMEMBERSAPPOINTMENT = "GetPagedAdditionalMembersAppointment";
		private const string GETADDITIONALMEMBERSAPPOINTMENTMAXIMUMID = "GetAdditionalMembersAppointmentMaximumId";
		private const string GETADDITIONALMEMBERSAPPOINTMENTROWCOUNT = "GetAdditionalMembersAppointmentRowCount";	
		private const string GETADDITIONALMEMBERSAPPOINTMENTBYQUERY = "GetAdditionalMembersAppointmentByQuery";
		#endregion
		
		#region Constructors
		public AdditionalMembersAppointmentDataAccess(ClientContext context) : base(context) { }
		public AdditionalMembersAppointmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="additionalMembersAppointmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, AdditionalMembersAppointmentBase additionalMembersAppointmentObject)
		{	
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_AppointmentId, additionalMembersAppointmentObject.AppointmentId));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_CompanyId, additionalMembersAppointmentObject.CompanyId));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_CustomerId, additionalMembersAppointmentObject.CustomerId));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_EmployeeId, additionalMembersAppointmentObject.EmployeeId));
			AddParameter(cmd, pDateTime(AdditionalMembersAppointmentBase.Property_AppointmentDate, additionalMembersAppointmentObject.AppointmentDate));
			AddParameter(cmd, pNVarChar(AdditionalMembersAppointmentBase.Property_AppointmentStartTime, 50, additionalMembersAppointmentObject.AppointmentStartTime));
			AddParameter(cmd, pNVarChar(AdditionalMembersAppointmentBase.Property_AppointmentEndTime, 50, additionalMembersAppointmentObject.AppointmentEndTime));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_CreatedBy, additionalMembersAppointmentObject.CreatedBy));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_LastUpdatedBy, additionalMembersAppointmentObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(AdditionalMembersAppointmentBase.Property_LastUpdatedDate, additionalMembersAppointmentObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(AdditionalMembersAppointmentBase.Property_MemberAppointmentId, additionalMembersAppointmentObject.MemberAppointmentId));
			AddParameter(cmd, pBool(AdditionalMembersAppointmentBase.Property_IsAllDay, additionalMembersAppointmentObject.IsAllDay));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AdditionalMembersAppointment
        /// </summary>
        /// <param name="additionalMembersAppointmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AdditionalMembersAppointmentBase additionalMembersAppointmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADDITIONALMEMBERSAPPOINTMENT);
	
				AddParameter(cmd, pInt32Out(AdditionalMembersAppointmentBase.Property_Id));
				AddCommonParams(cmd, additionalMembersAppointmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					additionalMembersAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					additionalMembersAppointmentObject.Id = (Int32)GetOutParameter(cmd, AdditionalMembersAppointmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(additionalMembersAppointmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AdditionalMembersAppointment
        /// </summary>
        /// <param name="additionalMembersAppointmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AdditionalMembersAppointmentBase additionalMembersAppointmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADDITIONALMEMBERSAPPOINTMENT);
				
				AddParameter(cmd, pInt32(AdditionalMembersAppointmentBase.Property_Id, additionalMembersAppointmentObject.Id));
				AddCommonParams(cmd, additionalMembersAppointmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					additionalMembersAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(additionalMembersAppointmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AdditionalMembersAppointment
        /// </summary>
        /// <param name="Id">Id of the AdditionalMembersAppointment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADDITIONALMEMBERSAPPOINTMENT);	
				
				AddParameter(cmd, pInt32(AdditionalMembersAppointmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AdditionalMembersAppointment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AdditionalMembersAppointment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AdditionalMembersAppointment object to retrieve</param>
        /// <returns>AdditionalMembersAppointment object, null if not found</returns>
		public AdditionalMembersAppointment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADDITIONALMEMBERSAPPOINTMENTBYID))
			{
				AddParameter( cmd, pInt32(AdditionalMembersAppointmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AdditionalMembersAppointment objects 
        /// </summary>
        /// <returns>A list of AdditionalMembersAppointment objects</returns>
		public AdditionalMembersAppointmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADDITIONALMEMBERSAPPOINTMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AdditionalMembersAppointment objects by PageRequest
        /// </summary>
        /// <returns>A list of AdditionalMembersAppointment objects</returns>
		public AdditionalMembersAppointmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADDITIONALMEMBERSAPPOINTMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AdditionalMembersAppointmentList _AdditionalMembersAppointmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AdditionalMembersAppointmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all AdditionalMembersAppointment objects by query String
        /// </summary>
        /// <returns>A list of AdditionalMembersAppointment objects</returns>
		public AdditionalMembersAppointmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADDITIONALMEMBERSAPPOINTMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AdditionalMembersAppointment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AdditionalMembersAppointment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADDITIONALMEMBERSAPPOINTMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AdditionalMembersAppointment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AdditionalMembersAppointment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AdditionalMembersAppointmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADDITIONALMEMBERSAPPOINTMENTROWCOUNT))
			{
				SqlDataReader reader;
				_AdditionalMembersAppointmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AdditionalMembersAppointmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AdditionalMembersAppointment object
        /// </summary>
        /// <param name="additionalMembersAppointmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AdditionalMembersAppointmentBase additionalMembersAppointmentObject, SqlDataReader reader, int start)
		{
			
				additionalMembersAppointmentObject.Id = reader.GetInt32( start + 0 );			
				additionalMembersAppointmentObject.AppointmentId = reader.GetGuid( start + 1 );			
				additionalMembersAppointmentObject.CompanyId = reader.GetGuid( start + 2 );			
				additionalMembersAppointmentObject.CustomerId = reader.GetGuid( start + 3 );			
				additionalMembersAppointmentObject.EmployeeId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) additionalMembersAppointmentObject.AppointmentDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) additionalMembersAppointmentObject.AppointmentStartTime = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) additionalMembersAppointmentObject.AppointmentEndTime = reader.GetString( start + 7 );			
				additionalMembersAppointmentObject.CreatedBy = reader.GetGuid( start + 8 );			
				additionalMembersAppointmentObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				additionalMembersAppointmentObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
				additionalMembersAppointmentObject.MemberAppointmentId = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) additionalMembersAppointmentObject.IsAllDay = reader.GetBoolean( start + 12 );			
			FillBaseObject(additionalMembersAppointmentObject, reader, (start + 13));

			
			additionalMembersAppointmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AdditionalMembersAppointment object
        /// </summary>
        /// <param name="additionalMembersAppointmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AdditionalMembersAppointmentBase additionalMembersAppointmentObject, SqlDataReader reader)
		{
			FillObject(additionalMembersAppointmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AdditionalMembersAppointment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AdditionalMembersAppointment object</returns>
		private AdditionalMembersAppointment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AdditionalMembersAppointment additionalMembersAppointmentObject= new AdditionalMembersAppointment();
					FillObject(additionalMembersAppointmentObject, reader);
					return additionalMembersAppointmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AdditionalMembersAppointment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AdditionalMembersAppointment objects</returns>
		private AdditionalMembersAppointmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AdditionalMembersAppointment list
			AdditionalMembersAppointmentList list = new AdditionalMembersAppointmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AdditionalMembersAppointment additionalMembersAppointmentObject = new AdditionalMembersAppointment();
					FillObject(additionalMembersAppointmentObject, reader);

					list.Add(additionalMembersAppointmentObject);
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
