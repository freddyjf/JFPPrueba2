(function () {
    angular.module("precedente")
    .factory("busquedasFactory", ["$http", function ($http) {
        return {
            Consultar: function (busquedaModel) {
                return $http.post("/Busquedas/Data", busquedaModel);
            }
        }
    }]);
})();