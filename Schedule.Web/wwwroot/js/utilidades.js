﻿/**
 * Genera una alerta de confirmacion con Jquery-Confirm
 * @param {String} title Titulo de la alerta
 * @param {String} type Color de la alerta (red)
 * @param {String} iconClass Icono que se muestra al lado del titulo ('fa fa-question-circle')
 * @param {String} content Contenido que se muestra dentro de la alerta, puede ser html
 * @param {Object} buttons Objeto que contiene los botones a crear
 */
function confirmAlert(title, type, iconClass, content, buttons, onContentReady = function () { }, columnClass = "col s12 m6 offset-m3") {
    if (buttons === null) {
        buttons = {
            ok: {
                text: 'Ok',
                btnClass: 'btn-red',
                action: function () {
                }
            }
        };
    }
    $.confirm({
        title: title,
        type: type,
        closeIcon: true,
        columnClass: columnClass,
        closeIconClass: 'fa fa-close',
        icon: iconClass,
        typeAnimated: true,
        escapeKey: "Cancelar",
        content: content,
        buttons: buttons,
        theme: 'material',
        onContentReady: onContentReady,
        onOpenBefore: function () {
            $('select').material_select();
        }
    });
}
/**
 * Genera un Toast con el contenido que le pases
 * @param {string} contenido Texto a mostrar dentro del toast
 * @param {Number} duration Tiempo que se mostrara el toast, 3 seg. por defecto
 */
function toast(contenido, duration = 3000) {
    var toastContent = $("<span>" + contenido + "</span>").add($('<button class="btn-flat toast-action dismissToast">Ok</button>'));
    Materialize.toast(toastContent, duration);
}

/**
 * Realiza un ajax a la urlBaseAPI + url con la data que le pases y devuelve la data a la funcion que le pases
 * @param {string} url Url a donde ira el ajax (webservice + /GetAllUsers)
 * @param {function(object)} callback Funcion de callback
 * @param {function(object)} callbackError Funcion de callback para el error
 * @param {object} data Data a enviar
 * @param {string} type POST, GET (POST por default)
 */
function makeAjaxCall(url, callback, callbackError, data = null, type = "POST") {
    var d;
    if (data !== null) {
        d = JSON.stringify(data);
    }
    $.ajax({
        url: urlBaseAPI + url,
        data: d,
        dataType: "json",
        type: type,
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            return callback(msg);
        },
        error: function (result) {
            return callbackError(result);
        }
    });
}

/**
 * Crea un handler que elimina todos los toast generados al pulsar
 * sobre el boton con el selector que se indique. Ten en cuenta que debe ser llamdada
 * esta funcion solo despues de haber creado el/los toast
 * @param {*} selector Selector del boton presionado dentro del toast (.dismissToast por defecto)
 */
function dismissToast(selector = ".dismissToast") {
    //Para que se borren todos los toast
    $(selector).click(function () {
        Materialize.Toast.removeAll();
    });
}

/**
 * Crea solamente la tabla y su thead de acuerdo al numero de columnas que tenga.
 * @param {string} selector Selector donde se adicionara(append) esta tabla (#boxBody). Debe estar dentro de un div
 * @param {string[]} columnsData Array de strings que contiene el texto a mostrar en el thead
 */
function createTable(selector, columnsData) {
    var tableUpper = '<table id="datatable" class="highlight centered" style="width: 100%">' +
        '<thead><tr>';
    var tableLower = '</tr></thead></table>';
    var thead = "";

    for (var i in columnsData) {
        thead += '<th>' + $.trim(columnsData[i]) + '</th>';
    }
    $("div" + selector).append(tableUpper + thead + tableLower);
}


/**
 * Aplica DataTable a la tabla indicada
 * @param {String} selector Nombre de la tabla #datatable
 * @param {Object[]} data Array de objetos que contiene las rows, puede ser como sea siempre y cuando lo parsees bien en columnsData
 * @param {Object[]} columnsData Array de objetos para especificar de donde sale la informacion de las filas [{"data":"Empleado.Nombre"}]. Puede ser null
 * @param {Number[]} hiddenColum Array que contiene los numeros de columnas a ocultar. Pasar -1 en caso de no ocultar nada
 * @param {Boolean} needsSelectedRowHandler Handler especifico
 * @param {String} selectStyle Tipo de select (al seleccionar una fila), "single"/"multi"
 */
function initDataTable(selector, data, columnsData, hiddenColum, needsSelectedRowHandler, selectStyle) {
    var columnDefs;
    if (hiddenColum === -1) {
        columnDefs = [];
    } else {
        columnDefs = [{
            "targets": hiddenColum,
            "visible": false,
            "searching": false
        }];
    }

    var table = $(selector).DataTable({
        //"autoWidth": false,
        responsive: {
            details: {
                renderer: function (api, rowIdx, columns) {
                    var width = $("#boxBody").width();
                    var length = columns.length;
                    if (length === 0) {
                        return false;
                    }
                    var cell = "";
                    var row = "";
                    var table = '<table>';
                    var contador = 0;
                    //console.log("la longitud del padre (datatable) es: "+width);
                    var customWidth = (width / 3);
                    //console.log("el tamaño de la celda es de: "+customWidth);
                    if (width < 490) { //Si el tamaño de la pantalla es menor a 490 px muestro varias filas con una celda
                        for (var i in columns) {
                            if (!columns[i].hidden) {
                                continue;
                            }
                            row += '<tr data-dt-row="' + columns[i].rowIndex[0] + '" data-dt-column="' + columns[i].columnIndex + '" >' +
                                '<td style="white-space:pre-wrap;width:' + width + 'px" class="text-center"><b>' + $.trim(columns[i].title) + ': ' + '</b>' + $.trim(columns[i].data) + '</td></tr>';
                        }
                        return (table + row + "</table>");
                    } else { //Si el tamaño de la pantalla es mayor a 490 px muestro filas con 3 celdas
                        for (var i = 0; i < length; i++) {
                            //console.log(columns[i]);
                            if (!columns[i].hidden) {
                                continue;
                            }
                            //padding-right:'+padding+'px; class="text-left"
                            cell += '<td style="white-space:pre-wrap;width:' + customWidth + 'px" class="text-center"><b>' + $.trim(columns[i].title) + ': ' + '</b>' + $.trim(columns[i].data) + '</td>';
                            contador++;
                            if (contador === 3 || i === (length - 1)) {
                                contador = 0;
                                row = row + '<tr data-dt-row="' + columns[i].rowIndex[0] + '" data-dt-column="' + columns[i].columnIndex + '">' + cell + "</tr>";
                                cell = "";
                            }
                        }
                    }
                    return (table + row + "</table>");
                }
            }
        },
        "aLengthMenu": [
            [5, 10, 20, 50, -1],
            [5, 10, 20, 50, "Todos"]
        ],
        "dom": "<'row'<'col s4'l> <'col s4'B> <'col s4'f> >" +
            "<'row'<'col s12'tr>>" +
            "<'row'<'col s5'i><'col s7'p>>",
        select: {
            style: selectStyle,
            info: false
        },
        columnDefs: columnDefs,
        language: {
            "paginate": {
                "first": "Primera",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            infoFiltered: "",
            search: "Filtrar:",
            searchPlaceholder: "Filtrar resultados",
            lengthMenu: "Mostrar _MENU_ resultados.",
            zeroRecords: "No se encontraron resultados.",
            info: " Pagina _PAGE_ de _PAGES_ . Total de resultados: _MAX_",
            infoEmpty: "No hay resultados disponibles."
        },
        buttons: [ //Esto permite agregar botones para copiar, pdf, excel e imprimir
            {
                extend: 'pdf',
                text: '<span class="fa fa-file-pdf-o fa-2x" data-toggle="tooltip" title="Exportar a PDF"/>',
                className: 'red waves-effect waves-light btn',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                text: '<span class="fa fa-file-excel-o fa-2x" data-toggle="tooltip" title="Exportar a Excel"/>',
                className: 'green waves-effect waves-light btn',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<span class="fa fa-print fa-2x" data-toggle="tooltip" title="Imprimir"/>',
                orientation: 'landscape',
                className: 'blue waves-effect waves-light btn',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],
        order: [
            [1, "asc"]
        ], //con esto elimino que se muestre el icono de ordenamiento de la columna 0
        data: data,
        columns: columnsData,
        initComplete: function () {
            $("#datatable_length").html('<label>Mostrar resultados.</label><select id="select-length" name="datatable_length" aria-controls="datatable" class=""><option value="5">5</option><option value="10">10</option><option value="20">20</option><option value="50">50</option><option value="-1">Todos</option></select>');
            $('#select-length').change(function () {
                table.page.len($(this).val()).draw();
            });
            $('select').material_select();
            //Remueve todas las clases y agrega las que deseo
            $(".dt-buttons a").removeClass("dt-button");
        },
        drawCallback: function (settings) {
            var table = $(selector).DataTable();
            table.columns.adjust();
        }
    });

    if (!needsSelectedRowHandler) {
        return;
    }
    selectedRowHandler(selector);
}