using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class KnowledgeBaseFlagUser 
	{
		
	}
	public class KnowledgeBaseFlagUserCustom
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public DateTime Date { get; set; }
		public string DateC { get; set; }
	}
}