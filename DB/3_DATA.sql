/* ########################## FLOW_SERVICE ########################## */

INSERT INTO FLOW_SERVICE (FLOW_NAME) VALUES ('REGISTER_EMPLOYEE'), ('ACCESS'), ('SIGN_APPROVED');

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