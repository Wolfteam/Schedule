/**
 * Genera una alerta de confirmacion con Jquery-Confirm
 * @param {String} title Titulo de la alerta
 * @param {String} type Color de la alerta (red)
 * @param {String} iconClass Icono que se muestra al lado del titulo ('fa fa-question-circle')
 * @param {String} content Contenido que se muestra dentro de la alerta, puede ser html
 * @param {Object} buttons Objeto que contiene los botones a crear
 */
function confirmAlert(title, type, iconClass, content, buttons) {
    $.confirm({
        title: title,
        type: type,
        closeIcon: true,
        closeIconClass: 'fa fa-close',
        icon: iconClass,
        typeAnimated: true,
        escapeKey: "Cancelar",
        content: content,
        buttons: buttons,
        theme: 'material'
    });
}