using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class RestMenuItem 
	{
        public int MenuId { get; set; }
        public List<RestToppingCategory> Toppings { get; set; }
        public List<RestCategory> Categories { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string CategoryModel { get; set; }
        public string ToppingModel { get; set; }
        public List<string> CategoryNameList { get; set; }
        public List<string> ToppingNameList { get; set; }
        public List<string> AvailableDays { get; set; }
        public string MenuName { get; set; }
        public WebsiteLocation WebsiteLocation { get; set; }
        public RestMenu Menus { get; set; }
        public Category category { get; set; }
        public string MenuStr { get; set; }
        public List<RestMenuItemAdditionalContent> ListAdditionalContent { get; set; }
        public int PrevMenuId { get; set; }
    }

    public class MenuItemListModel
    {
        public List<RestMenuItem> MenuItems { get; set; }
        public string Searchtext { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string order { get; set; }
        public int TotalCount { get; set; }
    }
}
