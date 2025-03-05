using HS.Entities;
using HS.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LocalizeHelper = HS.Web.UI.Helper.LanguageHelper;
namespace HS.Web.UI.Helper
{
    public class AddressHelper
    {

        public static string ShippingAddress { get { return "ShippingAddress"; } }
        public static string BillingAddress { get { return "BillingAddress"; } }

        /// <summary>
        /// AddressType => 1.'ShippingAddress' / 'BillingAddress',2.globset Key ="CustomerAddressPdfFormat"
        /// Address Template is from global settings Key ="CustomerAddressPdfFormat"
        /// </summary>
        public static string MakeCustomerAddress(Customer tempCustomer, string AddressType, string AddressTemplate)
        {
            //customer -> Address = Shipping Address 
            //customer -> street,city,state,zipcode = Shipping

            //customer -> Address2 = Billing Address
            //customer -> streetprev,cityprev,stateprev,zipcodeprev = Billing
            string address = "";
            //string MainAddress = "";
            string NameFormat = "";
            string CustomerStreet = "";
            string CustomerCity = "";
            string CustomerState = "";
            string CustomerZip = "";
            string CustomerAccountNo = "";

            #region Customer Address Format
            if (AddressType == "ShippingAddress")
            {
                //if shipping address not null take shipping address
                if (!string.IsNullOrWhiteSpace(tempCustomer.Address))
                {
                    if (!string.IsNullOrWhiteSpace(tempCustomer.Street))
                    {
                        CustomerStreet = tempCustomer.Street;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.City))
                    {
                        CustomerCity = tempCustomer.City;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.State))
                    {
                        CustomerState = tempCustomer.State;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.ZipCode))
                    {
                        CustomerZip = tempCustomer.ZipCode;
                    }
                }
                //if shipping adderss is null take billing address
                else if (!string.IsNullOrWhiteSpace(tempCustomer.Address2))
                {
                    if (!string.IsNullOrWhiteSpace(tempCustomer.StreetPrevious))
                    {
                        CustomerStreet = tempCustomer.StreetPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.CityPrevious))
                    {
                        CustomerCity = tempCustomer.CityPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.StatePrevious))
                    {
                        CustomerState = tempCustomer.StatePrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.ZipCodePrevious))
                    {
                        CustomerZip = tempCustomer.ZipCodePrevious;
                    }
                }
            }
            else if (AddressType == "BillingAddress" || AddressType == "PickUpLocation" || AddressType == "DropOffLocation")
            {
                //if billing address not null take billing address
                if (!string.IsNullOrWhiteSpace(tempCustomer.Address2))
                {
                    if (!string.IsNullOrWhiteSpace(tempCustomer.StreetPrevious))
                    {
                        CustomerStreet = tempCustomer.StreetPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.CityPrevious))
                    {
                        CustomerCity = tempCustomer.CityPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.StatePrevious))
                    {
                        CustomerState = tempCustomer.StatePrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.ZipCodePrevious))
                    {
                        CustomerZip = tempCustomer.ZipCodePrevious;
                    }
                }
                //if billing adderss is null take shipping address
                else if (!string.IsNullOrWhiteSpace(tempCustomer.Address))
                {
                    if (!string.IsNullOrWhiteSpace(tempCustomer.Street))
                    {
                        CustomerStreet = tempCustomer.Street;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.City))
                    {
                        CustomerCity = tempCustomer.City;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.State))
                    {
                        CustomerState = tempCustomer.State;
                    }
                    if (!string.IsNullOrWhiteSpace(tempCustomer.ZipCode))
                    {
                        CustomerZip = tempCustomer.ZipCode;
                    }
                }
            }
            #endregion

            #region Customer name format
            //if customer has firstname and last name
            //if (!string.IsNullOrWhiteSpace(tempCustomer.FirstName) && !string.IsNullOrWhiteSpace(tempCustomer.FirstName))
            //{
            //    NameFormat = tempCustomer.FirstName.UppercaseFirst() + " "
            //        + tempCustomer.LastName.UppercaseFirst();
            //}
            ////if customer has business name
            //if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
            //{
            //    NameFormat += "<br />" + tempCustomer.BusinessName;
            //}
            if(AddressType == "BillingAddress" || AddressType == "PickUpLocation" || AddressType == "DropOffLocation")
            {
                if (string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    tempCustomer.DisplayName = tempCustomer.FirstName + " " + tempCustomer.LastName;
                }
                else
                {
                    tempCustomer.DisplayName = tempCustomer.BusinessName;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tempCustomer.DisplayName))
                {
                    if (string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                    {
                        tempCustomer.DisplayName = tempCustomer.FirstName + " " + tempCustomer.LastName;
                    }
                    else
                    {
                        tempCustomer.DisplayName = tempCustomer.BusinessName;
                    }
                }
            }
          
            NameFormat = tempCustomer.DisplayName;
            #endregion
            if (!string.IsNullOrWhiteSpace(tempCustomer.CustomerNo))
            {
                CustomerAccountNo = tempCustomer.CustomerNo;
            }
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("CustomerName", NameFormat);
            datatemplate.Add("CustomerStreet", CustomerStreet);
            datatemplate.Add("CustomerCity", CustomerCity);
            datatemplate.Add("CustomerState", CustomerState);
            datatemplate.Add("CustomerZip", CustomerZip);
            datatemplate.Add("CustomerAccountNo", CustomerAccountNo);
            if (string.IsNullOrEmpty(CustomerCity) && string.IsNullOrEmpty(CustomerState) && string.IsNullOrEmpty(CustomerZip))
            {
                address = NameFormat;
            }
            else
            {
                address = LabelHelper.ParserHelper(AddressTemplate, datatemplate);
            }
            return address;
        }

        public static string MakeAddress(Customer cus)
        {
            string address = "";
            address += cus.Street;
            if (!string.IsNullOrEmpty(cus.Appartment))
            {
                address += string.Format(" #{0}", cus.Appartment);
            }
            if (!string.IsNullOrEmpty(cus.City))
            {
                address += string.Format(" {0}", cus.City.CapitalizeFirst());
            }
            if (!string.IsNullOrEmpty(address) && (!string.IsNullOrEmpty(cus.State) || !string.IsNullOrEmpty(cus.ZipCode)))
            {
                address += ", ";
            }
            address += string.Format("{0} {1}", cus.State, cus.ZipCode);
            return address;
        }
        public static string MakeAddressForPickUporDropof(CustomerAddress cus)
        {
            string address = "";
            if(cus != null)
            {
                if (!string.IsNullOrEmpty(cus.Street))
                {
                    address += cus.Street;
                }


                if (!string.IsNullOrEmpty(cus.City))
                {
                    address += string.Format(" {0}", cus.City.CapitalizeFirst());
                }
                if (!string.IsNullOrEmpty(address) && (!string.IsNullOrEmpty(cus.State) || !string.IsNullOrEmpty(cus.ZipCode)))
                {
                    address += ", ";
                }
                address += string.Format("{0} {1}", cus.State, cus.ZipCode);
            }
           
            return address;
        }
        public static string MakeCustomerAddressForList(Customer cus, List<GridSetting> grids)
        {
            string address = "";
            foreach (var grd in grids)
            {

                if (!string.IsNullOrWhiteSpace(cus.Street) && grd.SelectedColumn == LocalizeHelper.T("Street"))
                {
                    if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && grd.SelectedColumn.ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment) && cus.StreetType != "-1")
                    {

                        address += cus.Street + " " + cus.StreetType + " " + cus.Appartment + " <br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && cus.StreetType != "-1")
                    {

                        address += cus.Street + " " + cus.StreetType + "<br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment))
                    {

                        address += cus.Street + " " + cus.Appartment + "<br>";
                    }
                    else
                    {
                        address += cus.Street + "<br>";
                    }
                }
                else
                {
                    if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && cus.StreetType != "-1")
                    {
                        address += cus.StreetType + "<br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment))
                    {
                        address += cus.Appartment + "<br>";
                    }
                }
                if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("City") && !string.IsNullOrWhiteSpace(cus.City))
                {
                    address += cus.City + ", ";
                }
                else if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("State") && !string.IsNullOrWhiteSpace(cus.State))
                {
                    address += cus.State+" ";
                }
                else if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("ZipCode") && !string.IsNullOrWhiteSpace(cus.ZipCode))
                {
                    address += cus.ZipCode+" ";
                }
            }

            return address.TrimEnd(',');
        }
        public static string MakeCustomerAddress2ForList(Customer cus, List<GridSetting> grids)
        {
            string address = "";
            foreach (var grd in grids)
            {

                if (!string.IsNullOrWhiteSpace(cus.StreetPrevious) && grd.SelectedColumn == LocalizeHelper.T("StreetPrevious"))
                {
                    if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && grd.SelectedColumn.ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment) && cus.StreetType != "-1")
                    {

                        address += cus.StreetPrevious + " " + cus.StreetType + " " + cus.Appartment + " <br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && cus.StreetType != "-1")
                    {

                        address += cus.StreetPrevious + " " + cus.StreetType + "<br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment))
                    {

                        address += cus.StreetPrevious + " " + cus.Appartment + "<br>";
                    }
                    else
                    {
                        address += cus.StreetPrevious + "<br>";
                    }
                }
                else
                {
                    if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("streettype") && !string.IsNullOrWhiteSpace(cus.StreetType) && cus.StreetType != "-1")
                    {
                        address += cus.StreetType + "<br>";
                    }
                    else if (LocalizeHelper.T(grd.SelectedColumn).ToLower() == LocalizeHelper.T("appartment") && !string.IsNullOrWhiteSpace(cus.Appartment))
                    {
                        address += cus.Appartment + "<br>";
                    }
                }
                if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("CityPrevious") && !string.IsNullOrWhiteSpace(cus.CityPrevious))
                {
                    address += cus.CityPrevious + ", ";
                }
                else if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("StatePrevious") && !string.IsNullOrWhiteSpace(cus.StatePrevious))
                {
                    address += cus.StatePrevious + " ";
                }
                else if (LocalizeHelper.T(grd.SelectedColumn) == LocalizeHelper.T("ZipCodePrevious") && !string.IsNullOrWhiteSpace(cus.ZipCodePrevious))
                {
                    address += cus.ZipCodePrevious + " ";
                }
            }

            return address.TrimEnd(',');
        }
        public static string MakeInstallAddress(Customer cus)
        {
            string installAddress = "";
            installAddress += cus.Street;
            if (!string.IsNullOrEmpty(cus.Appartment))
            {
                installAddress += string.Format(" #{0}", cus.Appartment);
            }
            return installAddress;
        }

        public static string MakeCompanyAddress(Company TempCompany, string CompanyAddressTemplate)
        {
            string CompanyAddress = "";
            string CompanyName = "";
            string Address = "";
            string Street = "";
            string City = "";
            string State = "";
            string Zip = "";
            string CompanyPhone = "";
            string EmailAddress = "";
            string WebAddress = "";
            if (!string.IsNullOrWhiteSpace(TempCompany.CompanyName))
            {
                CompanyName = TempCompany.CompanyName;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.Address))
            {
                Address = TempCompany.Address;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.Street))
            {
                Street = TempCompany.Street;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.City))
            {
                City = TempCompany.City;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.State))
            {
                State = TempCompany.State;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.ZipCode))
            {
                Zip = TempCompany.ZipCode;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.Phone))
            {
                CompanyPhone = TempCompany.Phone;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.EmailAdress))
            {
                EmailAddress = TempCompany.EmailAdress;
            }
            if (!string.IsNullOrWhiteSpace(TempCompany.Website))
            {
                WebAddress = TempCompany.Website;
            }
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("ComapnyName", CompanyName);
            datatemplate.Add("Address", Address);
            datatemplate.Add("Street", Street);
            datatemplate.Add("City", City);
            datatemplate.Add("State", State);
            datatemplate.Add("Zip", Zip);
            datatemplate.Add("CompanyPhone", CompanyPhone);
            datatemplate.Add("EmailAddress", EmailAddress);
            datatemplate.Add("WebAddress", WebAddress);
            if (string.IsNullOrEmpty(City) && string.IsNullOrEmpty(State) && string.IsNullOrEmpty(Zip))
            {
                CompanyAddress = CompanyName;
            }
            else
            {
                CompanyAddress = LabelHelper.ParserHelper(CompanyAddressTemplate, datatemplate);
            }
            return CompanyAddress;
        }
    }
}