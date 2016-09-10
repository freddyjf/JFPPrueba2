(function () {

    //  Permite obtener el hermano o compañero del link que se hizo click para agregar o quitar un análisis como favorito.
    function getAnalisisTabToContainerSibling($element) {
        if ($element.is("button")) {
            return $(".tab-content").find("[data-analisis-tab-to-container=" + $element.attr("data-analisis-tab-to-container") + "]");
        }

        if ($element.is("a")) {
            return $(".tabs-analisis").find("[data-analisis-tab-to-container=" + $element.attr("data-analisis-tab-to-container") + "]");
        }

        return null;
    }

    //  Permite agregar configuración común a los dos elementos en caso de que uno sea x elemento y el hermano otro elemento.
    function setConfigInAnalisisTabToContainerSiblings($element, $sibling, config) {
        config = config || {};

        //  Si $element es un <a>...
        if ($element.is("a")) {
            //  Establezcale el titulo de remover texto al <a>.
            $element.find("span.textFromLinkDrop").text(config.text);

            //  Establezcale el titulo de remover texto en el tooltip al <button>.
            $sibling.attr("title", config.text);

        } //  Si $element es un <button>...
        else if ($element.is("button")) {
            //  Establezcale el titulo de remover texto en el tooltip al <button>.
            $element.attr("title", config.text);

            //  Establezcale el titulo de remover texto al <a>.
            $sibling.find("span.textFromLinkDrop").text(config.text);

            //  Agregue el texto de agregar o remover favorito cuando el usuario haya quitado el mouse del elemento, esto es porque jTip.js reemplaza el atributo title una vez el usuario a quitado el mouse del elemento.
            if (!!config.includeTextOnElementMouseLeave) {
                $element.one("mouseleave", function () {
                    $element.attr("title", config.text);
                });
            }
        }
    }

    angular.module("precedente")
    .directive("agregarFavoritos", function () {
        return {
            scope: {},
            templateUrl: "add-favorites",
            link: function (scope, $element, $attrs) {
                var duration = 300;
                var mostrarEnSelector = "button.favorito:not(.agregado), a.favorito:not(.agregado)";
                var quitarEnSelector = "button.favorito.agregado, a.favorito.agregado";
                var addedClass = "agregado";
                var agregarText = "Agregar a favoritos";
                var removerText = "Eliminar de favoritos";

                var $agregarFavoritosPopover = $element.find(".agregaFavorito");
                var $close = $element.find(".cerrar");
                var $tabsContainer = $(".fichaTabs");
                var $thisButton = null;

                $tabsContainer.on("click", mostrarEnSelector, function (e) {
                    e.preventDefault();

                    scope.setDefaults();
                    scope.fill();

                    $thisButton = $(this);
                    var offset = $thisButton.offset();

                    scope.crearFavorito.codAnalisis = $thisButton.parent("li").attr("data-analisis") || scope.crearFavorito.codAnalisis;

                    if ($thisButton.is("button")) {
                        scope.crearFavorito.nombreFavorito = $thisButton.next("a").find("span.titulo-sentencia").text();
                    } else if ($thisButton.is("a")) {
                        scope.crearFavorito.nombreFavorito = $(".tabs-analisis").find("li.fichas_tabs.active").find("span.titulo-sentencia").text();
                    }

                    offset.top += 30;
                    offset.left += 5;

                    // #region Valida fuera pantalla

                    //  Se obtiene el ancho de la pantalla.
                    var anchoPantalla = $(document).width();
                    var anchoPopover = $agregarFavoritosPopover.width();

                    //  Se valida que no se salga de la pantalla.
                    if ((offset.left + anchoPopover) > anchoPantalla) {
                        offset.left = (anchoPantalla - anchoPopover) - 10;
                    }

                    // #endregion Fin valida fuera pantalla

                    if ($agregarFavoritosPopover.css("display") === "none") {
                        $agregarFavoritosPopover.css(offset);
                        $agregarFavoritosPopover.fadeIn(duration);
                    } else {
                        $agregarFavoritosPopover.animate(offset);
                    }
                });

                $tabsContainer.on("click", quitarEnSelector, function () {
                    var $this = $(this);
                    var $parent = $this.parent("li");
                    var codFavorito = $parent.attr("data-favorito");

                    scope.eliminarFavorito(codFavorito).then(function () {
                        //  Obtiene el hermano del botón actual, si es un <button> entonces retorna <a> o viceversa.
                        var $siblingElement = getAnalisisTabToContainerSibling($this);

                        //  Remueva las clases de "agregado a favoritos" en el <button> y en el <a>.
                        $this.removeClass(addedClass);
                        $siblingElement.removeClass(addedClass);

                        //  Remueva los atributos del id del favorito en el <button> y en el <a>.
                        $parent.removeAttr("data-favorito");
                        $siblingElement.parent("li").removeAttr("data-favorito");

                        //  Agregue configuración común a uno o a los dos elementos.
                        setConfigInAnalisisTabToContainerSiblings($this, $siblingElement, { text: agregarText, includeTextOnElementMouseLeave: true });
                    });
                });

                $close.on("click", function (e) {
                    e.preventDefault();
                    scope.close();
                });

                $element.on("$destroy", function () {
                    scope.$destroy();
                });

                scope.close = function () {
                    $agregarFavoritosPopover.fadeOut({
                        complete: function () {
                            $(this).offset({ top: 0, left: 0 });
                        },
                        duration: duration
                    });
                };

                scope.closeSuccess = function (response) {
                    scope.close();

                    //  $thisButton puede ser un <button> de los tabs o un <a> en el dropdown.
                    if (!!$thisButton) {

                        //  Agregue la clase de "agregado a favoritos" al elemento
                        $thisButton.addClass(addedClass);

                        //  Al padre <li> establezcale el id del favorito.
                        $thisButton.parent("li").attr("data-favorito", response.data);

                        //  Obtiene el hermano del botón actual, si es un <button> entonces retorna <a> o viceversa.
                        var $siblingElement = getAnalisisTabToContainerSibling($thisButton);

                        //  Agregue la clase de "agregado a favoritos" al elemento
                        $siblingElement.addClass(addedClass);

                        //  Agreguele el id del favorito.
                        $siblingElement.parent("li").attr("data-favorito", response.data);

                        //  Agregue configuración común a uno o a los dos elementos.
                        setConfigInAnalisisTabToContainerSiblings($thisButton, $siblingElement, { text: removerText });
                    }
                };
            },
            controller: ["$scope", "favoritosFactory", function ($scope, favoritosFactory) {

                $scope.carpetas = [];
                $scope.crearFavoritoForm = {};

                var onError = function (err) {
                    alert(err.data.Message);
                };

                $scope.guardar = function () {
                    var obj = $scope.crearFavorito;

                    if (obj.codCarpeta === -1) {
                        favoritosFactory.CrearCarpeta(obj.nombreCarpeta).then(function (response) {
                            var carpeta = response.data;
                            $scope.carpetas.push(carpeta);

                            $scope.crearFavorito.codCarpeta = carpeta.Id;
                            $scope.crearFavorito.nombreCarpeta = "";

                            favoritosFactory.CrearFavorito(obj.codAnalisis, obj.codCarpeta, obj.nombreFavorito).then($scope.closeSuccess).catch(onError);
                        }).catch(onError);
                    } else {
                        favoritosFactory.CrearFavorito(obj.codAnalisis, obj.codCarpeta, obj.nombreFavorito).then($scope.closeSuccess).catch(onError);
                    }
                };

                $scope.fill = function () {
                    favoritosFactory.ObtenerCarpetas().then(function (response) {
                        $scope.carpetas = response.data.Carpetas;
                        $scope.carpetas.push({ Id: -1, Nombre: "Nueva carpeta", FechaCreacion: new Date() });
                    });
                };

                $scope.setDefaults = function () {
                    $scope.crearFavorito = {
                        codCarpeta: 0,
                        codAnalisis: null,
                        nombreFavorito: "",
                        nombreCarpeta: ""
                    };
                };

                $scope.eliminarFavorito = function (codFavoritos) {
                    return favoritosFactory.EliminarFavoritos(codFavoritos);
                };

            }]
        }
    });
})();