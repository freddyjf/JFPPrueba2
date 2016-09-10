(function () {
    angular.module("precedente")
    .directive("analisisConsultados", function () {
        return {
            templateUrl: "analisis_consultados",
            link: function () { },
            controller: ["$scope", "analisisConsultadosFactory", function ($scope, analisisConsultadosFactory) {
                $scope.index = 1;
                $scope.consultados = [];
                $scope.noTieneConsultados = false;
                $scope.tieneConsultados = true;

                $scope.actualizar = function (symbol) {
                    if (symbol === "+") { ++$scope.index; } else { --$scope.index; }

                    if ($scope.index < 1)
                        ++$scope.index;

                    $scope.fill();
                };

                $scope.fill = function (firstLoad) {
                    
                    analisisConsultadosFactory.ObtenerAnalisis($scope.index).then(function (response) {

                        var _mainData = response.data;
                        var _hasResults = _mainData.tieneMasRegistros;
                        var _data = _mainData.data;

                        if (!!firstLoad) {
                            $scope.consultados = _data;
                            $scope.tieneConsultados = _hasResults;
                            $scope.noTieneConsultados = _data.length === 0;
                        } else {
                            if (_data.length === 0)
                                --$scope.index;
                            else {
                                $scope.consultados = _data;
                                $scope.tieneConsultados = _hasResults;
                            }
                        }
                    });
                };

                $scope.fill(true);
            }]
        }
    });
})();