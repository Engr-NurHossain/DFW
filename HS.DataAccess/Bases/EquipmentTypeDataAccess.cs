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
	public partial class EquipmentTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTTYPE = "InsertEquipmentType";
		private const string UPDATEEQUIPMENTTYPE = "UpdateEquipmentType";
		private const string DELETEEQUIPMENTTYPE = "DeleteEquipmentType";
		private const string GETEQUIPMENTTYPEBYID = "GetEquipmentTypeById";
		private const string GETALLEQUIPMENTTYPE = "GetAllEquipmentType";
		private const string GETPAGEDEQUIPMENTTYPE = "GetPagedEquipmentType";
		private const string GETEQUIPMENTTYPEMAXIMUMID = "GetEquipmentTypeMaximumId";
		private const string GETEQUIPMENTTYPEROWCOUNT = "GetEquipmentTypeRowCount";	
		private const string GETEQUIPMENTTYPEBYQUERY = "GetEquipmentTypeByQuery";
		#endregion
		
		#region Constructors
		public EquipmentTypeDataAccess(ClientContext context) : base(context) { }
		public EquipmentTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentTypeBase equipmentTypeObject)
		{	
			AddParameter(cmd, pGuid(EquipmentTypeBase.Property_CompanyId, equipmentTypeObject.CompanyId));
			AddParameter(cmd, pNVarChar(EquipmentTypeBase.Property_Name, 50, equipmentTypeObject.Name));
			AddParameter(cmd, pInt32(EquipmentTypeBase.Property_OrderBy, equipmentTypeObject.OrderBy));
			AddParameter(cmd, pBool(EquipmentTypeBase.Property_IsActive, equipmentTypeObject.IsActive));
			AddParameter(cmd, pInt32(EquipmentTypeBase.Property_ParentId, equipmentTypeObject.ParentId));
			AddParameter(cmd, pDateTime(EquipmentTypeBase.Property_LastUpdatedDate, equipmentTypeObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(EquipmentTypeBase.Property_LastUpdatedBy, 50, equipmentTypeObject.LastUpdatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentType
        /// </summary>
        /// <param name="equipmentTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentTypeBase equipmentTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTTYPE);
	
				AddParameter(cmd, pInt32Out(EquipmentTypeBase.Property_Id));
				AddCommonParams(cmd, equipmentTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentTypeObject.Id = (Int32)GetOutParameter(cmd, EquipmentTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentType
        /// </summary>
        /// <param name="equipmentTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentTypeBase equipmentTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTTYPE);
				
				AddParameter(cmd, pInt32(EquipmentTypeBase.Property_Id, equipmentTypeObject.Id));
				AddCommonParams(cmd, equipmentTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentType
        /// </summary>
        /// <param name="Id">Id of the EquipmentType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTTYPE);	
				
				AddParameter(cmd, pInt32(EquipmentTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentType object to retrieve</param>
        /// <returns>EquipmentType object, null if not found</returns>
		public EquipmentType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTYPEBYID))
			{
				AddParameter( cmd, pInt32(EquipmentTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentType objects 
        /// </summary>
        /// <returns>A list of EquipmentType objects</returns>
		public EquipmentTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentType objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentType objects</returns>
		public EquipmentTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentTypeList _EquipmentTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentType objects by query String
        /// </summary>
        /// <returns>A list of EquipmentType objects</returns>
		public EquipmentTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentType object
        /// </summary>
        /// <param name="equipmentTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentTypeBase equipmentTypeObject, SqlDataReader reader, int start)
		{
			
				equipmentTypeObject.Id = reader.GetInt32( start + 0 );			
				equipmentTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) equipmentTypeObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentTypeObject.OrderBy = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) equipmentTypeObject.IsActive = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) equipmentTypeObject.ParentId = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) equipmentTypeObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) equipmentTypeObject.LastUpdatedBy = reader.GetString( start + 7 );			
			FillBaseObject(equipmentTypeObject, reader, (start + 8));

			
			equipmentTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentType object
        /// </summary>
        /// <param name="equipmentTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentTypeBase equipmentTypeObject, SqlDataReader reader)
		{
			FillObject(equipmentTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentType object</returns>
		private EquipmentType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentType equipmentTypeObject= new EquipmentType();
					FillObject(equipmentTypeObject, reader);
					return equipmentTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentType objects</returns>
		private EquipmentTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentType list
			EquipmentTypeList list = new EquipmentTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentType equipmentTypeObject = new EquipmentType();
					FillObject(equipmentTypeObject, reader);

					list.Add(equipmentTypeObject);
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
