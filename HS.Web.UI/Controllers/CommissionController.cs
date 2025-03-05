using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class CommissionController : BaseController
    {
        // GET: Commission
        public ActionResult Index()
        {
            return View();
        }

       
        [Authorize]
        public ActionResult AddCommissionType(int? id)
        {
             
   
               var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CommisionType model = new CommisionType();
            if (id > 0)
            {
                model = _Util.Facade.CommissionFacade.GetCommissionTypeById(id.Value);
            }
            else
            {
                model = new CommisionType();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCommissionType(CommisionType CommisionType)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if(CommisionType.Id > 0)
            {
                _Util.Facade.CommissionFacade.UpdateCommissionType(CommisionType);
            }
            else
            {
                _Util.Facade.CommissionFacade.InsertCommissionType(CommisionType);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCommissionType(int Id)
        {
            bool result = false;
            _Util.Facade.CommissionFacade.DeleteCommissionType(Id);
            return Json(result);
        }

      
        [Authorize]
        public ActionResult AddCommissionSession(int? id)
        {
            CommisionSession model = new CommisionSession();
            if(id > 0)
            {
                model = _Util.Facade.CommissionFacade.GetCommissionSessionById(id.Value);
            }
            else
            {
                model = new CommisionSession();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCommissionSession(CommisionSession CommisionSession)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CommisionSession.Id > 0)
            {
                _Util.Facade.CommissionFacade.UpdateCommissionSession(CommisionSession);
            }
            else
            {
                _Util.Facade.CommissionFacade.InsertCommissionSession(CommisionSession);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCommissionSession(int Id)
        {
            bool result = false;
            _Util.Facade.CommissionFacade.DeleteCommissionSession(Id);
            return Json(result);
        }

        [Authorize]
        public ActionResult CommissionRangePartial(string id, string sid)
        {
            List<CommisionRange> ListRange = _Util.Facade.CommissionFacade.GetAllCommissionRangeWithCommissionTypeAndSession();
            List<SelectListItem> TypeCommission = new List<SelectListItem>();
            TypeCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionType> CommisionTypeDropDown = _Util.Facade.CommissionFacade.GetAllCommissionType();
            if(CommisionTypeDropDown != null && CommisionTypeDropDown.Count > 0)
            {
                TypeCommission.AddRange(CommisionTypeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }

            
            ViewBag.TypeCommission = TypeCommission;
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });

            List<CommisionSession> CommisionSessionDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if(CommisionSessionDropDown != null && CommisionSessionDropDown.Count > 0)
            {
                SessionCommission.AddRange(CommisionSessionDropDown.OrderBy(x => x.Name).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.Id.ToString()
                    }).ToList());
                        ViewBag.SessionCommission = SessionCommission;
            }
            
            return View(ListRange);
        }

        [Authorize]
        public ActionResult AddCommissionRange(int? id)
        {
            CommisionRange model = new CommisionRange();
            if (id > 0)
            {
                model = _Util.Facade.CommissionFacade.GetCommissionRangeById(id.Value);
            }
            else
            {
                model = new CommisionRange();
            }
            List<SelectListItem> TypeCommission = new List<SelectListItem>();
            TypeCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionType> CommisionTypeDropDown = _Util.Facade.CommissionFacade.GetAllCommissionType();

            if(CommisionTypeDropDown != null && CommisionTypeDropDown.Count > 0)
            {
                TypeCommission.AddRange(CommisionTypeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());

            }

            ViewBag.TypeCommission = TypeCommission;
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionSession> CommisionSessionDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if(CommisionSessionDropDown != null && CommisionSessionDropDown.Count > 0)
            {
                SessionCommission.AddRange(CommisionSessionDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
                    
            }
            ViewBag.SessionCommission = SessionCommission;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCommissionRange(CommisionRange CommisionRange)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CommisionRange.Id > 0)
            {
                _Util.Facade.CommissionFacade.UpdateCommissionRange(CommisionRange);
            }
            else
            {
                _Util.Facade.CommissionFacade.InsertCommissionRange(CommisionRange);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCommissionRange(int Id)
        {
            bool result = false;
            _Util.Facade.CommissionFacade.DeleteCommissionRange(Id);
            return Json(result);
        }

        [Authorize]
        public ActionResult CommissionPartial()
        {
            List<Commision> CommisssionList = _Util.Facade.CommissionFacade.GetAllCommissionWithCommissionTypeAndSession();
            List<SelectListItem> TypeCommission = new List<SelectListItem>();
            TypeCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionType> CommisionTypeDropDown = _Util.Facade.CommissionFacade.GetAllCommissionType();
            if(CommisionTypeDropDown != null && CommisionTypeDropDown.Count > 0)
            {
                TypeCommission.AddRange(CommisionTypeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.TypeCommission = TypeCommission;
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });

            List<CommisionSession> CommisionSessionDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if(CommisionSessionDropDown !=  null && CommisionSessionDropDown.Count > 0)
            {
                SessionCommission.AddRange(_Util.Facade.CommissionFacade.GetAllCommissionSession().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SessionCommission = SessionCommission;
            return View(CommisssionList);
        }

        [Authorize]
        public ActionResult AddCommission(int? id)
        {
            Commision model = new Commision();
            if (id > 0)
            {
                model = _Util.Facade.CommissionFacade.GetCommissionById(id.Value);
            }
            else
            {
                model = new Commision();
            }
            List<SelectListItem> TypeCommission = new List<SelectListItem>();
            TypeCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionType> CommisionTypeDropDown = _Util.Facade.CommissionFacade.GetAllCommissionType();
            if(CommisionTypeDropDown != null && CommisionTypeDropDown.Count > 0)
            {
                TypeCommission.AddRange(CommisionTypeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.TypeCommission = TypeCommission;
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionSession> CommisionSessionDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if(CommisionSessionDropDown != null && CommisionSessionDropDown.Count > 0)
            {
                SessionCommission.AddRange(_Util.Facade.CommissionFacade.GetAllCommissionSession().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SessionCommission = SessionCommission;
            List<SelectListItem> TimeFrame = new List<SelectListItem>();
            TimeFrame.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("TimeFrame").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.TimeFrame = TimeFrame;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCommission(Commision Commision)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Commision.Id > 0)
            {
                _Util.Facade.CommissionFacade.UpdateCommission(Commision);
            }
            else
            {
                _Util.Facade.CommissionFacade.InsertCommission(Commision);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCommission(int Id)
        {
            bool result = false;
            _Util.Facade.CommissionFacade.DeleteCommission(Id);
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult FilterCommissionRange(string changeval, string sval)
        {
            bool result = false;
            List<CommisionRange> ListRange = new List<CommisionRange>();
            if (!string.IsNullOrWhiteSpace(changeval) || !string.IsNullOrWhiteSpace(sval))
            {
                ListRange = _Util.Facade.CommissionFacade.GetAllCommissionRangeByTypeId(changeval, sval);
                result = true;
            }
            return Json(new { result = result, valrange = changeval, srange = sval });
        }

        [Authorize]
        public ActionResult FilterCommissionRangePartial(string id, string sid)
        {
            List<CommisionRange> model = new List<CommisionRange>();
            if (!string.IsNullOrWhiteSpace(id) || !string.IsNullOrWhiteSpace(sid))
            {
                model = _Util.Facade.CommissionFacade.GetAllCommissionRangeByTypeId(id, sid);
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult FilterCommission(string changeval, string sval)
        {
            bool result = false;
            List<Commision> ListRange = new List<Commision>();
            if (!string.IsNullOrWhiteSpace(changeval) || !string.IsNullOrWhiteSpace(sval))
            {
                ListRange = _Util.Facade.CommissionFacade.GetAllCommissionByTypeId(changeval, sval);
                result = true;
            }
            return Json(new { result = result, valrange = changeval, srange = sval });
        }

        [Authorize]
        public ActionResult FilterCommissionPartial(string id, string sid)
        {
            List<Commision> model = new List<Commision>();
            if (!string.IsNullOrWhiteSpace(id) || !string.IsNullOrWhiteSpace(sid))
            {
                model = _Util.Facade.CommissionFacade.GetAllCommissionByTypeId(id, sid);
            }
            return View(model);
        }
    }
}