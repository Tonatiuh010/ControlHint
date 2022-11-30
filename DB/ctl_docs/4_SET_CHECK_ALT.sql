/*PROCEDURE SET CHECK ALT*/
USE CTL_ACCESS;
DROP PROCEDURE IF EXISTS SET_CHECK_ALT;
DELIMITER //
CREATE PROCEDURE SET_CHECK_ALT (	
    IN IN_DEVICE INT,
    IN IN_EMPLOYEE INT,
    IN IN_USER VARCHAR(50),
    IN IN_VL_CHECK_DT DATETIME,
    IN IN_VL_TYPE VARCHAR(10),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_CHECK_ALT] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	SET OUT_RESULT = 'OK';
    
    SET IN_VL_CHECK_DT = NOW();
	SET IN_VL_TYPE = IF( IN_VL_CHECK_DT > GET_CHECK(IN_EMPLOYEE, FALSE), 'OUT', 'IN');
    
    INSERT INTO control_check 
	(
		EMPLOYEE_ID,
		TIME_EXP,
		CHECK_DT,
		TYPE,		
		STATUS,
		CREATED_ON,
		CREATED_BY,
		DEVICE_ID
	) VALUES (
		IN_EMPLOYEE,
		DATE_FORMAT(IN_VL_CHECK_DT, '%H%i%s'),
		IN_VL_CHECK_DT,
		IN_VL_TYPE,		
		'ENABLED',
		NOW(),
		IN_USER,
		IN_DEVICE
	);
    
END //
DELIMITER ;