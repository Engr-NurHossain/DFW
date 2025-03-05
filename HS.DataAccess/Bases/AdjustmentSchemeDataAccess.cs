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
	public partial class AdjustmentSchemeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTADJUSTMENTSCHEME = "InsertAdjustmentScheme";
		private const string UPDATEADJUSTMENTSCHEME = "UpdateAdjustmentScheme";
		private const string DELETEADJUSTMENTSCHEME = "DeleteAdjustmentScheme";
		private const string GETADJUSTMENTSCHEMEBYID = "GetAdjustmentSchemeById";
		private const string GETALLADJUSTMENTSCHEME = "GetAllAdjustmentScheme";
		private const string GETPAGEDADJUSTMENTSCHEME = "GetPagedAdjustmentScheme";
		private const string GETADJUSTMENTSCHEMEMAXIMUMID = "GetAdjustmentSchemeMaximumId";
		private const string GETADJUSTMENTSCHEMEROWCOUNT = "GetAdjustmentSchemeRowCount";	
		private const string GETADJUSTMENTSCHEMEBYQUERY = "GetAdjustmentSchemeByQuery";
		#endregion
		
		#region Constructors
		public AdjustmentSchemeDataAccess(ClientContext context) : base(context) { }
		public AdjustmentSchemeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="adjustmentSchemeObject"></param>
		private void AddCommonParams(SqlCommand cmd, AdjustmentSchemeBase adjustmentSchemeObject)
		{	
			AddParameter(cmd, pNVarChar(AdjustmentSchemeBase.Property_Name, 50, adjustmentSchemeObject.Name));
			AddParameter(cmd, pInt32(AdjustmentSchemeBase.Property_ComissionSessionId, adjustmentSchemeObject.ComissionSessionId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AdjustmentScheme
        /// </summary>
        /// <param name="adjustmentSchemeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AdjustmentSchemeBase adjustmentSchemeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTADJUSTMENTSCHEME);
	
				AddParameter(cmd, pInt32Out(AdjustmentSchemeBase.Property_Id));
				AddCommonParams(cmd, adjustmentSchemeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					adjustmentSchemeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					adjustmentSchemeObject.Id = (Int32)GetOutParameter(cmd, AdjustmentSchemeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(adjustmentSchemeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AdjustmentScheme
        /// </summary>
        /// <param name="adjustmentSchemeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AdjustmentSchemeBase adjustmentSchemeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEADJUSTMENTSCHEME);
				
				AddParameter(cmd, pInt32(AdjustmentSchemeBase.Property_Id, adjustmentSchemeObject.Id));
				AddCommonParams(cmd, adjustmentSchemeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					adjustmentSchemeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(adjustmentSchemeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AdjustmentScheme
        /// </summary>
        /// <param name="Id">Id of the AdjustmentScheme object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEADJUSTMENTSCHEME);	
				
				AddParameter(cmd, pInt32(AdjustmentSchemeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AdjustmentScheme), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AdjustmentScheme object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AdjustmentScheme object to retrieve</param>
        /// <returns>AdjustmentScheme object, null if not found</returns>
		public AdjustmentScheme Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTSCHEMEBYID))
			{
				AddParameter( cmd, pInt32(AdjustmentSchemeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AdjustmentScheme objects 
        /// </summary>
        /// <returns>A list of AdjustmentScheme objects</returns>
		public AdjustmentSchemeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLADJUSTMENTSCHEME))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AdjustmentScheme objects by PageRequest
        /// </summary>
        /// <returns>A list of AdjustmentScheme objects</returns>
		public AdjustmentSchemeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDADJUSTMENTSCHEME))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AdjustmentSchemeList _AdjustmentSchemeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AdjustmentSchemeList;
			}
		}
		
		/// <summary>
        /// Retrieves all AdjustmentScheme objects by query String
        /// </summary>
        /// <returns>A list of AdjustmentScheme objects</returns>
		public AdjustmentSchemeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTSCHEMEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AdjustmentScheme Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AdjustmentScheme
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTSCHEMEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AdjustmentScheme Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AdjustmentScheme
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AdjustmentSchemeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETADJUSTMENTSCHEMEROWCOUNT))
			{
				SqlDataReader reader;
				_AdjustmentSchemeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AdjustmentSchemeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AdjustmentScheme object
        /// </summary>
        /// <param name="adjustmentSchemeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AdjustmentSchemeBase adjustmentSchemeObject, SqlDataReader reader, int start)
		{
			
				adjustmentSchemeObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) adjustmentSchemeObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) adjustmentSchemeObject.ComissionSessionId = reader.GetInt32( start + 2 );			
			FillBaseObject(adjustmentSchemeObject, reader, (start + 3));

			
			adjustmentSchemeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AdjustmentScheme object
        /// </summary>
        /// <param name="adjustmentSchemeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AdjustmentSchemeBase adjustmentSchemeObject, SqlDataReader reader)
		{
			FillObject(adjustmentSchemeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AdjustmentScheme object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AdjustmentScheme object</returns>
		private AdjustmentScheme GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AdjustmentScheme adjustmentSchemeObject= new AdjustmentScheme();
					FillObject(adjustmentSchemeObject, reader);
					return adjustmentSchemeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AdjustmentScheme objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AdjustmentScheme objects</returns>
		private AdjustmentSchemeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AdjustmentScheme list
			AdjustmentSchemeList list = new AdjustmentSchemeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AdjustmentScheme adjustmentSchemeObject = new AdjustmentScheme();
					FillObject(adjustmentSchemeObject, reader);

					list.Add(adjustmentSchemeObject);
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
