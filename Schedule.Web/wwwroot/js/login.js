$(document).ready(function () {
    $("#formLogin").validate({
        rules: {
            username: {
                required: true,
                minlength: 4
            },
            password: {
                required: true,
                minlength: 4
            }
        },
        messages: {
            username: {
                required: "El username es requerido",
                minlength:"La longitud minima es de 4 caracteres"
            },
            password: {
                required: "La password es requerida",
                minlength: "La longitud minima es de 4 caracteres"
            }
        }
    });
});