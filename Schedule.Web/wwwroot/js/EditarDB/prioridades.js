/**
 * Obtiene todos las prioridades que se pueden asignar
 * @param {Function} callback Funcion de callback
 */
function getAllPrioridades(callback) {
    makeAjaxCall(apiPrioridadesProfesor,
        function (data, textStatus, xhr) {
            return callback(data);
        },
        onError, null, "GET"
    );
}