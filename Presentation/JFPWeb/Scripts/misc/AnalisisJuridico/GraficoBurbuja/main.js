$(document).ready(function () {
    var model = JSON.parse($("#model").text());

    var ocultarFiltros = function () {
        $(".filtros-temas-patrones:first").addClass("oculto");
    }

    // Invocar la ayuda/tour 
    $("li[id=divAyuda]").on("click", function () {
        var tour = new Tour({
            template: '<div class="popover tour" role="tooltip" id="step-0" style="display: block;"> <div class="arrow" style="left: 50%;"></div> <h3 class="popover-title"></h3> <div class="popover-content"></div> <div class="popover-navigation"> <div class="btn-group"> <button class="btn btn-sm btn-default" data-role="prev">&lt;&lt;</button> <button class="btn btn-sm btn-default" data-role="next">&gt;&gt;</button>  </div> <button class="btn btn-sm btn-default" data-role="end">Finalizar</button> </div> </div>',
            steps: [
                {
                    element: "svg:first",
                    title: "Ayuda 1/6",
                    content: model.MensajesTour.Uno,
                    placement: "top",
                    onShow: ocultarFiltros
                },
                {
                    element: "circle[id^=cirSmall]:first",
                    title: "Ayuda 2/6",
                    content: model.MensajesTour.Dos,
                    placement: "top",
                    onShow: ocultarFiltros
                },
                {
                    element: "circle[id^=cirSmall]:first",
                    title: "Ayuda 3/6",
                    content: model.MensajesTour.Tres,
                    placement: "top",
                    onShow: ocultarFiltros
                },
                {
                    element: "text:contains('100%')",
                    title: "Ayuda 4/6",
                    content: model.MensajesTour.Cuatro,
                    onShow: ocultarFiltros
                },
                {
                    element: "div[class=filtros-grafico-container] a:first",
                    title: "Ayuda 5/6",
                    content: model.MensajesTour.Cinco,
                    placement: "left",
                    onShow: ocultarFiltros
                },
                {
                    element: ".filtros-temas-patrones:first",
                    title: "Ayuda 6/6",
                    content: model.MensajesTour.Seis,
                    placement: "left",
                    onShow: function () {
                        $(".filtros-temas-patrones:first").removeClass("oculto");
                    }
                }
            ]
        });

        // "@T("AJ.Grafico.Ayuda.Seis")"
        // Initialize the tour
        tour.init(true);

        // Start the tour
        tour.start(true);
    });

    //  Aplicar el filtro 
    $("a[id=filterApply]").on("click", function () {
        var filterData = "";

        // El filtro por tema 
        $.each(["t", "p"], function () {
            var i = this;
            var className = i == "t" ? "temas" : "patrones";
            if ($("div[class=" + className + "] ul[id=" + i + "-e] a").length > 0) {
                filterData = filterData + (filterData.length > 0 ? ";" : "") + i + "i";
                $.each($("div[class=" + className + "] ul[id=" + i + "-i] a"), function () {
                    filterData = filterData + "," + $(this).attr("data-value");
                });
            }
        });

        // Todo está excluído? No se envía nada. 
        if (filterData == "ti;pi" || filterData == "ti" || filterData == "pi")
            return;

        var $form = $("#filtrarForm");
        $form.find("#codAnalisis").val(model.Id);
        $form.find("#filtros").val(filterData);
        $form.submit();
    });

    // Incluir/Excluir todos 
    $("a[class=filtro-comando]").on("click", function () {
        var dType = $(this).attr("data-type");
        var dStatus = $(this).attr("data-status");
        var liS = $("ul[id=" + dType + "-" + dStatus + "] li");

        for (var i = liS.length; i >= 0; i--) {
            swapFilterItem(liS.children("a")[i]);
        }
    });

    // Mecánica del filtro individual 
    $("ul[class=terminos] a[data-type]").on("click", function () {
        swapFilterItem(this);
    });

    // Include/Exclude un ítem 
    function swapFilterItem(item) {
        var $this = $(item);
        var dtype = $this.attr("data-type");
        var dstatus = $this.attr("data-status");
        //var dvalue = $this.attr('data-value');
        var otherStatus = (dstatus == "e" ? "i" : "e");
        var otherClass = (dstatus == "e" ? "fa-angle-double-right" : "fa-angle-double-left");
        var otherUl = $("ul[id=" + dtype + "-" + otherStatus + "]");

        var $li = $this.parent("li").detach();
        var aLink = $li.children().first();
        aLink.attr("data-status", otherStatus);
        aLink.children("i").attr("class", "fa " + otherClass);
        otherUl.append($li);

    }

    // Presentación de los datos de la burburja 
    $("[id^=cirSmall]").on("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#cirGroup_" + id).css("display", "inline");
    });

    // Ocultar los datos de la burburja 
    $("[id^=cirGroup]").on("mouseleave", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#cirGroup_" + id).css("display", "none");
    });

    // Popup para las modulaciones de una burbuja 
    window.PopupModulacion = function (codAnalisis) {
        $.get("/AnalisisJuridico/GraficoBurbujaModulacion", { codCurrentAnalisisId: model.Id, otherAnalisisId: codAnalisis }).done(function (html) {
            var $html = $(html);

            $html.find(".tip").jTip();

            bootbox.alert({
                message: $html,
                size: "small",
                closeButton: false
            });

            $(".tratamiento-graph").maphilight({
                "stroke": false,
                "fillColor": "000000",
                "fillOpacity": 1
            });
        });
    };

    $("text.bubble_decorator").on("mouseenter", function () {
        var $this = $(this);
        var $circle = $this.prev("circle");

        $circle.css({ "fill": "#d9534f" });
        $this.css({ "fill": "white" });

    }).on("mouseleave", function () {
        var $this = $(this);
        var $circle = $this.prev("circle");

        $circle.css({ "fill": "" });
        $this.css({ "fill": "" });
    });
});