using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class RestToppingCategory 
	{
		public string ToppingName { get; set; }

		public string ToppingPrice { get; set; }

		public int CategoryId { get; set; }
	}
}
