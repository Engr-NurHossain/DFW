var TicketSave = function (Tid) {
    var user = $("#userDropdown").val(),
        status = $("#ChangeResult").val();
    if (user != '-1' && user != '' && user != null && user != 'undefined' && status == 'Edit') {
        var Param = {
            TicketId: Tid,
            UserId: user
        };
        $.ajax({
            type: "POST",
            url: domainurl + "/Calendar/UpdateTicketUser",
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data != '' && data != null && data != 'undefined') {
                    var result = 'Assigned - ' + data;
                    var spanId = 'span_' + Tid,
                        editId = 'EventEdit_' + Tid,
                        updateId = 'EventUpdate_' + Tid;
                    $('#' + spanId).html(result);
                    $("#ChangeResult").val("Update");
                    $('#' + editId).show();
                    $('#' + updateId).hide();
                }
            }
        });
    }
    else {
        return false;
    }
}
var TicketEdit = function (Tid) {
    var html = '<span>Assigned - <select class="form-control" id="userDropdown" name="userDropdown"></select></span>';
    var spanId = 'span_' + Tid,
        editId = 'EventEdit_' + Tid,
        updateId = 'EventUpdate_' + Tid,
        innerHtml = $("#" + spanId).html();
    $('#' + spanId).html(html);
    $("#htmlValue").val(innerHtml);
    $("#ChangeResult").val("Edit");
    $('#' + editId).hide();
    $('#' + updateId).show();
    $.ajax({
        type: "GET",
        url: domainurl + "/Calendar/GetAllTechList",
        data: "{}",
        success: function (result) {
            if (result.length > 0) {
                var options = '<option value="-1">Select a user</option>';
                for (var i = 0; i < result.length; i++) {
                    options += '<option value="' + result[i].Value + '">' + result[i].Text + '</option>';
                }
                $("#userDropdown").html(options);
            }
        }
    });
}

function initialize() {
    var map;
    var latlng = new google.maps.LatLng(31.616285, -99.121231);
    var myOptions = {        
        center: latlng,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_initial"), myOptions);
    if (address.length > 0) {
        for (i = 0; i < address.length; i++) {
            var locationaddress = address[i].EventStreet + " " + address[i].EventLocate;
            if (address[i].EventType == 'Company') {
                InfoMarkers.push({
                    Info: address[i].EventResourceName,
                    InfoColor: address[i].EventColor,
                    InfoLocation: locationaddress,
                    HoverTitle: address[i].HoverTitle,
                    LabelText: '0'
                });
                markers.push({
                    Lat: address[i].EventLatitude,
                    Lng: address[i].EventLongitude
                });
            }
            else {                
                var intLength, exist = false;
                if (InfoMarkers.length > 0) {
                    for (let j = 0; j < InfoMarkers.length; j++) {
                        if (InfoMarkers[j].InfoLocation == locationaddress) {
                            exist = true;
                            intLength = j;
                        }
                    }
                }
                if (exist) {
                    InfoMarkers[intLength].Info = InfoMarkers[intLength].Info + '<hr />' + address[i].EventResourceName;
                }
                else {
                    InfoMarkers.push({
                        Info: address[i].EventResourceName,
                        InfoColor: address[i].EventColor,
                        InfoLocation: locationaddress,
                        HoverTitle: address[i].HoverTitle,
                        LabelText: address[i].EventCalendarCount
                    });
                    markers.push({
                        Lat: address[i].EventLatitude,
                        Lng: address[i].EventLongitude
                    });
                }
            }
        }
    }
}
function InitializeMapSchedule() {
    var map;
    var infowindow = new google.maps.InfoWindow();
    var bounds = new google.maps.LatLngBounds();
    var latlng = new google.maps.LatLng(31.616285, -99.121231);
    var myOptions = {
        zoom: parseInt(ZoomLevel),
        center: latlng,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("googleMapId"), myOptions);
    for (i = 0; i < markers.length; i++) {
        var pos = new google.maps.LatLng(markers[i].Lat, markers[i].Lng);
        marker = new google.maps.Marker({
            position: pos,
            map: map,
            title: InfoMarkers[i].HoverTitle,
            label: {
                text: InfoMarkers[i].LabelText,
                color: "Black",
                fontSize: '18px'
            },
            icon: {
                path: 'M0-48c-9.8 0-17.7 7.8-17.7 17.4 0 15.5 17.7 30.6 17.7 30.6s17.7-15.4 17.7-30.6c0-9.6-7.9-17.4-17.7-17.4z',
                fillColor: "#" + InfoMarkers[i].InfoColor,
                fillOpacity: 1,
                strokeColor: 'black',
                strokeWeight: 1,
                scale: 1,
                labelOrigin: { x: 0, y: -28 }
            }
        });
        bounds.extend(marker.getPosition());
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(InfoMarkers[i].Info);
                infowindow.open(map, marker);
            };
        })(marker, i));
    }
    map.fitBounds(bounds);
}
var GetAllMapData = function (uservalue, currentdate, typevalue) {
    $("#map_initial").html('');
    $("#googleMapId").html('');
    markers = [];
    InfoListMarkers = [];
    InfoMarkers = [];
    $.ajax({
        type: "POST",
        url: domainurl + "/Calendar/AllLoadMapCalendar",
        data: JSON.stringify({
            Default: uservalue,
            startdate: currentdate,
            typeval: typevalue
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Result) {
                address = data.Model;
                ZoomLevel = data.ZoomVal;
                initialize();
                setTimeout(function () {
                    InitializeMapSchedule();
                }, 5000);
                $(".LoaderWorkingDiv").hide();
            }

        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}
var CalendarSearchFilter = function () {
    var pd = $('#eventDate').val();
    var types = $("#ListTicketType").val();
    var users = $("#ListEmployee").val();
    GetAllMapData(users, pd, types);
}
var EditDateHeader = function () {
    $('#DateTilteHide').hide();
    $('#DateTitleShow').show();
}
var CloseDateHeader = function () {
    $('#DateTitleShow').hide();
    $('#DateTilteHide').show();
}
var CalendarPreviousButton = function () {
    var pd = $('#eventDate').val();
    var pred = new Date(pd);
    var zone = String(TimeZoneValue(pred));
    var firstChar = zone.charAt(0);
    if (firstChar == '-') {
        pred.setDate(pred.getDate() + 1);
    }
        pred.setDate(pred.getDate() - 1);
        var py = pred.getFullYear();
        var pm = (pred.getMonth() + 1).toString();
        if (pm.length == 1) { pm = 0 + pm; }
        var pday = (pred.getDate()).toString();
        if (pday.length == 1) { pday = 0 + pday; }
        pred = py + '-' + pm + '-' + pday;
    $('#eventDate').val(pred);
    if ($("#SelectedDateReload").val() == "true") {
        window.location.href = domainurl + '/CalendarMap?TicketDate=' + pred;
    }
    else {
        var types = $("#ListTicketType").val();
        var users = $("#ListEmployee").val();
        GetAllMapData(users, pred, types);
        $(".LoaderWorkingDiv").show();
    }
}
var CalendarNextButton = function () {
    var nd = $('#eventDate').val();
    var nxd = new Date(nd);
    var zone = String(TimeZoneValue(nxd));
    var firstChar = zone.charAt(0);
    if (firstChar == '-') {
        nxd.setDate(nxd.getDate() + 1);
    }
        nxd.setDate(nxd.getDate() + 1);
        var ny = nxd.getFullYear();
        var nm = (nxd.getMonth() + 1).toString();
        if (nm.length == 1) { nm = 0 + nm; }
        var nday = (nxd.getDate()).toString();
        if (nday.length == 1) { nday = 0 + nday; }
        nxd = ny + '-' + nm + '-' + nday;
    $('#eventDate').val(nxd);
    if ($("#SelectedDateReload").val() == "true") {
        window.location.href = domainurl + '/CalendarMap?TicketDate=' + nxd;
    }
    else {
        var types = $("#ListTicketType").val();
        var users = $("#ListEmployee").val();
        GetAllMapData(users, nxd, types);
        $(".LoaderWorkingDiv").show();
    }
}
var CalendarDateChange = function (date) {
    var selecteddate = date.value;
    if ($("#SelectedDateReload").val() == "true") {
        window.location.href = domainurl + '/CalendarMap?TicketDate=' + selecteddate;
    }
    else {
        var types = $("#ListTicketType").val();
        var users = $("#ListEmployee").val();
        GetAllMapData(users, selecteddate, types);
        $(".LoaderWorkingDiv").show();
    }
}
var OpenBkById = function (bkId, cusId) {
    if (typeof (bkId) != "undefined" && bkId > 0) {
        if (typeof (cusId) == "undefined") {
            cusId = 0;
        }
        OpenTopToBottomModal("/Booking/AddLeadBooking/?customerid=" + cusId + "&Id=" + bkId);
    }
}
function successFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    codeLatLng(lat, lng);
}
function errorFunction() {
    alert("Geocoder failed");
}
function codeLatLng(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            currentformatedAddress = results[0].formatted_address;
                            window.open("https://www.google.com/maps?saddr=" + currentformatedAddress.replace(",", "").replace(" ", "+") + "&daddr=" + DestinationCusAddress.replace(",", "").replace(" ", "+"), "_blank");
                            break;
                        }
                    }
                }
            } else {
                alert("No results found");
            }
        } else {
            alert("Geocoder failed due to: " + status);
        }
    });
}
var GetDirection = function (ev) {
    var Data = ev.currentTarget.dataset.address;
    DestinationCusAddress = Data;
    Directioninitialize();
}
var Directioninitialize = function() {
    $('.tt-menu').hide();
    geocoder = new google.maps.Geocoder();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successFunction, errorFunction);
    }
}
var TimeZoneValue = function (dt) {
    return (-dt.getTimezoneOffset() < 0 ? '-' : '+') + (Math.abs(dt.getTimezoneOffset() / 60) < 10 ? '0' : '') + (Math.abs(dt.getTimezoneOffset() / 60)) + '00';
}
$(document).ready(function () {
    console.log("test");
    $("#ListTicketType").selectpicker('val', typeval);
    $("#ListEmployee").selectpicker('val', UserVal);
    var types = $("#ListTicketType").val();
    var users = $("#ListEmployee").val();
    $('#eventDate').val(selectrdDate);
    $('#DateTilteHide').show();
    GetAllMapData(users, selectrdDate, types);
    $(".filter-class").click(function () {
        var source = $(this).attr("data-filter");
        GetAllMapData($("#ListEmployee").val(), $('#eventDate').val(), source);
    });
});