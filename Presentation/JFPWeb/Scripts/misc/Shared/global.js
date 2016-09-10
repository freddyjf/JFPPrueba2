$(document).ready(function () {

    // Añade boton para renovar la linea
    $(".listaLineas .vencida .tituloLinea").after("<button href='#' class='principal renovar'>Renovar</button>");

    // Cambia los tabs en acordeones en dispositivos pq
    // https://github.com/flatlogic/bootstrap-tabcollapse/blob/master/bootstrap-tabcollapse.js
    $(".tabsColapsables").tabCollapse();

    // Inicialización de tooltips
    //jTip plugin para hacer tooltips
    //http://vanity.enavu.com/documentation/
    $(".tip").jTip();

    // Inicializacion de popovers
    $('[data-toggle="popover"]').popover();

    $(".carpeta").change(function () {
        var $val = $(this).val();
        if ($val === "nuevo") {
            $(this).closest(".cuerpo").find(".nuevo").removeClass("oculto").find("input").focus();
        } else {
            $(this).closest(".cuerpo").find(".nuevo").addClass("oculto");
        }

    });

    $(".etiqueta button.close").click(function () {
        var granpa = $(this).closest(".well-fltr");
        $(this).parent().remove();
        var etiqs = granpa.children().length;
        console.log(etiqs);
        if (etiqs <= 0) {
            granpa.remove();
        }
    });

    $(".btn-filtro").on("click", function () {
        $(".filtro-mobile").fadeToggle(300);
        if ($(".orden-mobile").css("display") == "block" || $(".busca-mobile").css("display") == "block") {
            $(".orden-mobile").fadeOut(100);
            $(".busca-mobile").fadeOut(100);
        };
    });

    $(".btn-orden").on("click", function () {
        $(".orden-mobile").fadeToggle(300);
        if ($(".filtro-mobile").css("display") == "block" || $(".busca-mobile").css("display") == "block") {
            $(".filtro-mobile").fadeOut(100);
            $(".busca-mobile").fadeOut(100);
        };
    });

    $(".orden-mobile").find("a").not(".show-submenu").on("click", function () {
        $(".btn-orden").trigger("click");
    });

    $(".btn-busca").on("click", function () {
        $(".busca-mobile").fadeToggle(300);
        if ($(".filtro-mobile").css("display") == "block" || $(".orden-mobile").css("display") == "block") {
            $(".filtro-mobile").fadeOut(100);
            $(".orden-mobile").fadeOut(100);
        };
    });

    $(".submenu").on("mouseleave", function () {
        $(this).removeClass("showing");
    });

    $(".show-submenu").click(function () {
        $(".orden-mobile .submenu").fadeToggle(200);
    });

    $(".cerrar").click(function () {
        $(this).closest(".agregaFavorito").fadeOut(200);
    });

    $(".search-submit").click(function () {
        var $form = $(this).parents("form");

        $form.submit();
    });

    $(document).on("click", ".ppv, .ppvNoStyle", function (e) {
        e.preventDefault();
        var ppvHtml = $($("#ppv").html());
        var $this = $(this);

        ppvHtml.find("#ppv_lineanombre").text($this.data("ppvlineanombre"));
        ppvHtml.find("#ppv_lineaprecio").text($this.data("ppvlineaprecio"));
        ppvHtml.find("#ppv_lineaqty").text($this.data("ppvlineaqty"));
        ppvHtml.find("#ppv_lineaurl").attr("href", $this.data("ppvlineaurl"));

        bootbox.dialog({ message: ppvHtml, title: "Contenido disponible sólo para suscriptores" });
    });

    //FILTROS GRAFICO ************************************
    var $filterToggler = $(".btn.temas-patrones-toggle");
    var $filterContainer = $(".filtros-temas-patrones");
    var $includeTermino = $(".incluir .filtro-termino");
    var $excludeTermino = $(".excluir .filtro-termino");

    $filterToggler.click(function (e) {
        $filterContainer.toggleClass("oculto");
        e.preventDefault();
    });

    //  CHAT DE AYUDA
    $("#opAyudaChat").click(function () {
        window.open($("#chatUrl").text(), "_blank", "width=550,height=490");
    });

    $(document.body).on("click", "[data-put-loading]", function () {
        Spin.start();
    });

    //  Inicializa las galerias de imagenes en la aplicación, para más info revise el partial del slider.
    $(".blueimp-gallery-initializer").each(function () {
        var $this = $(this);
        var links = JSON.parse($this.text());
        var container = document.getElementById($this.data("gallery-container"));

        blueimp.Gallery(links, {
            container: container,
            carousel: true,
            onslide: function (index, slide) {
                var $thisSlide = $(slide);
                var $img = $thisSlide.children("img");
                var $title = $thisSlide.parent(".slides").siblings(".title");
                var $description = $thisSlide.parent(".slides").siblings(".description");

                $title.html($img.attr("title"));
                $description.html($img.data("description"));
            }
        });

        $this.remove();
    });

    var urlTour = $("#tourUrl").text().trim();

    //  Muestra el modal al hacer click en el tour.
    $("#goTour").on("click", function (e) {
        e.preventDefault();

        var $iframe = $("<iframe />",
        {
            src: urlTour,
            style: "border: none",
            allowfullscreen: "true",
            "class": "embed-responsive-item"
        });

        bootbox.dialog({
            message: $iframe
        })
        .find(".modal-body")
        .css("max-height", "510px")
        .find(".bootbox-body")
        .addClass("embed-responsive embed-responsive-16by9");
    });
});