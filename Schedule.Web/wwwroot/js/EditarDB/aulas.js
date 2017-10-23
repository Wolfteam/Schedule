var contenidoAula =
    '<form id="form_aulas" class="col s12">' +
    '<div class="row">' +
    '<div class="input-field col s12">' +
    '<i class="material-icons prefix">comment</i>' +
    '<input id="id_aula" name="id_aula" type="hidden" >' +
    '<input id="nombre_aula" name="nombre_aula" type="text" class="validate" minlength="4" maxlength="20" required>' +
    '<label for="nombre_aula">Nombre del aula</label>' +
    '</div>' +
    '</div>' +
    '<div class="row">' +
    '<div class="input-field col s12">' +
    '<i class="material-icons prefix">filter_9_plus</i>' +
    '<input id="capacidad" name="capacidad" type="text" class="validate" minlength="2" maxlength="2" required>' +
    '<label for="capacidad">Capacidad</label>' +
    '</div>' +
    '</div>' +
    '<div class="row">' +
    '<div class="switch"><label>Teoria<input id="tipo_aula" type="checkbox"><span class="lever"></span>Laboratorio</label></div>' +
    '</div>' +
    '</form>';


function confirmCreateAulas() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var aula = {
                    nombreAula: this.$content.find("#nombre_aula").val(),
                    capacidad: this.$content.find("#capacidad").val(),
                    idTipo: this.$content.find("#tipo_aula").is(":checked") ? 2 : 1
                };
                if (aula.nombreAula.length < 4) {
                    this.$content.find("#nombre_aula").focus();
                    return false;
                }
                if (aula.capacidad.length !== 2) {
                    this.$content.find("#capacidad").focus();
                    return false;
                }
                createAula(aula);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var onContentReady = function () {
        this.$content.find("#capacidad").on("keypress", onlyNum);
    };
    confirmAlert("Agregar Aulas", "blue", "fa fa-plus", contenidoAula, buttons, onContentReady);
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
    confirmAlert("Editar Aula", "red", "fa fa-question-circle", "¿Está seguro que desea continuar?", buttons);
}

function confirmEditAulas(idAula, nombreAula, capacidad, idTipoAula) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var aula = {
                    nombreAula: this.$content.find("#nombre_aula").val(),
                    capacidad: this.$content.find("#capacidad").val(),
                    idTipo: this.$content.find("#tipo_aula").is(":checked") ? 2 : 1
                };
                if (aula.nombreAula.length < 4) {
                    this.$content.find("#nombre_aula").focus();
                    return false;
                }
                if (aula.capacidad.length !== 2) {
                    this.$content.find("#capacidad").focus();
                    return false;
                }
                updateAula(this.$content.find("#id_aula").val(), aula);
            }
        },
        Cancelar: {
            text: 'Cancelar',
            action: function () {}
        }
    };

    var onContentReady = function () {
        this.$content.find("#id_aula").val(idAula);
        this.$content.find("#nombre_aula").val(nombreAula);
        this.$content.find("#capacidad").val(capacidad);
        this.$content.find("#tipo_aula").prop("checked", idTipoAula == 2 ? true : false);
        this.$content.find("#capacidad").on("keypress", onlyNum);
        this.$content.find("#nombre_aula").focus();
        this.$content.find("#capacidad").focus();
        validarAula("#form_aulas");
    };
    confirmAlert("Editar Aula", "orange", "fa fa-pencil-square-o", contenidoAula, buttons, onContentReady);
}


/**
 * Crea una aula
 * @param {Object} aula Objeto de tipo aula
 */
function createAula(aula) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Aulas/Create",
        function (data) {
            if (data) {
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
        },
        onError, aula, "POST", onComplete
    );
}

/**
 * Elimina el aula
 * @param {number} id Id del aula a eliminar
 */
function deleteAula(id) {
    makeAjaxCall("/api/Aulas/Delete/" + id,
        function (data) {
            if (!data) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar el aula.");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina varias aulas selecionadas
 * @param {number[]} idArray Array de Ids de las aulas a eliminar
 */
function deleteAulas(idArray) {
    $("#barra-progeso").show();
    for (var i = 0; i < idArray.length; i++) {
        deleteAula(idArray[i].idAula);
    }
    checkPendingRequest();
}

//TODO: Hacer esta funcion mas generica
function checkPendingRequest() {
    if ($.active > 0) {
        window.setTimeout(checkPendingRequest, 1000);
    }
    else {
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
    }
};

/**
 * Obtiene todas las aulas
 */
function getAllAulas() {
    makeAjaxCall("/api/Aulas/GetAll",
        function (data) {
            var titulos = ["Id", "Nombre", "Capacidad", "Tipo Aula"];
            var columnsData = [{
                    "data": "idAula"
                },
                {
                    "data": "nombreAula"
                },
                {
                    "data": "capacidad"
                },
                {
                    "data": "tipoAula.nombreTipo"
                }
            ];
            //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
            createTable("#tabla", titulos);
            initDataTable("#datatable", data, columnsData, 0, false, "multi");
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Actualiza una aula
 * @param {number} id Id del aula a actualizar
 * @param {Object} aula Objeto de tipo aula
 */
function updateAula(id, aula) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Aulas/Update/" + id,
        function (data) {
            if (data) {
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
        },
        onError, aula, "PUT", onComplete
    );
}

function validarAula(selector) {
    $(selector).validate({
        rules: {
            nombre_aula: {
                required: true,
                minlength: 4
            },
            capacidad: {
                required: true,
                minlength: 2
            }
        },
        messages: {
            nombre_aula: {
                required: "El nombre del aula es requerido",
                minlength: "La longitud minima es de 4 caracteres"
            },
            capacidad: {
                required: "La capacidad es requerida",
                minlength: "La longitud minima/maxima es de 2 digitos"
            }
        },
        invalidHandler: function (event, validator) { //display error alert on form submit              
            console.log("formulario malo");
            return false;
        },
    });
}