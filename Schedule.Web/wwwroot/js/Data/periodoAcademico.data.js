/**
 * Crea un periodo academico
 * @param {Object} periodo Objeto de tipo periodo
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function createPeriodo(periodo, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiPeriodoCarrera,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        periodo, "POST",
        onComplete,
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Elimina el periodo academico correspondiente al id
 * @param {number} id Id del periodo academico a eliminar
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function deletePeriodo(id, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiPeriodoCarrera + "/" + id,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "DELETE",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene todos los periodos academicos
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getAllPeriodos(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiPeriodoCarrera,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene el periodo academico actual
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getCurrentPeriodo(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiPeriodoCarrera + "/Current",
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Actualiza un periodo en en particular
 * @param {number} id Id del periodo a actualizar
 * @param {Object} periodo Objeto de tipo periodo
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function updatePeriodo(id, periodo, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiPeriodoCarrera + "/" + id,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        periodo, "PUT",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}