/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/knockout.kogrid/ko-grid.d.ts" />


class Grid {

    myWonders: KnockoutObservableArray<any> = ko.observableArray();
    allWonders: KnockoutObservableArray<any> = ko.observableArray();
    filterOptions: FilterOptions = new FilterOptions();
    pagingOptions: PagingOptions = new PagingOptions();
    gridOptions: GridOptions;// = new GridOptions();

    constructor() {

        this.filterOptions.filterText.subscribe((data) => {
            this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        });

        this.pagingOptions.pageSizes.subscribe((data) => {
            this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        });

        this.pagingOptions.pageSize.subscribe((data) => {
            this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        });

        this.pagingOptions.totalServerItems.subscribe((data) => {
            this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        });

        this.pagingOptions.currentPage.subscribe((data) => {
            this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        });

        this.getMyWonders(this.pagingOptions.pageSize(), this.pagingOptions.currentPage());
        this.gridOptions = {
            data: this.myWonders,
            enablePaging: true,
            pagingOptions: this.pagingOptions,
            filterOptions: this.filterOptions
        };
    }

    getMyWonders = (pageSize, page) => {

        if (this.allWonders().length > 0) {
            this.setPagingData(this.allWonders(), page, pageSize);
        }
        else {
            setTimeout(() => {
                $.ajax({
                    type: "GET",
                    url: "/api/wonder/all",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    statusCode: {
                        200: (data) => {
                            this.allWonders(data);
                            this.setPagingData(this.allWonders(), page, pageSize);
                        },
                        500: (error) => {
                            alert(error);
                        }
                    }
                });
            }, 100);
        }
    }

    setPagingData = (data, page, pageSize) => {
        var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        this.myWonders(pagedData);
        this.pagingOptions.totalServerItems(data.length);
    }
}

class FilterOptions {
    filterText: KnockoutObservable<string> = ko.observable("");
    useExternalFilter: boolean = true;
}

class PagingOptions {
    pageSizes: KnockoutObservableArray<number> = ko.observableArray([5, 10, 15]);
    pageSize: KnockoutObservable<number> = ko.observable(5);
    totalServerItems: KnockoutObservable<number> = ko.observable(0);
    currentPage: KnockoutObservable<number> = ko.observable(1);
}

class GridOptions {
    data: KnockoutObservableArray<any>;// = ko.observableArray();
    enablePaging: boolean;// = true;
    pagingOptions: PagingOptions;// = new PagingOptions();
    filterOptions: FilterOptions;// = new FilterOptions();
}

