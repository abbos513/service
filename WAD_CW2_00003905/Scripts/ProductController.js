(function (app) {
    var ProductController = function ($scope, $http) {
        var   x=angular.element(document.getElementById("emailTextBox"));
        $http.get("/api/EmailApi/EmailExsists?email=" + x)
        .success(function (data) {
            $scope.products.data;
        });
    };
    app.controller("ProductController", ProductController);
}(angular.module("products")));