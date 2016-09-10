$(document).ready(function () {
    var $model = $("#consideraciones");
    var data = JSON.parse($model.text());
    $model.remove();

    //#region Consideraciones

    var $checkboxConsideraciones = $("#toggle-consideracion");

    var classesToAdd = $checkboxConsideraciones.is(":checked") ? "consideraciones active" : "consideraciones";

    $.each(data, function (key, value) {
        var $tag = $getElementById(value.Parrafo);
        $tag.addClass(classesToAdd);
    });

    $checkboxConsideraciones.on("change", function () {
        $(".consideraciones").toggleClass("active");
    });

    var actualConsideracion = 0;
    var limiteActualConsideracion = 1;

    var totalConsideracion = data.length;

    var $actualConsideracion = $("#actual-consideracion").text(actualConsideracion);
    $("#total-consideracion").text(totalConsideracion);

    $("#anterior-consideracion").on("click", function () {
        if (actualConsideracion <= limiteActualConsideracion) { return; }

        $actualConsideracion.text(--actualConsideracion);

        doScroll();
    });

    $("#siguiente-consideracion").on("click", function () {
        if (actualConsideracion >= totalConsideracion) { return; }

        $actualConsideracion.text(++actualConsideracion);

        doScroll();
    });

    $(".consideracion-trigger").on("click", function (e) {
        e.preventDefault();
        doScroll();
    });

    function doScroll() {
        var selector = data[actualConsideracion - 1];

        if (!selector) { return; }

        var parrafo = selector.Parrafo;

        var $tag = $getElementById(parrafo);

        scrollToElement($tag.offset().top);

        callRegistrarUso("IrConsideracionesImportantes");
    }

    //#endregion

    //#region Secciones

    $(".seccion-select-documento").on("change", function () {
        var $this = $(this);
        var $tag = $getElementById($this.val());

        if ($tag.length === 0) {
            callRegistrarUso("IrASeccion", ["MetaData", "[Seccion:Home]"]);
            return scrollTop();
        }

        callRegistrarUso("IrASeccion", ["MetaData", "[Seccion:" + $this.find("option[value=" + $this.val() + "]").text() + "]"]);

        return scrollToElement($tag.offset().top);
    });

    $(".seccion-select-documento-trigger").on("click", function(e) {
        e.preventDefault();
        $(".seccion-select-documento").trigger("change");
    });

    //#endregion

    //#region Sticky Toolbar

    var sticky = new Waypoint.Sticky({
        element: $('#toolbar')
    });

    //#endregion

    //#region Utils

    function $getElementById(id) {
        return $("#" + id);
    }

    (function () {
        if (!!queryString["scrollTo"]) {
            var $tag = $getElementById(queryString["scrollTo"]);

            if ($tag.length === 0) { return; }

            setTimeout(function () {
                scrollToElement($tag.offset().top);
            }, 1000);
        }

    })();

    //#endregion
});