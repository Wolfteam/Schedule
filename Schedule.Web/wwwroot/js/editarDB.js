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

    var error = function (error) {
        toast("Falla al comunicarse con la api." + error.statusText + error.status);
        $("#barra-progeso").hide();
    };

    switch (opcionSeleccionada) {
        case "1": //Aulas
            makeAjaxCall("/api/Aulas/GetAll",
                function (data) {
                    var titulos = ["Id", "Nombre", "Capacidad", "Tipo Aula"];
                    createTable("#tabla", titulos);
                    var columnsData = [{
                            "data": "id"
                        },
                        {
                            "data": "nombre"
                        },
                        {
                            "data": "capacidad"
                        },
                        {
                            "data": "idTipoAula"
                        }
                    ];
                    //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                    initDataTable("#datatable", data, columnsData, 0, false, "single");
                    $("#barra-progeso").hide();
                },
                error,
                null, "GET"
            );
            break;

        case "3": //Materias
            makeAjaxCall("/api/Materias/GetAll",
                function (data) {
                    var titulos = ["Codigo", "Asignatura", "IdSemestre", "Semestre",
                        "IdCarrera", "Carrera", "IdTipoAulaMateria", "Tipo Materia",
                        "Horas Academicas Semanales", "Horas Academicas Totales"
                    ];
                    createTable("#tabla", titulos);
                    var columnsData = [{
                            "data": "codigo"
                        },
                        {
                            "data": "nombre"
                        },
                        {
                            "data": "semestre.id"
                        },
                        {
                            "data": "semestre.nombreSemestre"
                        },
                        {
                            "data": "carrera.id"
                        },
                        {
                            "data": "carrera.nombreCarrera"
                        },
                        {
                            "data": "tipoMateria.id"
                        },
                        {
                            "data": "tipoMateria.nombre"
                        },
                        {
                            "data": "horasAcademicasSemanales"
                        },
                        {
                            "data": "horasAcademicasTotales"
                        }
                    ];
                    //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                    initDataTable("#datatable", data, columnsData, [2, 4, 6], false, "single");
                    $("#barra-progeso").hide();
                },
                error,
                null, "GET"
            );
            break;
        case "4": //Profesores
            makeAjaxCall("/api/Profesor/GetAll",
                function (data) {
                    var titulos = [
                        "Cedula", "Nombre", "Apellido",
                        "IdPrioridad", "Prioridad", "Horas a Cumplir"
                    ];
                    createTable("#tabla", titulos);
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
                    initDataTable("#datatable", data, columnsData, 3, false, "single");
                    $("#barra-progeso").hide();
                },
                error,
                null, "GET"
            );
            break;
        case "5": //ProfesorxMateria
            makeAjaxCall("/api/ProfesorMateria/GetAll",
                function (data) {
                    var titulos = [
                        "Id", "Cedula", "Nombre", "Apellido",
                        "Codigo", "Asignatura", "Semestre", "Carrera"
                    ];
                    createTable("#tabla", titulos);
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
                            "data": "materia.nombre"
                        },
                        {
                            "data": "materia.semestre.nombreSemestre"
                        },
                        {
                            "data": "materia.carrera.nombreCarrera"
                        },
                    ];
                    //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
                    initDataTable("#datatable", data, columnsData, 0, false, "single");
                    $("#barra-progeso").hide();
                },
                error,
                null, "GET"
            );
            break;
        case "6": //Secciones
            makeAjaxCall("/api/Secciones/GetAll",
                function (data) {
                    console.log(data);
                    var titulos = [
                        "Codigo Materia", "Asignatura", "Semestre",
                        "Secciones", "Cantidad Alumnos", "Carrera"
                    ];
                    createTable("#tabla", titulos);
                    var columnsData = [{
                            "data": "materia.codigo"
                        },
                        {
                            "data": "materia.nombre"
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
                    initDataTable("#datatable", data, columnsData, -1, false, "single");
                    $("#barra-progeso").hide();
                },
                error,
                null, "GET"
            );
            break;
        default:
            toast("Debe seleccionar una opcion");
            $("#barra-progeso").hide();
            break;
    }

    dismissToast();
}

function btnCrearOnClick() {
    var opcionSeleccionada = $("#select_editarDB").val();
    switch (opcionSeleccionada) {
        case "1":
            var buttons = {
                ok: {
                    text: 'Guardar',
                    btnClass: 'btn-blue',
                    action: function () {}
                },
                cancel: {
                    text: 'Cancelar',
                    action: function () {}
                }
            };
            var contenido =
                '<form class="col s12">' +
                '<div class="row">' +
                '<div class="input-field col s12">' +
                '<i class="material-icons prefix">comment</i>' +
                '<input id="icon_prefix" type="text" class="validate">' +
                '<label for="icon_prefix">Nombre del aula</label>' +
                '</div>' +
                '</div>' +
                '<div class="row">' +
                '<div class="input-field col s12">' +
                '<i class="material-icons prefix">filter_9_plus</i>' +
                '<input id="icon_telephone" type="number" class="validate">' +
                '<label for="icon_telephone">Capacidad</label>' +
                '</div>' +
                '</div>' +
                '<div class="row">' +
                '<div class="switch"><label>Teoria<input type="checkbox"><span class="lever"></span>Laboratorio</label></div>' +
                '</div>' +
                '</form>';
            confirmAlert("Agregar Aulas", "blue", "fa fa-plus", contenido, buttons);
            
            break;
        case "3":

            break;
        case "4":

            break;
        case "5":

            break;
        case "6":

            break;
        default:
            break;
    }
}

function btnEditarOnClick() {
    console.log("editar");
}

function btnBorrarOnClick() {
    console.log("borrar");
}
//------------------------OnClicks------------------------//


function removeTable(selector = "#tabla div") {
    $(selector).remove();
}