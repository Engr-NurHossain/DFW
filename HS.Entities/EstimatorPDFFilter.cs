using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EstimatorPDFFilter 
	{
        //public Nullable<Boolean> OneTimeService { get; set; } 
    }
    public class EstimatorPDFFilterForEmailBase
    { 
        #region Properties		
        [DataMember]
        public Int32 Id { get; set; }

        [DataMember]
        public Guid CompanyId
        {
            get; set;
        }

        [DataMember]
        public Guid CustomerId
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbyNone
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbyCategory
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbyLabor
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbyLaborAndMaterial
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbyMaterial
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> GroupedbySupplier
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeCost
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeImage
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeManufacturer
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeMargin
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeOverhead
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludePDF
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeProfit
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeService
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> WithoutIndividualLaborPricing
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> WithoutIndividualMaterialPricing
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> WithoutPricing
        {
            get; set;
        }

        [DataMember]
        public Guid CreatedBy
        {
            get; set;
        }

        [DataMember]
        public DateTime CreatedDate
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> IncludeVariation
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> WithoutQTY
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> WithoutIndividualItem
        {
            get; set;
        }

        [DataMember]
        public Nullable<Boolean> AddBlankPage
        {
            get; set;
        }

        [DataMember]
        public Nullable<Int32> FileId
        {
            get; set;
        }

        [DataMember]
        public Nullable<Int32> EstimatorId
        {
            get; set;
        }

        #endregion 
    }
}
