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
	nombre_tipo VARCHAR(255) NOT NULL,
	PRIMARY KEY (id_tipo)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE tipo_asignacion(
	id_asignacion INT AUTO_INCREMENT,
	nombre_asignacion VARCHAR(255) NOT NULL,
	PRIMARY KEY (id_asignacion)
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
		ON DELETE CASCADE
		ON UPDATE CASCADE
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
	FOREIGN KEY (id_semestre) REFERENCES semestre (id_semestre)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_tipo) REFERENCES tipo_aula_materia (id_tipo)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_carrera) REFERENCES carreras (id_carrera)
		ON DELETE CASCADE
		ON UPDATE CASCADE
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
		ON DELETE CASCADE
		ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE profesores_materias (
	id INT UNSIGNED AUTO_INCREMENT,
	cedula INT UNSIGNED,
	codigo SMALLINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula,codigo),
    KEY (id),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (codigo) REFERENCES materias (codigo)
		ON DELETE CASCADE
		ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE secciones(
	codigo SMALLINT UNSIGNED,
	numero_secciones TINYINT UNSIGNED NOT NULL,
	cantidad_alumnos TINYINT UNSIGNED NOT NULL,
	id_periodo INT NOT NULL,
	PRIMARY KEY (codigo, id_periodo),
	FOREIGN KEY (codigo) REFERENCES materias (codigo)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_periodo) REFERENCES periodo_carrera (id_periodo)
		ON DELETE CASCADE
		ON UPDATE CASCADE		
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE disponibilidad_profesores(
	cedula INT UNSIGNED,
	id_dia TINYINT UNSIGNED NOT NULL,
	id_hora_inicio TINYINT UNSIGNED NOT NULL,
	id_hora_fin TINYINT UNSIGNED NOT NULL,
	id_periodo INT NOT NULL,
	PRIMARY KEY (cedula,id_dia,id_hora_inicio,id_hora_fin),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_dia) REFERENCES dias (id_dia)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_hora_inicio) REFERENCES horas (id_hora)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_hora_fin) REFERENCES horas (id_hora)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_periodo) REFERENCES periodo_carrera (id_periodo)
		ON DELETE CASCADE
		ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE horario_profesores(
	cedula INT UNSIGNED,
	codigo SMALLINT UNSIGNED NOT NULL,
	id_dia TINYINT UNSIGNED NOT NULL,
	id_hora_inicio TINYINT UNSIGNED NOT NULL,
	id_hora_fin TINYINT UNSIGNED NOT NULL,
	id_aula TINYINT UNSIGNED NOT NULL,
	numero_seccion TINYINT UNSIGNED NOT NULL,
	id_periodo INT NOT NULL,
	id_asignacion INT NOT NULL,
	PRIMARY KEY (cedula,codigo,id_aula,id_dia,id_hora_inicio,id_hora_fin,id_periodo),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (codigo) REFERENCES secciones (codigo)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_dia) REFERENCES dias (id_dia)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_hora_inicio) REFERENCES horas (id_hora)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_hora_fin) REFERENCES horas (id_hora)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_aula) REFERENCES aulas (id_aula)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_periodo) REFERENCES periodo_carrera (id_periodo)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_asignacion) REFERENCES tipo_asignacion (id_asignacion)
		ON DELETE CASCADE
		ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE admin(
	cedula INT UNSIGNED,
	username VARCHAR(20) NOT NULL,
	password VARCHAR(255) NOT NULL,
	id_privilegio TINYINT UNSIGNED NOT NULL,
	PRIMARY KEY (cedula),
	UNIQUE (username),
	FOREIGN KEY (cedula) REFERENCES profesores (cedula)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	FOREIGN KEY (id_privilegio) REFERENCES privilegios (id_privilegio)
		ON DELETE CASCADE
		ON UPDATE CASCADE
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
		ON DELETE CASCADE
		ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO periodo_carrera (nombre_periodo, status, fecha_creacion) VALUES
('2016-II', 0, '20160809'),
('2017-I', 0, '20170409'),
('2017-II', 1, '20170809');

INSERT INTO periodo_carrera (nombre_periodo, status, fecha_creacion) VALUES
('2016-II', 0, '20171109');

INSERT INTO tipo_aula_materia (nombre_tipo) VALUES 
('Teoria'),
('Laboratorio de Sistemas Eléctricos'),
('Laboratorio de Convertidores Eléctricos'),
('Laboratorio de Sistemas de Comunicación'),
('Laboratorio de Electrónica Industrial'),
('Laboratorio de Sistemas Digitales II'),
('Laboratorio de Sistemas Digitales I'),
('Laboratorio de Computación'),
('Laboratorio de Sistemas Electrónicos I y II'),
('Laboratorio de Sistemas Digitales III'),
('Laboratorio de Sistemas de Control II'),
('Laboratorio de Sistemas de Control III');

INSERT INTO tipo_asignacion VALUES
(1,'Automatica'),
(2, 'Random');

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
(2, 'Lab. Sistemas Eléctricos I y II', 16, 2),
(3, 'Lab. Convertidores Eléctricos', 15, 3),
(4, '2407', 45, 1),
(5, '2409', 35, 1),
(6, '2410', 35, 1),
(7, '2411', 35, 1),
(8, '2412', 35, 1),
(9, '2414', 45, 1),
(10, 'Lab. Sistemas de Comunicaciones', 10, 4),
(11, 'Lab. Sistemas Electrónicos II', 16, 9),
(12, 'Lab. Electrónica Industrial', 16, 5),
(13, 'Lab. Sistemas Digitales II', 12, 6),
(14, 'Lab. Computacion', 35, 8),
(15, 'Lab. Sistemas Electrónicos I', 12, 9),
(16, 'Lab. Sistemas Digitales III', 15, 10),
(17, 'Lab. Sistemas de Control II', 15, 11),
(18, 'Lab. Sistemas de Control III', 15, 12),
(19, 'Lab. Sistemas Digitales I', 15, 7);

INSERT INTO materias (codigo, asignatura, id_semestre, id_tipo, id_carrera, horas_academicas_totales, horas_academicas_semanales) VALUES
(41144, 'Electrotecnia', 14, 1, 3, 72, 4),
(41151, 'Lab. Electrotecnia', 14, 3, 3, 54, 3),
(41514, 'Sistemas Eléctricos I', 3, 1, 1, 90, 5),
(41521, 'Lab. Sistemas Eléctricos I', 3, 2, 1, 54, 3),
(41534, 'Sistemas Eléctricos II', 4, 1, 1, 90, 5),
(41541, 'Lab. Sistemas Eléctricos II', 4, 2, 1, 54, 3),
(41553, 'Convertidores Eléctricos', 5, 1, 1, 72, 4),
(41561, 'Lab. Convertidores Eléctricos', 5, 3, 1, 54, 3),
(41594, 'Canalizaciones', 10, 1, 1, 90, 5),
(42513, 'Sistemas Digitales I', 5, 1, 1, 72, 4),
(42521, 'Lab. Sistemas Digitales I', 5, 7, 1, 54, 3),
(42533, 'Sistemas Digitales II', 6, 1, 1, 72, 4),
(42541, 'Lab. Sistemas Digitales II', 6, 6, 1, 54, 3),
(42553, 'Sistemas Digitales III', 7, 1, 1, 72, 4),
(42561, 'Lab. Sistemas Digitales III', 7, 10, 1, 54, 3),
(43113, 'Controles Industriales', 11, 1, 1, 72, 4),
(43514, 'Sistemas de Control I', 5, 1, 1, 90, 5),
(43523, 'Instrumentación Industrial', 6, 1, 1, 72, 4),
(43531, 'Lab. Instrumentación Industrial', 6, 1, 1, 54, 3),
(43545, 'Modelaje y Simulación Digital', 6, 1, 1, 108, 6),
(43554, 'Sistemas de Control II', 7, 1, 1, 90, 5),
(43561, 'Lab. Sistemas de Control II', 7, 11, 1, 54, 3),
(43573, 'Sistemas de Control III', 8, 1, 1, 72, 4),
(43581, 'Lab. Sistemas de Control III', 8, 12, 1, 54, 3),
(43592, 'Proyectos Industriales', 9, 1, 1, 72, 4),
(43623, 'Control Óptimo', 11, 1, 1, 72, 4),
(44023, 'Sistemas de Comunicación', 8, 1, 1, 72, 4),
(44031, 'Lab. Sistemas de Comunicación', 8, 4, 1, 54, 3),
(44043, 'Transmisión de Datos', 9, 1, 1, 72, 4),
(44103, 'Antenas', 12, 1, 1, 72, 4),
(44203, 'Telefonía', 12, 1, 1, 72, 4),
(44303, 'Fibra Óptica', 12, 1, 1, 72, 4),
(44423, 'Sistemas de Comunicaciones Móviles', 12, 1, 1, 72, 4),
(44513, 'Sistemas de Señales', 7, 1, 1, 72, 4),
(44623, 'Redes Digitales de Comunicaciones', 12, 1, 1, 72, 4),
(45514, 'Sistemas Eléctronicos I', 4, 1, 1, 90, 5),
(45521, 'Lab. Sistemas Eléctronicos I', 4, 9, 1, 54, 3),
(45534, 'Sistemas Eléctronicos II', 5, 1, 1, 90, 5),
(45541, 'Lab. Sistemas Eléctronicos II', 5, 9, 1, 54, 3),
(45553, 'Electrónica Industrial', 6, 1, 1, 72, 4),
(45561, 'Lab. Electrónica Industrial', 6, 5, 1, 54, 3),
(46513, 'Programación Digital', 3, 1, 1, 90, 5),
(46523, 'Procesamiento de Datos', 7, 8, 1, 72, 4),
(46533, 'Sistemas de Información I', 13, 8, 1, 72, 4),
(46543, 'Sistemas de Información II', 13, 8, 1, 72, 4),
(46573, 'Base de Datos', 13, 8, 1, 72, 4),
(46583, 'Programación Orientada a Objetos', 13, 8, 1, 72, 4),
(46593, 'Ingeniería de Software', 13, 1, 1, 72, 4);

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

INSERT INTO disponibilidad_profesores 
(cedula, id_dia, id_hora_inicio, id_hora_fin, id_periodo) VALUES 
(23, 2, 8, 12, 3),
(23, 4, 8, 11, 3),
(28, 3, 1, 6, 3),
(28, 3, 8, 10, 3),
(3223864, 1, 5, 7, 3),
(3223864, 1, 8, 12, 3),
(3223864, 4, 5, 7, 3),
(3223864, 4, 8, 10, 3),
(3223864, 5, 5, 7, 3),
(3223864, 5, 8, 12, 3),
(4119381, 1, 1, 7, 3),
(4119381, 2, 1, 6, 3),
(4119381, 3, 1, 6, 3),
(4254143, 4, 2, 6, 3),
(4254143, 5, 1, 7, 3),
(4254143, 5, 8, 10, 3),
(4884447, 1, 1, 6, 3),
(4884447, 1, 8, 12, 3),
(4884447, 3, 1, 6, 3),
(4884447, 3, 8, 10, 3),
(5432639, 1, 1, 5, 3),
(5432639, 2, 1, 5, 3),
(5432639, 4, 1, 5, 3),
(5432639, 5, 1, 5, 3),
(6163902, 1, 2, 6, 3),
(6163902, 1, 8, 12, 3),
(6163902, 2, 1, 5, 3),
(6163902, 2, 8, 12, 3),
(6255322, 2, 3, 7, 3),
(6255322, 2, 8, 12, 3),
(6255322, 4, 8, 12, 3),
(6255322, 5, 8, 12, 3),
(6334158, 2, 1, 7, 3),
(6334158, 4, 8, 14, 3),
(6334158, 6, 1, 5, 3),
(6343581, 1, 2, 7, 3),
(6343581, 2, 8, 13, 3),
(6343581, 5, 1, 7, 3),
(6373667, 2, 11, 14, 3),
(6373667, 4, 11, 14, 3),
(6373667, 6, 1, 7, 3),
(6728390, 2, 8, 12, 3),
(6728390, 4, 2, 5, 3),
(6728390, 5, 2, 6, 3),
(6728390, 5, 8, 13, 3),
(6867877, 2, 1, 5, 3),
(6867877, 2, 9, 13, 3),
(6867877, 3, 1, 5, 3),
(6867877, 3, 9, 13, 3),
(6869357, 1, 1, 6, 3),
(6869357, 1, 9, 13, 3),
(6869357, 3, 1, 5, 3),
(6869357, 3, 9, 12, 3),
(8309660, 1, 1, 5, 3),
(8309660, 3, 1, 5, 3),
(8309660, 3, 8, 12, 3),
(8309660, 5, 1, 5, 3),
(10110448, 2, 8, 12, 3),
(10110448, 3, 8, 14, 3),
(10110448, 4, 8, 14, 3),
(10345341, 5, 2, 7, 3),
(10345341, 6, 1, 7, 3),
(10345341, 6, 8, 13, 3),
(10379875, 3, 1, 7, 3),
(10379875, 3, 8, 12, 3),
(10379875, 4, 1, 4, 3),
(10379875, 4, 8, 11, 3),
(11201707, 1, 1, 4, 3),
(11201707, 2, 1, 4, 3),
(11201707, 3, 1, 4, 3),
(11201707, 4, 1, 4, 3),
(11201707, 5, 1, 5, 3),
(12642255, 1, 1, 5, 3),
(12642255, 2, 1, 7, 3),
(12642255, 4, 1, 7, 3),
(12688253, 3, 1, 7, 3),
(12688253, 4, 1, 5, 3),
(12688253, 5, 1, 7, 3),
(13383418, 1, 3, 7, 3),
(13383418, 1, 8, 14, 3),
(13383418, 6, 1, 7, 3),
(13944531, 2, 2, 4, 3),
(13944531, 3, 1, 6, 3),
(13944531, 4, 1, 6, 3),
(13945188, 6, 1, 7, 3),
(13945188, 6, 8, 14, 3),
(14196635, 5, 8, 14, 3),
(14196635, 6, 8, 14, 3),
(15800780, 1, 1, 3, 3),
(15800780, 2, 1, 3, 3),
(15800780, 3, 1, 3, 3),
(15800780, 5, 1, 5, 3),
(15800780, 6, 1, 7, 3),
(23144433, 1, 1, 5, 3),
(23144433, 2, 1, 7, 3),
(23144433, 4, 1, 7, 3),
(81394456, 1, 1, 5, 3),
(81394456, 2, 1, 3, 3),
(81394456, 3, 1, 6, 3),
(81394456, 5, 1, 6, 3);

INSERT INTO secciones (codigo, numero_secciones, cantidad_alumnos, id_periodo) VALUES 
(41514, 2, 30, 3),
(41521, 3, 16, 3),
(41534, 1, 45, 3),
(41541, 2, 16, 3),
(41553, 1, 45, 3),
(41561, 2, 15, 3),
(41594, 2, 45, 3),
(42513, 1, 35, 3),
(42521, 3, 15, 3),
(42533, 1, 35, 3),
(42541, 3, 12, 3),
(42553, 1, 30, 3),
(42561, 3, 10, 3),
(43113, 1, 20, 3),
(43514, 1, 35, 3),
(43523, 1, 45, 3),
(43531, 1, 45, 3),
(43545, 1, 45, 3),
(43554, 1, 35, 3),
(43561, 2, 15, 3),
(43573, 1, 40, 3),
(43581, 3, 15, 3),
(43592, 1, 45, 3),
(43623, 1, 20, 3),
(44023, 2, 35, 3),
(44031, 3, 10, 3),
(44043, 1, 45, 3),
(44103, 1, 20, 3),
(44203, 1, 20, 3),
(44303, 1, 20, 3),
(44423, 1, 20, 3),
(44513, 2, 35, 3),
(44623, 1, 20, 3),
(45514, 3, 35, 3),
(45521, 4, 16, 3),
(45534, 1, 45, 3),
(45541, 4, 16, 3),
(45553, 2, 35, 3),
(45561, 4, 16, 3),
(46513, 3, 35, 3),
(46523, 2, 35, 3),
(46533, 1, 20, 3),
(46543, 1, 20, 3),
(46573, 2, 20, 3),
(46583, 1, 30, 3);

CREATE OR REPLACE VIEW vHorariosProfesores AS 
SELECT 
	p.nombre AS Profesor,
    m.asignatura Asignatura,
    m.codigo AS Codigo,
    h1.nombre_hora AS HoraInicio,
    h2.nombre_hora AS HoraFin,
    a.nombre_aula AS Aula,
    d.nombre_dia AS Dia,
    hp.numero_seccion AS Seccion,
    m.id_semestre AS Semestre,
    p.id_prioridad AS Prioridad,
	ta.nombre_asignacion AS TipoAsignacion
FROM 
	horario_profesores hp
INNER JOIN profesores p on hp.cedula = p.cedula
INNER JOIN materias m on hp.codigo = m.codigo
INNER JOIN horas h1 on hp.id_hora_inicio = h1.id_hora
INNER JOIN horas h2 ON hp.id_hora_fin = h2.id_hora
INNER JOIN aulas a on hp.id_aula = a.id_aula
INNER JOIN dias d on hp.id_dia = d.id_dia
INNER JOIN tipo_asignacion ta on hp.id_asignacion = ta.id_asignacion


DELIMITER //
CREATE PROCEDURE spGetHorariosProfesores(
    IN idSemestre INT,
	IN idAula INT 
)
BEGIN
	DECLARE idPeriodo INT;
    SET idPeriodo = (SELECT pc.id_periodo FROM periodo_carrera pc WHERE pc.status = 1 LIMIT 1);
    IF (idSemestre IS NOT NULL) THEN
    BEGIN
        SELECT
            hp.cedula,
            hp.id_periodo, 
            concat(p.nombre, " ", p.apellido) AS Profesor,
            m.asignatura Asignatura,
            m.codigo AS Codigo,
            h1.id_hora AS IdHoraInicio,
            h1.nombre_hora AS HoraInicio,
            h2.id_hora AS IdHoraFin,
            h2.nombre_hora AS HoraFin,
            a.nombre_aula AS Aula,
            d.id_dia AS IdDia,
            d.nombre_dia AS Dia,
            hp.numero_seccion AS NumeroSeccion,
            sec.cantidad_alumnos AS CantidadAlumnos,
            m.id_semestre AS Semestre,
            p.id_prioridad AS Prioridad,
            ta.nombre_asignacion AS TipoAsignacion
        FROM 
            horario_profesores hp
        INNER JOIN profesores p on hp.cedula = p.cedula
        INNER JOIN materias m on hp.codigo = m.codigo
        INNER JOIN horas h1 on hp.id_hora_inicio = h1.id_hora
        INNER JOIN horas h2 ON hp.id_hora_fin = h2.id_hora
        INNER JOIN aulas a on hp.id_aula = a.id_aula
        INNER JOIN dias d on hp.id_dia = d.id_dia
        INNER JOIN tipo_asignacion ta on hp.id_asignacion = ta.id_asignacion
        INNER JOIN secciones sec on hp.codigo = sec.codigo
        WHERE
            m.id_semestre = idSemestre
            AND hp.id_periodo = idPeriodo
        ORDER BY
             d.id_dia, hp.numero_seccion;
    END;
	ELSE -- IF (idAula IS NOT NULL) THEN
    BEGIN
    	SELECT
            hp.cedula,
            hp.id_periodo, 
            concat(p.nombre, " ", p.apellido) AS Profesor,
            m.asignatura Asignatura,
            m.codigo AS Codigo,
            h1.id_hora AS IdHoraInicio,
            h1.nombre_hora AS HoraInicio,
            h2.id_hora AS IdHoraFin,
            h2.nombre_hora AS HoraFin,
            a.nombre_aula AS Aula,
            d.id_dia AS IdDia,
            d.nombre_dia AS Dia,
            hp.numero_seccion AS NumeroSeccion,
            sec.cantidad_alumnos AS CantidadAlumnos,
            m.id_semestre AS Semestre,
            p.id_prioridad AS Prioridad,
            ta.nombre_asignacion AS TipoAsignacion
        FROM 
            horario_profesores hp
        INNER JOIN profesores p on hp.cedula = p.cedula
        INNER JOIN materias m on hp.codigo = m.codigo
        INNER JOIN horas h1 on hp.id_hora_inicio = h1.id_hora
        INNER JOIN horas h2 ON hp.id_hora_fin = h2.id_hora
        INNER JOIN aulas a on hp.id_aula = a.id_aula
        INNER JOIN dias d on hp.id_dia = d.id_dia
        INNER JOIN tipo_asignacion ta on hp.id_asignacion = ta.id_asignacion
        INNER JOIN secciones sec on hp.codigo = sec.codigo
        WHERE
            hp.id_aula = idAula
            AND hp.id_periodo = idPeriodo
        ORDER BY
             d.id_dia, hp.id_hora_inicio;
    END;
    END IF;
END //
DELIMITER ;