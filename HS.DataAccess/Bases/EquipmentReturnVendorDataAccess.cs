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
	public partial class EquipmentReturnVendorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTRETURNVENDOR = "InsertEquipmentReturnVendor";
		private const string UPDATEEQUIPMENTRETURNVENDOR = "UpdateEquipmentReturnVendor";
		private const string DELETEEQUIPMENTRETURNVENDOR = "DeleteEquipmentReturnVendor";
		private const string GETEQUIPMENTRETURNVENDORBYID = "GetEquipmentReturnVendorById";
		private const string GETALLEQUIPMENTRETURNVENDOR = "GetAllEquipmentReturnVendor";
		private const string GETPAGEDEQUIPMENTRETURNVENDOR = "GetPagedEquipmentReturnVendor";
		private const string GETEQUIPMENTRETURNVENDORMAXIMUMID = "GetEquipmentReturnVendorMaximumId";
		private const string GETEQUIPMENTRETURNVENDORROWCOUNT = "GetEquipmentReturnVendorRowCount";	
		private const string GETEQUIPMENTRETURNVENDORBYQUERY = "GetEquipmentReturnVendorByQuery";
		#endregion
		
		#region Constructors
		public EquipmentReturnVendorDataAccess(ClientContext context) : base(context) { }
		public EquipmentReturnVendorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentReturnVendorObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentReturnVendorBase equipmentReturnVendorObject)
		{	
			AddParameter(cmd, pGuid(EquipmentReturnVendorBase.Property_CompanyId, equipmentReturnVendorObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentReturnVendorBase.Property_ReturnId, equipmentReturnVendorObject.ReturnId));
			AddParameter(cmd, pNVarChar(EquipmentReturnVendorBase.Property_Status, 50, equipmentReturnVendorObject.Status));
			AddParameter(cmd, pNVarChar(EquipmentReturnVendorBase.Property_Description, equipmentReturnVendorObject.Description));
			AddParameter(cmd, pGuid(EquipmentReturnVendorBase.Property_LastUpdatedBy, equipmentReturnVendorObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(EquipmentReturnVendorBase.Property_LastUpdatedDate, equipmentReturnVendorObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentReturnVendor
        /// </summary>
        /// <param name="equipmentReturnVendorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentReturnVendorBase equipmentReturnVendorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTRETURNVENDOR);
	
				AddParameter(cmd, pInt32Out(EquipmentReturnVendorBase.Property_Id));
				AddCommonParams(cmd, equipmentReturnVendorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentReturnVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentReturnVendorObject.Id = (Int32)GetOutParameter(cmd, EquipmentReturnVendorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentReturnVendorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentReturnVendor
        /// </summary>
        /// <param name="equipmentReturnVendorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentReturnVendorBase equipmentReturnVendorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTRETURNVENDOR);
				
				AddParameter(cmd, pInt32(EquipmentReturnVendorBase.Property_Id, equipmentReturnVendorObject.Id));
				AddCommonParams(cmd, equipmentReturnVendorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentReturnVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentReturnVendorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentReturnVendor
        /// </summary>
        /// <param name="Id">Id of the EquipmentReturnVendor object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTRETURNVENDOR);	
				
				AddParameter(cmd, pInt32(EquipmentReturnVendorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentReturnVendor), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentReturnVendor object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentReturnVendor object to retrieve</param>
        /// <returns>EquipmentReturnVendor object, null if not found</returns>
		public EquipmentReturnVendor Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNVENDORBYID))
			{
				AddParameter( cmd, pInt32(EquipmentReturnVendorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentReturnVendor objects 
        /// </summary>
        /// <returns>A list of EquipmentReturnVendor objects</returns>
		public EquipmentReturnVendorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTRETURNVENDOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentReturnVendor objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentReturnVendor objects</returns>
		public EquipmentReturnVendorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTRETURNVENDOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentReturnVendorList _EquipmentReturnVendorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentReturnVendorList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentReturnVendor objects by query String
        /// </summary>
        /// <returns>A list of EquipmentReturnVendor objects</returns>
		public EquipmentReturnVendorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNVENDORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentReturnVendor Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentReturnVendor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNVENDORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentReturnVendor Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentReturnVendor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentReturnVendorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTRETURNVENDORROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentReturnVendorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentReturnVendorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentReturnVendor object
        /// </summary>
        /// <param name="equipmentReturnVendorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentReturnVendorBase equipmentReturnVendorObject, SqlDataReader reader, int start)
		{
			
				equipmentReturnVendorObject.Id = reader.GetInt32( start + 0 );			
				equipmentReturnVendorObject.CompanyId = reader.GetGuid( start + 1 );			
				equipmentReturnVendorObject.ReturnId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentReturnVendorObject.Status = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) equipmentReturnVendorObject.Description = reader.GetString( start + 4 );			
				equipmentReturnVendorObject.LastUpdatedBy = reader.GetGuid( start + 5 );			
				equipmentReturnVendorObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(equipmentReturnVendorObject, reader, (start + 7));

			
			equipmentReturnVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentReturnVendor object
        /// </summary>
        /// <param name="equipmentReturnVendorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentReturnVendorBase equipmentReturnVendorObject, SqlDataReader reader)
		{
			FillObject(equipmentReturnVendorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentReturnVendor object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentReturnVendor object</returns>
		private EquipmentReturnVendor GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentReturnVendor equipmentReturnVendorObject= new EquipmentReturnVendor();
					FillObject(equipmentReturnVendorObject, reader);
					return equipmentReturnVendorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentReturnVendor objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentReturnVendor objects</returns>
		private EquipmentReturnVendorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentReturnVendor list
			EquipmentReturnVendorList list = new EquipmentReturnVendorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentReturnVendor equipmentReturnVendorObject = new EquipmentReturnVendor();
					FillObject(equipmentReturnVendorObject, reader);

					list.Add(equipmentReturnVendorObject);
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
