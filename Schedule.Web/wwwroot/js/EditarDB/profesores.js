function getAllProfesores() {
    makeAjaxCall("/api/Profesor/GetAll",
        function (data) {
            var titulos = [
                "Cedula", "Nombre", "Apellido",
                "IdPrioridad", "Prioridad", "Horas a Cumplir"
            ];
            var columnsData = [{
                    "data": "cedula"
                },
                {
                    "data": "nombre"
                },
                {
                    "data": "apellido"
                },
                {
                    "data": "prioridad.id"
                },
                {
                    "data": "prioridad.codigoPrioridad"
                },
                {
                    "data": "prioridad.horasACumplir"
                }
            ];
            //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
            createTable("#tabla", titulos);
            initDataTable("#datatable", data, columnsData, 3, false, "single");
        },
        onError, null, "GET", onComplete
    );
}