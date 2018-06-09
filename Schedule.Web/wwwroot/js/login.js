showLoading(false);
$(document).ready(function () {
    //Si existen errores en el formulario muestra un toast con los mismos
    $(".error").each(function () {
        toast($(this).val());
    });

    $("#btnLogin").click(() => showLoading(true));

    //Valida el formulario de lado cliente
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
                minlength: "La longitud minima es de 4 caracteres"
            },
            password: {
                required: "La password es requerida",
                minlength: "La longitud minima es de 4 caracteres"
            }
        },
        invalidHandler: function (event, validator) { //display error alert on form submit              
            showLoading(false);
        },
    });
});