var wonderModule = angular.module("wonderModule", ['ngRoute', 'ui.bootstrap', 'angular-loading-bar']);

wonderModule.factory("dataService", ["$http", "$q", function ($http, $q) {

    var _wonders = [];
    var _users = [];
    var _cities = [];
    var _isInit = false;
    var _wonderModel = {};

    var _isReady = function () {
        return _isInit;
    };

    var _getWonders = function () {

        var deferred = $q.defer();
        _populateLatLong();

        $http.post("/api/app/wonders", _wonderModel)
          .then(function (result) {
              // success
              angular.copy(result.data, _wonders);
              _setDistanceForWonders();
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getPriorityWonders = function () {

        var deferred = $q.defer();
        _populateLatLong();

        $http.post("/api/app/priority", _wonderModel)
          .then(function (result) {
              // success
              angular.copy(result.data, _wonders);
              _setDistanceForWonders();
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getNearestWonders = function (radius) {

        var deferred = $q.defer();
        _populateLatLong();

        $http.post("/api/app/nearest/" + radius, _wonderModel)
          .then(function (result) {
              // success
              angular.copy(result.data, _wonders);
              _setDistanceForWonders();
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getPopularWonders = function (take, from) {

        var deferred = $q.defer();
        _populateLatLong();

        $http.post("/api/app/popular/" + take + "/" + from, _wonderModel)
          .then(function (result) {
              // success
              angular.copy(result.data, _wonders);
              _setDistanceForWonders();
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getMyWonders = function () {
        var deferred = $q.defer();
        _populateLatLong();

        $http.get("/api/app/myWonders/" + _wonderModel.userId)
          .then(function (result) {
              // success
              angular.copy(result.data, _wonders);
              _setDistanceForWonders();
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getCities = function () {
        var deferred = $q.defer();

        $http.get("/api/app/city")
          .then(function (result) {
              // success
              angular.copy(result.data, _cities);
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _getUsers = function () {

        var deferred = $q.defer();

        $http.get("/api/app/user/")
          .then(function (result) {
              // success
              angular.copy(result.data, _users);
              _isInit = true;
              deferred.resolve();
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    }

    var _like = function (wonderId) {
        var deferred = $q.defer();

        $http.get("/api/app/like/" + _wonderModel.userId + "/" + wonderId)
          .then(function (result) {
              // success   
              _wonders = $.grep(_wonders, function (o) { return o.id === wonderId; }, true);             
              deferred.resolve(_wonders);
          },
          function () {
              // error
              deferred.reject();
          });

        return deferred.promise;
    };

    var _populateLatLong = function() {
        var city = $.grep(_cities, function(e) { return e.id == _wonderModel.cityId; });
        _wonderModel.latitude = city[0].location.latitude;
        _wonderModel.longitude = city[0].location.longitude;
    };

    var _setDistanceForWonders = function () {
        $.each(_wonders, function (i, deal) {
            deal.distanceFrom = _getDistanceFromLatLonInKm(_wonderModel.latitude,
             _wonderModel.longitude, deal.location.latitude, deal.location.longitude).toFixed(2);
        });       
    };

    var _getDistanceFromLatLonInKm = function(lat1, lon1, lat2, lon2) {
        var R = 6371; // Radius of the earth in km
        var dLat = _deg2rad(lat2 - lat1); // deg2rad below
        var dLon = _deg2rad(lon2 - lon1);
        var a =
            Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(_deg2rad(lat1)) * Math.cos(_deg2rad(lat2)) *
                    Math.sin(dLon / 2) * Math.sin(dLon / 2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    };

    var _deg2rad = function(deg) {
        return deg * (Math.PI / 180)
    };

    return {
        wonders: _wonders,
        wonderModel: _wonderModel,
        users: _users,
        cities: _cities,
        isReady: _isReady,
        getWonders: _getWonders,
        getUsers: _getUsers,
        getCities: _getCities,
        getPriorityWonders: _getPriorityWonders,
        getNearestWonders: _getNearestWonders,
        getPopularWonders: _getPopularWonders,
        getMyWonders: _getMyWonders,
        like: _like
    };
}]);

wonderModule.controller("wonderController", function ($scope, $http, dataService, $location, $anchorScroll, $window) {
    $scope.data = dataService;
    $scope.isMyWonder = false;
    $scope.wonders = dataService.wonders;

    $scope.init = function () {
        dataService.getCities();
        dataService.getUsers();
    };

    $scope.getWonders = function () {
        dataService.getWonders().then(function () {
            $scope.isMyWonder = false;
        });
    };

    $scope.getPriorityWonders = function () {
        dataService.getPriorityWonders().then(function () {
            $scope.isMyWonder = false;;
        });
    };

    $scope.getNearestWonders = function (radius) {
        dataService.getNearestWonders(radius).then(function () {
            $scope.isMyWonder = false;
        });
    };

    $scope.getPopularWonders = function () {
        dataService.getPopularWonders(1, 10).then(function () {
            $scope.isMyWonder = false;
        });
    };

    $scope.getMyWonders = function () {
        dataService.getMyWonders().then(function() {
            $scope.isMyWonder = true;
        });
    };

    $scope.like = function (wonderId) {
        dataService.like(wonderId).then(function (wonders) {
            $scope.wonders = wonders;
        });
    };

})