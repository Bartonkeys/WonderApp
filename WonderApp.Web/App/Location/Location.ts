/// <reference path="../../scripts/typings/google/google.maps.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jqueryui/jqueryui.d.ts" />

    function initializeLocationTab() {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = 'https://maps.googleapis.com/maps/api/js?v=3.exp&' +
        'callback=initializeMap';
        document.body.appendChild(script);
    }

    function initializeMap() {
        var viewModel = new LocationViewModel($('#map_canvas')[0]);

        $("#removeLocation").click(function () { viewModel.removeLocation() });
    }

    class LocationViewModel {

        map: google.maps.Map;
        marker: google.maps.Marker;
        geocoder: google.maps.Geocoder;
        latitudeField: JQuery;
        longitudeField: JQuery;
        _self: LocationViewModel;

        constructor(element: Element) {

            this.latitudeField = $('#Latitude');
            this.longitudeField = $('#Longitude');

            var myOptions = {
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            this.geocoder = new google.maps.Geocoder();

            this.map = new google.maps.Map(element, myOptions);

            this.findInitialLocation();

            google.maps.event.addListener(this.map, 'click', (e) => {
                this.placeMarker(e.latLng);
            });

            $("#searchLocation").click((event) => {
                this.findAddressFromSearch();
            });

            $("#searchLocation, #locationAddress").keypress((event) => {
                if (event.which == 13) {
                    this.findAddressFromSearch();
                }
            });
        }

        findInitialLocation = () => {
            var latitude = parseFloat(this.latitudeField.val().toString());
            var longitude = parseFloat(this.longitudeField.val().toString());

            var location: google.maps.LatLng;

            // Use existing location
            if (latitude != 0 && longitude != 0) {
                location = new google.maps.LatLng(latitude, longitude);
                this.setInitialLocation(location, true);
                return;
            }

            this.findUsersLocationOrDefault();

        };

        findUsersLocationOrDefault = () => {
            var model = this;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    function (position) {
                        var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                        model.setInitialLocation(pos);
                    },
                    function () {
                        model.setInitialLocation();
                    });
            }
            else {
                this.setInitialLocation();
            }
        };

    setInitialLocation = (location?: google.maps.LatLng, placeMarker?: boolean) => {
            if (typeof (location) == 'undefined') {
                //default location is san fran yoda statue
                location = new google.maps.LatLng(37.7988548822927, -122.45047029772684);
            }

            this.marker = new google.maps.Marker({
                map: this.map,
                draggable: true
            });

            google.maps.event.addListener(this.marker, 'dragend', (e) => {
                this.enddrag(e.latLng);
            });

            this.map.setCenter(location);

            if (placeMarker === true) {
                this.marker.setPosition(location);
                this.updateLatLongFields(location, false);
            }
        };

        updateLatLongFields = (position, triggerChange) => {
            this.latitudeField.val(position.lat().toString());
            this.longitudeField.val(position.lng().toString());

            if (triggerChange) {
                this.latitudeField.change();
            }

            $("#removeLocation").show();
        };

        placeMarker = (position) => {
            this.marker.setPosition(position);
            this.map.panTo(position);
            this.updateLatLongFields(position, true);
        };

        enddrag = (position) => {
            this.map.panTo(position);
            this.updateLatLongFields(position, true);
        };

        findAddressFromSearch = () => {
            var searchAddress = $('#DealModel_Location_Name').val();
            if (searchAddress != "") {
                this.geocoder.geocode({ 'address': searchAddress }, (results, status) => {
                    if ((status == google.maps.GeocoderStatus.OK) && (results.length >= 0)) {
                        this.map.setCenter(results[0].geometry.location);
                        this.marker.setPosition(results[0].geometry.location);
                        this.updateLatLongFields(results[0].geometry.location, true);
                    }
                    else {
                        alert("Address not found");
                    }
                });
            }
            else {
                alert("Please enter an address");
            }
        };

        removeLocation = () => {
            this.marker.setPosition(null);
            this.latitudeField.val('');
            this.longitudeField.val('');
            $("#removeLocation").hide();
        };
}