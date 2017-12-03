var onError = function (error) {
    toast("Error, Fallo al comunicar con la api. Codigo: " + error.status + ", " + error.statusText);
};

var onComplete = function () {
    $("#barra-progeso").hide();
};

$(document).ready(function () {
    $("#user").html(getCookie("User"));
    $("#barra-progeso").show();
    getCurrentPeriodo(function (data, textStatus, xhr) {
        if (data === undefined || data === null)
            $("#periodo").html("No se ha encontrado un periodo activo");
        else
            $("#periodo").html(data.nombrePeriodo);
    }); 
});

