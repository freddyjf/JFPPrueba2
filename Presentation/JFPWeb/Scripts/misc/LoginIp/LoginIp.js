
//*************************************************************************************************/
//***********         funcionalidades centralizadas para la creacion de usarios IP y la autenticacion**************/
//***********Referenciado en: 
//**************************    LoginSubAtencion.cshtml                                       **********/

$(document).ready(function () {
    $("#formRegistrarse").on("submit", function (e) {
        e.preventDefault();

        var $this = $(this);

        Spin.start();

        $.ajax({
            type: $this.attr("method"),
            url: $this.attr("action"),
            data: $this.serialize()
        }).done(function (data) {
            bootbox.dialog({
                title: "Confirmación creacion de usuario",
                message: $("<p />", { text: data.message }),
                buttons: {
                    Cerrar: {}
                }
            });
        }).fail(function (err) {
            var data = err.responseJSON;

            if (data.errors.length > 0) {
                var $ul = $("<ul />");

                $.each(data.errors, function (i, val) {
                    var $li = $("<li />");
                    $("<p />", { text: val }).appendTo($li);
                    $li.appendTo($ul);
                });

                var $message = $("<p>Por Favor verifique la siguiente Informacion </p>");
                var $div = $("<div />").append($message).append($ul);

                bootbox.dialog({ title: "Confirmación creacion de usuario", message: $div, buttons: { Cerrar: {} } });
            } else
                bootbox.dialog({ title: "Confirmación creacion de usuario", message: "<p>En estos momento no fue posible crear el usuario, por favor intentelo más tarde.</p>", buttons: { Cerrar: {} } });
        }).always(function() {
            Spin.stop();
        });
    });
});