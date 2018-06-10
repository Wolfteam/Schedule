$(document).ready(function () {
    $("#user").html(getCookie("User"));
    showLoading(true);
    getCurrentPeriodo((data, textStatus, xhr) => {
        if (data === undefined || data === null)
            $("#periodo").html("No se ha encontrado un periodo activo");
        else
            $("#periodo").html(data.nombrePeriodo);
    }); 
});

