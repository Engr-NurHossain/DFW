using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeEvaluation 
	{
        private string _StrEvaluationReminderDate { set; get; }
        private string _StrNextEvaluationDate { set; get; }
        private string _StrLastEvaluationDate { set; get; }


        public string StrLastEvaluationDate {
            get { return _StrLastEvaluationDate; }
            set
            { 
                _StrLastEvaluationDate = value;
                this.LastEvaluationDate = value.ToDateTime(); 
            }
        }

        public string StrEvaluationReminderDate {
            get { return _StrEvaluationReminderDate; }
            set
            { 
                _StrEvaluationReminderDate = value; 
                this.EvaluationReminderDate = value.ToDateTime(); 
            }

        }
        public string StrNextEvaluationDate {
            get { return _StrNextEvaluationDate; }
            set
            { 
                _StrNextEvaluationDate = value;
                this.NextEvaluationDate = value.ToDateTime();
            }
        }
    }
}
