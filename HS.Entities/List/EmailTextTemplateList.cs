using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmailTextTemplateList", Namespace = "http://www.piistech.com//list")]	
	public class EmailTextTemplateList : BaseCollection<EmailTextTemplate>
	{
		#region Constructors
	    public EmailTextTemplateList() : base() { }
        public EmailTextTemplateList(EmailTextTemplate[] list) : base(list) { }
        public EmailTextTemplateList(List<EmailTextTemplate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

