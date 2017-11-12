function confirmCreateProfesores() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var profesor = prepareProfesorData(this.$content);
                createProfesor(profesor);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        var content = this.$content;
        content.find(".onlyNum").on("keypress", onlyNum);

        getAllPrioridades(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.id,
                    text: obj.codigoPrioridad
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options);
        });

        globalFunction = function () {
            $('select').material_select();
            $(".progressBar").hide();
            $("#form_profesores").show();
        };
        checkPendingRequest();
    };
    confirmAlert("Agregar Profesores", "blue", "fa fa-plus", "url:../modals/ContenidoProfesores.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

function confirmDeleteProfesores(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteProfesores(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Profesor", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditProfesores(cedula, nombre, apellido, idPrioridad) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var profesor = prepareProfesorData(this.$content);
                updateProfesor(cedula, profesor);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };

    var onContentReady = function () {
        var content = this.$content;
        getAllPrioridades(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.id,
                    text: obj.codigoPrioridad
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options).val(idPrioridad);
        });

        globalFunction = function () {
            $('select').material_select();
            $(".progressBar").hide();
            $("#form_profesores").show();
            content.find("#form_profesores :input").each(function () {
                $(this).focus();
            });
        };
        checkPendingRequest();

        content.find("#cedula").val(cedula);
        content.find("#nombre").val(nombre);
        content.find("#apellido").val(apellido);
        content.find(".onlyNum").on("keypress", onlyNum);
    };
    confirmAlert("Editar Profesores", "orange", "fa fa-pencil-square-o", "url:../modals/ContenidoProfesores.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Crea un profesor
 * @param {Object} profesor Objeto de tipo profesor 
 */
function createProfesor(profesor) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Profesor",
        function (data, textStatus, xhr) {
            if (xhr.status !== 500) {
                var buttons = {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se guardo el profesor correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo guardar el profesor.");
        },
        onError, profesor, "POST", onComplete
    );
}

/**
 * Elimina el profesor indicado
 * @param {number} cedula Cedula del profesor a eliminar
 */
function deleteProfesor(cedula) {
    makeAjaxCall("/api/Profesor/" + cedula,
        function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el profesor.");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina los profesores indicados
 * @param {number[]} arrayCedulas Array de cedulas de los profesores a eliminar
 */
function deleteProfesores(arrayCedulas) {
    $("#barra-progeso").show();
    for (var i = 0; i < arrayCedulas.length; i++) {
        deleteProfesor(arrayCedulas[i].cedula);
    }
    globalFunction = function () {
        $("#btn_buscar").trigger("click");
        var buttons = {
            Ok: {
                text: 'Ok',
                btnClass: 'btn-green',
                action: function () {}
            }
        };
        confirmAlert("Proceso completado", "green", "fa fa-check", "Se completo el proceso correctamente.", buttons);
        $("#barra-progeso").hide();
    };
    checkPendingRequest();
}

/**
 * Obtiene todos los profesores
 */
function getAllProfesores() {
    makeAjaxCall("/api/Profesor",
        function (data) {
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
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto
 * @param {Object} object Objeto de tipo Jquery-Confirm
 * @returns {Object} Objeto de tipo Profesor
 */
function prepareProfesorData(object) {
    var profesor = {
        cedula: object.find("#cedula").val(),
        nombre: object.find("#nombre").val(),
        apellido: object.find("#apellido").val(),
        idPrioridad: object.find("#select_prioridad").val()
    };
    return profesor;
}

/**
 * Actualiza una Materia en particular
 * @param {number} cedula Cedula del profesor a actualizar
 * @param {Object} profesor Objeto de tipo Profesor
 */
function updateProfesor(cedula, profesor) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Profesor/" + cedula,
        function (data, textStatus, xhr) {
            if (xhr.status === 204) {
                var buttons = {
                    ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo el profesor correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar el profesor.");
        },
        onError, profesor, "PUT", onComplete
    );
}