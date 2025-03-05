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
	public partial class RestrictedZipCodeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTRICTEDZIPCODE = "InsertRestrictedZipCode";
		private const string UPDATERESTRICTEDZIPCODE = "UpdateRestrictedZipCode";
		private const string DELETERESTRICTEDZIPCODE = "DeleteRestrictedZipCode";
		private const string GETRESTRICTEDZIPCODEBYID = "GetRestrictedZipCodeById";
		private const string GETALLRESTRICTEDZIPCODE = "GetAllRestrictedZipCode";
		private const string GETPAGEDRESTRICTEDZIPCODE = "GetPagedRestrictedZipCode";
		private const string GETRESTRICTEDZIPCODEMAXIMUMID = "GetRestrictedZipCodeMaximumId";
		private const string GETRESTRICTEDZIPCODEROWCOUNT = "GetRestrictedZipCodeRowCount";	
		private const string GETRESTRICTEDZIPCODEBYQUERY = "GetRestrictedZipCodeByQuery";
		#endregion
		
		#region Constructors
		public RestrictedZipCodeDataAccess(ClientContext context) : base(context) { }
		public RestrictedZipCodeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restrictedZipCodeObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestrictedZipCodeBase restrictedZipCodeObject)
		{	
			AddParameter(cmd, pNVarChar(RestrictedZipCodeBase.Property_Zipcode, 6, restrictedZipCodeObject.Zipcode));
			AddParameter(cmd, pNVarChar(RestrictedZipCodeBase.Property_CreatedBy, 50, restrictedZipCodeObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestrictedZipCodeBase.Property_CreatedDate, restrictedZipCodeObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestrictedZipCode
        /// </summary>
        /// <param name="restrictedZipCodeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestrictedZipCodeBase restrictedZipCodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTRICTEDZIPCODE);
	
				AddParameter(cmd, pInt32Out(RestrictedZipCodeBase.Property_Id));
				AddCommonParams(cmd, restrictedZipCodeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restrictedZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restrictedZipCodeObject.Id = (Int32)GetOutParameter(cmd, RestrictedZipCodeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restrictedZipCodeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestrictedZipCode
        /// </summary>
        /// <param name="restrictedZipCodeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestrictedZipCodeBase restrictedZipCodeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTRICTEDZIPCODE);
				
				AddParameter(cmd, pInt32(RestrictedZipCodeBase.Property_Id, restrictedZipCodeObject.Id));
				AddCommonParams(cmd, restrictedZipCodeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restrictedZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restrictedZipCodeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestrictedZipCode
        /// </summary>
        /// <param name="Id">Id of the RestrictedZipCode object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTRICTEDZIPCODE);	
				
				AddParameter(cmd, pInt32(RestrictedZipCodeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestrictedZipCode), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestrictedZipCode object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestrictedZipCode object to retrieve</param>
        /// <returns>RestrictedZipCode object, null if not found</returns>
		public RestrictedZipCode Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTRICTEDZIPCODEBYID))
			{
				AddParameter( cmd, pInt32(RestrictedZipCodeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestrictedZipCode objects 
        /// </summary>
        /// <returns>A list of RestrictedZipCode objects</returns>
		public RestrictedZipCodeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTRICTEDZIPCODE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestrictedZipCode objects by PageRequest
        /// </summary>
        /// <returns>A list of RestrictedZipCode objects</returns>
		public RestrictedZipCodeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTRICTEDZIPCODE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestrictedZipCodeList _RestrictedZipCodeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestrictedZipCodeList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestrictedZipCode objects by query String
        /// </summary>
        /// <returns>A list of RestrictedZipCode objects</returns>
		public RestrictedZipCodeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTRICTEDZIPCODEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}

        #endregion
        public DataSet GetAllRestrictedZipCode(int pageno, int pagesize,string searchtext)

        {
           
            string searchQuery = "";
            string sqlQuery = "";
            string filterquery = "";
            string DateQuery = "";
            int pagestart = (pageno - 1) * pagesize;
            int pageend = pagesize;
            string subquery = "";
          
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                searchQuery = string.Format("and rz.ZipCode like '%{0}%'", searchtext);
            }
      

            sqlQuery = @"
                                

								select *
								into #RZData
								from RestrictedZipCode rz
                                where rz.Id>0 
                                {4}
					
                                
								select * into #RZIdData from #RZData
								select top({1}) *  from #RZIdData
								where Id not in (Select TOP ({2}) Id from #RZData #rz order by #rz.ZipCode asc )
							    order by ZipCode Asc

								 
							
                                  

                                select Count(Id) As TotalCount from #RZData
                    

								drop table #RZData
								drop table #RZIdData";
            sqlQuery = string.Format(sqlQuery, pageno, pagesize, pagestart, pageend, searchQuery);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetRestrictedZipcodelistbyzipcode(string searchtext)

        {

            string searchQuery = "";
            string sqlQuery = "";




            sqlQuery = @"
                                

								select * into #RestrictedZipCodeTab
						
								from RestrictedZipCode rz
                                where rz.Id>0 
                                and rz.ZipCode = '{0}'
					
                                
							
							    order by ZipCode Asc

								 
							
                                  
	                             select * from  #RestrictedZipCodeTab 
                                select Count(Id) As TotalCount from  #RestrictedZipCodeTab 
								drop table #RestrictedZipCodeTab
                             "


                            ;
            sqlQuery = string.Format(sqlQuery, searchtext);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Get RestrictedZipCode Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of RestrictedZipCode
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTRICTEDZIPCODEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestrictedZipCode Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestrictedZipCode
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestrictedZipCodeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTRICTEDZIPCODEROWCOUNT))
			{
				SqlDataReader reader;
				_RestrictedZipCodeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestrictedZipCodeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestrictedZipCode object
        /// </summary>
        /// <param name="restrictedZipCodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestrictedZipCodeBase restrictedZipCodeObject, SqlDataReader reader, int start)
		{
			
				restrictedZipCodeObject.Id = reader.GetInt32( start + 0 );			
				restrictedZipCodeObject.Zipcode = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) restrictedZipCodeObject.CreatedBy = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restrictedZipCodeObject.CreatedDate = reader.GetDateTime( start + 3 );			
			FillBaseObject(restrictedZipCodeObject, reader, (start + 4));

			
			restrictedZipCodeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestrictedZipCode object
        /// </summary>
        /// <param name="restrictedZipCodeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestrictedZipCodeBase restrictedZipCodeObject, SqlDataReader reader)
		{
			FillObject(restrictedZipCodeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestrictedZipCode object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestrictedZipCode object</returns>
		private RestrictedZipCode GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestrictedZipCode restrictedZipCodeObject= new RestrictedZipCode();
					FillObject(restrictedZipCodeObject, reader);
					return restrictedZipCodeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestrictedZipCode objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestrictedZipCode objects</returns>
		private RestrictedZipCodeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestrictedZipCode list
			RestrictedZipCodeList list = new RestrictedZipCodeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestrictedZipCode restrictedZipCodeObject = new RestrictedZipCode();
					FillObject(restrictedZipCodeObject, reader);

					list.Add(restrictedZipCodeObject);
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
