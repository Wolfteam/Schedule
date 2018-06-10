function confirmCreateMaterias() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_materias").valid();
                if (!isValid)
                    return false;
                var materia = prepareMateriaData(this.$content);
                showLoading(true);
                createMateria(materia, (data, textStatus, xhr) => {
                    if (xhr.status !== 500) {
                        var buttons = {
                            Ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo la materia correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo crear la materia.");
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

        getAllSemestres(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idSemestre,
                    text: obj.nombreSemestre
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_semestre").append(options);

        }, onError, () => {});

        getAllCarreras((data) => {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idCarrera,
                    text: obj.nombreCarrera
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_carrera").append(options);
        }, onError, () => {});

        getAllTipoAulaMateria(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idTipo,
                    text: obj.nombreTipo
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_tipo_materia").append(options);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_materias");
        };
        checkPendingRequest();
        validateMateriaHandler();
    };
    confirmAlert("Agregar Materias", "blue", "fa fa-plus", "url:" + urlBase + "modals/Materias.html", buttons, onContentReady, "col s12 l10 offset-l1");
}

function confirmDeleteMaterias(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteMaterias(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Materia", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditMaterias(codigo, asignatura, idSemestre, idTipoMateria, idCarrera, horasAcademicasTotales, horasAcademicasSemanales) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var isValid = $("#form_materias").valid();
                if (!isValid)
                    return false;
                showLoading(true)
                var materia = prepareMateriaData(this.$content);
                updateMateria(codigo, materia, (data, textStatus, xhr) => {
                    if (xhr.status === 204) {
                        var buttons = {
                            ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo la materia correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar la materia.");
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
        getAllSemestres(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idSemestre,
                    text: obj.nombreSemestre
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_semestre").append(options).val(idSemestre);
        }, onError, () => {});

        getAllCarreras(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idCarrera,
                    text: obj.nombreCarrera
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_carrera").append(options).val(idCarrera);
        }, onError, () => {});

        getAllTipoAulaMateria(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idTipo,
                    text: obj.nombreTipo
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_tipo_materia").append(options).val(idTipoMateria);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_materias");
        };
        checkPendingRequest();

        content.find("#codigo").val(codigo);
        content.find("#asignatura").val(asignatura);
        content.find("#horas_academicas_t").val(horasAcademicasTotales);
        content.find("#horas_academicas_s").val(horasAcademicasSemanales);
        content.find(".onlyNum").on("keypress", onlyNum);
        validateMateriaHandler();
    };
    confirmAlert("Editar Materias", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Materias.html", buttons, onContentReady, "col s12 l10 offset-l1");
}

/**
 * Prepara la data introducida/existente de los modales y devuelve un objeto
 * @param {Object} object Objeto de tipo $content de jquery-confirm
 * @returns {Object} Objeto de tipo materia
 */
function prepareMateriaData(object) {
    var materia = {
        codigo: object.find("#codigo").val(),
        asignatura: object.find("#asignatura").val(),
        idSemestre: object.find("#select_semestre").val(),
        idTipo: object.find("#select_tipo_materia").val(),
        idCarrera: object.find("#select_carrera").val(),
        horasAcademicasTotales: object.find("#horas_academicas_t").val(),
        horasAcademicasSemanales: object.find("#horas_academicas_s").val()
    };
    return materia;
}

/**
 * Elimina las materias indicadas
 * @param {number[]} arrayCodigos Array de codigos de las materias a eliminar
 */
function deleteMaterias(arrayCodigos) {
    showLoading(true);
    for (var i = 0; i < arrayCodigos.length; i++) {
        deleteMateria(arrayCodigos[i].codigo, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la materia.");
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
 * Valdia que una materia tenga todas sus propiedades
 * @param {string} selector Selector del formulario materia (#form_materias por defecto)
 */
function validateMateriaHandler(selector = "#form_materias") {
    var valdiate = $(selector).validate({
        rules: {
            codigo: {
                required: true,
                minlength: 4,
                maxlength: 5
            },
            asignatura: {
                required: true,
                minlength: 4,
            },
            select_semestre: {
                required: true
            },
            select_tipo_materia: {
                required: true
            },
            select_carrera: {
                required: true
            },
            horas_academicas_t: {
                required: true,
                range: [10, 99]
            },
            horas_academicas_s: {
                required: true,
                range: [1, 6]
            },
        },
        messages: {
            codigo: {
                required: "El codigo de la materia es requerido. ",
                minlength: "El codigo de la materia debe contener 4-5 digitos. "
            },
            asignatura: {
                required: "El nombre de la asignatura es requerido. ",
                minlength: "El nombre de la asignatura debe contener 4 caracteres minimo. "
            },
            select_semestre: {
                required: "Debe seleccionar un semestre. ",
            },
            select_carrera: {
                required: "Debe seleccionar una carrera. ",
            },
            select_tipo_materia: {
                required: "Debe seleccionar una materia. "
            },
            horas_academicas_t: {
                required: "El numero de horas academicas totales es requerido. ",
                range: "El numero de horas academicas totales debe estar entre 10-99 horas. "
            },
            horas_academicas_s: {
                required: "El numero de horas academicas semanales es requerido. ",
                range: "El numero de horas academicas semanales debe estar entre 1-6 horas. "
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(selector);
        }
    });
}