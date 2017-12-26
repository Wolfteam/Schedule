function confirmCreateUsuarios() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                // var isValid = $("#form_usuarios").valid();        
                // if (!isValid)
                //     return false;  
                // var usuario = prepareUsuarioData();
                // createUsuario(usuario);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        globalFunction = function () {
            onRequestsFinished("#form_usuarios");
        };
        checkPendingRequest();
        //this.$content.find("#capacidad").on("keypress", onlyNum);
        validateUsuarioHandler();
    };
    confirmAlert("Agregar Usuarios", "blue", "fa fa-plus", "url:" + urlBase + "modals/Usuarios.html", buttons, onContentReady);
}

function confirmDeleteUsuarios(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteUsuarios(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Usuario", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditUsuario(cedula, username, password, idPrivilegios) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {           
                // var isValid = $("#form_aulas").valid();        
                // if (!isValid)
                //     return false;  
                // var aula = prepareAulaData();
                // updateAula(this.$content.find("#id_aula").val(), aula);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        globalFunction = function () {
            onRequestsFinished("#form_usuarios");
        };
        Materialize.updateTextFields();
        checkPendingRequest();
        //this.$content.find("#capacidad").on("keypress", onlyNum);
        validateUsuarioHandler();
    };
    confirmAlert("Editar Usuario", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Usuarios.html", buttons, onContentReady);
}


/**
 * Crea una usuario
 * @param {Object} usuario Objeto de tipo usuario
 */
function createUsuario(usuario) {
    $("#barra-progeso").show();
    makeAjaxCall(apiAccount,
        function (data, textStatus, xhr) {
            if (xhr.status !== 500) {
                var buttons = {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo el usuario correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo crear el usuario.");
        },
        onError, usuario, "POST", onComplete
    );
}

/**
 * Elimina el usuario indicado
 * @param {number} cedula cedula del usuario a eliminar
 */
function deleteUsuario(cedula) {
    makeAjaxCall(apiAccount + "/" + cedula,
        function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el usuario.");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina varios usuarios selecionadas
 * @param {number[]} cedulaArray Array de cedulas de los usuarios a eliminar
 */
function deleteUsuarios(cedulaArray) {
    $("#barra-progeso").show();
    for (var i = 0; i < cedulaArray.length; i++) {
        deleteAula(cedulaArray[i].cedula);
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
 * Obtiene todos los usuarios registrados
 * @param {Function} callback Funcion de callback
 */
function getAllUsuarios(callback) {
    makeAjaxCall(apiAccount,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto
 * tipo usuario
 * @returns {Object} Objeto de tipo usuario
 */
function prepareUsuarioData() {
    var usuario = {
        // nombreAula: $("#nombre_aula").val(),
        // capacidad: $("#capacidad").val(),
        // idTipo: $("#tipo_aula").is(":checked") ? 2 : 1
    };
    return usuario;
}

/**
 * Actualiza un usuario
 * @param {number} cedula Cedula del usuario a actualizar
 * @param {Object} usuario Objeto de tipo usuario
 */
function updateUsuario(cedula, usuario) {
    $("#barra-progeso").show();
    makeAjaxCall(apiAccount + "/" + cedula,
        function (data, textStatus, xhr) {
            if (xhr.status === 204) {
                var buttons = {
                    ok: {
                        text: 'Ok',
                        btnClass: 'btn-green',
                        action: function () {}
                    }
                };
                confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo el usuario correctamente.", buttons);
                $("#btn_buscar").trigger("click");
                return;
            }
            confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar el usuario.");
        },
        onError, usuario, "PUT", onComplete
    );
}

/**
 * Valdia que un usuario tenga todas sus propiedades
 * @param {string} selector Selector del formulario aula (#form_usuarios por defecto)
 */
function validateUsuarioHandler(selector = "#form_usuarios") {
    var valdiate = $(selector).validate({
        rules: {
            nombre_aula: {
                required: true,
                minlength: 4
            },
            capacidad: {
                required: true,
                range: [10, 99]
            }
        },
        messages: {
            nombre_aula: {
                required: "El nombre del aula es requerido.",
                minlength: "El nombre del aula debe contener 4 caracteres minimo."
            },
            capacidad: {
                required: "La capacidad del aula es requerida.",
                minlength: "La capacidad del aula debe ser de 2 digitos.",
                range: "La capacidad del aula debe ser mayor a 10 y menor a 99."
            },
        },
        errorPlacement: function (error, element) {
            error.appendTo("#form_aulas");
        }
    });
}