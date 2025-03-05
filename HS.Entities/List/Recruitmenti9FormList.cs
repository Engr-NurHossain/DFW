using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "Recruitmenti9FormList", Namespace = "http://www.piistech.com//list")]	
	public class Recruitmenti9FormList : BaseCollection<Recruitmenti9Form>
	{
		#region Constructors
	    public Recruitmenti9FormList() : base() { }
        public Recruitmenti9FormList(Recruitmenti9Form[] list) : base(list) { }
        public Recruitmenti9FormList(List<Recruitmenti9Form> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

