using System;
using System.Collections.Generic;
using System.ServiceModel;
using HS.Entities;
using HS.Framework;

namespace HS.Facade
{
    public class TheFacade : IDisposable, System.ServiceModel.IExtension<System.ServiceModel.OperationContext>
    {
        private ClientContext _Context;

        public TheFacade(Client client)
        {
            _Context = new ClientContext(client);
        }
        public LocalizeFacade LocalizeFacade
        {
            get
            {
                return (LocalizeFacade)_Context[typeof(LocalizeFacade)];
            }
        }
        public MailFacade MailFacade
        {
            get
            {
                return (MailFacade)_Context[typeof(MailFacade)];
            }
        }
        public CompanyHolidayFacade CompanyHolidayFacade
        {
            get
            {
                return (CompanyHolidayFacade)_Context[typeof(CompanyHolidayFacade)];
            }
        }
        public CustomSurveyFacade CustomSurveyFacade
        {
            get
            {
                return (CustomSurveyFacade)_Context[typeof(CustomSurveyFacade)];
            }
        }
        public ApiFacade ApiFacade
        {
            get
            {
                return (ApiFacade)_Context[typeof(ApiFacade)];
            }
        }
        public HSApiFacade HSApiFacade
        {
            get
            {
                return (HSApiFacade)_Context[typeof(HSApiFacade)];
            }
        }
        public HSMainApiFacade HSMainApiFacade
        {
            get
            {
                return (HSMainApiFacade)_Context[typeof(HSMainApiFacade)];
            }
        }
        public UserActivityFacade UserActivityFacade
        {
            get
            {
                return (UserActivityFacade)_Context[typeof(UserActivityFacade)];
            }
        }
        public UserActivityCustomerFacade UserActivityCustomerFacade
        {
            get
            {
                return (UserActivityCustomerFacade)_Context[typeof(UserActivityCustomerFacade)];
            }
        }
        public UserLoginFacade UserLoginFacade
        {
            get
            {
                return (UserLoginFacade)_Context[typeof(UserLoginFacade)];
            }
        }

        public ThirdPartyCustomerFacade ThirdPartyCustomerFacade
        {
            get
            {
                return (ThirdPartyCustomerFacade)_Context[typeof(ThirdPartyCustomerFacade)];
            }
        }
        public RestaurantSiteConfigurationFacade RestaurantSiteConfigurationFacade
        {
            get
            {
                return (RestaurantSiteConfigurationFacade)_Context[typeof(RestaurantSiteConfigurationFacade)];
            }
        }
        public ServiceFeeFacade ServiceFeeFacade
        {
            get
            {
                return (ServiceFeeFacade)_Context[typeof(ServiceFeeFacade)];
            }
        }
        public TicketTimeClockFacade TicketClockFacade
        {
            get
            {
                return (TicketTimeClockFacade)_Context[typeof(TicketTimeClockFacade)];
            }
        }
        public CompanyFacade CompanyFacade
        {
            get
            {
                return (CompanyFacade)_Context[typeof(CompanyFacade)];
            }
        }
        public ManufacturerFacade ManufacturerFacade
        {
            get
            {
                return (ManufacturerFacade)_Context[typeof(ManufacturerFacade)];
            }
        }


        public CustomerFacade CustomerFacade
        {
            get
            {
                return (CustomerFacade)_Context[typeof(CustomerFacade)];
            }
        }
        public ServiceAreaFacade ServiceAreaFacade
        {
            get
            {
                return (ServiceAreaFacade)_Context[typeof(ServiceAreaFacade)];
            }
        }

        public SupplierFacade SupplierFacade
        {
            get
            {
                return (SupplierFacade)_Context[typeof(SupplierFacade)];
            }
        }


        public MarchantFacade MarchantFacade
        {
            get
            {
                return (MarchantFacade)_Context[typeof(MarchantFacade)];
            }
        }

        public ActivationFeeFacade ActivationFeeFacade
        {
            get
            {
                return (ActivationFeeFacade)_Context[typeof(ActivationFeeFacade)];
            }
        }

        public PermissionFacade PermissionFacade
        {
            get
            {
                return (PermissionFacade)_Context[typeof(PermissionFacade)];
            }
        }
        public LookupFacade LookupFacade
        {
            get
            {
                return (LookupFacade)_Context[typeof(LookupFacade)];
            }
        }

        public EquipmentTypeFacade EquipmentTypeFacade
        {
            get
            {
                return (EquipmentTypeFacade)_Context[typeof(EquipmentTypeFacade)];
            }
        }
        public UserCompanyFacade UserCompanyFacade
        {
            get
            {
                return (UserCompanyFacade)_Context[typeof(UserCompanyFacade)];
            }
        }
        public MmrFacade MmrFacade
        {
            get
            {
                return (MmrFacade)_Context[typeof(MmrFacade)];
            }
        }

        public InventoryFacade InventoryFacade
        {
            get
            {
                return (InventoryFacade)_Context[typeof(InventoryFacade)];
            }
        }
        public PayrollFacade PayrollFacade
        {
            get
            {
                return (PayrollFacade)_Context[typeof(PayrollFacade)];
            }
        }
        public MenuFacade MenuFacade
        {
            get
            {
                return (MenuFacade)_Context[typeof(MenuFacade)];
            }
        }
        public CategoryFacade CategoryFacade
        {
            get
            {
                return (CategoryFacade)_Context[typeof(CategoryFacade)];
            }
        }
        public ToppingFacade ToppingFacade
        {
            get
            {
                return (ToppingFacade)_Context[typeof(ToppingFacade)];
            }
        }
        public EmployeeFacade EmployeeFacade
        {
            get
            {
                return (EmployeeFacade)_Context[typeof(EmployeeFacade)];
            }
        }
     
        public TechScheduleFacade TechScheduleFacade
        {
            get
            {
                return (TechScheduleFacade)_Context[typeof(TechScheduleFacade)];
            }
        }


        public EquipmentFacade EquipmentFacade
        {
            get
            {
                return (EquipmentFacade)_Context[typeof(EquipmentFacade)];
            }
        }
        public GlobalSettingsFacade GlobalSettingsFacade
        {
            get
            {
                return (GlobalSettingsFacade)_Context[typeof(GlobalSettingsFacade)];
            }
        }
        public MatrixFacade MatrixFacade
        {
            get
            {
                return (MatrixFacade)_Context[typeof(MatrixFacade)];
            }
        }
        public NotesFacade NotesFacade
        {
            get
            {
                return (NotesFacade)_Context[typeof(NotesFacade)];
            }
        }
        public CustomerFileFacade CustomerFileFacade
        {
            get
            {
                return (CustomerFileFacade)_Context[typeof(CustomerFileFacade)];
            }
        }
        public InvoiceFacade InvoiceFacade
        {
            get
            {
                return (InvoiceFacade)_Context[typeof(InvoiceFacade)];
            }
        }
        public CustomerAppoinmentFacade CustomerAppoinmentFacade
        {
            get
            {
                return (CustomerAppoinmentFacade)_Context[typeof(CustomerAppoinmentFacade)];
            }
        }
        public GridSettingsFacade GridSettingsFacade
        {
            get
            {
                return (GridSettingsFacade)_Context[typeof(GridSettingsFacade)];
            }
        }
        public CustomerSnapshotFacade CustomerSnapshotFacade
        {
            get
            {
                return (CustomerSnapshotFacade)_Context[typeof(CustomerSnapshotFacade)];
            }
        }
        public FundFacade FundFacade
        {
            get
            {
                return (FundFacade)_Context[typeof(FundFacade)];
            }
        }
        public CustomerAppoinmentDetailFacade CustomerAppoinmentDetailFacade
        {
            get
            {
                return (CustomerAppoinmentDetailFacade)_Context[typeof(CustomerAppoinmentDetailFacade)];
            }
        }
        public NoteAssignFacade NoteAssignFacade
        {
            get
            {
                return (NoteAssignFacade)_Context[typeof(NoteAssignFacade)];
            }
        }
        public AccountTypeFacade AccountTypeFacade
        {
            get
            {
                return (AccountTypeFacade)_Context[typeof(AccountTypeFacade)];
            }
        }
        public CustomerBillFacade CustomerBillFacade
        {
            get
            {
                return (CustomerBillFacade)_Context[typeof(CustomerBillFacade)];
            }
        }
        public CustomerSystemInfoFacade CustomerSystemInfoFacade
        {
            get
            {
                return (CustomerSystemInfoFacade)_Context[typeof(CustomerSystemInfoFacade)];
            }
        }
        public CustomerApiSettingFacade CustomerApiSettingFacade
        {
            get
            {
                return (CustomerApiSettingFacade)_Context[typeof(CustomerApiSettingFacade)];
            }
        }
        public EmailTextTemplateFacade EmailTextTemplateFacade
        {
            get
            {
                return (EmailTextTemplateFacade)_Context[typeof(EmailTextTemplateFacade)];
            }
        }
        public CustomerViewFacade CustomerViewFacade
        {
            get
            {
                return (CustomerViewFacade)_Context[typeof(CustomerViewFacade)];
            }
        }
        public CustomerSystemNoFacade CustomerSystemNoFacade
        {
            get
            {
                return (CustomerSystemNoFacade)_Context[typeof(CustomerSystemNoFacade)];
            }
        }
        public PanelTypeFacade PanelTypeFacade
        {
            get
            {
                return (PanelTypeFacade)_Context[typeof(PanelTypeFacade)];
            }
        }

        public AccountHolderFacade AccountHolderFacade
        {
            get
            {
                return (AccountHolderFacade)_Context[typeof(AccountHolderFacade)];
            }
        }
        public CredentialSettingFacade CredentialSettingFacade
        {
            get
            {
                return (CredentialSettingFacade)_Context[typeof(CredentialSettingFacade)];
            }
        }

        public CompanyBranchFacade CompanyBranchFacade
        {
            get
            {
                return (CompanyBranchFacade)_Context[typeof(CompanyBranchFacade)];
            }
        }
        public CityTaxFacade CityTaxFacade
        {
            get
            {
                return (CityTaxFacade)_Context[typeof(CityTaxFacade)];
            }
        }
        public ThirdPartySettingFacade ThirdPartySettingFacade
        {
            get
            {
                return (ThirdPartySettingFacade)_Context[typeof(ThirdPartySettingFacade)];
            }
        }
        public HrDocFacade HrDocFacade
        {
            get
            {
                return (HrDocFacade)_Context[typeof(HrDocFacade)];
            }
        }
        public HrFacade HrFacade
        {
            get
            {
                return (HrFacade)_Context[typeof(HrFacade)];
            }
        }
        public EmergencyContactFacade EmergencyContactFacade
        {
            get
            {
                return (EmergencyContactFacade)_Context[typeof(EmergencyContactFacade)];
            }
        }
        public PackageFacade PackageFacade
        {
            get
            {
                return (PackageFacade)_Context[typeof(PackageFacade)];
            }
        }
        public PaymentInfoFacade PaymentInfoFacade
        {
            get
            {
                return (PaymentInfoFacade)_Context[typeof(PaymentInfoFacade)];
            }
        }
        public PaymentInfoCustomerFacade PaymentInfoCustomerFacade
        {
            get
            {
                return (PaymentInfoCustomerFacade)_Context[typeof(PaymentInfoCustomerFacade)];
            }
        }
        public QAQuestionFacade QAQuestionFacade
        {
            get
            {
                return (QAQuestionFacade)_Context[typeof(QAQuestionFacade)];
            }
        }
        public QaAnswerFacade QaAnswerFacade
        {
            get
            {
                return (QaAnswerFacade)_Context[typeof(QaAnswerFacade)];
            }
        }
        public ScheduleFacade ScheduleFacade
        {
            get
            {
                return (ScheduleFacade)_Context[typeof(ScheduleFacade)];
            }
        }
        public AdditionalContactFacade AdditionalContactFacade
        {
            get
            {
                return (AdditionalContactFacade)_Context[typeof(AdditionalContactFacade)];
            }
        }
        public InvoiceNoteFacade InvoiceNoteFacade
        {
            get
            {
                return (InvoiceNoteFacade)_Context[typeof(InvoiceNoteFacade)];
            }
        }
        public TransactionFacade TransactionFacade
        {
            get
            {
                return (TransactionFacade)_Context[typeof(TransactionFacade)];
            }
        }
        public ExpenseFacade ExpenseFacade
        {
            get
            {
                return (ExpenseFacade)_Context[typeof(ExpenseFacade)];
            }
        }
        public BillFacade BillFacade
        {
            get
            {
                return (BillFacade)_Context[typeof(BillFacade)];
            }
        }
        public DashboardFacade DashboardFacade
        {
            get
            {
                return (DashboardFacade)_Context[typeof(DashboardFacade)];
            }
        }
        public UserOrganizationFacade UserOrganizationFacade
        {
            get
            {
                return (UserOrganizationFacade)_Context[typeof(UserOrganizationFacade)];
            }
        }
        public PaymentRevenueFacade PaymentRevenueFacade
        {
            get
            {
                return (PaymentRevenueFacade)_Context[typeof(PaymentRevenueFacade)];
            }
        }
        public CityZipcodeFacade CityZipcodeFacade
        {
            get
            {
                return (CityZipcodeFacade)_Context[typeof(CityZipcodeFacade)];
            }
        }
        public FormGeneratorFacade FormGeneratorFacade
        {
            get
            {
                return (FormGeneratorFacade)_Context[typeof(FormGeneratorFacade)];
            }
        }
        public RecruitFacade RecruitFacade
        {
            get
            {
                return (RecruitFacade)_Context[typeof(RecruitFacade)];
            }
        }
        public CustomerAgreementFacade CustomerAgreementFacade
        {
            get
            {
                return (CustomerAgreementFacade)_Context[typeof(CustomerAgreementFacade)];
            }
        }
        public CommissionFacade CommissionFacade
        {
            get
            {
                return (CommissionFacade)_Context[typeof(CommissionFacade)];
            }
        }
        public AdjustmentFacade AdjustmentFacade
        {
            get
            {
                return (AdjustmentFacade)_Context[typeof(AdjustmentFacade)];
            }
        }
        public AlarmFacade AlarmFacade
        {
            get
            {
                return (AlarmFacade)_Context[typeof(AlarmFacade)];
            }
        }
        public AgreementFacade AgreementFacade
        {
            get
            {
                return (AgreementFacade)_Context[typeof(AgreementFacade)];
            }
        }
        public ShortUrlFacade ShortUrlFacade
        {
            get
            {
                return (ShortUrlFacade)_Context[typeof(ShortUrlFacade)];
            }
        }
        public LeadCorrespondenceFacade LeadCorrespondenceFacade
        {
            get
            {
                return (LeadCorrespondenceFacade)_Context[typeof(LeadCorrespondenceFacade)];
            }
        }
        public SMSFacade SMSFacade
        {
            get
            {
                return (SMSFacade)_Context[typeof(SMSFacade)];
            }
        }
        //[Shariful-30-9-19]
        public FileFacade FileFacade
        {
            get
            {
                return (FileFacade)_Context[typeof(FileFacade)];
            }
        }
        //[~Shariful-30-9-19]
        public SmartPackageFacade SmartPackageFacade
        {
            get
            {
                return (SmartPackageFacade)_Context[typeof(SmartPackageFacade)];
            }
        }
        public ReceivePaymentFacade ReceivePaymentFacade
        {
            get
            {
                return (ReceivePaymentFacade)_Context[typeof(ReceivePaymentFacade)];
            }
        }
        public CompanyFileFacade CompanyFileFacade
        {
            get
            {
                return (CompanyFileFacade)_Context[typeof(CompanyFileFacade)];
            }
        }
        public TicketFacade TicketFacade
        {
            get
            {
                return (TicketFacade)_Context[typeof(TicketFacade)];
            }
        }
        public PurchaseOrderFacade PurchaseOrderFacade
        {
            get
            {
                return (PurchaseOrderFacade)_Context[typeof(PurchaseOrderFacade)];
            }
        }
        public EquipmentFileFacade EquipmentFileFacade
        {
            get
            {
                return (EquipmentFileFacade)_Context[typeof(EquipmentFileFacade)];
            }
        }
        public VehicleFacade VehicleFacade
        {
            get
            {
                return (VehicleFacade)_Context[typeof(VehicleFacade)];
            }
        }
        public EmployeeComputerFacade EmployeeComputerFacade
        {
            get
            {
                return (EmployeeComputerFacade)_Context[typeof(EmployeeComputerFacade)];
            }
        }
        public NotificationFacade NotificationFacade
        {
            get
            {
                return (NotificationFacade)_Context[typeof(NotificationFacade)];
            }
        }
        public EmployeeTimeClockFacade EmployeeTimeClockFacade
        {
            get
            {
                return (EmployeeTimeClockFacade)_Context[typeof(EmployeeTimeClockFacade)];
            }
        }
        public TimeClockFacade TimeClockFacade
        {
            get
            {
                return (TimeClockFacade)_Context[typeof(TimeClockFacade)];
            }
        }
        public PtoFacade PtoFacade
        {
            get
            {
                return (PtoFacade)_Context[typeof(PtoFacade)];
            }
        }
        public EmployeeReviewFacade EmployeeReviewFacade
        {
            get
            {
                return (EmployeeReviewFacade)_Context[typeof(EmployeeReviewFacade)];
            }
        }

        public CustomerDraftFacade CustomerDraftFacade
        {
            get
            {
                return (CustomerDraftFacade)_Context[typeof(CustomerDraftFacade)];
            }
        }
        public DeclinedTransactionsFacade DeclinedTransactionsFacade
        {
            get
            {
                return (DeclinedTransactionsFacade)_Context[typeof(DeclinedTransactionsFacade)];
            }
        }
        public OpportunityFacade OpportunityFacade
        {
            get
            {
                return (OpportunityFacade)_Context[typeof(OpportunityFacade)];
            }
        }
        public ContactFacade ContactFacade
        {
            get
            {
                return (ContactFacade)_Context[typeof(ContactFacade)];
            }
        }
        public ActivityFacade ActivityFacade
        {
            get
            {
                return (ActivityFacade)_Context[typeof(ActivityFacade)];
            }
        }
        public CustomerSignatureFacade CustomerSignatureFacade
        {
            get
            {
                return (CustomerSignatureFacade)_Context[typeof(CustomerSignatureFacade)];
            }
        }
        public CustomerInspectionFacade CustomerInspectionFacade
        {
            get
            {
                return (CustomerInspectionFacade)_Context[typeof(CustomerInspectionFacade)];
            }
        }
        public EstimatorFacade EstimatorFacade
        {
            get
            {
                return (EstimatorFacade)_Context[typeof(EstimatorFacade)];
            }
        }
        public BookingFacade BookingFacade
        {
            get
            {
                return (BookingFacade)_Context[typeof(BookingFacade)];
            }
        }

        #region Digiture

        public ErrorFacade ErrorFacade
        {
            get
            {
                return (ErrorFacade)_Context[typeof(ErrorFacade)];
            }
        }

        #endregion Digiture

        /// <summary>
        ///  IExtension<OperationContext> Members
        /// </summary>
        /// <param name="owner"></param>
        public void Attach(OperationContext owner) { }

        public void Detach(OperationContext owner) { }

        public void Dispose()
        {
            if (_Context != null)
                _Context.Dispose();

            _Context = null;
        }
    }
}
