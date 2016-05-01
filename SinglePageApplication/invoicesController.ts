/// <reference path='angular.d.ts' />  
/// <reference path='model.ts' />

module invoicesApp {
    export interface Scope {
        newInvoiceId: number;
        newInvoiceDescription: string;
        newInvoiceCustomer: string;
        Invoices: Model.Invoice[];
        addNewInvoice: Function;
    }

    export class Controller {
        private baseUrl: string;
        private httpService: any;
        constructor($scope: Scope, $http: any) {
            this.baseUrl = "http://localhost:8080";

            this.httpService = $http;

            this.refreshInvoices($scope);

            var controller = this;

            $scope.addNewInvoice = function () {
                var newInvoice = new Model.Invoice();
                newInvoice.Id = $scope.newInvoiceId;
                newInvoice.Description = $scope.newInvoiceDescription;
                newInvoice.Customer = $scope.newInvoiceCustomer;

                controller.addInvoice(newInvoice, function () {
                    controller.getAllInvoices(function (data) {
                        $scope.Invoices = data;
                    }, function (error) {
                        alert(error);
                    });
                });
            };

        }

        getAllInvoices(successCallback: Function, errorCallback: Function): void {
            this.httpService.get(this.baseUrl + '/api/invoices', {
                headers: { 'Origin': 'http://localhost' }
            })
                .success(function (data, status) {
                    successCallback(data);
                })
                .error(function (data, status) {
                    errorCallback(status);
                });
        }

        addInvoice(Invoice: Model.Invoice, successCallback: Function): void {
            this.httpService.post(this.baseUrl + '/api/invoices', Invoice, {
                headers: { 'Origin': 'http://localhost' }
            }).success(function () {
                successCallback();
            });
        }

        deleteInvoice(InvoiceId: string, successCallback: Function): void {
            this.httpService.delete(this.baseUrl + '/api/Invoices/' + InvoiceId, {
                headers: { 'Origin': 'http://localhost' }
            }).success(function () {
                successCallback();
            });
        }

        refreshInvoices(scope: Scope) {
            this.getAllInvoices(function (data) {
                scope.Invoices = data;
            }, function (error) {
                alert(error);
            });
        }
    }
}