/// <binding Clean='clean'/>
"use strict";

var gulp = require('gulp');

var paths = {};
paths.webroot = "wwwroot/";
paths.npmSrc = "./node_modules/";
paths.libs = paths.webroot + "lib/";

gulp.task("default", ["copy-files"]);

gulp.task("copy-files", function () {
    gulp.src(paths.npmSrc + '/font-awesome/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/font-awesome/css/'));

    gulp.src(paths.npmSrc + '/font-awesome/fonts/*')
        .pipe(gulp.dest(paths.libs + '/font-awesome/fonts/'));

    gulp.src(paths.npmSrc + '/jquery/dist/**/*.min.js')
        .pipe(gulp.dest(paths.libs + '/jquery/'));

    gulp.src(paths.npmSrc + '/jquery-confirm/dist/**/*.*')
        .pipe(gulp.dest(paths.libs + '/jquery-confirm/'));

    gulp.src(paths.npmSrc + '/materialize-css/dist/**/*.*')
        .pipe(gulp.dest(paths.libs + '/materialize-css/'));

    gulp.src(paths.npmSrc + '/jquery-validation/dist/*')
        .pipe(gulp.dest(paths.libs + '/jquery-validation/'));

    gulp.src(paths.npmSrc + '/datatables.net/js/*')
        .pipe(gulp.dest(paths.libs + '/datatables.net/'));

    gulp.src(paths.npmSrc + '/datatables.net-dt/**/*.*')
        .pipe(gulp.dest(paths.libs + '/datatables.net/'));

    gulp.src(paths.npmSrc + '/datatables.net-buttons/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-buttons/'));

    gulp.src(paths.npmSrc + '/datatables.net-buttons-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-buttons/'));

    gulp.src(paths.npmSrc + '/datatables.net-keytable/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-keytable/'));

    gulp.src(paths.npmSrc + '/datatables.net-keytable-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-keytable/'));

    gulp.src(paths.npmSrc + '/datatables.net-responsive/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-responsive/'));

    gulp.src(paths.npmSrc + '/datatables.net-responsive-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-responsive/'));

    gulp.src(paths.npmSrc + '/datatables.net-select/js/*.min.js')
        .pipe(gulp.dest(paths.libs + '/datatables.net-select/'));

    gulp.src(paths.npmSrc + '/datatables.net-select-dt/css/*.min.css')
        .pipe(gulp.dest(paths.libs + '/datatables.net-select/'));

    gulp.src(paths.npmSrc + '/jszip/dist/*.min.js')
        .pipe(gulp.dest(paths.libs + '/jszip/'));

    gulp.src(paths.npmSrc + '/pdfmake/build/*')
        .pipe(gulp.dest(paths.libs + '/pdfmake/'));
});