function getAllMaterias() {
    makeAjaxCall("/api/Materias/GetAll",
        function (data) {
            var titulos = ["Codigo", "Asignatura", "IdSemestre", "Semestre",
                "IdCarrera", "Carrera", "IdTipoAulaMateria", "Tipo Materia",
                "Horas Academicas Semanales", "Horas Academicas Totales"
            ];
            var columnsData = [{
                    "data": "codigo"
                },
                {
                    "data": "asignatura"
                },
                {
                    "data": "semestre.idSemestre"
                },
                {
                    "data": "semestre.nombreSemestre"
                },
                {
                    "data": "carrera.idCarrera"
                },
                {
                    "data": "carrera.nombreCarrera"
                },
                {
                    "data": "tipoMateria.idTipo"
                },
                {
                    "data": "tipoMateria.nombreTipo"
                },
                {
                    "data": "horasAcademicasSemanales"
                },
                {
                    "data": "horasAcademicasTotales"
                }
            ];
            //$.fn.dataTable.ext.classes.sPageButton = 'button primary_button';
            createTable("#tabla", titulos);
            initDataTable("#datatable", data, columnsData, [2, 4, 6], false, "single");
        },
        onError, null, "GET", onComplete
    );
}