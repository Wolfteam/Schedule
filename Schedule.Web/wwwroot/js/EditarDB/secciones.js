function getAllSecciones() {
    makeAjaxCall("/api/Secciones",
        function (data) {
            var titulos = [
                "Codigo Materia", "Asignatura", "Semestre",
                "Secciones", "Cantidad Alumnos", "Carrera"
            ];
            var columnsData = [{
                    "data": "materia.codigo"
                },
                {
                    "data": "materia.asignatura"
                },
                {
                    "data": "materia.semestre.nombreSemestre"
                },
                {
                    "data": "numeroSecciones"
                },
                {
                    "data": "cantidadAlumnos"
                },
                {
                    "data": "materia.carrera.nombreCarrera"
                }
            ];
            //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
            createTable("#tabla", titulos);
            initDataTable("#datatable", data, columnsData, -1, false, "single");
        },
        onError, null, "GET", onComplete
    );
}