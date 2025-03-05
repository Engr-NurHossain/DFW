using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ContactList", Namespace = "http://www.piistech.com//list")]	
	public class ContactList : BaseCollection<Contact>
	{
		#region Constructors
	    public ContactList() : base() { }
        public ContactList(Contact[] list) : base(list) { }
        public ContactList(List<Contact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

