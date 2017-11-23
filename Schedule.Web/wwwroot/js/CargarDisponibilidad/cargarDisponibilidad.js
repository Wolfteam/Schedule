$('select').material_select("destroy");

$(document).ready(function () {
    $("#select_profesor").select2({
        placeholder: "Seleccione una opcion",
        width: '100%'
    });
});