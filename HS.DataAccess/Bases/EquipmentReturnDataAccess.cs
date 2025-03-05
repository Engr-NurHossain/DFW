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
	public partial class EquipmentReturnDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTRETURN = "InsertEquipmentReturn";
		private const string UPDATEEQUIPMENTRETURN = "UpdateEquipmentReturn";
		private const string DELETEEQUIPMENTRETURN = "DeleteEquipmentReturn";
		private const string GETEQUIPMENTRETURNBYID = "GetEquipmentReturnById";
		private const string GETALLEQUIPMENTRETURN = "GetAllEquipmentReturn";
		private const string GETPAGEDEQUIPMENTRETURN = "GetPagedEquipmentReturn";
		private const string GETEQUIPMENTRETURNMAXIMUMID = "GetEquipmentReturnMaximumId";
		private const string GETEQUIPMENTRETURNROWCOUNT = "GetEquipmentReturnRowCount";	
		private const string GETEQUIPMENTRETURNBYQUERY = "GetEquipmentReturnByQuery";
		#endregion
		
		#region Constructors
		public EquipmentReturnDataAccess(ClientContext context) : base(context) { }
		public EquipmentReturnDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentReturnObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentReturnBase equipmentReturnObject)
		{	
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_CompanyId, equipmentReturnObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_ReturnId, equipmentReturnObject.ReturnId));
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_CustomerId, equipmentReturnObject.CustomerId));
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_TechnicianId, equipmentReturnObject.TechnicianId));
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_EquipmentId, equipmentReturnObject.EquipmentId));
			AddParameter(cmd, pInt32(EquipmentReturnBase.Property_Quantity, equipmentReturnObject.Quantity));
			AddParameter(cmd, pNVarChar(EquipmentReturnBase.Property_InvoiceNo, 50, equipmentReturnObject.InvoiceNo));
			AddParameter(cmd, pDateTime(EquipmentReturnBase.Property_PurchaseDate, equipmentReturnObject.PurchaseDate));
			AddParameter(cmd, pBool(EquipmentReturnBase.Property_WanrantyAvailable, equipmentReturnObject.WanrantyAvailable));
			AddParameter(cmd, pNVarChar(EquipmentReturnBase.Property_Description, equipmentReturnObject.Description));
			AddParameter(cmd, pNVarChar(EquipmentReturnBase.Property_Status, 50, equipmentReturnObject.Status));
			AddParameter(cmd, pGuid(EquipmentReturnBase.Property_LastUpdatedBy, equipmentReturnObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EquipmentReturnBase.Property_LastUpdatedDate, equipmentReturnObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(EquipmentReturnBase.Property_Reason, 150, equipmentReturnObject.Reason));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentReturn
        /// </summary>
        /// <param name="equipmentReturnObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentReturnBase equipmentReturnObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTRETURN);
	
				AddParameter(cmd, pInt32Out(EquipmentReturnBase.Property_Id));
				AddCommonParams(cmd, equipmentReturnObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentReturnObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentReturnObject.Id = (Int32)GetOutParameter(cmd, EquipmentReturnBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentReturnObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentReturn
        /// </summary>
        /// <param name="equipmentReturnObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentReturnBase equipmentReturnObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTRETURN);
				
				AddParameter(cmd, pInt32(EquipmentReturnBase.Property_Id, equipmentReturnObject.Id));
				AddCommonParams(cmd, equipmentReturnObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentReturnObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentReturnObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentReturn
        /// </summary>
        /// <param name="Id">Id of the EquipmentReturn object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTRETURN);	
				
				AddParameter(cmd, pInt32(EquipmentReturnBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentReturn), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentReturn object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentReturn object to retrieve</param>
        /// <returns>EquipmentReturn object, null if not found</returns>
		public EquipmentReturn Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNBYID))
			{
				AddParameter( cmd, pInt32(EquipmentReturnBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentReturn objects 
        /// </summary>
        /// <returns>A list of EquipmentReturn objects</returns>
		public EquipmentReturnList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTRETURN))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentReturn objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentReturn objects</returns>
		public EquipmentReturnList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTRETURN))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentReturnList _EquipmentReturnList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentReturnList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentReturn objects by query String
        /// </summary>
        /// <returns>A list of EquipmentReturn objects</returns>
		public EquipmentReturnList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentReturn Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentReturn
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentReturn Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentReturn
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentReturnRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentReturnRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentReturnRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentReturn object
        /// </summary>
        /// <param name="equipmentReturnObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentReturnBase equipmentReturnObject, SqlDataReader reader, int start)
		{
			
				equipmentReturnObject.Id = reader.GetInt32( start + 0 );			
				equipmentReturnObject.CompanyId = reader.GetGuid( start + 1 );			
				equipmentReturnObject.ReturnId = reader.GetGuid( start + 2 );			
				equipmentReturnObject.CustomerId = reader.GetGuid( start + 3 );			
				equipmentReturnObject.TechnicianId = reader.GetGuid( start + 4 );			
				equipmentReturnObject.EquipmentId = reader.GetGuid( start + 5 );			
				equipmentReturnObject.Quantity = reader.GetInt32( start + 6 );			
				if(!reader.IsDBNull(7)) equipmentReturnObject.InvoiceNo = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) equipmentReturnObject.PurchaseDate = reader.GetDateTime( start + 8 );			
				equipmentReturnObject.WanrantyAvailable = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) equipmentReturnObject.Description = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) equipmentReturnObject.Status = reader.GetString( start + 11 );			
				equipmentReturnObject.LastUpdatedBy = reader.GetGuid( start + 12 );			
				equipmentReturnObject.LastUpdatedDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) equipmentReturnObject.Reason = reader.GetString( start + 14 );			
			FillBaseObject(equipmentReturnObject, reader, (start + 15));

			
			equipmentReturnObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentReturn object
        /// </summary>
        /// <param name="equipmentReturnObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentReturnBase equipmentReturnObject, SqlDataReader reader)
		{
			FillObject(equipmentReturnObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentReturn object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentReturn object</returns>
		private EquipmentReturn GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentReturn equipmentReturnObject= new EquipmentReturn();
					FillObject(equipmentReturnObject, reader);
					return equipmentReturnObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentReturn objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentReturn objects</returns>
		private EquipmentReturnList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentReturn list
			EquipmentReturnList list = new EquipmentReturnList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentReturn equipmentReturnObject = new EquipmentReturn();
					FillObject(equipmentReturnObject, reader);

					list.Add(equipmentReturnObject);
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
