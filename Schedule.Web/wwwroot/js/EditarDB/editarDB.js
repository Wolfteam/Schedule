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
            getAllAulas(function (data, textStatus, xhr) {
                titulos = ["Id", "Nombre", "Capacidad", "Tipo Aula"];
                columnsData = [{
                        "data": "idAula"
                    },
                    {
                        "data": "nombreAula"
                    },
                    {
                        "data": "capacidad"
                    },
                    {
                        "data": "tipoAula.nombreTipo"
                    }
                ];
                createTable("#tabla", titulos);
                initDataTable("#datatable", data, columnsData, 0, false, "multi");
            });
            break;
        case "3": //Materias
            getAllMaterias(function (data, textStatus, xhr) {
                var titulos = ["Codigo", "Asignatura", "IdSemestre", "Semestre",
                    "IdCarrera", "Carrera", "IdTipoAulaMateria", "Tipo Materia",
                    "Horas Academicas Semanales", "Horas Academicas Totales"
                ];
                var columnsData = [{
                        "data": "codigo"
                    },
                    {
                        "data": "asignatura"
                    },
                    {
                        "data": "semestre.idSemestre"
                    },
                    {
                        "data": "semestre.nombreSemestre"
                    },
                    {
                        "data": "carrera.idCarrera"
                    },
                    {
                        "data": "carrera.nombreCarrera"
                    },
                    {
                        "data": "tipoMateria.idTipo"
                    },
                    {
                        "data": "tipoMateria.nombreTipo"
                    },
                    {
                        "data": "horasAcademicasSemanales"
                    },
                    {
                        "data": "horasAcademicasTotales"
                    }
                ];
                //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                createTable("#tabla", titulos);
                initDataTable("#datatable", data, columnsData, [2, 4, 6], false, "multi");
            });
            break;
        case "4": //Profesores
            getAllProfesores(function (data, textStatus, xhr) {
                var titulos = [
                    "Cedula", "Nombre", "Apellido",
                    "IdPrioridad", "Prioridad", "Horas a Cumplir"
                ];
                var columnsData = [{
                        "data": "cedula"
                    },
                    {
                        "data": "nombre"
                    },
                    {
                        "data": "apellido"
                    },
                    {
                        "data": "prioridad.id"
                    },
                    {
                        "data": "prioridad.codigoPrioridad"
                    },
                    {
                        "data": "prioridad.horasACumplir"
                    }
                ];
                //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                createTable("#tabla", titulos);
                initDataTable("#datatable", data, columnsData, 3, false, "multi");
            });
            break;
        case "5": //ProfesorxMateria
            getAllProfesoresMateria(function (data, textStatus, xhr) {
                var titulos = [
                    "Id", "Cedula", "Nombre", "Apellido",
                    "Codigo", "Asignatura", "Semestre", "Carrera"
                ];
                var columnsData = [{
                        "data": "id"
                    },
                    {
                        "data": "profesor.cedula"
                    },
                    {
                        "data": "profesor.nombre"
                    },
                    {
                        "data": "profesor.apellido"
                    },
                    {
                        "data": "materia.codigo"
                    },
                    {
                        "data": "materia.asignatura"
                    },
                    {
                        "data": "materia.semestre.nombreSemestre"
                    },
                    {
                        "data": "materia.carrera.nombreCarrera"
                    },
                ];
                createTable("#tabla", titulos);
                initDataTable("#datatable", data, columnsData, 0, false, "multi");
            });
            break;
        case "6": //Secciones
            getAllSecciones(function (data, textStatus, xhr) {
                var titulos = [
                    "Codigo Materia", "Asignatura", "Semestre",
                    "Secciones", "Cantidad Alumnos", "Carrera"
                ];
                var columnsData = [{
                        "data": "materia.codigo"
                    },
                    {
                        "data": "materia.asignatura"
                    },
                    {
                        "data": "materia.semestre.nombreSemestre"
                    },
                    {
                        "data": "numeroSecciones"
                    },
                    {
                        "data": "cantidadAlumnos"
                    },
                    {
                        "data": "materia.carrera.nombreCarrera"
                    }
                ];
                //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                createTable("#tabla", titulos);
                initDataTable("#datatable", data, columnsData, -1, false, "multi");
            });
            break;
        default:
            toast("Debe seleccionar una opcion");
            $("#barra-progeso").hide();
            $("#btn-flotante").hide();
            break;
    }
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
            confirmCreateProfesores();
            break;
        case "5":
            confirmCreateProfesorMateria();
            break;
        case "6":
            confirmCreateSecciones();
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
            confirmEditAulas(data[0].idAula, data[0].nombreAula, data[0].capacidad, data[0].tipoAula.idTipo);
            break;
        case "3":
            confirmEditMaterias(data[0].codigo, data[0].asignatura, data[0].semestre.idSemestre,
                data[0].tipoMateria.idTipo, data[0].carrera.idCarrera, data[0].horasAcademicasTotales, data[0].horasAcademicasSemanales);
            break;
        case "4":
            confirmEditProfesores(data[0].cedula, data[0].nombre, data[0].apellido, data[0].prioridad.id);
            break;
        case "5":
            confirmEditProfesorMateria(data[0].id, data[0].profesor.cedula, data[0].materia.codigo);
            break;
        case "6":
            confirmEditSecciones(data[0].materia.codigo, data[0].cantidadAlumnos, data[0].numeroSecciones);
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
            confirmDeleteProfesores(data);
            break;
        case "5":
            confirmDeleteProfesorMateria(data);
            break;
        case "6":
            confirmDeleteSecciones(data);
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