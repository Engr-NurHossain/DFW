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
	public partial class SeoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSEO = "InsertSeo";
		private const string UPDATESEO = "UpdateSeo";
		private const string DELETESEO = "DeleteSeo";
		private const string GETSEOBYID = "GetSeoById";
		private const string GETALLSEO = "GetAllSeo";
		private const string GETPAGEDSEO = "GetPagedSeo";
		private const string GETSEOMAXIMUMID = "GetSeoMaximumId";
		private const string GETSEOROWCOUNT = "GetSeoRowCount";	
		private const string GETSEOBYQUERY = "GetSeoByQuery";
		#endregion
		
		#region Constructors
		public SeoDataAccess(ClientContext context) : base(context) { }
		public SeoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="seoObject"></param>
		private void AddCommonParams(SqlCommand cmd, SeoBase seoObject)
		{	
			AddParameter(cmd, pGuid(SeoBase.Property_CompanyId, seoObject.CompanyId));
			AddParameter(cmd, pNVarChar(SeoBase.Property_PageUrl, seoObject.PageUrl));
			AddParameter(cmd, pNVarChar(SeoBase.Property_MetaTitle, seoObject.MetaTitle));
			AddParameter(cmd, pNVarChar(SeoBase.Property_MetaDescription, seoObject.MetaDescription));
			AddParameter(cmd, pNVarChar(SeoBase.Property_MetaKeywords, seoObject.MetaKeywords));
			AddParameter(cmd, pNVarChar(SeoBase.Property_OgTitle, seoObject.OgTitle));
			AddParameter(cmd, pNVarChar(SeoBase.Property_OgType, seoObject.OgType));
			AddParameter(cmd, pNVarChar(SeoBase.Property_OgImage, seoObject.OgImage));
			AddParameter(cmd, pNVarChar(SeoBase.Property_OgUrl, seoObject.OgUrl));
			AddParameter(cmd, pNVarChar(SeoBase.Property_OgDescription, seoObject.OgDescription));
			AddParameter(cmd, pNVarChar(SeoBase.Property_TwitterCard, seoObject.TwitterCard));
			AddParameter(cmd, pNVarChar(SeoBase.Property_TwitterUrl, seoObject.TwitterUrl));
			AddParameter(cmd, pNVarChar(SeoBase.Property_TwitterTitle, seoObject.TwitterTitle));
			AddParameter(cmd, pNVarChar(SeoBase.Property_TwitterDescription, seoObject.TwitterDescription));
			AddParameter(cmd, pNVarChar(SeoBase.Property_TwitterImage, seoObject.TwitterImage));
			AddParameter(cmd, pNVarChar(SeoBase.Property_ItemScopePageType, seoObject.ItemScopePageType));
			AddParameter(cmd, pNVarChar(SeoBase.Property_ItemPropName, seoObject.ItemPropName));
			AddParameter(cmd, pNVarChar(SeoBase.Property_ItemPropTitle, seoObject.ItemPropTitle));
			AddParameter(cmd, pNVarChar(SeoBase.Property_ItemPropDescription, seoObject.ItemPropDescription));
			AddParameter(cmd, pNVarChar(SeoBase.Property_ItemPropImage, seoObject.ItemPropImage));
			AddParameter(cmd, pBool(SeoBase.Property_IsActive, seoObject.IsActive));
			AddParameter(cmd, pNVarChar(SeoBase.Property_Name, 150, seoObject.Name));
			AddParameter(cmd, pBool(SeoBase.Property_IsFolder, seoObject.IsFolder));
			AddParameter(cmd, pNVarChar(SeoBase.Property_FolderOption, 250, seoObject.FolderOption));
			AddParameter(cmd, pBool(SeoBase.Property_IsNav, seoObject.IsNav));
			AddParameter(cmd, pNVarChar(SeoBase.Property_PublishOption, 250, seoObject.PublishOption));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Seo
        /// </summary>
        /// <param name="seoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(SeoBase seoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSEO);
	
				AddParameter(cmd, pInt32Out(SeoBase.Property_Id));
				AddCommonParams(cmd, seoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					seoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					seoObject.Id = (Int32)GetOutParameter(cmd, SeoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(seoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Seo
        /// </summary>
        /// <param name="seoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(SeoBase seoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESEO);
				
				AddParameter(cmd, pInt32(SeoBase.Property_Id, seoObject.Id));
				AddCommonParams(cmd, seoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					seoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(seoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Seo
        /// </summary>
        /// <param name="Id">Id of the Seo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESEO);	
				
				AddParameter(cmd, pInt32(SeoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Seo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Seo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Seo object to retrieve</param>
        /// <returns>Seo object, null if not found</returns>
		public Seo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSEOBYID))
			{
				AddParameter( cmd, pInt32(SeoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Seo objects 
        /// </summary>
        /// <returns>A list of Seo objects</returns>
		public SeoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSEO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Seo objects by PageRequest
        /// </summary>
        /// <returns>A list of Seo objects</returns>
		public SeoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSEO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				SeoList _SeoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _SeoList;
			}
		}
		
		/// <summary>
        /// Retrieves all Seo objects by query String
        /// </summary>
        /// <returns>A list of Seo objects</returns>
		public SeoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSEOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Seo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Seo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSEOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Seo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Seo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _SeoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSEOROWCOUNT))
			{
				SqlDataReader reader;
				_SeoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _SeoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Seo object
        /// </summary>
        /// <param name="seoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(SeoBase seoObject, SqlDataReader reader, int start)
		{
			
				seoObject.Id = reader.GetInt32( start + 0 );			
				seoObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) seoObject.PageUrl = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) seoObject.MetaTitle = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) seoObject.MetaDescription = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) seoObject.MetaKeywords = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) seoObject.OgTitle = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) seoObject.OgType = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) seoObject.OgImage = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) seoObject.OgUrl = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) seoObject.OgDescription = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) seoObject.TwitterCard = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) seoObject.TwitterUrl = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) seoObject.TwitterTitle = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) seoObject.TwitterDescription = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) seoObject.TwitterImage = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) seoObject.ItemScopePageType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) seoObject.ItemPropName = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) seoObject.ItemPropTitle = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) seoObject.ItemPropDescription = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) seoObject.ItemPropImage = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) seoObject.IsActive = reader.GetBoolean( start + 21 );			
				if(!reader.IsDBNull(22)) seoObject.Name = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) seoObject.IsFolder = reader.GetBoolean( start + 23 );			
				if(!reader.IsDBNull(24)) seoObject.FolderOption = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) seoObject.IsNav = reader.GetBoolean( start + 25 );			
				if(!reader.IsDBNull(26)) seoObject.PublishOption = reader.GetString( start + 26 );			
			FillBaseObject(seoObject, reader, (start + 27));

			
			seoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Seo object
        /// </summary>
        /// <param name="seoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(SeoBase seoObject, SqlDataReader reader)
		{
			FillObject(seoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Seo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Seo object</returns>
		private Seo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Seo seoObject= new Seo();
					FillObject(seoObject, reader);
					return seoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Seo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Seo objects</returns>
		private SeoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Seo list
			SeoList list = new SeoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Seo seoObject = new Seo();
					FillObject(seoObject, reader);

					list.Add(seoObject);
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
