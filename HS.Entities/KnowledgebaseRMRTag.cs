using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class KnowledgebaseRMRTag 
	{
		public string CreatedUser { get; set; }
		public int UsedCount { get; set; }
	}
}