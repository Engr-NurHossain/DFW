$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();

    $(".ListViewLoader").show();
    setTimeout(function () {
        $(".ListContents").load(domainurl + "/Customer/CustomerSystemNoPrefixListPartial");
        //$(".ListContents").slideDown();
    }, 500);
    $("#AddCustomerSystemNoPrefix").click(function () {
        OpenRightToLeftModal(domainurl + "/Customer/AddCustomerSystemNoPrefix");
    });
    $(".back-to-cus-no-partial").click(function () {
        LoadCustomerSystemNo();
    })
});

