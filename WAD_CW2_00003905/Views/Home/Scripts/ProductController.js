(function (app) {
    var ProductController = function ($scope, $http) {
        $http.get("/api/EmailApi/EmailExsists")
        .success(function (data) {
            $scope.products.data;
        });
    };
    app.controller("ProductController", ProductController);
}(angular.module("products")));