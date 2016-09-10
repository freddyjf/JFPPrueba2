(function () {
    angular.module("precedente")
        .controller("notificacionesMiniController", ["$scope", "$rootScope", "notificacionesFactory", "filterFilter", function ($scope, $rootScope, notificacionesFactory, $filter) {
            var $dropdown = $("#notificacionesMini");
            $scope.data = [];

            $scope.start = 0;
            $scope.end = 5;

            $scope.leida = function (notificacion) {
                if (!!notificacion.Leido) { return; }

                notificacion.Leido = true;

                notificacionesFactory.MarcarTodasLeidas();

                $rootScope.$broadcast("rn", notificacion.Id);
            };

            $scope.numNotificaciones = function () {
                return $scope.data.NumNotificacionesNoLeidas;
            };

            function doRefresh() {
                notificacionesFactory.ObtenerNotificaciones($scope.start, $scope.end).then(function (response) {
                    $scope.data = response.data;
                });
            }

            $scope.$on("rn-mini", function (event, idNotificacion) {
                var notificacion = ($filter($scope.data.Notificaciones, function (value) { return value.Id === idNotificacion }))[0];
                if (!notificacion) { return; }
                notificacion.Leido = true;
            });

            $dropdown.on("show.bs.dropdown hide.bs.dropdown", function () {
                doRefresh();
            });

            doRefresh();
        }]);
})();