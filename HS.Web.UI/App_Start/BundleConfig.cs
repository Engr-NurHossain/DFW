using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace HS.Web.UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Layout
            bundles.Add(new StyleBundle("~/styles/layout").Include(
                       "~/Content/Css/Shared/Header.css",
                       "~/Content/Bootstrap/css/bootstrap.min.css",
                       "~/Content/FontAwesome/css/font-awesome.css",
                       "~/Content/Jquery-ui/jquery-ui.css",
                       "~/Content/MagnificPopUp/magnific-popup.css",
                       "~/Content/perfect-scrollbar/css/perfect-scrollbar.css",
                       "~/Content/Css/Shared/Layout.css"
                       ));
            bundles.Add(new ScriptBundle("~/scripts/layout").Include(
                       "~/Content/Jquery-ui/jquery.js",
                       "~/Content/Js/Layout/utils.js",
                       "~/Content/Bootstrap/js/bootstrap.min.js",
                       //"~/Content/Jquery-ui/jquery-ui.js",
                       "~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
                       "~/Content/MagnificPopUp/jquery.magnific-popup.js",
                       "~/Content/perfect-scrollbar/js/perfect-scrollbar.jquery.js",
                       "~/Content/Js/Site/Validation.js",
                       "~/Content/JQuery/jquery.cookie.js"
                       ));
            #endregion
            #region PrivateLayout
            bundles.Add(new StyleBundle("~/styles/privatelayout").Include(
                      "~/Content/Css/Layout/startmin.css",
                      "~/Content/Css/Layout/Loader.css",
                      "~/Content/Css/Layout/PrivateLayout.css",
                      //"~/Content/Select2/select2.css",
                      "~/Content/datatable/dataTables.bootstrap.css",
                      "~/Content/Css/PackageSettings/bootstrap-toggle.min.css",
                      "~/Content/PikDay/css/pikaday.css"
                      ));
            bundles.Add(new ScriptBundle("~/scripts/privatelayout").Include(
                       "~/Content/Js/Layout/metisMenu.min.js",
                       "~/Content/Js/Layout/startmin.js",
                       "~/Content/JQueryFileUpload/jquery.fileupload.js",
                       "~/Content/JQueryFileUpload/jquery.fileupload-ui.js",
                       "~/Content/Js/Layout/PrivateLayout.js",
                       //"~/Content/Select2/Select2.min.js",
                       "~/Content/datatable/jquery.dataTables.min.js",
                       "~/Content/datatable/dataTables.bootstrap.js",
                       "~/Content/Js/PackageSetup/bootstrap-toggle.min.js",
                       "~/Content/PikDay/js/moment.js",
                       "~/Content/PikDay/js/pikaday.js",
                       "~/Content/Js/Dashboard/SessionChecker.js"
                       ));
            #endregion
            #region Dashboard 
            bundles.Add(new StyleBundle("~/styles/dashboard").Include(
                      "~/Content/Css/Dashboard/timeline.css",
                      "~/Content/Css/Dashboard/morris.css",
                       "~/Content/Css/Dashboard/Dashboard.css"
                      ));
            bundles.Add(new ScriptBundle("~/scripts/dashboard").Include(
                      "~/Content/Js/Dashboard/raphael.min.js",
                      "~/Content/Js/Dashboard/morris.min.js",
                      "~/Content/Js/Dashboard/morris-data.js",
                      "~/Content/Js/Dashboard/dashboard.js"
                       ));

            #endregion
            #region DataTableBundles 
            bundles.Add(new StyleBundle("~/styles/DataTableBundles").Include(
                       "~/Content/datatable/dataTables.bootstrap.css"
                      ));
            bundles.Add(new ScriptBundle("~/scripts/DataTableBundles").Include(
                       "~/Content/datatable/jquery.dataTables.min.js",
                       "~/Content/datatable/dataTables.bootstrap.js"
                       ));
            #endregion
            #region Login
            bundles.Add(new ScriptBundle("~/scripts/login").Include(
                    "~/Content/Js/Login/login.js",
                    "~/Content/Js/CryptoJS/3.1.9-1-crypto-js.min.js"
                     ));
            #endregion
            #region Customer
            bundles.Add(new StyleBundle("~/styles/customer").Include(
                    "~/Content/Css/Customer/customer.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/customer").Include(
                    "~/Content/Js/Customer/customer.js"
                     ));


            bundles.Add(new ScriptBundle("~/scripts/customerlite").Include(
                    "~/Content/Js/Customer/customerLite.js"
                     ));
            #endregion
            #region CustomerLite
            bundles.Add(new StyleBundle("~/styles/customer").Include(
                "~/Content/Css/Customer/customer.css"
            ));
            bundles.Add(new ScriptBundle("~/scripts/customerLite").Include(
                "~/Content/Js/Customer/customerLite.js"
            ));
            #endregion
            #region AddCustomer
            bundles.Add(new StyleBundle("~/styles/AddCustomer").Include(
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/CityStateZipAutoFill.css",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Popups/AddCustomerPopup.css",
                   "~/Content/Css/Customer/AddCustomer_custom.css"
                    ));

            bundles.Add(new ScriptBundle("~/scripts/AddCustomer").Include(
                     "~/Content/Select2/Select2.min.js",
                     "~/Content/Js/CityStateZipAutoFill.js",
                     "~/Content/Js/Card/CardValidation.js",
                     "~/Content/Js/Site/Validation.js",
                     "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                     "~/Scripts/CustomerSelect.js",
                     "~/Content/Js/Popups/HomeOwnerVerification.js",
                     "~/Content/Js/Customer/Addcustomer.js",
                     "~/Content/Js/Customer/CustomerSubscription.js",
                    "~/Content/Js/Popups/AddCustomer.js"
                     ));
            #endregion
            #region ProductCategory
            bundles.Add(new StyleBundle("~/styles/ProductCategory").Include(

                    "~/Content/Css/Customer/CustomerList.css",
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/ProductCategory/ProductCategory.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ProductCategory").Include(
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/Js/ProductCategory/ProductCategory.js"
                     ));
            #endregion

            #region Ticket
            bundles.Add(new StyleBundle("~/styles/ticketdashboard").Include(
                    "~/Content/Css/Ticket/TicketDashboard.css"
                    ));

            #endregion
            #region TicketEdit
            bundles.Add(new StyleBundle("~/styles/ticketedit").Include(
                    "~/Content/Css/Ticket/TicketEdit.css"
                    ));

            #endregion
            #region Estimate
            bundles.Add(new StyleBundle("~/styles/Estimate").Include(

                    "~/Content/Css/Estimate/Estimate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Estimate").Include(

                    "~/Content/Js/Estimate/Estimate.js"
                     ));
            #endregion
            #region CustomerFile
            bundles.Add(new StyleBundle("~/styles/CustomerFile").Include(

                    "~/Content/Css/CustomerFile/CustomerFile.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerFile").Include(

                    "~/Content/Js/CustomerFile/CustomerFile.js"
                     ));
            #endregion

            #region CustomerInspection
            bundles.Add(new StyleBundle("~/styles/CustomerInspection").Include(

                    "~/Content/Css/Customer/LoadCustomerInspection.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerInspection").Include(
                "~/Content/Js/SignaturePad/SignaturePad.js",
                    "~/Content/Js/Customer/LoadCustomerInspection.js"
                     ));
            #endregion
            #region EquipmentClass
            bundles.Add(new StyleBundle("~/styles/EquipmentClass").Include(

                    "~/Content/Css/EquipmentClass/EquipmentClass.css",
                    "~/Content/Css/Customer/CustomerList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EquipmentClass").Include(

                    "~/Content/Js/EquipmentClass/EquipmentClass.js"
                     ));
            #endregion
            #region Invoice
            bundles.Add(new StyleBundle("~/styles/Invoice").Include(

                    "~/Content/Css/Invoice/Invoice.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Invoice").Include(

                    "~/Content/Js/Invoice/Invoice.js"
                     ));
            #endregion
            #region Leads
            bundles.Add(new StyleBundle("~/styles/Leads").Include(
                    //"~/Content/Select2/select2.css",
                    "~/Content/Css/Lead/Leads.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Leads").Include(

                    "~/Content/datatable/jquery.dataTables.min.js",
                    //"~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Lead/Leads.js"
                     ));
            bundles.Add(new ScriptBundle("~/scripts/LeadsLite").Include(

                 "~/Content/datatable/jquery.dataTables.min.js",
                 //"~/Content/Select2/Select2.min.js",
                 "~/Content/Js/Lead/LeadsLite.js"
                  ));
            bundles.Add(new StyleBundle("~/styles/LeadTrackingPartialCss").Include(
                "~/Content/Css/WorkOrder/Pagination.css",
              "~/Content/Css/Lead/LeadTracking/LeadTrackingPartial.css"
              ));

            bundles.Add(new ScriptBundle("~/scripts/LeadTrackingPartialJs").Include(
                 "~/Content/Js/LeadTracking/LeadTrackingPartial.js"
                  ));


            #endregion
            #region LeadVerifyInfo
            bundles.Add(new StyleBundle("~/styles/LeadVerifyInfo").Include(
                    "~/Content/Css/Lead/AddLeadVerifyInfo/AddLeadVerifyInfo.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LeadVerifyInfo").Include(
                    "~/Content/Js/AddLead/AddLeadVerifyInfo/AddLeadverifyInfo.js"
                     ));
            #endregion
            #region Notes
            bundles.Add(new StyleBundle("~/styles/Notes").Include(

                    "~/Content/Css/Notes/Notes.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Notes").Include(

                    "~/Content/Js/Note/Note.js"
                     ));
            #endregion
            #region Sales
            bundles.Add(new StyleBundle("~/styles/Sales").Include(

                    "~/Content/Css/Sales/Sales.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Sales").Include(

                    "~/Content/Js/Sales/Sales.js"
                     ));
            #endregion
            #region ServiceOrder
            bundles.Add(new StyleBundle("~/styles/ServiceOrder").Include(

                    "~/Content/Css/ServiceOrder/ServiceOrder.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ServiceOrder").Include(

                    "~/Content/Js/ServiceOrder/ServiceOrder.js"
                     ));
            #endregion
            #region Mmrs
            bundles.Add(new StyleBundle("~/styles/Mmrs").Include(

                    "~/Content/Css/Mmr/Mmr.css",
                    "~/Content/Css/Customer/CustomerList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Mmrs").Include(

                    "~/Content/Js/Mmr/Mmr.js"
                     ));
            #endregion
            #region Supplier
            bundles.Add(new StyleBundle("~/styles/Supplier").Include(

                    "~/Content/Css/Supplier/Supplier.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Supplier").Include(

                    "~/Content/Js/Supplier/Supplier.js"
                     ));
            #endregion
            #region TechSchedule
            bundles.Add(new StyleBundle("~/styles/TechSchedule").Include(

                    "~/Content/Css/Techschedule/Schedule.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/TechSchedule").Include(

                    "~/Content/Js/TechSchedule/Schedule.js"
                     ));
            #endregion
            #region AddProductCategory
            bundles.Add(new StyleBundle("~/styles/AddProductCategory").Include(

                    "~/Content/Css/ProductCategory/AddProductCategory.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddProductCategory").Include(
                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/Popups/AddProductCategory.js"

                     ));
            #endregion
            #region CustomerDetails
            bundles.Add(new StyleBundle("~/styles/CustomerDetails").Include(

                    "~/Content/Css/Customer/CustomerDetails.css",
                    "~/Content/Css/Techschedule/techSchedule.css",
                    "~/Content/Css/CustomerDetails/CustomerDetail.css",
                    "~/Content/Css/CustomerDetails/CustomerDetailsCustom.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerDetails").Include(

                    //"~/Content/Js/Popups/AddCustomer.js",
                    "~/Content/Js/CustomerDetails/CustomerDetails.js"
                     ));
            #endregion
            #region CustomerList
            bundles.Add(new StyleBundle("~/styles/CustomerList").Include(

                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Customer/CustomerList.css",
                    "~/Content/Css/Customer/CustomerListCustom.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerList").Include(

                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/CustomerList/CustomerList.js"
                     ));
            #endregion
            #region CustomerListLite
            bundles.Add(new StyleBundle("~/styles/CustomerList").Include(

                "~/Content/datatable/dataTables.bootstrap.css",
                "~/Content/Css/Customer/CustomerList.css",
                "~/Content/Css/Customer/CustomerListCustom.css"
            ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerListLite").Include(

                "~/Content/datatable/jquery.dataTables.min.js",
                "~/Content/datatable/dataTables.bootstrap.js",
                "~/Content/Js/CustomerList/CustomerList.js"
            ));
            #endregion
            #region CustomerListFilter
            bundles.Add(new StyleBundle("~/styles/CustomerListFilter").Include(
                    //"~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Customer/CustomerList.css",
                     "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerListFilter").Include(

                      //"~/Content/datatable/jquery.dataTables.min.js",
                      //"~/Content/datatable/dataTables.bootstrap.js",
                      "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                     "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/CustomerLIstFilter/CustomerListFilter.js"

                     ));
            #endregion
            #region AddFile
            bundles.Add(new StyleBundle("~/styles/AddFile").Include(

                    "~/Content/Css/AddFile/AddFile.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddFile").Include(

                    //"~/Content/JQuery/jquery-3.2.1.min.js",
                    //"~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
                    //"~/Content/Js/Site/Validation.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload-ui.js",
                    "~/Content/Js/Customer/CustomerFileUpload.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddCustomerLeadImportFile").Include(
                    "~/Content/Js/Customer/CustomerLeadImportFile.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddExpenseVendorImportFile").Include(
                   "~/Content/Js/Expense/ExpenseVendorImportFile.js"
                   ));

            bundles.Add(new ScriptBundle("~/scripts/FileTemplateManagement").Include(
                    "~/Content/Js/FileTemplate/FileTemplateManagement.js"
                    ));
            #endregion
            #region LoadFileTemplateManagement
            bundles.Add(new StyleBundle("~/styles/LoadFileTemplateManagement").Include(
                   "~/Content/Css/FileTemplate/LeadDocumentFileManagementPartial.css"
                   ));
            bundles.Add(new ScriptBundle("~/scripts/LoadFileTemplateManagement").Include(
                    "~/Content/Js/FileTemplate/LoadFileTemplateManagement.js"
                    ));
            #endregion

            #region AddBundle
            bundles.Add(new StyleBundle("~/styles/AddBundle").Include(

                    "~/Content/Css/AddBundle/AddBundle.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddBundle").Include(

                    "~/Content/Js/AddBundle/AddBundle.js"
                    ));
            #endregion
            #region AddProductClass
            bundles.Add(new StyleBundle("~/styles/AddProductClass").Include(

                    "~/Content/Css/AddProductClass/AddProductClass.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddProductClass").Include(
                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/Popups/AddProductClass.js"
                    ));
            #endregion
            #region LeadDetails
            bundles.Add(new StyleBundle("~/styles/LeadDetails").Include(

                    "~/Content/Css/Customer/CustomerDetails.css",
                    "~/Content/Css/Techschedule/techSchedule.css",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/LeadDetails/LeadDetails.css",
                    "~/Content/Css/LeadDetails/LeadDetailsCustom.css",
                    "~/startmin/css/dataTables/DataTableMaterial.css",
                    "~/Content/Css/LeadDetails/_LeadDetails.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LeadDetails").Include(

                    "~/Content/Js/Popups/AddLeads.js",
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Js/LeadDetails/LeadDetails.js",
                    "~/Content/Js/Layout/Device.js",
                    "~/Content/Js/LeadDetails/_LeadDetails.js"
                    ));
            #endregion
            #region LeadVerificationPartial
            bundles.Add(new StyleBundle("~/styles/LeadVerificationPartialCS").Include(
                    "~/Content/Css/Customer/CustomerDetails.css",
                    "~/Content/Css/Techschedule/techSchedule.css",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/LeadDetails/LeadDetails.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/LeadDetails/LeadDetailsCustom.css",
                    "~/Content/Css/Lead/AddLeadVerifyInfo/AddLeadVerifyInfo.css",
                    "~/Content/Css/CityStateZipAutoFill.css",
                    "~/Content/Css/LeadVerificationPartial/LeadVerificationPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LeadVerificationPartialJS").Include(
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/CityStateZipAutoFill.js",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Scripts/CustomerSelect.js",
                    "~/Content/Js/Popups/HomeOwnerVerification.js",
                    "~/Content/Js/AddLead/AddLeadVerifyInfo/AddLeadverifyInfo.js",
                    "~/Content/Js/LeadVerificationPartial/LeadVerification.js"
                    ));
            #endregion

            #region SmartLeadSetupParial
            bundles.Add(new StyleBundle("~/styles/SmartLeadSetupParialCS").Include(

                    "~/Content/Css/Customer/CustomerDetails.css",
                    "~/Content/Css/Techschedule/techSchedule.css",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/LeadDetails/LeadDetails.css",
                    "~/Content/Css/LeadDetails/LeadDetailsCustom.css",
                    "~/Content/Css/Lead/LeadSetupPartial.css",
                    "~/Content/Css/Shared/breadcrumb_style.css"
                    ));
            //bundles.Add(new ScriptBundle("~/scripts/LeadVerificationPartialJS").Include(

            //        "~/Content/Js/Popups/AddLeads.js",
            //        "~/Content/datatable/jquery.dataTables.min.js",
            //        "~/Content/datatable/dataTables.bootstrap.js",
            //        "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
            //        "~/Content/Js/LeadDetails/LeadDetails.js"
            //        ));
            bundles.Add(new ScriptBundle("~/scripts/SmartLeadSetupParialJS").Include(
                "~/Content/MagnificPopUp/jquery.magnific-popup.js",
                "~/Content/Js/Popups/AddSmartLeadSetup.js",
                "~/Content/Js/LeadSetup/SmartLeadSetup.js",
                "~/Content/Js/Layout/Device.js",
                "~/Content/Js/SmartLeadSetupPartial.js"
                    ));
            bundles.Add(new StyleBundle("~/styles/SmartLeadSmartServiceNew").Include(
                   "~/Content/Css/Lead/PackagePartial.css",
                   "~/Content/Css/LeadEquipment/LeadEquipment.css",
                   "~/Content/Css/LeadEquipment/LeadSetupEquipments.css",
                   "~/Content/Css/Lead/SmartServicePartialNew.css"
                   ));
            bundles.Add(new ScriptBundle("~/scripts/SmartLeadSmartServiceNew").Include(
                    "~/Content/Js/SmartLeads/SmartServicePartialNew.js"
                    ));


            bundles.Add(new StyleBundle("~/styles/SmartEquipmentPartial").Include(
                    "~/Content/Css/Lead/PackagePartial.css",
                    "~/Content/Css/LeadEquipment/LeadEquipment.css",
                    "~/Content/Css/LeadEquipment/LeadSetupEquipments.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SmartEquipmentPartial").Include(
                    "~/Content/Js/LeadEquipment/SmartLeadEquipment.js"
                    ));
            #endregion

            #region LeadList
            bundles.Add(new StyleBundle("~/styles/LeadList").Include(

                    "~/Content/Css/Customer/CustomerList.css",
                    "~/Content/Css/Lead/LeadListCustom.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LeadList").Include(
                    "~/Content/Js/LeadList/LeadList.js"
                    ));
            #endregion

            #region MenuList
            bundles.Add(new StyleBundle("~/styles/MenuListPartial").Include(

                    "~/Content/Css/Menu/MenuListPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/MenuList").Include(
                    "~/Content/Js/Menu/MenuList.js"
                    ));
            #endregion

            #region MenuList
            bundles.Add(new StyleBundle("~/styles/CategoryListPartial").Include(

                    "~/Content/Css/Category/CategoryListPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CategoryList").Include(
                    "~/Content/Js/Category/CategoryList.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddCategory").Include(
                    "~/Content/Js/Category/AddCategory.js"
                    ));
            #endregion

            #region Topping
            bundles.Add(new ScriptBundle("~/scripts/AddTopping").Include(
                    "~/Content/Js/Topping/AddTopping.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ToppingList").Include(
                    "~/Content/Js/Topping/ToppingList.js"
                    ));
            #endregion

            #region LeadDetails
            bundles.Add(new StyleBundle("~/styles/_FilterLeadListPartialCSS").Include(

                    //"~/Content/Css/Customer/CustomerDetails.css",
                    //"~/Content/Css/Techschedule/techSchedule.css",
                    //"~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    //"~/Content/Css/LeadDetails/LeadDetails.css",
                    //"~/Content/Css/LeadDetails/LeadDetailsCustom.css",
                    //"~/startmin/css/dataTables/DataTableMaterial.css",
                    //"~/Content/Css/LeadDetails/_LeadDetails.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_FilterLeadListPartialJS").Include(

                    "~/Content/Js/LeadList/_FilterLeadListPartial.js"
                    ));
            #endregion

            #region AddLead
            bundles.Add(new StyleBundle("~/styles/AddLead").Include(

                    "~/Content/Bootstrap/css/bootstrap.css",
                    "~/Content/Css/Popups/AddCustomerPopup.css"
                    //"~/Content/Select2/select2.css"
                    //"~/Content/PikDay/css/pikaday.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddLead").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Bootstrap/js/bootstrap.min.js",
                    "~/Content/Js/Site/Validation.js",
                    //"~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Popups/AddLeads.js",
                    //"~/Content/PikDay/js/moment.js",
                    //"~/Content/PikDay/js/pikaday.js",
                    "~/Content/Js/AddLead/Addlead.js"
                    ));
            #endregion
            #region SalesMatrix
            bundles.Add(new StyleBundle("~/styles/SalesMatrix").Include(

                    "~/Content/Css/Customer/CustomerList.css",
                    "~/Content/Css/SalesMatrix/SalesMatrix.css",
                    "~/Content/Css/Shared/Header.css",
                    "~/Content/Css/Shared/ListsCommon.css",
                    "~/Content/datatable/dataTables.bootstrap.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SalesMatrix").Include(

                    "~/Content/TreeTable/jquery.treetable.js",
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/Popups/AddProductCategory.js",
                    "~/Content/Js/SalesMatrix/Salesmatrix.js"
                    ));
            #endregion
            #region AddSalesMatrix
            bundles.Add(new StyleBundle("~/styles/AddSalesMatrix").Include(

                    "~/Content/Css/AddSalesMatrix/AddSalesMatrix.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddSalesMatrix").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddSalesMatrix/AddSalesMatrix.js"
                    ));
            #endregion
            #region AddNote
            bundles.Add(new StyleBundle("~/styles/AddNote").Include(

                    "~/Content/PikDay/css/pikaday.css",
                    "~/Content/Css/AddNotes/AddNote.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddNote").Include(

                    "~/Content/Js/Popups/AddNotes.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/PikDay/js/moment.js",
                    "~/Content/PikDay/js/pikaday.js",
                    "~/Content/Js/AddNote/AddNote.js"
                    ));
            #endregion

            #region AddFollowUpReminder
            bundles.Add(new StyleBundle("~/styles/AddFollowUpReminder").Include(
                    "~/Content/Css/Lead/AddFollowUpReminder/AddFollowUpReminder.css",
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddFollowUpReminder").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Js/Lead/AddFollowUpReminder/AddFollowUpReminder.js"
                    ));
            #endregion

            #region AddCustomerFollowUpReminder
            bundles.Add(new StyleBundle("~/styles/AddCustomerFollowUpReminder").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Lead/AddFollowUpReminder/AddFollowUpReminder.css"
                    
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddCustomerFollowUpReminder").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Js/Customer/AddCustomerFollowUp/AddCustomerFollowUpReminder.js"
                    ));
            #endregion

            #region SalesCalendar
            bundles.Add(new StyleBundle("~/styles/SalesCalendar").Include(

                    "~/Content/FullCalendar/fullcalendar.css",
                    "~/Content/Css/SalesCalendar/SalesCalendar.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SalesCalendar").Include(

                    "~/Content/momentJs/moment.min.js",
                    "~/Content/FullCalendar/fullcalendar.js",
                    "~/Content/Js/SalesCalendar/SalesCalendar.js"
                    ));
            #endregion
            #region AddSale
            bundles.Add(new StyleBundle("~/styles/AddSale").Include(

                    "~/Content/FullCalendar/fullcalendar.css",
                    //"~/Content/PikDay/css/pikaday.css",
                    "~/Content/Css/AddSale/AddSale.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddSale").Include(

                    "~/Content/Js/Popups/AddSales.js",
                    /*"~/Content/momentJs/moment.min.js",*/
                    "~/Content/FullCalendar/fullcalendar.js",
                    //"~/Content/PikDay/js/moment.js",
                    //"~/Content/PikDay/js/pikaday.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddSale/AddSale.js"
                    ));
            #endregion
            #region AddServiceOrder
            bundles.Add(new StyleBundle("~/styles/AddServiceOrder").Include(

                    "~/Content/Css/AddServiceOrder/AddServiceOrder.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddServiceOrder").Include(

                    "~/Content/Js/Popups/AddServiceOrder.js",
                    //"~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddServiceOrder/AddServiceorder.js"
                    ));
            #endregion
            #region AddMmr
            bundles.Add(new StyleBundle("~/styles/AddMmr").Include(

                    "~/Content/Css/AddMmr/AddMmr.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddMmr").Include(
                    "~/Content/Js/Popups/AddMMR.js"
                    ));
            #endregion
            #region Setting
            bundles.Add(new StyleBundle("~/styles/Setting").Include(

                    //"~/Content/Css/Settings/Settings.css",
                    "~/Content/Css/Settings/Settings.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Setting").Include(

                    "~/Content/Js/Popups/EditSettings.js",
                    "~/Content/Js/Setting/Setting.js"
                    ));
            #endregion
            #region Modals
            bundles.Add(new StyleBundle("~/styles/Modals").Include(

                    "~/Content/Css/Modals/Modals.css",
                    "~/Content/FontAwesome/css/font-awesome.css",
                    "~/Content/Css/Shared/RightModal.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Modals").Include(
                    "~/Content/Js/Modals/Modals.js",
                     "~/Content/Js/Login/login.js",
                    "~/Content/Js/CryptoJS/3.1.9-1-crypto-js.min.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ModalsPublic").Include(
                    "~/Content/Js/Modals/Modals.js"
                    ));
            #endregion
            #region _LoadSalesReports
            bundles.Add(new StyleBundle("~/styles/_LoadSalesReportsCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/Report/LeadsReportPartial.css",
                    "~/Content/Css/Report/ReportsPartial.css"

                    ));
            #endregion
            #region AddSupplier
            bundles.Add(new StyleBundle("~/styles/AddSupplier").Include(

                    //"~/Content/Bootstrap/css/bootstrap.css",
                    "~/Content/Css/Popups/AddSupplierPopup.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddSupplier").Include(

                    //"~/Content/Jquery-ui/jquery.js",
                    //"~/Content/Bootstrap/js/bootstrap.min.js",
                    ///"~/Content/Js/Site/Validation.js",
                    "~/Content/Js/Popups/AddSupplier.js"
                    //"~/Content/DatePicker/Js/bootstrap-datepicker.min.js"
                    ));
            #endregion
            #region AddSchedule
            bundles.Add(new StyleBundle("~/styles/AddSchedule").Include(

                    "~/Content/Css/AddSchedule/AddSchedule.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddSchedule").Include(

                    "~/Content/Js/Popups/AddTechSchedule.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddSchedule/AddSchedule.js"
                    ));
            #endregion
            #region UserList
            bundles.Add(new StyleBundle("~/styles/UserList").Include(

                    "~/Content/Css/Shared/ListsCommon.css",
                    "~/Content/Css/Customer/CustomerList.css",
                     "~/Content/Css/Recruit/RecruitTabAdmin.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/UserList").Include(

                    "~/Content/Js/UserList/UserList.js"
                    ));
            #endregion
            #region UserManagement
            bundles.Add(new StyleBundle("~/styles/UserManagement").Include(

                    "~/Content/Css/UserManagement/UserManagement.css",
                    "~/Content/Css/UserManagement/UserGroup.css"

                    ));
            bundles.Add(new ScriptBundle("~/scripts/UserManagement").Include(

                    "~/Content/Js/UserManagement/UserManagement.js"
                    ));

            bundles.Add(new ScriptBundle("~/scripts/UserGroup").Include(

                    "~/Content/Js/UserManagement/UserGroup.js"
                    ));
            #endregion
            #region AddUser
            bundles.Add(new StyleBundle("~/styles/AddUser").Include(

                    "~/Content/Bootstrap/css/bootstrap.css",
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Popups/SmallPopups.css",
                    "~/Content/Css/AddUser/AddUser.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddUser").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddUser/AddUser.js"
                    ));
            #endregion
            #region Inventory
            bundles.Add(new StyleBundle("~/styles/Inventory").Include(

                    "~/Content/Css/Inventory/Inventory.css",
                    "~/Content/Css/Customer/CustomerList.css"
                    //"~/Content/Css/Shared/Header.css",
                    //"~/Content/datatable/dataTables.bootstrap.css",

                    ));
            bundles.Add(new ScriptBundle("~/scripts/Inventory").Include(

                    //"~/Content/datatable/jquery.dataTables.min.js",
                    //"~/Content/datatable/dataTables.bootstrap.js",
                    //"~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload-ui.js",
                    "~/Content/Js/Inventory/Inventory.js"
                    ));

            bundles.Add(new ScriptBundle("~/scripts/Transfer").Include(

                    "~/Content/Js/Inventory/InventoryTransfer.js"
                    ));
            bundles.Add(new StyleBundle("~/styles/Transfer").Include(

                    "~/Content/Css/Inventory/InventoryTransfer.css"
                    ));
            #endregion
            #region SupplierList
            bundles.Add(new StyleBundle("~/styles/SupplierList").Include(
                    "~/Content/Css/Supplier/SupplierList.css",
                    "~/Content/Css/Popups/AddSupplierPopup.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SupplierList").Include(

                    "~/Content/Js/SupplierList/SupplierList.js"
                    ));
            #endregion
            #region AddEquipment
            bundles.Add(new StyleBundle("~/styles/AddEquipment").Include(
                    "~/Content/Css/AddEquipment/AddEquipment.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddEquipment").Include(
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddEquipment/AddInventoryEquipment.js"

                    ));
            #endregion
            #region AddService
            bundles.Add(new StyleBundle("~/styles/AddService").Include(
                    "~/Content/Css/AddService/AddService.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddService").Include(
                    "~/Content/Js/Site/Validation.js",
                    "~/Content/Js/AddEquipment/AddService.js"
                    ));
            #endregion
            #region AdjustProductServiceQuantity
            bundles.Add(new StyleBundle("~/styles/AdjustProductServiceQuantity").Include(
                    "~/Content/Css/AdjustProductServiceQuantity/AdjustProductServiceQuantity.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AdjustProductServiceQuantity").Include(
                    "~/Content/Js/AdjustProductServiceQuantity/AdjustProductServiceQuantity.js"
                    ));
            #endregion
            #region AdjustStartingValue
            bundles.Add(new StyleBundle("~/styles/AdjustStartingValue").Include(
                    "~/Content/Css/AdjustStartingValue/AdjustStartingValue.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AdjustStartingValue").Include(
                    "~/Content/Js/AdjustStartingValue/AdjustStartingValue.js"
                    ));
            #endregion
            #region EquipmentList
            bundles.Add(new StyleBundle("~/styles/EquipmentList").Include(
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Equipment/EuipmentList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EquipmentList").Include(
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/EquipmentList/EquipmentList.js"
                    ));
            #endregion
            #region Filter EquipmentList
            bundles.Add(new StyleBundle("~/styles/FilterEquipmentList").Include(
                    "~/Content/Css/Equipment/FilteredEquipmentListCustom.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/FilterEquipmentList").Include(
                    "~/Content/Js/Inventory/FilterEquipmentList.js",
                    "~/Content/Js/FilteredEquipmentList/FilteredEquipmentList.js"
                    ));
            #endregion
            #region Tech Filter EquipmentList
            bundles.Add(new StyleBundle("~/styles/TechFilterEquipmentList").Include(
                    "~/Content/Css/Equipment/FilteredEquipmentList.css",
                    "~/Content/Css/Equipment/FilteredEquipmentListCustom.css",
                    "~/Content/Css/Inventory/TechFilteredEquipmentList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/TechFilterEquipmentList").Include(
                    "~/Content/Js/FilteredEquipmentList/FilteredEquipmentList.js",
                    "~/Content/Js/Inventory/TechFilterEquipmentList.js"
                    ));
            #endregion
            #region FilteredEquipmentList
            bundles.Add(new StyleBundle("~/styles/FilteredEquipmentList").Include(
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Equipment/EuipmentList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/FilteredEquipmentList").Include(
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/FilteredEquipmentList/FilteredEquipmentList.js"
                    ));
            #endregion
            #region StockStatusPartialView
            bundles.Add(new StyleBundle("~/styles/StockStatusPartialView").Include(
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Equipment/EuipmentList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/StockStatusPartialView").Include(
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js"
                    ));
            #endregion
            #region GetEstimate
            bundles.Add(new StyleBundle("~/styles/GetEstimate").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetEstimate/GetEstimate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/GetEstimate").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/GetEstimate/GetEstimate.js"
                    ));
            #endregion
            #region AddEstimate
            bundles.Add(new StyleBundle("~/styles/AddEstimate").Include(
                    "~/Content/Css/Estimate/AddEstimate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddEstimate").Include(
                    "~/Content/Js/AddEstimate/AddEstimateScript2.js"
                    ));
            #endregion
            #region GetInvoice
            bundles.Add(new StyleBundle("~/styles/GetInvoice").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetInvoice/GetInvoice.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/GetInvoice").Include(

                    "~/Content/Jquery-ui/jquery.js",

                    "~/Content/Js/GetInvoice/GetInvoice.js"
                    ));
            #endregion
            #region GetWorkOrder
            bundles.Add(new StyleBundle("~/styles/GetWorkOrder").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetInvoice/GetInvoice.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/GetWorkOrder").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/GetWorkOrder/GetWorkOrder.js"

                    ));
            #endregion
            #region AddInvoice
            bundles.Add(new StyleBundle("~/styles/AddInvoice").Include(
                    "~/Content/Css/Invoice/AddInvoice.css"
                    //"~/Content/PikDay/css/pikaday.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddInvoice").Include(
                    //"~/Content/DatePicker/Js/bootstrap-datepicker.min.js",
                    //"~/Content/Jquery-ui/jquery-ui.js",
                    //"~/Content/datatable/jquery.dataTables.min.js",
                    //"~/Content/Js/Invoice/AddInvoice.js",
                    //"~/Content/PikDay/js/moment.js",
                    //"~/Content/PikDay/js/pikaday.js",
                    "~/Content/Js/AddInvoice/Addinvoice2.js",
                    "~/Content/Js/Invoice/AddInvoice.js"
                    ));
            #endregion
            #region AddEquipmentServiceBundle
            bundles.Add(new StyleBundle("~/styles/AddEquipmentServiceBundle").Include(
                    "~/Content/Css/Inventory/AddEquipmentServiceBundle.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddEquipmentServiceBundle").Include(
                    "~/Content/Js/AddEquipmentServiceBundle/AddEquipmentServiceBundle.js"
                    ));
            #endregion
            #region CustomerSnapshot
            bundles.Add(new StyleBundle("~/styles/CustomerSnapshot").Include(
                    "~/Content/Css/CustomerSnapshot/CustomerSnapshot.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerSnapshot").Include(
                    "~/Content/Js/CustomerSnapshot/CustomerSnapshot.js"
                    ));
            #endregion
            #region Funding
            bundles.Add(new StyleBundle("~/styles/Funding").Include(
                    "~/Content/Css/Funding/Funding.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/Funding").Include(
                    "~/Content/Js/Funding/Funding.js"
                    ));
            #endregion
            #region WorkOrder
            bundles.Add(new StyleBundle("~/styles/WorkOrder").Include(
                    "~/Content/Css/WorkOrder/WorkOrder.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/WorkOrder").Include(
                    "~/Content/Js/WorkOrder/WorkOrder.js"
                    ));
            #endregion
            #region AddIncome
            bundles.Add(new StyleBundle("~/styles/AddIncome").Include(
                    "~/Content/Css/AddIncome/AddIncome.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddIncome").Include(
                    "~/Content/Js/Popups/AddIncome.js"
                    ));
            #endregion
            #region AddExpense
            bundles.Add(new StyleBundle("~/styles/AddExpense").Include(
                    "~/Content/Css/AddExpense/AddExpense.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddExpense").Include(

                    "~/Content/Js/Popups/AddExpense.js"
                    ));
            #endregion
            #region ServiceOrderProductInstallation
            bundles.Add(new StyleBundle("~/styles/ServiceOrderProductInstallation").Include(
                    "~/Content/Css/ServiceOrderProductInstallation/ServiceOrderProductInstallation.css",
                     "~/Content/Css/ServiceOrder/TopToBottomModalServiceOrder.css"
                    //"~/Content/PikDay/css/pikaday.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ServiceOrderProductInstallation").Include(
                    "~/Content/Js/ServiceOrderProductInstallation/ServiceOrderProductInstallation.js",
                    //"~/Content/PikDay/js/moment.js",
                    //"~/Content/PikDay/js/pikaday.js",
                    "~/Content/Js/ServiceOrderProductInstallation/AddProductServiceOrder.js"
                    ));
            #endregion
            #region CompanyBranch
            bundles.Add(new StyleBundle("~/styles/CompanyBranch").Include(
                    "~/Content/Css/CompanyBranch/CompanyBranch.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CompanyBranch").Include(
                    "~/Content/Js/Layout/metisMenu.min.js",
                    "~/Content/datatable/jquery.dataTables.min.js",
                    "~/Content/datatable/dataTables.bootstrap.js",
                    "~/Content/Js/CompanyBranch/CompanyBranch.js"
                    ));
            #endregion
            #region AddCompanyBranch
            bundles.Add(new StyleBundle("~/styles/AddCompanyBranch").Include(
                    "~/Content/Css/AddCompanyBranch/AddCompanyBranch.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddCompanyBranch").Include(
                    "~/Content/Js/Popups/AddCompanyBranch.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload-ui.js",
                    //"~/Content/JQueryFileUpload/jquery.fileupload.js",
                    "~/Content/Js/AddCompanyBranch/AddCompanyBranch.js"
                    ));
            #endregion
            #region CityTax
            bundles.Add(new StyleBundle("~/styles/CityTax").Include(
                    "~/Content/Css/CityTax/CityTax.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CityTax").Include(
                    "~/Content/Js/CityTax/CityTax.js"
                    ));
            #endregion
            #region AddCityTax
            bundles.Add(new StyleBundle("~/styles/AddCityTax").Include(
                    "~/Content/Css/AddCityTax/AddCityTax.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddCityTax").Include(
                    "~/Content/Js/Popups/AddCityTax.js"
                    ));
            #endregion
            #region TimeClock
            bundles.Add(new StyleBundle("~/styles/TimeClockHome").Include(
                    "~/Content/Css/TimeClockPto/TimeClockHome.css"
                    ));
            #endregion 
            #region TimeClockPartial
            bundles.Add(new StyleBundle("~/styles/TimeClockPartial").Include(
                    "~/Content/Css/TimeClockPto/TimeClockPartial.css"
                    ));
            #endregion
            #region PTOPartial
            bundles.Add(new StyleBundle("~/styles/PTOPartial").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/TimeClockPto/PTOPartial.css"
                    ));
            #endregion
            #region EmployeesPto
            bundles.Add(new StyleBundle("~/styles/EmployeesPto").Include(
                    "~/Content/Css/TimeClockPto/EmployeesPto.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EmployeesPto").Include(
                    "~/Content/Js/EmployeesPtoPartial.js"
                    ));
            #endregion 
            #region EmployeeTimeClocksHome
            bundles.Add(new StyleBundle("~/styles/EmployeeTimeClocksHome").Include(
                    "~/Content/Css/TimeClockPto/EmployeeTimeClocksHome.css"
                    ));
            #endregion
            #region EmployeeTimeClock
            bundles.Add(new StyleBundle("~/styles/EmployeeTimeClock").Include(
                    "~/Content/Css/TimeClockPto/EmployeeTimeClock.css"
                    ));
            #endregion
            #region EquipmentDetailPage
            bundles.Add(new StyleBundle("~/styles/EquipmentDetailPage").Include(
                    "~/Content/Css/Inventory/EquipmentDetailPage.css"
                    ));
            #endregion


            #region OpenEditBillingAddress
            bundles.Add(new StyleBundle("~/styles/OpenEditBillingAddressCS").Include(
                    "~/Content/Css/CityStateZipAutoFill.css",
                    "~/Content/Css/Booking/OpenEditBillingAddressModel.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/OpenEditBillingAddressJS").Include(
                    "~/Content/Js/CityStateZipAutoFill.js",
                    "~/Content/Js/Booking/OpenEditBillingAddressModel.js"
                    ));
            #endregion

            #region AddLeadBooking
            bundles.Add(new StyleBundle("~/styles/AddLeadBooking").Include(
                    "~/Content/Css/Estimate/AddEstimate.css",
                    "~/Content/Css/Booking/AddBooking.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddLeadBooking").Include(
                    "~/Content/Js/TinyInvoice.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddLeadBooking2").Include(
                    "~/Content/Js/LeadBooking/BookingCommons.js",
                    "~/Content/Js/LeadBooking/AddLeadBooking.js"
                   ));
            #endregion
            #region CCAddViewPaymentMethod
            bundles.Add(new StyleBundle("~/styles/CCAddViewPaymentMethodCSS").Include(
                    "~/Content/Css/CityStateZipAutoFill.css",
                    "~/Content/Css/SmartLeads/CCAddViewPaymentMethod.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CCAddViewPaymentMethodJS").Include(
                      "~/Content/Js/CityStateZipAutoFill.js",
                      "~/Content/Js/SmartLeads/CCAddViewPaymentMethod.js"

            ));
            #endregion

            #region SmartPackagePartial
            bundles.Add(new StyleBundle("~/styles/SmartPackagePartial").Include(
                 "~/Content/Css/Lead/PackagePartial.css"

                 ));
            bundles.Add(new ScriptBundle("~/scripts/SmartPackagePartial").Include(
                    "~/Content/Js/Package/SmartPackagePartial.js"

                    ));
            bundles.Add(new StyleBundle("~/styles/AddPackageService").Include(
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/PackageSettings/AddPackageOptional.css",
                    "~/Content/Css/PackageSettings/AddCompanyPackageServicesPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddPackageService").Include(
                "~/Content/Js/Site/Validation.js",
                "~/Content/Select2/Select2.min.js",
                "~/Content/Js/AddEquipment/EquipmentSelect2.js",
                "~/Content/Js/PackageSetup/AddNewSmartPackageServices.js"
                    ));
            #endregion

            #region CustomerCancellationConfirm
            bundles.Add(new StyleBundle("~/styles/CancellationConfirm").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                "~/Content/Select2/select2.css",
                 "~/Content/Css/Customer/CustomerCancellationConfirm.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/CancellationConfirm").Include(
                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js"
                 , "~/Scripts/CustomerSelect.js",
                 "~/Content/Js/CancellationQueue/CancellationConfirm.js"
                    ));
            #endregion
            #region TicketPreviewPopup
            bundles.Add(new ScriptBundle("~/scripts/TicketPreviewPopupJS").Include(


                    "~/Content/Jquery-ui/jquery.js",


                    "~/Content/Js/TicketPreviewPopup/TicketPreviewPopup.js"
                    ));
            #endregion
            #region CustomerCancellationPopup
            bundles.Add(new StyleBundle("~/styles/CancellationPopup").Include(
                 "~/Content/Css/CancellationQueue/CancellationPopup.css",
                  "~/Content/FontAwesome/css/font-awesome.min.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/CancellationPopup").Include(
                        "~/Content/Js/Login/domainurl.js",
                        "~/Content/Jquery-ui/jquery.js",
                        "~/Content/Js/SignaturePad/SignaturePad.js",
                        "~/Content/Js/NumberFormet/NumberFormat.js",
                        "~/Content/Js/CancellationQueue/CancellationPopUp.js"
                    ));
            bundles.Add(new StyleBundle("~/styles/CancellationCue").Include(
                 "~/Content/Css/Report/ReportsPartial.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/CancellationCue").Include(
                        "~/Content/Js/CancellationQueue/CancellationCue.js"
                    ));
            bundles.Add(new StyleBundle("~/styles/CancellationCuePartial").Include(
                 "~/Content/Css/Report/LeadsReportPartial.css",
                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Select2/select2.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/CancellationCuePartial").Include(
                        "~/Content/Select2/Select2.min.js",
                        "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                        "~/Content/Js/CancellationQueue/CancellationCuePartial.js"
                    ));
            #endregion
            #region Report
            bundles.Add(new ScriptBundle("~/scripts/Report").Include(
                        "~/Content/Js/Report/report.js"
                    ));
            #endregion
            #region CustomerCancellationAgreement
            bundles.Add(new StyleBundle("~/styles/CustomerCancellationAgreement").Include(
                             "~/Content/Css/CancellationQueue/CancellationAgrement.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerCancellationAgreement").Include(

                      "~/Content/Js/CancellationQueue/CancellationAgreement.js"
                     ));
            #endregion

            #region AddVendorBill
            bundles.Add(new StyleBundle("~/styles/AddVendorBillCSS").Include(
                    "~/Content/Css/Invoice/AddInvoice.css",
                    "~/Content/Css/Expense/AddVendorBill.css"
                    //"~/Content/PikDay/css/pikaday.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddVendorBillJS").Include(
                 "~/Content/Js/Expense/AddVendorBill.js",
                 //"~/Content/Tiny/jquery.tinymce.min.js",
                 //"~/Content/Tiny/tinymce.min.js",
                 "~/Content/Js/TinyInvoice.js"
                    ));
            #endregion

            #region CustomerSmartUiSettingPartial
            bundles.Add(new StyleBundle("~/styles/CustomerSmartUiSettingPartialCS").Include(
                    "~/Content/Css/Setup/CustomerUiSettingsPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CustomerSmartUiSettingPartialJS").Include(
                    "~/Content/Js/CustomerSmartUiSettingPartial/CustomerSmartUiSettingPartial.js"
                     ));
            #endregion

            #region SmartUiSetup
            bundles.Add(new StyleBundle("~/styles/SmartUiSetup").Include(
                  "~/Content/Css/Setup/SetupDetails.css"
                  ));
            bundles.Add(new ScriptBundle("~/scripts/SmartUiSetup").Include(
                    "~/Content/Js/Setup/SmartUiSetup.js"
                     ));
            #endregion


            #region TicketPreviewPopup
            bundles.Add(new ScriptBundle("~/scripts/TicketPreviewPopupJS").Include(
                "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/TicketPreviewPopup/TicketPreviewPopup.js",
                    "~/Content/Js/GetPrintPreviewTicket.js"
                     ));
            #endregion

            #region AddEmployeesPto
            bundles.Add(new StyleBundle("~/styles/AddEmployeesPtoCSS").Include(
                    "~/Content/Css/AddPtoPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddEmployeesPtoJS").Include(
                "~/Content/Js/Addemployeespto.js"
                    ));
            #endregion

            #region AddPto
            bundles.Add(new StyleBundle("~/styles/AddPtoCSS").Include(
                    "~/Content/Css/AddPtoPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddPtoJS").Include(
                "~/Content/Js/AddPtoPartial.js"
                    ));
            #endregion
            #region AddClock
            bundles.Add(new StyleBundle("~/styles/AddClock").Include(
                "~/Content/PikDay/css/bootstrap-timepicker.min.css",
                    "~/Content/PikDay/css/pikaday.css",
                    "~/Content/Css/WickedPicker/WickedPicker.css",

                    "~/Content/Css/AddClock.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddClock").Include(
               "~/Content/PikDay/js/pikaday.js",
                "~/Content/PikDay/js/bootstrap-timepicker.js",
                "~/Content/Js/AddClockInOut/AddClock.js",
                "~/Content/Js/WickedPicker/wickedpicker.js"
                    ));
            #endregion

            #region PayrollReportTimeClock
            bundles.Add(new ScriptBundle("~/scripts/PayrollReportTimeClock").Include(
                "~/Content/Js/PayrollReportTimeClock.js"
                    ));
            #endregion

            #region EmployeeTimeClockList
            bundles.Add(new StyleBundle("~/styles/EmployeeTimeClockList").Include(
                "~/Content/Css/Pagination.css",
                "~/Content/Css/TimeClockPto/EmployeeTimeClockList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EmployeeTimeClockList").Include(
                "~/Content/Js/EmployeeTimeClockList.js"
                    ));
            #endregion

            #region FileTempletes
            bundles.Add(new StyleBundle("~/styles/FileTemplate").Include(
                "~/Content/Css/FileTemplate/FileTemplate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/FileTemplate").Include(
                "~/Content/Js/FileTemplate/FileTemplate.js"
                    ));
            #endregion
            #region AddFileTempletes
            bundles.Add(new StyleBundle("~/styles/AddFileTemplate").Include(
                "~/Content/Css/FileTemplate/AddFileTemplate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddFileTemplate").Include(
                "~/Content/Js/FileTemplate/AddFileTemplate.js"
                    ));
            #endregion

            #region AddMenu
            bundles.Add(new StyleBundle("~/styles/AddMenu").Include(
                "~/Content/Css/Menu/AddMenu.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddMenu").Include(
                "~/Content/Js/Menu/AddMenu.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddMenuItem").Include(
                "~/Content/Js/Menu/AddMenuItem.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/MenuItemList").Include(
                "~/Content/Js/Menu/MenuItemList.js"
                    ));
            #endregion

            #region PayrollReportTimeClock1
            bundles.Add(new StyleBundle("~/styles/PayrollReportTimeClock1").Include(
                "~/Content/Css/Pagination.css",
                "~/Content/Css/TimeClockPto/PayrollReport.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/PayrollReportTimeClock1").Include(
                "~/Content/Js/PayrollReportTC.js"
                    ));
            #endregion

            #region TicketBooking
            bundles.Add(new ScriptBundle("~/scripts/TicketBooking").Include(
                "~/Content/Js/LeadBooking/BookingCommons.js",
                "~/Content/Js/TIcket/TicketBooking.js"
                 ));
            #endregion

            #region _AddTicKet
            bundles.Add(new StyleBundle("~/styles/AddTicKetCS").Include(
                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css",
                     "~/Content/Css/Ticket/AddTicket.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AddTicKetJSAll").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Scripts/CustomerSelect.js",
                   "~/Content/Js/ez.countimer.js",
                   "~/Content/Js/TIcket/PackageChange.js",
                   "~/Content/Js/TicketSalesComission.js"
                   ));

            bundles.Add(new ScriptBundle("~/scripts/AddTicKetJS").Include(
                    "~/Content/Js/TIcket/AddTicket.js"
                    ));
            #endregion

            #region UserInformation
            bundles.Add(new StyleBundle("~/styles/UserInformation").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                "~/Content/Select2/select2.css",
                "~/Content/Css/UserManagement/UserInformation.css",
                "~/Content/Css/CustomerDetails/CustomerDetailsCustom.css",
                "~/Content/Css/CityStateZipAutoFill.css"

                    ));
            bundles.Add(new ScriptBundle("~/scripts/UserInformation").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                "~/Content/Select2/Select2.min.js",
                "~/Content/Js/CityStateZipAutoFill.js",
                "~/Content/Js/UserInformation.js"
                    ));
            #endregion


            #region _LeadEstimatePartial
            bundles.Add(new StyleBundle("~/styles/_LeadEstimatePartialCS").Include(
                    "~/Content/Css/Estimate/Estimate.css",
                    "~/Content/Css/Estimate/_LeadEstimatePartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LeadEstimatePartialJS").Include(
                   "~/Content/Js/Estimate/_LeadEstimatePartial.js"
                    ));
            #endregion

            #region CorrespondenceList
            bundles.Add(new StyleBundle("~/styles/CorrespondenceList").Include(
                    "~/Content/Css/Lead/CorrespondenceList.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CorrespondenceList").Include(
                   "~/Content/Js/CorrespondenceList.js"
                    ));
            #endregion

            #region SchedulePartial
            bundles.Add(new StyleBundle("~/styles/SchedulePartial").Include(
                    "~/Content/Css/SchedulePartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SchedulePartial").Include(
                   "~/Content/Js/SchedulePartial.js"
                    ));
            #endregion
            #region SendEmailEstimate
            bundles.Add(new StyleBundle("~/styles/SendEmailEstimateCS").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetEstimate/GetEstimate.css",
                    "~/Content/Css/Estimate/SendEmailEstimate.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SendEmailEstimateJS").Include(
                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Login/domainurl.js",
                    "~/Content/Js/GetEstimate/GetEstimate.js",
                    "~/Content/Js/Estimate/SendEmailEstimate.js",
                    "~/Content/Js/Layout/utils.js",
                    "~/Content/Bootstrap/js/bootstrap.min.js",
                    "~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
                    "~/Content/JQueryFileUpload/jquery.fileupload.js",
                    "~/Content/JQueryFileUpload/jquery.fileupload-ui.js"
                    ));
            #endregion
            #region EstimatePartial
            bundles.Add(new StyleBundle("~/styles/EstimatePartialCS").Include(
                    "~/Content/Css/Estimate/Estimate.css",
                    "~/Content/Css/Estimate/EstimatePartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EstimatePartialJS").Include(
                    "~/Content/Js/Estimate/Estimate.js",
                    "~/Content/Js/Estimate/EstimatePartial.js"
                     ));
            #endregion

            #region EstimatorPartial

            bundles.Add(new ScriptBundle("~/scripts/AddEstimator").Include(
                    "~/Content/Js/Estimator/Estimator.js",
                    "~/Content/Js/Estimator/EstimatorPartial.js"
                     ));
            #endregion

            #region _InvoiceListPartial
            bundles.Add(new StyleBundle("~/styles/InvoiceListPartialCS").Include(

                    "~/Content/Css/Invoice/Invoice.css",
                    "~/Content/Css/Invoice/_InvoiceListPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/InvoiceListPartialJS").Include(

                    "~/Content/Js/Invoice/Invoice.js",
                    "~/Content/Js/Invoice/_InvoiceListPartial.js"
                     ));
            #endregion

            #region SendEmailInvoicePartial
            bundles.Add(new StyleBundle("~/styles/SendEmailInvoicePartialCS").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetEstimate/GetEstimate.css",
                    "~/Content/Css/Invoice/SendEmailInvoicePartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SendEmailInvoicePartialJS").Include(

                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Invoice/SendEmailInvoicePartial.js"
                    ));
            #endregion
            #region SendEmailInvoice
            bundles.Add(new StyleBundle("~/styles/SendEmailInvoiceCS").Include(
                    "~/Content/Bootstrap/css/bootstrap.min.css",
                    "~/Content/Css/GetEstimate/GetEstimate.css",
                    "~/Content/Css/Invoice/SendEmailInvoice.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/SendEmailInvoiceJS").Include(
                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/GetEstimate/GetEstimate.js",
                    "~/Content/Js/Login/domainurl.js",
                    "~/Content/Js/Invoice/SendEmailInvoice.js"
                    ));
            #endregion

            #region ScheduleGoogleMap
            bundles.Add(new StyleBundle("~/styles/ScheduleGoogleMap").Include(
                    "~/Content/Css/ScheduleGoogleMap.css",
                    "~/Content/FontAwesome/css/font-awesome.min.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/ScheduleGoogleMap").Include(
                    "~/Content/JQuery/jquery-3.2.1.min.js",
                    "~/Content/Js/Login/domainurl.js",
                    "~/Content/Js/Modals/Modals.js",
                    "~/Content/Js/ScheduleGoogleMap.js"
                    ));
            #endregion
            #region TechnicianDashboard
            bundles.Add(new StyleBundle("~/styles/TechnicianDashboardCS").Include(
                    "~/Content/Css/App/TechnicianDashboard.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/TechnicianDashboardJS").Include(
                   "~/Content/Js/TechnicianDashBoard/TechnicianDashBoard.js"
                    ));
            #endregion
            #region Alarm.com

            bundles.Add(new StyleBundle("~/styles/Alarm").Include(
                          "~/Content/Css/AlarmDotCom/alarm.css"
                          ));
            bundles.Add(new StyleBundle("~/styles/AlarmDetails").Include(
                        "~/Content/Css/AlarmDotCom/AlarmDetails.css"
                        ));
            bundles.Add(new ScriptBundle("~/scripts/AlarmEquipment").Include(
                       "~/Content/Js/AlarmDotCom/alarmequipment.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/AlarmDetails").Include(
                    "~/Content/Js/AlarmDotCom/alarmdetails.js"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/Alarm").Include(
                        "~/Content/Js/AlarmDotCom/alarm.js"
             ));
            #endregion
            #region BadInventoryUserAssign
            bundles.Add(new StyleBundle("~/styles/BadInventoryUserAssign").Include(
                    "~/Content/Css/BadInventoryUserAssign.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/BadInventoryUserAssign").Include(
                    "~/Content/JQuery/jquery-3.2.1.min.js",
                    "~/Content/Js/BadInventoryUserAssign.js"
                    ));
            #endregion
            #region TechnicianUpsoldList
            bundles.Add(new StyleBundle("~/styles/TechnicianUpsoldListCS").Include(
                    "~/Content/Css/Technician/TechnicianUpsoldList.css",
                    "~/Content/Css/Expense/AddVendorBill.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/TechnicianUpsoldListJS").Include(
                    "~/Content/Js/TechnicianDashboard/TechnicianUpsoldList.js"
                    ));
            #endregion
            #region TechnicianUpsoldPartialList
            bundles.Add(new StyleBundle("~/styles/TechnicianUpsoldListPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Technician/TechnicianEquipmentListPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/TechnicianUpsoldListPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Js/TechnicianDashboard/TechnicianUpsoldListPartial.js"
                    ));
            #endregion

            #region _LoadBookingSalesReportPartial
            bundles.Add(new StyleBundle("~/styles/_LoadBookingSalesReportPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Report/_LoadBookingSalesReportPartial.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LoadBookingSalesReportPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_LoadBookingSalesReportPartial.js"
                    ));
            #endregion

            #region _LoadPartnerReportPartial
            bundles.Add(new StyleBundle("~/styles/_LoadPartnerReportPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Report/_LoadPartnerReportPartial.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LoadPartnerReportPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_LoadPartnerReportPartial.js"
                    ));
            #endregion
            #region BookingSalesReportsPartial
            bundles.Add(new StyleBundle("~/styles/BookingSalesReportsCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/Report/LeadsReportPartial.css",
                    "~/Content/Css/Report/ReportsPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/BookingSalesReportsJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/BookingSalesReports.js"
                    ));
            #endregion

            #region _PartnerReportsPartial
            bundles.Add(new StyleBundle("~/styles/_PartnerReportCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/Report/LeadsReportPartial.css",
                    "~/Content/Css/Report/ReportsPartial.css"
                    //"~/Content/Css/Report/PartnerReports.css" 
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_PartnerReportsJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_PartnerReports.js"
                    ));
            #endregion

            #region _LoadPartnerReportBarPartial
            bundles.Add(new StyleBundle("~/styles/_LoadPartnerReportBarPartialCS").Include(
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Report/_LoadPartnerReportBarPartial.css"
                    ));

            #endregion

            #region _DateViewPartial
            bundles.Add(new StyleBundle("~/styles/_DateViewPartialCS").Include(
                    "~/Content/Css/Customer/DateViewPartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_DateViewPartialJS").Include(
                    "~/Content/Js/Customer/DateViewPartial.js"

                    ));
            #endregion

            #region _LeadSourceReportsPartial
            bundles.Add(new StyleBundle("~/styles/_LeadSourceReportCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/Report/LeadsReportPartial.css",
                    "~/Content/Css/Report/ReportsPartial.css"
                    //"~/Content/Css/Report/PartnerReports.css" 
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LeadSourceReportsJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_LeadSourceReports.js"
                    ));
            #endregion

            #region _LoadLeadSourceReportPartial
            bundles.Add(new StyleBundle("~/styles/_LoadLeadSourceReportPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Report/_LoadLeadSourceReportPartial.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LoadLeadSourceReportPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_LoadLeadSourceReportPartial.js"
                    ));
            #endregion

            #region _LoadLeadSourceReportBarPartial
            bundles.Add(new StyleBundle("~/styles/_LoadLeadSourceReportBarPartialCS").Include(
                    "~/Content/datatable/dataTables.bootstrap.css",
                    "~/Content/Css/Report/_LoadLeadSourceReportBarPartial.css"
                    ));

            #endregion

            #region _LoadCollectionReportPartial
            bundles.Add(new StyleBundle("~/styles/_LoadCollectionReportPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Report/_LoadCollectionReportPartial.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LoadCollectionReportPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/Report/_LoadCollectionReportPartial.js"
                    ));
            #endregion

            #region _LoadLogPartial
            bundles.Add(new StyleBundle("~/styles/_LoadLogPartialCS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/UserActivityLog/_LoadLogPartial.css",
                    "~/Content/Select2/select2.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/_LoadLogPartialJS").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/UserActivityLog/_LoadLogPartial.js"
                    ));
            #endregion

            #region PublicAgreement
            bundles.Add(new StyleBundle("~/styles/PublicAgreement").Include(
                   "~/Content/Bootstrap/css/bootstrap.min.css",
                   "~/Content/Css/GetInvoice/PublicAgreement.css",
                   "~/Content/Css/Public/LeadsAgreementDocument.css",
                   "~/Content/Css/Layout/Loader.css"
                   ));
            bundles.Add(new ScriptBundle("~/scripts/PublicAgreement").Include(
                    "~/Content/Js/Layout/Device.js",
                    "~/Content/Js/LeadsAgreementDocument/LeadsAgreementDocument.js"
                    ));
            #endregion

            #region Customerinfoshare

            bundles.Add(new ScriptBundle("~/scripts/CustomerInfoJS").Include(
                    "~/Content/Jquery-ui/jquery.js",
                    "~/Content/Js/Login/domainurl.js",
                    "~/Content/Js/CustomerDetails/CustomerInfo.js",
                    "~/Content/Js/Layout/utils.js",
                    "~/Content/Bootstrap/js/bootstrap.min.js",
                    "~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
                    "~/Content/JQueryFileUpload/jquery.fileupload.js",
                    "~/Content/JQueryFileUpload/jquery.fileupload-ui.js"
                    ));

            #endregion
            #region Calendar Bundles
            #region Calendar view
            bundles.Add(new StyleBundle("~/styles/CalendarPartial").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Calendar/CalendarPartial.css",
                    "~/Content/Css/SchedulePartial.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CalendarPartial").Include(
                "~/Content/momentJs/MomentSchedule.js",
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Content/Js/Calendar/CalendarPartial.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LoadCalenerMap").Include(
                "~/Content/Js/Login/domainurl.js",
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                "~/Content/Js/Calendar/CalendarGoogleMap.js"
                    ));
            #endregion
            #region CalendarSetup
            bundles.Add(new StyleBundle("~/styles/CalendarSetupDetails").Include(
                    "~/Content/Css/Setup/SetupDetails.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/CalendarSetupDetails").Include(
                   "~/Content/Js/Calendar/CalendarSetupDetails.js"
                    ));
            #endregion
            #region EditCalendarSettings
            bundles.Add(new StyleBundle("~/styles/EditCalendarSettings").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                "~/Content/Css/Settings/Settings.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EditCalendarSettings").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                "~/Content/Tiny/jquery.tinymce.min.js",
                "~/Content/Tiny/tinymce.min.js",
                "~/Content/Tiny/tiny.js",
                "~/Content/Js/Calendar/EditCalendarSettings.js"
                    ));
            #endregion
            #region CalendarPTO
            bundles.Add(new ScriptBundle("~/scripts/CalendarPTO").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Content/Js/Calendar/CalendarPTO.js"
                    ));
            #endregion
            #region CalendarVarJS
            bundles.Add(new ScriptBundle("~/scripts/CalendarVarJS").Include(
                   "~/Content/Js/Calendar/CalendarVarJS.js"
                    ));
            #endregion
            #region CalendarScheduleCalendar
            bundles.Add(new ScriptBundle("~/scripts/CalendarScheduleCalendar").Include(
                   "~/Content/Js/Calendar/ScheduleCalendar.js"
                    ));
            #endregion
            #region CalendarGoogleMap
            bundles.Add(new ScriptBundle("~/scripts/CalendarGoogleMap").Include(
                    "~/Content/JQuery/jquery-3.2.1.min.js",
                    "~/Content/Js/Login/domainurl.js",
                    "~/Content/Js/Modals/Modals.js",
                    "~/Content/Js/ScheduleGoogleMap.js"
                    ));
            #endregion
            #region ScheduleUserListPartial
            bundles.Add(new ScriptBundle("~/scripts/ScheduleUserListPartial").Include(
                   "~/Content/Js/Calendar/ScheduleUserListPartial.js"
                    ));
            #endregion
            #region TicketStatusImageSettings
            bundles.Add(new ScriptBundle("~/scripts/TicketStatusImageSettings").Include(
                "~/Content/Js/Calendar/StatusImageUpload.js",
                   "~/Content/Js/Calendar/TicketStatusImageSettings.js"
                    ));
			#region AddInvoiceTinyMce
            bundles.Add(new ScriptBundle("~/scripts/AddInvoiceTinyMce").Include(
                   "~/Content/Tiny/jquery.tinymce.min.js",
                   "~/Content/Tiny/tinymce.min.js",
                   "~/Content/Js/TinyInvoice.js"
                   ));
            #endregion

            #region Export Confirm 

            bundles.Add(new StyleBundle("~/styles/exportconfirm").Include(

                   "~/Content/perfect-scrollbar/css/perfect-scrollbar.css",
                   "~/Content/Css/Report/ExportConfirm.css"
                   ));

            bundles.Add(new ScriptBundle("~/scripts/exportconfirm").Include(
                   "~/Content/JQuery/jquery-3.1.1.js",
                   "~/Content/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js",
                   "~/Content/Js/Login/domainurl.js",
                   "~/Content/Js/Report/ExportConfirm.js"
                   ));

            #endregion

            #region GoBackReportList 



            bundles.Add(new ScriptBundle("~/scripts/GoBackReportListJS").Include(

                   "~/Content/Js/Report/GoBackReportList.js"
                   ));

            #endregion
			#region AppointmentDateReportList 
            bundles.Add(new ScriptBundle("~/scripts/appointmentdatereportlist").Include(
                   "~/Content/Js/Report/appointmentdatereportlist.js"
                   ));
            #endregion
			           #region DateReferenceReportList 
            bundles.Add(new ScriptBundle("~/scripts/datereferencereportlist").Include(
                   "~/Content/Js/Report/datereferencereportlist.js"
                   ));
            #endregion
            #region LoadInvoiceListReportPartial
            bundles.Add(new StyleBundle("~/styles/LoadInvoiceListReportPartial").Include(

                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Select2/select2.css",
                 "~/Content/Css/Ticket/TicketListPartial.css"));
            #endregion
			
            bundles.Add(new ScriptBundle("~/scripts/LoadInvoiceListReportPartial").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Content/Select2/Select2.min.js",
                   "~/Content/Js/Report/_LoadInvoiceListReportPartial.js"
                   ));
            #endregion
            #region LoadSalesReportPartial
            bundles.Add(new StyleBundle("~/styles/LoadSalesReportPartial").Include(

                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Select2/select2.css",
                 "~/Content/Css/Ticket/TicketListPartial.css"
                 ));

            bundles.Add(new ScriptBundle("~/scripts/LoadSalesReportPartial").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Content/Select2/Select2.min.js",
                   "~/Content/Js/Report/_LoadSalesReportPartial.js"
                   ));
            #endregion
            #region InstalledDealsReportList
            bundles.Add(new StyleBundle("~/styles/InstalledDealsReportList").Include(

                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Css/Ticket/TicketListPartial.css",
                 "~/Content/Css/Report/RecurringBillingReportPartial.css"
                 ));

            bundles.Add(new ScriptBundle("~/scripts/InstalledDealsReportList").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js"
       
                   ));
            #endregion
            #region NewSalesReportPartialList
            bundles.Add(new StyleBundle("~/styles/NewSalesReportPartialList").Include(

                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Css/Ticket/TicketListPartial.css",
                 "~/Content/Css/Report/RecurringBillingReportPartial.css"
                 ));

            bundles.Add(new ScriptBundle("~/scripts/NewSalesReportPartialList").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js"

                   ));
            #endregion
            #region SalesSummaryReportPartial
            bundles.Add(new StyleBundle("~/styles/SalesSummaryReportPartial").Include(

                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                 "~/Content/Css/Ticket/TicketListPartial.css",
                 "~/Content/Css/Report/RecurringBillingReportPartial.css"
                 ));

            bundles.Add(new ScriptBundle("~/scripts/SalesSummaryReportPartial").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                   "~/Content/Js/Report/SalesSummaryReportPartial.js"));

            #region CalendarTicketTypeColorChange

            bundles.Add(new ScriptBundle("~/scripts/CalendarTicketTypeColorChange").Include(
               "~/Content/Js/Calendar/StatusImageUpload.js",
                  "~/Content/Js/Calendar/CalendarTicketTypeColorChange.js"
                   ));
            #endregion
            #endregion
            #region Rug Report 
            #region Employee Tracking Report
            bundles.Add(new StyleBundle("~/styles/EmployeeTrackingReport").Include(
                    "~/Content/Css/Report/PayrollReport.css",
                    "~/Content/Css/Report/EmployeeTrackingReport.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/EmployeeTrackingReport").Include(
                   "~/Content/Js/Report/EmployeeTrackingReport.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LoadEmployeeTrackingReportPartial").Include(
                   "~/Content/Js/RugReport/LoadEmployeeTrackingReportPartial.js"
                    ));
            #endregion
            #region Pad Order Report
            bundles.Add(new StyleBundle("~/styles/PadOrderReport").Include(
                    "~/Content/Css/Report/PayrollReport.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/PadOrderReport").Include(
                   "~/Content/Js/Report/PadOrderReport.js"
                    ));
            #endregion
            #region Pad Order Report
            bundles.Add(new StyleBundle("~/styles/PadOrderReport").Include(
                    "~/Content/Css/Report/PayrollReport.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/PadOrderReport").Include(
                   "~/Content/Js/Report/PadOrderReport.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/OversalesReport").Include(
                  "~/Content/Js/Report/OversalesReport.js"
                   ));
            #endregion
            #region Ticket Report
            bundles.Add(new StyleBundle("~/styles/TicketReport").Include(
                "~/Content/Css/Report/LeadsReportPartial.css",
                 "~/Content/Css/Report/ReportsPartial.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/TicketReport").Include(
                  "~/Content/Js/Report/TicketReport.js"
                   ));
            bundles.Add(new ScriptBundle("~/scripts/TicketReportPartial").Include(
                  "~/Content/Js/Report/TicketReportPartial.js"
                   ));
            bundles.Add(new ScriptBundle("~/scripts/TicketReportAppointmentList").Include(
                  "~/Content/Js/Report/TicketReportAppointmentList.js"
                   ));
            bundles.Add(new StyleBundle("~/styles/LoadTicketReportList").Include(                 
                 "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                    "~/Content/Css/Ticket/TicketListPartial.css",
                    "~/Content/Select2/select2.css",
                    "~/Content/Css/Report/LoadTicketReportList.css"
                 ));
            bundles.Add(new ScriptBundle("~/scripts/LoadTicketReportList").Include(
                "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                  "~/Content/Js/Report/LoadTicketReportList.js"
                   ));
            #endregion
            #region Rug Progression Report
            bundles.Add(new ScriptBundle("~/scripts/RugProgressionReport").Include(
                   "~/Content/Js/Report/RugProgressionReport.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/RugProgressionReportPopup").Include(
                   "~/Content/Js/RugReport/RugProgressionReportPopup.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/RugProgressionReportDetails").Include(
                   "~/Content/Js/RugReport/RugProgressionReportDetails.js"
                    ));
            #endregion
            #endregion
            #region Lookup Setup
            #region Lookup Items
            bundles.Add(new StyleBundle("~/styles/LookupItems").Include(
                   "~/Content/Css/Setup/LookupItems.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LookupItems").Include(
               "~/Content/Js/LookupItem/lookupitem.js",
                "~/Content/Js/jscolor.js"
                    ));
            #endregion
            #region Lookup Items With Parent
            bundles.Add(new StyleBundle("~/styles/LookupItemsWithParent").Include(
                   "~/Content/Css/Setup/LookupItemsWithParent.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/LookupItemsWithParent").Include(
               "~/Content/Js/LookupItem/lookupitemwithparent.js",
                "~/Content/Js/jscolor.js"
                    ));
            #endregion
            #region Rug Condition
            bundles.Add(new StyleBundle("~/styles/RugConditionPopup").Include(
                  "~/Content/Css/Ticket/RugConditionPopup.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/RugConditionPopup").Include(
            "~/Content/Jquery-ui/jquery.js",
            "~/Content/JQueryFileUpload/jquery-ui-1.9.2.min.js",
            "~/Content/JQueryFileUpload/jquery.fileupload.js",
            "~/Content/JQueryFileUpload/jquery.fileupload-ui.js",
            "~/Content/Js/TIcket/RugConditionPopup.js"
                    ));
            #endregion
            #endregion
            #region Note Ticket Reply
            bundles.Add(new StyleBundle("~/styles/TicketJS").Include(
                  "~/Content/Css/Ticket/AddTicket.css"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/NoteTicketReplyPartial").Include(
            "~/Content/Js/Note/LoadNoteTicketReplyPartial.js"
                    ));
            #endregion

            #region Rug Report
            bundles.Add(new StyleBundle("~/styles/RugReport").Include(
                   "~/Content/Bootstrap/MultipleSelect/bootstrap-select.css",
                  "~/Content/Select2/select2.css",
                  "~/Content/Css/Report/ReportsPartial.css",
                  "~/Content/Css/Report/LeadsReportPartial.css"

                 ));
            bundles.Add(new ScriptBundle("~/scripts/RugReport").Include(
                    "~/Content/Bootstrap/MultipleSelect/bootstrap-select.js",
                    "~/Content/Select2/Select2.min.js",
                    "~/Content/Js/RugReport/RugReport.js"
                    ));
            #endregion
            #region Rug Repair Report
            bundles.Add(new ScriptBundle("~/scripts/RugRepairReport").Include(
                   "~/Content/Js/RugReport/RugRepairReport.js"
                   ));
            #endregion
            #endregion
            BundleTable.EnableOptimizations = false;

            //if(!string.IsNullOrWhiteSpace(AppConfig.Production) && AppConfig.Production.ToLower() == "true")
            //{
            //    BundleTable.EnableOptimizations = true;
            //}
            //else
            //{
            //    BundleTable.EnableOptimizations = false;
            //}
            //BundleTable.EnableOptimizations = false;
            //BundleTable.EnableOptimizations = true;
        }
    }
}