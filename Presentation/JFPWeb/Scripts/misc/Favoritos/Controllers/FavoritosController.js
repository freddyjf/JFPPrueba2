(function () {
    angular.module("precedente")
        .controller("FavoritosController", ["$scope", "filterFilter", "favoritosFactory", function ($scope, $filter, favoritosFactory) {

            //#region Constructor

            // Se cargan los datos en la primera carga de la página.
            var $modelTag = $("#model");
            var data = JSON.parse($modelTag.text());

            // Se remueven los tags con la información de la primera carga de la página.
            $modelTag.remove();
            //#endregion

            //#region private
            var removerFavoritos = function (favoritos) {
                $scope.data.Favoritos = $filter($scope.data.Favoritos, function (favorito) {
                    return favoritos.indexOf(favorito.Id) === -1;
                });
            };

            var obtenerCarpeta = function (codCarpeta) {
                return $filter($scope.data.Carpetas, function (carpeta) {
                    return carpeta.Id === codCarpeta;
                })[0];
            };

            var obtenerFavoritos = function (codFavoritos) {
                return $filter($scope.data.Favoritos, function (favorito) {
                    return codFavoritos.indexOf(favorito.Id) > -1;
                });
            };

            var moverFavoritoDialog = function (callback) {
                var dataToMove = $.map($scope.data.Carpetas, function (value) {
                    return { text: value.Nombre, value: value.Id };
                });

                alerts.moverCarpeta(dataToMove, callback);
            };

            //#endregion

            $scope.data = data;

            $scope.selected = data.Carpetas[0];

            $scope.favoritosBorrarMover = {};

            $scope.busquedaModel = {
                Pagina: 1,
                ItemsPorPagina : 10
            };

            $scope.setSelected = function (carpeta) {
                $scope.selected = carpeta;
            };

            $scope.crearCarpeta = function () {
                alerts.nuevaCarpeta(function (nombre) {
                    favoritosFactory.CrearCarpeta(nombre).then(function (response) {
                        $scope.data.Carpetas.push(response.data);
                    }).catch(function (response) {
                        alerts.mensaje(response.data.Message, function () {
                            $scope.crearCarpeta();
                        });
                    });
                }).on("modal.bs.canceled", function() {
                    callRegistrarUso("CancelarCrearCarpetaFavoritos");
                });
            };

            $scope.renombrarCarpeta = function (carpeta) {
                if (carpeta.Nombre === carpeta.NombreOriginal) {
                    carpeta.editMode = false;
                    return;
                }

                favoritosFactory.RenombrarCarpeta(carpeta.Nombre, carpeta.Id).then(function () {
                    carpeta.editMode = false;
                    carpeta.NombreOriginal = carpeta.Nombre;
                }).catch(function (response) {
                    alerts.mensaje(response.data.Message);
                });
            };

            $scope.eliminarCarpeta = function (carpeta) {
                alerts.eliminarCarpetas(carpeta.NombreOriginal, function () {
                    favoritosFactory.EliminarCarpetas(carpeta.Id).then(function () {
                        $scope.data.Carpetas = $filter($scope.data.Carpetas, function (carpetaFiltrar) {
                            return carpetaFiltrar.Id !== carpeta.Id;
                        });
                        $scope.selected = data.Carpetas[0];
                    });
                }).on("modal.bs.canceled", function() {
                    callRegistrarUso("CancelarEliminarCarpetaFavoritos");
                });
            };

            $scope.cancelarCarpeta = function(carpeta) {
                carpeta.Nombre = carpeta.NombreOriginal;
                carpeta.editMode = false;
            };

            $scope.cancelarFavorito = function (favorito) {
                favorito.Nombre = favorito.NombreOriginal;
                favorito.editMode = false;
            };

            $scope.renombrarFavorito = function (favorito) {
                favorito.editMode = false;

                if (favorito.Nombre === favorito.NombreOriginal)
                    return;

                favoritosFactory.RenombrarFavorito(favorito.Nombre, favorito.Id).then(function () {
                    favorito.NombreOriginal = favorito.Nombre;
                });
            };

            $scope.favoritosBorrarMoverArr = function () {
                return $.map($scope.favoritosBorrarMover, function (value, key) {
                    return value ? parseInt(key) : null;
                });
            };

            $scope.eliminarFavorito = function (favorito) {
                var carpeta = obtenerCarpeta(favorito.CodCarpeta);

                alerts.eliminarAnalisis(favorito.Nombre, carpeta.Nombre, function () {
                    favoritosFactory.EliminarFavoritos(favorito.Id).then(function () {
                        removerFavoritos([favorito.Id]);
                        delete $scope.favoritosBorrarMover[favorito.Id];
                    });
                });
            };

            $scope.eliminarFavoritos = function () {
                var favoritosBorrar = $scope.favoritosBorrarMoverArr();

                alerts.eliminarAnalisisMultiple(favoritosBorrar.length, function () {
                    favoritosFactory.EliminarFavoritos(favoritosBorrar).then(function () {
                        removerFavoritos(favoritosBorrar);
                        $scope.favoritosBorrarMover = {};
                    });
                });
            };

            $scope.moverFavorito = function (favorito) {
                moverFavoritoDialog(function (codCarpeta) {
                    favoritosFactory.MoverFavoritos(favorito.Id, codCarpeta).then(function () {
                        favorito.CodCarpeta = codCarpeta;
                        $scope.selected = obtenerCarpeta(codCarpeta);
                        delete $scope.favoritosBorrarMover[favorito.Id];
                    });
                });
            };

            $scope.moverFavoritos = function () {
                moverFavoritoDialog(function (codCarpeta) {
                    var favoritosMover = $scope.favoritosBorrarMoverArr();
                    var favoritos = obtenerFavoritos(favoritosMover);

                    favoritosFactory.MoverFavoritos(favoritosMover, codCarpeta).then(function () {
                        $.each(favoritos, function (key, favorito) {
                            favorito.CodCarpeta = codCarpeta;
                        });

                        $scope.favoritosBorrarMover = {};
                        $scope.selected = obtenerCarpeta(codCarpeta);
                    });
                });
            };

            $scope.paginationChanged = function() {
                favoritosFactory.ObtenerFavoritos().then(function (response) {
                    $scope.data = response.data;
                });
            };
        }])
        .filter("renderFavoritos", ["filterFilter", function ($filter) {
            return function (arr, codCarpeta) {
                if (!codCarpeta) {
                    return arr;
                }

                return $filter(arr, function (favorito) {
                    return favorito.CodCarpeta === codCarpeta;
                });
            }
        }]);
})();