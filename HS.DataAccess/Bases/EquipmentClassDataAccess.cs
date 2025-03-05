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
	public partial class EquipmentClassDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTCLASS = "InsertEquipmentClass";
		private const string UPDATEEQUIPMENTCLASS = "UpdateEquipmentClass";
		private const string DELETEEQUIPMENTCLASS = "DeleteEquipmentClass";
		private const string GETEQUIPMENTCLASSBYID = "GetEquipmentClassById";
		private const string GETALLEQUIPMENTCLASS = "GetAllEquipmentClass";
		private const string GETPAGEDEQUIPMENTCLASS = "GetPagedEquipmentClass";
		private const string GETEQUIPMENTCLASSMAXIMUMID = "GetEquipmentClassMaximumId";
		private const string GETEQUIPMENTCLASSROWCOUNT = "GetEquipmentClassRowCount";	
		private const string GETEQUIPMENTCLASSBYQUERY = "GetEquipmentClassByQuery";
		#endregion
		
		#region Constructors
		public EquipmentClassDataAccess(ClientContext context) : base(context) { }
		public EquipmentClassDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentClassObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentClassBase equipmentClassObject)
		{	
			AddParameter(cmd, pGuid(EquipmentClassBase.Property_CompanyId, equipmentClassObject.CompanyId));
			AddParameter(cmd, pNVarChar(EquipmentClassBase.Property_Name, 50, equipmentClassObject.Name));
			AddParameter(cmd, pInt32(EquipmentClassBase.Property_OrderBy, equipmentClassObject.OrderBy));
			AddParameter(cmd, pBool(EquipmentClassBase.Property_IsActive, equipmentClassObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentClass
        /// </summary>
        /// <param name="equipmentClassObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentClassBase equipmentClassObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTCLASS);
	
				AddParameter(cmd, pInt32Out(EquipmentClassBase.Property_Id));
				AddCommonParams(cmd, equipmentClassObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentClassObject.Id = (Int32)GetOutParameter(cmd, EquipmentClassBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentClassObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentClass
        /// </summary>
        /// <param name="equipmentClassObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentClassBase equipmentClassObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTCLASS);
				
				AddParameter(cmd, pInt32(EquipmentClassBase.Property_Id, equipmentClassObject.Id));
				AddCommonParams(cmd, equipmentClassObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentClassObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentClass
        /// </summary>
        /// <param name="Id">Id of the EquipmentClass object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTCLASS);	
				
				AddParameter(cmd, pInt32(EquipmentClassBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentClass), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentClass object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentClass object to retrieve</param>
        /// <returns>EquipmentClass object, null if not found</returns>
		public EquipmentClass Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTCLASSBYID))
			{
				AddParameter( cmd, pInt32(EquipmentClassBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentClass objects 
        /// </summary>
        /// <returns>A list of EquipmentClass objects</returns>
		public EquipmentClassList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTCLASS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentClass objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentClass objects</returns>
		public EquipmentClassList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTCLASS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentClassList _EquipmentClassList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentClassList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentClass objects by query String
        /// </summary>
        /// <returns>A list of EquipmentClass objects</returns>
		public EquipmentClassList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTCLASSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentClass Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentClass
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTCLASSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentClass Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentClass
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentClassRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTCLASSROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentClassRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentClassRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentClass object
        /// </summary>
        /// <param name="equipmentClassObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentClassBase equipmentClassObject, SqlDataReader reader, int start)
		{
			
				equipmentClassObject.Id = reader.GetInt32( start + 0 );			
				equipmentClassObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) equipmentClassObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentClassObject.OrderBy = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) equipmentClassObject.IsActive = reader.GetBoolean( start + 4 );			
			FillBaseObject(equipmentClassObject, reader, (start + 5));

			
			equipmentClassObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentClass object
        /// </summary>
        /// <param name="equipmentClassObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentClassBase equipmentClassObject, SqlDataReader reader)
		{
			FillObject(equipmentClassObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentClass object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentClass object</returns>
		private EquipmentClass GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentClass equipmentClassObject= new EquipmentClass();
					FillObject(equipmentClassObject, reader);
					return equipmentClassObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentClass objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentClass objects</returns>
		private EquipmentClassList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentClass list
			EquipmentClassList list = new EquipmentClassList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentClass equipmentClassObject = new EquipmentClass();
					FillObject(equipmentClassObject, reader);

					list.Add(equipmentClassObject);
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
