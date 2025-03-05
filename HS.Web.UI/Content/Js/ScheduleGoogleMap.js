
function initialize() {
    var geocoder;
    var map;
    var latlng = new google.maps.LatLng(31.616285, -99.121231);
    var myOptions = {
        //zoom: 10,
        center: latlng,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas_initial"), myOptions);
    geocoder = new google.maps.Geocoder();
    if (address.length > 0) {
        for (i = 0; i < address.length; i++) {
            var startTimeStamp = address[i].EventStartDate;
            var StartdateVal = startTimeStamp.split(' ');
            var finalStartTime = StartdateVal[1].split(':');
            var Starthours = finalStartTime[0];
            var Startminutes = finalStartTime[1];
            var ampm = finalStartTime[0] >= 12 ? 'PM' : 'AM';
            Starthours = Starthours % 12;
            Starthours = Starthours ? Starthours : 12; // the hour '0' should be '12'
            Startminutes = Startminutes < 10 ? '0' + Startminutes : Startminutes;
            var strStartTime = Starthours + ':' + Startminutes + ' ' + ampm;
            strTime = Starthours + ':' + Startminutes + ampm;
            console.log("end");
            var EndTimeStamp = address[i].EventEndDate;
            var EndDateVal = EndTimeStamp.split(' ');
            var finalEndTime = EndDateVal[1].split(':');
            var Endhours = finalEndTime[0];
            var Endminutes = finalEndTime[1];
            var ampm = finalEndTime[0] >= 12 ? 'PM' : 'AM';
            Endhours = Endhours % 12;
            Endhours = Endhours ? Endhours : 12; // the hour '0' should be '12'
            Endminutes = Endminutes < 10 ? '0' + Endminutes : Endminutes;
            var strEndTime = Endhours + ':' + Endminutes + ' ' + ampm;

            var locationDate = finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2];
            var locationUser = "Assigned - " + address[i].EventTitle;
            var locationCustomer = address[i].EventCustomerName + " - " + address[i].EventDisplayType;
            var locationaddress = address[i].EventStreet + " " + address[i].EventLocate;

            InfoMarkers.push({
                Info: '<b>' + locationDate + "<br>" + locationUser + "<br>" + locationCustomer + "<br>" + locationaddress + '</b>',
                InfoType: address[i].EventType,
                InfoColor: address[i].EventColor,
            })
            if (geocoder) {
                geocoder.geocode({ 'address': address[i].EventStreet + " " + address[i].EventLocate }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                            map.setCenter(results[0].geometry.location);
                            markers.push({
                                Lat: results[0].geometry.location.lat(),
                                Lng: results[0].geometry.location.lng(),
                            });
                        }
                        else {
                            alert("No results found");
                        }
                    }
                });
            }
        }
    }
}
function InitializeMapSchedule() {
    console.log("map initialize");
    var map;
    var infowindow = new google.maps.InfoWindow();
    var bounds = new google.maps.LatLngBounds();
    var latlng = new google.maps.LatLng(31.616285, -99.121231);
    var myOptions = {
        //zoom: 10,
        center: latlng,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    for (i = 0; i < markers.length; i++) {
        var pos = new google.maps.LatLng(markers[i].Lat, markers[i].Lng);
        var type = InfoMarkers[i].InfoType;
        var color = InfoMarkers[i].InfoColor;
        marker = new google.maps.Marker({
            position: pos,
            map: map,
            icon: {
                path: 'M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z M -2,-30 a 2,2 0 1,1 4,0 2,2 0 1,1 -4,0',
                fillColor: "#" + color,
                fillOpacity: 1,
                strokeColor: 'black',
                strokeWeight: 1,
                scale: 1
            },
        });
        bounds.extend(marker.getPosition());
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(InfoMarkers[i].Info);
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
    map.fitBounds(bounds);
}
$(document).ready(function () {
    initialize();
    setTimeout(function () {
        InitializeMapSchedule();
    }, 5000);
});