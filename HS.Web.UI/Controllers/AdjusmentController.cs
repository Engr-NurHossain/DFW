using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class AdjusmentController : BaseController
    {
        // GET: Adjusment
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        [Authorize]
        public ActionResult AdjusmentPartial()
        {
            return View();
        }

        public ActionResult AdjustmentSchemePartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<AdjustmentScheme> AdjustmentSchemeList = _Util.Facade.AdjustmentFacade.GetAllAdjustmentSchemeWithCommissionSession();
            return View(AdjustmentSchemeList);
        }

        [Authorize]
        public ActionResult AddAdjustmentScheme(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            AdjustmentScheme model = new AdjustmentScheme();
            if (id > 0)
            {
                model = _Util.Facade.AdjustmentFacade.GetAdjustmentSchemeById(id.Value);
            }
            else
            {
                model = new AdjustmentScheme();
            }
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionSession> CommisionSessionListDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if(CommisionSessionListDropDown != null && CommisionSessionListDropDown.Count > 0)
            {
                SessionCommission.AddRange(CommisionSessionListDropDown.OrderBy(x => x.Name).Select(x =>
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
        public JsonResult AddAdjustmentScheme(AdjustmentScheme AdjustmentScheme)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (AdjustmentScheme.Id > 0)
            {
                _Util.Facade.AdjustmentFacade.UpdateAdjustmentScheme(AdjustmentScheme);
            }
            else
            {
                _Util.Facade.AdjustmentFacade.InsertAdjustmentScheme(AdjustmentScheme);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAdjustmentScheme(int Id)
        {
            bool result = false;
            _Util.Facade.AdjustmentFacade.DeleteAdjustmentScheme(Id);
            return Json(result);
        }

        public ActionResult AdjustmentRulePartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<AdjustmentRule> AdjustmentRuleList = _Util.Facade.AdjustmentFacade.GetAllAdjustmentRuleWithCommissionSessionandScheme();
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionSession> CommisionSessionListDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();
            if (CommisionSessionListDropDown != null && CommisionSessionListDropDown.Count > 0)
            {
                SessionCommission.AddRange(CommisionSessionListDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SessionCommission = SessionCommission;
            List<SelectListItem> SchemeAdjustment = new List<SelectListItem>();
            SchemeAdjustment.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<AdjustmentScheme> AdjustmentSchemeDropDown = _Util.Facade.AdjustmentFacade.GetAllAdjustmentScheme();

            if(AdjustmentSchemeDropDown != null && AdjustmentSchemeDropDown.Count > 0)
            {
                SchemeAdjustment.AddRange(AdjustmentSchemeDropDown.OrderBy(x => x.Name).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.Id.ToString()
                    }).ToList());
            }
            
            ViewBag.SchemeAdjustment = SchemeAdjustment;
            return View(AdjustmentRuleList);
        }

        [Authorize]
        public ActionResult AddAdjustmentRule(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            AdjustmentRule model = new AdjustmentRule();
            if (id > 0)
            {
                model = _Util.Facade.AdjustmentFacade.GetAdjustmentRuleById(id.Value);
            }
            else
            {
                model = new AdjustmentRule();
            }
            List<SelectListItem> SessionCommission = new List<SelectListItem>();
            SessionCommission.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<CommisionSession> CommissionSessionDropDown = _Util.Facade.CommissionFacade.GetAllCommissionSession();

            if (CommissionSessionDropDown != null && CommissionSessionDropDown.Count > 0)
            {
                SessionCommission.AddRange(CommissionSessionDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SessionCommission = SessionCommission;
            List<SelectListItem> SchemeAdjustment = new List<SelectListItem>();
            SchemeAdjustment.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<AdjustmentScheme> AdjustmentSchemeDropDown = _Util.Facade.AdjustmentFacade.GetAllAdjustmentScheme();
            if(AdjustmentSchemeDropDown != null && AdjustmentSchemeDropDown.Count> 0)
            {
                SchemeAdjustment.AddRange(AdjustmentSchemeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SchemeAdjustment = SchemeAdjustment;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAdjustmentRule(AdjustmentRule AdjustmentRule)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (AdjustmentRule.Id > 0)
            {
                _Util.Facade.AdjustmentFacade.UpdateAdjustmentRule(AdjustmentRule);
            }
            else
            {
                _Util.Facade.AdjustmentFacade.InsertAdjustmentRule(AdjustmentRule);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAdjustmentRule(int Id)
        {
            bool result = false;
            _Util.Facade.AdjustmentFacade.DeleteAdjustmentRule(Id);
            return Json(result);
        }

        public ActionResult AdjustmentPartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Adjustment> AdjustmentList = _Util.Facade.AdjustmentFacade.GetAllAdjustmentWithCommissionSessionandScheme();
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
            List<SelectListItem> SchemeAdjustment = new List<SelectListItem>();
            SchemeAdjustment.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<AdjustmentScheme> AdjustmentSchemeDropDown = _Util.Facade.AdjustmentFacade.GetAllAdjustmentScheme();
            if(AdjustmentSchemeDropDown != null && AdjustmentSchemeDropDown.Count > 0)
            {
                SchemeAdjustment.AddRange(AdjustmentSchemeDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SchemeAdjustment = SchemeAdjustment;
            return View(AdjustmentList);
        }

        [Authorize]
        public ActionResult AddAdjustment(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Adjustment model = new Adjustment();
            if (id > 0)
            {
                model = _Util.Facade.AdjustmentFacade.GetAdjustmentById(id.Value);
            }
            else
            {
                model = new Adjustment();
            }
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
            List<SelectListItem> SchemeAdjustment = new List<SelectListItem>();
            SchemeAdjustment.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<AdjustmentScheme> AdjustmentSchemeDropDown = _Util.Facade.AdjustmentFacade.GetAllAdjustmentScheme();
            if (AdjustmentSchemeDropDown != null && AdjustmentSchemeDropDown.Count > 0)
            {
                SchemeAdjustment.AddRange(AdjustmentSchemeDropDown.OrderBy(x => x.Name).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.Name.ToString(),
                      Value = x.Id.ToString()
                  }).ToList());

            }

              
            ViewBag.SchemeAdjustment = SchemeAdjustment;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAdjustment(Adjustment Adjustment)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Adjustment.Id > 0)
            {
                _Util.Facade.AdjustmentFacade.UpdateAdjustment(Adjustment);
            }
            else
            {
                _Util.Facade.AdjustmentFacade.InsertAdjustment(Adjustment);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAdjustment(int Id)
        {
            bool result = false;
            _Util.Facade.AdjustmentFacade.DeleteAdjustment(Id);
            return Json(result);
        }

        public ActionResult OverridePartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Override> OverrideList = _Util.Facade.AdjustmentFacade.GetAllOverride();
            return View(OverrideList);
        }

        [Authorize]
        public ActionResult AddOverride(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Override model = new Override();
            if (id > 0)
            {
                model = _Util.Facade.AdjustmentFacade.GetOverrideById(id.Value);
            }
            else
            {
                model = new Override();
            }
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
        public JsonResult AddOverride(Override Override)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Override.Id > 0)
            {
                _Util.Facade.AdjustmentFacade.UpdateOverride(Override);
            }
            else
            {
                _Util.Facade.AdjustmentFacade.InsertOverride(Override);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteOverride(int Id)
        {
            bool result = false;
            _Util.Facade.AdjustmentFacade.DeleteOverride(Id);
            return Json(result);
        }

        public ActionResult OverrideRangePartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<OverrideRange> OverrideRangeList = _Util.Facade.AdjustmentFacade.GetAllOverrideRangeWithOverrideName();
            return View(OverrideRangeList);
        }

        [Authorize]
        public ActionResult AddOverrideRange(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            OverrideRange model = new OverrideRange();
            if (id > 0)
            {
                model = _Util.Facade.AdjustmentFacade.GetOverrideRangeById(id.Value);
            }
            else
            {
                model = new OverrideRange();
            }
            List<SelectListItem> overriderange = new List<SelectListItem>();
            overriderange.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });

            List<Override> OverrideDropDown = _Util.Facade.AdjustmentFacade.GetAllOverride();

            if(OverrideDropDown != null && OverrideDropDown.Count > 0)
            {
                overriderange.AddRange(OverrideDropDown.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());

            }
            ViewBag.overriderange = overriderange;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddOverrideRange(OverrideRange OverrideRange)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (OverrideRange.Id > 0)
            {
                _Util.Facade.AdjustmentFacade.UpdateOverrideRange(OverrideRange);
            }
            else
            {
                _Util.Facade.AdjustmentFacade.InsertOverrideRange(OverrideRange);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteOverrideRange(int Id)
        {
            bool result = false;
            _Util.Facade.AdjustmentFacade.DeleteOverrideRange(Id);
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult FilterAdjustmentRule(string changeval, string sval)
        {
            bool result = false;
            List<AdjustmentRule> ListRange = new List<AdjustmentRule>();
            if (!string.IsNullOrWhiteSpace(changeval) || !string.IsNullOrWhiteSpace(sval))
            {
                ListRange = _Util.Facade.AdjustmentFacade.GetAllAdjustmentRuleBySchemeId(changeval, sval);
                result = true;
            }
            return Json(new { result = result, valrange = changeval, srange = sval });
        }

        [Authorize]
        public ActionResult FilterAdjustmentRulePartial(string id, string sid)
        {
            List<AdjustmentRule> model = new List<AdjustmentRule>();
            if (!string.IsNullOrWhiteSpace(id) || !string.IsNullOrWhiteSpace(sid))
            {
                model = _Util.Facade.AdjustmentFacade.GetAllAdjustmentRuleBySchemeId(id, sid);
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult FilterAdjustment(string changeval, string sval)
        {
            bool result = false;
            List<Adjustment> ListRange = new List<Adjustment>();
            if (!string.IsNullOrWhiteSpace(changeval) || !string.IsNullOrWhiteSpace(sval))
            {
                ListRange = _Util.Facade.AdjustmentFacade.GetAllAdjustmentBySchemeId(changeval, sval);
                result = true;
            }
            return Json(new { result = result, valrange = changeval, srange = sval });
        }

        [Authorize]
        public ActionResult FilterAdjustmentPartial(string id, string sid)
        {
            List<Adjustment> model = new List<Adjustment>();
            if (!string.IsNullOrWhiteSpace(id) || !string.IsNullOrWhiteSpace(sid))
            {
                model = _Util.Facade.AdjustmentFacade.GetAllAdjustmentBySchemeId(id, sid);
            }
            return View(model);
        }
    }
}