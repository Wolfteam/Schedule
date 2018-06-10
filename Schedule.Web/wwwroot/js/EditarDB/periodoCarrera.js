function confirmCreatePeriodo() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_periodos").valid();
                if (!isValid)
                    return false;
                var periodo = preparePeriodoData(this.$content);
                showLoading(true);
                createPeriodo(periodo, (data, textStatus, xhr) => {
                    if (xhr.status !== 500) {
                        var buttons = {
                            Ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo el periodo academico correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo guardar el periodo academico.");
                });
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        globalFunction = function () {
            onRequestsFinished("#form_periodos");
        };
        checkPendingRequest();
        validatePeriodoHandler();
    };
    confirmAlert("Agregar Periodos", "blue", "fa fa-plus", "url:" + urlBase + "modals/Periodo.html", buttons, onContentReady);
}

function confirmDeletePeriodo(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deletePeriodos(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Periodos", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditPeriodo(id, nombre, status) {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_periodos").valid();
                if (!isValid)
                    return false;
                var periodo = preparePeriodoData(this.$content);
                showLoading(true);
                updatePeriodo(id, periodo, (data, textStatus, xhr) => {
                    if (xhr.status === 204) {
                        var buttons = {
                            ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo el periodo correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar el periodo.");
                });
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        this.$content.find("#nombre_periodo").val(nombre);
        this.$content.find("#periodo_status").prop("checked", status);

        globalFunction = function () {
            onRequestsFinished("#form_periodos");
        };
        checkPendingRequest();
        validatePeriodoHandler();
    };
    confirmAlert("Editar Periodos", "orange", "fa fa-plus", "url:" + urlBase + "modals/Periodo.html", buttons, onContentReady);
}

/**
 * Elimina los periodos academicos correspondientes a los ids en el array
 * @param {number[]} arrayIds Array de ids de los periodos academicos seleccionados
 */
function deletePeriodos(arrayIds) {
    showLoading(true);
    for (var i = 0; i < arrayIds.length; i++) {
        deletePeriodo(arrayIds[i].idPeriodo, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el(los) periodo(s) academico(s).");
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
        $("#barra-progeso").hide();
    };
    checkPendingRequest();
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto de tipo periodo
 * @param {Object} object Objeto de tipo Jquery-Confirm
 * @returns {Object} Objeto de tipo periodo
 */
function preparePeriodoData(object) {
    var periodo = {
        nombrePeriodo: object.find("#nombre_periodo").val(),
        status: object.find("#periodo_status").is(":checked")
    };
    return periodo;
}

/**
 * Valdia que una periodo tenga todas sus propiedades
 * @param {string} selector Selector del formulario periodo (#form_periodos por defecto)
 */
function validatePeriodoHandler(selector = "#form_periodos") {
    var valdiate = $(selector).validate({
        rules: {
            nombre_periodo: {
                required: true,
                minlength: 4,
                maxlength: 10
            }
        },
        messages: {
            nombre_periodo: {
                required: "El nombre del periodo academico es requerido. ",
                minlength: "El nombre del periodo academico debe contener minimo 4 caracteres. ",
                maxlength: "El nombre del periodo academico debe contener maximo 10 caracteres. "
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(selector);
        }
    });
}