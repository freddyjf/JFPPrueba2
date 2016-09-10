(function () {
    // ReSharper disable once InconsistentNaming
    var Alerts = function () { };

    var messages = JSON.parse($("#dialogs").text()) || {};

    Alerts.prototype.nuevaCarpeta = function (callback) {
        var $modal = bootbox.dialog({
            className: "noscroll",
            size: "small",
            title: "Crear nueva carpeta",
            message: "<form>\n  <div class=\"form-group\">\n    <label>Nombre</label>\n    <input class=\"form-control\" type=\"text\" name=\"Carpeta\">\n  </div>\n</form>",
            buttons: {
                "Crear carpeta": {
                    className: "nuevaCarpeta btn-precedente",
                    callback: function () {
                        return handleForm($(this).find("form"));
                    }
                }
            },
            onEscape: function () {
                $(this).trigger("modal.bs.canceled");
            }
        });

        function handleForm(form) {
            if (typeof callback === "function") {
                var data = $(form).serializeObject().Carpeta;
                if (!data) {
                    return false;
                }
                callback(data);
            }
        }

        $modal.find("form").on("submit", function (e) {
            e.preventDefault();
            $modal.find(".nuevaCarpeta").trigger("click");
        });

        return $modal;
    };

    Alerts.prototype.eliminarAnalisis = function (nombreAnalisis, nombreCarpeta, callback) {

        var message = messages.EliminarFavorito.replace("{{nombreAnalisis}}", nombreAnalisis).replace("{{nombreCarpeta}}", nombreCarpeta);

        var $modal = bootbox.dialog({
            className: "noscroll",
            size: "small",
            closeButton: false,
            message: message,
            buttons: { "": {} },
            show: false
        });

        var $aceptar = $("<a class=\"btn btn-precedente\">Eliminar análisis</a>");
        var $cancelar = $("<a class=\"pointer\" data-dismiss=\"modal\">Cancelar</a>");

        $aceptar.on("click", function () {
            if (typeof callback === "function") {
                callback();
            }
            $modal.modal("hide");
        });

        $modal.find(".modal-footer").empty().append($aceptar).append($cancelar);

        $modal.modal("show");
    };

    Alerts.prototype.eliminarAnalisisMultiple = function (count, callback) {

        var message = messages.EliminarFavoritoMultiple.replace("{{count}}", count);

        var $modal = bootbox.dialog({
            className: "noscroll",
            size: "small",
            closeButton: false,
            message: message,
            buttons: { "": {} },
            show: false
        });

        var $aceptar = $("<a class=\"btn btn-precedente\">Eliminar análisis</a>");
        var $cancelar = $("<a class=\"pointer\" data-dismiss=\"modal\">Cancelar</a>");

        $aceptar.on("click", function () {
            if (typeof callback === "function") {
                callback();
            }
            $modal.modal("hide");
        });

        $modal.find(".modal-footer").empty().append($aceptar).append($cancelar);

        $modal.modal("show");
    };

    Alerts.prototype.eliminarCarpetas = function (nombreCarpeta, callback) {

        var $modal = bootbox.dialog({
            className: "noscroll",
            size: "small",
            closeButton: false,
            message: messages.EliminarCarpeta.replace("{{nombreCarpeta}}", nombreCarpeta),
            buttons: { "": {} },
            show: false
        });

        var $aceptar = $("<a class=\"btn btn-precedente\">Eliminar carpeta</a>");
        var $cancelar = $("<a class=\"pointer\" data-dismiss=\"modal\">Cancelar</a>");

        $cancelar.on("click", function() {
            $modal.trigger("modal.bs.canceled");
        });

        $aceptar.on("click", function () {
            if (typeof callback === "function") {
                callback();
            }
            $modal.modal("hide");
        });

        $modal.find(".modal-footer").empty().append($aceptar).append($cancelar);

        $modal.modal("show");

        return $modal;
    };

    Alerts.prototype.mensaje = function (mensaje, callback) {

        bootbox.dialog({
            className: "noscroll",
            size: "small",
            closeButton: false,
            message: mensaje,
            buttons: {
                "Aceptar": {
                    className: "btn-precedente",
                    callback: callback
                }
            }
        });
    };

    Alerts.prototype.moverCarpeta = function (data, callback) {
        var message = "<div>\n<p class=\"smallModal\">\n   Seleccione la carpeta a la cual desea mover el favorito\n</p>\n\n<form>\n   <div class=\"form-group\">\n      <select class=\"form-control selecciona-carpeta\"></select>\n   </div>\n</form>\n</div>";

        var $modal = bootbox.dialog({
            className: "noscroll",
            size: "small",
            closeButton: false,
            message: message,
            buttons: { "": {} },
            show: false
        });

        var $select = $modal.find("select").option(data);
        var $aceptar = $("<a class=\"btn btn-precedente\">Mover</a>");
        var $cancelar = $("<a class=\"pointer\" data-dismiss=\"modal\">Cancelar</a>");

        $aceptar.on("click", function () {
            if (typeof callback === "function") {
                callback(parseInt($select.val()));
            }
            $modal.modal("hide");
        });

        $modal.find(".modal-footer").empty().append($aceptar).append($cancelar);

        $modal.modal("show");
    };

    window.alerts = new Alerts();
})();