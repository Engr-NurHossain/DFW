using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class ResturantOrderDetail 
	{
		public string Toppings { get; set; }
		public string SpecialInstruction { get; set; }
		public string ItemDescription { get; set; }
		public double NetPrice { get; set; }
	}
}
