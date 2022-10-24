DROP DATABASE IF EXISTS CTL_HINT;
CREATE DATABASE CTL_HINT;
USE CTL_HINT;

/* ############################# CONFIGURATION ############################### */

DROP TABLE IF EXISTS CON_API;
CREATE TABLE CON_API (
	API_ID INT PRIMARY KEY AUTO_INCREMENT,
    URL_BASE VARCHAR(450) NOT NULL,    
    DESCRIPTION VARCHAR (300)
);

DROP TABLE IF EXISTS CON_URL_ENDPOINT;
CREATE TABLE CON_URL_ENDPOINT (
	ENDPOINT_ID INT PRIMARY KEY AUTO_INCREMENT,
    ENDPOINT VARCHAR(450),
    REQUEST_TYPE VARCHAR(30),    
    API_ID INT NOT NULL
);

ALTER TABLE CON_URL_ENDPOINT ADD CONSTRAINT FK_ENDPOINT_API FOREIGN KEY (API_ID) REFERENCES CON_API (API_ID);

DROP TABLE IF EXISTS CON_ENDPOINT_PARAMETER;
CREATE TABLE CON_ENDPOINT_PARAMETER (
	PARAMETER_ID INT PRIMARY KEY AUTO_INCREMENT,
    PARAMETER VARCHAR (450),
    TYPE VARCHAR(30) DEFAULT 'TEXT',
    IS_REQUIRED BOOL DEFAULT FALSE,
    DESCRIPTION VARCHAR(300),    
    ENDPOINT_ID INT NOT NULL
);

ALTER TABLE CON_ENDPOINT_PARAMETER ADD CONSTRAINT FK_PARAMETER_ENDPOINT FOREIGN KEY (ENDPOINT_ID) REFERENCES CON_URL_ENDPOINT (ENDPOINT_ID);

/* ############################# FLOW SERVICE ############################### */

DROP TABLE IF EXISTS FLOW_SERVICE;
CREATE TABLE FLOW_SERVICE (
	FLOW_ID INT PRIMARY KEY AUTO_INCREMENT,
    FLOW_NAME VARCHAR(100) NOT NULL
);

ALTER TABLE FLOW_SERVICE ADD CONSTRAINT UQ_FLOW_NAME UNIQUE (FLOW_NAME);

DROP TABLE IF EXISTS FLOW_DETAIL;
CREATE TABLE FLOW_DETAIL (
	FLOW_DET_ID INT PRIMARY KEY AUTO_INCREMENT,
    SEQUENCE INT NOT NULL,
    IS_REQUIRED BOOL DEFAULT TRUE,
    ENDPOINT_ID INT NOT NULL,
    FLOW_ID INT NOT NULL,
    DESCRIPTION VARCHAR(450)
);

ALTER TABLE FLOW_DETAIL ADD CONSTRAINT FK_FLOW_ENDPOINT FOREIGN KEY (ENDPOINT_ID) REFERENCES CON_URL_ENDPOINT (ENDPOINT_ID),
						ADD CONSTRAINT FK_DETAIL_FLOW FOREIGN KEY (FLOW_ID) REFERENCES FLOW_SERVICE (FLOW_ID),
                        ADD CONSTRAINT UQ_FLOW_ENDPOINT UNIQUE (ENDPOINT_ID, FLOW_ID);                        
                        
/* ############################# TRANSACTION ############################### */

DROP TABLE IF EXISTS TXN_STATUS;
CREATE TABLE TXN_STATUS (
	TXN_STS_ID INT PRIMARY KEY AUTO_INCREMENT,
    TXN_STS_GROUP VARCHAR(450) DEFAULT 'HEADER',
    STATUS_VALUE VARCHAR(30) NOT NULL
);
ALTER TABLE TXN_STATUS ADD CONSTRAINT UQ_STATUS UNIQUE (STATUS_VALUE);

DROP TABLE IF EXISTS TXN_HEADER;
CREATE TABLE TXN_HEADER (
	TXN_ID INT PRIMARY KEY AUTO_INCREMENT,
    FLOW_ID INT NOT NULL,
    FLOW_NAME VARCHAR(100) NOT NULL,
    TXN_STS_ID INT NOT NULL,
    TXN_STS VARCHAR(30) NOT NULL,
    LAST_UPDATE DATETIME NOT NULL DEFAULT NOW()
);

ALTER TABLE TXN_HEADER ADD CONSTRAINT FK_TXN_FLOW FOREIGN KEY (FLOW_ID) REFERENCES FLOW_SERVICE (FLOW_ID),
					   ADD CONSTRAINT FK_TXN_FLOW_NAME FOREIGN KEY (FLOW_NAME) REFERENCES FLOW_SERVICE (FLOW_NAME),
                       ADD CONSTRAINT FK_TXN_STATUS FOREIGN KEY (TXN_STS_ID) REFERENCES TXN_STATUS (TXN_STS_ID),
                       ADD CONSTRAINT FK_TXN_STATUS_VAL FOREIGN KEY (TXN_STS) REFERENCES TXN_STATUS (STATUS_VALUE);

DROP TABLE IF EXISTS TXN_DETAIL;
CREATE TABLE TXN_DETAIL (
	TXN_DET_ID INT PRIMARY KEY AUTO_INCREMENT,
    ENDPOINT_ID INT NOT NULL,
    TXN_STS_ID INT NOT NULL,
    TXN_STS VARCHAR(30) NOT NULL,
    TXN_ID INT NOT NULL,
    LAST_UPDATE DATETIME NOT NULL DEFAULT NOW()
);

ALTER TABLE TXN_DETAIL ADD CONSTRAINT FK_TXN_DET_ENDPOINT FOREIGN KEY (ENDPOINT_ID) REFERENCES CON_URL_ENDPOINT (ENDPOINT_ID),
					   ADD CONSTRAINT FK_TXN_DET_HEADER FOREIGN KEY (TXN_ID) REFERENCES TXN_HEADER (TXN_ID),
                       ADD CONSTRAINT FK_TXN_DET_STATUS FOREIGN KEY (TXN_STS_ID) REFERENCES TXN_STATUS (TXN_STS_ID),
                       ADD CONSTRAINT FK_TXN_DET_STATUS_VAL FOREIGN KEY (TXN_STS) REFERENCES TXN_STATUS (STATUS_VALUE);

/* ############################# LOG ############################### */

DROP TABLE IF EXISTS LOG_TABLE;
CREATE TABLE LOG_TABLE (
	LOG_ID INT PRIMARY KEY AUTO_INCREMENT,
    LOG_GROUP VARCHAR(450) NOT NULL,
    VALUE1 VARCHAR(450),
    VALUE2 VARCHAR(450),
    VALUE3 VARCHAR(450),
    SEQUENCE INT NOT NULL DEFAULT 1,
    MESSAGE VARCHAR(450)
);


/*######################### ADD AUDIT COLUMNS TO ALL TABLES! ##############################*/
DROP PROCEDURE IF EXISTS ADD_AUDIT_COLUMNS;
DELIMITER // 
CREATE PROCEDURE ADD_AUDIT_COLUMNS(
	OUT OUT_MSG VARCHAR(500)
)
BEGIN	
    DECLARE TABLE_N VARCHAR(300) DEFAULT '';
	DECLARE TABLES_C CURSOR FOR SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_SCHEMA = 'CTL_HINT';
	DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @errno = MYSQL_ERRNO, @errmsg = MESSAGE_TEXT;
		SET OUT_MSG = CONCAT( 'ERROR EXECUTING PROCESS [DYNAMIC_SQL_STMT] - (', @errno ,'): ', @errmsg);
	END;
    
	SET OUT_MSG = 'OK';
    
    OPEN TABLES_C;    
    TABLE_LOOP: LOOP
    
		FETCH TABLES_C INTO TABLE_N;        
        
        SET @s = CONCAT(
			'ALTER TABLE ', 
				TABLE_N ,
			' ADD STATUS VARCHAR(40) NOT NULL DEFAULT \'ENABLED\' CHECK (STATUS IN (\'ENABLED\', \'DISABLED\')),
              ADD CREATED_ON DATETIME NOT NULL DEFAULT NOW(),
              ADD CREATED_BY VARCHAR(50),
              ADD UPDATED_ON DATETIME,
              ADD UPDATED_BY VARCHAR(50)'
		);			
        
		PREPARE stmt FROM @s;
		EXECUTE stmt;
		DEALLOCATE PREPARE stmt;
        
	END LOOP;    
    CLOSE TABLES_C;
	
END
//
DELIMITER ;

set @msg = null;
call ADD_AUDIT_COLUMNS(@msg);
SELECT @msg;