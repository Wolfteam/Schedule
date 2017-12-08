function confirmCreateProfesorMateria() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_profesores_materias").valid();        
                if (!isValid)
                    return false;
                var relacion = prepareProfesorMateriaData(this.$content);
                createProfesorMateria(relacion);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        var content = this.$content;
        getAllProfesores(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options);
        });

        getAllMaterias(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.codigo,
                    text: obj.codigo + " " + obj.asignatura
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_materia").append(options);
        });

        globalFunction = function () {
            onRequestsFinished("#form_profesores_materias");
        };
        checkPendingRequest();
        validateProfesorMateriaHandler();
    };
    confirmAlert("Agregar Relacion", "blue", "fa fa-plus", "url:" + urlBase + "modals/ProfesorMateria.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

function confirmDeleteProfesorMateria(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteProfesoresMateria(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Relacion", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditProfesorMateria(id, cedula, codigo) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var isValid = $("#form_profesores_materias").valid();        
                if (!isValid)
                    return false;
                var relacion = prepareProfesorMateriaData(this.$content);
                updateProfesorMateria(id, relacion);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };

    var onContentReady = function () {
        var content = this.$content;
        getAllProfesores(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options);
            content.find("#select_profesor").append(options).val(cedula);
        });

        getAllMaterias(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.codigo,
                    text: obj.codigo + " " + obj.asignatura
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_materia").append(options);
            content.find("#select_materia").append(options).val(codigo);
        });

        globalFunction = function () {
            onRequestsFinished("#form_profesores_materias");
        };
        checkPendingRequest();
        validateProfesorMateriaHandler();
    };
    confirmAlert("Editar Relacion", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/ProfesorMateria.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Crea una relacion entre un profesor y una materia
 * @param {Object} relacion Objeto de tipo ProfesorMateria
 */
function createProfesorMateria(relacion) {
    $("#barra-progeso").show();
    makeAjaxCall(apiProfesorMateria,
        function (data, textStatus, xhr) {
            if (xhr.status !== 200) {
                var buttons = {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo la relacion correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo crear la relacion.");
        },
        onError, relacion, "POST", onComplete
    );
}

/**
 * Elimina una relacion indicada
 * @param {number} id Id de la relacion a eliminar
 */
function deleteProfesorMateria(id) {
    makeAjaxCall(apiProfesorMateria + "/" + id,
        function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la relacion.");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina las relaciones indicadas
 * @param {number[]} arrayID Array de ID de las relaciones a eliminar
 */
function deleteProfesoresMateria(arrayID) {
    $("#barra-progeso").show();
    for (var i = 0; i < arrayID.length; i++) {
        deleteProfesorMateria(arrayID[i].id);
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
 * Obtiene todos los profesores x materias
 * @param {Function} callback Funcion de callback
 */
function getAllProfesoresMateria(callback) {
    makeAjaxCall(apiProfesorMateria,
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr);
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto
 * @param {Object} object Objeto de tipo Jquery-Confirm
 * @returns {Object} Objeto de tipo ProfesorMateria
 */
function prepareProfesorMateriaData(object) {
    var relacion = {
        cedula: object.find("#select_profesor").val(),
        codigo: object.find("#select_materia").val()
    };
    return relacion;
}

/**
 * Actualiza una relacion en particular
 * @param {number} id Id de la relacion
 * @param {Object} relacion Objeto de tipo ProfesorMateria
 */
function updateProfesorMateria(id, relacion) {
    $("#barra-progeso").show();
    makeAjaxCall(apiProfesorMateria + "/" + id,
        function (data, textStatus, xhr) {
            if (xhr.status === 204) {
                var buttons = {
                    ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo la relacion correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar la relacion.");
        },
        onError, relacion, "PUT", onComplete
    );
}

/**
 * Valdia que una relacion profesor_materia tenga todas sus propiedades
 * @param {string} selector Selector del formulario profesor_materia (#form_profesores_materias por defecto)
 */
function validateProfesorMateriaHandler(selector = "#form_profesores_materias") {
    var valdiate = $(selector).validate({
        rules: {
            select_profesor: {
                required: true
            },
            select_materia: {
                required: true
            }
        },
        messages: {
            select_profesor: {
                required: "Debe seleccionar un profesor. "
            },
            select_materia: {
                required: "Debe seleccionar una materia. "
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(selector);
        }
    });
}