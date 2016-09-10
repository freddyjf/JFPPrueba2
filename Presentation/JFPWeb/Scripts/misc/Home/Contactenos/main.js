(function () {
    $("#frmContacto").on("submit", function (e) {
        e.preventDefault(); // avoid to execute the actual submit of the form.
        Spin.start();

        var $form = $(this);

        var url = "/Contact/ContactSave";
        var data = $form.serializeObject();

        $.post(url, data).done(function (response) {
            bootbox.alert("<p>" + response.Message + "</p>");
            $form.resetForm();
            $("#CodSilenioDepartamento").prepend($("<option />")).val("");
            $("#CodSilenioCiudad").empty();
        }).fail(function (err) {
            var response = err.responseJSON;

            if (!response && !response.Message) {
                console.info(err);
                return;
            }
                
            bootbox.alert("<p>" + response.Message + "</p>");
        }).always(function() {
            Spin.stop();
        });
    });

    var onFirstChange = false;

    $("#CodSilenioDepartamento").on("change", function () {
        var $this = $(this);
        var $ciudadesSelect = $("#CodSilenioCiudad");
        
        if (!onFirstChange)
            onFirstChange = !!$this.find("option:first").remove();

        $.getJSON("/Contact/ObtenerCiudades", { codDepartamento: $this.val() }).done(function (data) {
            $ciudadesSelect.option(data, function (value) { return { text: value.Nombre, value: value.CodSilenio } }, { replace: true });
        });
    });
})();