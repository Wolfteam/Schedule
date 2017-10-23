function getAllProfesoresxMateria() {
    makeAjaxCall("/api/ProfesorMateria/GetAll",
        function (data) {
            var titulos = [
                "Id", "Cedula", "Nombre", "Apellido",
                "Codigo", "Asignatura", "Semestre", "Carrera"
            ];
            var columnsData = [{
                    "data": "id"
                },
                {
                    "data": "profesor.cedula"
                },
                {
                    "data": "profesor.nombre"
                },
                {
                    "data": "profesor.apellido"
                },
                {
                    "data": "materia.codigo"
                },
                {
                    "data": "materia.asignatura"
                },
                {
                    "data": "materia.semestre.nombreSemestre"
                },
                {
                    "data": "materia.carrera.nombreCarrera"
                },
            ];
            //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
            createTable("#tabla", titulos);
            initDataTable("#datatable", data, columnsData, 0, false, "single");
        },
        onError, null, "GET", onComplete
    );
}