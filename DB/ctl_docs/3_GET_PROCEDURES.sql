USE CTL_DOCS;

-- GET_DOCUMENTS TABLES DOC_TYPE, DOCUMENT, DOC FILE
DROP PROCEDURE IF EXISTS GET_DOCUMENTS;
DELIMITER //
CREATE PROCEDURE GET_DOCUMENTS(
    IN IN_DOCUMENT_ID INT,
    
    OUT OUT_MSG VARCHAR(500)
) 
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_MSG := CONCAT('ERROR -> ON [GET_DOCUMENTS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;	
    
    SET OUT_MSG = 'OK';
    
    WITH CUR_DOC AS (
		SELECT 
			DT.TYPE_ID,
			DT.TYPE_CODE,
			D.DOCUMENT_ID,
			D.NAME,        
			DF.FILE_ID,        
			DF.DOC_IMG,
			DF.DOCUMENT_OBJ
		FROM 
			DOC_TYPE DT JOIN DOCUMENT D ON DT.TYPE_ID = D.TYPE_ID 
			LEFT JOIN DOC_FILE DF ON D.DOCUMENT_ID = DF.DOCUMENT_ID 
	) SELECT 
		* 
      FROM 
		CUR_DOC 
	  WHERE 
		DOCUMENT_ID = IFNULL(IN_DOCUMENT_ID, DOCUMENT_ID);	
END
// DELIMITER ;

-- CALL GET_DOCUMENTS(NULL, @OUT_MSG);
-- SELECT @OUT_MSG

-- GET_FLOWS TABLES DOC_TYPE, DOC_FLOW
DROP PROCEDURE IF EXISTS GET_FLOWS;
DELIMITER //
CREATE PROCEDURE GET_FLOWS(
    IN IN_DOC_FLOW_ID INT,
    
    OUT OUT_MSG VARCHAR(500)
)
BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_MSG := CONCAT('ERROR -> ON [GET_FLOWS] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	
    SET OUT_MSG = 'OK';
    
    SELECT 
		DT.TYPE_ID,
        DT.TYPE_CODE,
        DF.DOC_FLOW_ID,
        DF.TYPE_ID,
        DF.KEY1,
        DF.KEY2,
        DF.KEY3,
        DF.KEY4
	FROM 
		DOC_TYPE DT JOIN DOC_FLOW DF ON DT.TYPE_ID = DF.TYPE_ID
		WHERE DF.DOC_FLOW_ID = IFNULL(IN_DOC_FLOW_ID, DF.DOC_FLOW_ID);
END
// DELIMITER ;

-- CALL GET_FLOWS(NULL, @OUT_MSG);
-- GET_FLOWS_APPROVERS TABLES DOC_FLOW, DOC_APPROVER

DROP PROCEDURE IF EXISTS GET_FLOWS_APPROVERS;
DELIMITER //
CREATE PROCEDURE GET_FLOWS_APPROVERS(
	IN IN_DOC_FLOW_ID INT,
    
    OUT OUT_MSG VARCHAR(500)
)
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
    BEGIN 
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;
        SET OUT_MSG := CONCAT ('ERROR ON -> [GET_FLOWS_APPROVERS]', @SQL_STATUS, '-', @ERR_MSG);
	END;
    
    SET OUT_MSG = 'OK';
    
    SELECT 
		DF.DOC_FLOW_ID,
        DF.TYPE_ID,
        DF.KEY1,
        DF.KEY2,
        DF.KEY3,
        DF.KEY4,
        DA.DOC_APPROVER_ID,
        DA.DOC_FLOW_ID,
        DA.APPROVER_ID,
        DA.SEQUENCE,
        DA.NAME,
        DA.ACTION
	FROM 
		DOC_FLOW DF JOIN DOC_APPROVER DA ON DF.DOC_FLOW_ID = DA.DOC_FLOW_ID 
    WHERE DF.DOC_FLOW_ID = IFNULL(IN_DOC_FLOW_ID, DF.DOC_FLOW_ID);
END
// DELIMITER ;

-- CALL GET_FLOWS_APPROVERS(NULL, @OUT_MSG);

DROP PROCEDURE IF EXISTS GET_APPROVERS;
DELIMITER //
CREATE PROCEDURE GET_APPROVERS(
	IN IN_APPROVER_ID INT,
    
    OUT OUT_MSG VARCHAR(500)
)
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
    BEGIN 
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;
        SET OUT_MSG := CONCAT('ERROR ON -> [GET_APPROVERS]', @SQL_STATUS, '-', @ERR_MSG);
	END;
    
    SET OUT_MSG = 'OK';
    
    SELECT
		A.APPROVER_ID,
        A.FULL_NAME,
        A.POSITION_ID,
        A.DEPTO_ID
    FROM 
		APPROVER A
    WHERE 
		A.APPROVER_ID = IFNULL(IN_APPROVER_ID, A.APPROVER_ID);
END
// DELIMITER ;

-- CALL GET_APPROVERS(NULL, @OUT_MSG);


DROP PROCEDURE IF EXISTS GET_DOC_TXN;
DELIMITER //
CREATE PROCEDURE GET_DOC_TXN(
	IN IN_DOCUMENT_ID INT,
    IN IN_APPROVER_ID INT,
    OUT OUT_MSG VARCHAR(450)
) BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
    BEGIN 
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;
        SET OUT_MSG := CONCAT('ERROR ON -> [GET_DOC_TXN]', @SQL_STATUS, '-', @ERR_MSG);
	END;
    
    SET OUT_MSG = 'OK';
    
    SELECT 
		DT.DOCUMENT_ID,
		DT.STATUS_TXN,
		DT.COMMENTS,
        DT.DOC_APPROVER_ID,
		A.APPROVER_ID,
		A.FULL_NAME,
		DA.NAME,
		DA.SEQUENCE,
		DF.KEY1 DEPTO,
		T.TYPE_ID,
		T.TYPE_CODE   
	FROM 
		DOC_TXN DT JOIN APPROVER A ON DT.APPROVER_ID = A.APPROVER_ID  
		JOIN DOC_APPROVER DA ON DT.DOC_APPROVER_ID = DA.DOC_APPROVER_ID
		JOIN DOC_FLOW DF ON DA.DOC_FLOW_ID = DF.DOC_FLOW_ID
		JOIN DOC_TYPE T ON DF.TYPE_ID = T.TYPE_ID
	WHERE 	
		DT.DOCUMENT_ID = IFNULL(IN_DOCUMENT_ID, DT.DOCUMENT_ID)
	AND DT.APPROVER_ID = IFNULL(IN_APPROVER_ID, DT.APPROVER_ID)
	ORDER BY SEQUENCE;	
    
END //
DELIMITER ;

-- CALL GET_DOC_TXN(NULL, NULL, @RESULT);
-- SELECT @RESULT;