//Para que select2 sea en spanish por default
$.fn.select2.defaults.set('language', 'es');

//Esto se encarga de que cuando se presione ctrl + shift se muestre el sidenav
var map = {
    17: false,
    16: false
};
$(document).keydown(function (e) {
    if (e.keyCode in map) {
        map[e.keyCode] = true;
        if (map[17] && map[16]) {
            $('.button-collapse').sideNav('show');
        }
    }
}).keyup(function (e) {
    if (e.keyCode in map) {
        map[e.keyCode] = false;
    }
});

//Inicializa el sidenav
$('.button-collapse').sideNav({
    //menuWidth: 300, // Default is 300
    //edge: 'right', // Choose the horizontal origin
    //closeOnClick: true, // Closes side-nav on <a> clicks, useful for Angular/Meteor
    //draggable: true, // Choose whether you can drag to open on touch screens,
    //onOpen: function (el) {
    //},
    //onClose: function (el) {
    //}
});

//inicializa los select
$('select').material_select();

/**
 * Esto permite logearse, ya que estoy usando un tag a
 */
$("#logout").click(function (e) {
    e.preventDefault();
    $("#logout_form").submit();
});

var apiAccount = "api/Usuarios",
    apiAula = "api/Aulas",
    apiCarreras = "api/Carreras",
    apiDisponibilidad = "api/Disponibilidad",
    apiMaterias = "api/Materias",
    apiPeriodoCarrera = "api/PeriodoCarrera",
    apiPrioridadesProfesor = "api/Prioridades",
    apiPrivilegios = "api/Privilegios",
    apiProfesores = "api/Profesores",
    apiProfesorMateria = "api/ProfesorMateria",
    apiSecciones = "api/Secciones",
    apiSemestres = "api/Semestres",
    apiTipoAulaMateria = "api/TipoAulaMateria";