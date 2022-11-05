/* ########################## FLOW_SERVICE ########################## */

INSERT INTO FLOW_SERVICE (FLOW_NAME) 
	VALUES 
('REGISTER_HINT'), 
('ACCESS'), 
('SIGN_APPROVED');

/* ########################## TXN_STATUS ########################## */

INSERT INTO TXN_STATUS (TXN_STS_GROUP, STATUS_VALUE) 
	VALUES 
('HEADER', 'STARTED'),
('HEADER', 'IN_PROGRESS'),
('HEADER', 'COMPLETED'),
('HEADER', 'MANUAL'),
('HEADER', 'CANCELLED'),
('HEADER', 'ERROR')
-- , ('DETAIL', 'STARTED'),
-- ('DETAIL', 'IN_PROGRESS'),
-- ('DETAIL', 'COMPLETED'),
-- ('DETAIL', 'MANUAL'),
-- ('DETAIL', 'CANCELLED'),
-- ('DETAIL', 'ERROR')
;

/* devices */
INSERT INTO DEV_CONNECTION (DEVICE_ID, DEVICE_NAME, DEVICE_MODEL, IP_ADDRESS, IS_ACTIVE) VALUES 
(  
	1,
    'ESP-FG-TNT',
    'ESP-32-DEV-KIT',
    INET_ATON("127.0.0.1"),
    true
); 