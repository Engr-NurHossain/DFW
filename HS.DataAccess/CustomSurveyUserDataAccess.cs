using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class CustomSurveyUserDataAccess
	{
        public CustomSurveyUserDataAccess(string ConStr) : base(ConStr) { }
    }	
}
