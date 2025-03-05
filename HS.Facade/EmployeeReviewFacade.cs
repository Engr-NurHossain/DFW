using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class EmployeeReviewFacade: BaseFacade
    {
        public EmployeeReviewFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EmployeeReviewDataAccess _EmployeeReviewDataAccess
        {
            get
            {
                return (EmployeeReviewDataAccess)_ClientContext[typeof(EmployeeReviewDataAccess)];
            }
        }

        public EmployeeReview GetEmployeeReviewByReviewId(Guid reviewId)
        {
            return _EmployeeReviewDataAccess.GetByQuery(string.Format("ReviewId = '{0}'",reviewId)).FirstOrDefault();
        }

        public List<EmployeeReview> GetEmployeeReviewsByUserId(Guid userId)
        {
            return _EmployeeReviewDataAccess.GetEmployeeReviewsByUserId(userId);
        }

        public int InsertEmployeeReview(EmployeeReview obj)
        {
            return (int)_EmployeeReviewDataAccess.Insert(obj);
        }

        public bool Update(EmployeeReview model)
        {
            return _EmployeeReviewDataAccess.Update(model) > 0;
        }
    }
}
