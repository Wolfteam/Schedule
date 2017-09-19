delimiter //
CREATE PROCEDURE sp_AuthenticateUser (
	in username VARCHAR(20), in password VARCHAR(20)
)
BEGIN
	SELECT * FROM admin a WHERE a.username = username AND a.password = password;
END

delimiter //
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
END

DELIMITER
    //
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
END

DELIMITER
		//
CREATE PROCEDURE sp_GetToken(
    IN token VARCHAR(255)
)
BEGIN
	SELECT * FROM tokens t WHERE t.token = token
END

DELIMITER
    //
CREATE PROCEDURE sp_GetToken(IN token VARCHAR(255))
BEGIN
    SELECT
        *
    FROM
        tokens t
    WHERE
        t.token = token ;
END

DELIMITER
    //
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
END

DELIMITER
    //
CREATE PROCEDURE sp_DeleteToken(IN token VARCHAR(255))
BEGIN
    DELETE
        t
    FROM
        tokens t
    WHERE
        t.token = token ;
END

DELIMITER
    //
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
END