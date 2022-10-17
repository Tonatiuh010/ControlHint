SELECT 
	CA.API_ID, 
    CA.DESCRIPTION API_DESCRIPTION, 
    CA.URL_BASE, 
    CUE.ENDPOINT_ID, 
    CUE.ENDPOINT, 
    CUE.REQUEST_TYPE
FROM 
	CON_API CA JOIN CON_URL_ENDPOINT CUE ON CA.API_ID = CUE.API_ID;
    
    
SELECT 
	CUE.API_ID, 
    CUE.ENDPOINT_ID, 
    CUE.ENDPOINT, 
    CUE.REQUEST_TYPE,
    CEP.PARAMETER_ID,
    CEP.PARAMETER,
    CEP.TYPE,
    CEP.IS_REQUIRED,
    CEP.DESCRIPTION
FROM
	CON_URL_ENDPOINT CUE JOIN CON_ENDPOINT_PARAMETER CEP ON CUE.ENDPOINT_ID = CEP.ENDPOINT_ID;
    
    
SELECT 
	FS.FLOW_ID,
    FS.FLOW_NAME,
    FD.FLOW_DET_ID,
    FD.SEQUENCE,
    FD.IS_REQUIRED,
    FD.ENDPOINT_ID,
    FD.DESCRIPTION
FROM
	FLOW_SERVICE FS JOIN FLOW_DETAIL FD ON FS.FLOW_ID = FD.FLOW_ID
ORDER BY FD.SEQUENCE DESC;
