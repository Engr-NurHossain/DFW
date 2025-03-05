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
	public partial class MMRDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMMR = "InsertMMR";
		private const string UPDATEMMR = "UpdateMMR";
		private const string DELETEMMR = "DeleteMMR";
		private const string GETMMRBYID = "GetMMRById";
		private const string GETALLMMR = "GetAllMMR";
		private const string GETPAGEDMMR = "GetPagedMMR";
		private const string GETMMRMAXIMUMID = "GetMMRMaximumId";
		private const string GETMMRROWCOUNT = "GetMMRRowCount";	
		private const string GETMMRBYQUERY = "GetMMRByQuery";
		#endregion
		
		#region Constructors
		public MMRDataAccess(ClientContext context) : base(context) { }
		public MMRDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="mMRObject"></param>
		private void AddCommonParams(SqlCommand cmd, MMRBase mMRObject)
		{	
			AddParameter(cmd, pGuid(MMRBase.Property_CompanyId, mMRObject.CompanyId));
			AddParameter(cmd, pNVarChar(MMRBase.Property_Name, 50, mMRObject.Name));
			AddParameter(cmd, pDouble(MMRBase.Property_Value, mMRObject.Value));
			AddParameter(cmd, pBool(MMRBase.Property_IsActivve, mMRObject.IsActivve));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MMR
        /// </summary>
        /// <param name="mMRObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MMRBase mMRObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMMR);
	
				AddParameter(cmd, pInt32Out(MMRBase.Property_Id));
				AddCommonParams(cmd, mMRObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					mMRObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					mMRObject.Id = (Int32)GetOutParameter(cmd, MMRBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(mMRObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MMR
        /// </summary>
        /// <param name="mMRObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MMRBase mMRObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMMR);
				
				AddParameter(cmd, pInt32(MMRBase.Property_Id, mMRObject.Id));
				AddCommonParams(cmd, mMRObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					mMRObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(mMRObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MMR
        /// </summary>
        /// <param name="Id">Id of the MMR object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMMR);	
				
				AddParameter(cmd, pInt32(MMRBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MMR), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MMR object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MMR object to retrieve</param>
        /// <returns>MMR object, null if not found</returns>
		public MMR Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMMRBYID))
			{
				AddParameter( cmd, pInt32(MMRBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MMR objects 
        /// </summary>
        /// <returns>A list of MMR objects</returns>
		public MMRList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMMR))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MMR objects by PageRequest
        /// </summary>
        /// <returns>A list of MMR objects</returns>
		public MMRList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMMR))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MMRList _MMRList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MMRList;
			}
		}
		
		/// <summary>
        /// Retrieves all MMR objects by query String
        /// </summary>
        /// <returns>A list of MMR objects</returns>
		public MMRList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMMRBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MMR Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MMR
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMMRMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MMR Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MMR
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MMRRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMMRROWCOUNT))
			{
				SqlDataReader reader;
				_MMRRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MMRRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MMR object
        /// </summary>
        /// <param name="mMRObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MMRBase mMRObject, SqlDataReader reader, int start)
		{
			
				mMRObject.Id = reader.GetInt32( start + 0 );			
				mMRObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) mMRObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) mMRObject.Value = reader.GetDouble( start + 3 );			
				if(!reader.IsDBNull(4)) mMRObject.IsActivve = reader.GetBoolean( start + 4 );			
			FillBaseObject(mMRObject, reader, (start + 5));

			
			mMRObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MMR object
        /// </summary>
        /// <param name="mMRObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MMRBase mMRObject, SqlDataReader reader)
		{
			FillObject(mMRObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MMR object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MMR object</returns>
		private MMR GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MMR mMRObject= new MMR();
					FillObject(mMRObject, reader);
					return mMRObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MMR objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MMR objects</returns>
		private MMRList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MMR list
			MMRList list = new MMRList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MMR mMRObject = new MMR();
					FillObject(mMRObject, reader);

					list.Add(mMRObject);
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
