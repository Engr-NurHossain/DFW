using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using Newtonsoft.Json;

namespace HS.DataAccess
{
	public partial class AssignedInventoryTechReceivedDataAccess : BaseDataAccess
	{
		private ErrorLogDataAccess _ErrorLogDataAccess;

		#region Constants
		private const string INSERTASSIGNEDINVENTORYTECHRECEIVED = "InsertAssignedInventoryTechReceived";
		private const string UPDATEASSIGNEDINVENTORYTECHRECEIVED = "UpdateAssignedInventoryTechReceived";
		private const string UPDATEASSIGNEDINVENTORYTECHRECEIVED_DG = "UpdateAssignedInventoryTechReceived_v3";
		private const string DELETEASSIGNEDINVENTORYTECHRECEIVED = "DeleteAssignedInventoryTechReceived";
		private const string GETASSIGNEDINVENTORYTECHRECEIVEDBYID = "GetAssignedInventoryTechReceivedById";
		private const string GETALLASSIGNEDINVENTORYTECHRECEIVED = "GetAllAssignedInventoryTechReceived";
		private const string GETPAGEDASSIGNEDINVENTORYTECHRECEIVED = "GetPagedAssignedInventoryTechReceived";
		private const string GETASSIGNEDINVENTORYTECHRECEIVEDMAXIMUMID = "GetAssignedInventoryTechReceivedMaximumId";
		private const string GETASSIGNEDINVENTORYTECHRECEIVEDROWCOUNT = "GetAssignedInventoryTechReceivedRowCount";	
		private const string GETASSIGNEDINVENTORYTECHRECEIVEDBYQUERY = "GetAssignedInventoryTechReceivedByQuery";
		#endregion
		
		#region Constructors
		public AssignedInventoryTechReceivedDataAccess(ClientContext context) : base(context) {
			_ErrorLogDataAccess = new ErrorLogDataAccess(context);
		}
		public AssignedInventoryTechReceivedDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) {
			_ErrorLogDataAccess = new ErrorLogDataAccess(transaction, context);
		}
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="assignedInventoryTechReceivedObject"></param>
		private void AddCommonParams(SqlCommand cmd, AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject)
		{	
			AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_TechnicianId, assignedInventoryTechReceivedObject.TechnicianId));
			AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_EquipmentId, assignedInventoryTechReceivedObject.EquipmentId));
			AddParameter(cmd, pInt32(AssignedInventoryTechReceivedBase.Property_Quantity, assignedInventoryTechReceivedObject.Quantity));
			AddParameter(cmd, pBool(AssignedInventoryTechReceivedBase.Property_IsReceived, assignedInventoryTechReceivedObject.IsReceived));
			AddParameter(cmd, pDateTime(AssignedInventoryTechReceivedBase.Property_ReceivedDate, assignedInventoryTechReceivedObject.ReceivedDate));
			AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_ReceivedBy, assignedInventoryTechReceivedObject.ReceivedBy.Value));
			AddParameter(cmd, pDateTime(AssignedInventoryTechReceivedBase.Property_CreatedDate, assignedInventoryTechReceivedObject.CreatedDate));
			AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_CreatedBy, assignedInventoryTechReceivedObject.CreatedBy));
			AddParameter(cmd, pBool(AssignedInventoryTechReceivedBase.Property_IsApprove, assignedInventoryTechReceivedObject.IsApprove));
			AddParameter(cmd, pBool(AssignedInventoryTechReceivedBase.Property_IsDecline, assignedInventoryTechReceivedObject.IsDecline));
			if (cmd.CommandText == INSERTASSIGNEDINVENTORYTECHRECEIVED)
			{
				AddParameter(cmd, pNVarChar(AssignedInventoryTechReceivedBase.Property_ReqSrc, assignedInventoryTechReceivedObject.ReqSrc));
			}
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AssignedInventoryTechReceived
        /// </summary>
        /// <param name="assignedInventoryTechReceivedObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTASSIGNEDINVENTORYTECHRECEIVED);
	
				AddParameter(cmd, pInt32Out(AssignedInventoryTechReceivedBase.Property_Id));
				AddCommonParams(cmd, assignedInventoryTechReceivedObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					assignedInventoryTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					assignedInventoryTechReceivedObject.Id = (Int32)GetOutParameter(cmd, AssignedInventoryTechReceivedBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(assignedInventoryTechReceivedObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AssignedInventoryTechReceived
        /// </summary>
        /// <param name="assignedInventoryTechReceivedObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEASSIGNEDINVENTORYTECHRECEIVED_DG);
				
				AddParameter(cmd, pInt32(AssignedInventoryTechReceivedBase.Property_Id, assignedInventoryTechReceivedObject.Id));
                AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_ClosedBy, assignedInventoryTechReceivedObject.ClosedBy));

                AddCommonParams(cmd, assignedInventoryTechReceivedObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					assignedInventoryTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(assignedInventoryTechReceivedObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AssignedInventoryTechReceived
        /// </summary>
        /// <param name="Id">Id of the AssignedInventoryTechReceived object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEASSIGNEDINVENTORYTECHRECEIVED);	
				
				AddParameter(cmd, pInt32(AssignedInventoryTechReceivedBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AssignedInventoryTechReceived), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AssignedInventoryTechReceived object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AssignedInventoryTechReceived object to retrieve</param>
        /// <returns>AssignedInventoryTechReceived object, null if not found</returns>
		public AssignedInventoryTechReceived Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETASSIGNEDINVENTORYTECHRECEIVEDBYID))
			{
				AddParameter( cmd, pInt32(AssignedInventoryTechReceivedBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AssignedInventoryTechReceived objects 
        /// </summary>
        /// <returns>A list of AssignedInventoryTechReceived objects</returns>
		public AssignedInventoryTechReceivedList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLASSIGNEDINVENTORYTECHRECEIVED))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AssignedInventoryTechReceived objects by PageRequest
        /// </summary>
        /// <returns>A list of AssignedInventoryTechReceived objects</returns>
		public AssignedInventoryTechReceivedList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDASSIGNEDINVENTORYTECHRECEIVED))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AssignedInventoryTechReceivedList _AssignedInventoryTechReceivedList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AssignedInventoryTechReceivedList;
			}
		}
		
		/// <summary>
        /// Retrieves all AssignedInventoryTechReceived objects by query String
        /// </summary>
        /// <returns>A list of AssignedInventoryTechReceived objects</returns>
		public AssignedInventoryTechReceivedList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETASSIGNEDINVENTORYTECHRECEIVEDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AssignedInventoryTechReceived Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AssignedInventoryTechReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETASSIGNEDINVENTORYTECHRECEIVEDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AssignedInventoryTechReceived Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AssignedInventoryTechReceived
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AssignedInventoryTechReceivedRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETASSIGNEDINVENTORYTECHRECEIVEDROWCOUNT))
			{
				SqlDataReader reader;
				_AssignedInventoryTechReceivedRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AssignedInventoryTechReceivedRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AssignedInventoryTechReceived object
        /// </summary>
        /// <param name="assignedInventoryTechReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject, SqlDataReader reader, int start)
		{
			
				assignedInventoryTechReceivedObject.Id = reader.GetInt32( start + 0 );			
				assignedInventoryTechReceivedObject.TechnicianId = reader.GetGuid( start + 1 );			
				assignedInventoryTechReceivedObject.EquipmentId = reader.GetGuid( start + 2 );			
				assignedInventoryTechReceivedObject.Quantity = reader.GetInt32( start + 3 );			
				assignedInventoryTechReceivedObject.IsReceived = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) assignedInventoryTechReceivedObject.ReceivedDate = reader.GetDateTime( start + 5 );			
				if(!reader.IsDBNull(6)) assignedInventoryTechReceivedObject.ReceivedBy = reader.GetGuid( start + 6 );			
				assignedInventoryTechReceivedObject.CreatedDate = reader.GetDateTime( start + 7 );			
				assignedInventoryTechReceivedObject.CreatedBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) assignedInventoryTechReceivedObject.IsApprove = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) assignedInventoryTechReceivedObject.IsDecline = reader.GetBoolean( start + 10 );			
			FillBaseObject(assignedInventoryTechReceivedObject, reader, (start + 11));

			
			assignedInventoryTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AssignedInventoryTechReceived object
        /// </summary>
        /// <param name="assignedInventoryTechReceivedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject, SqlDataReader reader)
		{
			FillObject(assignedInventoryTechReceivedObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AssignedInventoryTechReceived object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AssignedInventoryTechReceived object</returns>
		private AssignedInventoryTechReceived GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AssignedInventoryTechReceived assignedInventoryTechReceivedObject= new AssignedInventoryTechReceived();
					FillObject(assignedInventoryTechReceivedObject, reader);
					return assignedInventoryTechReceivedObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AssignedInventoryTechReceived objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AssignedInventoryTechReceived objects</returns>
		private AssignedInventoryTechReceivedList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AssignedInventoryTechReceived list
			AssignedInventoryTechReceivedList list = new AssignedInventoryTechReceivedList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AssignedInventoryTechReceived assignedInventoryTechReceivedObject = new AssignedInventoryTechReceived();
					FillObject(assignedInventoryTechReceivedObject, reader);

					list.Add(assignedInventoryTechReceivedObject);
				}
				
				// Close the reader in order to receive output parameters
				// Output parameters are not available until reader is closed.
				reader.Close();
			}

			return list;
		}

		#endregion

		#region Digiture

		//public long InsertTechTransfer(TechTransferRequest assignedInventoryTechReceivedObject)
		//{
		//	try
		//	{
		//		SqlCommand cmd = GetSPCommand(INSERTASSIGNEDINVENTORYTECHRECEIVED);
		//		foreach (var item in assignedInventoryTechReceivedObject.Items)
		//              {
		//			AddParameter(cmd, pInt32Out(AssignedInventoryTechReceivedBase.Property_Id));
		//			AddCommonParams(cmd, item);
		//		}



		//		AddParameter(cmd, pInt32Out(AssignedInventoryTechReceivedBase.Property_Id));
		//		AddCommonParams(cmd, assignedInventoryTechReceivedObject);

		//		long result = InsertRecord(cmd);
		//		if (result > 0)
		//		{
		//			assignedInventoryTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
		//			assignedInventoryTechReceivedObject.Id = (Int32)GetOutParameter(cmd, AssignedInventoryTechReceivedBase.Property_Id);
		//		}
		//		return result;
		//	}
		//	catch (SqlException x)
		//	{
		//		throw new ObjectInsertException(assignedInventoryTechReceivedObject, x);
		//	}
		//}

		public long Update_DG(AssignedInventoryTechReceivedBase assignedInventoryTechReceivedObject)
		{
			long result = 0;
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEASSIGNEDINVENTORYTECHRECEIVED_DG);

				AddParameter(cmd, pInt32(AssignedInventoryTechReceivedBase.Property_Id, assignedInventoryTechReceivedObject.Id));
                AddParameter(cmd, pGuid(AssignedInventoryTechReceivedBase.Property_ClosedBy, assignedInventoryTechReceivedObject.ClosedBy));

                AddCommonParams(cmd, assignedInventoryTechReceivedObject);

				result = UpdateRecord(cmd);
				if (result > 0)
					assignedInventoryTechReceivedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				
			}
			catch (SqlException ex)
			{
				_ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|AssignedInventoryTechReceived|Update_DG", Message = string.Format("{0} || {1}", ex.Message, JsonConvert.SerializeObject(assignedInventoryTechReceivedObject)), TimeUtc = DateTime.Now });
				//throw new ObjectUpdateException(assignedInventoryTechReceivedObject, x);
			}
			return result;
		}

		#endregion Digiture
	}
}
