using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class TransactionExpense
    {
        public string StrExpenseDate
        {
            get { return _StrExpenseDate; }
            set
            {
                _StrExpenseDate = value;
                this.ExpenseDate = value.ToDateTime();
            }
        }
        public string FileName2 { set; get; }
        public string ExpenseTypeVal { get; set; }
        public string CreatedByVal { get; set; }
        public string ExpenseBy { get; set; }
        private string _StrExpenseDate { set; get; }

    }
    public class TransactionExpenseCount
    {
        public int TotalCount { get; set; }
    }
    public class TransactionExpenseModel
    {
        public List<TransactionExpense> TransactionExpenseList { get; set; }
        public TransactionExpenseCount TransactionExpenseCount { get; set; }
    }
}
