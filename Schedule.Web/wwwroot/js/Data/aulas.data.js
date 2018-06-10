/**
 * Crea una aula
 * @param {Object} aula Objeto de tipo aula
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function createAula(aula, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiAula,
        (data, textStatus, xhr) => onSucceedCallback(data, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        aula, "POST",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Elimina el aula
 * @param {number} id Id del aula a eliminar
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function deleteAula(id, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiAula + "/" + id,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "DELETE",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene todas las aulas
 * @param {Function} callback Funcion de callback
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getAllAulas(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiAula,
        (data, textStatus, xhr) => onSucceedCallback(data, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Actualiza una aula
 * @param {number} id Id del aula a actualizar
 * @param {Object} aula Objeto de tipo aula
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function updateAula(id, aula, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiAula + "/" + id,
        (data, textStatus, xhr) => onSucceedCallback(data, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        aula, "PUT",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}