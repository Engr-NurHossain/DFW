using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class DocumentLibrary 
	{
        public int AttachmentCount { get; set; }
        public string UpadtedBy { get; set; }
        public List<DocumentLibraryWeblink> DocumentLibraryWeblinkList { get; set; }
        public List<DocumentLibraryWeblink> RelatedArticleList { get; set; } 
        public List<KnowledgeImage> Images { get; set; }
        public List<EstimateImage> SavedImages { get; set; }
        public List<string> TagsStr { get; set; }
        public string FlagByName { get; set; }
        public List<KnowledgeBaseFlagUserCustom> ListKnowledgeBaseFlagUser { get; set; }
        public string Comments { get; set; }
    }
}