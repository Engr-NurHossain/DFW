var editGlobalSetting = function (id) {
   // console.log("clicked");
   // console.log("editId " + id);
    OpenRightToLeftModal(domainurl + "/Setup/EditSettings?id=" + id);
}
var editCustomerUiSetting = function (id) {
    //console.log("clicked");
  //  console.log("editId " + id);
    OpenRightToLeftModal(domainurl + "/Setup/EditCustomerUiSettings?id=" + id);
}
var editInventoryUiSetting = function (id) {
     // console.log("clicked");
   // console.log("editId " + id);
    OpenRightToLeftModal(domainurl + "/Setup/EditInventoryUiSettings?id=" + id);
}
var editEquipmentUiSetting = function (id) {
     // console.log("clicked");
   // console.log("editId " + id);
    OpenRightToLeftModal(domainurl + "/Setup/EditEquipmentUiSettings?id=" + id);
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })

        $(".LoaderWorkingDiv").hide();
        
        //$('#editSettings').click(function () {
            
        //    console.log("clicked");
        //    var editId = $(this).attr('idval');
        //    console.log("editId " + editId);
        //    OpenRightToLeftModal("/Setup/EditSettings?id=" + editId);
        //    //$(".background-hide-class").show();
            
        //});
        //$('#customeruibtn').click(function () {

        //    console.log("clicked");
        //    var editId = $(this).attr('idval');
        //    console.log("editId " + editId);
        //    OpenRightToLeftModal("/Setup/EditCustomerUiSettings?id=" + editId);
        //    //$(".background-hide-class").show();

        //});
    })