/**
 * Crea un profesor
 * @param {Object} profesor Objeto de tipo profesor
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function createProfesor(profesor, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiProfesores,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        profesor, "POST",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Elimina el profesor indicado
 * @param {number} cedula Cedula del profesor a eliminar
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function deleteProfesor(cedula, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiProfesores + "/" + cedula,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "DELETE",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Obtiene todos los profesores
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function getAllProfesores(onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiProfesores,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        null, "GET",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}

/**
 * Actualiza una Materia en particular
 * @param {number} cedula Cedula del profesor a actualizar
 * @param {Object} profesor Objeto de tipo Profesor
 * @param {Function} onSucceedCallback Funcion de callback en caso de success
 * @param {Function} onErrorCallback Funcion de callback en caso de error. Null por default
 * @param {Function} onCompleteCallback Funcion de callback al completar el ajax. Null por default
 */
function updateProfesor(cedula, profesor, onSucceedCallback, onErrorCallback = null, onCompleteCallback = null) {
    makeAjaxCall(apiProfesores + "/" + cedula,
        (response, textStatus, xhr) => onSucceedCallback(response, textStatus, xhr),
        (error) => onErrorCallback === null ? onError(error) : onErrorCallback(error),
        profesor, "PUT",
        () => onCompleteCallback === null ? onComplete() : onCompleteCallback()
    );
}