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
	public partial class RecruitmentDocFormDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRECRUITMENTDOCFORM = "InsertRecruitmentDocForm";
		private const string UPDATERECRUITMENTDOCFORM = "UpdateRecruitmentDocForm";
		private const string DELETERECRUITMENTDOCFORM = "DeleteRecruitmentDocForm";
		private const string GETRECRUITMENTDOCFORMBYID = "GetRecruitmentDocFormById";
		private const string GETALLRECRUITMENTDOCFORM = "GetAllRecruitmentDocForm";
		private const string GETPAGEDRECRUITMENTDOCFORM = "GetPagedRecruitmentDocForm";
		private const string GETRECRUITMENTDOCFORMMAXIMUMID = "GetRecruitmentDocFormMaximumId";
		private const string GETRECRUITMENTDOCFORMROWCOUNT = "GetRecruitmentDocFormRowCount";	
		private const string GETRECRUITMENTDOCFORMBYQUERY = "GetRecruitmentDocFormByQuery";
		#endregion
		
		#region Constructors
		public RecruitmentDocFormDataAccess(ClientContext context) : base(context) { }
		public RecruitmentDocFormDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="recruitmentDocFormObject"></param>
		private void AddCommonParams(SqlCommand cmd, RecruitmentDocFormBase recruitmentDocFormObject)
		{	
			AddParameter(cmd, pGuid(RecruitmentDocFormBase.Property_FormId, recruitmentDocFormObject.FormId));
			AddParameter(cmd, pNVarChar(RecruitmentDocFormBase.Property_Name, 250, recruitmentDocFormObject.Name));
			AddParameter(cmd, pNVarChar(RecruitmentDocFormBase.Property_FileLocation, 500, recruitmentDocFormObject.FileLocation));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RecruitmentDocForm
        /// </summary>
        /// <param name="recruitmentDocFormObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RecruitmentDocFormBase recruitmentDocFormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRECRUITMENTDOCFORM);
	
				AddParameter(cmd, pInt32Out(RecruitmentDocFormBase.Property_Id));
				AddCommonParams(cmd, recruitmentDocFormObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					recruitmentDocFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					recruitmentDocFormObject.Id = (Int32)GetOutParameter(cmd, RecruitmentDocFormBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(recruitmentDocFormObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RecruitmentDocForm
        /// </summary>
        /// <param name="recruitmentDocFormObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RecruitmentDocFormBase recruitmentDocFormObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERECRUITMENTDOCFORM);
				
				AddParameter(cmd, pInt32(RecruitmentDocFormBase.Property_Id, recruitmentDocFormObject.Id));
				AddCommonParams(cmd, recruitmentDocFormObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					recruitmentDocFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(recruitmentDocFormObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RecruitmentDocForm
        /// </summary>
        /// <param name="Id">Id of the RecruitmentDocForm object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERECRUITMENTDOCFORM);	
				
				AddParameter(cmd, pInt32(RecruitmentDocFormBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RecruitmentDocForm), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RecruitmentDocForm object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RecruitmentDocForm object to retrieve</param>
        /// <returns>RecruitmentDocForm object, null if not found</returns>
		public RecruitmentDocForm Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTDOCFORMBYID))
			{
				AddParameter( cmd, pInt32(RecruitmentDocFormBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RecruitmentDocForm objects 
        /// </summary>
        /// <returns>A list of RecruitmentDocForm objects</returns>
		public RecruitmentDocFormList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRECRUITMENTDOCFORM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RecruitmentDocForm objects by PageRequest
        /// </summary>
        /// <returns>A list of RecruitmentDocForm objects</returns>
		public RecruitmentDocFormList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRECRUITMENTDOCFORM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RecruitmentDocFormList _RecruitmentDocFormList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RecruitmentDocFormList;
			}
		}
		
		/// <summary>
        /// Retrieves all RecruitmentDocForm objects by query String
        /// </summary>
        /// <returns>A list of RecruitmentDocForm objects</returns>
		public RecruitmentDocFormList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTDOCFORMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RecruitmentDocForm Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RecruitmentDocForm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTDOCFORMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RecruitmentDocForm Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RecruitmentDocForm
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RecruitmentDocFormRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRECRUITMENTDOCFORMROWCOUNT))
			{
				SqlDataReader reader;
				_RecruitmentDocFormRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RecruitmentDocFormRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RecruitmentDocForm object
        /// </summary>
        /// <param name="recruitmentDocFormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RecruitmentDocFormBase recruitmentDocFormObject, SqlDataReader reader, int start)
		{
			
				recruitmentDocFormObject.Id = reader.GetInt32( start + 0 );			
				recruitmentDocFormObject.FormId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) recruitmentDocFormObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) recruitmentDocFormObject.FileLocation = reader.GetString( start + 3 );			
			FillBaseObject(recruitmentDocFormObject, reader, (start + 4));

			
			recruitmentDocFormObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RecruitmentDocForm object
        /// </summary>
        /// <param name="recruitmentDocFormObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RecruitmentDocFormBase recruitmentDocFormObject, SqlDataReader reader)
		{
			FillObject(recruitmentDocFormObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RecruitmentDocForm object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RecruitmentDocForm object</returns>
		private RecruitmentDocForm GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RecruitmentDocForm recruitmentDocFormObject= new RecruitmentDocForm();
					FillObject(recruitmentDocFormObject, reader);
					return recruitmentDocFormObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RecruitmentDocForm objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RecruitmentDocForm objects</returns>
		private RecruitmentDocFormList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RecruitmentDocForm list
			RecruitmentDocFormList list = new RecruitmentDocFormList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RecruitmentDocForm recruitmentDocFormObject = new RecruitmentDocForm();
					FillObject(recruitmentDocFormObject, reader);

					list.Add(recruitmentDocFormObject);
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
