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
	public partial class CommisionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCOMMISION = "InsertCommision";
		private const string UPDATECOMMISION = "UpdateCommision";
		private const string DELETECOMMISION = "DeleteCommision";
		private const string GETCOMMISIONBYID = "GetCommisionById";
		private const string GETALLCOMMISION = "GetAllCommision";
		private const string GETPAGEDCOMMISION = "GetPagedCommision";
		private const string GETCOMMISIONMAXIMUMID = "GetCommisionMaximumId";
		private const string GETCOMMISIONROWCOUNT = "GetCommisionRowCount";	
		private const string GETCOMMISIONBYQUERY = "GetCommisionByQuery";
		#endregion
		
		#region Constructors
		public CommisionDataAccess(ClientContext context) : base(context) { }
		public CommisionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="commisionObject"></param>
		private void AddCommonParams(SqlCommand cmd, CommisionBase commisionObject)
		{	
			AddParameter(cmd, pNVarChar(CommisionBase.Property_Name, 50, commisionObject.Name));
			AddParameter(cmd, pNVarChar(CommisionBase.Property_TimeFrame, 50, commisionObject.TimeFrame));
			AddParameter(cmd, pInt32(CommisionBase.Property_CommisionTypeId, commisionObject.CommisionTypeId));
			AddParameter(cmd, pInt32(CommisionBase.Property_CommisionSessionId, commisionObject.CommisionSessionId));
			AddParameter(cmd, pBool(CommisionBase.Property_IsActive, commisionObject.IsActive));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Commision
        /// </summary>
        /// <param name="commisionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CommisionBase commisionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCOMMISION);
	
				AddParameter(cmd, pInt32Out(CommisionBase.Property_Id));
				AddCommonParams(cmd, commisionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					commisionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					commisionObject.Id = (Int32)GetOutParameter(cmd, CommisionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(commisionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Commision
        /// </summary>
        /// <param name="commisionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CommisionBase commisionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECOMMISION);
				
				AddParameter(cmd, pInt32(CommisionBase.Property_Id, commisionObject.Id));
				AddCommonParams(cmd, commisionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					commisionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(commisionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Commision
        /// </summary>
        /// <param name="Id">Id of the Commision object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECOMMISION);	
				
				AddParameter(cmd, pInt32(CommisionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Commision), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Commision object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Commision object to retrieve</param>
        /// <returns>Commision object, null if not found</returns>
		public Commision Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONBYID))
			{
				AddParameter( cmd, pInt32(CommisionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Commision objects 
        /// </summary>
        /// <returns>A list of Commision objects</returns>
		public CommisionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCOMMISION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Commision objects by PageRequest
        /// </summary>
        /// <returns>A list of Commision objects</returns>
		public CommisionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCOMMISION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CommisionList _CommisionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CommisionList;
			}
		}
		
		/// <summary>
        /// Retrieves all Commision objects by query String
        /// </summary>
        /// <returns>A list of Commision objects</returns>
		public CommisionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Commision Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Commision
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Commision Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Commision
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CommisionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCOMMISIONROWCOUNT))
			{
				SqlDataReader reader;
				_CommisionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CommisionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Commision object
        /// </summary>
        /// <param name="commisionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CommisionBase commisionObject, SqlDataReader reader, int start)
		{
			
				commisionObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) commisionObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) commisionObject.TimeFrame = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) commisionObject.CommisionTypeId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) commisionObject.CommisionSessionId = reader.GetInt32( start + 4 );			
				if(!reader.IsDBNull(5)) commisionObject.IsActive = reader.GetBoolean( start + 5 );			
			FillBaseObject(commisionObject, reader, (start + 6));

			
			commisionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Commision object
        /// </summary>
        /// <param name="commisionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CommisionBase commisionObject, SqlDataReader reader)
		{
			FillObject(commisionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Commision object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Commision object</returns>
		private Commision GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Commision commisionObject= new Commision();
					FillObject(commisionObject, reader);
					return commisionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Commision objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Commision objects</returns>
		private CommisionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Commision list
			CommisionList list = new CommisionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Commision commisionObject = new Commision();
					FillObject(commisionObject, reader);

					list.Add(commisionObject);
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
