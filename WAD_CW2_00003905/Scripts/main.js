(function () {
    angular
        .module("app", [])
        .controller("Controller", function ($scope, $http) {
            $scope.data = [];
            $http.get("/api/rest/getLogs").then(function (response) {
                $scope.data = response.data;
            });
        });
})()