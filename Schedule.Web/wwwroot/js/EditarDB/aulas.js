function confirmCreateAulas() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var isValid = $("#form_aulas").valid();
                if (!isValid)
                    return false;
                var aula = prepareAulaData();
                showLoading(true);
                createAula(aula, (data, textStatus, xhr) => {
                    if (xhr.status !== 500) {
                        var buttons = {
                            Ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se creo el aula correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo crear el aula.");
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
        content.find("#capacidad").on("keypress", onlyNum);
        getAllTipoAulaMateria(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idTipo,
                    text: obj.nombreTipo
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_tipo_aula").append(options);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_aulas");
        };
        checkPendingRequest();
        validateAulaHandler();
    };
    confirmAlert("Agregar Aulas", "blue", "fa fa-plus", "url:" + urlBase + "modals/Aulas.html", buttons, onContentReady);
}

function confirmDeleteAula(data) {
    var buttons = {
        Borrar: {
            text: 'Borrar',
            btnClass: 'btn-red',
            action: function () {
                deleteAulas(data);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    confirmAlert("Borrar Aula", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditAulas(idAula, nombreAula, capacidad, idTipoAula) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var isValid = $("#form_aulas").valid();
                if (!isValid)
                    return false;
                var aula = prepareAulaData();
                let aulaId = this.$content.find("#id_aula").val();
                showLoading(true);
                updateAula(aulaId, aula, (data, textStatus, xhr) => {
                    if (xhr.status === 204) {
                        var buttons = {
                            ok: {
                                text: 'Ok',
                                btnClass: 'btn-green',
                                action: function () {}
                            }
                        };
                        confirmAlert("Proceso completado", "green", "fa fa-check", "Se actualizo el aula correctamente.", buttons);
                        $("#btn_buscar").trigger("click");
                        return;
                    }
                    confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo actualizar el aula.");
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
        getAllTipoAulaMateria(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idTipo,
                    text: obj.nombreTipo
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_tipo_aula").append(options).val(idTipoAula);
        }, onError, () => {});

        globalFunction = function () {
            onRequestsFinished("#form_aulas");
        };
        checkPendingRequest();

        this.$content.find("#id_aula").val(idAula);
        this.$content.find("#nombre_aula").val(nombreAula);
        this.$content.find("#capacidad").val(capacidad);
        this.$content.find("#capacidad").on("keypress", onlyNum);
        validateAulaHandler();
    };
    confirmAlert("Editar Aula", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Aulas.html", buttons, onContentReady);
}

/**
 * Elimina varias aulas selecionadas
 * @param {number[]} idArray Array de Ids de las aulas a eliminar
 */
function deleteAulas(idArray) {
    showLoading(true);
    for (var i = 0; i < idArray.length; i++) {
        deleteAula(idArray[i].idAula, (data, textStatus, xhr) => {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el aula.");
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
 * tipo aula
 * @returns {Object} Objeto de tipo aula
 */
function prepareAulaData() {
    var aula = {
        nombreAula: $("#nombre_aula").val(),
        capacidad: $("#capacidad").val(),
        idTipo: $("#select_tipo_aula").val()
    };
    return aula;
}

/**
 * Valdia que una aula tenga todas sus propiedades
 * @param {string} selector Selector del formulario aula (#form_aulas por defecto)
 */
function validateAulaHandler(selector = "#form_aulas") {
    var valdiate = $(selector).validate({
        rules: {
            nombre_aula: {
                required: true,
                minlength: 4
            },
            capacidad: {
                required: true,
                range: [10, 99]
            },
            select_tipo_aula: {
                required: true
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
            select_tipo_aula: {
                required: "Debe seleccionar un tipo de aula. ",
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo("#form_aulas");
        }
    });
}