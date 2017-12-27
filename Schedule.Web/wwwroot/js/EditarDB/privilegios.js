/**
 * Obtiene todos los privilegios que se pueden asignar a un usuario
 * @param {Function} callback Funcion de callback
 */
function getAllPrivilegios(callback) {
    makeAjaxCall(apiPrivilegios,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET"
    );
}