$(document).ready(() => {
    validateChangePasswordHandler();
    $("#btn_save_changes").click(onBtnSaveChangesClick);
});

function onBtnSaveChangesClick() {
    let isValid = $("#div_change_password").valid();
    if (!isValid) {
        return false;
    }
    let request = getChangePasswordData();
    changePassword(request, (response) => {
        toast(response.message);            
        $('input').val("");
    });
}

function getChangePasswordData() {
    let request = {
        currentPassword: $("#currentPassword").val(),
        newPassword: $("#password").val(),
        newPasswordConfirmation: $("#password_confirm").val()
    };
    return request;
}

/**
 * Cambia la password al usuario que se encuentra actualmente logeado
 * @param {Object} request Contiene la password actual, la nueva y la confirmacion de la nueva
 * @param {Function} callback Funcion de callback en caso de success
 */
function changePassword(request, callback) {
    showLoading(true);
    makeAjaxCall("/" + apiAccount + "/ChangePassword",
        (response) => callback(response),
        (error) => onError(error),
        request, "PUT", () => showLoading(false)
    );
}

/**
 * Valdia que el cambio de password tenga todas sus propiedades
 * @param {string} selector Selector del formulario aula (#form_aulas por defecto)
 */
function validateChangePasswordHandler(selector = "#div_change_password") {
    var valdiate = $(selector).validate({
        rules: {
            currentPassword: {
                required: true,
                minlength: 8,
                maxlength: 10
            },
            password: {
                required: true,
                minlength: 8,
                maxlength: 10
            },
            password_confirm: {
                required: true,
                minlength: 8,
                maxlength: 10,
                // equalsTo: password
            }
        },
        messages: {
            currentPassword: {
                required: "La password actual es requerida.",
                minlength: "El minimo numero de caracteres es 8.",
                maxlength: "El maximo numero de caracteres es 10."
            },
            password: {
                required: "La password es requerido.",
                minlength: "El minimo numero de caracteres es 8.",
                maxlength: "El maximo numero de caracteres es 10."
            },
            password_confirm: {
                required: "La confirmacion del password es requerida.",
                minlength: "El minimo numero de caracteres es 8.",
                maxlength: "El maximo numero de caracteres es 10."
            }
        }
    });
}