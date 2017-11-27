DROP DATABASE IF EXISTS horarios;
CREATE DATABASE horarios;
USE horarios;

CREATE TABLE periodo_carrera(
	id_periodo INT AUTO_INCREMENT,
	nombre_periodo VARCHAR(20) NOT NULL,
	status BIT NOT NULL,
	fecha_creacion DATETIME,
	PRIMARY KEY (id_periodo)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE tipo_aula_materia(
	id_tipo TINYINT UNSIGNED AUTO_INCREMENT,
	nombre_tipo VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_tipo)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE carreras (
	id_carrera TINYINT UNSIGNED AUTO_INCREMENT,
	nombre_carrera VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_carrera)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE prioridad_profesor(
	id_prioridad TINYINT UNSIGNED AUTO_INCREMENT,
	codigo_prioridad VARCHAR(10) NOT NULL,
	horas_a_cumplir TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (id_prioridad)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE dias(
	id_dia TINYINT UNSIGNED AUTO_INCREMENT ,
	nombre_dia VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_dia)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE horas (
	id_hora TINYINT UNSIGNED AUTO_INCREMENT ,
	nombre_hora VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_hora)	
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE semestre(
	id_semestre TINYINT UNSIGNED AUTO_INCREMENT,
	nombre_semestre VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_semestre)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=3;

CREATE TABLE privilegios(
	id_privilegio TINYINT UNSIGNED AUTO_INCREMENT,
	nombre_privilegio VARCHAR(20) NOT NULL,
	PRIMARY KEY (id_privilegio)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE aulas (
	id_aula TINYINT UNSIGNED AUTO_INCREMENT,
	nombre_aula VARCHAR(35) NOT NULL,
	capacidad TINYINT UNSIGNED NOT NULL,
	id_tipo TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (id_aula),
	FOREIGN KEY (id_tipo) REFERENCES tipo_aula_materia (id_tipo)	
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE materias(
	codigo SMALLINT UNSIGNED,
	asignatura VARCHAR(40) NOT NULL,
	id_semestre TINYINT UNSIGNED NOT NULL,	
	id_tipo TINYINT UNSIGNED NOT NULL,
	id_carrera TINYINT UNSIGNED NOT NULL,
	horas_academicas_totales TINYINT(3) UNSIGNED NOT NULL,
	horas_academicas_semanales TINYINT(2) UNSIGNED NOT NULL,
	PRIMARY KEY (codigo),
	FOREIGN KEY (id_semestre) REFERENCES semestre (id_semestre),
	FOREIGN KEY (id_tipo) REFERENCES tipo_aula_materia (id_tipo),
	FOREIGN KEY (id_carrera) REFERENCES carreras (id_carrera)	
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE profesores(
	cedula INT UNSIGNED,
	nombre VARCHAR(30) NOT NULL,
	nombre2 VARCHAR(30),
	apellido VARCHAR(30) NOT NULL,
	apellido2 VARCHAR(30),
	id_prioridad TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula),
	FOREIGN KEY (id_prioridad) REFERENCES prioridad_profesor (id_prioridad)	
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE profesores_materias (
	id INT UNSIGNED AUTO_INCREMENT,
	cedula INT UNSIGNED,
	codigo SMALLINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula,codigo),
    KEY (id),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula),
	FOREIGN KEY (codigo) REFERENCES materias (codigo)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE secciones(
	codigo SMALLINT UNSIGNED,
	numero_secciones TINYINT UNSIGNED NOT NULL,
	cantidad_alumnos TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (codigo),
	FOREIGN KEY (codigo) REFERENCES materias (codigo)	
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE disponibilidad_profesores(
	cedula INT UNSIGNED,
	id_dia TINYINT UNSIGNED NOT NULL,
	id_hora_inicio TINYINT UNSIGNED NOT NULL,
	id_hora_fin TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula,id_dia,id_hora_inicio,id_hora_fin),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula),
	FOREIGN KEY (id_dia) REFERENCES dias (id_dia),
	FOREIGN KEY (id_hora_inicio) REFERENCES horas (id_hora),
	FOREIGN KEY (id_hora_fin) REFERENCES horas (id_hora)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE horario_profesores(
	cedula INT UNSIGNED,
	codigo SMALLINT UNSIGNED NOT NULL,
	id_dia TINYINT UNSIGNED NOT NULL,
	id_hora_inicio TINYINT UNSIGNED NOT NULL,
	id_hora_fin TINYINT UNSIGNED NOT NULL,
	id_aula TINYINT UNSIGNED NOT NULL,
	numero_seccion TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula,codigo,id_aula,id_dia,id_hora_inicio,id_hora_fin),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula),
	FOREIGN KEY (codigo) REFERENCES secciones (codigo),
	FOREIGN KEY (id_dia) REFERENCES dias (id_dia),
	FOREIGN KEY (id_hora_inicio) REFERENCES horas (id_hora),
	FOREIGN KEY (id_hora_fin) REFERENCES horas (id_hora),
	FOREIGN KEY (id_aula) REFERENCES aulas (id_aula)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE admin(
	cedula INT UNSIGNED,
	username VARCHAR(20) NOT NULL,
	password VARCHAR(255) NOT NULL,
	id_privilegio TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula),
	UNIQUE (username),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula),
	FOREIGN KEY (id_privilegio) REFERENCES privilegios (id_privilegio)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE tokens(
	id_token INT UNSIGNED AUTO_INCREMENT,
  username VARCHAR(20) NOT NULL,
	token VARCHAR(255) NOT NULL,
	fecha_creacion DATETIME NOT NULL,
	fecha_expiracion DATETIME NOT NULL,
	PRIMARY KEY (id_token),
	UNIQUE (token),
	FOREIGN KEY (username) REFERENCES admin (username)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO periodo_carrera (nombre_periodo, status, fecha_creacion) VALUES
('2016-II', 0, '20160809'),
('2017-I', 0, '20170409'),
('2017-II', 1, '20170809')

INSERT INTO periodo_carrera (nombre_periodo, status, fecha_creacion) VALUES
('2016-II', 0, '20171109')

INSERT INTO tipo_aula_materia (nombre_tipo) VALUES 
("Teoria"),
("Laboratorio");

INSERT INTO carreras (nombre_carrera) VALUES 
("Sistemas"),
("Mecanica"),
("Industrial");

INSERT INTO prioridad_profesor (codigo_prioridad,horas_a_cumplir) VALUES 
("C-H",7),
("C-MT",12),
("C-TC",16),
("MT",12),
("TC",16),
("DE",16);

INSERT INTO dias (nombre_dia) VALUES 
("Lunes"),
("Martes"),
("Miercoles"),
("Jueves"),
("Viernes"),
("Sabado");

INSERT INTO horas (nombre_hora) VALUES 
("7:00"),
("7:50"),
("8:40"),
("9:30"),
("10:20"),
("11:10"),
("12:00"),
("1:00"),
("1:50"),
("2:40"),
("3:30"),
("4:20"),
("5:10"),
("6:00");

INSERT INTO semestre(nombre_semestre) VALUES
("3"),
("4"),
("5"),
("6"),
("7"),
("8"),
("9"),
("Electiva Cod-41"),
("Electiva Cod-43"),
("Electiva Cod-44"),
("Electiva Cod-46"),
("Otros");

INSERT INTO privilegios (nombre_privilegio) VALUES
("Profesor"),
("Administrador");

INSERT INTO aulas (id_aula, nombre_aula, capacidad, id_tipo) VALUES
(1, '2201', 45, 1),
(2, 'Lab. Sistemas Eléctricos I', 16, 2),
(3, 'Lab. Sistemas Eléctricos II', 16, 2),
(4, 'Lab. Convertidores Eléctricos', 15, 2),
(5, '2407', 45, 1),
(6, '2409', 35, 1),
(7, '2410', 35, 1),
(8, '2411', 35, 1),
(9, '2412', 35, 1),
(10, '2414', 45, 1),
(11, 'Lab. Sistemas de Comunicaciones', 10, 2),
(12, 'Lab. Sistemas Electrónicos II', 16, 2),
(13, 'Lab. Electrónica Industrial', 16, 2),
(14, 'Lab. Sistemas Digitales II', 12, 2),
(15, 'Lab. Computacion', 35, 2),
(16, 'Lab. Sistemas Electrónicos I', 12, 2),
(17, 'Lab. Sistemas Digitales III', 15, 2),
(18, 'Lab. Sistemas de Control II', 15, 2),
(19, 'Lab. Sistemas de Control III', 15, 2);

INSERT INTO materias (codigo, asignatura, id_semestre, horas_academicas_totales, horas_academicas_semanales, id_tipo, id_carrera) VALUES
(41514, 'Sistemas Eléctricos I', 3, 90, 5, 1, 1),
(46513, 'Programación Digital', 3, 90, 5, 1, 1),
(41534, 'Sistemas Eléctricos II', 4, 90, 5, 1, 1),
(41521, 'Lab. Sistemas Eléctricos I', 3, 54, 3, 2, 1),
(41541, 'Lab. Sistemas Eléctricos II', 4, 54, 3, 2, 1),
(45514, 'Sistemas Eléctronicos I', 4, 90, 5, 1, 1),
(45521, 'Lab. Sistemas Eléctronicos I', 4, 54, 3, 2, 1),
(42513, 'Sistemas Digitales I', 5, 72, 4, 1, 1),
(42521, 'Lab. Sistemas Digitales I', 5, 54, 3, 2, 1),
(41553, 'Convertidores Eléctricos', 5, 72, 4, 1, 1),
(41561, 'Lab. Convertidores Eléctricos', 5, 54, 3, 2, 1),
(43514, 'Sistemas de Control I', 5, 90, 5, 1, 1),
(45534, 'Sistemas Eléctronicos II', 5, 90, 5, 1, 1),
(45541, 'Lab. Sistemas Eléctronicos II', 5, 54, 3, 2, 1),
(43523, 'Instrumentación Industrial', 6, 72, 4, 1, 1),
(43531, 'Lab. Instrumentación Industrial', 6, 54, 3, 2, 1),
(42533, 'Sistemas Digitales II', 6, 72, 4, 1, 1),
(42541, 'Lab. Sistemas Digitales II', 6, 54, 3, 2, 1),
(45553, 'Electrónica Industrial', 6, 72, 4, 1, 1),
(45561, 'Lab. Electrónica Industrial', 6, 54, 3, 2, 1),
(43545, 'Modelaje y Simulación Digital', 6, 108, 6, 1, 1),
(46523, 'Procesamiento de Datos', 7, 72, 4, 1, 1),
(42553, 'Sistemas Digitales III', 7, 72, 4, 1, 1),
(42561, 'Lab. Sistemas Digitales III', 7, 54, 3, 2, 1),
(43554, 'Sistemas de Control II', 7, 90, 5, 1, 1),
(43561, 'Lab. Sistemas de Control II', 7, 54, 3, 2, 1),
(44513, 'Sistemas de Señales', 7, 72, 4, 1, 1),
(44023, 'Sistemas de Comunicación', 8, 72, 4, 1, 1),
(44031, 'Lab. Sistemas de Comunicación', 8, 54, 3, 2, 1),
(43573, 'Sistemas de Control III', 8, 72, 4, 1, 1),
(43581, 'Lab. Sistemas de Control III', 8, 54, 3, 2, 1),
(44043, 'Transmisión de Datos', 9, 72, 4, 1, 1),
(43113, 'Controles Industriales', 11, 72, 4, 1, 1),
(43623, 'Control Óptimo', 11, 72, 4, 1, 1),
(44303, 'Fibra Óptica', 12, 72, 4, 1, 1),
(44623, 'Redes Digitales de Comunicaciones', 12, 72, 4, 1, 1),
(44103, 'Antenas', 12, 72, 4, 1, 1),
(44203, 'Telefonía', 12, 72, 4, 1, 1),
(44423, 'Sistemas de Comunicaciones Móviles', 12, 72, 4, 1, 1),
(41594, 'Canalizaciones', 10, 90, 5, 1, 1),
(46533, 'Sistemas de Información I', 13, 72, 4, 1, 1),
(46543, 'Sistemas de Información II', 13, 72, 4, 1, 1),
(46573, 'Base de Datos', 13, 72, 4, 1, 1),
(46583, 'Programación Orientada a Objetos', 13, 72, 4, 1, 1),
(46593, 'Ingeniería de Software', 13, 72, 4, 1, 1),
(41144, 'Electrotecnia', 14, 72, 4, 1, 3),
(41151, 'Lab. Electrotecnia', 14, 54, 3, 2, 3),
(43592, 'Proyectos Industriales', 9, 72, 4, 1, 1);

INSERT INTO profesores (cedula, nombre, apellido, id_prioridad) VALUES
(12688253, 'Jenny', 'Cruz', 5),
(8309660, 'Jesús', 'Noriega', 6),
(4884447, 'Pedro', 'Márquez', 6),
(6869357, 'Walter', 'Rivero', 5),
(15800780, 'María', 'Franco', 6),
(6867877, 'Henry', 'Arias', 6),
(10345341, 'José', 'Pinzon', 5),
(6163902, 'Francisco', 'Ledo', 6),
(13383418, 'Manuel', 'Fajardo', 5),
(12642255, 'Wilson', 'Mendoza', 5),
(10379875, 'Alexis', 'Cabello', 6),
(10110448, 'Jorge', 'Lara', 6),
(6255322, 'Ángel', 'Ramos', 6),
(6343581, 'Oswaldo', 'Fornerino', 5),
(5432639, 'Federico', 'Ochoa', 6),
(14196635, 'Frank', 'Capote', 4),
(6334158, 'Carlos', 'Bethancourt', 6),
(4254143, 'Luis', 'León', 2),
(23144433, 'Alexander', 'Tinoco', 5),
(81394456, 'Margarita', 'Aguayo', 5),
(6728390, 'David', 'Jaén', 6),
(23, 'Jesús', 'Guaregua', 1),
(6373667, 'Nando', 'Vitti', 4),
(4119381, 'José', 'Tovar', 6),
(3223864, 'Miguel', 'Contreras', 3),
(13945188, 'Kelly', 'Pérez', 4),
(28, 'Fernando', 'Dávila', 1),
(11201707, 'Mixaida', 'Delgado', 5),
(12069935, 'Wilfredo', 'Márquez', 3),
(13944531, 'Luis', 'Romero', 2),
(21255727, 'Efrain', 'Bastidas', 2);

-- Aca no se si mejor creo una tabla que diga departamentos, de tal forma que un prof. este asignado 
-- a un departamento, el problema esque un prof. podria quedar asignado a cualquier materia de su departamento
-- e.g: si tovar es del dpto de controles, pudiera quedar en controles 3 en vez de dar proyectos
INSERT INTO profesores_materias (cedula, codigo) VALUES
(12688253, 41514),
(12688253, 41521),
(11201707, 46513),
(8309660, 41534),
(8309660, 41541),
(4884447, 45514),
(4884447, 45521),
(6869357, 45514),
(6869357, 45521),
(6869357, 45541),
(15800780, 42513),
(15800780, 42521),
(6867877, 41553),
(6867877, 41561),
(10345341, 43514),
(10345341, 43561),
(6163902, 45534),
(6163902, 45541),
(13383418, 43113),
(13383418, 43523),
(13383418, 43531),
(12642255, 42533),
(12642255, 42541),
(10379875, 45553),
(10379875, 45561),
(10110448, 45553),
(10110448, 45561),
(6255322, 43545),
(6255322, 43554),
(6343581, 46523),
(6343581, 46533),
(6343581, 46543),
(5432639, 42553),
(5432639, 42561),
(14196635, 42553),
(14196635, 42561),
(6334158, 42561),
(4254143, 44513),
(23144433, 44023),
(23144433, 44031),
(81394456, 44031),
(81394456, 44043),
(81394456, 44303),
(6728390, 43573),
(6728390, 43581),
(6728390, 43623),
(23, 41151),
(23, 41594),
(6373667, 44043),
(6373667, 44623),
(4119381, 43592),
(3223864, 44103),
(3223864, 44203),
(3223864, 44423),
(13945188, 46573),
(28, 46583),
(12069935, 41151);

INSERT INTO admin (cedula,username,password,id_privilegio) VALUES 
(4119381,"unexpolcm", "sistemas",2),
(21255727,"wolfteam20", "sistemas",1);

-- Fijate como creo las FK a partir de una tabla que tiene una PK compuesta
-- FOREIGN KEY (codigo,numero_seccion) REFERENCES secciones (codigo,numero_seccion),

-- CREATE OR REPLACE VIEW materias_view AS SELECT m.codigo,m.asignatura,m.semestre,m.horas_academicas_totales,m.horas_academicas_semanales,tam.nombre_tipo,c.nombre_carrera FROM materias m, tipo_aula_materia tam,carreras c WHERE m.id_tipo=tam.id_tipo AND m.id_carrera=c.id_carrera

-- este select no se como falle xq nombre_hora se repite 2 veces
-- SELECT hp.cedula,p.nombre,hp.codigo,hp.numero_seccion,d.nombre_dia,h.nombre_hora,h2.nombre_hora,a.nombre_aula,s.cantidad_alumnos from horario_profesores hp INNER JOIN profesores p ON hp.cedula=p.cedula INNER JOIN secciones s ON hp.codigo=s.codigo AND hp.numero_seccion=s.numero_seccion INNER JOIN dias d ON hp.id_dia=d.id_dia INNER JOIN horas h ON hp.id_hora_inicio=h.id_hora INNER JOIN horas h2 ON hp.id_hora_fin=h2.id_hora INNER JOIN aulas a ON hp.id_aula=a.id_aula

-- este select funciona para la DB "base de datos"
-- SELECT hp.cedula,p.nombre,p.apellido,hp.codigo,m.asignatura,sem.nombre_semestre,hp.numero_seccion,d.nombre_dia,h.nombre_hora,h2.nombre_hora,a.nombre_aula,s.cantidad_alumnos from horario_profesores hp INNER JOIN profesores p ON hp.cedula=p.cedula INNER JOIN secciones s ON hp.codigo=s.codigo INNER JOIN dias d ON hp.id_dia=d.id_dia INNER JOIN horas h ON hp.id_hora_inicio=h.id_hora INNER JOIN horas h2 ON hp.id_hora_fin=h2.id_hora INNER JOIN aulas a ON hp.id_aula=a.id_aula INNER JOIN materias m ON hp.codigo=m.codigo INNER JOIN semestre sem ON m.id_semestre=sem.id_semestre

-- este select te sirve para la db "base de datos" para sacar la consulta de validateChoqueSemestre
-- SELECT hp.cedula,hp.codigo,hp.id_dia,hp.id_hora_inicio,hp.id_hora_fin,hp.id_aula,hp.numero_seccion FROM horario_profesores hp INNER JOIN materias m ON hp.codigo=m.codigo WHERE m.id_semestre=12