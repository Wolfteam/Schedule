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
                showLoading(true);
                createProfesorMateria(relacion, (data, textStatus, xhr) => {
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
        getAllProfesores((data) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options);
        }, onError, () => {});

        getAllMaterias((data) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.codigo,
                    text: obj.codigo + " " + obj.asignatura
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_materia").append(options);
        }, onError, () => {});

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
                showLoading(true);
                updateProfesorMateria(id, relacion, (data, textStatus, xhr) => {
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
        getAllProfesores((data) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.cedula,
                    text: obj.nombre + " " + obj.apellido
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_profesor").append(options).val(cedula);
        }, onError, () => {});

        getAllMaterias((data) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.codigo,
                    text: obj.codigo + " " + obj.asignatura
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_materia").append(options).val(codigo);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_profesores_materias");
        };
        checkPendingRequest();
        validateProfesorMateriaHandler();
    };
    confirmAlert("Editar Relacion", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/ProfesorMateria.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Elimina las relaciones indicadas
 * @param {number[]} arrayID Array de ID de las relaciones a eliminar
 */
function deleteProfesoresMateria(arrayID) {
    showLoading(true);
    for (var i = 0; i < arrayID.length; i++) {
        deleteProfesorMateria(arrayID[i].id, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la relacion.");
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