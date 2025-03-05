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
	public partial class AAAAlifSecuirtyCCInfo2DataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAAAALIFSECUIRTYCCINFO2 = "InsertAAAAlifSecuirtyCCInfo2";
		private const string UPDATEAAAALIFSECUIRTYCCINFO2 = "UpdateAAAAlifSecuirtyCCInfo2";
		private const string DELETEAAAALIFSECUIRTYCCINFO2 = "DeleteAAAAlifSecuirtyCCInfo2";
		private const string GETAAAALIFSECUIRTYCCINFO2BYID = "GetAAAAlifSecuirtyCCInfo2ById";
		private const string GETALLAAAALIFSECUIRTYCCINFO2 = "GetAllAAAAlifSecuirtyCCInfo2";
		private const string GETPAGEDAAAALIFSECUIRTYCCINFO2 = "GetPagedAAAAlifSecuirtyCCInfo2";
		private const string GETAAAALIFSECUIRTYCCINFO2MAXIMUMID = "GetAAAAlifSecuirtyCCInfo2MaximumId";
		private const string GETAAAALIFSECUIRTYCCINFO2ROWCOUNT = "GetAAAAlifSecuirtyCCInfo2RowCount";	
		private const string GETAAAALIFSECUIRTYCCINFO2BYQUERY = "GetAAAAlifSecuirtyCCInfo2ByQuery";
		#endregion
		
		#region Constructors
		public AAAAlifSecuirtyCCInfo2DataAccess(ClientContext context) : base(context) { }
		public AAAAlifSecuirtyCCInfo2DataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="aAAAlifSecuirtyCCInfo2Object"></param>
		private void AddCommonParams(SqlCommand cmd, AAAAlifSecuirtyCCInfo2Base aAAAlifSecuirtyCCInfo2Object)
		{	
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_NameOnCard, aAAAlifSecuirtyCCInfo2Object.NameOnCard));
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_CardNumber, aAAAlifSecuirtyCCInfo2Object.CardNumber));
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_ExpDate, aAAAlifSecuirtyCCInfo2Object.ExpDate));
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_SecurityCode, aAAAlifSecuirtyCCInfo2Object.SecurityCode));
			AddParameter(cmd, pInt32(AAAAlifSecuirtyCCInfo2Base.Property_CustomerId, aAAAlifSecuirtyCCInfo2Object.CustomerId));
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_FirstName, aAAAlifSecuirtyCCInfo2Object.FirstName));
			AddParameter(cmd, pNVarChar(AAAAlifSecuirtyCCInfo2Base.Property_LastName, aAAAlifSecuirtyCCInfo2Object.LastName));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AAAAlifSecuirtyCCInfo2
        /// </summary>
        /// <param name="aAAAlifSecuirtyCCInfo2Object">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AAAAlifSecuirtyCCInfo2Base aAAAlifSecuirtyCCInfo2Object)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAAAALIFSECUIRTYCCINFO2);
	
				AddParameter(cmd, pInt32(AAAAlifSecuirtyCCInfo2Base.Property_Id, aAAAlifSecuirtyCCInfo2Object.Id));
				AddCommonParams(cmd, aAAAlifSecuirtyCCInfo2Object);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					aAAAlifSecuirtyCCInfo2Object.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(aAAAlifSecuirtyCCInfo2Object, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AAAAlifSecuirtyCCInfo2
        /// </summary>
        /// <param name="aAAAlifSecuirtyCCInfo2Object">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AAAAlifSecuirtyCCInfo2Base aAAAlifSecuirtyCCInfo2Object)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAAAALIFSECUIRTYCCINFO2);
				
				AddParameter(cmd, pInt32(AAAAlifSecuirtyCCInfo2Base.Property_Id, aAAAlifSecuirtyCCInfo2Object.Id));
				AddCommonParams(cmd, aAAAlifSecuirtyCCInfo2Object);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					aAAAlifSecuirtyCCInfo2Object.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(aAAAlifSecuirtyCCInfo2Object, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AAAAlifSecuirtyCCInfo2
        /// </summary>
        /// <param name="Id">Id of the AAAAlifSecuirtyCCInfo2 object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAAAALIFSECUIRTYCCINFO2);	
				
				AddParameter(cmd, pInt32(AAAAlifSecuirtyCCInfo2Base.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AAAAlifSecuirtyCCInfo2), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AAAAlifSecuirtyCCInfo2 object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AAAAlifSecuirtyCCInfo2 object to retrieve</param>
        /// <returns>AAAAlifSecuirtyCCInfo2 object, null if not found</returns>
		public AAAAlifSecuirtyCCInfo2 Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETAAAALIFSECUIRTYCCINFO2BYID))
			{
				AddParameter( cmd, pInt32(AAAAlifSecuirtyCCInfo2Base.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AAAAlifSecuirtyCCInfo2 objects 
        /// </summary>
        /// <returns>A list of AAAAlifSecuirtyCCInfo2 objects</returns>
		public AAAAlifSecuirtyCCInfo2List GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAAAALIFSECUIRTYCCINFO2))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AAAAlifSecuirtyCCInfo2 objects by PageRequest
        /// </summary>
        /// <returns>A list of AAAAlifSecuirtyCCInfo2 objects</returns>
		public AAAAlifSecuirtyCCInfo2List GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAAAALIFSECUIRTYCCINFO2))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AAAAlifSecuirtyCCInfo2List _AAAAlifSecuirtyCCInfo2List = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AAAAlifSecuirtyCCInfo2List;
			}
		}
		
		/// <summary>
        /// Retrieves all AAAAlifSecuirtyCCInfo2 objects by query String
        /// </summary>
        /// <returns>A list of AAAAlifSecuirtyCCInfo2 objects</returns>
		public AAAAlifSecuirtyCCInfo2List GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAAAALIFSECUIRTYCCINFO2BYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AAAAlifSecuirtyCCInfo2 Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AAAAlifSecuirtyCCInfo2
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAAAALIFSECUIRTYCCINFO2MAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AAAAlifSecuirtyCCInfo2 Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AAAAlifSecuirtyCCInfo2
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AAAAlifSecuirtyCCInfo2RowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAAAALIFSECUIRTYCCINFO2ROWCOUNT))
			{
				SqlDataReader reader;
				_AAAAlifSecuirtyCCInfo2RowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AAAAlifSecuirtyCCInfo2RowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AAAAlifSecuirtyCCInfo2 object
        /// </summary>
        /// <param name="aAAAlifSecuirtyCCInfo2Object">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AAAAlifSecuirtyCCInfo2Base aAAAlifSecuirtyCCInfo2Object, SqlDataReader reader, int start)
		{
			
				aAAAlifSecuirtyCCInfo2Object.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) aAAAlifSecuirtyCCInfo2Object.NameOnCard = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) aAAAlifSecuirtyCCInfo2Object.CardNumber = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) aAAAlifSecuirtyCCInfo2Object.ExpDate = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) aAAAlifSecuirtyCCInfo2Object.SecurityCode = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) aAAAlifSecuirtyCCInfo2Object.CustomerId = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) aAAAlifSecuirtyCCInfo2Object.FirstName = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) aAAAlifSecuirtyCCInfo2Object.LastName = reader.GetString( start + 7 );			
			FillBaseObject(aAAAlifSecuirtyCCInfo2Object, reader, (start + 8));

			
			aAAAlifSecuirtyCCInfo2Object.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AAAAlifSecuirtyCCInfo2 object
        /// </summary>
        /// <param name="aAAAlifSecuirtyCCInfo2Object">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AAAAlifSecuirtyCCInfo2Base aAAAlifSecuirtyCCInfo2Object, SqlDataReader reader)
		{
			FillObject(aAAAlifSecuirtyCCInfo2Object, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AAAAlifSecuirtyCCInfo2 object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AAAAlifSecuirtyCCInfo2 object</returns>
		private AAAAlifSecuirtyCCInfo2 GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AAAAlifSecuirtyCCInfo2 aAAAlifSecuirtyCCInfo2Object= new AAAAlifSecuirtyCCInfo2();
					FillObject(aAAAlifSecuirtyCCInfo2Object, reader);
					return aAAAlifSecuirtyCCInfo2Object;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AAAAlifSecuirtyCCInfo2 objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AAAAlifSecuirtyCCInfo2 objects</returns>
		private AAAAlifSecuirtyCCInfo2List GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AAAAlifSecuirtyCCInfo2 list
			AAAAlifSecuirtyCCInfo2List list = new AAAAlifSecuirtyCCInfo2List();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AAAAlifSecuirtyCCInfo2 aAAAlifSecuirtyCCInfo2Object = new AAAAlifSecuirtyCCInfo2();
					FillObject(aAAAlifSecuirtyCCInfo2Object, reader);

					list.Add(aAAAlifSecuirtyCCInfo2Object);
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
