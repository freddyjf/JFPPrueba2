// Para los llamados de registro de estadisticas desde el cliente.
var sArchivo = "Estadisticas.js";
var sFuncion = "Raiz";

function callRegistrarUso(tipo, campos) {
    sFuncion = "callRegistrarUso";
    try {
        if (tipo.length < 1) {
            if (top.bDebug) { top.alert(sArchivo + " [" + sFuncion + "] ERROR: Objeto de usos vacio."); }
        } else {
            if ((campos == undefined) || (campos.length < 1))
                campos = new Array();
            $.ajax({
                type: "POST",
                url: "/Statistics/RegistraEstadisticasCliente",
                data: { uso: tipo, campos: campos },
                cache: false,
                dataType: "json"
            });
        }
    }
    catch (e) {
        if (top.bDebug) { top.alert(sArchivo + " [" + sFuncion + "] ERROR: " + e.description, "Estadisticas de Logos"); }
    }
}