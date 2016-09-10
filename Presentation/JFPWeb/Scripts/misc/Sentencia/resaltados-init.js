$(document).ready(function () {
    var $model = $("#model");
    var data = JSON.parse($model.text());
    $model.remove();

    if (data.CodSuscriptor === -1) { return; }

    var mensajeNoResaltado = $("<li />").append($("<p />", { text: $("#no-hay-resaltados").text().trim() }));

    var marker = new LegisMarker({
        dataOnChange: function (arrayData) {
            if (arrayData.length === 0) {
                $("#MisResaltados").html(mensajeNoResaltado);
            }
        }
});
    marker.init(".marcoSentencia");
    marker.GetAllMarkers(data.CodSuscriptor, data.CodSentencia);
});