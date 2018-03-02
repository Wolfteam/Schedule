/**
 * Obtiene todos los tipos de aula-materia
 * @param {Function} callback Funcion de callback
 */
function getAllTipoAulaMateria(callback) {
    makeAjaxCall(apiTipoAulaMateria,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET"
    );
}