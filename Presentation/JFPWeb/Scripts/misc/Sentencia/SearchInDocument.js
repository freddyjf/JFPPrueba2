$(document).ready(function () {
    // Buscar en este documento ===================================================
    var CurrentHit = 0;
    var TotalHits = 0;

    var scapeCharacters = "\\" + [
        "|",
        "(",
        ")",
        "[",
        "]",
        "\\",
        "+",
        "*",
        "?",
        "^",
        "$",
        ".",
        "{",
        "}"
    ].join("\\");

    var regexScape = new RegExp("([" + scapeCharacters + "])", "g");

    $("#searchBy").on("input", function () {
        var $this = $(this);
        delay("searchBy", function () {
            var valorBuscar = $this.val();
            var expresion = !valorBuscar ? "/#@#//" : valorBuscar
                .trim()
                .replace(regexScape, "\\$1");;

            expresion = includeRegexAccents(expresion);
            var hits = $(".marcoSentencia ").blast({
                delimiter: new RegExp(expresion, "i")
            });

            TotalHits = hits.length;
            CurrentHit = 0;
            ScrollToHit(0);
        }, 800);
    });

    // Siguiente hit.
    $("#searchByNext").on("click", function () {
        CurrentHit++;
        if (CurrentHit >= TotalHits)
            CurrentHit = 0;
        ScrollToHit(CurrentHit);
    });

    // Anterior hit.
    $("#searchByPrevious").on("click", function () {
        CurrentHit--;
        if (CurrentHit < 0)
            CurrentHit = TotalHits - 1;
        ScrollToHit(CurrentHit);
    });

    // Hacer scroll hasta el hit seleccionado.
    function ScrollToHit(hitNumber) {
        var AllHits = $("span.blast[aria-hidden=true]");
        if (AllHits.length > 0 && hitNumber < TotalHits)
            scrollToElement($(AllHits[hitNumber]).offset().top);

        callRegistrarUso("BuscarEnDocumento", ["MetaData", "[TerminoBusqueda:" + $("#searchBy").val() + "]"]);
    }

    //  Si hay una aclaración de voto o salvamento, haga scroll en el documento.

    (function () {

        var findTextToScroll = [
            "Aclaración[\\s]+de[\\s]+voto",
            "Salvamento[\\s]+de[\\s]+voto"
        ];

        findTextToScroll = $.map(findTextToScroll, function (value) {
            return new RegExp(includeRegexAccents(value), "i");
        });

        var idFound = function (id) {
            var $tag = $("#" + id);

            setTimeout(function () {
                scrollToElement($tag.offset().top);
            }, 1000);
        };

        var keepSearching = true;

        $(".contenidoSentencia span").each(function () {
            if (!keepSearching) {
                return false;
            }

            var thisSpan = this;

            $.each(findTextToScroll, function () {
                var regex = this;
                var text = thisSpan.textContent || thisSpan.innerText;

                if (regex.test(text)) {
                    idFound($(thisSpan).attr("id"));
                    return keepSearching = false;
                }
            });
        });
    })();
});