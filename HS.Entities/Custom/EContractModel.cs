using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Custom
{
    public class EContractModel
    {
        public double ActivationFee { get; set; }
        public DateTime InstallStartDate { get; set; }
        public DateTime InstallFinishDate { get; set; }
        public DateTime PaymentEffectiveDate { get; set; }
        public int PromotionMonth { get; set; }
        public int PrepaidMonth { get; set; }
        public string EcontractId { get; set; }
    }
}
