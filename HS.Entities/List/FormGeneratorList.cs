using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FormGeneratorList", Namespace = "http://www.piistech.com//list")]	
	public class FormGeneratorList : BaseCollection<FormGenerator>
	{
		#region Constructors
	    public FormGeneratorList() : base() { }
        public FormGeneratorList(FormGenerator[] list) : base(list) { }
        public FormGeneratorList(List<FormGenerator> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

