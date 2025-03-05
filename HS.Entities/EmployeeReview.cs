using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeReview 
	{
        public string ReviewedByName { set; get; }
        public string Permission { set; get; }

        public string UserType { set; get; }
        public string Signature { set; get; }
         
        #region DateTimes
        private string _StrEmpSignatureDate { set; get; }
        private string _StrReviewDate { set; get; }
        private string _StrManagerSignatureDate { set; get; }
        public string StrEmpSignatureDate {
            get { return _StrEmpSignatureDate; }
            set
            {
                _StrEmpSignatureDate = value;
                EmpSignatureDate = value.ToDateTime();
            }
        }
        public string StrReviewDate 
        {
            get { return _StrReviewDate; }
            set
            {
                _StrReviewDate = value;
                ReviewDate = value.ToDateTime();
            }
        }
        public string StrManagerSignatureDate 
        {
            get { return _StrManagerSignatureDate; }
            set
            {
                _StrManagerSignatureDate = value;
                ManagerSignatureDate = value.ToDateTime();
            }
        }
        #endregion
    }
}
