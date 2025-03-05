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
	public partial class ResturantOrderInstructionToppingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTURANTORDERINSTRUCTIONTOPPING = "InsertResturantOrderInstructionTopping";
		private const string UPDATERESTURANTORDERINSTRUCTIONTOPPING = "UpdateResturantOrderInstructionTopping";
		private const string DELETERESTURANTORDERINSTRUCTIONTOPPING = "DeleteResturantOrderInstructionTopping";
		private const string GETRESTURANTORDERINSTRUCTIONTOPPINGBYID = "GetResturantOrderInstructionToppingById";
		private const string GETALLRESTURANTORDERINSTRUCTIONTOPPING = "GetAllResturantOrderInstructionTopping";
		private const string GETPAGEDRESTURANTORDERINSTRUCTIONTOPPING = "GetPagedResturantOrderInstructionTopping";
		private const string GETRESTURANTORDERINSTRUCTIONTOPPINGMAXIMUMID = "GetResturantOrderInstructionToppingMaximumId";
		private const string GETRESTURANTORDERINSTRUCTIONTOPPINGROWCOUNT = "GetResturantOrderInstructionToppingRowCount";	
		private const string GETRESTURANTORDERINSTRUCTIONTOPPINGBYQUERY = "GetResturantOrderInstructionToppingByQuery";
		#endregion
		
		#region Constructors
		public ResturantOrderInstructionToppingDataAccess(ClientContext context) : base(context) { }
		public ResturantOrderInstructionToppingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="resturantOrderInstructionToppingObject"></param>
		private void AddCommonParams(SqlCommand cmd, ResturantOrderInstructionToppingBase resturantOrderInstructionToppingObject)
		{	
			AddParameter(cmd, pGuid(ResturantOrderInstructionToppingBase.Property_OrderId, resturantOrderInstructionToppingObject.OrderId));
			AddParameter(cmd, pInt32(ResturantOrderInstructionToppingBase.Property_ItemId, resturantOrderInstructionToppingObject.ItemId));
			AddParameter(cmd, pNVarChar(ResturantOrderInstructionToppingBase.Property_SpecialInstruction, resturantOrderInstructionToppingObject.SpecialInstruction));
			AddParameter(cmd, pDateTime(ResturantOrderInstructionToppingBase.Property_CreatedDate, resturantOrderInstructionToppingObject.CreatedDate));
			AddParameter(cmd, pGuid(ResturantOrderInstructionToppingBase.Property_CreatedBy, resturantOrderInstructionToppingObject.CreatedBy));
			AddParameter(cmd, pNVarChar(ResturantOrderInstructionToppingBase.Property_Toppings, resturantOrderInstructionToppingObject.Toppings));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ResturantOrderInstructionTopping
        /// </summary>
        /// <param name="resturantOrderInstructionToppingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ResturantOrderInstructionToppingBase resturantOrderInstructionToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTURANTORDERINSTRUCTIONTOPPING);
	
				AddParameter(cmd, pInt32Out(ResturantOrderInstructionToppingBase.Property_Id));
				AddCommonParams(cmd, resturantOrderInstructionToppingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					resturantOrderInstructionToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					resturantOrderInstructionToppingObject.Id = (Int32)GetOutParameter(cmd, ResturantOrderInstructionToppingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(resturantOrderInstructionToppingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ResturantOrderInstructionTopping
        /// </summary>
        /// <param name="resturantOrderInstructionToppingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ResturantOrderInstructionToppingBase resturantOrderInstructionToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTURANTORDERINSTRUCTIONTOPPING);
				
				AddParameter(cmd, pInt32(ResturantOrderInstructionToppingBase.Property_Id, resturantOrderInstructionToppingObject.Id));
				AddCommonParams(cmd, resturantOrderInstructionToppingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					resturantOrderInstructionToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(resturantOrderInstructionToppingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ResturantOrderInstructionTopping
        /// </summary>
        /// <param name="Id">Id of the ResturantOrderInstructionTopping object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTURANTORDERINSTRUCTIONTOPPING);	
				
				AddParameter(cmd, pInt32(ResturantOrderInstructionToppingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ResturantOrderInstructionTopping), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ResturantOrderInstructionTopping object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ResturantOrderInstructionTopping object to retrieve</param>
        /// <returns>ResturantOrderInstructionTopping object, null if not found</returns>
		public ResturantOrderInstructionTopping Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERINSTRUCTIONTOPPINGBYID))
			{
				AddParameter( cmd, pInt32(ResturantOrderInstructionToppingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ResturantOrderInstructionTopping objects 
        /// </summary>
        /// <returns>A list of ResturantOrderInstructionTopping objects</returns>
		public ResturantOrderInstructionToppingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTURANTORDERINSTRUCTIONTOPPING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ResturantOrderInstructionTopping objects by PageRequest
        /// </summary>
        /// <returns>A list of ResturantOrderInstructionTopping objects</returns>
		public ResturantOrderInstructionToppingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTURANTORDERINSTRUCTIONTOPPING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ResturantOrderInstructionToppingList _ResturantOrderInstructionToppingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ResturantOrderInstructionToppingList;
			}
		}
		
		/// <summary>
        /// Retrieves all ResturantOrderInstructionTopping objects by query String
        /// </summary>
        /// <returns>A list of ResturantOrderInstructionTopping objects</returns>
		public ResturantOrderInstructionToppingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERINSTRUCTIONTOPPINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ResturantOrderInstructionTopping Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ResturantOrderInstructionTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERINSTRUCTIONTOPPINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ResturantOrderInstructionTopping Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ResturantOrderInstructionTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ResturantOrderInstructionToppingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTURANTORDERINSTRUCTIONTOPPINGROWCOUNT))
			{
				SqlDataReader reader;
				_ResturantOrderInstructionToppingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ResturantOrderInstructionToppingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ResturantOrderInstructionTopping object
        /// </summary>
        /// <param name="resturantOrderInstructionToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ResturantOrderInstructionToppingBase resturantOrderInstructionToppingObject, SqlDataReader reader, int start)
		{
			
				resturantOrderInstructionToppingObject.Id = reader.GetInt32( start + 0 );			
				resturantOrderInstructionToppingObject.OrderId = reader.GetGuid( start + 1 );			
				resturantOrderInstructionToppingObject.ItemId = reader.GetInt32( start + 2 );			
				resturantOrderInstructionToppingObject.SpecialInstruction = reader.GetString( start + 3 );			
				resturantOrderInstructionToppingObject.CreatedDate = reader.GetDateTime( start + 4 );			
				resturantOrderInstructionToppingObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) resturantOrderInstructionToppingObject.Toppings = reader.GetString( start + 6 );			
			FillBaseObject(resturantOrderInstructionToppingObject, reader, (start + 7));

			
			resturantOrderInstructionToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ResturantOrderInstructionTopping object
        /// </summary>
        /// <param name="resturantOrderInstructionToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ResturantOrderInstructionToppingBase resturantOrderInstructionToppingObject, SqlDataReader reader)
		{
			FillObject(resturantOrderInstructionToppingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ResturantOrderInstructionTopping object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ResturantOrderInstructionTopping object</returns>
		private ResturantOrderInstructionTopping GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ResturantOrderInstructionTopping resturantOrderInstructionToppingObject= new ResturantOrderInstructionTopping();
					FillObject(resturantOrderInstructionToppingObject, reader);
					return resturantOrderInstructionToppingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ResturantOrderInstructionTopping objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ResturantOrderInstructionTopping objects</returns>
		private ResturantOrderInstructionToppingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ResturantOrderInstructionTopping list
			ResturantOrderInstructionToppingList list = new ResturantOrderInstructionToppingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ResturantOrderInstructionTopping resturantOrderInstructionToppingObject = new ResturantOrderInstructionTopping();
					FillObject(resturantOrderInstructionToppingObject, reader);

					list.Add(resturantOrderInstructionToppingObject);
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
