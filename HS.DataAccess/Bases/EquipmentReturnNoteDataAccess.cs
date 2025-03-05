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
	public partial class EquipmentReturnNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTRETURNNOTE = "InsertEquipmentReturnNote";
		private const string UPDATEEQUIPMENTRETURNNOTE = "UpdateEquipmentReturnNote";
		private const string DELETEEQUIPMENTRETURNNOTE = "DeleteEquipmentReturnNote";
		private const string GETEQUIPMENTRETURNNOTEBYID = "GetEquipmentReturnNoteById";
		private const string GETALLEQUIPMENTRETURNNOTE = "GetAllEquipmentReturnNote";
		private const string GETPAGEDEQUIPMENTRETURNNOTE = "GetPagedEquipmentReturnNote";
		private const string GETEQUIPMENTRETURNNOTEMAXIMUMID = "GetEquipmentReturnNoteMaximumId";
		private const string GETEQUIPMENTRETURNNOTEROWCOUNT = "GetEquipmentReturnNoteRowCount";	
		private const string GETEQUIPMENTRETURNNOTEBYQUERY = "GetEquipmentReturnNoteByQuery";
		#endregion
		
		#region Constructors
		public EquipmentReturnNoteDataAccess(ClientContext context) : base(context) { }
		public EquipmentReturnNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentReturnNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentReturnNoteBase equipmentReturnNoteObject)
		{	
			AddParameter(cmd, pGuid(EquipmentReturnNoteBase.Property_CompanyId, equipmentReturnNoteObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentReturnNoteBase.Property_ReturnId, equipmentReturnNoteObject.ReturnId));
			AddParameter(cmd, pNVarChar(EquipmentReturnNoteBase.Property_Description, equipmentReturnNoteObject.Description));
			AddParameter(cmd, pGuid(EquipmentReturnNoteBase.Property_LastUpdatedBy, equipmentReturnNoteObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EquipmentReturnNoteBase.Property_LastUpdatedDate, equipmentReturnNoteObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentReturnNote
        /// </summary>
        /// <param name="equipmentReturnNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentReturnNoteBase equipmentReturnNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTRETURNNOTE);
	
				AddParameter(cmd, pInt32Out(EquipmentReturnNoteBase.Property_Id));
				AddCommonParams(cmd, equipmentReturnNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentReturnNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentReturnNoteObject.Id = (Int32)GetOutParameter(cmd, EquipmentReturnNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentReturnNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentReturnNote
        /// </summary>
        /// <param name="equipmentReturnNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentReturnNoteBase equipmentReturnNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTRETURNNOTE);
				
				AddParameter(cmd, pInt32(EquipmentReturnNoteBase.Property_Id, equipmentReturnNoteObject.Id));
				AddCommonParams(cmd, equipmentReturnNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentReturnNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentReturnNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentReturnNote
        /// </summary>
        /// <param name="Id">Id of the EquipmentReturnNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTRETURNNOTE);	
				
				AddParameter(cmd, pInt32(EquipmentReturnNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentReturnNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentReturnNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentReturnNote object to retrieve</param>
        /// <returns>EquipmentReturnNote object, null if not found</returns>
		public EquipmentReturnNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNNOTEBYID))
			{
				AddParameter( cmd, pInt32(EquipmentReturnNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentReturnNote objects 
        /// </summary>
        /// <returns>A list of EquipmentReturnNote objects</returns>
		public EquipmentReturnNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTRETURNNOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentReturnNote objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentReturnNote objects</returns>
		public EquipmentReturnNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTRETURNNOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentReturnNoteList _EquipmentReturnNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentReturnNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentReturnNote objects by query String
        /// </summary>
        /// <returns>A list of EquipmentReturnNote objects</returns>
		public EquipmentReturnNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNNOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentReturnNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentReturnNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNNOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentReturnNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentReturnNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentReturnNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNNOTEROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentReturnNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentReturnNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentReturnNote object
        /// </summary>
        /// <param name="equipmentReturnNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentReturnNoteBase equipmentReturnNoteObject, SqlDataReader reader, int start)
		{
			
				equipmentReturnNoteObject.Id = reader.GetInt32( start + 0 );			
				equipmentReturnNoteObject.CompanyId = reader.GetGuid( start + 1 );			
				equipmentReturnNoteObject.ReturnId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentReturnNoteObject.Description = reader.GetString( start + 3 );			
				equipmentReturnNoteObject.LastUpdatedBy = reader.GetGuid( start + 4 );			
				equipmentReturnNoteObject.LastUpdatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(equipmentReturnNoteObject, reader, (start + 6));

			
			equipmentReturnNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentReturnNote object
        /// </summary>
        /// <param name="equipmentReturnNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentReturnNoteBase equipmentReturnNoteObject, SqlDataReader reader)
		{
			FillObject(equipmentReturnNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentReturnNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentReturnNote object</returns>
		private EquipmentReturnNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentReturnNote equipmentReturnNoteObject= new EquipmentReturnNote();
					FillObject(equipmentReturnNoteObject, reader);
					return equipmentReturnNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentReturnNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentReturnNote objects</returns>
		private EquipmentReturnNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentReturnNote list
			EquipmentReturnNoteList list = new EquipmentReturnNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentReturnNote equipmentReturnNoteObject = new EquipmentReturnNote();
					FillObject(equipmentReturnNoteObject, reader);

					list.Add(equipmentReturnNoteObject);
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
