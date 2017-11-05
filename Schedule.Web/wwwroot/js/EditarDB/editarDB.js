var onError = function (error) {
    toast("Error, Fallo al comunicar con la api. Codigo: " + error.status + ", " + error.statusText);
};
var onComplete = function () {
    $("#barra-progeso").hide();
};

$(document).ready(function () {
    $("#btn_buscar").click(btnBuscarOnClick);

    $("#btn-crear").click(btnCrearOnClick);
    $("#btn-editar").click(btnEditarOnClick);
    $("#btn-borrar").click(btnBorrarOnClick);

    $("#select_editarDB").change(function () {
        removeTable();
        $("#btn_buscar").addClass("pulse");
        $("#btn-flotante").hide();
    });
});

//------------------------OnClicks------------------------//
function btnBuscarOnClick() {
    removeTable();
    $("#btn_buscar").removeClass("pulse");
    $("#barra-progeso").show();
    $("#btn-flotante").show();

    var opcionSeleccionada = $("#select_editarDB").val();

    switch (opcionSeleccionada) {
        case "1": //Aulas
            getAllAulas();
            break;
        case "3": //Materias
            getAllMaterias();
            break;
        case "4": //Profesores
            getAllProfesores();
            break;
        case "5": //ProfesorxMateria
            getAllProfesoresxMateria();
            break;
        case "6": //Secciones
            getAllSecciones();
            break;
        default:
            toast("Debe seleccionar una opcion");
            $("#barra-progeso").hide();
            $("#btn-flotante").hide();
            break;
    }
    dismissToast();
}

function btnCrearOnClick() {
    var opcionSeleccionada = $("#select_editarDB").val();
    switch (opcionSeleccionada) {
        case "1":
            confirmCreateAulas();
            break;
        case "3":
            confirmCreateMaterias();
            break;
        case "4":

            break;
        case "5":

            break;
        case "6":

            break;
        default:
            toast("Debe seleccionar una opcion");
            break;
    }
}

function btnEditarOnClick() {
    var opcionSeleccionada = $("#select_editarDB").val();
    var data = $('#datatable').DataTable().rows('.selected').data();
    if (data.length === 0) {
        toast("Debes seleccionar una fila.");
        return;
    }

    if (data.length > 1) {
        toast("No puedes editar mas de un registro a la vez.");
        return;
    }

    switch (opcionSeleccionada) {
        case "1":
            confirmEditAulas(data[0].idAula, data[0].nombreAula, data[0].capacidad, data[0].idTipo);
            break;
        case "3":
            confirmEditMaterias(data[0].codigo, data[0].asignatura, data[0].idSemestre, data[0].idTipo,
                data[0].idCarrera, data[0].horasAcademicasTotales, data[0].horasAcademicasSemanales);
            break;
        case "4":

            break;
        case "5":

            break;
        case "6":

            break;
        default:
            toast("Debe seleccionar una opcion.");
            break;
    }
}

function btnBorrarOnClick() {
    var opcionSeleccionada = $("#select_editarDB").val();
    var data = $('#datatable').DataTable().rows('.selected').data();
    if (data.length === 0) {
        toast("Debes seleccionar una fila.");
        return;
    }

    switch (opcionSeleccionada) {
        case "1":
            confirmDeleteAula(data);
            break;
        case "3":
            confirmDeleteMaterias(data);
            break;
        case "4":

            break;
        case "5":

            break;
        case "6":

            break;
        default:
            toast("Debe seleccionar una opcion.");
            break;
    }
}
//------------------------OnClicks------------------------//

function removeTable(selector = "#tabla div") {
    $(selector).remove();
}