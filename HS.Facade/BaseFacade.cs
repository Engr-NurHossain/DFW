using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace HS.Facade
{
    public class BaseFacade : IDisposable
    {
        private const string CONTEXT_FACADE = "HS.Facade.TheFacade";

        protected Logger logger= LogManager.GetCurrentClassLogger();
        protected ClientContext _ClientContext { get; set; }
        public BaseFacade()
        {
        }
        public BaseFacade(ClientContext clientContext)
        {
            _ClientContext = clientContext;
        }

        //public TheFacade Facade
        //{
        //    get
        //    {
        //        var facade = _ClientContext[CONTEXT_FACADE] as TheFacade;
        //        if (null == facade)
        //        {
        //            facade = new TheFacade(new Client());
        //            _ClientContext[CONTEXT_FACADE] = facade;
        //        }

        //        return facade;
        //    }
        //}

        /// <summary>
        ///  IDisposable Members
        /// </summary>
        public void Dispose()
        {
        }
    }

    //public class FacadeUtil
    //{
    //    public TheFacade Facade
    //    {
    //        get
    //        {
    //            var facade = _Context.Items[CONTEXT_FACADE] as TheFacade;
    //            if (null == facade)
    //            {
    //                facade = new TheFacade(Client);
    //                _Context.Items[CONTEXT_FACADE] = facade;
    //            }

    //            return facade;
    //        }
    //    }
}
