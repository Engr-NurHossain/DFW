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
	public partial class PanelTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPANELTYPE = "InsertPanelType";
		private const string UPDATEPANELTYPE = "UpdatePanelType";
		private const string DELETEPANELTYPE = "DeletePanelType";
		private const string GETPANELTYPEBYID = "GetPanelTypeById";
		private const string GETALLPANELTYPE = "GetAllPanelType";
		private const string GETPAGEDPANELTYPE = "GetPagedPanelType";
		private const string GETPANELTYPEMAXIMUMID = "GetPanelTypeMaximumId";
		private const string GETPANELTYPEROWCOUNT = "GetPanelTypeRowCount";	
		private const string GETPANELTYPEBYQUERY = "GetPanelTypeByQuery";
		#endregion
		
		#region Constructors
		public PanelTypeDataAccess(ClientContext context) : base(context) { }
		public PanelTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="panelTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PanelTypeBase panelTypeObject)
		{	
			AddParameter(cmd, pGuid(PanelTypeBase.Property_CompanyId, panelTypeObject.CompanyId));
			AddParameter(cmd, pNVarChar(PanelTypeBase.Property_Name, 50, panelTypeObject.Name));
			AddParameter(cmd, pNVarChar(PanelTypeBase.Property_Value, 50, panelTypeObject.Value));
			AddParameter(cmd, pBool(PanelTypeBase.Property_IsActive, panelTypeObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PanelType
        /// </summary>
        /// <param name="panelTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PanelTypeBase panelTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPANELTYPE);
	
				AddParameter(cmd, pInt32Out(PanelTypeBase.Property_Id));
				AddCommonParams(cmd, panelTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					panelTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					panelTypeObject.Id = (Int32)GetOutParameter(cmd, PanelTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(panelTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PanelType
        /// </summary>
        /// <param name="panelTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PanelTypeBase panelTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPANELTYPE);
				
				AddParameter(cmd, pInt32(PanelTypeBase.Property_Id, panelTypeObject.Id));
				AddCommonParams(cmd, panelTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					panelTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(panelTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PanelType
        /// </summary>
        /// <param name="Id">Id of the PanelType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPANELTYPE);	
				
				AddParameter(cmd, pInt32(PanelTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PanelType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PanelType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PanelType object to retrieve</param>
        /// <returns>PanelType object, null if not found</returns>
		public PanelType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPANELTYPEBYID))
			{
				AddParameter( cmd, pInt32(PanelTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PanelType objects 
        /// </summary>
        /// <returns>A list of PanelType objects</returns>
		public PanelTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPANELTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PanelType objects by PageRequest
        /// </summary>
        /// <returns>A list of PanelType objects</returns>
		public PanelTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPANELTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PanelTypeList _PanelTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PanelTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PanelType objects by query String
        /// </summary>
        /// <returns>A list of PanelType objects</returns>
		public PanelTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPANELTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PanelType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PanelType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPANELTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PanelType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PanelType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PanelTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPANELTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_PanelTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PanelTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PanelType object
        /// </summary>
        /// <param name="panelTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PanelTypeBase panelTypeObject, SqlDataReader reader, int start)
		{
			
				panelTypeObject.Id = reader.GetInt32( start + 0 );			
				panelTypeObject.CompanyId = reader.GetGuid( start + 1 );			
				panelTypeObject.Name = reader.GetString( start + 2 );			
				panelTypeObject.Value = reader.GetString( start + 3 );			
				panelTypeObject.IsActive = reader.GetBoolean( start + 4 );			
			FillBaseObject(panelTypeObject, reader, (start + 5));

			
			panelTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PanelType object
        /// </summary>
        /// <param name="panelTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PanelTypeBase panelTypeObject, SqlDataReader reader)
		{
			FillObject(panelTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PanelType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PanelType object</returns>
		private PanelType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PanelType panelTypeObject= new PanelType();
					FillObject(panelTypeObject, reader);
					return panelTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PanelType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PanelType objects</returns>
		private PanelTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PanelType list
			PanelTypeList list = new PanelTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PanelType panelTypeObject = new PanelType();
					FillObject(panelTypeObject, reader);

					list.Add(panelTypeObject);
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
