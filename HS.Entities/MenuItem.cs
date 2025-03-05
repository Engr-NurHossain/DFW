using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class MenuItem 
	{
        public int MenuId { get; set; }
        public List<ToppingCategory> Toppings { get; set; }
        public List<Category> Categories { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string CategoryModel { get; set; }
        public string ToppingModel { get; set; }
        public List<string> CategoryNameList { get; set; }
        public List<string> ToppingNameList { get; set; }
        public List<string> AvailableDays { get; set; }
        public string MenuName { get; set; }
        public WebsiteLocation WebsiteLocation { get; set; }
        public Menu Menus { get; set; }
        public Category category { get; set; }
    }

    
}
