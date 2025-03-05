var LoadcustomerList = function () {
	setTimeout(function () {
		$(".ListContents").hide();
		$(".ListViewLoader").show();
		//$(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
		$(".ListContents").load(domainurl + "/Customer/CustomersListLitePartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
	}, 500);
}
$(document).ready(function () {
	$(".LoaderWorkingDiv").hide();
	$("#LoadManufacturers").addClass("active");
	var customerpopwinowwith = 920;
	var customerpopwinowheight = 600;
	if (Device.All()) {
		customerpopwinowwith = window.innerWidth;
		customerpopwinowheight = window.innerHeight;
	}
	var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: customerpopwinowwith, height: customerpopwinowheight }
	];
	jQuery.each(idlist, function (i, val) {
		magnificPopupObj(val);
	});
	$(".ListViewLoader").show();
	$("#successmessageClose").click(function () {
		setTimeout(function () {
			//$(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
			$(".ListContents").load(domainurl + "/Customer/CustomersListLitePartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
			//$(".ListContents").slideDown();
		}, 200);
	});

	console.log('CustomerLite.Js 3')
	//$(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
	$(".ListContents").load(domainurl + "/Customer/CustomersListLitePartial?firstdate=" + firstdate + "&lastdate=" + lastdate);

});