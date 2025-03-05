using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "LanguageList", Namespace = "http://www.piistech.com//list")]	
	public class LanguageList : BaseCollection<Language>
	{
		#region Constructors
	    public LanguageList() : base() { }
        public LanguageList(Language[] list) : base(list) { }
        public LanguageList(List<Language> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

