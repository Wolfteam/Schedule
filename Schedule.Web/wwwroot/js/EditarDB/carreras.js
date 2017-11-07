/**
 * Obtiene todas las carreras
 * @param {Function} callback Funcion de callback
 */
function getAllCarreras(callback) {
    makeAjaxCall("/api/Carreras",
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET"
    );
}