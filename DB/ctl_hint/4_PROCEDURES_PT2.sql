/* ############################# GET_FLOW ############################### */
DROP PROCEDURE IF EXISTS GET_FLOW;
DELIMITER //
CREATE PROCEDURE GET_FLOW(
	IN IN_FLOW_ID INT,
    IN IN_FLOW_NAME VARCHAR(100),
    IN IN_FLOW_DET INT,
    OUT OUT_MSG VARCHAR(450)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_MSG := CONCAT('ERROR -> ON [GET_FLOW] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_MSG = 'OK';
    
    IF IN_FLOW_NAME IS NOT NULL THEN 
		SELECT * FROM VW_FLOW_CONFIGURATION 
		WHERE 
			FLOW_NAME = IN_FLOW_NAME
		AND FLOW_DET_ID = IFNULL(IN_FLOW_DET, FLOW_DET_ID);
	ELSE
		SELECT * FROM VW_FLOW_CONFIGURATION 
		WHERE 
			FLOW_ID = IFNULL(IN_FLOW_ID, FLOW_ID)
		AND FLOW_DET_ID = IFNULL(IN_FLOW_DET, FLOW_DET_ID);
    END IF;
    
END //
DELIMITER ;

-- SET @msg = '';
-- CALL GET_FLOW(NULL, NULL, NULL, @msg);
-- SELECT @msg;

/* ############################# GET_FLOW_PARAMETERS ############################### */
DROP PROCEDURE IF EXISTS GET_FLOW_PARAMETERS;
DELIMITER //
CREATE PROCEDURE GET_FLOW_PARAMETERS  (
	IN IN_PARAMETER_ID INT,	
    IN IN_FLOW_DET_ID INT,
    IN IN_API_ID INT,
    IN IN_FLOW_ID INT,
    OUT OUT_MSG VARCHAR(450)
) 
BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_MSG := CONCAT('ERROR -> ON [GET_FLOW_PARAMETERS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_MSG = 'OK';
    
	SELECT 
		FD.FLOW_DET_ID,
        FD.SEQUENCE,
        FD.IS_REQUIRED FLOW_DETAIL_REQUIRED,
        FD.DESCRIPTION FLOW_DETAIL_DESCRIPTION,
        FD.FLOW_ID,
		CUE.API_ID, 
		CUE.ENDPOINT_ID, 
		CUE.ENDPOINT, 
		CUE.REQUEST_TYPE,
		CEP.PARAMETER_ID,
		CEP.PARAMETER,
		CEP.TYPE,
		CEP.IS_REQUIRED URL_ENDPOINT_REQUIRED,
		CEP.DESCRIPTION
	FROM
		FLOW_DETAIL FD JOIN CON_URL_ENDPOINT CUE ON FD.ENDPOINT_ID = CUE.ENDPOINT_ID 
					   JOIN CON_ENDPOINT_PARAMETER CEP ON CUE.ENDPOINT_ID = CEP.ENDPOINT_ID
	WHERE 
		CEP.PARAMETER_ID = IFNULL(IN_PARAMETER_ID, CEP.PARAMETER_ID)
	AND CUE.API_ID = IFNULL(IN_API_ID, CUE.API_ID)
    AND FD.FLOW_DET_ID = IFNULL(IN_FLOW_DET_ID, FD.FLOW_DET_ID)
    AND FD.FLOW_ID = IFNULL(IN_FLOW_ID, FD.FLOW_ID);
    
END //
DELIMITER ;

-- SET @msg = '';
-- CALL GET_FLOW_PARAMETERS(NULL, NULL, NULL, NULL, @msg);
-- SELECT @msg;

/* ############################# GET_TRANSACTIONS ############################### */
DROP PROCEDURE IF EXISTS GET_TRANSACTIONS;
DELIMITER //
CREATE PROCEDURE GET_TRANSACTIONS (
	IN IN_TXN_ID INT,
    IN IN_TXN_DET_ID INT,
    IN IN_STS_HEADER_ID INT,
    IN IN_STS_DETAIL_ID INT,
    OUT OUT_MSG VARCHAR(450)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_MSG := CONCAT('ERROR -> ON [GET_TRANSACTIONS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_MSG = 'OK';
    
    SELECT 
		 TH.TXN_ID,
		 TH.FLOW_ID,
		 TH.FLOW_NAME,
		 TH.TXN_STS_ID,
		 TH.TXN_STS,
		 TD.TXN_DET_ID,
		 TD.ENDPOINT_ID,
		 TD.TXN_STS_ID STATUS_DETAIL_ID,
		 TD.TXN_STS STATUS_DETAIL
	FROM 
		TXN_HEADER TH JOIN TXN_DETAIL TD ON TH.TXN_ID = TD.TXN_ID
	WHERE
		TH.TXN_ID = IFNULL(IN_TXN_ID, TH.TXN_ID)
	AND TH.TXN_STS_ID = IFNULL(IN_STS_HEADER_ID, TH.TXN_STS_ID)
	AND TD.TXN_DET_ID = IFNULL(IN_TXN_DET_ID, TD.TXN_DET_ID)
    AND TD.TXN_STS_ID = IFNULL(IN_STS_DETAIL_ID, TD.TXN_STS_ID);
    
END //
DELIMITER ;

SET @msg = '';
CALL GET_TRANSACTIONS(NULL, NULL, NULL, NULL, @msg);
SELECT @msg;