/**
 * Obtiene el periodo academico actual
 * @param {Function} callback Funcion de callback
 */
function getCurrentPeriodo(callback) {
    makeAjaxCall(apiPeriodoCarrera + "/Current",
        function (data, textStatus, xhr) {
            return callback(data, textStatus, xhr)
        },
        onError, null, "GET", onComplete
    );
}