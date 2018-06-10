function confirmCreateUsuarios() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_usuarios").valid();
                if (!isValid)
                    return false;
                var usuario = prepareUsuarioData(this.$content);
                showLoading(true);
                createUsuario(usuario, (data, textStatus, xhr) => {
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
                });
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        var content = this.$content;
        getAllProfesores(function (data, textStatus, xhr) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options);
        }, onError, () => {});

        getAllPrivilegios((data, textStatus, xhr) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idPrivilegio,
                    text: obj.nombrePrivilegio
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_usuarios");
        };
        checkPendingRequest();
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

function confirmEditUsuario(cedula, username, password, idPrivilegio) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var isValid = $("#form_usuarios").valid();
                if (!isValid)
                    return false;
                var usuario = prepareUsuarioData(this.$content);
                showLoading(true);
                updateUsuario(cedula, usuario, (data, textStatus, xhr) => {
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
                });
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        var content = this.$content;
        getAllProfesores(function (data, textStatus, xhr) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options).val(cedula);
        }, onError, () => {});

        getAllPrivilegios((data, textStatus, xhr) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idPrivilegio,
                    text: obj.nombrePrivilegio
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_prioridad").append(options).val(idPrivilegio);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_usuarios");
        };

        content.find("#username").val(username);
        content.find("#password").val(password);
        checkPendingRequest();
        validateUsuarioHandler();
    };
    confirmAlert("Editar Usuario", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Usuarios.html", buttons, onContentReady);
}

/**
 * Elimina varios usuarios selecionadas
 * @param {number[]} cedulaArray Array de cedulas de los usuarios a eliminar
 */
function deleteUsuarios(cedulaArray) {
    showLoading(true);
    for (var i = 0; i < cedulaArray.length; i++) {
        deleteUsuario(cedulaArray[i].cedula, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el usuario.");
                return;
            }
        }, onError, () => {});
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
        showLoading(false);
    };
    checkPendingRequest();
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto
 * tipo usuario
 * @param {Object} content Objeto de tipo jquery-confirm
 * @returns {Object} Objeto de tipo usuario
 */
function prepareUsuarioData(content) {
    var usuario = {
        cedula: content.find("#select_profesor").val(),
        username: content.find("#username").val(),
        password: content.find("#password").val(),
        idPrivilegio: content.find("#select_prioridad").val()
    };
    return usuario;
}

/**
 * Valdia que un usuario tenga todas sus propiedades
 * @param {string} selector Selector del formulario aula (#form_usuarios por defecto)
 */
function validateUsuarioHandler(selector = "#form_usuarios") {
    var valdiate = $(selector).validate({
        rules: {
            username: {
                required: true,
                minlength: 4
            },
            password: {
                required: true,
                minlength: 4
            },
            select_profesor: {
                required: true
            },
            select_prioridad: {
                required: true
            }
        },
        messages: {
            username: {
                required: "El nombre del usuario es requerido.",
                minlength: "El nombre del usuario debe contener 4 caracteres minimo."
            },
            password: {
                required: "La password es requerida.",
                minlength: "La password debe contener 4 caracteres minimo."
            },
            select_profesor: {
                required: "Debe seleccionar un profesor. "
            },
            select_prioridad: {
                required: "Debe seleccionar una prioridad. "
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo("#form_usuarios");
        }
    });
}