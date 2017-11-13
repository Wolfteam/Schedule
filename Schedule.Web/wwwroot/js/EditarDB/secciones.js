/**
 * Obtiene todas las secciones
 * @param {Function} callback Funcion de callback
 */
function getAllSecciones(callback) {
    makeAjaxCall("/api/Secciones",
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr)
        },
        onError, null, "GET", onComplete
    );
}