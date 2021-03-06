function confirmCreateSecciones() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_secciones").valid();
                if (!isValid)
                    return false;
                var seccion = prepareSeccionData(this.$content);
                showLoading(true);
                createSecciones(seccion, (data, textStatus, xhr) => {
                    if (xhr.status !== 500) {
                        var buttons = {
                            Ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo la(s) seccion(es) correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo guardar la(s) seccion(es).");
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
        content.find(".onlyNum").on("keypress", onlyNum);

        getAllMaterias(function (data) {
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
            onRequestsFinished("#form_secciones");
        };
        checkPendingRequest();
        validateSeccionHandler();
    };
    confirmAlert("Agregar Secciones", "blue", "fa fa-plus", "url:" + urlBase + "modals/Secciones.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

function confirmDeleteSecciones(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteSecciones(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Secciones", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditSecciones(codigo, cantidadAlumnos, numeroSecciones) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var isValid = $("#form_secciones").valid();
                if (!isValid)
                    return false;
                var seccion = prepareSeccionData(this.$content);
                showLoading(true);
                updateSeccion(codigo, seccion, (data, textStatus, xhr) => {
                    if (xhr.status === 204) {
                        var buttons = {
                            ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo la seccion correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar la seccion.");
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
        getAllMaterias(function (data) {
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
            onRequestsFinished("#form_secciones");
        };
        checkPendingRequest();

        content.find("#numero_secciones").val(numeroSecciones);
        content.find("#cantidad_alumnos").val(cantidadAlumnos);
        content.find(".onlyNum").on("keypress", onlyNum);
        validateSeccionHandler();
    };
    confirmAlert("Editar Secciones", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Secciones.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Elimina  las secciones correspondientes a los codigos en el array
 * @param {number[]} arrayCodigos Array de codigos de las materias a las cuales se eliminaran las secciones
 */
function deleteSecciones(arrayCodigos) {
    showLoading(true);
    for (var i = 0; i < arrayCodigos.length; i++) {
        deleteSeccion(arrayCodigos[i].materia.codigo, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la(s) seccion(es).");
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
 * Prepara la data introducida/existente de los modales y devuelve un objeto de tipo seccion
 * @param {Object} object Objeto de tipo Jquery-Confirm
 * @returns {Object} Objeto de tipo Seccion
 */
function prepareSeccionData(object) {
    var seccion = {
        codigo: object.find("#select_materia").val(),
        cantidadAlumnos: object.find("#cantidad_alumnos").val(),
        numeroSecciones: object.find("#numero_secciones").val()
    };
    return seccion;
}

/**
 * Valdia que una seccion tenga todas sus propiedades
 * @param {string} selector Selector del formulario seccion (#form_secciones por defecto)
 */
function validateSeccionHandler(selector = "#form_secciones") {
    var valdiate = $(selector).validate({
        rules: {
            numero_secciones: {
                required: true,
                range: [1, 9]
            },
            cantidad_alumnos: {
                required: true,
                range: [10, 45]
            },
            select_materia: {
                required: true
            }
        },
        messages: {
            numero_secciones: {
                required: "El numero de secciones es requerido. ",
                range: "El numero de secciones debe estar entre 1-9. "
            },
            cantidad_alumnos: {
                required: "La cantidad de alumnos de la seccion es requerida. ",
                range: "La cantidad de alumnos debe estar entre 10-45. "
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