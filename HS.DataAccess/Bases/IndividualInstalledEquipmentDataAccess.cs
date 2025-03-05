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
	public partial class IndividualInstalledEquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTINDIVIDUALINSTALLEDEQUIPMENT = "InsertIndividualInstalledEquipment";
		private const string UPDATEINDIVIDUALINSTALLEDEQUIPMENT = "UpdateIndividualInstalledEquipment";
		private const string DELETEINDIVIDUALINSTALLEDEQUIPMENT = "DeleteIndividualInstalledEquipment";
		private const string GETINDIVIDUALINSTALLEDEQUIPMENTBYID = "GetIndividualInstalledEquipmentById";
		private const string GETALLINDIVIDUALINSTALLEDEQUIPMENT = "GetAllIndividualInstalledEquipment";
		private const string GETPAGEDINDIVIDUALINSTALLEDEQUIPMENT = "GetPagedIndividualInstalledEquipment";
		private const string GETINDIVIDUALINSTALLEDEQUIPMENTMAXIMUMID = "GetIndividualInstalledEquipmentMaximumId";
		private const string GETINDIVIDUALINSTALLEDEQUIPMENTROWCOUNT = "GetIndividualInstalledEquipmentRowCount";	
		private const string GETINDIVIDUALINSTALLEDEQUIPMENTBYQUERY = "GetIndividualInstalledEquipmentByQuery";
		#endregion
		
		#region Constructors
		public IndividualInstalledEquipmentDataAccess(ClientContext context) : base(context) { }
		public IndividualInstalledEquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="individualInstalledEquipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, IndividualInstalledEquipmentBase individualInstalledEquipmentObject)
		{	
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_AppointmentEquipmentId, individualInstalledEquipmentObject.AppointmentEquipmentId));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_Category, 50, individualInstalledEquipmentObject.Category));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_Manufacturer, 50, individualInstalledEquipmentObject.Manufacturer));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_Description, 50, individualInstalledEquipmentObject.Description));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_TicketType, 50, individualInstalledEquipmentObject.TicketType));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_EmpUser, 50, individualInstalledEquipmentObject.EmpUser));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_TicketId, individualInstalledEquipmentObject.TicketId));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_RepliesCount, individualInstalledEquipmentObject.RepliesCount));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_AttachmentsCount, individualInstalledEquipmentObject.AttachmentsCount));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_CusIdInt, individualInstalledEquipmentObject.CusIdInt));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_CustomerName, 50, individualInstalledEquipmentObject.CustomerName));
			AddParameter(cmd, pDateTime(IndividualInstalledEquipmentBase.Property_CompletionDate, individualInstalledEquipmentObject.CompletionDate));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_SKU, 50, individualInstalledEquipmentObject.SKU));
			AddParameter(cmd, pDouble(IndividualInstalledEquipmentBase.Property_TotalPoint, individualInstalledEquipmentObject.TotalPoint));
			AddParameter(cmd, pBool(IndividualInstalledEquipmentBase.Property_IsClosed, individualInstalledEquipmentObject.IsClosed));
			AddParameter(cmd, pDouble(IndividualInstalledEquipmentBase.Property_CompanyCost, individualInstalledEquipmentObject.CompanyCost));
			AddParameter(cmd, pDouble(IndividualInstalledEquipmentBase.Property_CustomerCost, individualInstalledEquipmentObject.CustomerCost));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_Quantity, individualInstalledEquipmentObject.Quantity));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_InstalledEquipment, individualInstalledEquipmentObject.InstalledEquipment));
			AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_Qty, individualInstalledEquipmentObject.Qty));
			AddParameter(cmd, pNVarChar(IndividualInstalledEquipmentBase.Property_Status, 50, individualInstalledEquipmentObject.Status));
			AddParameter(cmd, pGuid(IndividualInstalledEquipmentBase.Property_CreatedBy, individualInstalledEquipmentObject.CreatedBy));
			AddParameter(cmd, pDateTime(IndividualInstalledEquipmentBase.Property_CreatedDate, individualInstalledEquipmentObject.CreatedDate));
			AddParameter(cmd, pGuid(IndividualInstalledEquipmentBase.Property_InstalledByUid, individualInstalledEquipmentObject.InstalledByUid));
			AddParameter(cmd, pGuid(IndividualInstalledEquipmentBase.Property_EquipmentId, individualInstalledEquipmentObject.EquipmentId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts IndividualInstalledEquipment
        /// </summary>
        /// <param name="individualInstalledEquipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(IndividualInstalledEquipmentBase individualInstalledEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTINDIVIDUALINSTALLEDEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(IndividualInstalledEquipmentBase.Property_Id));
				AddCommonParams(cmd, individualInstalledEquipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					individualInstalledEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					individualInstalledEquipmentObject.Id = (Int32)GetOutParameter(cmd, IndividualInstalledEquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(individualInstalledEquipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates IndividualInstalledEquipment
        /// </summary>
        /// <param name="individualInstalledEquipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(IndividualInstalledEquipmentBase individualInstalledEquipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEINDIVIDUALINSTALLEDEQUIPMENT);
				
				AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_Id, individualInstalledEquipmentObject.Id));
				AddCommonParams(cmd, individualInstalledEquipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					individualInstalledEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(individualInstalledEquipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes IndividualInstalledEquipment
        /// </summary>
        /// <param name="Id">Id of the IndividualInstalledEquipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEINDIVIDUALINSTALLEDEQUIPMENT);	
				
				AddParameter(cmd, pInt32(IndividualInstalledEquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(IndividualInstalledEquipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves IndividualInstalledEquipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the IndividualInstalledEquipment object to retrieve</param>
        /// <returns>IndividualInstalledEquipment object, null if not found</returns>
		public IndividualInstalledEquipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETINDIVIDUALINSTALLEDEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(IndividualInstalledEquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all IndividualInstalledEquipment objects 
        /// </summary>
        /// <returns>A list of IndividualInstalledEquipment objects</returns>
		public IndividualInstalledEquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLINDIVIDUALINSTALLEDEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all IndividualInstalledEquipment objects by PageRequest
        /// </summary>
        /// <returns>A list of IndividualInstalledEquipment objects</returns>
		public IndividualInstalledEquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDINDIVIDUALINSTALLEDEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				IndividualInstalledEquipmentList _IndividualInstalledEquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _IndividualInstalledEquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all IndividualInstalledEquipment objects by query String
        /// </summary>
        /// <returns>A list of IndividualInstalledEquipment objects</returns>
		public IndividualInstalledEquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETINDIVIDUALINSTALLEDEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get IndividualInstalledEquipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of IndividualInstalledEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINDIVIDUALINSTALLEDEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get IndividualInstalledEquipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of IndividualInstalledEquipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _IndividualInstalledEquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETINDIVIDUALINSTALLEDEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_IndividualInstalledEquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _IndividualInstalledEquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills IndividualInstalledEquipment object
        /// </summary>
        /// <param name="individualInstalledEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(IndividualInstalledEquipmentBase individualInstalledEquipmentObject, SqlDataReader reader, int start)
		{
			
				individualInstalledEquipmentObject.Id = reader.GetInt32( start + 0 );			
				individualInstalledEquipmentObject.AppointmentEquipmentId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) individualInstalledEquipmentObject.Category = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) individualInstalledEquipmentObject.Manufacturer = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) individualInstalledEquipmentObject.Description = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) individualInstalledEquipmentObject.TicketType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) individualInstalledEquipmentObject.EmpUser = reader.GetString( start + 6 );			
				individualInstalledEquipmentObject.TicketId = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) individualInstalledEquipmentObject.RepliesCount = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) individualInstalledEquipmentObject.AttachmentsCount = reader.GetInt32( start + 9 );			
				individualInstalledEquipmentObject.CusIdInt = reader.GetInt32( start + 10 );			
				if(!reader.IsDBNull(11)) individualInstalledEquipmentObject.CustomerName = reader.GetString( start + 11 );			
				individualInstalledEquipmentObject.CompletionDate = reader.GetDateTime( start + 12 );			
				if(!reader.IsDBNull(13)) individualInstalledEquipmentObject.SKU = reader.GetString( start + 13 );			
				individualInstalledEquipmentObject.TotalPoint = reader.GetDouble( start + 14 );			
				individualInstalledEquipmentObject.IsClosed = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) individualInstalledEquipmentObject.CompanyCost = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) individualInstalledEquipmentObject.CustomerCost = reader.GetDouble( start + 17 );			
				individualInstalledEquipmentObject.Quantity = reader.GetInt32( start + 18 );			
				individualInstalledEquipmentObject.InstalledEquipment = reader.GetInt32( start + 19 );			
				individualInstalledEquipmentObject.Qty = reader.GetInt32( start + 20 );			
				individualInstalledEquipmentObject.Status = reader.GetString( start + 21 );			
				individualInstalledEquipmentObject.CreatedBy = reader.GetGuid( start + 22 );			
				individualInstalledEquipmentObject.CreatedDate = reader.GetDateTime( start + 23 );			
				individualInstalledEquipmentObject.InstalledByUid = reader.GetGuid( start + 24 );			
				individualInstalledEquipmentObject.EquipmentId = reader.GetGuid( start + 25 );			
			FillBaseObject(individualInstalledEquipmentObject, reader, (start + 26));

			
			individualInstalledEquipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills IndividualInstalledEquipment object
        /// </summary>
        /// <param name="individualInstalledEquipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(IndividualInstalledEquipmentBase individualInstalledEquipmentObject, SqlDataReader reader)
		{
			FillObject(individualInstalledEquipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves IndividualInstalledEquipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>IndividualInstalledEquipment object</returns>
		private IndividualInstalledEquipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					IndividualInstalledEquipment individualInstalledEquipmentObject= new IndividualInstalledEquipment();
					FillObject(individualInstalledEquipmentObject, reader);
					return individualInstalledEquipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of IndividualInstalledEquipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of IndividualInstalledEquipment objects</returns>
		private IndividualInstalledEquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//IndividualInstalledEquipment list
			IndividualInstalledEquipmentList list = new IndividualInstalledEquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					IndividualInstalledEquipment individualInstalledEquipmentObject = new IndividualInstalledEquipment();
					FillObject(individualInstalledEquipmentObject, reader);

					list.Add(individualInstalledEquipmentObject);
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
