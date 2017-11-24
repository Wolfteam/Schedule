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
    $("#select_profesor").select2({
        placeholder: "Seleccione una opcion",
        width: '100%'
    });

    $("#select_profesor").change(selectProfesorOnChange);
    //Si el selector esta en una option diferente a la default, se permite el cambio
    //de color de las celdas al hacer click y se actualizan las horas_restantes
    $("td").click(function () {
        if ($("#select_profesor").val() !== "-1" && $("#select_profesor").val() !== null && !$(this).hasClass("hora")) {
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

function selectProfesorOnChange() {
    cleanCells();
    var cedula = $("#select_profesor").val();
    if (cedula === null || cedula === "-1") return;
    $("#barra-progeso").show();
    getDisponibilidadProfesor(cedula, function (data) {
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

/**
 * Obtiene la disponibilidad de un profesor en particular
 * @param {Number} cedula Cedula del profesor
 * @param {Function} callback Funcion de callback
 */
function getDisponibilidadProfesor(cedula, callback) {
    makeAjaxCall("api/Disponibilidad/" + cedula,
        function (data, textStatus, xhr) {  
            return callback(data);
        },
        onError, null, "GET", onComplete
    );
}

function encodeData() {

}

/**
 * Rellena las celdas de la tabla de disponibilidad
 * @param {Object[]} data Array de objetos que contiene la data de la disponibilidad del profesor 
 */
function fillCells(data) {
    for (var k = 0; k < Object.keys(data).length; k++) {
        var tabla = document.getElementById("tabla_horario");
        for (var j = 1; j < 7; j++) {
            for (var i = 1; i < 13; i++) {
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
 * Cambia el color de fondo de la celda solo si se selecciono algun profesor 
 * @param {String} backgroundColor Color a colocar
 * @param {Element} celda Jquery element $(this)
 */
function changeCellBackgroundColor(backgroundColor, celda) {
    if ($("#select_profesor").val() !== "-1" && $("#select_profesor").val() !== null && !celda.hasClass("hora")) {
        if (celda.attr("bgcolor") != "#5cb85c") {
            celda.attr("bgcolor", backgroundColor);
        }
    }
}

/**
 * Limpia las celdas de la tabla al igual que los input
 */
function cleanCells() {
    $("#tabla_horario tbody tr td").attr("bgcolor", "#fff");
    $('input').val("");
}