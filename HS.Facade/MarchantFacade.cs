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
    public class MarchantFacade:BaseFacade
    {
        public MarchantFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        MarchantDataAccess _MarchantDataAccess
        {
            get
            {
                return (MarchantDataAccess)_ClientContext[typeof(MarchantDataAccess)];
            }
        }

        public List<Marchant> GetAllMarchants()
        {
            return _MarchantDataAccess.GetAll();
        }

        public Marchant GetById(int value)
        {
            return _MarchantDataAccess.Get(value);
        }

        public bool UpdateMarchant(Marchant manu)
        {
            return _MarchantDataAccess.Update(manu)>0;
        }

        public long InsertMarchant(Marchant manu)
        {
            return _MarchantDataAccess.Insert(manu);
        }
    }
}
