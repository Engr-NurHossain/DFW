using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Knowledgebase 
	{
         
        public int AttachmentCount { get; set; }
        public Boolean IsFavourite { get; set; }
        public string UpadtedBy { get; set; }
        public string Comments { get; set; }
        public List<KnowledgebaseWeblink> KnowledgeWeblinkList { get; set; }
        public List<KnowledgebaseWeblink> RelatedArticleList { get; set; }
        public List<KnowledgeImage> Images { get; set; }
        public List<EstimateImage> SavedImages { get; set; }
        public List<string> TagsStr { get; set; }
        public string FlagByName { get; set; }
        public List<Knowledgebase> SearchedKnowledgebase { get; set; }
        public List<Knowledgebase> DeletedKnowledgebaseList { get; set; }
        public List<KnowledgeBaseFlagUserCustom> ListKnowledgeBaseFlagUser { get; set; }
        public int TotalCount { get; set; }
        public List<int> UserGroups { get; set; }
        public bool IsDefault { get; set; }
        public List<string> StrUserGroups { get; set; }
        public KnowledgebaseAccountability IsAssigned { get; set; }
        public string AssignTo { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class KnowledgebaseListModel
    {
        public List<Knowledgebase> KnowledgebaseList { get; set; }
        public Guid CompanyId { get; set; }
        public int TotalCount { get; set; }
        public int DeletedCount { get; set; }
        public int TotalKnFlagCount { get; set; }
        public int TotalKnFavoriteCount { get; set; }
        public int TotalKnDeleteFavoriteCount { get; set; }
    }
    public class KnowledgebaseHomeModel
    {
        public List<Knowledgebase> RecentViewedlist { get; set; }
        public List<Knowledgebase> MostViewedlist { get; set; }
        public List<KnowledgeSearchedKeyword> KnowledgeSearchedKeywordList { get; set; }
        public List<KnowledgebaseRMRTag> RMRTagList { get; set; }
        public int FlaggedCount { get; set; }
        public int FavoriteCount { get; set; }
    }
    public class AccessedKnowledgebase
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public bool IsDefault { get; set; }
    }
    public class QtiFilter
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public string Order { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDownload { get; set; }
        public bool IsDocumentLibrary { get; set; }
        public string Tag { get; set; }
        public string assignTo { get; set; }
        public bool IsFlaged { get; set; }
        public string UserRole { get; set; }
        public string IsContact { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> NavList { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsContent { get; set; }
        public bool? IsComplete { get; set; }
        public bool IsFavorite { get; set; }


    }
}