(function () {
    angular.module("precedente")
        .controller("notificacionesController", ["$scope", "$rootScope", "notificacionesFactory", "filterFilter", function ($scope, $rootScope, notificacionesFactory, $filter) {

            //#region Constructor

            // Se cargan los datos en la primera carga de la página.
            var $modelTag = $("#model");
            var data = JSON.parse($modelTag.text());

            // Se remueven los tags con la información de la primera carga de la página.
            $modelTag.remove();
            //#endregion

            $scope.data = data;
            $scope.dataCountToDelete = 0;
            $scope.pluralizeMessageToDeleteSelected = {
                "1": "Eliminar una notificación",
                "other": "Eliminar {} notificaciones"
            };

            $scope.paginator = {
                page: 1,
                itemPerPage: $scope.data.ItemsPorPagina
            };

            var getNotificationsForDelete = function () {
                return $filter($scope.data.Notificaciones, function (item) { return item.Borrar });
            };

            $scope.leida = function (notificacion) {
                if (!!notificacion.Leido) { return; }

                notificacion.Leido = true;

                notificacionesFactory.MarcarTodasLeidas();

                $rootScope.$broadcast("rn-mini", notificacion.Id);
            };

            $scope.paginationChanged = function () {
                $scope.doSearch();
            };

            $scope.doSearch = function () {
                $scope.dataCountToDelete = 0;

                notificacionesFactory.ObtenerNotificaciones(($scope.paginator.page - 1) * $scope.paginator.itemPerPage, $scope.paginator.itemPerPage).then(function (response) {
                    $scope.data = response.data;
                });
            };

            $scope.checkForDelete = function () {
                $scope.dataCountToDelete = getNotificationsForDelete().length;
            };

            $scope.deleteSelected = function () {
                alerts.EliminarNotificacionSeleccionada(function () {
                    Spin.start();
                    notificacionesFactory.EliminarNotas(_.map(getNotificationsForDelete(), "Id")).then($scope.doSearch).finally(Spin.stop);
                });
            };

            $scope.deleteAll = function () {
                alerts.EliminarNotificaciones(function () {
                    Spin.start();
                    notificacionesFactory.EliminarTodasNotas().then($scope.doSearch).finally(Spin.stop);
                });
            };

            $scope.$on("rn", function (event, idNotificacion) {
                var notificacion = ($filter($scope.data.Notificaciones, function (value) { return value.Id === idNotificacion }))[0];
                if (!notificacion) { return; }
                notificacion.Leido = true;
            });
        }]);
})();