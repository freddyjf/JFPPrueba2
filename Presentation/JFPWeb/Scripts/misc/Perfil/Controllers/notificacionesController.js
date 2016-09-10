(function () {
    angular.module("precedente")
        .controller("notificacionesController", ["$scope", "$timeout", "perfilFactory", function ($scope, $timeout, perfilFactory) {
            $scope.tipoNotificaciones = {};
            $scope.notificacionesForm = {};
            $scope.mensaje = "";

            $scope.notificacionesModel = {
                notificaciones: [],
                frecuencia: null
            };

            function reloadNotificacions() {
                $scope.notificacionesModel.notificaciones = $.map($scope.tipoNotificaciones, function (value, key) {
                    if (value) {
                        return parseInt(key);
                    }
                    return null;
                });
                return $scope.notificacionesModel.notificaciones;
            };

            $scope.noHabilitarFrecuencia = function () {
                var notificaciones = reloadNotificacions();

                if (notificaciones.length <= 0) {
                    $scope.notificacionesModel.frecuencia = null;
                    $scope.notificacionesForm.$setPristine();
                    return true;
                }

                return false;
            };

            $scope.submit = function () {
                perfilFactory.GuardarNotificaciones(reloadNotificacions(), $scope.notificacionesModel.frecuencia)
                    .then(function (response) {
                        $scope.mensaje = response.data.Message;
                        $timeout(function() {
                            $scope.mensaje = "";
                        }, 2000);
                    });
            };
        }]);
})();