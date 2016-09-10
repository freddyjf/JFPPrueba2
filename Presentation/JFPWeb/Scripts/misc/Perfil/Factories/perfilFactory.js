(function () {
    angular.module("precedente")
    .factory("perfilFactory", ["$http", function ($http) {
        return {
            CambiarClave: function (claveActual, claveNueva, confirmarClaveNueva) {
                return $http.post("/Perfil/ChangePassword", { claveActual: claveActual, claveNueva: claveNueva, confirmarClaveNueva: confirmarClaveNueva });
            },
            GuardarNotificaciones: function (notificaciones, frecuencia) {
                return $http.post("/Perfil/GuardarNotificaciones", { notificaciones: notificaciones, frecuencia: frecuencia });
            }
        }
    }]);
})();