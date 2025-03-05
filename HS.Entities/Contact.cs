using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Contact 
	{

        public string Name { get; set; }
        public string CreatedByName { get; set; }
        public List<Guid> CustomerId { get; set; }
        public List<Guid> LeadId { get; set; }
        public List<Guid> OpportunityId { get; set; }
        public string FromCustomer { get; set; }
        public List<UserContact> UserContacts { set; get; }
        public bool ContactTab { get; set; }
        public string ContactOwnerVal { get; set; }
        public LeadCorrespondence LeadCorrespondence { get; set; }
        public List<RMRTagCustomModel> TagList { get; set; }
        public string ListTag { get; set; }
        public bool IsWorkNoVerified { get; set; }
        public bool IsMobileVerified { get; set; }
    }
    public class ContactCount
    {
        public int TotalCount { get; set; }
    }
    public class ContactModel
    {
        public List<Contact> ContactList { get; set; }
        public ContactCount TotalCount { get; set; }
    }
    public class ContactFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string  Order { get; set; }
        public string SearchText { get; set; }
        public string UserTag { get; set; }
        public Guid FromCustomer { get; set; }
        public string Mobile { get; set; }
        public string Work { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Email { get; set; }
        public Guid ContactOwnerId { get; set; }
        public List<string> ContactsList { get; set; }
        public string Identifier { get; set; }
        public string soldby { get; set; }
    }
}
