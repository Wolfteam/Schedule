//Esto se encarga de que cuando se presione ctrl + space se muestre el sidenav
var map = { 17: false, 32: false };
$(document).keydown(function (e) {
    if (e.keyCode in map) {
        map[e.keyCode] = true;
        if (map[17] && map[32]) {
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