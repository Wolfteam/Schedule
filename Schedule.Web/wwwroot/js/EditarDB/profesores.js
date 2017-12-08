function confirmCreateProfesores() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_profesores").valid();        
                if (!isValid)
                    return false; 
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
                    text: obj.codigoPrioridad + ". Horas a cumplir: " + obj.horasACumplir
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options);
        });

        globalFunction = function () {
            onRequestsFinished("#form_profesores");
        };
        checkPendingRequest();
        validateProfesorHandler();
    };
    confirmAlert("Agregar Profesores", "blue", "fa fa-plus", "url:" + urlBase + "modals/Profesores.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
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
                var isValid = $("#form_profesores").valid();        
                if (!isValid)
                    return false; 
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
                    text: obj.codigoPrioridad + ". Horas a cumplir: " + obj.horasACumplir
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options).val(idPrioridad);
        });

        globalFunction = function () {
            onRequestsFinished("#form_profesores");
        };
        checkPendingRequest();

        content.find("#cedula").val(cedula);
        content.find("#nombre").val(nombre);
        content.find("#apellido").val(apellido);
        content.find(".onlyNum").on("keypress", onlyNum);
        validateProfesorHandler();
    };
    confirmAlert("Editar Profesores", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Profesores.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Crea un profesor
 * @param {Object} profesor Objeto de tipo profesor 
 */
function createProfesor(profesor) {
    $("#barra-progeso").show();
    makeAjaxCall(apiProfesores,
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
    makeAjaxCall(apiProfesores + "/" + cedula,
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
 * @param {Function} callback Funcion de callback
 */
function getAllProfesores(callback) {
    makeAjaxCall(apiProfesores,
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr);
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
    makeAjaxCall(apiProfesores + "/" + cedula,
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

/**
 * Valdia que una profesor tenga todas sus propiedades
 * @param {string} selector Selector del formulario profesor (#form_profesores por defecto)
 */
function validateProfesorHandler(selector = "#form_profesores"){
    var valdiate = $(selector).validate({
        ignore:[],
        rules: {
            cedula: {
                required: true,
                minlength: 8,
                maxlength: 8
            },
            select_prioridad: "required",
            nombre: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            apellido: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
        },
        messages: {
            cedula: {
                required: "La cedula es requerida. ",
                minlength: "La cedula debe contener 8 digitos. ",
                maxlength: "La cedula debe contener 8 digitos. "
            },
            select_prioridad: "Debe seleccionar una prioridad .",
            nombre: {
                required: "El nombre es requerido. ",
                minlength: "El nombre debe contener 3 caracteres como minimo. ",
                maxlength: "El nombre debe contener 20 caracteres como maximo. "
            },
            apellido: {
                required: "El apellido es requerido. ",
                minlength: "El apellido debe contener 3 caracteres como minimo. ",
                maxlength: "El apellido debe contener 20 caracteres como maximo. "
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(selector);
        }
    });
}