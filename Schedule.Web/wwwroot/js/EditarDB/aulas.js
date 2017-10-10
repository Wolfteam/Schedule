function crearAulas() {
    var buttons = {
        ok: {
            text: 'Guardar',
            btnClass: 'btn-blue',
            action: function () {}
        },
        cancel: {
            text: 'Cancelar',
            action: function () {}
        }
    };
    var contenido =
        '<form class="col s12">' +
        '<div class="row">' +
        '<div class="input-field col s12">' +
        '<i class="material-icons prefix">comment</i>' +
        '<input id="icon_prefix" type="text" class="validate">' +
        '<label for="icon_prefix">Nombre del aula</label>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="input-field col s12">' +
        '<i class="material-icons prefix">filter_9_plus</i>' +
        '<input id="icon_telephone" type="number" class="validate">' +
        '<label for="icon_telephone">Capacidad</label>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="switch"><label>Teoria<input type="checkbox"><span class="lever"></span>Laboratorio</label></div>' +
        '</div>' +
        '</form>';
    confirmAlert("Agregar Aulas", "blue", "fa fa-plus", contenido, buttons);
}