
    var NewSalesLoad = function (date, month, year) {
        OpenRightToLeftModal(domainurl + "/Sales/AddSales?id=0&customerid=" + customerId + "&Date=" + date + "&Month=" + month + "&Year=" + year)
        //$(".Left-Modal-Open").click()
        ////$("#NewSales").load("/Sales/AddSales/");
        //$("#NewSales").load();
    }
   
    $(document).ready(function () {
        parent.$('.close').click(function () {
            parent.$(".modal-body").html('');
        })
        $(".SalesAppoinmentCalender").load(domainurl + "/Customer/CustomerAppoinments/?Id=" + customerId + "&Parent=SalesAppoinmentCalender");
        
})
