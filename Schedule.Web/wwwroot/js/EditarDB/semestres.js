/**
 * Obtiene todos los semestres
 * @param {Function} callback Funcion de callback
 */
function getAllSemestres(callback) {
    makeAjaxCall(apiSemestres,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET"
    );
}