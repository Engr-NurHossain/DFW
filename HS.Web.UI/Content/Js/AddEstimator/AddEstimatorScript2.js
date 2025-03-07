var TotalProfit = 0;
var TotalOverHead = 0;   
var TotalCost = 0;
var TotalPrice = 0;
var TotalTaxablePrice = 0;
var TotalPriceWithTax = 0;

var TaxPercnetage = 0;
var TaxAmount = 0;  

var PoriftPercentage = 0; 
var OverheadCostPercentage = 0;


var TotalServicePrice = 0;
var TotalTaxablePriceService = 0; 
var ServiceTaxAmount = 0;
var TotalServicePriceWithTax = 0;

var TotalOneTimeServicePrice = 0;
var TotalTaxablePriceOneTimeService = 0;
var OneTimeServiceTaxAmount = 0;
var TotalOneTimeServicePriceWithTax = 0;


var CompletionDateDatepicker;
var StartDateDatepicker;
var ManufactParentDom;
//Confiremd



String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}
var PropertyUserSuggestiontemplate =
            '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-supplierCost="{8}" data-sku="{9}" data-eqTypeId="{10}" data-supplierId="{11}" data-unit="{12}" data-overhead="{13}" data-profitrate="{14}" data-description="{6}" data-manufacturerId="{15}">'
                //+ "<img src='{7}' class='EquipmentImage'>"
                + "<p class='tt-sug-text'>"
                    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
                    + "<em class='tt-eq-price'>${2}</em>"
                    if (LineItemEdit == "True") {
                        PropertyUserSuggestiontemplate += '<button class="editEquipmentsug" onclick="AddNewEqp(\'{0}\')"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>'
                    }
                    PropertyUserSuggestiontemplate += '<em class="tt-eq-sku"><br>SKU: {9}</em>'
                    + "<br />"
                    + "</p> "
                
                    + "</div>";

var ServiceTemplate =
    '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}" data-quantityonhand="{7}" data-suppliercost="{8}" data-retail="{9}" data-sku="{11}" data-point="{12}" data-equipvendorcost="{13}" data-isTaxable="{14}" >'
    //+ "<img src='{7}' class='EquipmentImage'>"
    + "<p class='tt-sug-text'>"
    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{10}</em>"
    + "<em class='tt-eq-price'>${2}</em>"
    + "<br />"
    + "</p> "
    + "</div>";
 
var NewEquipmentRowBase = "<tr>"
    + "<td valign='top' class='rowindex'></td>"
    + "<td valign='middle'>"
    + "<input type = 'text' class='txtPartDescription' onkeydown = 'SearchKeyDown(this,event)'  onkeyup='SearchKeyUp(this, event)' placeholder='Part Description'/>"
    //+ "<input type = 'text' class='txtPartNumber' onkeydown = 'SearchKeyDown(this,event)' onkeyup ='SearchKeyUp(this,event)' placeholder='Part Number'/>"
    + "<div class='tt-menu'>"
    + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
    + "</div>"
    + "<span class='spnPartDescription'>" + PartDescriptionText + "</span>" 
    if(LineItemEdit == "True") {
        NewEquipmentRowBase += "<button class='editEquipment'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>"
    } 
    NewEquipmentRowBase += "<span class='spnsku' title='SKU'></span>"
        //+ "<span class='spnPartNumber'>" + PartNumberText + "</span>"
        //+ "<select class='selManufacturer form-control'>{3}</select>"
        //+ "<select class='form-control selPartNumber' title='Part Number'><option value='-1'>Part Number</option></select>" 
    + "</td>"
    + "<td valign='middle'>"
        + "<select class='selSupplier form-control'>{1}</select>"
        + "<select class='selCategory form-control'>{0}</select>" 
    + "</td>"
    + "<td valign='middle'>"
        + "<select class='selUnit form-control'>{2}</select>"
        //+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtUnit'placeholder='Unit' /> "
        + "<input type = 'text' onkeydown = 'OthersKeyDown(this,event)' class='txtQunatity'placeholder = 'Quantity' /> " 
       // + "<span class='spnUnit'></span>"
        + "<span class='spnQunatity'></span>"
    + "</td>"
    + "<td valign='middle'>"
        + "<div class='C_S I_G'>"
        +   "<div class='input-group-prepend'>"
        +       "<div class='input-group-text'>" + Currency + "</div>"
        +   "</div>"
        +   "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtUnitCost'placeholder=' Unit Cost'/>"
        +   "<div class='input-group-prepend'>"
        +       "<div class='input-group-text'>" + Currency + "</div>"
        +   "</div>"
        +   "<input type = 'text' onkeydown = 'OthersKeyDown(this,event)' class='txtTotalCost' placeholder = '  Total Cost'/>"


        +   "<div class='input-group-prepend hidden'>"
        +       "<div class='input-group-text'>" + Currency + "</div>"
        +   "</div>"
        +   "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProfit hidden'placeholder='  Profit' />"
        +   "<div class='input-group-prepend hidden'>"
        +       "<div class='input-group-text'>" + Currency + "</div>"
        +   "</div>"
        +   "<input type ='text'onkeydown ='OthersKeyDown(this,event)'class='txtOverhead hidden'placeholder = '  Overhead' /> "
        +"</div>"



        + "<span title='Unit Cost' class='spnUnitCost'></span>"
        + "<span title='Total Cost' class='spnTotalCost'></span>"
        + "<span title='Profit' class='spnProfit hidden'></span>"
        + "<span title='Overhead' class='spnOverhead hidden'></span>"
    + "</td>"
    + "<td valign='top'>"
        +"<div class='C_S I_G'>"
        
        //+    "<div class='input-group-prepend'>"
        //+       "<div class='input-group-text'>" + Currency + "</div>"
        //+    "</div>"
        //+    " <input type = 'text' onkeydown = 'OthersKeyDown(this,event)' class='txtTotalCost' placeholder = '  Total Cost'/>"

        +   '<div class="input-group-prepend">'
        +       '<div class="input-group-text">%</div>'
        +   '</div>'
        +   '<input type="text" onkeydown="OthersKeyDown(this,event)" title="Profit Rate" class="txtProfitRate" value="" placeholder="Profit Rate">'
        +   '<div class="input-group-prepend">'
        +       '<div class="input-group-text">%</div>'
        +   '</div>'
        +   '<input type="text" onkeydown="OthersKeyDown(this,event)" title="Markup/Overhead Rate" class="txtOverheadRate" value="" placeholder="Markup/Overhead Rate">'

        +    "<div class='input-group-prepend'>"
        +       "<div class='input-group-text'>" + Currency + "</div>"
        +    "</div>"
        +    "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtTotalPrice' placeholder='  Total Price' />"
        +"</div>"
        //+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
        
        + '<span class="spnProfitRate" title="Profit Rate"></span> <span class="spnOverheadRate" title="Markup/Overhead Rate"></span>'
        + "<span class='spnTotalPrice'></span>"
    + "</td>" 
        if (Taxcheckbox == "True") {
            NewEquipmentRowBase += "<td valign='top' class='tableActions'>"
                + "<input type='checkbox' title='Is Taxable?' checked class='chkTaxable' style='display:block!important;' />"
                + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                + "</td>"
        }
        else {
            NewEquipmentRowBase+= "<td valign='top' class='tableActions'>"
                + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                + "</td>"
        }
           
+ "</tr>";

var NewServiceRow =  
    "<tr class='IsService'>" +
        "<td valign='middle'></td>" +
        "<td valign='top' class='tick_product_name'>" +
            '<input type="text" class="ProductName" onkeydown="SearchKeyDownService(this,event)" onkeyup="SearchKeyUpService(this,event,\'Service\')">'+
            "<div class='tt-menu'><div class='tt-dataset tt-dataset-autocomplete'></div></div>"+
            "<span class='spnProductName'></span>"+
        "</td>" + 
        '<td valign="top">' +
            '<input type="number" onkeydown="OthersKeyDown(this, event)" value="" class="txtUnitPrice" />' +
            '<span class="spnUnitPrice"></span>' +
        '</td>' +
        '<td valign="top">'+
            '<input type="text" onkeydown="OthersKeyDown(this, event)" value="" class="txtProductQuantity" />' +
            '<span class="spnProductQuantity"></span>'+
        '</td>'+ 

        "<td valign='top'>" +
            "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductAmount'>"+
            "<span class='spnProductAmount'></span>" +
       "</td>"
        if (Taxcheckbox == "True") {
            NewServiceRow+= "<td valign='top' class='tableActions'>"
                + "<input type='checkbox' title='Is Taxable?' class='chkTaxable' style='display:block!important;'>"
                + "<i class='fa fa-trash-o trash-hidden' aria-hidden='true' title='Delete'></i>"
            "</td>"
        }
        else {
            NewServiceRow += "<td valign='top' class='tableActions'>"
            +"<i class='fa fa-trash-o trash-hidden' aria-hidden='true' title='Delete'></i>"
            +"</td>"
        } 
"</tr>";

var NewOneTimeServiceRow =
    "<tr class='IsService'>" +
    "<td valign='middle'></td>" +
    "<td valign='top' class='tick_product_name'>" +
    '<input type="text" class="ProductName" onkeydown="SearchKeyDownService(this,event)" onkeyup="SearchKeyUpService(this,event,\'OneTimeService\')">' +
    "<div class='tt-menu'><div class='tt-dataset tt-dataset-autocomplete'></div></div>" +
    "<span class='spnProductName'></span>" +
    "</td>" +
    '<td valign="top">' +
    '<input type="number" onkeydown="OthersKeyDown(this, event)" value="" class="txtUnitPrice" />' +
    '<span class="spnUnitPrice"></span>' +
    '</td>' +
    '<td valign="top">' +
    '<input type="text" onkeydown="OthersKeyDown(this, event)" value="" class="txtProductQuantity" />' +
    '<span class="spnProductQuantity"></span>' +
    '</td>' +

    "<td valign='top'>" +
    "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductAmount'>" +
    "<span class='spnProductAmount'></span>" +
    "</td>"
if (Taxcheckbox == "True") {
    NewOneTimeServiceRow += "<td valign='top' class='tableActions'>"
        + "<input type='checkbox' title='Is Taxable?' class='chkTaxable' style='display:block!important;'>"
        + "<i class='fa fa-trash-o trash-hidden' aria-hidden='true' title='Delete'></i>"
    "</td>"
}
else {
    NewOneTimeServiceRow += "<td valign='top' class='tableActions'>"
        + "<i class='fa fa-trash-o trash-hidden' aria-hidden='true' title='Delete'></i>"
        + "</td>"
}
"</tr>";

 
var OpenClosingConfirmationMessage = function () {
    if (IsChanged) {
        OpenConfirmationMessageNew("Confirmation", "Do you want to leave? Changes you made may not be saved.", function () {
            CloseTopToBottomModal();
        });
    } else {
        CloseTopToBottomModal();
    }
}
var AddNewEqp = function (equipId) {
    $(".tt-menu").hide();
    OpenRightToLeftLgModal("/Inventory/AddEquepment/?OpenFromModal=true&EquipmentId=" + equipId);
}
var EditEqp = function (equipId, flag) {
    $(".tt-menu").hide();
    OpenRightToLeftLgModal("/Inventory/AddEquepment/?OpenFromModal=true&EquipmentId=" + equipId + "&Flag=" + flag);
}
var AddNewSrvc = function () {
    $(".tt-menu").hide();
    OpenRightToLeftLgModal("/Inventory/AddService?OpenFromModal=true");
}
var LoadSupplierDropdownAfterAdding = function (SupplierId, SupplierName) {
    $(".selSupplier").append(String.format("<option value='{0}' >{1}</option>", SupplierId, SupplierName));
    $("#SupplierDropdown").append(String.format("<option value='{0}' >{1}</option>", SupplierId, SupplierName));

    SetNewEquipmentRow();
}
var DeleteEstimator = function () {
    var param = JSON.stringify({
        EstimatorId: EstimatorId,  
    });
    $.ajax({
        url: domainurl + "/Estimator/DeleteEstimator",
        data: {
            EstimatorId: EstimatorId,  
        },
        //data: param,

        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Estimate deleted successfully!");
                OpenEstimatorTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });

}
var LoadManufacturerDropdownAfterAdding = function (ManufacturerID, ManufacturerName) { 
    SetNewEquipmentRow();
}
var SaveAndNew = function () { 
    SaveEstimate(false, false, "others"); 
    if ($(".HasItem").length != 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?customerid=" + customerId);
    }
}
var SaveAndClose = function () { 
    SaveEstimate(true, false, "others"); 
}
var SaveAndShare = function () { 
    SaveEstimate(false, true);
    CloseTopToBottomModal();
    OpenSuccessMessageNew("Success!", "Invoice Successfully Saved and Sent to Customer.", function () { OpenEstimateTab() });
}


var SaveAndSend = function () { 
    SaveEstimate(false, true, "preview");
    OpenEstimateTab(); 
}

var AddCoverLetter = function(id,EstimatorType){
    //$("#mceu_13").attr("style","display:block !important");
    //$("#mceu_45").attr("style","display:block !important");
    OpenRightToLeftLgModal(domainurl + "/Estimator/AddCoverLetter/?Estimatorid=" + id + "&EstimatorType=" + EstimatorType);
    
}
var AddEndPageCoverLetter = function (id, EstimatorType) { 
    OpenRightToLeftLgModal(domainurl + "/Estimator/AddCoverLetter/?Estimatorid=" + id + "&EstimatorType=" + EstimatorType);
}
var CreatePO = function(PO){
    if(PO == "True"){
        OpenConfirmationMessageNew("Confirmation", "Do You Want To Create POs For Assigned Supplier?", function () {
            SaveEstimate('True');
        });
    } else {
        //CloseTopToBottomModal();
    }
}

var EstimatorEqSuggestionclickbind = function (item) {
    console.log("Product item");
    $('.CustomerEstimateTab .tt-suggestion').click(function () { 
        console.log("after click");
        if (EditCost == "False") {
            $(".txtUnitCost").prop("disabled", true);
            $(".txtTotalCost").prop("disabled", true);
        } else {
            $(".txtUnitCost").prop("disabled", false);
            $(".txtTotalCost").prop("disabled", false);
        }

        if (EditPrice == "False") {
            $(".txtProfitRate").prop("disabled", true);
            $(".txtOverheadRate").prop("disabled", true);
            $(".txtTotalPrice").prop("disabled", true);
        } else {
            $(".txtProfitRate").prop("disabled", false);
            $(".txtOverheadRate").prop("disabled", false);
            $(".txtTotalPrice").prop("disabled", false);
        }

        var clickitem = this;
        $('.CustomerEstimateTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));


        $(item).attr('data-overheadrate', $(clickitem).attr('data-overhead'));
        $(item).attr('data-profitrate', $(clickitem).attr('data-profitrate'));

        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        $(item).parent().parent().attr('data-overheadrate', $(clickitem).attr('data-overhead'));
        $(item).parent().parent().attr('data-profitrate', $(clickitem).attr('data-profitrate'));

        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text(Currency + parseFloat($(this).attr('data-price')).toFixed(2));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val(parseFloat($(this).attr('data-price')).toFixed(2));

        /*Item Overhead Rate Set*/
        var spnOverheadRate = $(item).parent().parent().find('.spnOverheadRate');
        $(spnOverheadRate).text($(this).attr('data-overhead') + '%');
        var txtOverheadRate = $(item).parent().parent().find('.txtOverheadRate');
        $(txtOverheadRate).val($(this).attr('data-overhead'));

        /*Item Profit Rate Set*/
        var spnProfitRate = $(item).parent().parent().find('.spnProfitRate');
        $(spnProfitRate).text($(this).attr('data-profitrate') + '%');
        var txtProfitRate = $(item).parent().parent().find('.txtProfitRate');
        $(txtProfitRate).val($(this).attr('data-profitrate'));

        /*Item Description Set*/
        //var spnItemRate = $(item).parent().parent().find('.spnProductDesc');
        //$(spnItemRate).text($(this).attr('data-description'));
        //var txtItemRate = $(item).parent().parent().find('.txtProductDesc');
        //$(txtItemRate).val($(this).attr('data-description'));
        /*Item Quantity Set*/
        var spnQunatity = $(item).parent().parent().find('.spnQunatity');
        $(spnQunatity).text(1);
        var txtQunatity = $(item).parent().parent().find('.txtQunatity');
        $(txtQunatity).val(1); 

        var spnItemRetail = $(item).parent().parent().find('.spnRetailPrice');
        $(spnItemRetail).text(Currency + $(this).attr('data-supplierCost'));
        var txtItemRatail = $(item).parent().parent().find('.txtRetailPrice');
        $(txtItemRatail).val($(this).attr('data-supplierCost'));
    
        //var spnItemTotalRetail = $(item).parent().parent().find('.spnTotalRetalPrice');
        //$(spnItemTotalRetail).text(Currency + $(this).attr('data-supplierCost'));
        //var txtItemTotalRatail = $(item).parent().parent().find('.txtTotalRetailPrice');
        //$(txtItemTotalRatail).val($(this).attr('data-supplierCost'));
     

        /*Item Amount Set*/
        var spnUnitCost = $(item).parent().parent().find('.spnUnitCost');
        $(spnUnitCost).text(Currency + $(this).attr('data-supplierCost'));
        var txtUnitCost = $(item).parent().parent().find('.txtUnitCost');
        $(txtUnitCost).val($(this).attr('data-supplierCost'));

        /*Total Cost */
        var spnTotalCost = $(item).parent().parent().find('.spnTotalCost');
        $(spnTotalCost).text(Currency + $(this).attr('data-supplierCost'));
        var txtTotalCost = $(item).parent().parent().find('.txtTotalCost');
        $(txtTotalCost).val($(this).attr('data-supplierCost'));

        /*
        *TOTAL AMOUNT CALCULATION
        * */

        var OverheadRate = parseFloat($(clickitem).attr('data-overhead'));
        var ProfitRate = parseFloat($(clickitem).attr('data-profitrate'));
        var SupplierCost = parseFloat($(clickitem).attr('data-supplierCost'));

        var OverHead = (SupplierCost * OverheadRate / 100);
        var Profit = ((SupplierCost + OverHead) * ProfitRate / 100); //#ProfitCC

        var TotalPrice = SupplierCost + OverHead + Profit;
        /**
         * Total Price
         * */
        var spnTotalPrice = $(item).parent().parent().find('.spnTotalPrice');
        $(spnTotalPrice).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtTotalPrice = $(item).parent().parent().find('.txtTotalPrice');
        $(txtTotalPrice).val(TotalPrice.toFixed(2));
        
        /**
         * Profit
         * */
        var spnProfit = $(item).parent().parent().find('.spnProfit');
        $(spnProfit).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtProfit = $(item).parent().parent().find('.txtProfit');
        $(txtProfit).val(Profit.toFixed(2));

        /**
         * OverHead
         * */
        var spnOverhead = $(item).parent().parent().find('.spnOverhead');
        $(spnOverhead).text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtOverhead = $(item).parent().parent().find('.txtOverhead');
        $(txtOverhead).val(OverHead.toFixed(2));



        /*SKU*/
        var spnPartNumber = $(item).parent().parent().find('.spnPartNumber');
        $(spnPartNumber).text($(this).attr('data-sku'));

        var spnsku = $(item).parent().parent().find('.spnsku');
        $(spnsku).text($(this).attr('data-sku'));

        var txtPartNumber = $(item).parent().parent().find('.txtPartNumber');
        $(txtPartNumber).val($(this).attr('data-sku'));

        /*EquipmentType*/  
        var selCategory = $(item).parent().parent().find('.selCategory');
        $(selCategory).val($(this).attr('data-eqTypeId') == '0' ? '-1' : $(this).attr('data-eqTypeId'));
        if ($(selCategory).find("option:selected").text() != "Labor") {
            var chkTax = $(item).parent().parent().find('.chkTaxable');
            $(chkTax).prop('checked', false);
        }

        /*Supplier*/
        var selSupplier = $(item).parent().parent().find('.selSupplier');
        $(selSupplier).val($(this).attr('data-supplierId') == '0' ? '00000000-0000-0000-0000-000000000000' : $(this).attr('data-supplierId'));

        /*Supplier*/
        //var selManufacturer = $(item).parent().parent().find('.selManufacturer ');
        //$(selManufacturer).val($(this).attr('data-manufacturerId') == '0' ? '00000000-0000-0000-0000-000000000000' : $(this).attr('data-manufacturerId')); 

        /*Unit*/
        var selSupplier = $(item).parent().parent().find('.selUnit');
        $(selSupplier).val($(this).attr('data-unit') == '' ? '-1' : $(this).attr('data-unit')); 


        CalculateNewAmount();
        var EquipmentId = $(this).attr("data-id");
        ManufactParentDom = $(this).parent().parent().parent().parent();
        GetEquipmentManufacturerList(EquipmentId);




        setTimeout(function () {
            $(".MaterialMarkup").change();
            $(".LaborProfit").change();
            $(".LaborOverhead").change();

        },100);
    });
    $('.CustomerEstimateTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13 || event.keyCode == 9)
        return false;

    var SupplierId = $(item).parent().parent().find('.selSupplier').val();
    var CategoryId = $(item).parent().parent().find('.selCategory').val();
    $.ajax({
        url: domainurl + "/Estimator/GetEquipmentListByKey",
        data: {
            key: $(item).val(),
            SupplierId: SupplierId,
            CategoryId: CategoryId,
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) { 
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'> <div class='AddNewEqp' onclick='AddNewEqp()'>Add New Equipment</div>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType,
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/ '', //resultparse[i].ManufacturerName.replaceAll('"', '\'\''),/* ImageSource*/
                         /*8*/resultparse[i].SupplierCost.toFixed(3),
                         /*9*/ resultparse[i].SKU,
                         /*10*/ resultparse[i].EquipmentTypeId,
                         /*11*/(resultparse[i].SupplierId == null ? '00000000-0000-0000-0000-000000000000' : resultparse[i].SupplierId),
                         /*12*/ resultparse[i].Unit,
                         /*13*/ resultparse[i].OverheadRate,
                         /*14*/ resultparse[i].ProfitRate,
                         /*15*/(resultparse[i].ManufacturerId == null ? '00000000-0000-0000-0000-000000000000' : resultparse[i].ManufacturerId)

                    );
                }

                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                EstimatorEqSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }

            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            $($(trselected).next('tr')).find('input.txtPartDescription').focus();
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');

        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }

            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.txtPartDescription').focus();
        }
    }
}
var OthersKeyDown = function (item, event) {
    if (event.keyCode == 40) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).next('tr')).addClass('focusedItem');
      //  if ($(event.target).hasClass('PartDesc')) {
       //     $($(trselected).next('tr')).find('input.PartDesc').focus();
        //if ($(event.target).hasClass('txtProductDesc')) {
          // $($(trselected).next('tr')).find('input.txtProductDesc').focus();
        //} else
        if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).next('tr')).find('input.txtProductQuantity').focus();
        }
        /*
         * No longer in use
        //else if ($(event.target).hasClass('txtRetailPrice')) {
        //    $($(trselected).next('tr')).find('input.txtRetailPrice').focus();
        //}
        //else if ($(event.target).hasClass('txtTotalRetailPrice')) {
        //    $($(trselected).next('tr')).find('input.txtTotalRetailPrice').focus();
        //}
        */
        else if ($(event.target).hasClass('txtProductRate')) {
            $($(trselected).next('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).next('tr')).find('input.txtProductAmount').focus();
        }
    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');

        //if ($(event.target).hasClass('PartDesc')) {
        //   $($(trselected).prev('tr')).find('input.PartDesc').focus();
        //if ($(event.target).hasClass('txtProductDesc')) {
        //  $($(trselected).prev('tr')).find('input.txtProductDesc').focus();
        //} else

        if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).prev('tr')).find('input.txtProductQuantity').focus();
        }
        /*
         * No longer in use
        //else if ($(event.target).hasClass('txtRetailPrice')) {
        //    $($(trselected).prev('tr')).find('input.txtRetailPrice').focus();
        //}
        //else if ($(event.target).hasClass('txtTotalRetailPrice')) {
        //    $($(trselected).prev('tr')).find('input.txtTotalRetailPrice').focus();
        //}
        */

        else if ($(event.target).hasClass('txtProductRate')) {
            $($(trselected).prev('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).prev('tr')).find('input.txtProductAmount').focus();
        }
    }
    else if (event.keyCode == 9 && $(event.target).hasClass('txtProductAmount')) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        var trfocuseditem = $(trselected).next('tr');
        $(trfocuseditem).addClass('focusedItem');
        $($(trfocuseditem).find('input.ProductName')).focus();
        event.preventDefault();
    }

}
var OneTimeServiceEqSuggestionclickbind = function (item) {
    console.log("One Time service click bind");
    $('#CustomerOneTimeServiceTable .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CustomerOneTimeServiceInvoiceTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));

        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());


        var AddedString = "";
        if (CurrentUserFullName != "") {
            AddedString += "Added By: " + CurrentUserFullName;
        }
        if ($(clickitem).attr('data-sku') != "") {
            AddedString += ", SKU: " + $(clickitem).attr('data-sku');
        }
        if (AddedString != "") {
            $(itemName).attr('title', AddedString);
        }

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnUnitPrice');
        var txtItemRate = $(item).parent().parent().find('.txtUnitPrice');
        var spnProductRateValue = $(this).attr('data-suppliercost');
        $(spnItemRate).text(TransCurrency + parseFloat(spnProductRateValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(txtItemRate).val(parseFloat(spnProductRateValue).toFixed(2));

        /*Item Description Set*/
        var spnItemDesc = $(item).parent().parent().find('.spnProductDesc');
        $(spnItemDesc).text($(this).attr('data-description'));
        var txtItemDesc = $(item).parent().parent().find('.txtProductDesc');
        $(txtItemDesc).val($(this).attr('data-description'));

         
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(1);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemRate).val(1); 
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        var spnProductAmountValue = $(this).attr('data-retail');

        var spnItemVendorCost = $(item).parent().parent().find('.spnProductEquipmentvendorcost');
        var spnProductVndorcostValue = $(this).attr('data-equipvendorcost');
        $(spnItemVendorCost).attr("data-equipvendorcost", spnProductVndorcostValue);
        $(spnItemVendorCost).text(TransCurrency + parseFloat(spnProductVndorcostValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemVendorCost).val(spnProductVndorcostValue);

        $(spnItemRate).text(TransCurrency + parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val(spnProductAmountValue);
        var txtUnitRate = $(item).parent().parent().find('.txtProductUnitPrice');
        $(txtUnitRate).val(spnProductAmountValue);
        var spnItemProductRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemProductRate).text(TransCurrency + parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemRate).attr("data-unitprice", spnProductAmountValue);

        var isTaxable = $(this).attr('data-isTaxable');
        var chkTax = $(item).parent().parent().find('.chkTaxable');
        if (isTaxable == 'true') {
            $(chkTax).prop('checked', true);
        } else {
            $(chkTax).prop('checked', false);
        }



        /*checkbox style*/
        var chkItemCheck = $(item).parent().parent().find('.chkQuantityValid');
        $(chkItemCheck).addClass("block-important");
        CalculateNewAmount();
        $(".focusedItem.HasItem").removeClass('focusedItem');
    }); 
    $('.CustomerOneTimeServiceInvoiceTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var InvoiceEqSuggestionclickbind = function (item) {
    $('#CustomerServiceTable .tt-suggestion').click(function () {  
        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));

        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());


        var AddedString = "";
        if (CurrentUserFullName != "") {
            AddedString += "Added By: " + CurrentUserFullName;
        }
        if ($(clickitem).attr('data-sku') != "") {
            AddedString += ", SKU: " + $(clickitem).attr('data-sku');
        }
        if (AddedString != "") {
            $(itemName).attr('title', AddedString);
        }

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnUnitPrice');
        var txtItemRate = $(item).parent().parent().find('.txtUnitPrice');
        var spnProductRateValue = $(this).attr('data-suppliercost');
        $(spnItemRate).text(TransCurrency + parseFloat(spnProductRateValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(txtItemRate).val(parseFloat(spnProductRateValue).toFixed(2));

        /*Item Description Set*/
        var spnItemDesc = $(item).parent().parent().find('.spnProductDesc');
        $(spnItemDesc).text($(this).attr('data-description'));
        var txtItemDesc = $(item).parent().parent().find('.txtProductDesc');
        $(txtItemDesc).val($(this).attr('data-description'));

        /*Item Point Set*/
        //var spnItemRate = $(item).parent().parent().find('.spnProductPoint');
        //if (parseInt($(this).attr('data-point')) < 0) {
        //    $(spnItemRate).text(0);
        //    $(spnItemRate).attr("data-point", 0);
        //}
        //else {
        //    $(spnItemRate).text($(this).attr('data-point'));
        //    $(spnItemRate).attr("data-point", $(this).attr('data-point'));
        //}

        /*Item Quantity on hand Set*/
        //var spnItemRate = $(item).parent().parent().find('.spnProductQuantityOnHand');
        //if (parseInt($(this).attr('data-quantityonhand')) < 0) {
        //    $(spnItemRate).text(0);
        //}
        //else {
        //    $(spnItemRate).text($(this).attr('data-quantityonhand'));
        //}
        //if (parseInt($(this).attr('data-quantityonhand')) == 0) {
        //    $(spnItemRate).parent().addClass("qtyhandwarning");
        //}
        /*Item Quantity Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(1);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemRate).val(1);

        //var spnQtyLeft = $(item).parent().parent().find('.spnProductQuantityLeft');
        //$(spnQtyLeft).text(0);
        //var txtQtyLeft = $(item).parent().parent().find('.txtProductQuantityLeft');
        //$(txtQtyLeft).val(0);

        /*Item Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        var spnProductAmountValue = $(this).attr('data-retail');

        var spnItemVendorCost = $(item).parent().parent().find('.spnProductEquipmentvendorcost');
        var spnProductVndorcostValue = $(this).attr('data-equipvendorcost');
        $(spnItemVendorCost).attr("data-equipvendorcost", spnProductVndorcostValue);
        $(spnItemVendorCost).text(TransCurrency + parseFloat(spnProductVndorcostValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemVendorCost).val(spnProductVndorcostValue);

        $(spnItemRate).text(TransCurrency + parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val(spnProductAmountValue);
        var txtUnitRate = $(item).parent().parent().find('.txtProductUnitPrice');
        $(txtUnitRate).val(spnProductAmountValue);
        var spnItemProductRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemProductRate).text(TransCurrency + parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemRate).attr("data-unitprice", spnProductAmountValue);

        var isTaxable = $(this).attr('data-isTaxable'); 
        var chkTax = $(item).parent().parent().find('.chkTaxable');
        if (isTaxable == 'true') {
            $(chkTax).prop('checked', true);
        } else {
            $(chkTax).prop('checked', false);
        }
        


        /*checkbox style*/
        var chkItemCheck = $(item).parent().parent().find('.chkQuantityValid');
        $(chkItemCheck).addClass("block-important");
        CalculateNewAmount(); 
        $(".focusedItem.HasItem").removeClass('focusedItem');
    });
    $('.CustomerInvoiceTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    }); 
}



var SearchKeyUpService = function (item, event, Service) {
    console.log("test serv", Service);
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13 || event.keyCode == 9)
        return false;
    $.ajax({
        url: domainurl + "/Invoice/GetOnlyServiceListByKey",
        data: {
            key: $(item).val(),
            ServiceEquipment: Service,
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) { 
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'> <div class='AddNewEqp' onclick='AddNewSrvc()'>Add New Service</div>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(ServiceTemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''), 
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/resultparse[i].QuantityOnHand,
                        /*8*/resultparse[i].SupplierCost,
                        /*9*/resultparse[i].RetailPrice,
                        /*10*/resultparse[i].ManufacturerName.replaceAll('"', '\'\''),
                        /*11*/resultparse[i].SKU,
                        /*12*/resultparse[i].Point,
                        /*13*/resultparse[i].Equipmentvendorcost,
                        /*14*/resultparse[i].IsTaxable
                    );
                }

                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show(); 
                if (Service == 'OneTimeService') {
                    OneTimeServiceEqSuggestionclickbind(item);
                } else {
                    InvoiceEqSuggestionclickbind(item);
                } 
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDownService = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }

            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            $($(trselected).next('tr')).find('input.txtPartDescription').focus();
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');

        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }

            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.txtPartDescription').focus();
        }
    }
}


var NewAmountCallCount = 0;
var IsTaxableItem = false;
var calculateServiceAmount = function () {
    var ServiceTaxPercentage = 0;
    TotalServicePrice = 0;
    TotalTaxablePriceService = 0;
    TotalServicePriceWithTax = 0;
    ServicePlanAmount = 0;

    var OneTimeServiceTaxPercentage = 0;
    TotalOneTimeServicePrice = 0;
    TotalTaxablePriceOneTimeService = 0;
    TotalOneTimeServicePriceWithTax = 0;
    OneTimeServicePlanAmount = 0;

    //ServiceTaxPercentage = parseFloat($("#ServicetaxType").val());

    $("#CustomerServiceTable .HasItem").each(function () {
        var Price = parseFloat($(this).find(".txtProductAmount").val());
        TotalServicePrice += Price; 
        if (TaxExemption.toLowerCase() == "no") {
            IsTaxableItem = true;
        }
        
        if (IsTaxableItem) {
            TotalTaxablePriceService += Price;

        }
    });
    $("#CustomerOneTimeServiceTable .HasItem").each(function () {
        var Price = parseFloat($(this).find(".txtProductAmount").val());
        TotalOneTimeServicePrice += Price;
        if (TaxExemption.toLowerCase() == "no") {
            IsTaxableItem = true;
        }

        if (IsTaxableItem) {
            TotalTaxablePriceOneTimeService += Price; 
        }
    });
    /*console.log("ShowOneTimeServicePlan", ShowOneTimeServicePlan);*/
    //if (ShowOneTimeServicePlan) {
        
    //    OneTimeServicePlanAmount = TotalPrice * OneTimeServicePlanRate;
    //    TotalTaxablePriceOneTimeService += OneTimeServicePlanAmount;
    //    $(".OneTimeServicePlan").text(Currency + OneTimeServicePlanAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //    $(".OneTimeServicePlanSubtotal").text(Currency + (OneTimeServicePlanAmount + TotalOneTimeServicePrice).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //}
    
    if (ShowServicePlan) { 
        ServicePlanAmount = TotalPrice * ServicePlanRate;
        TotalTaxablePriceService += ServicePlanAmount;
        $(".ServicePlan").text(Currency + ServicePlanAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(".ServicePlanSubtotal").text(Currency + (ServicePlanAmount + TotalServicePrice ).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
    if (!isNaN(parseFloat($("#OneTimeServicetaxType").val()))) {
        ServiceTaxPercentage = parseFloat($("#OneTimeServicetaxType").val());
        ServiceTaxAmount = TotalTaxablePriceService * ServiceTaxPercentage / 100;
        TotalOneTimeServicePriceWithTax = OneTimeServiceTaxAmount + TotalOneTimeServicePrice + OneTimeServicePlanAmount;
    }

    $(".OneTimeServiceSubTotalWithoutTax").text(Currency + TotalOneTimeServicePrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".OneTimeServicetax_amount").text(Currency + OneTimeServiceTaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".OneTimeServicebalanceDueAmount").text(Currency + TotalOneTimeServicePriceWithTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    if (!isNaN(parseFloat($("#ServicetaxType").val()))) {
        ServiceTaxPercentage = parseFloat($("#ServicetaxType").val());
        ServiceTaxAmount = TotalTaxablePriceService * ServiceTaxPercentage / 100;
        TotalServicePriceWithTax = ServiceTaxAmount + TotalServicePrice + ServicePlanAmount;
    }
     
    $(".ServiceSubTotalWithoutTax").text(Currency + TotalServicePrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".Servicetax_amount").text(Currency + ServiceTaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".ServicebalanceDueAmount").text(Currency + TotalServicePriceWithTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));


    $(".amount-big").text(Currency + (TotalPriceWithTax + TotalServicePriceWithTax + TotalOneTimeServicePriceWithTax ).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
}

var CalculateNewAmount = function () {
    NewAmountCallCount++;
    if (NewAmountCallCount > 1) {
        IsChanged = true;
    }

    TotalProfit = 0;
    TotalOverHead = 0;
    TotalCost = 0;
    TotalPrice = 0;
    TotalTaxablePrice = 0;
    TotalPriceWithTax = 0;

    $("#CustomerEstimateTab .HasItem").each(function () {
        var IsLaborItem = true;
        var selCategory = $(this).find(".selCategory");
        if ($(selCategory).find("option:selected").text() != "Labor")
        {
            IsLaborItem = false;
        }
         
        var QTY = $(this).find(".txtQunatity").val();
        var UnitCost = parseFloat($(this).find(".txtUnitCost").val()); 
        var OverHead = parseFloat($(this).find(".txtOverhead").val());
        var Profit = parseFloat($(this).find(".txtProfit").val());;
        var Price = parseFloat($(this).find(".txtTotalPrice").val());
         

        $(this).find('.txtTotalCost').val((QTY * UnitCost).toFixed(3));
        $(this).find('.spnTotalCost').text(Currency + (QTY * UnitCost).toFixed(3).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
         

        if (!IsLaborItem) {
            TotalOverHead += 0;
            TotalProfit += (Profit + OverHead);
            TotalPrice += Price;
        }
        else {
            TotalOverHead += OverHead;
            TotalProfit += Profit;
            TotalPrice += Price;
        }
        
        TotalCost += UnitCost * QTY;
        //var IsTaxableItem = $(this).find(".chkTaxable").is(':checked'); 
        if (TaxExemption.toLowerCase() == "no") {
            IsTaxableItem = true;
        }
        if (IsTaxableItem) {
            TotalTaxablePrice += Price;
        }

    });

    
     
    if (!isNaN(parseFloat($("#taxType").val())))
    {
        TaxPercnetage = parseFloat($("#taxType").val()); 
        TaxAmount = TotalTaxablePrice * TaxPercnetage / 100;
        TotalPriceWithTax = TaxAmount + TotalPrice;
    }

    $(".TotalCost").text(Currency + TotalCost.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    $(".TotalProfit").text(Currency + TotalProfit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".TotalOverHead").text(Currency + TotalOverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    $(".SubTotalWithoutTax").text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".tax_amount").text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    
    $(".balanceDueAmount").text(Currency + TotalPriceWithTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    //$(".amount-big").text(Currency + TotalPriceWithTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    calculateServiceAmount();

    InitRowIndex();
    InitRowIndexService();
    InitRowIndexOneTimeService();
}
var InitRowIndex = function () {
    var i = 1;
    $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var InitRowIndexService = function () {
    var i = 1;
    $("#CustomerServiceTable tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var InitRowIndexOneTimeService = function () {
    var i = 1;
    $("#CustomerOneTimeServiceTable tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var ExpirationDateValidation = function () {
    var result = true;
    var now = new Date();
    var Today = new Date(now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate());
    var ExpirationDate = new Date($("#Invoice_DueDate").val());
    if (Today != "Invalid Date" && ExpirationDate != "Invalid Date") {
        if (ExpirationDate < Today) {
            $("#Invoice_DueDate").addClass("required");
            $("#DueDateGtToday").removeClass("hidden");
            result = false;
        } else {
            $("#Invoice_DueDate").removeClass("required");
            $("#DueDateGtToday").addClass("hidden");
            result = true;
        }
    }
    return result;
}
var CheckValue = []; 
var CheckValue = {
    
    Id: $("#EstimatorFilteId").val(),
    GroupedbyNone: $("#GroupedbyNoneVal").prop('checked') == true ? true : false,
    GroupedbyCategory: $("#GroupedbyCategoryVal").prop('checked') == true ? true : false,
    GroupedbySupplier: $("#GroupedbySupplierVal").prop('checked') == true ? true : false,
    GroupedbyLabor: $("#GroupedbyLaborVal").prop('checked') == true ? true : false,
    GroupedbyMaterial: $("#GroupedbyMaterialVal").prop('checked') == true ? true : false,
    GroupedbyLaborAndMaterial: $("#GroupedbyLaborAndMaterialVal").prop('checked') == true ? true : false,

    //ShowEverything: true,
    IncludeCosting: $("#IncludeCostVal").prop('checked') == true ? true : false,
    IncludeProfit: $("#IncludeProfitVal").prop('checked') == true ? true : false,
    IncludeMargin: $("#IncludeMarginVal").prop('checked') == true ? true : false,
    IncludeCost: $("#IncludeCostVal").prop('checked') == true ? true : false,
    IncludeOverhead: $("#IncludeOverheadVal").prop('checked') == true ? true : false,
    WithoutPricing: $("#WithoutPricingVal").prop('checked') == true ? true : false,
    OneTimeService: $("#OneTimeServiceVal").prop('checked') == true ? true : false,
    WithoutIndividualMaterialPricing: $("#WithoutIndividualMaterialPricingVal").prop('checked') == true ? true : false,
    WithoutIndividualLaborPricing: $("#WithoutIndividualLaborPricingVal").prop('checked') == true ? true : false,
    IncludeImage: $("#IncludeImageVal").prop('checked') == true ? true : false,
    IncludePDF: $("#IncludePDFVal").prop('checked') == true ? true : false,
    IncludeService: $("#IncludeServiceVal").prop('checked') == true ? true : false,
    IncludeManufacturer: $("#IncludeManufacturerVal").prop('checked') == true ? true : false,
    IncludeVariation: $("#IncludeVariationVal").prop('checked') == true ? true : false
}
var ResetCheckvalue = function ()
{
    CheckValue.GroupedbyNone= false,
    CheckValue.GroupedbyCategory = false;
    CheckValue.GroupedbySupplier = false; 
    CheckValue.GroupedbyLabor = false;
    CheckValue.GroupedbyMaterial = false;
    CheckValue.GroupedbyLaborAndMaterial= false,

    //CheckValue.ShowEverything = false;
    CheckValue.IncludeCost =false;
    CheckValue.IncludeProfit= false;
    CheckValue.IncludeMargin= false; 
    CheckValue.IncludeOverhead= false;
    CheckValue.WithoutPricing= false; 
    CheckValue.OneTimeService = false;
    CheckValue.WithoutIndividualMaterialPricing = false;
    CheckValue.WithoutIndividualLaborPricing = false;
    CheckValue.IncludeImage= false;
    CheckValue.IncludePDF = false;
    CheckValue.IncludeService = false;
    CheckValue.IncludeManufacturer = false;
    CheckValue.IncludeVariation = false;
}

var OpenEstimatorPreview = function () {
    setTimeout(function () {
        $(".CheckPrintAndPreview").click();
    }, 1000);
}
var OpenEstimatorContract = function () {
    setTimeout(function () {
        $(".CheckPrintAndOpenEstimatorContract").click();
    }, 1000);
}
var OpenEstimatorSend = function () {
    setTimeout(function () {
        $(".EstPreviewSend").click();
    }, 1000);
}

var EstimatorSaveMessage = function (text)
{
    $('.EstimatorSave_Message').html(text);
    setTimeout(function () {
        $('.EstimatorSave_Message').html("");
    }, 5000);
}

var SaveEstimate = function (CreatePO,PrintEstimator,EstimatorContract) {
    if ($(".HasItem").length == 0) {
        //OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", function () { });

        EstimatorSaveMessage("<span style='color:red;'>(No item has been added.)</span>");
        return;
    }
    var DetailList = [];
    var ServiceList = [];
    var OnerTimeServiceList = [];
    $("#CustomerEstimateTab .HasItem").each(function () {
        var PARTNumberTemp = $(this).find('.selPartNumber option:selected').text();
        var PARTNumber = "";
        var Variation = "";

        if ($(this).find('.selPartNumber').val() == "-1") {
            PARTNumber = "";
        }
        else {
            PARTNumber = PARTNumberTemp.split(' - ')[0];
            if (PARTNumberTemp.split(' - ').length > 1) {
                Variation = PARTNumberTemp.split(' - ')[1];
            }  
        }

        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            /*ManufacturerId: $(this).find('.selManufacturer').val(),*/
            PartDescription: $(this).find('.txtPartDescription').val(),
            SKU: $(this).find('.spnsku').text(),
            //PartNumber: $(this).find('.txtPartNumber').val(), //parseFloat($(this).find('.txtPartNumber').val().trim().replaceAll(',', '')),
            PartNumber: PARTNumber,
            CategoryId: $(this).find('.selCategory').val(),
            SupplierId: $(this).find('.selSupplier').val(),
            //ManufacturerId: $(this).find('.selManufacturer').val(),
            CategoryVal: $(this).find('.selCategory option:selected').text(),
            SupplierVal: $(this).find('.selSupplier option:selected').text(),
            //ManufacturerVal: $(this).find('.selManufacturer option:selected').text(),
            Unit: $(this).find('.selUnit').val(),
            Qunatity: $(this).find('.txtQunatity').val(),
            Overhead: $(this).find('.txtOverhead').val(),
            UnitCost: $(this).find('.txtUnitCost').val(),
            Profit: $(this).find('.txtProfit').val(),
            TotalPrice: $(this).find('.txtTotalPrice').val(),
            TotalCost: $(this).find('.txtTotalCost').val(),
            IsTaxable: $(this).find('.chkTaxable').is(":checked"),
            OverheadRate: $(this).find(".txtOverheadRate").val(),
            ProfitRate: $(this).find(".txtProfitRate").val(),
            EquipmentManufacturerId: $(this).find('.selPartNumber').val(),
            Variation: Variation,
            EstimatorId: EstimatorId,
        });
    });
    $("#CustomerServiceTable .HasItem").each(function () {
        ServiceList.push({
            EquipmentId: $(this).attr('data-id'),
            EquipmentName: $(this).find('.ProductName').val(),
            Amount: $(this).find('.txtProductAmount').val(),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtUnitPrice').val(),
            IsTaxable: $(this).find('.chkTaxable').is(":checked"),
            EstimatorId: EstimatorId,
        });
    }); 
    $("#CustomerOneTimeServiceTable .HasItem").each(function () {
        OnerTimeServiceList.push({
            EquipmentId: $(this).attr('data-id'),
            EquipmentName: $(this).find('.ProductName').val(),
            Amount: $(this).find('.txtProductAmount').val(),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtUnitPrice').val(),
            IsTaxable: $(this).find('.chkTaxable').is(":checked"),
            EstimatorId: EstimatorId,
        });
    });
    console.log("One time ", OnerTimeServiceList);
    if (CommonUiValidation()) {
        var url = "/Estimator/AddEstimator";
        var param = JSON.stringify({
            "Estimator.CustomerId": $('#Estimator_CustomerId').val(),
            "Estimator.EstimatorId": EstimatorId,
            "Estimator.BillingAddress": parent.tinyMCE.get('Estimator_BillingAddress').getContent(),
            "Estimator.ProjectAddress": parent.tinyMCE.get('Estimator_ProjectAddress').getContent(),
            "Estimator.ContractTerm": $("#Estimator_ContractTerm").val(),
            "Estimator.StartDate": $("#Estimator_StartDate").val(),
            "Estimator.CompletionDate": $("#Estimator_CompletionDate").val(),
            "Estimator.EmailAddress": $("#Estimator_EmailAddress").val(),
            "Estimator.Status": $('#EstimatorStatus option:selected').val(), //$("#EstimatorStatus").val(),
            "Estimator.Description": $(".InvoiceMessage").val(),
            "Estimator.TaxPercnetage": TaxPercnetage,
            "Estimator.TaxAmount": TaxAmount,
            "Estimator.TotalPrice": TotalPrice,
            "Estimator.PoriftPercentage": 0, //will be taken from the new field
            "Estimator.TotalProfitAmount": TotalProfit,
            "Estimator.OverheadCostPercentage": 0,//will be taken from the new field
            "Estimator.TotalOverheadCostAmount": TotalOverHead,
            "Estimator.TotalCost": TotalCost,
            "Estimator.DefaultMaterialMarkupRate": $(".MaterialMarkup").val(),
            "Estimator.DefaultOverheadRate": $(".LaborOverhead").val(),
            "Estimator.DefaultProfitRate": $(".LaborProfit").val(),
            "Estimator.PaymentTerm": $(".PaymentTerm").val(),
            "Estimator.EstimateDate": $("#Estimator_EstimateDate").val(),
            "Estimator.ExpiresOn": $("#Terms").val(),
            "Estimator.ServicePlanType": $("#Estimator_ServicePlanType").val(),
            "Estimator.ServicePlanRate": ServicePlanRate,
            "Estimator.ServicePlanAmount": ServicePlanAmount,
            "Estimator.ServiceTaxAmount": ServiceTaxAmount,
            "Estimator.ServiceTotalAmount": TotalServicePrice,
            "Estimator.OneTimeServiceTaxAmount": OneTimeServiceTaxAmount,
            "Estimator.OneTimeServiceTotalAmount": TotalOneTimeServicePrice,
            "Estimator.ActivationFee": $("#Estimator_ActivationFee").val(),
            estimatorDetails: DetailList,
            estimatorServices: ServiceList,
            estimatorOneTimeServices: OnerTimeServiceList,
            CreatePO: CreatePO,
            ServiceTax: ServiceTaxAmount,
            PrintEstimator: PrintEstimator
        });

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {

                if (CreatePO == "True") {
                    var resultparse = data.PurchaseOrderList;
                    if (resultparse.length > 0) {
                        var HeaderText = "<label><a class='cus-anchor' target='_blank' href='/Inventory?id=" + EstimatorId + "#PurchaseOrderTab'>Created POs:</a></label>"
                        var IdText = " <span class='cus-anchor' onclick=OpenPOByPurchaseOrderId(\'{0}\')>" + '{0}' + "</span>";
                        var PurchaseOrderText = "";
                        for (var i = 0; i < resultparse.length; i++) {

                            PurchaseOrderText = PurchaseOrderText + String.format(IdText, resultparse[i].PurchaseOrderId);

                        }

                        POText = HeaderText + PurchaseOrderText;
                        $(".POCreated_Header").removeClass("hidden");
                        $('.POCreated_Header').html(POText);
                    }
                    //$('.POCreated_Message').html("<a style='color:#2ca01c !important;' href='/Inventory#PurchaseOrderTab'>(POs CREATED)</a>");
                    $('.POCreated_Message').html("<span style='color:#2ca01c !important;'>(POs CREATED)</span>");
                    setTimeout(function () {
                        $('.POCreated_Message').html("");
                    }, 5000); //10000
                }
                if (EstimatorContract) {
                    $("#EstimatorContractPrintPopup").click();
                }
                if (PrintEstimator == "False") {
                    /*EstimatorSaveMessage("(Saved)");*/
                    OpenSuccessMessageNew("Success!", "", function () {
                        if (InitStatus == 'Init') {
                            OpenEstimatorTabLoad()
                            OpenEstimatorTab();
                            CloseTopToBottomModal();
                        }
                        else {
                            OpenEstimatorTabLoad()
                            OpenEstimatorTab();
                        }

                        /*window.location.reload();*/
                    });
                }
                if (PrintEstimator) {
                    $("#EstimatePrint").click();
                } else {
                    //CloseTopToBottomModal();
                }
                IsChanged = false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
    else {
        $('.estimator_contents_scroll').scrollTop(0);
    }
} 
 
//var EstimateSettingsInitialLoad = function () {
//    var WindowHeight = window.innerHeight;
//    var divHeight = WindowHeight - 100;
   
//    $(".estimator_contents_scroll").css("height", divHeight);
//}

var SetNewEquipmentRow = function () { 
    NewEquipmentRow = String.format(NewEquipmentRowBase, $("#CateGoryDropDown").html(), $("#SupplierDropdown").html(), $("#UnitDropdown").html(), $("#ManufacturerDropdown").html());
}


var GetManufacturerSKU = function (ManufacturerId, EquipmentId, RowNumber) {
    $.ajax({
        url: domainurl + "/Estimator/GetManufacturerSKUByEquipmentIdAndManuId",
        data: JSON.stringify({
            ManufacturerId: ManufacturerId,
            EquipmentId: EquipmentId,
            RowNumber: RowNumber,
        }),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                $("#CustomerEstimateTab tbody tr").each(function () {
                    if (parseInt($(this).find('.rowindex').text()) == data.RowNumber) {
                        $(this).find('.spnPartNumber').text(data.SKU);
                        $(this).find('.txtPartNumber').val(data.SKU); 
                    }
                });
            }
        }
    });
}

var GetVariationDropdown = function (ManufacturerId, EquipmentId, RowNumber)
{
     
    $.ajax({
        url: domainurl + "/Estimator/GetManuSKUByEquipIdAndManuId",
        data: JSON.stringify({
            ManufacturerId: ManufacturerId,
            EquipmentId: EquipmentId,
            RowNumber: RowNumber,
        }),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                $("#CustomerEstimateTab tbody tr").each(function () {
                    if (parseInt($(this).find('.rowindex').text()) == data.RowNumber) {
                         
                        var EMList = data.EMList;

                        var Index = 0;
                        var Dropdown = "<option value='-1'>Part Number</option>";
                        for (Index = 0; Index < EMList.length; Index++) {
                            Dropdown += "<option value='" + EMList[Index].Id + "'>" + EMList[Index].SKU + " " + (EMList[Index].Variation != null ? "- " + EMList[Index].Variation : "") + "</option>"
                        }
                        $(this).find('.selPartNumber').html(Dropdown); 
                    }
                });
            }
        }
    });
}
var GetEquipmentManufacturerList = function (EquipmentId) {
    $.ajax({
        url: domainurl + "/Estimator/GetEquipmentManufacturerWithPartNumber",
        data: JSON.stringify({
            EquipmentId: EquipmentId
        }),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                var resultparse = data.Manufacturer;
                if (resultparse.length > 0) {
                    //var IsPrimary = false;
                    var DefultText = "<option value='00000000-0000-0000-0000-000000000000'> Manufacturer</option><option value='11111111-1111-1111-1111-111111111111'> + Add New Manufacturer</option>";
                    var ddrmanufacturer = "<option value='{0}'> {1}</option>";
                    var ddrmanufacturerSelect = "<option selected='selected' value='{0}'> {1}</option>";
                    var ddrmanufacturerresult = "";
                    //if (resultparse.IsPrimary == true) {
                    //    ddrmanufacturer = "<option selected ='selected' value='{0}'> {1}</option>";
                    //}
                    for (var i = 0; i < resultparse.length; i++) {
                        if (resultparse[i].IsActive == true) {
                            ddrmanufacturerresult = ddrmanufacturerresult + String.format(ddrmanufacturerSelect,
                            resultparse[i].ManufacturerId,
                            resultparse[i].Name);
                        }
                        else {
                            ddrmanufacturerresult = ddrmanufacturerresult + String.format(ddrmanufacturer,
                            resultparse[i].ManufacturerId,
                            resultparse[i].Name
                        );
                        }
                        
                    }
                    ddrmanufacturerresult = DefultText + ddrmanufacturerresult;
                    //var resultDom = $(ManufactParentDom).find('.selManufacturer');
                    //$(resultDom).html(ddrmanufacturerresult);

                    GetVariationDropdown($(resultDom).val(), EquipmentId, $($(ManufactParentDom)).parent().find(".rowindex").text());

                }
            }
        }
    });
}
//var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

var OpenAcceptedTab = function (CustomerId) {
    $(".pendingestimatorTab li").removeClass('active');
    $(".openestimatorTab").removeClass('active');
    $(".openestimateTab_Load").removeClass('active');
     
    $(".acceptedestimatorTab").removeClass('hidden');
    $(".acceptedestimatorTab").addClass('active');
    $(".acceptedestimatorTab_Load").addClass('active');
    AcceptedEstimatorTabLoad();
    
}
var EstimeApproveById = function (EstimateConvertId) {
    $.ajax({
        url: domainurl + "/Estimator/EstimeApproveById",
        data: { Id: EstimateConvertId },
        type: "Post",
        dataType: "Json",
        success: function (data) {  
            CloseTopToBottomModal();
            if (typeof (OpenAcceptedTab) != "undefined") { 
                OpenEstimatorTabLoad()
                OpenEstimatorTab();
                AcceptedEstimatorTabLoad();
                OpenAcceptedTab(data.CustomerId);
            }
            OpenSuccessMessageNew("", data.message); 
            //CloseTopToBottomModal();
        }
    });
}
function formatActivationFee(input) {
    if (!input.value) {
        input.value = "0.00";
    } else {
        const numValue = parseFloat(input.value);
        if (!isNaN(numValue)) {
            input.value = numValue.toFixed(2);
        } else {
            input.value = "0.00";
        }
    }
}
$(document).ready(function () {
  
    if (EditCost == "False") {
        $(".txtUnitCost").prop("disabled", true);  
        $(".txtTotalCost").prop("disabled", true); 
    } else {
        $(".txtUnitCost").prop("disabled", false); 
        $(".txtTotalCost").prop("disabled", false);
    }

    if (EditPrice == "False") {
        $(".txtProfitRate").prop("disabled", true);  
        $(".txtOverheadRate").prop("disabled", true);  
        $(".txtTotalPrice").prop("disabled", true);  
    } else {
        $(".txtProfitRate").prop("disabled", false);  
        $(".txtOverheadRate").prop("disabled", false);  
        $(".txtTotalPrice").prop("disabled", false); 
    }

    $(".btnApprove").click(function () {
        var idval = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to approve this estimate?", function () {
            /*CloseTopToBottomModal();*/
            EstimeApproveById(idval);
        });
    });
    
    $("body").on('click', function (evt) {
        if (!$(evt.target).closest('#CustomerEstimateTab tr').length) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
        }
    });

    SetNewEquipmentRow();
    StartDateDatepicker = new Pikaday({
        field: $('#StartDate_Calendar')[0],
        trigger: $('#Estimator_StartDate')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    CompletionDateDatepicker = new Pikaday({
        field: $('#CompletionDate_Calendar')[0],
        trigger: $('#Estimator_CompletionDate')[0], 
        format: 'MM/DD/YYYY',
        firstDay: 1
    });

    EstimateDateDatepicker = new Pikaday({
        field: $('#EstimateDate_Calendar')[0],
        trigger: $('#EstimateDate')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    InitializeSuburbDropdownChild($('#Estimator_CustomerId'));
    if ($("#Terms").val() == '-1') {
        console.log("Test Default selected", $(this).val());
        $("#Estimator_CompletionDate").val('');
    }
    var ToDate = $("#Estimator_StartDate").val()
    $("#Terms").change(function () { 
        if ($(this).val() == '-1') {
            console.log("Test Default 01", $(this).val());
            $("#Estimator_CompletionDate").val('');
        }
        else if ($("#Terms").val() != 0 && $("#Terms").val() != "") {
            var NewStartDate = new Date(ToDate);
            if (NewStartDate == "Invalid Date") {
                NewStartDate = new Date();
                $("#Estimator_StartDate").val(NewStartDate.getMonth() + 1 + "/" + NewStartDate.getDate() + "/" + NewStartDate.getFullYear());
            }
            NewStartDate = NewStartDate.addDays(parseInt($("#Terms").val()));
            NewStartDate = NewStartDate.getMonth() + 1 + "/" + NewStartDate.getDate() + "/" + NewStartDate.getFullYear();
            $("#Estimator_CompletionDate").val(NewStartDate);
        }
        else {
            $("#Estimator_CompletionDate").val(ToDate);
        }
    });

    $(".estimator_contents_scroll").height(window.innerHeight - 90);

    var Popupwidth = 920;
    var Popupheight = 600;
    if (Device.All()) {
        Popupwidth = window.innerWidth;
        Popupheight = window.innerHeight;
    }

    var idlist = [{ id: ".EstPreview", type: 'iframe', width: Popupwidth, height: Popupheight }, { id: ".EstPreviewPopup", type: 'iframe', width: 600, height: Popupheight }, { id: ".EstPreviewSend", type: 'iframe', width: Popupwidth, height: Popupheight }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $(".EstimatePrintAndSend").click(function () {
        $(".PurchaseOrderId").addClass("hidden");
        $(".POCreated_Header").removeClass("hidden");
    });

    $("#Estimator_ServicePlanType").change(function () { 
        ServicePlanRate = parseFloat($("#Estimator_ServicePlanType").val());
        CalculateNewAmount();
    });
    //if(PurchaseOrder == "true"){
    //    //OpenSuccessMessageNew("Success!", data.message, function () { OpenEstimatorTabLoad() });
    //    $('.EstimatorName').hide();
    //    $('.EstimatorSave_Message').hide();
    //    $('.POCreated_Message').html("Estimator: " + EstimatorId +" (POs CREATED)");
    //}
    //if (PrintEstimator == "False") {
    //            //OpenSuccessMessageNew("Success!", data.message, function () { OpenEstimatorTabLoad() });
    //            $('.EstimatorName').hide();
    //            $('.POCreated_Message').hide();
    //            $('.EstimatorSave_Message').html("Estimator: " + EstimatorId +" (Saved)");
            //}

    $(".CustomerEstimateTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();
    $(".add-invoice-container input,.add-invoice-container textarea").change(function () {
        IsChanged = true;

    });
    $("#Invoice_DueDate").blur(function () {
        ExpirationDateValidation();
    }); 
    InitRowIndex();
    InitRowIndexService();
    InitRowIndexOneTimeService();
    $(".LoadParentAddress").click(function () {
        var editor = tinymce.get('Estimator_BillingAddress');
        editor.setContent($("#Estimator_ParentBillingAddress").val());
    });

    $("#CustomerList").focusout(function () {
        setTimeout(function () {
            $(".customer_name_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CustomerEstimateTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide();
        var ProductNameDom = $(this).parent().find('span.spnProductName'); 
        $(ProductNameDom).text($(this).val());
    });
    $("#CustomerEstimateTab tbody").on('blur', 'tr', function (item) { 
        if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined'
            && typeof ($(item.target).parent().parent().parent().attr('data-id')) == 'undefined') {
            var trdom = $(item.target).parent().parent();

            $(trdom).find("input.txtPartDescription").val('');
            $(trdom).find("span.spnPartDescription").html(PartDescriptionText);
            
            $(trdom).find("input.txtPartNumber").val('');
            $(trdom).find("span.spnPartNumber").html(PartNumberText);

            //$(trdom).find(".selCategory").val('-1'); // we will filter equipment by category and supplier thats why we will not 
            //$(trdom).find(".selSupplier").val('00000000-0000-0000-0000-000000000000');//clear these fields
            $(trdom).find(".selUnit").val('-1');

            $(trdom).find("input.txtProfit").val('');
            $(trdom).find("span.spnProfit").text('');

            $(trdom).find("input.txtOverhead").val('');
            $(trdom).find("span.spnOverhead").text('');

            $(trdom).find("input.txtUnitCost").val('');
            $(trdom).find("span.spnUnitCost").text('');

            $(trdom).find("input.txtTotalCost").val('');
            $(trdom).find("span.spnTotalCost").text('');

            $(trdom).find("input.txtTotalPrice").val('');
            $(trdom).find("span.spnTotalPrice").text('');

            CalculateNewAmount();
        }

    });

    $("#CustomerEstimateTab tbody").on('click', '.editEquipment', function (e)
    {
        var EquipmentId = $(this).parent().parent().attr('data-id');
        EditEqp(EquipmentId, "Edit");

    });
    

    $("#CustomerEstimateTab tbody").on('click', 'tr td', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if ($(e.target).hasClass("spnPartDescription")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtPartDescription').focus();
        }
        else if ($(e.target).hasClass("spnPartDescription2")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent().parent()).addClass("focusedItem");
            $(e.target).parent().parent().find('input.txtPartDescription').focus();
        }
        else if ($(e.target).hasClass("spnPartNumber")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtPartDescription').focus();
        }
        else if ($(e.target).hasClass("spnUnit")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtUnit').focus();
        }
        else if ($(e.target).hasClass("spnQunatity")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtQunatity').focus();
        }
        else if ($(e.target).hasClass("spnProfit")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtProfit').focus();
        }
        else if ($(e.target).hasClass("spnOverhead")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtOverhead').focus();
        }
        else if ($(e.target).hasClass("spnUnitCost")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtUnitCost').focus();
        }
        else if ($(e.target).hasClass("spnTotalCost")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtTotalCost').focus();
        }
        else if ($(e.target).hasClass("spnTotalPrice")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtTotalPrice').focus();
        }
        else if ($(e.target).hasClass("spnProfitRate")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtProfitRate').focus(); 
        }
        else if ($(e.target).hasClass("spnOverheadRate")) {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input.txtOverheadRate').focus();//
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT' || e.target.tagName.toUpperCase() == 'SELECT') {
            return;
        }
        else {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input:first-child').focus();
        }
    });

    $("#CustomerEstimateTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#CustomerEstimateTab tbody tr:last").after(NewEquipmentRow); 
        if (ShowRetailVal == "True") {
            $(".retail_area").show();
        }
        else {
            $(".retail_area").hide();
        }
        var i = 1;
        $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $("#CustomerEstimateTab tbody").on('change', 'tr td .selSupplier', function (e) {
        if ($(this).val() == "11111111-1111-1111-1111-111111111111") {
            OpenRightToLeftModal("/Supplier/AddSupplier/");
            $(this).val("00000000-0000-0000-0000-000000000000");
        }
    });
    //$("#CustomerEstimateTab tbody").on('change', 'tr td .selManufacturer', function (e) {
    //    if ($(this).val() == "11111111-1111-1111-1111-111111111111") {
    //        OpenRightToLeftModal("/app/AddManufacturer");
    //        $(this).val("00000000-0000-0000-0000-000000000000");
    //    } else {
    //        var ManufacturerId = $(this).val();
    //        var EquipmentId = $(this).parent().parent().attr('data-id');
    //        var RowNumber = $(this).parent().parent().find(".rowindex").text();

    //        GetVariationDropdown(ManufacturerId, EquipmentId, RowNumber);

    //    }
    //});

    $('#Estimator_CustomerId').change(function () {
        $.ajax({
            url: domainurl + "/Estimator/EstimatorCustomerChange",
            data: JSON.stringify({
                CustomerId: $(this).val() 
            }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    tinyMCE.get('Estimator_BillingAddress').setContent(data.BillingAddress);
                    tinyMCE.get('Estimator_ProjectAddress').setContent(data.ProjectAddress);
                }
            }
        });
    });


    //$("#CustomerEstimateTab tbody").on('change', 'tr td .selCategory', function (e) {
    //    var QTY = parseInt($(this).parent().parent().parent().find(".txtQunatity").val());
    //    console.log(QTY);
    //    var UnitCost = parseFloat($(this).parent().parent().parent().find(".txtUnitCost").val());
    //    var OverheadRate = parseFloat($(this).parent().parent().parent().attr('data-overheadrate'));
    //    var ProfitRate = parseFloat($(this).parent().parent().parent().attr('data-profitrate'));
    //    var SupplierCost = parseFloat($(this).parent().parent().parent().find('.txtUnitCost').val());
    //    var TotalCost = QTY * SupplierCost;
    //    var TotalPrice;
    //    var Profit;
    //    var OverHead;
    //    console.log($(this).parent().find(".selCategory option:selected").text());

    //    if ($(this).parent().find(".selCategory option:selected").text() == "Labor") {
    //        var LaborProfit = parseFloat($(".LaborProfit").val());
    //        if (!isNaN(LaborProfit)) {
    //            ProfitRate = LaborProfit;
    //        }
    //        OverHead = (TotalCost * OverheadRate / 100);
    //        Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC

    //        TotalPrice = TotalCost + OverHead + Profit;
    //    }
    //    else {
    //        var MaterialMarkup = parseFloat($(".MaterialMarkup").val());
    //        if (!isNaN(MaterialMarkup)) {
    //            OverheadRate = MaterialMarkup;
    //        }


    //        OverHead = (TotalCost * OverheadRate / 100);
    //        Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC

    //        TotalPrice = TotalCost + OverHead + Profit;
    //    }
    //    /**
    //    * Total Cost
    //    * */
    //    var spnTotalCost = $(this).parent().parent().parent().find('.spnTotalCost');
    //    $(spnTotalCost).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //    var txtTotalCost = $(this).parent().parent().parent().find('.txtTotalCost');
    //    $(txtTotalCost).val(TotalPrice.toFixed(2));

    //    /**
    //    * Total Price
    //    * */
    //    var spnTotalPrice = $(this).parent().parent().parent().find('.spnTotalPrice');
    //    $(spnTotalPrice).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //    var txtTotalPrice = $(this).parent().parent().parent().find('.txtTotalPrice');
    //    $(txtTotalPrice).val(TotalPrice.toFixed(2));
    //    /**
    //    * Profit
    //    * */
    //    var spnProfit = $(this).parent().parent().parent().find('.spnProfit');
    //    $(spnProfit).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //    var txtProfit = $(this).parent().parent().parent().find('.txtProfit');
    //    $(txtProfit).val(Profit.toFixed(2));
    //    /**
    //    * OverHead
    //    * */
    //    var spnOverhead = $(this).parent().parent().parent().find('.spnOverhead');
    //    $(spnOverhead).text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    //    var txtOverhead = $(this).parent().parent().parent().find('.txtOverhead');
    //    $(txtOverhead).val(OverHead.toFixed(2));

    //    CalculateNewAmount();
    //});
    
    $(".CustomerEstimateTab tbody").on('click', 'tr td i.fa.fa-trash-o', function (e) {
        $(this).parent().parent().remove();
        var i = 1;
        if ($(".CustomerEstimateTab tbody tr").length < 2) {
            $("#CustomerEstimateTab tbody tr:last").after(NewEquipmentRow);
            if (ShowRetailVal == "True") {
                $(".retail_area").show();
            }
            else {
                $(".retail_area").hide();
            }
        }
        $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewAmount();

    });

    // Start binding textbox value to span
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtPartDescription", function () {

        var PartDesc = $(this).parent().parent().find('input.txtPartDescription');
        var PartDescDom = $(this).parent().parent().find('span.spnPartDescription');
        $(PartDescDom).text($(PartDesc).val());
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtPartNumber", function () {

        var PartNumber = $(this).parent().parent().find('input.txtPartNumber');
        var PartNumberDom = $(this).parent().parent().find('span.spnPartNumber');
        $(PartNumberDom).text($(PartNumber).val());
    });
 
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtUnit", function () {
        var Unit = $(this).parent().parent().find('input.txtUnit');
        var UnitDom = $(this).parent().parent().find('span.spnUnit');
        $(UnitDom).text($(Unit).val());
    });

    $(".CustomerEstimateTab tbody").on('change', "tr td .txtQunatity", function () {
        //var Qty = parseInt($(this).parent().parent().find('input.txtQunatity').val());
        var Qty = $(this).parent().parent().find('input.txtQunatity').val();
        if (Qty > 0) {
            var QtyDom = $(this).parent().parent().find('span.spnQunatity');
            $(QtyDom).text(Qty);

            //Total Cost
            var UnitCost = parseFloat($(this).parent().parent().find('input.txtUnitCost').val());
            var TotalCost = Qty * UnitCost;

            //Total Cost Update
            $(this).parent().parent().find('input.txtTotalCost').val(TotalCost.toFixed(2));
            $(this).parent().parent().find('span.spnTotalCost').val(TotalCost.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //OverHead AND Profit
            var OverHeadRate = parseFloat($(this).parent().parent().find(".txtProfitRate").val());//.attr('data-overheadrate'));
            var ProfitRate = parseFloat($(this).parent().parent().find(".txtOverheadRate").val());//.attr('data-profitrate'));

            var selCategory = $(this).parent().parent().find(".selCategory");
            if ($(selCategory).find("option:selected").text() != "Labor") {
                var MaterialMarkup = parseFloat($(".MaterialMarkup").val());
                if (!isNaN(MaterialMarkup)) {
                    OverHeadRate = MaterialMarkup;
                }
                 
            } else {
                var LaborProfit = parseFloat($(".LaborProfit").val());
                if (!isNaN(LaborProfit)) {
                    ProfitRate = LaborProfit;
                }
                var LaborOverHead = parseFloat($(".LaborOverhead").val());
                if (!isNaN(LaborOverHead)) {
                    OverHeadRate = LaborOverHead;
                }
            }

            var OverHead = (TotalCost * OverHeadRate / 100);
            var Profit = ((TotalCost + OverHead) * ProfitRate / 100);//#ProfitCC

            //profit  Update
            $(this).parent().parent().find('input.txtProfit').val(Profit.toFixed(2));
            $(this).parent().parent().find('span.spnProfit').text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //overhead  Update
            $(this).parent().parent().find('input.txtOverhead').val( OverHead.toFixed(2));
            $(this).parent().parent().find('span.spnOverhead').text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //Total  Update
            $(this).parent().parent().find('input.txtTotalPrice').val((OverHead + Profit + TotalCost).toFixed(2));
            $(this).parent().parent().find('span.spnTotalPrice').text(Currency + (OverHead + Profit + TotalCost).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            CalculateNewAmount();
        }
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProfit", function () {
        var Profit = parseFloat($(this).parent().parent().find('input.txtProfit').val());
        var ProfitDom = $(this).parent().parent().find('span.spnProfit');
        $(ProfitDom).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var Overhead = parseFloat($(this).parent().parent().find('input.txtOverhead').val());
        var OverheadDom = $(this).parent().parent().find('span.spnOverhead');
        $(OverheadDom).text(Currency + Overhead.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var TotalCost = parseFloat($(this).parent().parent().parent().find('input.txtTotalCost').val());
        
        var TotalPrice = TotalCost + Profit + Overhead;

        $(this).parent().parent().parent().find('input.txtTotalPrice').val(TotalPrice.toFixed(2));
        $(this).parent().parent().parent().find('span.spnTotalPrice').val(TotalPrice.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtOverhead", function () {
        var Profit = parseFloat($(this).parent().parent().find('input.txtProfit').val());
        var ProfitDom = $(this).parent().parent().find('span.spnProfit');
        $(ProfitDom).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var Overhead = parseFloat($(this).parent().parent().find('input.txtOverhead').val());
        var OverheadDom = $(this).parent().parent().find('span.spnOverhead');
        $(OverheadDom).text(Currency + Overhead.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));


        var TotalCost = parseFloat($(this).parent().parent().parent().find('input.txtTotalCost').val());

        var TotalPrice = TotalCost + Profit + Overhead;

        $(this).parent().parent().parent().find('input.txtTotalPrice').val(TotalPrice.toFixed(2));
        $(this).parent().parent().parent().find('span.spnTotalPrice').val(TotalPrice.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));


    });

    $(".CustomerEstimateTab tbody").on('change', "tr td .txtUnitCost", function () { //Same AS QTY
        var UnitCost = parseFloat($(this).parent().parent().find('input.txtUnitCost').val());
        var UnitCostDom = $(this).parent().parent().find('span.spnUnitCost');
        $(UnitCostDom).text(Currency + UnitCost.toFixed(3).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        
        //var Qty = parseInt($(this).parent().parent().parent().find('input.txtQunatity').val());
        var Qty = $(this).parent().parent().parent().find('input.txtQunatity').val();
        if (Qty > 0) {
            var QtyDom = $(this).parent().parent().parent().find('span.spnQunatity');
            $(QtyDom).text(Qty);

            //Total Cost
            
            var TotalCost = Qty * UnitCost;

            //Total Cost Update
            $(this).parent().parent().find('input.txtTotalCost').val(TotalCost.toFixed(2));
            $(this).parent().parent().find('span.spnTotalCost').val(TotalCost.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //OverHead AND Profit
            var OverHeadRate = parseFloat($(this).parent().parent().parent().find(".txtOverheadRate").val());//.attr('data-overheadrate'));
            var ProfitRate = parseFloat($(this).parent().parent().parent().find(".txtProfitRate").val());//.attr('data-profitrate'));

            var selCategory = $(this).parent().parent().parent().find(".selCategory");
            if ($(selCategory).find("option:selected").text() != "Labor") {
                var MaterialMarkup = parseFloat($(".MaterialMarkup").val());
                if (!isNaN(MaterialMarkup)) {
                    OverHeadRate = MaterialMarkup;
                } 
            } else {
                var LaborProfit = parseFloat($(".LaborProfit").val());
                if (!isNaN(LaborProfit)) {
                    ProfitRate = LaborProfit;
                }
                var LaborOverHead = parseFloat($(".LaborOverhead").val());
                if (!isNaN(LaborOverHead)) {
                    OverHeadRate = LaborOverHead;
                }

            }

            var OverHead = (TotalCost * OverHeadRate / 100);
            var Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC
            
            //profit  Update
            $(this).parent().parent().parent().find('input.txtProfit').val(Profit.toFixed(2));
            $(this).parent().parent().parent().find('span.spnProfit').text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //overhead  Update
            $(this).parent().parent().parent().find('input.txtOverhead').val(OverHead.toFixed(2));
            $(this).parent().parent().parent().find('span.spnOverhead').text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //Total  Update
            $(this).parent().parent().parent().find('input.txtTotalPrice').val((OverHead + Profit + TotalCost).toFixed(2));
            $(this).parent().parent().parent().find('span.spnTotalPrice').text(Currency + (OverHead + Profit + TotalCost).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            CalculateNewAmount();
        }
         
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtTotalCost", function () {

        var TotalCost = parseFloat($(this).parent().parent().find('input.txtTotalCost').val());
        var TotalCostDom = $(this).parent().parent().find('span.spnTotalCost');
        $(TotalCostDom).text(Currency + TotalCost.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtTotalPrice", function () {
        var TotalPrice = parseFloat($(this).parent().parent().find('input.txtTotalPrice').val());
        var TotalPriceDom = $(this).parent().parent().find('span.spnTotalPrice');
        $(TotalPriceDom).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    });
    //End binding textbox value to span
    /*
    $('#txtProduct').change(function () {
        $('.txtProductprofit').val(parseFloat("0" + $('.txtProductunit').val()) + parseFloat("0" + $('.txtProductqty').val()));
    });
    */ 
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductAmount", function () {
        console.log("Product Amount Change");
        var ProductAmount = $(this).parent().parent().find('input.txtProductAmount');
    
        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');
        if ($(ProductAmount).val() != "" && parseFloat($(ProductAmount).val().trim().replaceAll(',', '')) >= 0)
        {
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewAmount();
        }
        else {
            var CalculateAmount = parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();
            $(ProductAmount).val(CalculateAmount);
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewAmount();
        }
    });
    $("#CustomerServiceTable tbody").on('change', "tr td .txtProductQuantity", function () {
        //var Quantity = parseInt($(this).val());
        var Quantity = $(this).val();
        var spnQuantity = $(this).parent().find(".spnProductQuantity");
        $(spnQuantity).text(Quantity);

        if (!isNaN(Quantity)) { 
            var SpnProductAmount = $(this).parent().parent().find(".spnProductAmount");
            var txtProductAmount = $(this).parent().parent().find(".txtProductAmount");

            var spnUnitPrice = $(this).parent().parent().find(".spnUnitPrice");
            var txtUnitPrice = $(this).parent().parent().find(".txtUnitPrice");

            var ProductAmount = parseFloat($(txtUnitPrice).val()) * Quantity;


            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtProductAmount).val( ProductAmount.toFixed(2));
        }
        CalculateNewAmount();
    });
    $("#CustomerOneTimeServiceTable tbody").on('change', "tr td .txtProductQuantity", function () {
        //var Quantity = parseInt($(this).val());
        var Quantity = $(this).val();
        var spnQuantity = $(this).parent().find(".spnProductQuantity");
        $(spnQuantity).text(Quantity);

        if (!isNaN(Quantity)) {
            var SpnProductAmount = $(this).parent().parent().find(".spnProductAmount");
            var txtProductAmount = $(this).parent().parent().find(".txtProductAmount");

            var spnUnitPrice = $(this).parent().parent().find(".spnUnitPrice");
            var txtUnitPrice = $(this).parent().parent().find(".txtUnitPrice");

            var ProductAmount = parseFloat($(txtUnitPrice).val()) * Quantity;


            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtProductAmount).val(ProductAmount.toFixed(2));
        }
        CalculateNewAmount();
    });

    $("#CustomerServiceTable tbody").on('change', "tr td .txtUnitPrice", function () {
        var UnitPrice = parseFloat($(this).val());
        var spnUnitPrice = $(this).parent().find(".spnUnitPrice");
        $(spnUnitPrice).text(Currency + UnitPrice);
        if (!isNaN(UnitPrice)) {
            var SpnProductAmount = $(this).parent().parent().find(".spnProductAmount");
            var txtProductAmount = $(this).parent().parent().find(".txtProductAmount");

            var spnProductQuantity = $(this).parent().parent().parent().find(".spnProductQuantity");
            var txtProductQuantity = $(this).parent().parent().parent().find(".txtProductQuantity");

            var ProductAmount = parseFloat($(txtProductQuantity).val()) * UnitPrice;


            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtProductAmount).val(ProductAmount.toFixed(2));
        }
        CalculateNewAmount();
    });
    $("#CustomerOneTimeServiceTable tbody").on('change', "tr td .txtUnitPrice", function () {
        var UnitPrice = parseFloat($(this).val());
        var spnUnitPrice = $(this).parent().find(".spnUnitPrice");
        $(spnUnitPrice).text(Currency + UnitPrice);
        if (!isNaN(UnitPrice)) {
            var SpnProductAmount = $(this).parent().parent().find(".spnProductAmount");
            var txtProductAmount = $(this).parent().parent().find(".txtProductAmount");

            var spnProductQuantity = $(this).parent().parent().parent().find(".spnProductQuantity");
            var txtProductQuantity = $(this).parent().parent().parent().find(".txtProductQuantity");

            var ProductAmount = parseFloat($(txtProductQuantity).val()) * UnitPrice;


            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtProductAmount).val(ProductAmount.toFixed(2));
        }
        CalculateNewAmount();
    });
    $("#CustomerServiceTable tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmount = parseFloat($(this).val());
        if (!isNaN(ProductAmount)) {

            var txtProductQuantity = $(this).parent().parent().parent().find(".txtProductQuantity");
            var spnUnitPrice = $(this).parent().parent().find(".spnUnitPrice");
            var txtUnitPrice = $(this).parent().parent().find(".txtUnitPrice");

            var ProductRate = ProductAmount / parseFloat($(txtProductQuantity).val());
            $(spnUnitPrice).text(Currency + ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtUnitPrice).val(ProductRate.toFixed(2));

            var SpnProductAmount = $(this).parent().find(".spnProductAmount");
            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        CalculateNewAmount();
    });
    $("#CustomerOneTimeServiceTable tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmount = parseFloat($(this).val());
        if (!isNaN(ProductAmount)) {

            var txtProductQuantity = $(this).parent().parent().parent().find(".txtProductQuantity");
            var spnUnitPrice = $(this).parent().parent().find(".spnUnitPrice");
            var txtUnitPrice = $(this).parent().parent().find(".txtUnitPrice");

            var ProductRate = ProductAmount / parseFloat($(txtProductQuantity).val());
            $(spnUnitPrice).text(Currency + ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(txtUnitPrice).val(ProductRate.toFixed(2));

            var SpnProductAmount = $(this).parent().find(".spnProductAmount");
            $(SpnProductAmount).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        CalculateNewAmount();
    });
    //$("#CustomerServiceTable tbody").on('change', "tr td .ProductName", function () {

    //    var Productname = $(this).val(); 
    //    var SpnProductAmount = $(this).parent().find(".spnProductName");
    //    $(SpnProductAmount).text(Productname);
          
    //});

    

    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductRate", function () {
        /*
         *If product rate changes make change to amount.
         */
        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRate = $(this).parent().parent().find('input.txtProductRate');
        var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        if ($(ProductRate).val() != "" && parseFloat($(ProductRate).val().trim().replaceAll(',', '')) >= 0) {
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();

                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
        else {
           
            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            var CalculateRate = parseFloat($(txtProductAmountDom).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val();
            $(ProductRate).val(CalculateRate);
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();


                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text("$" + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
  
    });
    /*$(".CustomerEstimateTab tbody").on('change', "tr td .txtProductDesc", function () {

        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });*/
    $('#taxExemption').on('input', function () {
        var taxExemption = $(this).val().trim().toLowerCase(); 
        if (taxExemption === "no") {
            CalculateNewAmount();
        }
    });
    //$(".CustomerEstimateTab tbody, #CustomerServiceTable tbody").on('change', "tr td .chkTaxable", function ()
    //{
    //    CalculateNewAmount();
    //});

    $(".InvoiceSaveButton").click(function () {
       
        if ($(".HasItem").length == 0) {
            OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
        } 
        else {
            /*SaveEstimate();*/
        } 
    });

    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProfitRate", function () {

    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtOverheadRate,tr td .txtProfitRate", function () {
        //var QTY = parseInt($(this).find(".txtQunatity").val());
        //var UnitCost = parseFloat($(this).find(".txtUnitCost").val()); 

        var ParentTR = $(this).parent().parent().parent();

        var TotalCost = parseFloat($(ParentTR).find(".txtTotalCost").val());
        var OverheadRate = parseFloat($(ParentTR).find('.txtOverheadRate').val());
        var ProfitRate = parseFloat($(ParentTR).find('.txtProfitRate').val());



        var OverHead = (TotalCost * OverheadRate / 100);
        var Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC

        var TotalPrice = TotalCost + OverHead + Profit;
         /**
        * spnProfit Rate & Overhead Rate Set
        * */
        $(ParentTR).find('.spnProfitRate').text(ProfitRate+ '%' );
        $(ParentTR).find('.spnOverheadRate').text(OverheadRate + '%');

        /**
        * Total Cost
        * */
        var spnTotalCost = $(ParentTR).find('.spnTotalCost');
        $(spnTotalCost).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtTotalCost = $(ParentTR).find('.txtTotalCost');
        $(txtTotalCost).val(TotalPrice.toFixed(2));

        /**
        * Total Price
        * */
        var spnTotalPrice = $(ParentTR).find('.spnTotalPrice');
        $(spnTotalPrice).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtTotalPrice = $(ParentTR).find('.txtTotalPrice');
        $(txtTotalPrice).val(TotalPrice.toFixed(2));
        /**
        * Profit
        * */
        var spnProfit = $(ParentTR).find('.spnProfit');
        $(spnProfit).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtProfit = $(ParentTR).find('.txtProfit');
        $(txtProfit).val(Profit.toFixed(2));
        /**
        * OverHead
        * */
        var spnOverhead = $(ParentTR).find('.spnOverhead');
        $(spnOverhead).text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtOverhead = $(ParentTR).find('.txtOverhead');
        $(txtOverhead).val(OverHead.toFixed(2));

        CalculateNewAmount();

    });

    $(".MaterialMarkup").change(function () {
        $(".CustomerEstimateTab .HasItem").each(function () {
            //var QTY = parseInt($(this).find(".txtQunatity").val());
            var QTY = $(this).find(".txtQunatity").val();
            var UnitCost = parseFloat($(this).find(".txtUnitCost").val());
            //var OverHeadRate = parseFloat($(this).attr('data-overheadrate'));
            //var ProfitRate = parseFloat($(this).attr('data-profitrate'));
            //var OverHead = parseFloat($(this).find(".txtOverhead").val());
            //var Profit = parseFloat($(this).find(".txtProfit").val());;
            var Price = parseFloat($(this).find(".txtTotalPrice").val());
            var selCategory = $(this).find(".selCategory");

            if ($(selCategory).find("option:selected").text() != "Labor") {
                var OverheadRate = parseFloat($(this).attr('data-overheadrate'));
                var ProfitRate = parseFloat($(this).attr('data-profitrate'));
                var SupplierCost = parseFloat($(this).find('.txtUnitCost').val());
                var TotalCost = QTY * SupplierCost;

                var MaterialMarkup = parseFloat($(".MaterialMarkup").val());
                if (!isNaN(MaterialMarkup)) {
                    OverheadRate = MaterialMarkup;
                }

                var spnOverheadRate = $(this).find(".spnOverheadRate");
                var txtOverheadRate = $(this).find(".txtOverheadRate");

                $(spnOverheadRate).text(OverheadRate + '%');
                $(txtOverheadRate).val(OverheadRate);
                


                var OverHead = (TotalCost * OverheadRate / 100);
                var Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC

                var TotalPrice = TotalCost + OverHead + Profit;


                /**
                * Total Cost
                * */
                var spnTotalCost = $(this).find('.spnTotalCost');
                $(spnTotalCost).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtTotalCost = $(this).find('.txtTotalCost');
                $(txtTotalCost).val(TotalPrice.toFixed(2));

                /**
                * Total Price
                * */
                var spnTotalPrice = $(this).find('.spnTotalPrice');
                $(spnTotalPrice).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtTotalPrice = $(this).find('.txtTotalPrice');
                $(txtTotalPrice).val(TotalPrice.toFixed(2));
                /**
                * Profit
                * */
                var spnProfit = $(this).find('.spnProfit');
                $(spnProfit).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtProfit = $(this).find('.txtProfit');
                $(txtProfit).val(Profit.toFixed(2));
                /**
                * OverHead
                * */
                var spnOverhead = $(this).find('.spnOverhead');
                $(spnOverhead).text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtOverhead = $(this).find('.txtOverhead');
                $(txtOverhead).val(OverHead.toFixed(2));

                CalculateNewAmount();
            }
        });
    });

    $(".LaborProfit, .LaborOverhead").change(function () {
        $(".CustomerEstimateTab .HasItem").each(function () {
            //var QTY = parseInt($(this).find(".txtQunatity").val());
            var QTY = $(this).find(".txtQunatity").val();
            var UnitCost = parseFloat($(this).find(".txtUnitCost").val());
            //var OverHeadRate = parseFloat($(this).attr('data-overheadrate'));
            //var ProfitRate = parseFloat($(this).attr('data-profitrate'));
            //var OverHead = parseFloat($(this).find(".txtOverhead").val());
            //var Profit = parseFloat($(this).find(".txtProfit").val());;
            var Price = parseFloat($(this).find(".txtTotalPrice").val());
            var selCategory = $(this).find(".selCategory");

            if ($(selCategory).find("option:selected").text() == "Labor") {
                var OverheadRate = parseFloat($(this).attr('data-overheadrate'));
                var ProfitRate = parseFloat($(this).attr('data-profitrate'));
                var SupplierCost = parseFloat($(this).find('.txtUnitCost').val());
                var TotalCost = QTY * SupplierCost;

                var LaborProfit = parseFloat($(".LaborProfit").val());
                if (!isNaN(LaborProfit)) {
                    ProfitRate = LaborProfit;
                }

                var LaborOverHead = parseFloat($(".LaborOverhead").val());
                if (!isNaN(LaborOverHead)) {
                    OverheadRate = LaborOverHead;
                }


                var spnProfitRate = $(this).find(".spnProfitRate");
                var txtProfitRate = $(this).find(".txtProfitRate");

                $(spnProfitRate).text(ProfitRate + '%');
                $(txtProfitRate).val(ProfitRate);

                

                var OverHead = (TotalCost * OverheadRate / 100);
                var Profit = ((TotalCost + OverHead) * ProfitRate / 100); //#ProfitCC

                var TotalPrice = TotalCost + OverHead + Profit;


                /**
                * Total Cost
                * */
                var spnTotalCost = $(this).find('.spnTotalCost');
                $(spnTotalCost).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtTotalCost = $(this).find('.txtTotalCost');
                $(txtTotalCost).val(TotalPrice.toFixed(2));

                /**
                * Total Price
                * */
                var spnTotalPrice = $(this).find('.spnTotalPrice');
                $(spnTotalPrice).text(Currency + TotalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtTotalPrice = $(this).find('.txtTotalPrice');
                $(txtTotalPrice).val(TotalPrice.toFixed(2));
                /**
                * Profit
                * */
                var spnProfit = $(this).find('.spnProfit');
                $(spnProfit).text(Currency + Profit.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtProfit = $(this).find('.txtProfit');
                $(txtProfit).val(Profit.toFixed(2));
                /**
                * OverHead
                * */
                var spnOverhead = $(this).find('.spnOverhead');
                $(spnOverhead).text(Currency + OverHead.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                var txtOverhead = $(this).find('.txtOverhead');
                $(txtOverhead).val(OverHead.toFixed(2));

                /*
                 *Overhead rate
                 */
                var spnOverheadRate = $(this).find(".spnOverheadRate");
                var txtOverheadRate = $(this).find(".txtOverheadRate"); 
                $(spnOverheadRate).text(OverheadRate + '%');
                $(txtOverheadRate).val(OverheadRate);


                CalculateNewAmount();
            }
        });
    });



    $("#Invoice_DiscountType").on('change', function () {
        CalculateNewAmount();
    });
     
    $('#Invoice_ShippingCost').focusout(function () {
        CalculateNewAmount();
    });
    
    $('#Invoice_Deposit').focusout(function () {
        CalculateNewAmount();
    });
    $("#taxType").change(function () {
        if ($("#taxType").val() == "") {
            $(".tax_amount").text(Currency + "0.00");
            CalculateNewAmount();
        }
        else {
            CalculateNewAmount();
        }

    });
    $("#ServicetaxType").change(function () {
        if ($("#ServicetaxType").val() == "") {
            $(".Servicetax_amount").text(Currency + "0.00");
            CalculateNewAmount();
        }
        else {
            CalculateNewAmount();
        }

    }); 
    $("#OneTimeServicetaxType").change(function () {
        if ($("#OneTimeServicetaxType").val() == "") {
            $(".OneTimeServicetax_amount").text(Currency + "0.00");
            CalculateNewAmount();
        }
        else {
            CalculateNewAmount();
        }

    }); 
    
    /*$("select.dropdown-search").select2();*/


    /*
     Service Table
     */


    $("#CustomerServiceTable tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        } 
        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {

            $("#CustomerServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        InitRowIndexService();
    });
    $("#CustomerOneTimeServiceTable tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {

            $("#CustomerOneTimeServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerOneTimeServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        InitRowIndexOneTimeService();
    });
    $("#CustomerServiceTable tbody").on('click', 'tr:last', function (e) { 
        $("#CustomerServiceTable tbody tr:last").after(NewServiceRow);
        InitRowIndexService();
    });
    $("#CustomerOneTimeServiceTable tbody").on('click', 'tr:last', function (e) {
        $("#CustomerOneTimeServiceTable tbody tr:last").after(NewOneTimeServiceRow);
        InitRowIndexOneTimeService();
    });
    $("#CustomerServiceTable tbody").on('click', 'tr td i.fa.fa-trash-o', function (e) {
        var ThisObject = $(this); 
        var ThisRow = ThisObject.parent().parent();
        var ProductName = $(ThisRow).find("input.ProductName").val();
        //if (ProductName.trim() != "") {
        //    RemovedEquipmentList = RemovedEquipmentList.concat(ProductName);
        //}
        ThisObject.parent().parent().remove(); 
        if ($("#CustomerServiceTable tbody tr").length < 2) {
            $("#CustomerServiceTable tbody tr:last").after(NewServiceRow);
        } 
        CalculateNewAmount();
        //if (typeof (ThisObject.attr('data-id')) != "undefined" && ThisObject.attr('data-id') != null && ThisObject.attr('data-id') != "") {
        //    CustomerCreditForTicketInvoice(ThisObject.attr('data-id'));
        //}
    });
    $("#CustomerOneTimeServiceTable tbody").on('click', 'tr td i.fa.fa-trash-o', function (e) {
        var ThisObject = $(this);
        var ThisRow = ThisObject.parent().parent();
        var ProductName = $(ThisRow).find("input.ProductName").val();
        //if (ProductName.trim() != "") {
        //    RemovedEquipmentList = RemovedEquipmentList.concat(ProductName);
        //}
        ThisObject.parent().parent().remove();
        if ($("#CustomerOneTimeServiceTable tbody tr").length < 2) {
            $("#CustomerOneTimeServiceTable tbody tr:last").after(NewOneTimeServiceRow);
        }
        CalculateNewAmount();
        //if (typeof (ThisObject.attr('data-id')) != "undefined" && ThisObject.attr('data-id') != null && ThisObject.attr('data-id') != "") {
        //    CustomerCreditForTicketInvoice(ThisObject.attr('data-id'));
        //}
    });
    $(".div-body-contents,.OtherInfos").click(function () {
        $(".CustomerEstimateTab tr").removeClass("focusedItem");
        $(".CustomerInvoiceTab tr").removeClass("focusedItem"); 
        $(".CustomerOneTimeServiceInvoiceTab tr").removeClass("focusedItem"); 

    });
    CalculateNewAmount();
});
$(window).resize(function () {

    $(".top_to_bottom_modal_container").height(window.innerHeight);
    setTimeout(function () {
        $(".estimator_contents_scroll").height(window.innerHeight - 90);
    }, 500);
   
    

});