using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorNoteList", Namespace = "http://www.hims-tech.com//list")]	
	public class EstimatorNoteList : BaseCollection<EstimatorNote>
	{
		#region Constructors
	    public EstimatorNoteList() : base() { }
        public EstimatorNoteList(EstimatorNote[] list) : base(list) { }
        public EstimatorNoteList(List<EstimatorNote> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
