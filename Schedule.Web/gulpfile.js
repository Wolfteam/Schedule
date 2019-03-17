/// <binding Clean='clean'/>
"use strict";

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglifyes = require('uglify-es'),
    composer = require('gulp-uglify/composer'),
    uglify = composer(uglifyes, console),
    merge = require('merge-stream');

var paths = {};
paths.webroot = "wwwroot/";
paths.npmSrc = "./node_modules/";
paths.libs = paths.webroot + "lib/";

gulp.task("default", gulp.series(copyFiles, concatAndMinify, concatAndMinifyJs));

//Copia los archivos necesarios a ser usados de node_modules
function copyFiles() {
    var materialIcons = gulp.src(
        [
            paths.npmSrc + '/material-design-icons-iconfont/dist/*/*',
            paths.npmSrc + '/material-design-icons-iconfont/dist/*.css'
        ]
    )
        .pipe(gulp.dest(paths.libs + '/material-icons/'));

    var fa = gulp.src(paths.npmSrc + '/font-awesome/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/font-awesome/css/'));

    var faFonts = gulp.src(paths.npmSrc + '/font-awesome/fonts/*')
        .pipe(gulp.dest(paths.libs + '/font-awesome/fonts/'));

    var jq = gulp.src(paths.npmSrc + '/jquery/dist/**/*.min.js')
        .pipe(gulp.dest(paths.libs + '/jquery/'));

    var jqc = gulp.src(paths.npmSrc + '/jquery-confirm/dist/**/*.*')
        .pipe(gulp.dest(paths.libs + '/jquery-confirm/'));

    var materialize = gulp.src(paths.npmSrc + '/materialize-css/dist/**/*.*')
        .pipe(gulp.dest(paths.libs + '/materialize-css/'));

    var jqvalidate = gulp.src(paths.npmSrc + '/jquery-validation/dist/*')
        .pipe(gulp.dest(paths.libs + '/jquery-validation/'));

    var dt = gulp.src(paths.npmSrc + '/datatables.net/js/*')
        .pipe(gulp.dest(paths.libs + '/datatables.net/'));

    var dtNet = gulp.src(paths.npmSrc + '/datatables.net-dt/**/*.*')
        .pipe(gulp.dest(paths.libs + '/datatables.net/'));

    var dtNetBJs = gulp.src(paths.npmSrc + '/datatables.net-buttons/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-buttons/'));

    var dtNetBCss = gulp.src(paths.npmSrc + '/datatables.net-buttons-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-buttons/'));

    var dtKeyJs = gulp.src(paths.npmSrc + '/datatables.net-keytable/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-keytable/'));

    var dtKeyCss = gulp.src(paths.npmSrc + '/datatables.net-keytable-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-keytable/'));

    var dtRJs = gulp.src(paths.npmSrc + '/datatables.net-responsive/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-responsive/'));

    var dtRCss = gulp.src(paths.npmSrc + '/datatables.net-responsive-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-responsive/'));

    var dtSJs = gulp.src(paths.npmSrc + '/datatables.net-select/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-select/'));

    var dtSCss = gulp.src(paths.npmSrc + '/datatables.net-select-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-select/'));

    var jsZip = gulp.src(paths.npmSrc + '/jszip/dist/*.min.js')
        .pipe(gulp.dest(paths.libs + '/jszip/'));

    var pdfMake = gulp.src(paths.npmSrc + '/pdfmake/build/*')
        .pipe(gulp.dest(paths.libs + '/pdfmake/'));

    var select2 = gulp.src(
        [
            paths.npmSrc + "/select2/dist/css/*.min.css",
            paths.npmSrc + "/select2/dist/js/**/*.js"
        ])
        .pipe(gulp.dest(paths.libs + '/select2/'));

    return merge(
        fa, faFonts,
        jq, jqc, jqvalidate,
        materialize, materialIcons,
        dt, dtNet, dtNetBCss, dtNetBJs, dtKeyCss, dtKeyJs, dtRCss, dtRJs, dtSCss, dtSJs,
        jsZip, pdfMake,
        select2);
}

//Esto no ta completo todavia, quizas quitar document ready de los js para poder
//concatenarlos a todos
function concatAndMinifyJs() {
    //data.min.js
    var data = gulp.src([
        paths.webroot + "js/Data/*.js",
        "!" + paths.webroot + 'js/Data/*.min.js'
    ])
        .pipe(concat("data.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js/Data"));
    //changePassword.min.js
    var changePassword = gulp.src([
        paths.webroot + "js/Account/*.js",
        "!" + paths.webroot + 'js/Account/*.min.js'
    ])
        .pipe(concat("account.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js/Account"));
    //cargarDisponibilidad.min.js
    var cargarDisponibilidad = gulp.src([
        paths.webroot + "js/CargarDisponibilidad/*.js",
        "!" + paths.webroot + 'js/CargarDisponibilidad/*.min.js'
    ])
        .pipe(concat("cargarDisponibilidad.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js/CargarDisponibilidad"));
    //editarDB.min.js
    var editarDb = gulp.src([
        paths.webroot + "js/EditarDB/*.js",
        "!" + paths.webroot + 'js/EditarDB/*.min.js'
    ])
        .pipe(concat("editarDB.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js/EditarDB"));
    //home.min.js
    var home = gulp.src([
        paths.webroot + "js/Home/*.js",
        paths.webroot + "js/EditarDB/periodoCarrera.js",
        "!" + paths.webroot + 'js/Home/*.min.js'
    ])
        .pipe(concat("home.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js/home"));
    return merge(data, changePassword, cargarDisponibilidad, editarDb, home);
}

//Esto concatena y minifca los js de las librerias/plugin utilizados ademas de las cosas comunes
function concatAndMinify() {
    //site.min.js
    return gulp.src([
        paths.webroot + "lib/jquery/jquery.min.js",
        paths.webroot + "lib/materialize-css/js/materialize.min.js",
        paths.webroot + "lib/jszip/jszip.min.js",
        paths.webroot + "lib/pdfmake/pdfmake.min.js",
        paths.webroot + "lib/pdfmake/vfs_fonts.js",
        paths.webroot + "lib/datatables.net/jquery.dataTables.js",
        paths.webroot + "lib/datatables.net-buttons/dataTables.buttons.min.js",
        paths.webroot + "lib/datatables.net-buttons/buttons.colVis.min.js",
        paths.webroot + "lib/datatables.net-buttons/buttons.flash.min.js",
        paths.webroot + "lib/datatables.net-buttons/buttons.html5.min.js",
        paths.webroot + "lib/datatables.net-buttons/buttons.print.min.js",
        paths.webroot + "lib/datatables.net-keytable/dataTables.keyTable.min.js",
        paths.webroot + "lib/datatables.net-responsive/dataTables.responsive.min.js",
        paths.webroot + "lib/datatables.net-select/dataTables.select.min.js",
        paths.webroot + "lib/jquery-confirm/jquery-confirm.min.js",
        paths.webroot + "lib/jquery-validation/jquery.validate.min.js",
        paths.webroot + "lib/select2/select2.min.js",
        paths.webroot + "lib/select2/i18n/es.js",
        paths.webroot + "js/common.js",
        paths.webroot + "js/utilidades.js"
    ])
        .pipe(concat("site.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + "js"));
}