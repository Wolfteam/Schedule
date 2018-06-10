/**
 * Crea una seccion
 * @param {Object} seccion Objeto de tipo Seccion
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function createSecciones(seccion, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiSecciones,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        seccion, "POST",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Elimina las secciones correspondientes a la materia indicada
 * @param {number} codigo Cedula del profesor a eliminar
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function deleteSeccion(codigo, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiSecciones + "/" + codigo,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "DELETE",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene todas las secciones
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getAllSecciones(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiSecciones,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Actualiza una seccion en particular
 * @param {number} codigo Codigo de la materia a actualizar
 * @param {Object} seccion Objeto de tipo seccion
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function updateSeccion(codigo, seccion, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiSecciones + "/" + codigo,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        seccion, "PUT",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}