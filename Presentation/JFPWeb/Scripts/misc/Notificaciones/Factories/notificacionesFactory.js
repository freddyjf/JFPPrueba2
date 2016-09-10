(function () {
    angular.module("precedente")
    .factory("notificacionesFactory", ["$http", function ($http) {
        return {
            ObtenerNotificaciones: function (start, end) {
                return $http.get("/Notificaciones/GetAnalisisNotificaciones", { params: { start: start, end: end } });
            },
            MarcarNotificacionLeida: function (codNotificacion) {
                return $http.post("/Notificaciones/NotificacionLeida", { codNotificacion: codNotificacion });
            },
            MarcarTodasLeidas: function () {
                return $http.post("/Notificaciones/MarcarTodasLeidas");
            },
            EliminarNotas: function (idNotas) {
                if (!angular.isArray(idNotas))
                    idNotas = [idNotas];

                return $http.post("/Notificaciones/EliminarNotas", idNotas);
            },
            EliminarTodasNotas: function () {
                return $http.post("/Notificaciones/EliminarTodasNotas");
            }
        }
    }]);
})();