DELIMITER $$
CREATE PROCEDURE sp_AuthenticateUser (
	in username VARCHAR(20), in password VARCHAR(20)
)
BEGIN
	SELECT * FROM admin a WHERE a.username = username AND a.password = password;
END$$
DELIMITER ;
-- *******************Sps Tokens*******************
DELIMITER $$
CREATE PROCEDURE sp_SaveToken(
    IN token VARCHAR(255),
    IN createDate DATETIME,
    IN expiricyDate DATETIME,
    IN username VARCHAR(20)
)
BEGIN
    INSERT INTO tokens(
        username,
        token,
        fecha_creacion,
        fecha_expiracion
    )
VALUES(
    username,
    token,
    createDate,
    expiricyDate
);
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_ValidateToken(
    IN token VARCHAR(255),
    IN expiricyDate DATETIME
)
BEGIN
    SELECT
        *
    FROM
        tokens t
    WHERE
        t.token = token AND T.fecha_expiracion > expiricyDate ;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetToken(
    IN token VARCHAR(255)
)
BEGIN
	SELECT * FROM tokens t WHERE t.token = token;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_UpdateToken(
    IN token VARCHAR(255),
    IN createDate DATETIME,
    IN expiricyDate DATETIME,
    IN username VARCHAR(20)
)
BEGIN
    UPDATE
        tokens t
    SET
        t.fecha_expiracion = expiricyDate,
        t.fecha_creacion = createDate
    WHERE
        t.token = token AND t.username = username ;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_DeleteToken(IN token VARCHAR(255))
BEGIN
    DELETE
        t
    FROM
        tokens t
    WHERE
        t.token = token ;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_PrivilegiosByToken(IN token VARCHAR(255))
BEGIN
    SELECT
        a.id_privilegio AS Privilegio
    FROM
        admin a
    INNER JOIN tokens t ON
        a.username = t.username
    WHERE
        t.token = token ;
END$$
DELIMITER ;

-- *******************Sps Aulas*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateAulas(
    IN nombre VARCHAR(35),
    IN capacidad TINYINT,
    in tipo TINYINT 
)
BEGIN
    INSERT INTO aulas (nombre_aula, capacidad, id_tipo) VALUES (nombre, capacidad, tipo);
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetAulas(
    IN id TINYINT
)
BEGIN
    IF (id IS NULL) THEN
        SELECT * FROM aulas;
    ELSE
        SELECT * FROM aulas a WHERE a.id_aula = id; 
    END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_DeleteAulas(
    IN id TINYINT
)
BEGIN
    DELETE FROM aulas WHERE id_aula = id;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_UpdateAulas(
    IN id TINYINT,
    IN nombre VARCHAR(35),
    capacidad TINYINT,
    idTipoAula TINYINT
)
BEGIN
    UPDATE aulas SET nombre_aula = nombre , capacidad = capacidad, id_tipo = idTipoAula
    WHERE id_aula = id;
END$$
DELIMITER ;

-- *******************Sps Carreras*******************
DELIMITER $$
CREATE PROCEDURE sp_GetCarreras(
    IN id TINYINT
)
BEGIN
    IF (id IS NULL) THEN
        SELECT * FROM carreras;
    ELSE
        SELECT * FROM carreras a WHERE a.id_carrera = id; 
    END IF;
END$$
DELIMITER ;

-- *******************Sps Dias*******************
DELIMITER $$
CREATE PROCEDURE sp_GetDiasHabiles(
    IN id TINYINT
)
BEGIN
    IF (id IS NULL) THEN
        SELECT * FROM dias;
    ELSE
        SELECT * FROM dias a WHERE a.id_dia = id; 
    END IF;
END$$
DELIMITER ;

-- *******************Sps Disponibilidad*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateDisponibilidadProfesor(
    IN cedula INT,
    IN idDia TINYINT,
    IN idHoraInicio TINYINT,
    IN idHoraFin TINYINT
)
BEGIN
    INSERT INTO disponibilidad_profesores (cedula, id_dia, id_hora_inicio, id_hora_fin)
    VALUES (cedula, idDia, idHoraInicio, idHoraFin);
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_DeleteDisponibilidadProfesor(
    IN cedula INT
)
BEGIN
    IF (cedula IS NULL) THEN
        DELETE FROM disponibilidad_profesores;
    ELSE
        DELETE dp FROM disponibilidad_profesores dp WHERE dp.cedula = cedula;
    END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetDisponibilidadesProfesor()
BEGIN
    SELECT * FROM disponibilidad_profesores;
END$$
DELIMITER ;

-- *******************Sps Horas*******************
DELIMITER $$
CREATE PROCEDURE sp_GetHoras(
    IN id TINYINT
)
BEGIN
    IF (id IS NULL) THEN
        SELECT * FROM horas;
    ELSE
        SELECT * FROM horas WHERE id_hora = id;
    END IF;
END$$
DELIMITER ;


-- *******************Sps Materias*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateMaterias(
    IN codigo INT,
    IN nombre VARCHAR(40),
    IN idCarrera TINYINT,
    IN idSemestre TINYINT,
    IN idTipoMateria TINYINT,
    IN horasSemanales TINYINT,
    IN horasAcademicasTotales TINYINT
)
BEGIN
    INSERT INTO materias (codigo, asignatura, id_semestre, id_tipo, id_carrera, horas_academicas_totales, horas_academicas_semanales)
    VALUES (codigo, nombre, idSemestre, idTipoMateria, idCarrera, horasAcademicasTotales, horasSemanales);
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetMaterias(
    IN codigo INT
)
BEGIN
    IF (codigo IS NULL) THEN 
        SELECT * FROM materias;
    ELSE
        SELECT * FROM materias a WHERE a.codigo = codigo;
    END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_DeleteMaterias(
    IN codigo SMALLINT
)
BEGIN
    DELETE FROM materias WHERE materias.codigo = codigo;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_UpdateMaterias(
    IN codigo INT,
    IN codigoNuevo INT,
    IN nombre VARCHAR(40),
    IN idCarrera TINYINT,
    IN idSemestre TINYINT,
    IN idTipoMateria TINYINT,
    IN horasSemanales TINYINT,
    IN horasAcademicasTotales TINYINT
)
BEGIN
    UPDATE materias m 
    SET 
        m.codigo = codigoNuevo, m.asignatura = nombre, m.id_semestre = idSemestre,
        m.id_tipo = idTipoMateria, m.id_carrera = idCarrera, m.horas_academicas_totales = horasAcademicasTotales,
        m.horas_academicas_semanales = horasSemanales
    WHERE m.codigo = codigo;
END$$
DELIMITER ;

-- *******************Sps Profesores*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateProfesor(
    IN cedula INT,
    IN nombre VARCHAR(30),
    IN apellido VARCHAR(30),
    IN idPrioridad TINYINT
)
BEGIN
    INSERT INTO profesores (cedula, nombre, apellido, id_prioridad)
    VALUES (cedula, nombre, apellido, idPrioridad);
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_GetProfesores(
    IN cedula INT
)
BEGIN
    IF (cedula IS NULL) THEN
        SELECT * FROM profesores;
    ELSE
        SELECT * FROM profesores p WHERE p.cedula = cedula;
    END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_DeleteProfesores(
    IN cedula INT
)
BEGIN
    DELETE p FROM profesores p WHERE p.cedula = cedula;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_UpdateProfesores(
    IN cedula INT,
    IN cedulaNueva INT,
    IN nombre VARCHAR(30),
    IN apellido VARCHAR(30),
    IN idPrioridad TINYINT
)
BEGIN
    UPDATE profesores p 
    SET 
        p.cedula = cedulaNueva, p.nombre = nombre, p.apellido = apellido,
        p.id_prioridad = idPrioridad
    WHERE 
        p.cedula = cedula;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_GetPrioridad(
    IN cedula INT
)
BEGIN
    SELECT pp.* 
    FROM profesores p
    INNER JOIN 
        prioridad_profesor pp ON p.id_prioridad = pp.id_prioridad
    WHERE p.cedula = cedula;
END$$
DELIMITER ;

-- *******************Sps ProfesoresxMaterias*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateProfesorxMateria(
    IN cedula INT,
    IN codigo SMALLINT UNSIGNED
)
BEGIN
    INSERT INTO profesores_materias (cedula, codigo) VALUES (cedula, codigo);
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_DeleteProfesorxMateria(
    IN id INT
)
BEGIN
    DELETE pm FROM profesores_materias pm WHERE pm.id = id;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetProfesoresxMaterias()
BEGIN
    SELECT * FROM profesores_materias;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_UpdateProfesorxMateria(
    IN cedula INT,
    IN cedulaNueva INT,
    IN codigo SMALLINT,
    IN codigoNuevo SMALLINT
)
BEGIN
    UPDATE profesores_materias pm 
    SET 
        pm.cedula = cedulaNueva, pm.codigo = codigoNuevo
    WHERE 
        pm.cedula = cedula AND pm.codigo = codigo;
END$$
DELIMITER ;

-- *******************Sps Secciones*******************
DELIMITER $$
CREATE PROCEDURE sp_CreateSecciones(
    IN codigo SMALLINT UNSIGNED,
    IN numeroSecciones TINYINT,
    IN cantidadAlumnos TINYINT
)
BEGIN
    INSERT INTO secciones (codigo, numero_secciones, cantidad_alumnos) 
    VALUES (codigo, numeroSecciones, cantidadAlumnos);
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE sp_DeleteSecciones(
    IN codigo SMALLINT UNSIGNED
)
BEGIN
    IF (codigo IS NULL) THEN
        DELETE s FROM secciones s;
    ELSE
        DELETE s FROM secciones s WHERE s.codigo = codigo;
    END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_GetSecciones(
    IN codigo SMALLINT UNSIGNED
)
BEGIN
    IF (codigo IS NULL) THEN
        SELECT * FROM secciones;
    ELSE
        SELECT * FROM secciones s WHERE s.codigo = codigo;
    END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE sp_UpdateSecciones(
    IN codigo SMALLINT UNSIGNED,
    IN codigoNuevo SMALLINT UNSIGNED,
    IN numeroSecciones TINYINT,
    IN cantidadAlumnos TINYINT
)
BEGIN
    UPDATE secciones s 
    SET s.codigo = codigoNuevo , s.numero_secciones = numeroSecciones, s.cantidad_alumnos = cantidadAlumnos
    WHERE s.codigo = codigo;
END$$
DELIMITER ;

-- *******************Sps Semestre*******************
DELIMITER $$
CREATE PROCEDURE sp_GetSemestre(
    IN id TINYINT
)
BEGIN
    IF (ID IS NULL) THEN
        SELECT * FROM semestre;
    ELSE
        SELECT * FROM semestre s WHERE s.id_semestre = id;
    END IF;
END$$
DELIMITER ;

-- *******************Sps TipoAulaMateria*******************
DELIMITER $$
CREATE PROCEDURE sp_GetTipoAulaMateria(
    IN id TINYINT
)
BEGIN
    IF (ID IS NULL) THEN
        SELECT * FROM tipo_aula_materia;
    ELSE
        SELECT * FROM tipo_aula_materia tam WHERE tam.id_tipo = id;
    END IF;
END$$
DELIMITER ;

