$('select').material_select("destroy");
var horasRestantes = 5,
    horasACumplir,
    colorAzul = "#057D97",
    colorBlanco = "#fff",
    colorVerde = "#5cb85c",
    onError = function (error) {
        toast("Error, Fallo al comunicar con la api. Codigo: " + error.status + ", " + error.statusText);
    },
    onComplete = function () {
        $("#barra-progeso").hide();
    };

$(document).ready(function () {
    $("#btn_guardar_cambios").click(btnGuardarCambiosOnClick);

    $("#select_profesor").select2({
        placeholder: "Seleccione una opcion",
        width: '100%'
    });

    $("#select_profesor").change(selectProfesorOnChange);
    //Si el selector esta en una option diferente a la default, se permite el cambio
    //de color de las celdas al hacer click y se actualizan las horas_restantes
    $("td").click(function () {
        if ($("#select_profesor").val() !== "-1" && $("#select_profesor").val() !== null && !$(this).hasClass("hora") && !$(this).hasClass("hora_almuerzo")) {
            if ($(this).attr("bgcolor") === colorBlanco || $(this).attr("bgcolor") === colorAzul) {
                if (horasRestantes > 0) {
                    $(this).attr("bgcolor", colorVerde);
                    horasRestantes--;
                }
            } else {
                $(this).attr("bgcolor", colorBlanco);
                horasRestantes++;
            }
            $("#horas_restantes").val(horasRestantes);
        }
    });

    //Cambia el color cuando pasas el mouse encima de una celda o sales de ella,
    //si y solo si la option del select es diferente a la default	
    $("td").mouseover(function () {
        changeCellBackgroundColor(colorAzul, $(this));
    });

    $("td").mouseout(function () {
        changeCellBackgroundColor(colorBlanco, $(this));
    });
});

//#region OnClicks 
function btnGuardarCambiosOnClick() {
    var cedula = $("#select_profesor").val();
    if (horasRestantes !== 0) {
        confirmAlert("Error", "red", "fa fa-exclamation", "No has asignado todas tus horas requeridas");
        return;
    }
    if (cedula === null || cedula === "-1") {
        confirmAlert("Error", "red", "fa fa-exclamation", "No has seleccionado a un profesor");
        return;
    }

    var data = encodeData();
    if (!validateData(data))
        confirmAlert("Error", "red", "fa fa-exclamation", "Un dia contiene menos de 2 horas academicas consecutivas");
    else {
        console.log(decodeData(data));
        $("#barra-progeso").show();
        createDisponibilidadProfesor(decodeData(data),
            function (data, textStatus, xhr) {
                if (xhr.status === 201) {
                    var buttons = {
                        ok: {
                            text: 'Ok',
                            btnClass: 'btn-green',
                            action: function () {}
                        }
                    };
                    confirmAlert("Operacion exitosa", "green", "fa fa-check", "Se guardo la disponibilidad correctamente", buttons);                    
                }
                else
                    confirmAlert("Error", "red", "fa fa-exclamation", "Ocurrio un error al guardar la disponibilidad");    
            }
        );
    }
}

function selectProfesorOnChange() {
    cleanCells();
    var cedula = $("#select_profesor").val();
    if (cedula === null || cedula === "-1") return;
    $("#barra-progeso").show();
    $("#btn_guardar_cambios").hide();
    getDisponibilidadProfesor(cedula, function (data) {
        $("#btn_guardar_cambios").show();
        horasACumplir = data.horasACumplir;
        if (data.disponibilidad === null || data.disponibilidad.length === 0) {
            horasRestantes = horasACumplir;
        } else {
            horasRestantes = horasACumplir - data.horasAsignadas;
            fillCells(data.disponibilidad);
        }
        $("#horas_restantes").val(horasRestantes);
        $("#horas_a_cumplir").val(horasACumplir);
    });
}
//#endregion


/**
 * Cambia el color de fondo de la celda solo si se selecciono algun profesor 
 * @param {String} backgroundColor Color a colocar
 * @param {Element} celda Jquery element $(this)
 */
function changeCellBackgroundColor(backgroundColor, celda) {
    if ($("#select_profesor").val() !== "-1" && $("#select_profesor").val() !== null && !celda.hasClass("hora") && !celda.hasClass("hora_almuerzo")) {
        if (celda.attr("bgcolor") != "#5cb85c") {
            celda.attr("bgcolor", backgroundColor);
        }
    }
}

/**
 * Limpia las celdas de la tabla al igual que los input
 */
function cleanCells() {
    $("#tabla_horario tbody tr td:not(.hora_almuerzo)").attr("bgcolor", "#fff");
    $('input').val("");
}

/**
 * Guarda la disponibilidad a un profesor en particular
 * @param {Object[]} data Array de objetos que contiene la data de la disponibilidad del profesor 
 * @param {Function} callback Funcion de callback
 */
function createDisponibilidadProfesor(data, callback) {
    makeAjaxCall(apiDisponibilidad,
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr);
        },
        onError, data, "POST", onComplete
    );
}

/**
 * Decodifica la data y la convierte en un objeto mas manejable
 * @param {String[]} data Array que contiene la data codificada (hora,dia) (e.g: '1,7')
 * @returns Array de objetos que contienen c/u la cedula, idDia, idHoraInicio, idHoraFin
 */
function decodeData(data) {
    var resultado = [],
        horasConsecutivas,
        indexA,
        indexB,
        indexC,
        cedula = parseInt($("#select_profesor").val());

    for (j = 1; j < 7; j++) {
        horasConsecutivas = 0;
        //Esta i debe ser el numero de filas de la tabla + 1
        for (i = 1; i <= 14; i++) {
            indexA = i + "," + j;
            indexB = (i + 1) + "," + j;
            indexC = (i - 1) + "," + j;
            if (data.includes(indexA) && data.includes(indexB)) {
                horasConsecutivas += 2;
                i++;
            } else if (data.includes(indexA) && data.includes(indexC) && i > 1) {
                horasConsecutivas += 1;
            } else {
                if (horasConsecutivas !== 0) {
                    resultado.push({
                        cedula: cedula,
                        idDia: j,
                        idHoraInicio: (i - horasConsecutivas),
                        idHoraFin: i
                    });
                    horasConsecutivas = 0;
                }
            }
        }
    }
    return resultado;
}

/**
 * Codifica las celdas seleccionadas de la tabla
 * @returns Array con la data de la tabla codificada en formato (hora,dia) (e.g: '1,7')
 */
function encodeData() {
    var arrayData = [],
        tabla = document.getElementById("tabla_horario");
    for (var j = 1; j < 7; j++) {
        for (var i = 1; i <= 13; i++) {
            if (i === 7) continue;
            if (tabla.rows[i].cells[j].getAttribute("bgcolor") === colorVerde)
                arrayData.push(i + "," + j);
        }
    }
    return arrayData;
}

/**
 * Rellena las celdas de la tabla de disponibilidad acorde a la data recibida
 * @param {Object[]} data Array de objetos que contiene la data de la disponibilidad del profesor 
 */
function fillCells(data) {
    var tabla = document.getElementById("tabla_horario");
    for (var k = 0; k < Object.keys(data).length; k++) {
        for (var j = 1; j < 7; j++) {
            for (var i = 1; i < 14; i++) {
                if (data[k].idDia == j && data[k].idHoraInicio == i) {
                    for (var t = i; t < data[k].idHoraFin; t++) {
                        tabla.rows[t].cells[j].setAttribute("bgcolor", "#5cb85c");
                    }
                }
            }
        }
    }
}

/**
 * Obtiene la disponibilidad de un profesor en particular
 * @param {Number} cedula Cedula del profesor
 * @param {Function} callback Funcion de callback
 */
function getDisponibilidadProfesor(cedula, callback) {
    makeAjaxCall(apiDisponibilidad + "/" + cedula,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET", onComplete
    );
}

/**
 * Valida que hayan minimo 2 horas academicas seguidas en un dia
 * @param {String[]} data Array que contiene la data codificada (hora,dia) (e.g: '1,7')
 */
function validateData(data) {
    var indexA, indexB, indexC, i, j, k;
    var arrayCoincidencias = [];
    for (j = 1; j < 7; j++) {
        //tomo el numero de horas seleccionadas en 1 dia
        var coincidencias = $("#tabla_horario tbody tr td[bgcolor='" + colorVerde + "']:not(.hora_almuerzo):nth-child(" + (j + 1) + ")").length;
        var coincidenciasObtenidas = 0;

        //El chiste aca es que coincidencias llegue a 0, si no lo hace es porque
        //no se cumple que hayan 2 hrs academicas consecutivas
        //Para validar pregunto por la fila en la q me encuentro y la siguiente,
        //si ambas estan seleccionadas resto 2 e incremento la fila para poder
        //preguntar por las 2 siguientes. Sino se cumple esa condicion, preg. si la celda
        //anterior esta seleccionada y si k>0 entonces resto 1. Este caso ocurre solo cuando
        //tienes mas de 3 celdas seguidas seleccionadas y la 4ta no lo esta
        for (i = 1; i <= 13; i++) {
            indexA = i + "," + j;
            indexB = (i + 1) + "," + j;
            indexC = (i - 1) + "," + j;
            for (k = 0; k < data.length; k++) {
                if (data[k] == indexA && data[k + 1] == indexB) {
                    coincidencias -= 2;
                    i++;
                    break;
                } else if (data[k - 1] == indexC && data[k] == indexA && k > 0) {
                    coincidencias -= 1;
                }
            }
        }
        if (coincidencias !== 0) {
            return false;
        }
    }
    return true;
}