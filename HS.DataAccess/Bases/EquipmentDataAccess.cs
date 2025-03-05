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
	public partial class EquipmentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENT = "InsertEquipment";
		private const string UPDATEEQUIPMENT = "UpdateEquipment";
		private const string DELETEEQUIPMENT = "DeleteEquipment";
		private const string GETEQUIPMENTBYID = "GetEquipmentById";
		private const string GETALLEQUIPMENT = "GetAllEquipment";
		private const string GETPAGEDEQUIPMENT = "GetPagedEquipment";
		private const string GETEQUIPMENTMAXIMUMID = "GetEquipmentMaximumId";
		private const string GETEQUIPMENTROWCOUNT = "GetEquipmentRowCount";	
		private const string GETEQUIPMENTBYQUERY = "GetEquipmentByQuery";
		#endregion
		
		#region Constructors
		public EquipmentDataAccess(ClientContext context) : base(context) { }
		public EquipmentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentBase equipmentObject)
		{	
			AddParameter(cmd, pGuid(EquipmentBase.Property_EquipmentId, equipmentObject.EquipmentId));
			AddParameter(cmd, pGuid(EquipmentBase.Property_CompanyId, equipmentObject.CompanyId));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Name, 150, equipmentObject.Name));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_SKU, 250, equipmentObject.SKU));
			AddParameter(cmd, pInt32(EquipmentBase.Property_ManufacturerId, equipmentObject.ManufacturerId));
			AddParameter(cmd, pInt32(EquipmentBase.Property_SupplierId, equipmentObject.SupplierId));
			AddParameter(cmd, pInt32(EquipmentBase.Property_EquipmentTypeId, equipmentObject.EquipmentTypeId));
			AddParameter(cmd, pInt32(EquipmentBase.Property_EquipmentClassId, equipmentObject.EquipmentClassId));
			AddParameter(cmd, pDouble(EquipmentBase.Property_Point, equipmentObject.Point));
			AddParameter(cmd, pDouble(EquipmentBase.Property_SupplierCost, equipmentObject.SupplierCost));
			AddParameter(cmd, pDouble(EquipmentBase.Property_Cost, equipmentObject.Cost));
			AddParameter(cmd, pDouble(EquipmentBase.Property_Retail, equipmentObject.Retail));
			AddParameter(cmd, pInt32(EquipmentBase.Property_EqOrder, equipmentObject.EqOrder));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Service, 500, equipmentObject.Service));
			AddParameter(cmd, pDateTime(EquipmentBase.Property_AsOfDate, equipmentObject.AsOfDate));
			AddParameter(cmd, pInt32(EquipmentBase.Property_reorderpoint, equipmentObject.reorderpoint));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsActive, equipmentObject.IsActive));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Comments, equipmentObject.Comments));
			AddParameter(cmd, pDateTime(EquipmentBase.Property_CreatedDate, equipmentObject.CreatedDate));
			AddParameter(cmd, pDateTime(EquipmentBase.Property_LastUpdatedDate, equipmentObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_LastUpdatedBy, 50, equipmentObject.LastUpdatedBy));
			AddParameter(cmd, pBool(EquipmentBase.Property_POOrder, equipmentObject.POOrder));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsKit, equipmentObject.IsKit));
			AddParameter(cmd, pDouble(EquipmentBase.Property_RepCost, equipmentObject.RepCost));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_RackNo, 50, equipmentObject.RackNo));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Location, 50, equipmentObject.Location));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Type, 50, equipmentObject.Type));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Model, 50, equipmentObject.Model));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Finish, 50, equipmentObject.Finish));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Capacity, 50, equipmentObject.Capacity));
			AddParameter(cmd, pDouble(EquipmentBase.Property_EquipmentPrice, equipmentObject.EquipmentPrice));
			AddParameter(cmd, pBool(EquipmentBase.Property_EquipmentPriceIsCharged, equipmentObject.EquipmentPriceIsCharged));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_ModelNumber, 50, equipmentObject.ModelNumber));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Barcode, 50, equipmentObject.Barcode));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Tag, 50, equipmentObject.Tag));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Note, equipmentObject.Note));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsWarrenty, equipmentObject.IsWarrenty));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsARBEnabled, equipmentObject.IsARBEnabled));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsUpsold, equipmentObject.IsUpsold));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsTaxable, equipmentObject.IsTaxable));
			AddParameter(cmd, pDouble(EquipmentBase.Property_OverheadRate, equipmentObject.OverheadRate));
			AddParameter(cmd, pDouble(EquipmentBase.Property_ProfitRate, equipmentObject.ProfitRate));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_Unit, 50, equipmentObject.Unit));
			AddParameter(cmd, pNVarChar(EquipmentBase.Property_TaggedEmail, 100, equipmentObject.TaggedEmail));
			AddParameter(cmd, pBool(EquipmentBase.Property_IsIncludeEstimate, equipmentObject.IsIncludeEstimate));
			AddParameter(cmd, pInt32(EquipmentBase.Property_whreorderpoint, equipmentObject.whreorderpoint));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Equipment
        /// </summary>
        /// <param name="equipmentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentBase equipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENT);
	
				AddParameter(cmd, pInt32Out(EquipmentBase.Property_Id));
				AddCommonParams(cmd, equipmentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentObject.Id = (Int32)GetOutParameter(cmd, EquipmentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Equipment
        /// </summary>
        /// <param name="equipmentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentBase equipmentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENT);
				
				AddParameter(cmd, pInt32(EquipmentBase.Property_Id, equipmentObject.Id));
				AddCommonParams(cmd, equipmentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Equipment
        /// </summary>
        /// <param name="Id">Id of the Equipment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENT);	
				
				AddParameter(cmd, pInt32(EquipmentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Equipment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Equipment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Equipment object to retrieve</param>
        /// <returns>Equipment object, null if not found</returns>
		public Equipment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTBYID))
			{
				AddParameter( cmd, pInt32(EquipmentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Equipment objects 
        /// </summary>
        /// <returns>A list of Equipment objects</returns>
		public EquipmentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Equipment objects by PageRequest
        /// </summary>
        /// <returns>A list of Equipment objects</returns>
		public EquipmentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentList _EquipmentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentList;
			}
		}
		
		/// <summary>
        /// Retrieves all Equipment objects by query String
        /// </summary>
        /// <returns>A list of Equipment objects</returns>
		public EquipmentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Equipment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Equipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Equipment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Equipment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Equipment object
        /// </summary>
        /// <param name="equipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentBase equipmentObject, SqlDataReader reader, int start)
		{
			
				equipmentObject.Id = reader.GetInt32( start + 0 );			
				equipmentObject.EquipmentId = reader.GetGuid( start + 1 );			
				equipmentObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentObject.Name = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) equipmentObject.SKU = reader.GetString( start + 4 );			
				equipmentObject.ManufacturerId = reader.GetInt32( start + 5 );			
				equipmentObject.SupplierId = reader.GetInt32( start + 6 );			
				equipmentObject.EquipmentTypeId = reader.GetInt32( start + 7 );			
				equipmentObject.EquipmentClassId = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) equipmentObject.Point = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) equipmentObject.SupplierCost = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) equipmentObject.Cost = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) equipmentObject.Retail = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) equipmentObject.EqOrder = reader.GetInt32( start + 13 );			
				if(!reader.IsDBNull(14)) equipmentObject.Service = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) equipmentObject.AsOfDate = reader.GetDateTime( start + 15 );			
				if(!reader.IsDBNull(16)) equipmentObject.reorderpoint = reader.GetInt32( start + 16 );			
				equipmentObject.IsActive = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) equipmentObject.Comments = reader.GetString( start + 18 );			
				equipmentObject.CreatedDate = reader.GetDateTime( start + 19 );			
				if(!reader.IsDBNull(20)) equipmentObject.LastUpdatedDate = reader.GetDateTime( start + 20 );			
				if(!reader.IsDBNull(21)) equipmentObject.LastUpdatedBy = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) equipmentObject.POOrder = reader.GetBoolean( start + 22 );			
				if(!reader.IsDBNull(23)) equipmentObject.IsKit = reader.GetBoolean( start + 23 );			
				if(!reader.IsDBNull(24)) equipmentObject.RepCost = reader.GetDouble( start + 24 );			
				if(!reader.IsDBNull(25)) equipmentObject.RackNo = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) equipmentObject.Location = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) equipmentObject.Type = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) equipmentObject.Model = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) equipmentObject.Finish = reader.GetString( start + 29 );			
				if(!reader.IsDBNull(30)) equipmentObject.Capacity = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) equipmentObject.EquipmentPrice = reader.GetDouble( start + 31 );			
				if(!reader.IsDBNull(32)) equipmentObject.EquipmentPriceIsCharged = reader.GetBoolean( start + 32 );			
				if(!reader.IsDBNull(33)) equipmentObject.ModelNumber = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) equipmentObject.Barcode = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) equipmentObject.Tag = reader.GetString( start + 35 );			
				if(!reader.IsDBNull(36)) equipmentObject.Note = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) equipmentObject.IsWarrenty = reader.GetBoolean( start + 37 );			
				if(!reader.IsDBNull(38)) equipmentObject.IsARBEnabled = reader.GetBoolean( start + 38 );			
				if(!reader.IsDBNull(39)) equipmentObject.IsUpsold = reader.GetBoolean( start + 39 );			
				if(!reader.IsDBNull(40)) equipmentObject.IsTaxable = reader.GetBoolean( start + 40 );			
				if(!reader.IsDBNull(41)) equipmentObject.OverheadRate = reader.GetDouble( start + 41 );			
				if(!reader.IsDBNull(42)) equipmentObject.ProfitRate = reader.GetDouble( start + 42 );			
				if(!reader.IsDBNull(43)) equipmentObject.Unit = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) equipmentObject.TaggedEmail = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) equipmentObject.IsIncludeEstimate = reader.GetBoolean( start + 45 );
			if (!reader.IsDBNull(47)) equipmentObject.whreorderpoint = reader.GetInt32(start + 47);
			FillBaseObject(equipmentObject, reader, (start + 48));

			
			equipmentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Equipment object
        /// </summary>
        /// <param name="equipmentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentBase equipmentObject, SqlDataReader reader)
		{
			FillObject(equipmentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Equipment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Equipment object</returns>
		private Equipment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Equipment equipmentObject= new Equipment();
					FillObject(equipmentObject, reader);
					return equipmentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Equipment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Equipment objects</returns>
		private EquipmentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Equipment list
			EquipmentList list = new EquipmentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Equipment equipmentObject = new Equipment();
					FillObject(equipmentObject, reader);

					list.Add(equipmentObject);
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
