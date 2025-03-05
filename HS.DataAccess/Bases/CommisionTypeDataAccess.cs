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
	public partial class CommisionTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMMISIONTYPE = "InsertCommisionType";
		private const string UPDATECOMMISIONTYPE = "UpdateCommisionType";
		private const string DELETECOMMISIONTYPE = "DeleteCommisionType";
		private const string GETCOMMISIONTYPEBYID = "GetCommisionTypeById";
		private const string GETALLCOMMISIONTYPE = "GetAllCommisionType";
		private const string GETPAGEDCOMMISIONTYPE = "GetPagedCommisionType";
		private const string GETCOMMISIONTYPEMAXIMUMID = "GetCommisionTypeMaximumId";
		private const string GETCOMMISIONTYPEROWCOUNT = "GetCommisionTypeRowCount";	
		private const string GETCOMMISIONTYPEBYQUERY = "GetCommisionTypeByQuery";
		#endregion
		
		#region Constructors
		public CommisionTypeDataAccess(ClientContext context) : base(context) { }
		public CommisionTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="commisionTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, CommisionTypeBase commisionTypeObject)
		{	
			AddParameter(cmd, pNVarChar(CommisionTypeBase.Property_Name, 50, commisionTypeObject.Name));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CommisionType
        /// </summary>
        /// <param name="commisionTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CommisionTypeBase commisionTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMMISIONTYPE);
	
				AddParameter(cmd, pInt32Out(CommisionTypeBase.Property_Id));
				AddCommonParams(cmd, commisionTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					commisionTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					commisionTypeObject.Id = (Int32)GetOutParameter(cmd, CommisionTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(commisionTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CommisionType
        /// </summary>
        /// <param name="commisionTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CommisionTypeBase commisionTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMMISIONTYPE);
				
				AddParameter(cmd, pInt32(CommisionTypeBase.Property_Id, commisionTypeObject.Id));
				AddCommonParams(cmd, commisionTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					commisionTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(commisionTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CommisionType
        /// </summary>
        /// <param name="Id">Id of the CommisionType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMMISIONTYPE);	
				
				AddParameter(cmd, pInt32(CommisionTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CommisionType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CommisionType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CommisionType object to retrieve</param>
        /// <returns>CommisionType object, null if not found</returns>
		public CommisionType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONTYPEBYID))
			{
				AddParameter( cmd, pInt32(CommisionTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CommisionType objects 
        /// </summary>
        /// <returns>A list of CommisionType objects</returns>
		public CommisionTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMMISIONTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CommisionType objects by PageRequest
        /// </summary>
        /// <returns>A list of CommisionType objects</returns>
		public CommisionTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMMISIONTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CommisionTypeList _CommisionTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CommisionTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all CommisionType objects by query String
        /// </summary>
        /// <returns>A list of CommisionType objects</returns>
		public CommisionTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CommisionType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CommisionType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CommisionType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CommisionType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CommisionTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_CommisionTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CommisionTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CommisionType object
        /// </summary>
        /// <param name="commisionTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CommisionTypeBase commisionTypeObject, SqlDataReader reader, int start)
		{
			
				commisionTypeObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) commisionTypeObject.Name = reader.GetString( start + 1 );			
			FillBaseObject(commisionTypeObject, reader, (start + 2));

			
			commisionTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CommisionType object
        /// </summary>
        /// <param name="commisionTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CommisionTypeBase commisionTypeObject, SqlDataReader reader)
		{
			FillObject(commisionTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CommisionType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CommisionType object</returns>
		private CommisionType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CommisionType commisionTypeObject= new CommisionType();
					FillObject(commisionTypeObject, reader);
					return commisionTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CommisionType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CommisionType objects</returns>
		private CommisionTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CommisionType list
			CommisionTypeList list = new CommisionTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CommisionType commisionTypeObject = new CommisionType();
					FillObject(commisionTypeObject, reader);

					list.Add(commisionTypeObject);
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
