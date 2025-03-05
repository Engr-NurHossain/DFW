using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerMigrationList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerMigrationList : BaseCollection<CustomerMigration>
	{
		#region Constructors
	    public CustomerMigrationList() : base() { }
        public CustomerMigrationList(CustomerMigration[] list) : base(list) { }
        public CustomerMigrationList(List<CustomerMigration> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

