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
	public partial class EquipmentVendorDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTVENDOR = "InsertEquipmentVendor";
		private const string UPDATEEQUIPMENTVENDOR = "UpdateEquipmentVendor";
		private const string DELETEEQUIPMENTVENDOR = "DeleteEquipmentVendor";
		private const string GETEQUIPMENTVENDORBYID = "GetEquipmentVendorById";
		private const string GETALLEQUIPMENTVENDOR = "GetAllEquipmentVendor";
		private const string GETPAGEDEQUIPMENTVENDOR = "GetPagedEquipmentVendor";
		private const string GETEQUIPMENTVENDORMAXIMUMID = "GetEquipmentVendorMaximumId";
		private const string GETEQUIPMENTVENDORROWCOUNT = "GetEquipmentVendorRowCount";	
		private const string GETEQUIPMENTVENDORBYQUERY = "GetEquipmentVendorByQuery";
		#endregion
		
		#region Constructors
		public EquipmentVendorDataAccess(ClientContext context) : base(context) { }
		public EquipmentVendorDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentVendorObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentVendorBase equipmentVendorObject)
		{	
			AddParameter(cmd, pGuid(EquipmentVendorBase.Property_EquipmentId, equipmentVendorObject.EquipmentId));
			AddParameter(cmd, pGuid(EquipmentVendorBase.Property_SupplierId, equipmentVendorObject.SupplierId));
			AddParameter(cmd, pDouble(EquipmentVendorBase.Property_Cost, equipmentVendorObject.Cost));
			AddParameter(cmd, pBool(EquipmentVendorBase.Property_IsPrimary, equipmentVendorObject.IsPrimary));
			AddParameter(cmd, pGuid(EquipmentVendorBase.Property_AddedBy, equipmentVendorObject.AddedBy));
			AddParameter(cmd, pDateTime(EquipmentVendorBase.Property_AddedDate, equipmentVendorObject.AddedDate));
			AddParameter(cmd, pNVarChar(EquipmentVendorBase.Property_SKU, 50, equipmentVendorObject.SKU));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentVendor
        /// </summary>
        /// <param name="equipmentVendorObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentVendorBase equipmentVendorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTVENDOR);
	
				AddParameter(cmd, pInt32Out(EquipmentVendorBase.Property_Id));
				AddCommonParams(cmd, equipmentVendorObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentVendorObject.Id = (Int32)GetOutParameter(cmd, EquipmentVendorBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentVendorObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentVendor
        /// </summary>
        /// <param name="equipmentVendorObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentVendorBase equipmentVendorObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTVENDOR);
				
				AddParameter(cmd, pInt32(EquipmentVendorBase.Property_Id, equipmentVendorObject.Id));
				AddCommonParams(cmd, equipmentVendorObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentVendorObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentVendor
        /// </summary>
        /// <param name="Id">Id of the EquipmentVendor object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTVENDOR);	
				
				AddParameter(cmd, pInt32(EquipmentVendorBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentVendor), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentVendor object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentVendor object to retrieve</param>
        /// <returns>EquipmentVendor object, null if not found</returns>
		public EquipmentVendor Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTVENDORBYID))
			{
				AddParameter( cmd, pInt32(EquipmentVendorBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentVendor objects 
        /// </summary>
        /// <returns>A list of EquipmentVendor objects</returns>
		public EquipmentVendorList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTVENDOR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentVendor objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentVendor objects</returns>
		public EquipmentVendorList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTVENDOR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentVendorList _EquipmentVendorList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentVendorList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentVendor objects by query String
        /// </summary>
        /// <returns>A list of EquipmentVendor objects</returns>
		public EquipmentVendorList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTVENDORBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentVendor Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentVendor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTVENDORMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentVendor Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentVendor
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentVendorRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTVENDORROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentVendorRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentVendorRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentVendor object
        /// </summary>
        /// <param name="equipmentVendorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentVendorBase equipmentVendorObject, SqlDataReader reader, int start)
		{
			
				equipmentVendorObject.Id = reader.GetInt32( start + 0 );			
				equipmentVendorObject.EquipmentId = reader.GetGuid( start + 1 );			
				equipmentVendorObject.SupplierId = reader.GetGuid( start + 2 );			
				equipmentVendorObject.Cost = reader.GetDouble( start + 3 );			
				equipmentVendorObject.IsPrimary = reader.GetBoolean( start + 4 );			
				equipmentVendorObject.AddedBy = reader.GetGuid( start + 5 );			
				equipmentVendorObject.AddedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) equipmentVendorObject.SKU = reader.GetString( start + 7 );			
			FillBaseObject(equipmentVendorObject, reader, (start + 8));

			
			equipmentVendorObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentVendor object
        /// </summary>
        /// <param name="equipmentVendorObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentVendorBase equipmentVendorObject, SqlDataReader reader)
		{
			FillObject(equipmentVendorObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentVendor object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentVendor object</returns>
		private EquipmentVendor GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentVendor equipmentVendorObject= new EquipmentVendor();
					FillObject(equipmentVendorObject, reader);
					return equipmentVendorObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentVendor objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentVendor objects</returns>
		private EquipmentVendorList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentVendor list
			EquipmentVendorList list = new EquipmentVendorList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentVendor equipmentVendorObject = new EquipmentVendor();
					FillObject(equipmentVendorObject, reader);

					list.Add(equipmentVendorObject);
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
