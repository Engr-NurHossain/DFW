using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class RestMenu 
	{
        public int MenuItemId { get; set; }
        public List<MenuItemDetail> MenuItemDetails { get; set; }
        public List<RestMenuItem> MenuItems { get; set; }
        public List<RestTopping> Toppings { get; set; }
        public List<RestCategory> Categories { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public List<string> CategoryModel { get; set; }
        public int NumberOfItems { get; set; }
        public List<string> AvailableDays { get; set; }
        public string ItemPhoto { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string ItemPrice { get; set; }
        public WebsiteLocation WebsiteLocation { get; set; }
        public RestCategory Category { get; set; }
    }

    public class MenuListModel
    {
        public List<RestMenu> Menus { get; set; }
        public string Searchtext { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string order { get; set; }
        public int TotalCount { get; set; }

        public WebsiteLocation WebsiteLocation { get; set; }
    }
}
