function confirmCreateMaterias() {
    var buttons = {
        Ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {
                var materia = prepareMateriaData(this.$content);
                createMateria(materia);
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

        });

        getAllCarreras(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idCarrera,
                    text: obj.nombreCarrera
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_carrera").append(options);
        });

        globalFunction = function () {
            $("#form_materias").find(".select2").select2({
                placeholder: "Seleccione una opcion",
                dropdownParent: $(".selectResults"),
                width: '100%'
            });
            $(".progressBar").hide();
            $("#form_materias").show();
        };
        checkPendingRequest();
    };
    confirmAlert("Agregar Materias", "blue", "fa fa-plus", "url:" + urlBase + "modals/Materias.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
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

function confirmEditMaterias(codigo, asignatura, idSemestre, idTipoAula, idCarrera, horasAcademicasTotales, horasAcademicasSemanales) {
    var buttons = {
        Ok: {
            text: 'Actualizar',
            btnClass: 'btn-orange',
            action: function () {
                var materia = prepareMateriaData(this.$content);
                updateMateria(codigo, materia);
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
        });

        getAllCarreras(function (data) {
            var arrayData = data.map(function (obj) {
                return {
                    id: obj.idCarrera,
                    text: obj.nombreCarrera
                };
            });
            var options = createSelectOptions(arrayData);
            content.find("#select_carrera").append(options).val(idCarrera);
        });

        globalFunction = function () {
            $("#form_materias").find(".select2").select2({
                placeholder: "Seleccione una opcion",
                dropdownParent: $(".selectResults"),
                width: '100%'
            });
            $(".progressBar").hide();
            $("#form_materias").show();
            content.find("#form_materias :input").each(function () {
                $(this).focus();
            });
        };
        checkPendingRequest();

        content.find("#codigo").val(codigo);
        content.find("#asignatura").val(asignatura);
        content.find("#horas_academicas_t").val(horasAcademicasTotales);
        content.find("#horas_academicas_s").val(horasAcademicasSemanales);
        content.find("#tipo_aula").prop("checked", idTipoAula == 2 ? true : false);
        content.find(".onlyNum").on("keypress", onlyNum);
    };
    confirmAlert("Editar Materias", "orange", "fa fa-pencil-square-o", "url:" + urlBase + "modals/Materias.html", buttons, onContentReady, "col s12 m12 l9 offset-l1");
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
        idTipo: object.find("#tipo_aula").is(":checked") ? 2 : 1,
        idCarrera: object.find("#select_carrera").val(),
        horasAcademicasTotales: object.find("#horas_academicas_t").val(),
        horasAcademicasSemanales: object.find("#horas_academicas_s").val()
    };
    return materia;
}

/**
 * Crea una materia
 * @param {Object} materia Objeto de tipo materia 
 */
function createMateria(materia) {
    $("#barra-progeso").show();
    makeAjaxCall(apiMaterias,
        function (data, textStatus, xhr) {
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
        },
        onError, materia, "POST", onComplete
    );
}

/**
 * Elimina la materia indicada
 * @param {number} codigo Codigo de la materia a eliminar
 */
function deleteMateria(codigo) {
    makeAjaxCall(apiMaterias + "/" + codigo,
        function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                confirmAlert("Error", "red", "fa fa-exclamation-triangle", "No se pudo borrar la materia.");
                return;
            }
        },
        onError, null, "DELETE"
    );
}

/**
 * Elimina las materias indicadas
 * @param {number[]} arrayCodigos Array de codigos de las materias a eliminar
 */
function deleteMaterias(arrayCodigos) {
    $("#barra-progeso").show();
    for (var i = 0; i < arrayCodigos.length; i++) {
        deleteMateria(arrayCodigos[i].codigo);
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
 * Obtiene todas las materias
 * @param {Function} callback Funcion de callback
 */
function getAllMaterias(callback) {
    $("#barra-progeso").show();
    makeAjaxCall(apiMaterias,
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr);
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Actualiza una Materia en particular
 * @param {number} codigo Codigo de la Materia a actualizar
 * @param {Object} materia Objeto de tipo Materia
 */
function updateMateria(codigo, materia) {
    $("#barra-progeso").show();
    makeAjaxCall(apiMaterias + "/" + codigo,
        function (data, textStatus, xhr) {
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
        },
        onError, materia, "PUT", onComplete
    );
}