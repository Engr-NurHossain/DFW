var searchresultstringZip;
var LoadCityState = function (zipval) {
    console.log("r");
    $.ajax({
        url: domainurl + "/Leads/GetCityStateZipListByKey",
        data: {
            key: zipval
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var searchresultstringState;
            var searchresultstringCity;
            var resultparse = JSON.parse(data.result);
            if (resultparse.length > 0) {
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstringState = resultparse[i].State;
                    searchresultstringCity = resultparse[i].City;
                    searchresultstringZip = resultparse[i].ZipCode;
                    $("#City").val(searchresultstringCity);
                    $("#State").val(searchresultstringState);
                }
            }
        }
    });
}
function initialize() { 
    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var latLongDb = position.coords.latitude + "," + position.coords.longitude;
            console.log("lat  : " + position.coords.latitude + " long " + position.coords.longitude);

            var google_maps_geocoder = new google.maps.Geocoder();
            google_maps_geocoder.geocode(
                { 'latLng': pos },
                function (results, status) {
                    var arrAddress = results[0].address_components;
                    var itemRoute = '';
                    var itemLocality = '';
                    var itemCountry = '';
                    var itemPc = '';
                    var itemSnumber = '';


                    // iterate through address_component array
                    $.each(arrAddress, function (i, address_component) {
                        console.log('address_component:' + i);

                        if (address_component.types[0] == "route") {
                            console.log(i + ": route:" + address_component.long_name);
                            itemRoute = address_component.long_name;
                            $("#Street").val(itemSnumber + ' ' + address_component.long_name);
                            $("#Latlng").val(latLongDb);
                            
                        }

                        if (address_component.types[0] == "locality") {
                            console.log("town:" + address_component.long_name);
                            itemLocality = address_component.long_name;
                            $("#City").val(address_component.long_name)
                        }

                        if (address_component.types[0] == "country") {
                            console.log("country:" + address_component.long_name);
                            itemCountry = address_component.long_name;

                            $("#Country").val(address_component.long_name);
                        }

                        if (address_component.types[0] == "postal_code" || address_component.types[0] == "postal_code_prefix") {
                            console.log("pc:" + address_component.long_name);
                            itemPc = address_component.long_name;
                            $("#ZipCode").val(address_component.long_name);
                        }

                        if (address_component.types[0] == "street_number") {
                            console.log("street_number:" + address_component.long_name);
                            itemSnumber = address_component.long_name;
                            $("#Street").val(address_component.long_name + ' ' + itemRoute);
                        }
                        //return false; // break the loop   
                    });
                    if ($("#ZipCode").val() != '')
                        LoadCityState($("#ZipCode").val());
                }
            );

        }, function () {
            console.log('map error');
        });
    } else {
        // Browser doesn't support Geolocation 
        console.log('map error');
    }
}

function loadScript() {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'https://maps.googleapis.com/maps/api/js?key=' + GoogleMapAPIKey +
        '&signed_in=true&callback=initialize';
    document.body.appendChild(script);
}
if ($("#Geo").val() == "true") {
    if ($("#Street").val() == "" && $("#ZipCode").val() == "" && $("#State").val() == "" && $("#City").val() == "") {
        function addGoogleMap() {
            if (typeof google == 'undefined') {
                loadScript();
            }
            else {
                initialize();
            }
        }
        addGoogleMap();
    }
}
