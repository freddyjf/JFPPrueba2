(function () {
    angular.module("precedente")
    .factory("analisisConsultadosFactory", ["$http", function ($http) {
        return {
            ObtenerAnalisis: function (page) {

                return $http.get("/AnalisisConsultados/" + page + "?noCache=" + new Date().getTime(), { cache: false });
            }
        }
    }]);
})();