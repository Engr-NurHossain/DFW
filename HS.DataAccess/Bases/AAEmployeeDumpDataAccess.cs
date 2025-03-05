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
	public partial class AAEmployeeDumpDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTAAEMPLOYEEDUMP = "InsertAAEmployeeDump";
		private const string UPDATEAAEMPLOYEEDUMP = "UpdateAAEmployeeDump";
		private const string DELETEAAEMPLOYEEDUMP = "DeleteAAEmployeeDump";
		private const string GETAAEMPLOYEEDUMPBYID = "GetAAEmployeeDumpById";
		private const string GETALLAAEMPLOYEEDUMP = "GetAllAAEmployeeDump";
		private const string GETPAGEDAAEMPLOYEEDUMP = "GetPagedAAEmployeeDump";
		private const string GETAAEMPLOYEEDUMPMAXIMUMID = "GetAAEmployeeDumpMaximumId";
		private const string GETAAEMPLOYEEDUMPROWCOUNT = "GetAAEmployeeDumpRowCount";	
		private const string GETAAEMPLOYEEDUMPBYQUERY = "GetAAEmployeeDumpByQuery";
		#endregion
		
		#region Constructors
		public AAEmployeeDumpDataAccess(ClientContext context) : base(context) { }
		public AAEmployeeDumpDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="aAEmployeeDumpObject"></param>
		private void AddCommonParams(SqlCommand cmd, AAEmployeeDumpBase aAEmployeeDumpObject)
		{	
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Name, 50, aAEmployeeDumpObject.Name));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Email, 50, aAEmployeeDumpObject.Email));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Mobile, 50, aAEmployeeDumpObject.Mobile));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Address, 100, aAEmployeeDumpObject.Address));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_BrinksUserName, 50, aAEmployeeDumpObject.BrinksUserName));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Roles, 50, aAEmployeeDumpObject.Roles));
			AddParameter(cmd, pNVarChar(AAEmployeeDumpBase.Property_Status, 50, aAEmployeeDumpObject.Status));
			AddParameter(cmd, pDateTime(AAEmployeeDumpBase.Property_Created, aAEmployeeDumpObject.Created));
			AddParameter(cmd, pDateTime(AAEmployeeDumpBase.Property_Updated, aAEmployeeDumpObject.Updated));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts AAEmployeeDump
        /// </summary>
        /// <param name="aAEmployeeDumpObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(AAEmployeeDumpBase aAEmployeeDumpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTAAEMPLOYEEDUMP);
	
				AddParameter(cmd, pInt32Out(AAEmployeeDumpBase.Property_Id));
				AddCommonParams(cmd, aAEmployeeDumpObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					aAEmployeeDumpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					aAEmployeeDumpObject.Id = (Int32)GetOutParameter(cmd, AAEmployeeDumpBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(aAEmployeeDumpObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates AAEmployeeDump
        /// </summary>
        /// <param name="aAEmployeeDumpObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(AAEmployeeDumpBase aAEmployeeDumpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEAAEMPLOYEEDUMP);
				
				AddParameter(cmd, pInt32(AAEmployeeDumpBase.Property_Id, aAEmployeeDumpObject.Id));
				AddCommonParams(cmd, aAEmployeeDumpObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					aAEmployeeDumpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(aAEmployeeDumpObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes AAEmployeeDump
        /// </summary>
        /// <param name="Id">Id of the AAEmployeeDump object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEAAEMPLOYEEDUMP);	
				
				AddParameter(cmd, pInt32(AAEmployeeDumpBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(AAEmployeeDump), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves AAEmployeeDump object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the AAEmployeeDump object to retrieve</param>
        /// <returns>AAEmployeeDump object, null if not found</returns>
		public AAEmployeeDump Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETAAEMPLOYEEDUMPBYID))
			{
				AddParameter( cmd, pInt32(AAEmployeeDumpBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all AAEmployeeDump objects 
        /// </summary>
        /// <returns>A list of AAEmployeeDump objects</returns>
		public AAEmployeeDumpList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLAAEMPLOYEEDUMP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all AAEmployeeDump objects by PageRequest
        /// </summary>
        /// <returns>A list of AAEmployeeDump objects</returns>
		public AAEmployeeDumpList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDAAEMPLOYEEDUMP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				AAEmployeeDumpList _AAEmployeeDumpList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _AAEmployeeDumpList;
			}
		}
		
		/// <summary>
        /// Retrieves all AAEmployeeDump objects by query String
        /// </summary>
        /// <returns>A list of AAEmployeeDump objects</returns>
		public AAEmployeeDumpList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETAAEMPLOYEEDUMPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get AAEmployeeDump Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of AAEmployeeDump
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAAEMPLOYEEDUMPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get AAEmployeeDump Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of AAEmployeeDump
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _AAEmployeeDumpRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETAAEMPLOYEEDUMPROWCOUNT))
			{
				SqlDataReader reader;
				_AAEmployeeDumpRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _AAEmployeeDumpRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills AAEmployeeDump object
        /// </summary>
        /// <param name="aAEmployeeDumpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(AAEmployeeDumpBase aAEmployeeDumpObject, SqlDataReader reader, int start)
		{
			
				aAEmployeeDumpObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) aAEmployeeDumpObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) aAEmployeeDumpObject.Email = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) aAEmployeeDumpObject.Mobile = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) aAEmployeeDumpObject.Address = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) aAEmployeeDumpObject.BrinksUserName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) aAEmployeeDumpObject.Roles = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) aAEmployeeDumpObject.Status = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) aAEmployeeDumpObject.Created = reader.GetDateTime( start + 8 );			
				if(!reader.IsDBNull(9)) aAEmployeeDumpObject.Updated = reader.GetDateTime( start + 9 );			
			FillBaseObject(aAEmployeeDumpObject, reader, (start + 10));

			
			aAEmployeeDumpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills AAEmployeeDump object
        /// </summary>
        /// <param name="aAEmployeeDumpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(AAEmployeeDumpBase aAEmployeeDumpObject, SqlDataReader reader)
		{
			FillObject(aAEmployeeDumpObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves AAEmployeeDump object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>AAEmployeeDump object</returns>
		private AAEmployeeDump GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					AAEmployeeDump aAEmployeeDumpObject= new AAEmployeeDump();
					FillObject(aAEmployeeDumpObject, reader);
					return aAEmployeeDumpObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of AAEmployeeDump objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of AAEmployeeDump objects</returns>
		private AAEmployeeDumpList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//AAEmployeeDump list
			AAEmployeeDumpList list = new AAEmployeeDumpList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					AAEmployeeDump aAEmployeeDumpObject = new AAEmployeeDump();
					FillObject(aAEmployeeDumpObject, reader);

					list.Add(aAEmployeeDumpObject);
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
