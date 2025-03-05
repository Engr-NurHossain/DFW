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
	public partial class FundingCompanyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTFUNDINGCOMPANY = "InsertFundingCompany";
		private const string UPDATEFUNDINGCOMPANY = "UpdateFundingCompany";
		private const string DELETEFUNDINGCOMPANY = "DeleteFundingCompany";
		private const string GETFUNDINGCOMPANYBYID = "GetFundingCompanyById";
		private const string GETALLFUNDINGCOMPANY = "GetAllFundingCompany";
		private const string GETPAGEDFUNDINGCOMPANY = "GetPagedFundingCompany";
		private const string GETFUNDINGCOMPANYMAXIMUMID = "GetFundingCompanyMaximumId";
		private const string GETFUNDINGCOMPANYROWCOUNT = "GetFundingCompanyRowCount";	
		private const string GETFUNDINGCOMPANYBYQUERY = "GetFundingCompanyByQuery";
		#endregion
		
		#region Constructors
		public FundingCompanyDataAccess(ClientContext context) : base(context) { }
		public FundingCompanyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="fundingCompanyObject"></param>
		private void AddCommonParams(SqlCommand cmd, FundingCompanyBase fundingCompanyObject)
		{	
			AddParameter(cmd, pNVarChar(FundingCompanyBase.Property_Name, 50, fundingCompanyObject.Name));
			AddParameter(cmd, pNVarChar(FundingCompanyBase.Property_Value, 50, fundingCompanyObject.Value));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts FundingCompany
        /// </summary>
        /// <param name="fundingCompanyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(FundingCompanyBase fundingCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTFUNDINGCOMPANY);
	
				AddParameter(cmd, pInt32Out(FundingCompanyBase.Property_Id));
				AddCommonParams(cmd, fundingCompanyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					fundingCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					fundingCompanyObject.Id = (Int32)GetOutParameter(cmd, FundingCompanyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(fundingCompanyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates FundingCompany
        /// </summary>
        /// <param name="fundingCompanyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(FundingCompanyBase fundingCompanyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEFUNDINGCOMPANY);
				
				AddParameter(cmd, pInt32(FundingCompanyBase.Property_Id, fundingCompanyObject.Id));
				AddCommonParams(cmd, fundingCompanyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					fundingCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(fundingCompanyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes FundingCompany
        /// </summary>
        /// <param name="Id">Id of the FundingCompany object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEFUNDINGCOMPANY);	
				
				AddParameter(cmd, pInt32(FundingCompanyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(FundingCompany), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves FundingCompany object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the FundingCompany object to retrieve</param>
        /// <returns>FundingCompany object, null if not found</returns>
		public FundingCompany Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETFUNDINGCOMPANYBYID))
			{
				AddParameter( cmd, pInt32(FundingCompanyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all FundingCompany objects 
        /// </summary>
        /// <returns>A list of FundingCompany objects</returns>
		public FundingCompanyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLFUNDINGCOMPANY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all FundingCompany objects by PageRequest
        /// </summary>
        /// <returns>A list of FundingCompany objects</returns>
		public FundingCompanyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDFUNDINGCOMPANY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				FundingCompanyList _FundingCompanyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _FundingCompanyList;
			}
		}
		
		/// <summary>
        /// Retrieves all FundingCompany objects by query String
        /// </summary>
        /// <returns>A list of FundingCompany objects</returns>
		public FundingCompanyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETFUNDINGCOMPANYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get FundingCompany Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of FundingCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFUNDINGCOMPANYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get FundingCompany Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of FundingCompany
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _FundingCompanyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETFUNDINGCOMPANYROWCOUNT))
			{
				SqlDataReader reader;
				_FundingCompanyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _FundingCompanyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills FundingCompany object
        /// </summary>
        /// <param name="fundingCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(FundingCompanyBase fundingCompanyObject, SqlDataReader reader, int start)
		{
			
				fundingCompanyObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) fundingCompanyObject.Name = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) fundingCompanyObject.Value = reader.GetString( start + 2 );			
			FillBaseObject(fundingCompanyObject, reader, (start + 3));

			
			fundingCompanyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills FundingCompany object
        /// </summary>
        /// <param name="fundingCompanyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(FundingCompanyBase fundingCompanyObject, SqlDataReader reader)
		{
			FillObject(fundingCompanyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves FundingCompany object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>FundingCompany object</returns>
		private FundingCompany GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					FundingCompany fundingCompanyObject= new FundingCompany();
					FillObject(fundingCompanyObject, reader);
					return fundingCompanyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of FundingCompany objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of FundingCompany objects</returns>
		private FundingCompanyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//FundingCompany list
			FundingCompanyList list = new FundingCompanyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					FundingCompany fundingCompanyObject = new FundingCompany();
					FillObject(fundingCompanyObject, reader);

					list.Add(fundingCompanyObject);
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
