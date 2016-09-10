(function () {
    angular.module("precedente")
    .controller("lineasController", ["$scope", "busquedasFactory", function ($scope, busquedasFactory) {

        //#region Constructor

        // Se cargan los datos en la primera carga de la página.
        var $modelTag = $("#model");
        var data = JSON.parse($modelTag.text());

        // Se cargan los datos para ordenar en la primera carga de la página.
        var $orderTag = $("#order");
        var dataOrder = JSON.parse($orderTag.text());

        // Se remueven los tags con la información de la primera carga de la página.
        $modelTag.remove();
        $orderTag.remove();
        //#endregion

        //#region Properties

        // Resultados de búsqueda. (object)
        $scope.data = data;

        //  Delimitador para los temas y patrones fácticos.
        $scope.joinBy = ", ";

        // Lista de los posibles tipos de ordenamiento. (object)
        $scope.dataOrder = dataOrder;

        // 1 = por año, 2 = por rango de fechas. (number)
        $scope.porFecha = 1;

        // Cantidad de resultados para mostrar en la lista de años. (number)
        $scope.verTodos = 5;

        // El orden actual de los resultados. (string)
        $scope.currentOrder = dataOrder.Option[0].Text;

        // Objeto que alimenta los multiselect del tipoDocumento. (oms)
        $scope.TipoDocumento = {};

        // Objeto que alimenta los multiselect de años. (oms)
        $scope.ByYear = {};

        // Objeto para realizar las consultas al servidor. (object)
        $scope.busquedaModel = {
            // Permite filtrar por linea. (number)
            Linea: parseInt($("#linea").text()),
            // Página actual. (number)
            Pagina: 1,
            // Criterio para ordenar los resultados. (string)
            Orden: dataOrder.Option[0].Value,
            // Termino para buscar en los resultados. (string)
            Termino: data.Termino,
            // Filtro por temas. (array<string>)
            Temas: [],
            // Filtro por patrones fácticos. (array<string>)
            Patrones: [],
            // Filtro por años. (array<string>)
            ByYear: [],
            // Filtro por tipos de documento. (array<string>)
            TipoDocumento: [],
            // Filtro por entidad. (array<string>)
            Entidad: [],
            // Filtro por rango de fechas. (datetime)
            FechaInicio: null,
            FechaFin: null
        };

        // Configuración de los plugins de rango de fechas.
        $scope.datepickers = {
            one: {
                format: "dd/MM/yyyy",
                maxDate: new Date(),
                value: new Date().addMonths(-1),
                visible: false
            },
            two: {
                format: "dd/MM/yyyy",
                maxDate: new Date(),
                value: new Date(),
                visible: false
            },
            formatPlaceholder: "dd/mm/aaaa"
        };

        // Mostrar todos para la búsqueda por años.
        $scope.mostrarTodos = false;

        //#endregion

        //#region private methods

        // Permite realizar búsquedas al servidor con el objeto $scope.busquedaModel.
        var doSearch = function (nombreUso) {

            if (!!nombreUso) {
                callRegistrarUso("AplicarFiltroResultado", ["MetaData", "[Origen:BusquedaPorLinea]" + nombreUso]);
            }

            return busquedasFactory.Consultar($scope.busquedaModel).then(function (response) {
                $scope.data = response.data;
            });
        };

        //#endregion

        // Invoca la vista del gráfico de Línea Jurisprudencial.
        $scope.irVistaGlobal = function (idLinea) {
            location.href = "/LineaJurisprudencial/ViewGraph?idLinea=" + idLinea;
        };

        // Permite configurar el orden de la búsqueda.
        $scope.setOrder = function (order) {
            var text = order.Text;
            var value = order.Value;

            // Si se hizo click, pero no ha cambiado el valor, no haga nada.
            if ($scope.busquedaModel.Orden === value)
                return;

            $scope.busquedaModel.Orden = value;

            $scope.busquedaModel.Pagina = 1;

            doSearch().then(function (response) {
                $scope.currentOrder = text;
            });
        };

        //  Evento cuando cambia la páginación.
        $scope.paginationChanged = function () {
            doSearch().then(function () {
                window.scrollTop();
            });
        };

        // Permite realizar búsquedas al servidor con el objeto $scope.busquedaModel.
        $scope.doSearch = function () {
            doSearch();
        };

        // Permite crear la consulta por entidad. Además establece checked en los checkbox que concuerden con hijo-padre.
        $scope.setEntidad = function (scope) {
            $scope.busquedaModel.Entidad = [];
            var entidadQuery = $scope.busquedaModel.Entidad;

            function removeEntidadDownwards(_scope) {
                var child = _scope.$$childHead;

                if (!!child) {
                    child.node.SecondValue = false;
                    removeEntidadDownwards(child);
                }
            }

            function setEntidadUpwards(_scope, _SecondValue) {
                var parent = _scope.$parent;
                var node = _scope.node;

                if (!node) { return; }

                _scope.node.SecondValue = _SecondValue;

                if (!!_SecondValue) {
                    entidadQuery.unshift(node.Value);
                    setEntidadUpwards(parent, _SecondValue);
                } else {
                    setEntidadUpwards(parent, true);
                }
            }

            removeEntidadDownwards(scope);
            setEntidadUpwards(scope, scope.node.SecondValue);

            doSearch("[Filtro:Entidad][Seleccion:" + scope.node.Value + "]");
        };

        // Permite limpiar o asignar valores a los campos de rango de fechas.
        $scope.cambiarFechas = function () {
            switch ($scope.porFecha) {
                case 1:
                    $scope.busquedaModel.FechaInicio = null;
                    $scope.busquedaModel.FechaFin = null;
                    doSearch();
                    break;
                case 2:
                    $scope.busquedaModel.FechaInicio = $scope.datepickers.one.value;
                    $scope.busquedaModel.FechaFin = $scope.datepickers.two.value;
                    break;
            }
        };

        // Coge el objeto multiselect (oms) y extrae las llaves que estan seleccionadas y luego realiza la búsqueda.
        $scope.doSearchWithObj = function (name) {
            var objKeys = $scope[name];

            if (!objKeys) { return; }

            $scope.busquedaModel[name] = $.map(objKeys, function (value, key) { if (value) { return key; } });
            
            doSearch("[Filtro:Año][Seleccion:" + $scope.busquedaModel[name].join(",") + "]");
        };

        $scope.temasChanged = function () {
            doSearch("[Filtro:TemasEspecificos][Seleccion:" + $scope.busquedaModel.Temas.join(",") + "]");
        };

        $scope.patronesChanged = function () {
            doSearch("[Filtro:PatronesFacticos][Seleccion:" + $scope.busquedaModel.Patrones.join(",") + "]");
        };
    }]);
})();