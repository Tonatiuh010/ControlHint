/*######################### INSERTS PROCEDURES TO DOCS ##############################*/
USE prueba2;
DROP PROCEDURE IF EXISTS APPROVER_INSERT;
DELIMITER //
CREATE PROCEDURE APPROVER_INSERT (
	IN 	IN_APPROVER INT,
    IN  IN_FULLNAME VARCHAR(100),
    IN	IN_POSITION INT,
	IN 	IN_DEPTO INT
)

BEGIN
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM APPROVER WHERE APPROVER_ID = IN_APPROVER);
	IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000' 
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
	ELSE 
		INSERT INTO APPROVER(APPROVER_ID, FULL_NAME, POSITION_ID, DEPTO_ID)
		VALUES (IN_APPROVER, IN_FULLNAME, IN_POSITION, IN_DEPTO);
	END IF;
END;
//DELIMITER ;

-- CALL APPROVER_INSERT(1,"AVILA MARCOS", 1, 1);
-- SELECT * FROM APPROVER;

DROP PROCEDURE IF EXISTS DOCAPPROVER_INSERT;
DELIMITER //
CREATE PROCEDURE DOCAPPROVER_INSERT(
	IN IN_DOCAPPROVER INT,
    IN IN_DOCFLOW INT,
    IN IN_APPROVERID INT,
    IN IN_SEQUENCE VARCHAR(100),
    IN IN_NAME VARCHAR(100),
    IN IN_ACTION INT
)
BEGIN
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM DOC_APPROVER WHERE DOC_APPROVER_ID = IN_DOCAPPROVER);
    IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
	ELSE
		INSERT INTO DOC_APPROVER (DOC_APPROVER_ID, DOC_FLOW_ID, APPROVER_ID, SEQUENCE, NAME, ACTION)
        VALUES (IN_DOCAPPROVER, IN_DOCFLOW, IN_APPROVERID, IN_SEQUENCE, IN_NAME, IN_ACTION);
	END IF;
END;
//DELIMITER ;

-- CALL DOCAPPROVER_INSERT (1,1,1,"NOSE", "SEPA", 1);
-- SELECT * FROM DOC_APPROVER;

DROP PROCEDURE IF EXISTS DOC_FILE_INSERT;
DELIMITER //
CREATE PROCEDURE DOC_FILE_INSERT (
	IN IN_FILE_ID INT,
    IN IN_DOCUMENT_ID INT,
    IN IN_DOC_IMG VARCHAR(100)
)
BEGIN
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM DOC_FILE WHERE FILE_ID = IN_FILE_ID);
    IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
    ELSE
		INSERT INTO DOC_FILE (FILE_ID, DOCUMENT_ID, DOC_IMG)
        VALUES (IN_FILE_ID, IN_DOCUMENT_ID, IN_DOC_IMG);
	END IF;
END ;
// DELIMITER ;

-- CALL DOC_FILE_INSERT (1, 1, "SEPALAVERGAKEVAKI");
-- SELECT * FROM DOC_FILE;

DROP PROCEDURE IF EXISTS DOC_FLOW_INSERT;
DELIMITER //
CREATE PROCEDURE DOC_FLOW_INSERT(
	IN IN_DOC_FLOW_ID INT,
    IN IN_TYPE_ID INT,
    IN IN_KEY1 VARCHAR(100),
    IN IN_KEY2 VARCHAR(100),
    IN IN_KEY3 VARCHAR(100),
    IN IN_KEY4 VARCHAR(100)
)
BEGIN
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM DOC_FLOW WHERE DOC_FLOW_ID = IN_DOC_FLOW_ID);
    IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
	ELSE
		INSERT INTO DOC_FLOW (DOC_FLOW_ID, TYPE_ID, KEY1, KEY2, KEY3, KEY4)
        VALUES (IN_DOC_FLOW_ID, IN_TYPE_ID, IN_KEY1, IN_KEY2, IN_KEY3, IN_KEY4);
	END IF;
END
// DELIMITER ;
-- CALL DOC_FLOW_INSERT (1,1,"SEPALAVERGA1","SEPALAVERGA2","SEPALAVERGA3","SEPALAVERGA4");
-- SELECT * FROM DOC_FLOW;

DROP PROCEDURE IF EXISTS DOC_TYPE_INSERT;
DELIMITER //
CREATE PROCEDURE DOC_TYPE_INSERT(
	IN IN_TYPE_ID INT,
    IN IN_TYPE_CODE VARCHAR(100)
)
BEGIN 
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM DOC_TYPE WHERE TYPE_ID = IN_TYPE_ID);
    IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
	ELSE
		INSERT INTO DOC_TYPE (TYPE_ID, TYPE_CODE)
        VALUES (IN_TYPE_ID, IN_TYPE_CODE);
	END IF;
END
// DELIMITER ;

-- CALL DOC_TYPE_INSERT(1,"SEPALAPORONGA")
-- SELECT *FROM DOC_TYPE;

DROP PROCEDURE IF EXISTS DOCUMENT_INSERT;
DELIMITER //
CREATE PROCEDURE DOCUMENT_INSERT (
	IN IN_DOCUMENT_ID INT,
    IN IN_NAME VARCHAR(100),
    IN IN_TYPE_ID INT
)
BEGIN
	SET @EXISTING_COUNT = (SELECT COUNT(*) FROM DOCUMENT WHERE DOCUMENT_ID = IN_DOCUMENT_ID);
    IF @EXISTING_COUNT > 0 THEN SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'EL REGISTRO YA EXISTE';
	ELSE
		INSERT INTO DOCUMENT(DOCUMENT_ID, NAME, TYPE_ID)
        VALUES (IN_DOCUMENT_ID, IN_NAME, IN_TYPE_ID);
	END IF;
END
// DELIMITER ;
-- CALL DOCUMENT_INSERT(1,"SEPALARIATA", 1);
-- SELECT * FROM DOCUMENT;