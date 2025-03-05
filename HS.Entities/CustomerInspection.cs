using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerInspection 
	{
        public int CusId { get; set; }
        public string PMSignatureImagePath { get; set; }
        public string HomeOwnerSignatureImagePath { get; set; }
        public string DrawingImagePath { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public string customerAddress { get; set; }
        public string customerStreet { get; set; }
        public string customerCity { get; set; }
        public string customerState { get; set; }
        public string customerZipCode { get; set; }
        public string customerPrimaryPhone { get; set; }
        public string customerSecondaryPhone { get; set; }
        public string customerEmail { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public string companyStreet { get; set; }
        public string companyCity { get; set; }
        public string companyState { get; set; }
        public string companyZipCode { get; set; }
        public string companyPhone { get; set; }
        public string companyFax { get; set; }
        public string companyEmail { get; set; }
        public string companyLogo { get; set; }
        public string companyWebsite { get; set; }
        public string StrGroundWaterRating { get; set; }
        public string StrIronBacteriaRating { get; set; }
        public string StrCondensationRating { get; set; }
        public string StrWallCracksRating { get; set; }
        public string StrFloorCracksRating { get; set; }
    }
    
}
