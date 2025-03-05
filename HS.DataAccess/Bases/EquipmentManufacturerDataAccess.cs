using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class EquipmentManufacturerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTMANUFACTURER = "InsertEquipmentManufacturer";
		private const string UPDATEEQUIPMENTMANUFACTURER = "UpdateEquipmentManufacturer";
		private const string DELETEEQUIPMENTMANUFACTURER = "DeleteEquipmentManufacturer";
		private const string GETEQUIPMENTMANUFACTURERBYID = "GetEquipmentManufacturerById";
		private const string GETALLEQUIPMENTMANUFACTURER = "GetAllEquipmentManufacturer";
		private const string GETPAGEDEQUIPMENTMANUFACTURER = "GetPagedEquipmentManufacturer";
		private const string GETEQUIPMENTMANUFACTURERMAXIMUMID = "GetEquipmentManufacturerMaximumId";
		private const string GETEQUIPMENTMANUFACTURERROWCOUNT = "GetEquipmentManufacturerRowCount";	
		private const string GETEQUIPMENTMANUFACTURERBYQUERY = "GetEquipmentManufacturerByQuery";
		#endregion
		
		#region Constructors
		public EquipmentManufacturerDataAccess(ClientContext context) : base(context) { }
		public EquipmentManufacturerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentManufacturerObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentManufacturerBase equipmentManufacturerObject)
		{	
			AddParameter(cmd, pGuid(EquipmentManufacturerBase.Property_EquipmentId, equipmentManufacturerObject.EquipmentId));
			AddParameter(cmd, pGuid(EquipmentManufacturerBase.Property_ManufacturerId, equipmentManufacturerObject.ManufacturerId));
			AddParameter(cmd, pNVarChar(EquipmentManufacturerBase.Property_SKU, 50, equipmentManufacturerObject.SKU));
			AddParameter(cmd, pDouble(EquipmentManufacturerBase.Property_Cost, equipmentManufacturerObject.Cost));
			AddParameter(cmd, pBool(EquipmentManufacturerBase.Property_IsPrimary, equipmentManufacturerObject.IsPrimary));
			AddParameter(cmd, pGuid(EquipmentManufacturerBase.Property_AddedBy, equipmentManufacturerObject.AddedBy));
			AddParameter(cmd, pDateTime(EquipmentManufacturerBase.Property_AddedDate, equipmentManufacturerObject.AddedDate));
			AddParameter(cmd, pNVarChar(EquipmentManufacturerBase.Property_Variation, 100, equipmentManufacturerObject.Variation));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentManufacturer
        /// </summary>
        /// <param name="equipmentManufacturerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentManufacturerBase equipmentManufacturerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTMANUFACTURER);
	
				AddParameter(cmd, pInt32Out(EquipmentManufacturerBase.Property_Id));
				AddCommonParams(cmd, equipmentManufacturerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentManufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentManufacturerObject.Id = (Int32)GetOutParameter(cmd, EquipmentManufacturerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentManufacturerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentManufacturer
        /// </summary>
        /// <param name="equipmentManufacturerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentManufacturerBase equipmentManufacturerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTMANUFACTURER);
				
				AddParameter(cmd, pInt32(EquipmentManufacturerBase.Property_Id, equipmentManufacturerObject.Id));
				AddCommonParams(cmd, equipmentManufacturerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentManufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentManufacturerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentManufacturer
        /// </summary>
        /// <param name="Id">Id of the EquipmentManufacturer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTMANUFACTURER);	
				
				AddParameter(cmd, pInt32(EquipmentManufacturerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentManufacturer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentManufacturer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentManufacturer object to retrieve</param>
        /// <returns>EquipmentManufacturer object, null if not found</returns>
		public EquipmentManufacturer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTMANUFACTURERBYID))
			{
				AddParameter( cmd, pInt32(EquipmentManufacturerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentManufacturer objects 
        /// </summary>
        /// <returns>A list of EquipmentManufacturer objects</returns>
		public EquipmentManufacturerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTMANUFACTURER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentManufacturer objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentManufacturer objects</returns>
		public EquipmentManufacturerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTMANUFACTURER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentManufacturerList _EquipmentManufacturerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentManufacturerList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentManufacturer objects by query String
        /// </summary>
        /// <returns>A list of EquipmentManufacturer objects</returns>
		public EquipmentManufacturerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTMANUFACTURERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}

        #endregion


        #region Get EquipmentManufacturer Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of EquipmentManufacturer
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTMANUFACTURERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentManufacturer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentManufacturer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentManufacturerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTMANUFACTURERROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentManufacturerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentManufacturerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentManufacturer object
        /// </summary>
        /// <param name="equipmentManufacturerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentManufacturerBase equipmentManufacturerObject, SqlDataReader reader, int start)
		{
			
				equipmentManufacturerObject.Id = reader.GetInt32( start + 0 );			
				equipmentManufacturerObject.EquipmentId = reader.GetGuid( start + 1 );			
				equipmentManufacturerObject.ManufacturerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentManufacturerObject.SKU = reader.GetString( start + 3 );			
				equipmentManufacturerObject.Cost = reader.GetDouble( start + 4 );			
				equipmentManufacturerObject.IsPrimary = reader.GetBoolean( start + 5 );			
				equipmentManufacturerObject.AddedBy = reader.GetGuid( start + 6 );			
				equipmentManufacturerObject.AddedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) equipmentManufacturerObject.Variation = reader.GetString( start + 8 );			
			FillBaseObject(equipmentManufacturerObject, reader, (start + 9));

			
			equipmentManufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentManufacturer object
        /// </summary>
        /// <param name="equipmentManufacturerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentManufacturerBase equipmentManufacturerObject, SqlDataReader reader)
		{
			FillObject(equipmentManufacturerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentManufacturer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentManufacturer object</returns>
		private EquipmentManufacturer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentManufacturer equipmentManufacturerObject= new EquipmentManufacturer();
					FillObject(equipmentManufacturerObject, reader);
					return equipmentManufacturerObject;
				}
				else
				{
					return null;
				}				
			}
		}

        /// <summary>
        /// Retrieves list of EquipmentManufacturer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentManufacturer objects</returns>
        private EquipmentManufacturerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentManufacturer list
			EquipmentManufacturerList list = new EquipmentManufacturerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentManufacturer equipmentManufacturerObject = new EquipmentManufacturer();
					FillObject(equipmentManufacturerObject, reader);

					list.Add(equipmentManufacturerObject);
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
