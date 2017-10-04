$(document).ready(function () {
    $("#btn_buscar").click(btnBuscarOnClick);
});

function btnBuscarOnClick(e) {
    e.preventDefault();
    var opcionSeleccionada = $("select_editarDB option:selected").val();
    if (opcionSeleccionada === "-1") {
        toast("Debe seleccionar una opcion");
        return;
    }
    var error = function (error) {
        toast("Fallo al comunicarse con la api." + error.statusText + e.status);
    };
    switch (opcionSeleccionada) {
        case "1":
            makeAjaxCall("/api/Aulas/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        case "2":
            makeAjaxCall("/api/Disponibilidad/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        case "3":
            makeAjaxCall("/api/Aulas/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        case "4":
            makeAjaxCall("/api/Materias/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        case "5":
            makeAjaxCall("/api/Aulas/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        case "6":
            makeAjaxCall("/api/Profesor/GetAll",
                function (data) {

                },
                error,
                null, "GET"
            );
            break;
        default:
            break;
    }

}