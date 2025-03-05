using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class SmartPackage 
	{
        public MMRRange MMRRange { get; set; }
        public double MMRMax { get; set; }
        public double MMRMin { get; set; }
       
        public string SystemType { get; set; }
        public string InstallType { get; set; }
        public int SmartSystemTypeId { get; set; }
        public int SmartInstallTypeId { get; set; }
        public string LastUpdatedName { get; set; }
    }

    public class SmartPackageDropdownList
    {
        public double? MinCredit { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public partial class SmartPackageFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public Guid CompanyId { get; set; }
        public string FilterText { get; set; }
    }
    public class PackageCount
    {
        public int TotalCount { get; set; }
    }
    public class PackageModel
    {
        public List<SmartPackage> PackageList { get; set; }
        public PackageCount TotalCount { get; set; }
    }
    public partial class ExistSmartPackage
    {
        public int ExistCount { get; set; }
    }
}
