/// <reference path="../../scripts/typings/google/google.maps.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jqueryui/jqueryui.d.ts" />
var locationDivId = '#DealModel_Location_Name';
function initializeLocationTab(divId) {
    locationDivId = divId;
    if (document.getElementById('locationScript') == null) {
        var script = document.createElement('script');
        script.setAttribute('id', 'locationScript');
        script.type = 'text/javascript';
        script.src = 'https://maps.googleapis.com/maps/api/js?v=3.exp&' + 'callback=initializeMap';
        document.body.appendChild(script);
    }
}

function initializeMap() {
    var viewModel = new LocationViewModel($('#map_canvas')[0]);
    $("#removeLocation").click(function () {
        viewModel.removeLocation();
    });
}

var LocationViewModel = (function () {
    function LocationViewModel(element) {
        var _this = this;
        this.findInitialLocation = function () {
            var latitude = parseFloat(_this.latitudeField.val().toString());
            var longitude = parseFloat(_this.longitudeField.val().toString());

            var location;

            // Use existing location
            if (Number(latitude) && Number(longitude)) {
                location = new google.maps.LatLng(latitude, longitude);
                _this.setInitialLocation(location, true);
                return;
            }

            _this.findUsersLocationOrDefault();
        };
        this.findUsersLocationOrDefault = function () {
            var model = _this;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                    model.setInitialLocation(pos);
                }, function () {
                    model.setInitialLocation();
                });
            } else {
                _this.setInitialLocation();
            }
        };
        this.setInitialLocation = function (location, placeMarker) {
            if (typeof (location) == 'undefined') {
                //default location is san fran yoda statue
                location = new google.maps.LatLng(37.7988548822927, -122.45047029772684);
            }

            _this.marker = new google.maps.Marker({
                map: _this.map,
                draggable: true
            });

            google.maps.event.addListener(_this.marker, 'dragend', function (e) {
                _this.enddrag(e.latLng);
            });

            _this.map.setCenter(location);

            if (placeMarker === true) {
                _this.marker.setPosition(location);
                _this.updateLatLongFields(location, false);
            }
        };
        this.updateLatLongFields = function (position, triggerChange) {
            _this.latitudeField.val(position.lat().toString());
            _this.longitudeField.val(position.lng().toString());

            if (triggerChange) {
                _this.latitudeField.change();
            }

            $("#removeLocation").show();
        };
        this.placeMarker = function (position) {
            _this.marker.setPosition(position);
            _this.map.panTo(position);
            _this.updateLatLongFields(position, true);
        };
        this.enddrag = function (position) {
            _this.map.panTo(position);
            _this.updateLatLongFields(position, true);
        };
        this.findAddressFromSearch = function () {
            var searchAddress = $(locationDivId).val();
            if (searchAddress != "") {
                _this.geocoder.geocode({ 'address': searchAddress }, function (results, status) {
                    if ((status == google.maps.GeocoderStatus.OK) && (results.length >= 0)) {
                        _this.map.setCenter(results[0].geometry.location);
                        _this.marker.setPosition(results[0].geometry.location);
                        _this.updateLatLongFields(results[0].geometry.location, true);
                    } else {
                        alert("Address not found");
                    }
                });
            } else {
                alert("Please enter an address");
            }
        };
        this.removeLocation = function () {
            _this.marker.setPosition(null);
            _this.latitudeField.val('');
            _this.longitudeField.val('');
            $("#removeLocation").hide();
        };
        this.latitudeField = $('#Latitude');
        this.longitudeField = $('#Longitude');

        var myOptions = {
            zoom: 15,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        this.geocoder = new google.maps.Geocoder();

        this.map = new google.maps.Map(element, myOptions);

        this.findInitialLocation();

        google.maps.event.addListener(this.map, 'click', function (e) {
            _this.placeMarker(e.latLng);
        });

        $("#searchLocation").click(function () {
            _this.findAddressFromSearch();
        });

        $("#searchLocation").keypress(function (event) {
            if (event.which == 13) {
                _this.findAddressFromSearch();
            }
        });
    }
    return LocationViewModel;
})();
//# sourceMappingURL=Location.js.map
