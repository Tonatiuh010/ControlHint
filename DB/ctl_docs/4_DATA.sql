USE CTL_DOCS;
/* DOC_TYPE */

INSERT INTO DOC_TYPE (
	TYPE_ID,
    TYPE_CODE
) VALUES 
	(1, 'QUO' ),
    (2, 'SALE');
    
    
/* DOC_FLOW */

INSERT INTO DOC_FLOW (
	DOC_FLOW_ID,
    TYPE_ID,
    KEY1,    
    CREATED_BY
) VALUES (
	1,
    1,
    'RH',
    'TEST_API'
),
(
	2,
    2,
    'RH',
    'TEST_API'
);

/* APPROVERS */

INSERT INTO APPROVER (
	APPROVER_ID,
    FULL_NAME,
    POSITION_ID,
    DEPTO_ID,
    CREATED_BY
) VALUES (
	1001,
    'TONATIUH LOPEZ',
    1, 
    1,
    'TEST_API'
), (
	1002,
    'NAYELI LEAL',
    1, 
    1,
    'TEST_API'
), (
	1003,
    'MARCOS AVILA',
    1, 
    1,
    'TEST_API'
), 
(
	1004,
    'JESUS HERNANDEZ',
    1, 
    1,
    'TEST_API'
);
    
/* DOC_APPROVER  */

INSERT INTO DOC_APPROVER (
	DOC_APPROVER_ID,
    DOC_FLOW_ID,
    APPROVER_ID,
    SEQUENCE,
    NAME,
    CREATED_BY
) VALUES (
	1,
    1,
    1001,
    1,
    'Manager 1',
    'API_TEST'
), (
	2,
    1,
    1002,
    2,
    'Manager 2',
    'API_TEST'
), (
	3,
    1,
    1004,
    2,
    'Manager 3',
    'API_TEST'
);

INSERT INTO DOC_APPROVER (
	DOC_APPROVER_ID,
    DOC_FLOW_ID,
    APPROVER_ID,
    SEQUENCE,
    NAME,
    CREATED_BY
) VALUES (
	4,
    2,
    1001,
    1,
    'Manager 1',
    'API_TEST'
), (
	5,
    2,
    1002,
    2,
    'Manager 2',
    'API_TEST'
), (
	6,
    2,
    1003,
    3,
    'Manager 3',
    'API_TEST'
);
