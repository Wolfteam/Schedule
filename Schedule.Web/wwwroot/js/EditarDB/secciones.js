function confirmCreateSecciones() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                 var seccion = prepareSeccionData(this.$content);
                 createSecciones(seccion);
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
        });

        globalFunction = function () {
            $("#select_materia").removeAttr("disabled");
            $("#form_secciones").find(".select2").select2({
                placeholder: "Seleccione una opcion",
                dropdownParent: $(".selectResults"),
                width: '100%'
            });
            $(".progressBar").hide();
            $("#form_secciones").show();
        };
        checkPendingRequest();
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
                var seccion = prepareSeccionData(this.$content);
                updateSeccion(codigo, seccion);
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
        });

        globalFunction = function () {
            $("#select_materia").removeAttr("disabled");
            $("#form_secciones").find(".select2").select2({
                placeholder: "Seleccione una opcion",
                dropdownParent: $(".selectResults"),
                width: '100%'
            });
            $(".progressBar").hide();
            $("#form_secciones").show();
            content.find("#form_secciones :input").each(function () {
                $(this).focus();
            });
        };
        checkPendingRequest();

        content.find("#numero_secciones").val(numeroSecciones);
        content.find("#cantidad_alumnos").val(cantidadAlumnos);
        content.find(".onlyNum").on("keypress", onlyNum);
    };
    confirmAlert("Editar Secciones", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Secciones.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
}

/**
 * Crea una seccion
 * @param {Object} seccion Objeto de tipo Seccion
 */
function createSecciones(seccion) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Secciones",
        function (data, textStatus, xhr) {
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
        },
        onError, seccion, "POST", onComplete
    );
}

/**
 * Elimina las secciones correspondientes a la materia indicada
 * @param {number} codigo Cedula del profesor a eliminar
 */
function deleteSeccion(codigo) {
    makeAjaxCall("/api/Secciones/" + codigo,
        function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la(s) seccion(es).");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina  las secciones correspondientes a los codigos en el array
 * @param {number[]} arrayCodigos Array de codigos de las materias a las cuales se eliminaran las secciones
 */
function deleteSecciones(arrayCodigos) {
    $("#barra-progeso").show();
    for (var i = 0; i < arrayCodigos.length; i++) {
        deleteSeccion(arrayCodigos[i].codigo);
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
 * Obtiene todas las secciones
 * @param {Function} callback Funcion de callback
 */
function getAllSecciones(callback) {
    makeAjaxCall("/api/Secciones",
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr);
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Actualiza una seccion en particular
 * @param {number} codigo Codigo de la materia a actualizar
 * @param {Object} seccion Objeto de tipo seccion
 */
function updateSeccion(codigo, seccion) {
    $("#barra-progeso").show();
    makeAjaxCall("/api/Secciones/" + codigo,
        function (data, textStatus, xhr) {
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
        },
        onError, seccion, "PUT", onComplete
    );
}