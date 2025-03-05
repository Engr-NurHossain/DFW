using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SMSTemplateList", Namespace = "http://www.piistech.com//list")]	
	public class SMSTemplateList : BaseCollection<SMSTemplate>
	{
		#region Constructors
	    public SMSTemplateList() : base() { }
        public SMSTemplateList(SMSTemplate[] list) : base(list) { }
        public SMSTemplateList(List<SMSTemplate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

