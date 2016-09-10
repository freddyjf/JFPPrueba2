var Model = JSON.parse($("#model").text());

var ModulacionIdActual;
var Mod1 = true;
var Mod2 = true;
var Mod4 = true;
var Mod8 = true;
var Mod16 = true;
var Mod32 = true;
var IncluirLinks = Model.IncluirLinksAnalisis;

var moveToElement = function (parent, elem) {
    var $parent = $(parent);
    var $elem = $(elem);

    if ($parent.length === 0 || $elem.length === 0)
        return;

    var scrollLeft = $parent.scrollLeft() - $parent.offset().left + $elem.offset().left;
    var diff = -30;

    $parent.scrollLeft(scrollLeft + diff);
};

// Procesa la colorización de las modulaciones.
function ModulacionMark() {
    ClearAllModulaciones();

    var mod = GetModulacionData(ModulacionIdActual);
    if (mod == null)
        return;

    // Recorrer las sentencias.
    for (var i = 0; i < mod.s.length; i++) {
        var sen = $("g[id='Sen" + mod.s[i].id + "']");
        if (sen == null)
            continue;

        var filtroActivo = false;

        switch (mod.s[i].mod) {
            case 1:
            case 2:
                filtroActivo = Mod1;
                break;
            case 4:
                filtroActivo = Mod4;
                break;
            case 8:
                filtroActivo = Mod8;
                break;
            case 16:
                filtroActivo = Mod16;
                break;
            case 32:
                filtroActivo = Mod32;
                break;
        }

        // Aplicar el color.
        $(sen[0]).attr("class", "Modulacion-" + mod.s[i].mod + (filtroActivo ? "-1" : "-0"));
    }

    hideSentenciaPopup();
}

// Quita el color de todas las modulaciones.
function ClearAllModulaciones() {
    $("g[id^='Sen']").attr("class", "Unselected");
}

// Click sobre algunos de los filtros por tipo de modulación.
$(".btnModulacionFiltro").on("click", function () {
    // Obtener el estado de los botones.
    var $this = $(this);
    var activo = !$this.hasClass("active");

    switch ($this.attr("data-modulacion")) {
        case "1":
        case "2":
            Mod1 = activo;
            Mod2 = activo;
            break;
        case "4":
            Mod4 = activo;
            break;
        case "8":
            Mod8 = activo;
            break;
        case "16":
            Mod16 = activo;
            break;
        case "32":
            Mod32 = activo;
            break;
    }

    ModulacionMark();
});

// Esconde el popup de la sentencia.
function hideSentenciaPopup() {
    $("#popupSentencia").hide();
}

// Click sobre alguna de las sentencias.
$("g[id^='Sen']").on("click", function (e) {
    e.stopPropagation();
    var $this = $(this);
    var offset = $this.offset();
    offset.top += 25;
    offset.left += 25;

    // Establecer los valores.
    $("#psSentenciaTitulo").text($this.attr("data-sentenciatitulo"));
    $("#psEntidad").text($this.attr("data-entidadorigen"));
    var analisis = $this.attr("data-analisis");

    if (analisis !== "") {
        // La lista de los análisis.
        var lista = "";
        var partes = analisis.split("|");
        var HTML = '';

        for (var i = 0; i < partes.length; i += 2) {
            if (IncluirLinks) {
                lista += "<li><a onclick='javascript:ModulacionSelect(\"" +
                    partes[i] + "\", \"" +
                    $this.attr("id") + "\", \"" +
                    partes[i + 1] +
                    "\");' title='Resaltar la modulación de este análisis' style='margin-right:50px;'>" +
                    partes[i + 1] +
                    "</a><a href='/AnalisisJuridico/Area/Analisis/" +
                    partes[i] +
                    "'><i class='fa fa-file-text-o' style='font-size:20px;float:right;' aria-hidden='true' title='Abrir este análisis'></i></a></li>";
            }
            else {
                lista += "<li><a onclick='javascript:ModulacionSelect(\"" +
                    partes[i] + "\", \"" +
                    $this.attr("id") + "\", \"" +
                    partes[i + 1] +
                    "\");' title='Resaltar la modulación de este análisis' style='margin-right:50px;'>" +
                    partes[i + 1] +
                    "</a></li>";
            }
        }

        // Agregar las modulaciones agrupadas
        if (IncluirLinks)
        {
            var Hijos = $("g[idparent='" + $this.attr('id').substring(3) + "']");
            if (Hijos.length > 0) {
                Hijos.each(function () {
                    var $this = $(this);
                    HTML = HTML
                        + '<div id="psAnalisis"><hr /><p>'
                        + $this.attr("data-sentenciatitulo")
                        + '</p><hr /><ul>';

                    // Agregar los análisis.
                    var analisisHijo = $this.attr("data-analisis");
                    if (analisis !== "") {
                        var listaHijo = "";
                        var partesHijo = analisisHijo.split("|");
                        for (var i = 0; i < partesHijo.length; i += 2) {
                            listaHijo += "<li><a onclick='javascript:ModulacionSelect(\"" +
                                partesHijo[i] + "\", \"" +
                                $this.attr("id") + "\", \"" +
                                partesHijo[i + 1] +
                                "\");' title='Resaltar la modulación de este análisis' style='margin-right:50px;'>" +
                                partesHijo[i + 1] +
                                "</a><a href='/AnalisisJuridico/Area/Analisis/" +
                                partesHijo[i] +
                                "'><i class='fa fa-file-text-o' style='font-size:20px;float:right;' aria-hidden='true' title='Abrir este análisis'></i></a></li>";
                        }
                        HTML = HTML + listaHijo;
                    }
                    HTML = HTML + '</ul></div>';
                });
            }
        }
        else
        {
            var Hijos = $("g[idparent='" + $this.attr('id').substring(3) + "']");
            if (Hijos.length > 0) {
                Hijos.each(function () {
                    var $this = $(this);
                    HTML = HTML
                        + '<div id="psAnalisis"><hr /><p>'
                        + $this.attr("data-sentenciatitulo")
                        + '</p><hr /><ul>';

                    // Agregar los análisis.
                    var analisisHijo = $this.attr("data-analisis");
                    if (analisis !== "") {
                        var listaHijo = "";
                        var partesHijo = analisisHijo.split("|");
                        for (var i = 0; i < partesHijo.length; i += 2) {
                            listaHijo += "<li><a onclick='javascript:ModulacionSelect(\"" +
                                partesHijo[i] + "\", \"" +
                                $this.attr("id") + "\", \"" +
                                partesHijo[i + 1] +
                                "\");' title='Resaltar la modulación de este análisis' style='margin-right:50px;'>" +
                                partesHijo[i + 1] +
                                "</a></li>";
                        }
                        HTML = HTML + listaHijo;
                    }
                    HTML = HTML + '</ul></div>';
                });
            }
        }


        $("#psAnalisis ul").html(lista);
        $("#psAnalisis").show();

        var HTMLGrupo = $("div[id='psGrupo']");
        HTMLGrupo.html(HTML);

    } else {
        $("#psAnalisis").hide();
    }

    // Cuadrar la posición del popup dependiendo de la posición de la sentencia.
    var dialog = $("#popupSentencia");
    var svgGrid = $("#SvgGraph").offset();

    if ((offset.top - svgGrid.top) > (Model.GridHeight / 2)) {
        offset.top = $this.offset().top - (dialog.height() + 25);
    }
    if ((offset.left - svgGrid.left) > (Model.GridWidth / 2)) {
        offset.left = $this.offset().left - (dialog.width() + 25);
    }

    dialog.css(offset);
    dialog.show();
});

// Selección de una modulación en una sentencia.
function ModulacionSelect(mod, sourceObject, nombreAnalisis) {
    ModulacionIdActual = mod;
    ModulacionMark();

    // Establecer los título.
    var source = $("#" + sourceObject);
    var nombreSentencia = source.attr("data-sentenciatitulo");

    if (source.is("[idparent]")) {
        // Si hay una sentencia padre, se unen los títulos.
        var SentenciaPadre = $("#Sen" + source.attr("idparent")).attr("data-sentenciatitulo");
        nombreSentencia = SentenciaPadre + " - " + nombreSentencia;
    }

    $("#titSentencia").text(nombreSentencia);
    $("#titAnalisis").text("Análisis Jurisprudencial: " + nombreAnalisis);

    // Animarlo.
    $("#divBackTitulos").hide();
    $("#divBackTitulos").fadeIn();

    if (!Model.ClienteExterno) {
        GetTesisFromModulacion(mod, nombreSentencia, nombreAnalisis);
    }

    if (source.is("[idparent]"))
        SetRectSelected($("#Sen" + source.attr("idparent")));
    else
        SetRectSelected($("#" + sourceObject));
}

function GetTesisFromModulacion(idAnalisis, nombreSentencia, nombreAnalisis) {
    var characters = angular.element(document).injector().get("charactersFilter");

    var $tesisContainer = $("#tesis-detail-container");
    var $tesisDetail = $tesisContainer.find("#tesis-detail");

    var $tesisSentencia = $tesisDetail.find("#tesis-sentencia");
    var $tesisAnalisis = $tesisDetail.find("#tesis-analisis");

    var $tesis = $tesisContainer.find("#tesis");
    var $tesisVerAnalisis = $("#tesis-ver-analisis");

    $tesisDetail.show();
    $tesisVerAnalisis.hide();

    $.get("/AnalisisJuridico/ObtenerTesis", { codAnalisis: idAnalisis }).done(function (tesis) {
        $tesis.text(characters(tesis, 330));

        $tesisSentencia.text(nombreSentencia);
        $tesisAnalisis.text(nombreAnalisis);

        $tesisVerAnalisis.attr("href", "/AnalisisJuridico/Area/Analisis/" + idAnalisis);
        $tesisVerAnalisis.show();
    });
}

function makeSVG(tag, attrs) {
    var el = document.createElementNS("http://www.w3.org/2000/svg", tag);
    for (var k in attrs) {
        if (attrs.hasOwnProperty(k))
            el.setAttribute(k, attrs[k]);
    }
    return el;
}

function SetRectSelected($gElement) {
    var $currentRectSelected = $("rect[class='UstedEstaAquiCaja']").detach();

    if ($currentRectSelected.length === 0) {
        $currentRectSelected = $(makeSVG("rect", { x: -4, y: -3, width: 0, height: 0, "class": "UstedEstaAquiCaja" }));
    }

    $gElement.prepend($currentRectSelected);

    var $siblingOfRectSelected = $currentRectSelected.next();
    $currentRectSelected.attr("width", +$siblingOfRectSelected.attr("width") + 8);
    $currentRectSelected.attr("height", +$siblingOfRectSelected.attr("height") + 6);
}

// Retorna la información de una modulación por id.
function GetModulacionData(id) {
    for (var i = 0; i < modulaciones.length; i++) {
        if (modulaciones[i].id == id)
            return modulaciones[i];
        else {
            // Buscarlo en los hijos, si hay.
            for (var j = 0; j < modulaciones[i].c.length; j++) {
                if (modulaciones[i].c[j].id == id)
                    return modulaciones[i].c[j];
            }
        }
    }
    return null;
}

// Datos de las modulaciones.
var modulaciones = Model.ModulacionesScript;

// Buscar modulaciones por número.
$("#searchDocument").on("input", function () {
    var value = $(this).val();

    if (value == "") {
        BusquedaOcultar();
        return;
    }

    value = value.toUpperCase();

    // Buscar el primer elemento con el nombre indicado.
    var busqueda = $("g[id^='Sen']").filter(function (key, item) {
        return $(item).text() === value;
    });

    if (busqueda.length === 0) {
        busqueda = $("g[id^='Sen'] text:contains('" + value + "')");
        if (busqueda.length == 0) {
            BusquedaOcultar();
            return;
        }
    }

    var $g = busqueda[0].nodeName === "g" ? busqueda.first() : busqueda.first().parent();

    moveToElement($(".graphlj-divgraph"), $g);

    // Marcarlo.
    var pos = GetTransformValues($g);

    var flecha = $("#svgBusqueda");
    flecha.show();
    flecha.attr("x", pos[0]);
    flecha.attr("y", pos[1] - 48);

    // Ocultarlo luego de 3 segundos.
    if (TimeOutFlecha != null)
        clearTimeout(TimeOutFlecha);
    TimeOutFlecha = setTimeout(function () { flecha.hide("Flecha"); }, 3000);
});

var TimeOutFlecha;

// Ocultar la flecha de búsqueda.
function BusquedaOcultar() {
    $("#svgBusqueda").hide();
}

// Funcionalidad del filtro por entidad.
$("#ulEntidades input:checkbox").on("change", AplicarFiltros);

// Aplica todos los filtros activados (excepto los de modulación).
function AplicarFiltros() {
    // Obtener la lista de todas las entidades.
    var allChecks = $("#ulEntidades input:checkbox");
    var entidadesSeleccionadas = "";

    for (var i = 0; i < allChecks.length; i++) {
        var ent = $(allChecks[i]);
        if (!ent.is(":checked"))
            continue;

        if (ent.attr("id") == "entTodas") {
            entidadesSeleccionadas = "";
            // Ocultar el combo.
            $("#divEntidades").attr("class", "btn-group");

            // Quitar todos los cheks.
            $("#ulEntidades input:checkbox").prop("checked", false);
            break;
        }

        entidadesSeleccionadas =
            entidadesSeleccionadas + "-" + ent.next().text().toUpperCase() + "-";
    }

    // Aplicar los filtros.
    $("g[id^='Sen']").each(function () {
        var $this = $(this);

        var aplicaFiltroEntidad = entidadesSeleccionadas != "" &&
            entidadesSeleccionadas.indexOf(("-" + $this.attr("data-entidadorigen").toUpperCase() + "-")) ==
            -1;

        if (aplicaFiltroEntidad) {
            // Si el elemento está afectado por el filtro, difuminarlo.
            $this.css("opacity", "0.3");
        } else {
            // Quitar todas las marcas de filtro/opacidad.
            $this.css("opacity", "1.0");
        }

    });
    hideSentenciaPopup();
}

// Para un transform-position retorna las coordenadas x,y
function GetTransformValues(element) {
    var rectPos = element.attr("transform").split("(")[1].split(")")[0];
    if (rectPos.indexOf(",") > -1)
        rectPos = rectPos.split(",");
    else
        rectPos = rectPos.split(" ");
    return rectPos;
}

// Inicializar el globo: "Usted está aqui".
$(document).ready(function () {
    // Posicionar el globo "Usted está aqui".
    var globo = $("#svgEstaAqui").first();
    var $ustedEstaAquiCaja = $("rect[class='UstedEstaAquiCaja']").first().parent();

    if ($ustedEstaAquiCaja.length == 0)
    {
        // Es un hijo. Buscar el padre.
        $ustedEstaAquiCaja = $("g.UstedEstaAquiCaja");
        $ustedEstaAquiCaja = $("#Sen" + $ustedEstaAquiCaja.attr("idparent"));
    }

    if (globo != null && $ustedEstaAquiCaja.length > 0) {
        var rectPos = GetTransformValues($ustedEstaAquiCaja);
        globo.attr("x", rectPos[0]);
        globo.attr("y", rectPos[1] - globo.attr("height") + 10);
        globo.attr("style", "display:inline;");

        // Ocultarlo luego de 3 segundos.
        setTimeout(function () { globo.hide("slow"); }, 3000);
    }

    moveToElement($(".graphlj-divgraph"), $(".UstedEstaAquiCaja").parent("g"));
});