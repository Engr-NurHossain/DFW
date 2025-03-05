using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorFileList", Namespace = "http://www.piistech.com//list")]	
	public class EstimatorFileList : BaseCollection<EstimatorFile>
	{
		#region Constructors
	    public EstimatorFileList() : base() { }
        public EstimatorFileList(EstimatorFile[] list) : base(list) { }
        public EstimatorFileList(List<EstimatorFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
