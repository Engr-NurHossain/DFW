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
	public partial class ManufacturerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMANUFACTURER = "InsertManufacturer";
		private const string UPDATEMANUFACTURER = "UpdateManufacturer";
		private const string DELETEMANUFACTURER = "DeleteManufacturer";
		private const string GETMANUFACTURERBYID = "GetManufacturerById";
		private const string GETALLMANUFACTURER = "GetAllManufacturer";
		private const string GETPAGEDMANUFACTURER = "GetPagedManufacturer";
		private const string GETMANUFACTURERMAXIMUMID = "GetManufacturerMaximumId";
		private const string GETMANUFACTURERROWCOUNT = "GetManufacturerRowCount";	
		private const string GETMANUFACTURERBYQUERY = "GetManufacturerByQuery";
		#endregion
		
		#region Constructors
		public ManufacturerDataAccess(ClientContext context) : base(context) { }
		public ManufacturerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="manufacturerObject"></param>
		private void AddCommonParams(SqlCommand cmd, ManufacturerBase manufacturerObject)
		{	
			AddParameter(cmd, pGuid(ManufacturerBase.Property_CompanyId, manufacturerObject.CompanyId));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_Name, 50, manufacturerObject.Name));
			AddParameter(cmd, pInt32(ManufacturerBase.Property_OrderBy, manufacturerObject.OrderBy));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_ContactPerson, 50, manufacturerObject.ContactPerson));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_Phone, 50, manufacturerObject.Phone));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_EmailAddress, 50, manufacturerObject.EmailAddress));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_Street, 500, manufacturerObject.Street));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_City, 50, manufacturerObject.City));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_State, 50, manufacturerObject.State));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_Zipcode, 50, manufacturerObject.Zipcode));
			AddParameter(cmd, pNVarChar(ManufacturerBase.Property_Country, 50, manufacturerObject.Country));
			AddParameter(cmd, pBool(ManufacturerBase.Property_IsActive, manufacturerObject.IsActive));
			AddParameter(cmd, pGuid(ManufacturerBase.Property_ManufacturerId, manufacturerObject.ManufacturerId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Manufacturer
        /// </summary>
        /// <param name="manufacturerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ManufacturerBase manufacturerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMANUFACTURER);
	
				AddParameter(cmd, pInt32Out(ManufacturerBase.Property_Id));
				AddCommonParams(cmd, manufacturerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					manufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					manufacturerObject.Id = (Int32)GetOutParameter(cmd, ManufacturerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(manufacturerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Manufacturer
        /// </summary>
        /// <param name="manufacturerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ManufacturerBase manufacturerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMANUFACTURER);
				
				AddParameter(cmd, pInt32(ManufacturerBase.Property_Id, manufacturerObject.Id));
				AddCommonParams(cmd, manufacturerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					manufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(manufacturerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Manufacturer
        /// </summary>
        /// <param name="Id">Id of the Manufacturer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMANUFACTURER);	
				
				AddParameter(cmd, pInt32(ManufacturerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Manufacturer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Manufacturer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Manufacturer object to retrieve</param>
        /// <returns>Manufacturer object, null if not found</returns>
		public Manufacturer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMANUFACTURERBYID))
			{
				AddParameter( cmd, pInt32(ManufacturerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
        public DataTable GetAllManufacturersExport(Guid companyId, string name)

        {
            
            string sqlQuery = @"";
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(name))
            {
                searchquery = string.Format("and Name like '%{0}%'", name);
            }

            sqlQuery = @"		select
                                   Name from Manufacturer where Id>0 {1}
                            ";
                sqlQuery = string.Format(sqlQuery, companyId, searchquery);
            
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region GetAll Method
        /// <summary>
        /// Retrieves all Manufacturer objects 
        /// </summary>
        /// <returns>A list of Manufacturer objects</returns>
        public ManufacturerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMANUFACTURER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Manufacturer objects by PageRequest
        /// </summary>
        /// <returns>A list of Manufacturer objects</returns>
		public ManufacturerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMANUFACTURER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ManufacturerList _ManufacturerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ManufacturerList;
			}
		}
		
		/// <summary>
        /// Retrieves all Manufacturer objects by query String
        /// </summary>
        /// <returns>A list of Manufacturer objects</returns>
		public ManufacturerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMANUFACTURERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Manufacturer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Manufacturer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMANUFACTURERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Manufacturer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Manufacturer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ManufacturerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMANUFACTURERROWCOUNT))
			{
				SqlDataReader reader;
				_ManufacturerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ManufacturerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Manufacturer object
        /// </summary>
        /// <param name="manufacturerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ManufacturerBase manufacturerObject, SqlDataReader reader, int start)
		{
			
				manufacturerObject.Id = reader.GetInt32( start + 0 );			
				manufacturerObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) manufacturerObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) manufacturerObject.OrderBy = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) manufacturerObject.ContactPerson = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) manufacturerObject.Phone = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) manufacturerObject.EmailAddress = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) manufacturerObject.Street = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) manufacturerObject.City = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) manufacturerObject.State = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) manufacturerObject.Zipcode = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) manufacturerObject.Country = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) manufacturerObject.IsActive = reader.GetBoolean( start + 12 );			
				manufacturerObject.ManufacturerId = reader.GetGuid( start + 13 );			
			FillBaseObject(manufacturerObject, reader, (start + 14));

			
			manufacturerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Manufacturer object
        /// </summary>
        /// <param name="manufacturerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ManufacturerBase manufacturerObject, SqlDataReader reader)
		{
			FillObject(manufacturerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Manufacturer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Manufacturer object</returns>
		private Manufacturer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Manufacturer manufacturerObject= new Manufacturer();
					FillObject(manufacturerObject, reader);
					return manufacturerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Manufacturer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Manufacturer objects</returns>
		private ManufacturerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Manufacturer list
			ManufacturerList list = new ManufacturerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Manufacturer manufacturerObject = new Manufacturer();
					FillObject(manufacturerObject, reader);

					list.Add(manufacturerObject);
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
