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
	public partial class EstimateImageDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATEIMAGE = "InsertEstimateImage";
		private const string UPDATEESTIMATEIMAGE = "UpdateEstimateImage";
		private const string DELETEESTIMATEIMAGE = "DeleteEstimateImage";
		private const string GETESTIMATEIMAGEBYID = "GetEstimateImageById";
		private const string GETALLESTIMATEIMAGE = "GetAllEstimateImage";
		private const string GETPAGEDESTIMATEIMAGE = "GetPagedEstimateImage";
		private const string GETESTIMATEIMAGEMAXIMUMID = "GetEstimateImageMaximumId";
		private const string GETESTIMATEIMAGEROWCOUNT = "GetEstimateImageRowCount";	
		private const string GETESTIMATEIMAGEBYQUERY = "GetEstimateImageByQuery";
		#endregion
		
		#region Constructors
		public EstimateImageDataAccess(ClientContext context) : base(context) { }
		public EstimateImageDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimateImageObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimateImageBase estimateImageObject)
		{	
			AddParameter(cmd, pGuid(EstimateImageBase.Property_CompanyId, estimateImageObject.CompanyId));
			AddParameter(cmd, pGuid(EstimateImageBase.Property_CustomerId, estimateImageObject.CustomerId));
			AddParameter(cmd, pNVarChar(EstimateImageBase.Property_InvoiceId, 250, estimateImageObject.InvoiceId));
			AddParameter(cmd, pNVarChar(EstimateImageBase.Property_ImageLoc, estimateImageObject.ImageLoc));
			AddParameter(cmd, pNVarChar(EstimateImageBase.Property_ImageType, 250, estimateImageObject.ImageType));
			AddParameter(cmd, pDateTime(EstimateImageBase.Property_SignDate, estimateImageObject.SignDate));
			AddParameter(cmd, pDateTime(EstimateImageBase.Property_UploadedDate, estimateImageObject.UploadedDate));
			AddParameter(cmd, pGuid(EstimateImageBase.Property_CreatedBy, estimateImageObject.CreatedBy));
			AddParameter(cmd, pBool(EstimateImageBase.Property_IsDocument, estimateImageObject.IsDocument));
			AddParameter(cmd, pDouble(EstimateImageBase.Property_Size, estimateImageObject.Size));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimateImage
        /// </summary>
        /// <param name="estimateImageObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimateImageBase estimateImageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATEIMAGE);
	
				AddParameter(cmd, pInt32Out(EstimateImageBase.Property_Id));
				AddCommonParams(cmd, estimateImageObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimateImageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimateImageObject.Id = (Int32)GetOutParameter(cmd, EstimateImageBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimateImageObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimateImage
        /// </summary>
        /// <param name="estimateImageObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimateImageBase estimateImageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATEIMAGE);
				
				AddParameter(cmd, pInt32(EstimateImageBase.Property_Id, estimateImageObject.Id));
				AddCommonParams(cmd, estimateImageObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimateImageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimateImageObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimateImage
        /// </summary>
        /// <param name="Id">Id of the EstimateImage object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATEIMAGE);	
				
				AddParameter(cmd, pInt32(EstimateImageBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimateImage), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimateImage object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimateImage object to retrieve</param>
        /// <returns>EstimateImage object, null if not found</returns>
		public EstimateImage Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATEIMAGEBYID))
			{
				AddParameter( cmd, pInt32(EstimateImageBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimateImage objects 
        /// </summary>
        /// <returns>A list of EstimateImage objects</returns>
		public EstimateImageList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATEIMAGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimateImage objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimateImage objects</returns>
		public EstimateImageList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATEIMAGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimateImageList _EstimateImageList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimateImageList;
			}
		}
		
		/// <summary>
        /// Retrieves all EstimateImage objects by query String
        /// </summary>
        /// <returns>A list of EstimateImage objects</returns>
		public EstimateImageList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATEIMAGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimateImage Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimateImage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATEIMAGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimateImage Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimateImage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimateImageRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATEIMAGEROWCOUNT))
			{
				SqlDataReader reader;
				_EstimateImageRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimateImageRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimateImage object
        /// </summary>
        /// <param name="estimateImageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimateImageBase estimateImageObject, SqlDataReader reader, int start)
		{
			
				estimateImageObject.Id = reader.GetInt32( start + 0 );			
				estimateImageObject.CompanyId = reader.GetGuid( start + 1 );			
				estimateImageObject.CustomerId = reader.GetGuid( start + 2 );			
				estimateImageObject.InvoiceId = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) estimateImageObject.ImageLoc = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) estimateImageObject.ImageType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) estimateImageObject.SignDate = reader.GetDateTime( start + 6 );			
				estimateImageObject.UploadedDate = reader.GetDateTime( start + 7 );			
				estimateImageObject.CreatedBy = reader.GetGuid( start + 8 );			
				estimateImageObject.IsDocument = reader.GetBoolean( start + 9 );			
				estimateImageObject.Size = reader.GetDouble( start + 10 );			
			FillBaseObject(estimateImageObject, reader, (start + 11));

			
			estimateImageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimateImage object
        /// </summary>
        /// <param name="estimateImageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimateImageBase estimateImageObject, SqlDataReader reader)
		{
			FillObject(estimateImageObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimateImage object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimateImage object</returns>
		private EstimateImage GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimateImage estimateImageObject= new EstimateImage();
					FillObject(estimateImageObject, reader);
					return estimateImageObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimateImage objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimateImage objects</returns>
		private EstimateImageList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimateImage list
			EstimateImageList list = new EstimateImageList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimateImage estimateImageObject = new EstimateImage();
					FillObject(estimateImageObject, reader);

					list.Add(estimateImageObject);
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
