DROP PROCEDURE IF EXISTS GET_EMPLOYEE_DETAIL;
DELIMITER //
CREATE PROCEDURE GET_EMPLOYEE_DETAIL(
	IN IN_EMPLOYEE INT,
    OUT OUT_RESULT VARCHAR(500)
)
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS 	= RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_EMPLOYEE_DETAIL] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	SET OUT_RESULT = 'OK';
	SELECT 		
		E.EMPLOYEE_ID, 
		E.FIRST_NAME, 
		E.LAST_NAME, 
		E.POSITION_ID, 
		E.SHIFT SHIFT_ID, 
		E.STATUS EMPLOYEE_STATUS,     
        E.IMAGE IMAGE,
		P.POSITION_ID, 
		P.NAME POSITION_NAME, 
		P.DEPARTMENT_ID,     
		P.JOB_ID,     
		D.NAME DEPARTMENT,
		D.CODE DEPTO_CODE,         	
		J.NAME JOB, 
		J.DESCRIPTION JOB_DETAIL, 	
		S.NAME SHIFT_CODE, 
		S.CLOCK_IN IN_SHIFT, 
		S.CLOCK_OUT OUT_SHIFT,
		S.LUNCH_TIME LUNCH, 
		S.DAY_COUNT SHIFT_INTERVAL		
	FROM 
		EMPLOYEE E LEFT JOIN POSITION P ON E.POSITION_ID = P.POSITION_ID
		LEFT JOIN DEPARTMENT D ON P.DEPARTMENT_ID = D.DEPARTMENT_ID
		LEFT JOIN JOB J ON P.JOB_ID = J.JOB_ID
		LEFT JOIN SHIFT S ON E.SHIFT = S.SHIFT_ID 		
	WHERE 
		E.EMPLOYEE_ID = IFNULL(IN_EMPLOYEE, E.EMPLOYEE_ID);	
        
END //
DELIMITER ;

/*set @OUT_RESULT = '';
call ctl_access.GET_EMPLOYEE_DETAIL(NULL, @OUT_RESULT);
select @OUT_RESULT;*/

DROP PROCEDURE IF EXISTS GET_EMPLOYEE_ACCESS_LEVEL;
DELIMITER //
CREATE PROCEDURE GET_EMPLOYEE_ACCESS_LEVEL (
	IN IN_EMPLOYEE INT, 
    OUT OUT_RESULT VARCHAR(500)   
) BEGIN 	
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_EMPLOYEE_ACCESS_LEVEL] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	SET OUT_RESULT = 'OK';
    
    SELECT 
		EAL.EMPLOYEE_ID,
		EAL.ACCESS_LEVEL_ID,
		AL.NAME,
		EAL.STATUS    
	FROM 
		EMPLOYEE_ACCESS_LEVEL EAL, ACCESS_LEVEL AL
	WHERE 
		EAL.ACCESS_LEVEL_ID = AL.ACCESS_LEVEL_ID
	AND	EAL.STATUS = 'ENABLED'
	AND	EAL.EMPLOYEE_ID = IFNULL(IN_EMPLOYEE, EAL.EMPLOYEE_ID);
    
END //
DELIMITER ;

/*set @OUT_RESULT = '0';
call ctl_access.GET_EMPLOYEE_ACCESS_LEVEL(NULL, @OUT_RESULT);
select @OUT_RESULT;*/

DROP PROCEDURE IF EXISTS GET_DEPARTMENTS;
DELIMITER //
CREATE PROCEDURE GET_DEPARTMENTS(
    IN IN_DEPTO INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_DEPARTMENTS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;

    SET OUT_RESULT = 'OK';

    SELECT 
        DEPARTMENT_ID, 
        CODE,
        NAME,
        STATUS 
    FROM 
        DEPARTMENT
    WHERE 
        DEPARTMENT_ID = IFNULL(IN_DEPTO, DEPARTMENT_ID)
        AND STATUS = 'ENABLED';
END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_DEPARTMENTS(1, @OUT_RESULT);
SELECT  @OUT_RESULT;*/

DROP PROCEDURE IF EXISTS GET_JOBS;
DELIMITER //
CREATE PROCEDURE GET_JOBS(
    IN  IN_JOB INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_JOBS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';

    SELECT 
		JOB_ID, DESCRIPTION, NAME
    FROM
        JOB
    WHERE
        JOB_ID = IFNULL(IN_JOB, JOB_ID)
    AND STATUS = 'ENABLED';

END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_JOBS(1, @OUT_RESULT);
SELECT  @OUT_RESULT;*/

DROP PROCEDURE IF EXISTS GET_SHIFTS;
DELIMITER //
CREATE PROCEDURE GET_SHIFTS(
    IN  IN_SHIFT INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_SHIFTS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';

    SELECT 
		SHIFT_ID,         
        NAME,
        CLOCK_IN,
        CLOCK_OUT,
        LUNCH_TIME,
        DAY_COUNT
    FROM
        SHIFT
    WHERE
        SHIFT_ID = IFNULL(IN_SHIFT, SHIFT_ID)
    AND STATUS = 'ENABLED';

END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_SHIFTS(1, @OUT_RESULT);
SELECT  @OUT_RESULT;*/

DROP PROCEDURE IF EXISTS GET_POSITIONS;
DELIMITER //
CREATE PROCEDURE GET_POSITIONS(
    IN  IN_POSITION INT,
    IN  IN_JOB INT,
    IN  IN_DEPTO INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_POSITIONS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';
    
	SELECT 
		P.POSITION_ID,
		P.NAME NAME,
		D.DEPARTMENT_ID,
		D.NAME DEPARTMENT,
		D.CODE DEPTO_CODE,
        J.JOB_ID,
		J.NAME JOB,
		J.DESCRIPTION JOB_DETAIL
	FROM 
		POSITION P 
			JOIN 
		JOB J ON P.JOB_ID = J.JOB_ID
			JOIN
		DEPARTMENT D ON D.DEPARTMENT_ID = P.DEPARTMENT_ID
	WHERE
		P.POSITION_ID = IFNULL(IN_POSITION, P.POSITION_ID)
	AND J.JOB_ID = IFNULL(IN_JOB, J.JOB_ID)
    AND	D.DEPARTMENT_ID = IFNULL(IN_DEPTO, D.DEPARTMENT_ID)
    AND P.STATUS = 'ENABLED';	
END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_POSITIONS(NULL, NULL, NULL, @OUT_RESULT);
SELECT  @OUT_RESULT;
*/	    

DROP PROCEDURE IF EXISTS GET_CHECKS;
DELIMITER //
CREATE PROCEDURE GET_CHECKS(
    IN  IN_CHECK 	INT,    
    IN  IN_EMPLOYEE INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_CHECKS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';
    
	SELECT
		CC.CHECK_ID CHECK_ID,         
		E.EMPLOYEE_ID,
		CC.TIME_EXP, CC.CHECK_DT, CC.TYPE,
		E.FIRST_NAME, E.LAST_NAME,        
        CC.DEVICE_ID
	FROM 
		CONTROL_CHECK CC, 
		EMPLOYEE E
	WHERE 
		E.EMPLOYEE_ID = CC.EMPLOYEE_ID	
		AND E.EMPLOYEE_ID = IFNULL(IN_EMPLOYEE, E.EMPLOYEE_ID)
		AND CC.CHECK_ID = IFNULL(IN_CHECK, CC.CHECK_ID);
END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_CHECKS(NULL, NULL, NULL, @OUT_RESULT);
SELECT  @OUT_RESULT;
*/

DROP PROCEDURE IF EXISTS GET_CHECK_DETAILS;
DELIMITER //
CREATE PROCEDURE GET_CHECK_DETAILS (
	IN 	IN_FROM_DT DATETIME,
    IN 	IN_TO_DT DATETIME,
    IN IN_EMPLOYEE_ID INT,
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_CHECK_DETAILS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';
    
    SELECT 
		CC.CHECK_ID,     
		CC.TIME_EXP, 
		CC.CHECK_DT, 
		CC.TYPE, 
        CC.DEVICE_ID,
        E.EMPLOYEE_ID,
		P.POSITION_ID, 
		P.NAME POSITION,
		J.JOB_ID,
		J.NAME JOB,
		D.DEPARTMENT_ID,
		D.NAME DEPARTMENT,
		D.CODE DEPARTMENT_CODE
	FROM  
		CONTROL_CHECK CC 
			JOIN 
		EMPLOYEE E ON E.EMPLOYEE_ID = CC.EMPLOYEE_ID
			LEFT JOIN 
		POSITION P ON E.POSITION_ID = P.POSITION_ID
			LEFT JOIN 
		JOB J ON P.JOB_ID = J.JOB_ID
			LEFT JOIN 
		DEPARTMENT D ON D.DEPARTMENT_ID = P.DEPARTMENT_ID		    
	WHERE 
		CC.CHECK_DT BETWEEN IN_FROM_DT and IN_TO_DT
        AND E.EMPLOYEE_ID = IFNULL(IN_EMPLOYEE_ID, E.EMPLOYEE_ID);
        
END //
DELIMITER ;

/*
set @OUT_RESULT = '0';
CALL GET_CHECK_DETAILS(date(now()), last_day(now()), @OUT_RESULT);
SELECT  @OUT_RESULT;
*/

CALL GET_CHECK_DETAILS(date(now()), last_day(now()), 1, @OUT_RESULT);

DROP PROCEDURE IF EXISTS GET_EMPLOYEE_HINTS;
DELIMITER //
CREATE PROCEDURE GET_EMPLOYEE_HINTS (
	IN IN_HINT_ID INT,
    IN IN_EMPLOYEE_ID INT,
    OUT OUT_RESULT VARCHAR(450)
) BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [GET_CHECK_DETAILS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';
    
    SELECT 
		EH.HINT_ID,
        EH.HINT_NAME,
        EH.LAST_UPDATE,
        EH.EXTENSION_IMG,
        EH.HINT_IMG,
        CONCAT(EH.HINT_NAME,'.',LOWER(EH.EXTENSION_IMG)) FULL_NAME,
        E.EMPLOYEE_ID,
        E.FIRST_NAME,
        E.LAST_NAME
	FROM 
		EMPLOYEE_HINT EH JOIN EMPLOYEE E ON EH.EMPLOYEE_ID = E.EMPLOYEE_ID
	WHERE 
		EH.HINT_ID = IFNULL(IN_HINT_ID, EH.HINT_ID)
	AND E.EMPLOYEE_ID = IFNULL(IN_EMPLOYEE_ID, E.EMPLOYEE_ID);
    
END //
DELIMITER ;

-- SET @msg = '';
-- CALL GET_EMPLOYEE_HINTS(null, 1, @msg);
-- SELECT @msg;