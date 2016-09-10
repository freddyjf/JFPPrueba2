$(document).ready(function () {

    //#region Modulacion

    function getSpan() {
        return $("<span class=\"cerrar-ficha\" style=\"visibility: hidden;\">&times;</span>");
    }

    function refreshImageMapModulacion() {

        $(".tratamiento-graph").maphilight({
            "stroke": false,
            "fillColor": "000000",
            "fillOpacity": 1
        });

        $(".tip").jTip();
    }

    var closeSelector = "span.cerrar-ficha";

    //  Abrir TAB
    $(".fichaDtContainer").on("click", "[data-analisis-modulacion]", function (e) {
        e.preventDefault();
        var codAnalisis = $(this).data("analisis-modulacion");
        var $tabAnalisis = $("[data-analisis={id}]".replace("{id}", codAnalisis)).first();

        if ($tabAnalisis.length > 0) {
            $tabAnalisis.find("a").tab("show");
            scrollTop();
            return;
        }

        var $tabs = $(".tabs-analisis");
        var $tabsContent = $(".tab-content");

        //  Cuando bootstrap-tabdrop termine de renderizar los tabs, haga scroll.
        $(window).on("tabDropDone", function () {
            scrollTop();
        });

        $.ajax("/AnalisisJuridico/CargarTab", { type: "GET", cache: false, data: { codAnalisis: codAnalisis } }).done(function (response) {
            var $tab = $(response.tab);
            var $tabContent = $(response.tabContent);

            $tabs.append($tab);
            $tabsContent.append($tabContent);

            $tab.find("a").tab("show").find(".tip").jTip();

            $tabs.find("li").filter(function () { return $(this).find(closeSelector).length === 0; }).find("a").prepend(getSpan());

            //  Ajusta los tabs presentes, este evento es escuchado por bootstrap-tabdrop.
            $(window).trigger("resize");

            refreshImageMapModulacion();
        });
    });

    //  Cerrar TAB
    $(".tabs-analisis").on("click", ".cerrar-ficha", function (e) {
        e.stopPropagation();
        e.preventDefault();

        // Se obtiene el span de cerrar.
        var $this = $(this);

        // Se obtiene el li (tab).
        var $li = $this.parents("li").first();

        // Se obtiene el hermano del li hacia la izquierda.
        var $leftLi = $li.prev(".fichas_tabs");

        // Se obtiene el hermano del li hacia la derecha.
        var $rightLi = $li.next(".fichas_tabs");

        // Se obtiene el a (click)(tab).
        var $a = $this.parent("a");

        // Si el li actual tiene la clase active.
        if ($li.hasClass("active")) {

            // Si no hay hermanos hacia la izquierda.
            if ($leftLi.length === 0) {
                // Muestre el tab hermano de la derecha.
                $rightLi.find("a").tab("show");
            } else {
                // Muestre el tab hermano de la izquierda.
                $leftLi.find("a").tab("show");
            }
        }

        // Remueva el contenido del tab.
        $($a.attr("href")).remove();
        // Remueva el tab.
        $li.remove();

        // Revise cuantos span de cerrar hay.
        var $equis = $(closeSelector);

        // Si solo hay uno, remueva el span de cerrar.
        if ($equis.length === 1) {
            $equis.remove();
        }

        var codAnalisis = $li.attr("data-analisis");
        $.post("/AnalisisJuridico/EliminarTabSesion", { codAnalisis: codAnalisis });

        //  Ajusta los tabs presentes, este evento es escuchado por bootstrap-tabdrop.
        $(window).trigger("resize");
    });

    //#endregion

    //#region Acordeon modulacion

    $(".modulacion-toogle").on("click", function () {
        $(this).next(".modulos").toggle();
    });

    //#endregion

    $(".tabs-analisis").tabdrop();

    refreshImageMapModulacion();

    //  Se agrega "click" al gráfico del tratamiento para ampliarlo.
    $("div.tratamiento-graph").on("click", function () {
        var $fichaTabsWrapper = $("<div />", { "class": "fichaTabs", style: "margin:0;" });
        var $tabContentWrapper = $("<div />", { "class": "tab-content", style: "border:none; padding:0" }).appendTo($fichaTabsWrapper);
        var $analisisSentenciaWrapper = $("<div />", { "class": "analisisSentencia", style: "border:none;" }).appendTo($tabContentWrapper);
        var $cuerpoAnalisisWrapper = $("<div />", { "class": "cuerpoAnalisis" }).appendTo($analisisSentenciaWrapper);
        var $modulacionHtml = $(this).parents(".modulacion").clone().removeClass("col-sm-4").appendTo($cuerpoAnalisisWrapper);

        $modulacionHtml.find("map").remove();

        bootbox.dialog({
            message: $fichaTabsWrapper
        }).on("shown.bs.modal", function () {
            $(this).find(".tratamiento-graph").maphilight({
                "stroke": false,
                "fillColor": "000000",
                "fillOpacity": 1
            });
        });
    });
});