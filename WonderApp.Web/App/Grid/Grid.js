/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/knockout.kogrid/ko-grid.d.ts" />
var Grid = (function () {
    function Grid() {
        var _this = this;
        this.myWonders = ko.observableArray();
        this.allWonders = ko.observableArray();
        this.filterOptions = new FilterOptions();
        this.pagingOptions = new PagingOptions();
        this.getMyWonders = function (pageSize, page) {
            if (_this.allWonders().length > 0) {
                _this.setPagingData(_this.allWonders(), page, pageSize);
            } else {
                setTimeout(function () {
                    $.ajax({
                        type: "GET",
                        url: "/api/wonder/all",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        statusCode: {
                            200: function (data) {
                                _this.allWonders(data);
                                _this.setPagingData(_this.allWonders(), page, pageSize);
                            },
                            500: function (error) {
                                alert(error);
                            }
                        }
                    });
                }, 100);
            }
        };
        this.setPagingData = function (data, page, pageSize) {
            var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
            _this.myWonders(pagedData);
            _this.pagingOptions.totalServerItems(data.length);
        };
        this.filterOptions.filterText.subscribe(function (data) {
            _this.getMyWonders(_this.pagingOptions.pageSize(), _this.pagingOptions.currentPage());
        });

        this.pagingOptions.pageSizes.subscribe(function (data) {
            _this.getMyWonders(_this.pagingOptions.pageSize(), _this.pagingOptions.currentPage());
        });

        this.pagingOptions.pageSize.subscribe(function (data) {
            _this.getMyWonders(_this.pagingOptions.pageSize(), _this.pagingOptions.currentPage());
        });

        this.pagingOptions.totalServerItems.subscribe(function (data) {
            _this.getMyWonders(_this.pagingOptions.pageSize(), _this.pagingOptions.currentPage());
        });

        this.pagingOptions.currentPage.subscribe(function (data) {
            _this.getMyWonders(_this.pagingOptions.pageSize(), _this.pagingOptions.currentPage());
        });

        this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        this.gridOptions = {
            data: this.myWonders,
            enablePaging: true,
            pagingOptions: this.pagingOptions,
            filterOptions: this.filterOptions
        };
    }
    return Grid;
})();

var FilterOptions = (function () {
    function FilterOptions() {
        this.filterText = ko.observable("");
        this.useExternalFilter = true;
    }
    return FilterOptions;
})();

var PagingOptions = (function () {
    function PagingOptions() {
        this.pageSizes = ko.observableArray([5, 10, 15]);
        this.pageSize = ko.observable(5);
        this.totalServerItems = ko.observable(0);
        this.currentPage = ko.observable(1);
    }
    return PagingOptions;
})();

var GridOptions = (function () {
    function GridOptions() {
    }
    return GridOptions;
})();
//# sourceMappingURL=Grid.js.map
