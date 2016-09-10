$(document).ready(function () {

    //#region "Eventos invocados para el registro de usos desde el cliente."

    // Click en el evento para registro de estadísticas, solo para el uso sin parametros.
    $(".regUsoSinParametros").on("click", function (e) {
        var sUso = getValueAttr(this, "nomuso");
        callRegistrarUso(sUso);
    });

    // Click en el evento para registro de estadísticas, solo el uso con el parametro MetaData.
    $(document).on("click", ".regUsoMetaData", function (e) {
        var sTxt = getValueAttr(this);
        var sUso = getValueAttr(this, "nomuso");
        var sPar = getValueAttr(this, "metadata");
        var sText = getValueAttr(this, "metadata-text");
        if (sText !== "")
            sPar = sPar.replace(sText, sTxt);
        else
            sPar += sTxt;
        var parCampos = new Array("MetaData", sPar);
        callRegistrarUso(sUso, parCampos);
    });

    // Click en el evento para registro de estadísticas, solo el uso con el parametro Procedencia.
    $(document).on("click", ".regUsoOrigen", function (e) {
        var sTxt = getValueAttr(this);
        var sUso = getValueAttr(this, "nomuso");
        var sOrgn = getValueAttrParent2(this, "origen");
        var sOrgMtd = getValueAttrParent2(this, "origen-metadata");
        var parCampos = new Array("Procedencia", sOrgn);
        if (sOrgMtd !== "")
            parCampos.push("MetaData", sOrgMtd);
        callRegistrarUso(sUso, parCampos);
    });

    // Click en el evento para registro de estadísticas, solo el uso con los parametros MetaData y Origen.
    $(document).on("click", ".regUsoMetaDataOrigen", function (e) {
        var sTxt = getValueAttr(this);
        var sUso = getValueAttr(this, "nomuso");
        var sPar = getValueAttr(this, "metadata");
        var sText = getValueAttr(this, "metadata-text");
        var sValues = getValueAttr(this, "metadata-text-values");
        var sOrgn = getValueAttrParent2(this, "origen");
        var sOrgMtd = getValueAttrParent2(this, "origen-metadata");
        if (sText !== "") {
            if (sValues !== "") {
                var sTid = "(";
                var sIds = sValues.split("|");
                jQuery.each(sIds, function (i, sVal) {
                    if (i > 0)
                        sTid += ")-(";
                    sTid += $("#" + sVal).val();
                });
                sTid += ")";
                sPar = sPar.replace(sText, sTid);
            } else
                sPar = sPar.replace(sText, sTxt);
        } else
            sPar += sTxt;
        if (sOrgMtd != "") {
            if (!/\[/i.test(sPar))
                sPar = "[" + sPar + "]";
            sPar += "[" + sOrgMtd + "]";
        }
        var parCampos = new Array("MetaData", sPar);
        parCampos.push("Procedencia", sOrgn);
        callRegistrarUso(sUso, parCampos);
    });

    // Click en el evento para registro de estadísticas, solo para el uso sin parametros.
    $(".regUsoConParametros").on("click", function (e) {
        var sUso = $(this).attr("data-nomuso");
        var sPar = $(this).attr("data-nomuser");
        callRegistrarUso(sUso, sPar);
    });

    //#endregion

    //#region "Funciones adicionales para el registro de las estadisticas."

    function getValueAttr(objThis, strAttr) {
        var sReturn;

        if ((strAttr != undefined) && (strAttr !== "")) {
            if (!/^data-/i.test(strAttr))
                strAttr = "data-" + strAttr;
            sReturn = $(objThis).attr(strAttr);
        } else {
            sReturn = $(objThis).text().replace(/[\n\r\t]+|^\s+|\s+$/, "").trim();
        }
        if (!sReturn)
            sReturn = "";
        return sReturn;
    }

    function getValueAttrParent2(objThis, strAttr) {
        if (!/^data-/i.test(strAttr))
            strAttr = "data-" + strAttr;

        var sReturn = $(objThis).parent().parent().attr(strAttr);
        if (!sReturn)
            sReturn = "";
        return sReturn;
    }

    //#endregion
});