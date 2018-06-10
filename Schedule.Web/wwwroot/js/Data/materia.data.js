/**
 * Crea una materia
 * @param {Object} materia Objeto de tipo materia 
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function createMateria(materia, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiMaterias,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        materia, "POST",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Elimina la materia indicada
 * @param {number} codigo Codigo de la materia a eliminar
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function deleteMateria(codigo, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiMaterias + "/" + codigo,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "DELETE",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene todas las materias
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getAllMaterias(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiMaterias,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Actualiza una Materia en particular
 * @param {number} codigo Codigo de la Materia a actualizar
 * @param {Object} materia Objeto de tipo Materia
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function updateMateria(codigo, materia, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiMaterias + "/" + codigo,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        materia, "PUT",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}