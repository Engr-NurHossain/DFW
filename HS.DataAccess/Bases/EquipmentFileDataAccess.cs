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
	public partial class EquipmentFileDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEQUIPMENTFILE = "InsertEquipmentFile";
		private const string UPDATEEQUIPMENTFILE = "UpdateEquipmentFile";
		private const string DELETEEQUIPMENTFILE = "DeleteEquipmentFile";
		private const string GETEQUIPMENTFILEBYID = "GetEquipmentFileById";
		private const string GETALLEQUIPMENTFILE = "GetAllEquipmentFile";
		private const string GETPAGEDEQUIPMENTFILE = "GetPagedEquipmentFile";
		private const string GETEQUIPMENTFILEMAXIMUMID = "GetEquipmentFileMaximumId";
		private const string GETEQUIPMENTFILEROWCOUNT = "GetEquipmentFileRowCount";	
		private const string GETEQUIPMENTFILEBYQUERY = "GetEquipmentFileByQuery";
		#endregion
		
		#region Constructors
		public EquipmentFileDataAccess(ClientContext context) : base(context) { }
		public EquipmentFileDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="equipmentFileObject"></param>
		private void AddCommonParams(SqlCommand cmd, EquipmentFileBase equipmentFileObject)
		{	
			AddParameter(cmd, pGuid(EquipmentFileBase.Property_CompanyId, equipmentFileObject.CompanyId));
			AddParameter(cmd, pGuid(EquipmentFileBase.Property_EquipmentId, equipmentFileObject.EquipmentId));
			AddParameter(cmd, pNVarChar(EquipmentFileBase.Property_FileDescription, equipmentFileObject.FileDescription));
			AddParameter(cmd, pNVarChar(EquipmentFileBase.Property_Filename, 500, equipmentFileObject.Filename));
			AddParameter(cmd, pNVarChar(EquipmentFileBase.Property_FileFullName, 500, equipmentFileObject.FileFullName));
			AddParameter(cmd, pNVarChar(EquipmentFileBase.Property_FileType, 50, equipmentFileObject.FileType));
			AddParameter(cmd, pDateTime(EquipmentFileBase.Property_Uploadeddate, equipmentFileObject.Uploadeddate));
			AddParameter(cmd, pBool(EquipmentFileBase.Property_IsActive, equipmentFileObject.IsActive));
			AddParameter(cmd, pDouble(EquipmentFileBase.Property_FileSize, equipmentFileObject.FileSize));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EquipmentFile
        /// </summary>
        /// <param name="equipmentFileObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EquipmentFileBase equipmentFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEQUIPMENTFILE);
	
				AddParameter(cmd, pInt32Out(EquipmentFileBase.Property_Id));
				AddCommonParams(cmd, equipmentFileObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					equipmentFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					equipmentFileObject.Id = (Int32)GetOutParameter(cmd, EquipmentFileBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(equipmentFileObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EquipmentFile
        /// </summary>
        /// <param name="equipmentFileObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EquipmentFileBase equipmentFileObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEQUIPMENTFILE);
				
				AddParameter(cmd, pInt32(EquipmentFileBase.Property_Id, equipmentFileObject.Id));
				AddCommonParams(cmd, equipmentFileObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					equipmentFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(equipmentFileObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EquipmentFile
        /// </summary>
        /// <param name="Id">Id of the EquipmentFile object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEQUIPMENTFILE);	
				
				AddParameter(cmd, pInt32(EquipmentFileBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EquipmentFile), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EquipmentFile object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EquipmentFile object to retrieve</param>
        /// <returns>EquipmentFile object, null if not found</returns>
		public EquipmentFile Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTFILEBYID))
			{
				AddParameter( cmd, pInt32(EquipmentFileBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EquipmentFile objects 
        /// </summary>
        /// <returns>A list of EquipmentFile objects</returns>
		public EquipmentFileList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEQUIPMENTFILE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EquipmentFile objects by PageRequest
        /// </summary>
        /// <returns>A list of EquipmentFile objects</returns>
		public EquipmentFileList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEQUIPMENTFILE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EquipmentFileList _EquipmentFileList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EquipmentFileList;
			}
		}
		
		/// <summary>
        /// Retrieves all EquipmentFile objects by query String
        /// </summary>
        /// <returns>A list of EquipmentFile objects</returns>
		public EquipmentFileList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTFILEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EquipmentFile Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EquipmentFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTFILEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EquipmentFile Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EquipmentFile
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EquipmentFileRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEQUIPMENTFILEROWCOUNT))
			{
				SqlDataReader reader;
				_EquipmentFileRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EquipmentFileRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EquipmentFile object
        /// </summary>
        /// <param name="equipmentFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EquipmentFileBase equipmentFileObject, SqlDataReader reader, int start)
		{
			
				equipmentFileObject.Id = reader.GetInt32( start + 0 );			
				equipmentFileObject.CompanyId = reader.GetGuid( start + 1 );			
				equipmentFileObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) equipmentFileObject.FileDescription = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) equipmentFileObject.Filename = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) equipmentFileObject.FileFullName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) equipmentFileObject.FileType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) equipmentFileObject.Uploadeddate = reader.GetDateTime( start + 7 );			
				equipmentFileObject.IsActive = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) equipmentFileObject.FileSize = reader.GetDouble( start + 9 );			
			FillBaseObject(equipmentFileObject, reader, (start + 10));

			
			equipmentFileObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EquipmentFile object
        /// </summary>
        /// <param name="equipmentFileObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EquipmentFileBase equipmentFileObject, SqlDataReader reader)
		{
			FillObject(equipmentFileObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EquipmentFile object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EquipmentFile object</returns>
		private EquipmentFile GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EquipmentFile equipmentFileObject= new EquipmentFile();
					FillObject(equipmentFileObject, reader);
					return equipmentFileObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EquipmentFile objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EquipmentFile objects</returns>
		private EquipmentFileList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EquipmentFile list
			EquipmentFileList list = new EquipmentFileList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EquipmentFile equipmentFileObject = new EquipmentFile();
					FillObject(equipmentFileObject, reader);

					list.Add(equipmentFileObject);
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
