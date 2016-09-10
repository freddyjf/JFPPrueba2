(function () {
    var messages = JSON.parse($("#dialogs").text()) || {};

    window.alerts = (function () {

        // ReSharper disable once InconsistentNaming
        var Alerts = function () { };

        Alerts.prototype.EliminarNotificaciones = function (callback) {
            return bootbox.dialog({
                className: "noscroll",
                size: "small",
                closeButton: false,
                message: messages.EliminarNotificaciones,
                buttons: {
                    "Eliminar notificaciones": {
                        className: "btn-precedente",
                        callback: callback
                    },
                    "Cancelar": {
                        className: "btn-link"
                    }
                }
            });
        };

        Alerts.prototype.EliminarNotificacionSeleccionada = function (callback) {
            return bootbox.dialog({
                className: "noscroll",
                size: "small",
                closeButton: false,
                message: messages.EliminarNotificacionSeleccionada,
                buttons: {
                    "Eliminar notificaciones": {
                        className: "btn-precedente",
                        callback: callback
                    },
                    "Cancelar": {
                        className: "btn-link"
                    }
                }
            });
        };

        return new Alerts();
    })();
})();